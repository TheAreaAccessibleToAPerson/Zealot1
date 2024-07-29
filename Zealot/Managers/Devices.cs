using System.Net;
using System.Text.Json;
using Butterfly;
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
        /// Сдесь хранятся ip адресса всех извлечоных из сети машинок.
        /// Данное значение нужно для того что бы при сканировании сети лишний раз не обращатся 
        /// по адресу уже подключонной машинки.
        /// </summary> <summary>
        private readonly List<string> _ipAddresseDevices = new();

        private int index = 0;

        /// <summary>
        /// Метод вызывается в потоке Header.Events.WORK_DEVICE
        /// </summary>
        public byte[] GetDevicesInforamtionMessage()
        {
            List<AsicStatus> buffer = new List<AsicStatus>();
            {
                int i = 0;
                foreach (IDevice device in _scanDevices.Values)
                {
                    buffer.Add(device.GetStatus());
                }
            }
            return ServerMessage.GetMessageArray(ServerMessage.TCPType.ADD_ALL_ASIC,
                JsonSerializer.SerializeToUtf8Bytes(buffer));
        }

        void Construction()
        {
            listen_echo_1_1<string, string[]>(BUS.GET_ADDRESSES_CONNECTION_DEVICES)
                .output_to((name, @return) =>
                {
                    Logger.I.To(this, $"{name} запросил адресса всем машинок которые уже подключены к серверу.");

                    @return.To(_ipAddresseDevices.ToArray());
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

        public struct BUS
        {
            public const string RECEIVE_SCAN_DEVICES = NAME + ":ReceiveScanDevices";
            public const string ADD_ASIC = NAME + ":AddAsic";
            public const string REMOVE_ASIC = NAME + ":AddAsic";

            public const string ADD_EMPTY = NAME + ":AddEmtpy";
            public const string REMOVE_EMPTY = NAME + ":AddEmtpy";


            public const string GET_ADDRESSES_CONNECTION_DEVICES = NAME + ":GetAddressesConnectionDevices";
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
