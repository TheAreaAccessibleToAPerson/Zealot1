﻿using System.Text.Json;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace Butterfly
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            //Result.Initialize();

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

                    new EventSetting(Zealot.Header.Events.SCAN_DEVICES,
                        Zealot.Header.Events.SCAN_DEVICES_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.REQUEST_DEVICES_INFORMATION_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION_TIME_DELAY)
                }
            });
        }
    }

    public static class Result
    {
        private static Dictionary<string, string> dic = new();

        public static void Initialize()
        {
            using (StreamReader reader = new StreamReader("mac.txt", false))
            {
                string[] macs = reader.ReadToEnd().Split("\n");
                Console.WriteLine($"Получено {macs.Length} маков.");
                count = macs.Length;

                foreach (string mac in macs)
                {
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
            List<WhatsMinerInterface> i = JsonSerializer.Deserialize<List<WhatsMinerInterface>>(mac);

            if (dic.ContainsKey(i[0].macaddr))
            {
                lock (locker)
                {
                    Console.WriteLine($"{current_count})MAC:{i[0].macaddr} ip:{address}");
                    current_count++;
                    dic[i[0].macaddr] = address;

                    if (current_count == 25)
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

                        /*
                        string m = "";
                        string ip = "";
                        foreach (var value in dic)
                        {
                            m += value.Key + "\n";
                            ip += value.Value + "\n";
                        }

                        string r = m + "\n" + ip;

                        using (StreamWriter writer = new StreamWriter("ip.txt", false))
                        {
                            writer.WriteLine(r);
                        }
                        */
                    }
                }
            }
        }
    }

    public class WhatsMinerInterface
    {
        public int rx_bytes { get; set; }
        public string ifname { get; set; }
        public int tx_bytes { get; set; }
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
}