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

        void Construction()
        {
            listen_message<string, string>(BUS.RECEIVE_SCAN_DEVICES)
                .output_to((address, html) => 
                {
                    Setting setting = new Setting()
                    {
                        IPAddress = address
                    };

                    switch(DeviceDetection.Process(html))
                    {
                        case WhatsMiner.NAME:

                            _scanDevices.Add(address, obj<WhatsMiner>(address, setting));

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
        public static string Process(string value)
        {
            if (value.Contains("\"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">"))
            {
                return WhatsMiner.NAME;
            }
            else 
                System.Console.WriteLine("KJKJK");

            return "";
        }
    }
}
