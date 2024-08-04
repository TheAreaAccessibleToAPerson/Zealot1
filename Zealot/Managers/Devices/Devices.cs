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

        void Construction()
        {
            listen_echo_1_1<string, List<AsicInit>>(BUS.GET_CLIENT_ASICS)
                .output_to((clientID, @return) =>
                {
                    List<AsicInit> result = new();
                    {
                        foreach (AsicInit asic in _allAsics.Values)
                            if (clientID == asic.Client.ID)
                            {
                                result.Add(asic);
                            }
                    }
                    @return.To(result);
                },
                Header.Events.WORK_DEVICE);

            listen_echo_1_1<string, string[]>(BUS.GET_ADDRESSES_CONNECTION_DEVICES)
                .output_to((name, @return) =>
                {
                    Logger.I.To(this, $"{name} запросил адресса всем машинок которые уже подключены к серверу.");

                    @return.To(_ipAddressDevices.ToArray());
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

            listen_message<IDevice>(BUS.ADD_ASIC)
                .output_to((device) =>
                {
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

                            string mac = device.GetMAC();

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
                                }
                            }
                            else if (mac == "")
                            {
                                Logger.W.To(this, $"Вам поступило устройсва без мака(IPAddress{device.GetAddress()})");

                                device.Destroy($"Вам поступило устройсва без мака(IPAddress{device.GetAddress()})");
                            }
                        }
                    }
                },
                Header.Events.WORK_DEVICE);

            listen_message<string, string>(BUS.RECEIVE_SCAN_DEVICES)
                .output_to((address, html) =>
                {

                    Setting setting = new Setting()
                    {
                        IPAddress = address
                    };

                    switch (DeviceDetection.Process(html, address))
                    {
                        case "ICE":

                            //Console(address);

                            break;

                        case WhatsMiner.NAME:

                            //_scanDevices.Add(address, obj<WhatsMiner>(address, setting));

                            break;
                        case "Antminer":

                            _scanDevices.Add(address, obj<AntminerDefault>(address, setting));

                            break;
                        default:

                            //Console(html);

                            //Console(address + html);

                            break;
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

                        /**********************************************/

                        // Пытаемся получить все машинки.
                        if (MongoDB.TryFind(DB.NAME, DB.AsicsCollections.NAME, out string findInfo,
                            out List<BsonDocument> asics))
                        {
                            try
                            {
                                foreach (BsonDocument asic in asics)
                                {
                                    AsicInit a = JsonSerializer.Deserialize<AsicInit>
                                        (asic[DB.AsicsCollections.Asic.NAME].ToString());

                                    string uniqueNumber = a.UniqueNumber;

                                    if (uniqueNumber != "")
                                    {
                                        if (_allAsics.ContainsKey(uniqueNumber))
                                        {
                                            Logger.S_E.To(this, $"Вы попытались дважды записать асики полученые из базы данных " +
                                                $"в колекцию по уникальному номеру {uniqueNumber}");

                                            destroy();

                                            return;
                                        }
                                        else
                                        {
                                            Logger.I.To(this, $"Полученый из бд асик с уникальным номером:[{uniqueNumber}] " +
                                                " записан в коллекцию.");

                                            _allAsics.Add(uniqueNumber, a);
                                        }
                                    }
                                    else
                                    {
                                        Logger.S_E.To(this, $"В базе данных записан асик с пустым уникальным номером.");

                                        destroy();

                                        return;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.W.To(this, ex.ToString());

                                destroy();
                            }
                        }

                        /**********************************************/
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
        }

        public struct BUS
        {
            public const string RECEIVE_SCAN_DEVICES = NAME + ":ReceiveScanDevices";
            public const string ADD_ASIC = NAME + ":AddAsic";
            public const string REMOVE_ASIC = NAME + ":AddAsic";

            /// <summary>
            /// Получить машинки клиeнта.
            /// </summary> <summary>
            public const string GET_CLIENT_ASICS = NAME + ":GetClientAsics";

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
                return "Antminer19k";
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
