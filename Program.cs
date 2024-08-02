using System.Text.Json;
using MongoDB.Bson;
using Zealot;
using static Zealot.manager.MainClient;

namespace Butterfly
{
    public sealed class Program
    {
        public const string ADDRESS = "127.0.0.1";
        public const int PORT = 5555;

        public static void Main(string[] args)
        {
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

                    new EventSetting(Zealot.Header.Events.LISTEN_CLIENT,
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

    /// <summary>
    /// Скрипт для добовления клинтов.
    /// </summary> <summary>
    public static class AddClient
    {
        public static void StartScript()
        {
            AddToDB(true, "root", "root", "00000000", "root", "root@main.ru", "root");
        }

        private static void AddToDB(bool isActivated, string login, string password, string id, 
            string name, string email, string accessRights)
        {
            if (Zealot.MongoDB.ContainsDatabase(DB.NAME, out string containsDBerror))
            {
                System.Console.WriteLine($"База данныx {DB.NAME} уже создана.");
            }
            else
            {
                if (containsDBerror != "")
                {
                    System.Console.WriteLine($"База данныx {DB.NAME} уже создана.");
                }
                else
                {
                    System.Console.WriteLine($"Создаем базу данных {DB.NAME}.");

                    if (Zealot.MongoDB.TryCreatingDatabase(DB.NAME, out string infoR))
                    {
                        System.Console.WriteLine(infoR);
                    }
                    else
                    {
                        System.Console.WriteLine(infoR);

                        return;
                    }
                }
            }

            // Проверяем наличие коллекции.
            if (Zealot.MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.ClientsCollection.NAME,
                out string error))
            {
                System.Console.WriteLine($"Коллекция [{DB.ClientsCollection.NAME}] в базе данных " +
                    $" [{DB.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (Zealot.MongoDB.TryCreatingCollection(DB.NAME, DB.ClientsCollection.NAME,
                        out string infoI))
                    {
                        System.Console.WriteLine(infoI);
                    }
                    else
                    {
                        System.Console.WriteLine(infoI);

                        return;
                    }
                }
                else
                {
                    System.Console.WriteLine(error);

                    return;
                }
            }

            // Проверяем наличие документа хранящего адреса и диопазоны адресов.
            if (Zealot.MongoDB.TryFind(DB.NAME, DB.ClientsCollection.NAME, out string findInfo,
                out List<BsonDocument> clients))
            {
                System.Console.WriteLine(findInfo);

                if (clients != null)
                {
                    try
                    {
                        foreach (BsonDocument doc in clients)
                        {
                            if (doc[DB.ClientsCollection.Client.LOGIN] == login)
                            {
                                System.Console.WriteLine($"Клиент с логином {login} уже добавлен.");

                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());

                        return;
                    }
                }
                else
                {
                    System.Console.WriteLine("Clinets listen is null.");

                    return;
                }
            }


            // Затем данное значение передадим в базу данных.
            if (Zealot.MongoDB.TryInsertOne(DB.NAME, DB.ClientsCollection.NAME,
            out string info, new BsonDocument()
            {
                { DB.ClientsCollection.Client.LOGIN, login},
                { DB.ClientsCollection.Client.PASSWORD, password},
                { DB.ClientsCollection.Client.IS_ACITVATED, isActivated},
                { DB.ClientsCollection.Client.NAME, name},
                { DB.ClientsCollection.Client.EMAIL, email},
                { DB.ClientsCollection.Client.ID, id},
                { DB.ClientsCollection.Client.ACCESS_RIGHTS.STR, accessRights},
            }))
            {
                System.Console.WriteLine(info);
            }
            else
            {
                System.Console.WriteLine(info);

                return;
            }
        }
    }

    public static class Result1
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
            if (dic.ContainsKey(mac))
            {
                lock (locker)
                {
                    Console.WriteLine($"{current_count})MAC:{mac} ip:{address}");
                    current_count++;
                    dic[mac] = address;

                    if (current_count == 12)
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

    public class PublicPrivateKeypairModel
    {
        public string Public { set; get; }
        public string Private { set; get; }
    }
}