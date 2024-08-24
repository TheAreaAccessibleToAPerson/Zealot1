using System.Text.Json;
using Butterfly;
using MongoDB.Bson;
using Zealot.device;

namespace Zealot.manager
{
    public sealed class Devices : Controller
    {
        public const string NAME = "DevicesManager";

        /// <summary>
        /// Сдесь находятся устройсва отсканированые в нутри сети.
        /// Хранится по мак адресу.
        /// </summary> <summary>
        /// <returns></returns>
        private readonly Dictionary<string, IDevice> _scanDevices = new();

        /// <summary>
        /// Сдесь находится информация о всех асиках(В работе, на стенде, на складе, вернувшиеся клиентам)
        /// Хранится по уникальному значению выдоному в нутри компании
        /// </summary> <summary>
        /// <returns></returns>
        private readonly Dictionary<string, AsicInit> _allAsics = new();

        /// <summary>
        /// Сдесь хранятся ip адресса всех извлечоных из сети машинок.
        /// Данное значение нужно для того что бы при сканировании сети лишний раз не обращатся 
        /// по адресу уже подключонной машинки.
        /// </summary> <summary>
        private readonly List<string> _ipAddressDevices = new();

        /// <summary>
        /// Сюда записываются разблокированые ip аддреса, которые запросит DevicesScaner
        /// </summary> 
        private readonly List<string> _unlockedIpAddressDevices = new();

        void Construction()
        {
            listen_echo_1_1<string, AsicInit>(BUS.Asic.GET_ASIC_INIT)
                .output_to((mac, @return) =>
                {
                    Console($"MAC:[{mac}]");
                    foreach (AsicInit asicInit in _allAsics.Values)
                    {
                        AsicInit.MACInformation macInfo = asicInit.MAC;

                        Console($"mac:[{macInfo.MAC3}]");

                        if (macInfo.MAC3 == mac)
                        {
                            @return.To(asicInit);

                            return;
                        }
                    }

                    @return.To(null);
                },
                Header.Events.WORK_DEVICE);

            listen_echo<List<IDevice>>(BUS.Client.ADMIN_GET_NOT_DB_ASICS)
                .output_to((@return) =>
                {
                    // 
                },
                Header.Events.WORK_DEVICE);

            listen_echo_1_1<IClientConnect, List<string[]>>(BUS.Client.SUBSCRIBE_TO_MESSAGE)
                .output_to((client, @return) =>
                {
                    string clientID = client.GetClientID();

                    List<string[]> result = new();
                    if (client.IsAdmin())
                    {
                        foreach (AsicInit asic in _allAsics.Values)
                        {
                            result.Add(new string[] { asic.MAC.MAC3, asic.SN.SN3 });
                            asic.ClientSubscribeToReceiveMessage(client);
                        }
                    }
                    else
                    {
                        foreach (AsicInit asic in _allAsics.Values)
                            if (clientID == asic.Client.ID)
                            {
                                result.Add(new string[] { asic.MAC.MAC3, asic.SN.SN3 });
                                asic.ClientSubscribeToReceiveMessage(client);
                            }
                    }

                    @return.To(result);
                },
                Header.Events.WORK_DEVICE);

            listen_message<IClientConnect>(BUS.Client.UNSUBSCRIBE_TO_MESSAGE)
                .output_to((client) =>
                {
                    string clientID = client.GetClientID();

                    string info = "Devices:\n";
                    int index = 0;
                    foreach (AsicInit asic in _allAsics.Values)
                        if (clientID == asic.Client.ID)
                        {
                            info += $"Unsubscribe -> {++index})SN:{asic.SN.SN3}, MAC:{asic.MAC.MAC3}, Location[Name:{asic.Location.Name}, StandNumber:{asic.Location.StandNumber}, SlotIndex:{asic.Location.SlotIndex}]";
                            asic.ClientUnsubscribeToReceiveMessage(client);
                        }

                    Logger.I.To(this, $"Отписываем клиента {client.GetKey()} от получения сообщений с машинок.\n" + info);
                },
                Header.Events.WORK_DEVICE);

            listen_echo_1_1<IClientConnect, List<AsicInit>>(BUS.Client.GET_CLIENT_ASICS)
                .output_to((client, @return) =>
                {
                    if (client.IsAdmin())
                    {
                        Logger.I.To(this, $"Получение общей информации о машинках находящихся на площадке для клиента с правами Admin:{client.GetKey()}");

                        @return.To(_allAsics.Values.ToList());
                    }
                    else
                    {
                        List<AsicInit> result = new();
                        {
                            string clientID = client.GetClientID();

                            foreach (AsicInit asic in _allAsics.Values)
                                if (clientID == asic.Client.ID)
                                {
                                    result.Add(asic);
                                }

                        }
                        @return.To(result);
                    }
                },
                Header.Events.WORK_DEVICE);

            listen_echo_2_3<string, bool, string[], string[], bool>(BUS.GET_ADDRESSES_CONNECTION_DEVICES)
                .output_to((name, isStart, @return) =>
                {
                    string info =  $"{name} запросил адресса всем машинок которые уже подключены к серверу и адресса которые нужно освободить.";

                    info = "\nАдресса для освобождения:";
                    for (int i = 0; i < _unlockedIpAddressDevices.Count; i++)
                    {
                        info += $"\n{i+1}){_unlockedIpAddressDevices[i]}";
                    }

                    string[] unlockedIpAddressesBuffer = _unlockedIpAddressDevices.ToArray();
                    _unlockedIpAddressDevices.Clear();

                    info += "\nАдресса не доступные для сканирония.";
                    for (int i = 0; i < _ipAddressDevices.Count; i++)
                    {
                        info += $"\n{i+1}){_ipAddressDevices[i]}";
                    }

                    Logger.I.To(this, info);

                    @return.To(_ipAddressDevices.ToArray(), unlockedIpAddressesBuffer, isStart);
                },
                Header.Events.WORK_DEVICE);

            listen_message<string>(BUS.ADD_EMPTY)
                .output_to((address) =>
                {
                    lock (StateInformation.Locker)
                    {
                        if (!StateInformation.IsDestroy && StateInformation.IsStart)
                        {
                            Logger.I.To(this, $"Вы добавлили ip адресс девайса который не смог считать данные.");
                        }
                    }
                },
                Header.Events.WORK_DEVICE);

            listen_echo_1_1<IDevice, bool>(BUS.ADD_ASIC)
                .output_to((device, @return) =>
                {
                    string mac = device.GetMAC();
                    string address = device.GetAddress();

                    Logger.I.To(this, $"Поступило новое устройсво по в {BUS.ADD_ASIC}, mac:[{mac}], ip:[{address}]");

                    if (address == "")
                    {
                        Logger.S_E.To(this, $"Вы получили в {BUS.ADD_ASIC} устройсво с пустым ip аддрессом.");

                        destroy();

                        return;
                    }

                    lock (StateInformation.Locker)
                    {
                        if (!StateInformation.IsDestroy && StateInformation.IsStart)
                        {
                            if (device == null)
                            {
                                Logger.S_E.To(this, $"В {BUS.ADD_ASIC} пришло null значение.");

                                destroy();

                                return;
                            }


                            if (mac != "")
                            {
                                if (_scanDevices.TryGetValue(mac, out IDevice i))
                                {
                                    if (device.GetAddress() == i.GetAddress())
                                    {
                                        Logger.W.To(this, $"Попытка дважды добавить устройсво с маком {mac} " +
                                            $" (ip Address:{device.GetAddress()})");
                                        device.Destroy($"Попытка дважды добавить устройсво с маком {mac} " +
                                            $" (ip Address:{device.GetAddress()})");
                                    }
                                    else
                                    {
                                        Logger.W.To(this, $"Попытка добавить устросво с мак адресом {mac}.(IPAddress:{device.GetAddress()})" +
                                            $"Устройсво с таким маком уже находится в сети но по другому адрессу (IPAddress:{i.GetAddress()})");

                                        device.Destroy($"Попытка добавить устросво в {NAME} с мак адресом {mac}.(IPAddress:{device.GetAddress()}" +
                                            $"  через {BUS.ADD_ASIC}." +
                                            $"Устройсво с таким маком уже находится в сети но по другому адрессу (IPAddress:{i.GetAddress()})");
                                    }
                                }
                                else
                                {
                                    Logger.I.To(this, $"Добавлен новый асик:MAC[{mac}], Address[{device.GetAddress()}]");

                                    _scanDevices.Add(mac, device);

                                    @return.To(true);
                                }
                            }
                            else if (mac == "")
                            {
                                Logger.W.To(this, $"Вам поступило устройсва без мака(IPAddress:{device.GetAddress()})");

                                device.Destroy($"Вам поступило устройсва без мака(IPAddress:{device.GetAddress()})");
                            }
                        }

                        @return.To(false);
                    }
                },
                Header.Events.WORK_DEVICE);

            listen_message<IDevice>(BUS.Asic.REMOTE_ASIC)
                .output_to((device) =>
                {
                    string address = device.GetAddress();
                    string mac = device.GetMAC();

                    string info = "";

                    if (_unlockedIpAddressDevices.Contains(address) == false)
                    {
                        _unlockedIpAddressDevices.Add(address);
                    }

                    if (_ipAddressDevices.Contains(address))
                    {
                        info += $"Адресс [{address}] был разлокирован для сканера девайсов.";

                        _ipAddressDevices.Remove(address);
                    }
                    else
                    {
                        if (mac != "")
                        {
                            if (_scanDevices.ContainsKey(mac))
                            {
                                Logger.S_E.To(this, $"Вы попытались разблокировать адресс [{address}] для сканера девайсов, но данный адресс небыл " +
                                    $"заблокирован ранее, машина под данных ip адрессом и мак адрессом [{mac}] была добавлена ранее в список девайсов.");
                            }
                            else Logger.S_E.To(this, $"Попытка удалить машину из списка отсканированых из сети машин, но машины " +
                                    $"записаной по мак адресу [{mac}] нету, так же не удалось разблокировать адресс [{address}] для сканера " +
                                    $"девайсов, так как данный адресс не был заблокирован ранее.");

                            destroy();

                            return;
                        }
                    }

                    if (mac != "")
                    {
                        if (_scanDevices.ContainsKey(mac))
                        {
                            info += $"Асик хронящийся по маку [{mac}] в спике полученых машин их сети был удален.";

                            _scanDevices.Remove(mac);
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Неудалось удалить асик по маку [{mac}] из списка устройсв полученых из сети, так как он небыл записан туда ранее");

                            destroy();

                            return;
                        }
                    }

                    Logger.I.To(this, info);
                },
                Header.Events.WORK_DEVICE);

            listen_message<string, string>(BUS.RECEIVE_SCAN_DEVICES)
                .output_to((address, html) =>
                {
                    Setting setting = new Setting()
                    {
                        IPAddress = address
                    };

                    if (_scanDevices.ContainsKey(address))
                    {
                        Logger.S_E.To(this, $"Вы попытались дважды добавить машинку по ключу {address} в _scanDevices.");

                        destroy();

                        return;
                    }
                    else
                    {
                        foreach (string a in _ipAddressDevices)
                        {
                            if (a == address)
                            {
                                Logger.S_E.To(this, $"Вы пытаетесь добавить в список хронящий ip адресса, повторяющийся адреесс {address}");

                                destroy();

                                return;
                            }
                        }

                        switch (DeviceDetection.Process(html, address))
                        {
                            case "ICE":

                                //Console(address);

                                break;

                            case WhatsMiner.NAME:

                                if (try_obj(address, out WhatsMiner asic))
                                {
                                    Logger.W.To(this, $"Уже добавлен WhatsMiner по аддресу(ключy) {address}.");
                                }
                                else
                                {
                                    obj<WhatsMiner>(address, setting);

                                    Logger.I.To(this, $"Из сети поступил WhatsMiner находящийся по адрессу {address}.\n" +
                                        $"Адресс {address} заблокирован для дальнейшего сканирования.");

                                    _ipAddressDevices.Add(address);
                                }


                                break;
                            case "Antminer":

                                if (try_obj(address, out AntminerDefault asic1))
                                {
                                    Logger.W.To(this, $"Уже добавлен WhatsMiner по аддресу(ключy) {address}.");
                                }
                                else
                                {
                                    obj<AntminerDefault>(address, setting);

                                    Logger.I.To(this, $"Из сети поступил WhatsMiner находящийся по адрессу {address}.\n" +
                                        $"Адресс {address} заблокирован для дальнейшего сканирования.");

                                    _ipAddressDevices.Add(address);
                                }

                                break;
                            default:

                                //Console(html);

                                //Console(address + html);

                                break;
                        }

                    }

                },
                Header.Events.WORK_DEVICE);
        }

        void Configurate()
        {
            if (MongoDB.ContainsDatabase(DB.NAME, out string containsDBerror))
            {
                Logger.S_I.To(this, $"База данныx {DB.NAME} уже создана.");
            }
            else
            {
                if (containsDBerror != "")
                {
                    Logger.S_E.To(this, containsDBerror);

                    destroy();

                    return;
                }
                else
                {
                    Logger.S_I.To(this, $"Создаем базу данных {DB.NAME}.");

                    if (MongoDB.TryCreatingDatabase(DB.NAME, out string info))
                    {
                        Logger.S_I.To(this, info);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();

                        return;
                    }
                }
            }

            // Проверяем наличие коллекции.
            if (MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.AsicsCollections.NAME,
                out string error))
            {
                Logger.S_I.To(this, $"Коллекция [{DB.AsicsCollections.NAME}] в базе данных " +
                    $" [{DB.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (MongoDB.TryCreatingCollection(DB.NAME, DB.AsicsCollections.NAME,
                        out string info))
                    {
                        Logger.S_I.To(this, info);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();

                        return;
                    }
                }
                else
                {
                    Logger.S_E.To(this, error);

                    destroy();

                    return;
                }
            }

            // Пытаемся получить все машинки.
            if (MongoDB.TryFind(DB.NAME, DB.AsicsCollections.NAME, out string findInfo,
                out List<BsonDocument> asics))
            {
                try
                {
                    foreach (BsonDocument asic in asics)
                    {
                        // Асик хранится по ключу "Уникальный номер выдоный в нутри компании".
                        string key = asic[AsicInit._.UNIQUE_NUMBER].ToString();

                        AsicInit a = new AsicInit()
                        {
                            UniqueNumber = asic[AsicInit._.UNIQUE_NUMBER].ToString(),

                            Client = new AsicInit.ClientInformation()
                            {
                                ID = asic[AsicInit._.CLIENT_ID].ToString(),
                            },

                            IsRunning = asic[AsicInit._.IS_RUNNING].ToBoolean(),

                            Model = new AsicInit.ModelInformation()
                            {
                                Name1 = asic[AsicInit._.MODEL_NAME1].ToString(),
                                Name2 = asic[AsicInit._.MODEL_NAME2].ToString(),
                                Name3 = asic[AsicInit._.MODEL_NAME3].ToString(),

                                Power = asic[AsicInit._.MODEL_POWER].ToString(),
                            },

                            SN = new AsicInit.SNInformation()
                            {
                                SN1 = asic[AsicInit._.SN1].ToString(),
                                SN2 = asic[AsicInit._.SN2].ToString(),
                                SN3 = asic[AsicInit._.SN3].ToString(),
                            },

                            MAC = new AsicInit.MACInformation()
                            {
                                MAC1 = asic[AsicInit._.MAC1].ToString(),
                                MAC2 = asic[AsicInit._.MAC2].ToString(),
                                MAC3 = asic[AsicInit._.MAC3].ToString(),
                            },

                            Location = new AsicInit.LocationInformation()
                            {
                                Name = asic[AsicInit._.LOCATION_NAME].ToString(),
                                StandNumber = asic[AsicInit._.LOCATION_STAND_NUMBER].ToString(),
                                SlotIndex = asic[AsicInit._.LOCATION_SLOT_INDEX].ToString(),
                            },

                            Pool = new AsicInit.PoolInformation()
                            {
                                Addr1 = asic[AsicInit._.POOL_ADDR_1].ToString(),
                                Name1 = asic[AsicInit._.POOL_NAME_1].ToString(),
                                Password1 = asic[AsicInit._.POOL_PASSWORD_1].ToString(),

                                Addr2 = asic[AsicInit._.POOL_ADDR_2].ToString(),
                                Name2 = asic[AsicInit._.POOL_NAME_2].ToString(),
                                Password2 = asic[AsicInit._.POOL_PASSWORD_2].ToString(),

                                Addr3 = asic[AsicInit._.POOL_ADDR_3].ToString(),
                                Name3 = asic[AsicInit._.POOL_NAME_3].ToString(),
                                Password3 = asic[AsicInit._.POOL_PASSWORD_3].ToString(),
                            },
                        };

                        if (_allAsics.ContainsKey(key))
                        {
                            Logger.S_E.To(this, $"Вы получили из БД более одного обьектa с полем UnitqueNumber(уникальное " +
                                $" значение выданое в нутри компании) равным {key}. Так же по этому значению асики хранятся в колекции.");

                            destroy();
                        }
                        else
                        {
                            _allAsics.Add(key, a);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console(ex.ToString());

                    destroy();
                }
            }
        }

        public struct BUS
        {
            public const string RECEIVE_SCAN_DEVICES = NAME + ":ReceiveScanDevices";
            public const string ADD_ASIC = NAME + ":AddAsic";
            public const string REMOVE_ASIC = NAME + ":RemoveAsic";

            // Это для клинта.
            public struct Client
            {
                /// <summary>
                /// Получить машинки клиeнта.
                /// </summary> <summary>
                public const string GET_CLIENT_ASICS = NAME + ":GetClientAsics";

                /// <summary>
                /// Подписывается на получение сообщений от машинок.
                /// </summary> <summary>
                public const string SUBSCRIBE_TO_MESSAGE = NAME + ":SubscribeToMessage";

                /// <summary>
                /// Отписывается от получение сообщений от машинок.
                /// </summary> <summary>
                public const string UNSUBSCRIBE_TO_MESSAGE = NAME + ":unsubscribeToMessage";

                /// <summary>
                /// Клиент являющийся админом, через определеные промежуток времени
                /// будет запрашивать машыны которые не сохранены в базе данных.
                /// </summary> <summary>
                public const string ADMIN_GET_NOT_DB_ASICS = NAME + ":AdminGetNotDBAsics";
            }

            public struct Asic
            {
                /// <summary>
                /// Асик пытается получить AsicInit по своему программному маку.
                /// </summary>
                public const string GET_ASIC_INIT = NAME + ":GetAsicInit";

                /// <summary>
                /// Удаляем машину и списка машин полученых их сети,
                /// так же удаляем адресс данной машины из списка заблокированых адрессов для сканирования.
                /// </summary> 
                public const string REMOTE_ASIC = NAME + ":RemoveAsic";
            }


            public const string ADD_EMPTY = NAME + ":AddEmtpy";
            public const string REMOVE_EMPTY = NAME + ":AddEmtpy";

            public const string GET_ADDRESSES_CONNECTION_DEVICES = NAME + ":GetAddressesConnectionDevices";
        }

        public struct DB
        {
            public const string NAME = "Asics";

            public struct AsicsCollections
            {
                public const string NAME = "AsicsCollection";

                public struct Asic
                {
                    public const string NAME = "Asic";
                }
            }
        }

        /// <summary>
        /// Интерфейс который описывает способ общения DevicesManager с клиентом.
        /// </summary> 
        public interface IClientConnect
        {
            public void SendMessage(byte[] message);
            public void SendMessage(string message);

            /// <summary>
            /// Ключ по которому создается обьект в нутри библиотеки.
            /// </summary>
            public string GetKey();

            /// <summary>
            /// Является ли клиент админом?
            /// </summary>
            public bool IsAdmin();

            /// <summary>
            /// 
            /// </summary>
            public string GetClientID();
        }
    }

    public static class DeviceDetection
    {
        public static string Process(string value, string address)
        {
            if (value.Contains("LuCI"))
            {
                return WhatsMiner.NAME;
            }
            else if (value.Contains("<title>PROMMINER FW</title>"))
            {
                //Console.WriteLine("PRO MINER");
            }
            //else if (value.Contains("<link rel=\"shortcut icon\" href=\"/favicon.ico\" />"))
            //{
            //    return "1";
            //return "ICE";
            //}
            // S19K
            else if (value.Contains("ANTMINER"))
            {
                return "Antminer";
            }
            else if (value.Contains("Antminer"))
            {
                return "Antminer";
            }
            else if (value.Contains("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">"))
            {
                return "1";
            }
            else if (value.Contains("Bluestar"))
            {
                return "1";
            }
            else if (value.Contains("<link rel=\"icon\" href=\"./favicon.ico\" />"))
            {
                return "1";
            }
            else if (value.Contains("<!-- Copyright &copy; 2010-2017 Hewlett Packard Enterprise Development LP. -->"))
            {
                return "1";
            }
            else if (value.Contains("<title>Web user login</title>"))
            {
                return "1";
            }
            else
            {
            }
            //else 
            //System.Console.WriteLine("KJKJK");


            return "";
        }
    }
}
