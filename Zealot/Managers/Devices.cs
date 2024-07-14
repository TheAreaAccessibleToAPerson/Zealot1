using Butterfly;
using Zealot.device;

namespace Zealot.manager
{
    public sealed class Devices : Controller
    {
        public const string NAME = "DevicesManager";

        /// <summary>
        /// Сдесь находятся устройсва отсканированые в нутри сети.
        /// </summary> <summary>
        /// <returns></returns>
        private Dictionary<string, IDevice> _scanDevices = new();

        private int index = 0;
        void Construction()
        {
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

                            _scanDevices.Add(address, obj<WhatsMiner>(address, setting));

                            break;

                        default:

                            //Console(address + html);

                            break;
                    }
                },
                Header.Events.WORK_DEVICE);
        }

        public struct BUS
        {
            public const string RECEIVE_SCAN_DEVICES = "ReceiveScanDevices";
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
            else if (value.Contains("<link rel=\"shortcut icon\" href=\"/favicon.ico\" />"))
            {
                return "1";
                //return "ICE";
            }
            else if (value.Contains("ANTMINER"))
            {
                return "1";
            }
            else if (value.Contains("Antminer"))
            {
                return "1";
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
