using MongoDB.Bson;
using static Zealot.manager.MainClient;

namespace Butterfly
{
    public sealed class Program
    {
        public const string ADDRESS = "127.0.0.1";
        public const int PORT = 5555;

        public static void Main(string[] args)
        {
            //Result1.InitializeIP();
            //Result1.Initialize();

            Butterfly.fly<Zealot.Header>(new Butterfly.Settings()
            {
                Name = "Program",

                SystemEvent = new EventSetting(Zealot.Header.Events.SYSTEM,
                    Zealot.Header.Events.SYSTEM_TIME_DELAY),

                EventsSetting = new EventSetting[]
                {
                    new EventSetting(Zealot.Header.Events.LOGGER,
                        Zealot.Header.Events.LOGGER_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.MONGO_DB,
                        Zealot.Header.Events.MONGO_DB_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.WORK_DEVICE,
                        Zealot.Header.Events.WORK_DEVICE_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.CLIENT_WORK,
                        Zealot.Header.Events.LISTEN_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.SEND_MESSAGE_TO_CLIENT,
                        Zealot.Header.Events.SEND_MESSAGE_TO_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.RECEIVE_MESSAGE_FROM_CLIENT,
                        Zealot.Header.Events.RECEIVE_MESSAGE_FROM_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.SCAN_DEVICES,
                        Zealot.Header.Events.SCAN_DEVICES_TIME_DELAY, 20000),

                    new EventSetting(Zealot.Header.Events.REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.REQUEST_DEVICES_INFORMATION_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION_TIME_DELAY)
                }
            });
        }
    }


    public static class Result1
    {
        private static Dictionary<string, string> dic = new();

        static int f = 0;

        public static void Initialize()
        {
            using (StreamReader reader = new StreamReader("mac.txt", false))
            {
                string[] macs = reader.ReadToEnd().Split("\n");
                Console.WriteLine($"Получено {macs.Length} маков.");
                count = macs.Length;

                foreach (string mac in macs)
                {
                    f++;
                    Console.WriteLine($"MAC:[{mac}]");
                    dic.Add(mac, "");
                }
            }

            /*
            using (StreamWriter writer = new StreamWriter("ip.txt", false))
            {
                //writer.WriteLine("KDJFKDJF");
            }
            */
        }

        public static void InitializeIP()
        {
            using (StreamReader reader = new StreamReader("ip.txt", false))
            {
                string[] macs = reader.ReadToEnd().Split("\n");
                Console.WriteLine($"Получено {macs.Length} маков.");
                count = macs.Length;

                foreach (string mac in macs)
                {
                    f++;
                    Console.WriteLine($"MAC:[{mac}]");
                    dic.Add(mac, "");
                }
            }

            /*
            using (StreamWriter writer = new StreamWriter("ip.txt", false))
            {
                //writer.WriteLine("KDJFKDJF");
            }
            */
        }

        private static object locker = new();
        private static int count = 0;
        private static int current_count = 0;

        public static void Reiceve(string address, string mac)
        {
            if (dic.ContainsKey(mac))
            {

                using (StreamWriter writer = new StreamWriter("ip.txt", false))
                {
                    writer.WriteLine(address);
                }

                lock (locker)
                {
                    Console.WriteLine($"{current_count})MAC:{mac} ip:{address}");
                    current_count++;
                    dic[mac] = address;

                    if (current_count == f)
                    {
                        Console.WriteLine("Все маки собраны.");

                        string m = "";
                        foreach (var value in dic)
                        {
                            m += value.Value + "\n";
                        }

                        using (StreamWriter writer = new StreamWriter("ip.txt", false))
                        {
                            writer.WriteLine(m);
                        }
                    }
                }
            }
        }

        public static void ReiceveIS(string status, string address)
        {
            if (dic.ContainsKey(address))
            {
                lock (locker)
                {
                    Console.WriteLine($"{current_count})IP:{address} GHSav:{status}");
                    current_count++;
                    //dic[address] = status;

                    using (StreamWriter writer = new StreamWriter("mac.txt", false))
                    {
                        writer.WriteLine(address + " " + status);
                    }
                }
            }
        }
    }
}

public class WhatsMinerInterface
{
    public ulong rx_bytes { get; set; }
    public string ifname { get; set; }
    public ulong tx_bytes { get; set; }
    public List<string> ipaddrs { get; set; }
    public string gwaddr { get; set; }
    public int tx_packets { get; set; }
    public List<string> dnsaddrs { get; set; }
    public int rx_packets { get; set; }
    public string proto { get; set; }
    public string id { get; set; }
    public List<object> ip6addrs { get; set; }
    public int uptime { get; set; }
    public List<object> subdevices { get; set; }
    public bool is_up { get; set; }
    public string macaddr { get; set; }
    public string type { get; set; }
    public string name { get; set; }
}

public class PublicPrivateKeypairModel
{
    public string Public { set; get; }
    public string Private { set; get; }
}