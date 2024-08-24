using MongoDB.Bson;
using static Zealot.manager.Devices;

namespace Zealot.script
{
    /// <summary>
    /// Скрипт для добовления клинтов.
    /// </summary> <summary>
    public static class AddAsic
    {
        public static void StartScript1()
        {
            //var m = MongoDB.Client.GetDatabase(DB.NAME);
            //m.DropCollection(DB.AsicsCollections.NAME);
        }
        //5 - это id тестового клиeнта.
        public static void StartScript2()
        {
            AddToDB("id000001", "5", true, 
            "", "", "WhatsMiner M50",
            "120T",
            // SN
            "", "", "HTM32X84KU23111512068123733A50975",
            // MAC
            "", "", "CC:08:1F:00:26:8C",
            "ЦЕХ", "6-1", "1", 
            "", "", "",
            "", "", "",
            "", "", ""
            );

            AddToDB("id000002", "5", true, 
            "", "", "WhatsMiner M50",
            "120T",
            // SN
            "", "", "HTM32X84KU23111512068123733A50733",
            // MAC
            "", "", "CC:09:01:00:34:A0",
            "ЦЕХ", "6-1", "2", 
            "", "", "",
            "", "", "",
            "", "", ""
            );

            AddToDB("id000003", "5", true, 
            "", "", "WhatsMiner M50",
            "120T",
            // SN
            "", "", "HTM32X84KU23111412068123733A50652",
            // MAC
            "", "", "CC:08:1F:00:26:5B",
            "ЦЕХ", "6-1", "3", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }
        public static void StartScript3()
        {
            AddToDB("id000005", "5", true, 
            "", "", "Antminer T21",
            "190T",
            // SN
            "", "", "DGAHF2ABDJBAG0CD5",
            // MAC
            "", "", "02:6E:26:9D:B5:49",
            "ЦЕХ", "11-9", "9", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript4()
        {
            AddToDB("id000006", "5", true, 
            "", "", "Antminer S19k Pro",
            "115T",
            // SN
            "", "", "JYZZAEBBCAAJF0BWS",
            // MAC
            "", "", "02:6E:FE:C6:D6:FD",
            "ЦЕХ", "1-1", "1", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript5()
        {
            AddToDB("id000007", "5", true, 
            "", "", "Antminer D9",
            "1770Gh",
            // SN
            "", "", "JYZZEKABCABBG0060",
            // MAC
            "", "", "42:33:39:83:2D:B1",
            "ЦЕХ", "6-10", "8", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript6()
        {
            AddToDB("id000008", "5", true, 
            "", "", "Antminer E9 Pro",
            "3680M",
            // SN
            "", "", "NGSBETRBCJDAA03B7",
            // MAC
            "", "", "78:07:19:A1:27:32",
            "ЦЕХ", "1-8", "15", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void RemoveDataBase()
        {
            Zealot.MongoDB.TryRemoveDatabase(DB.NAME, out string info);
            System.Console.WriteLine(info);
        }


        private static void AddToDB(string uniqueNumber, string clientID, bool isRunning,
            // Программный, москва, по факту(наклейка)
            string modelName1, string modelName2, string modelName3,
            string modelPower,
            // Программный, москва, по факту(наклейка)
            string SN1, string SN2, string SN3, 
            // Программный, москва, по факту(наклейка)
            string MAC1, string MAC2, string MAC3,
            string locationName, string locationStandNumber, string locationSlotIndex,
            string poolAddr1, string poolName1, string poolPassword1,
            string poolAddr2, string poolName2, string poolPassword2,
            string poolAddr3, string poolName3, string poolPassword3)
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
            if (Zealot.MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.AsicsCollections.NAME,
                out string error))
            {
                System.Console.WriteLine($"Коллекция [{DB.AsicsCollections.NAME}] в базе данных " +
                    $" [{DB.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (Zealot.MongoDB.TryCreatingCollection(DB.NAME, DB.AsicsCollections.NAME,
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
            if (Zealot.MongoDB.TryFind(DB.NAME, DB.AsicsCollections.NAME, out string findInfo,
                out List<BsonDocument> clients))
            {
                System.Console.WriteLine(findInfo);

                if (clients != null)
                {
                    try
                    {
                        foreach (BsonDocument doc in clients)
                        {
                            if (uniqueNumber == doc[AsicInit._.UNIQUE_NUMBER.ToString()])
                            {
                                System.Console.WriteLine($"Машинка с уникальным номером {uniqueNumber} " + 
                                    "уже добавленав базу данных.");

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
            if (Zealot.MongoDB.TryInsertOne(DB.NAME, DB.AsicsCollections.NAME,
            out string info, new BsonDocument()
            {
                { AsicInit._.UNIQUE_NUMBER, uniqueNumber},
                { AsicInit._.CLIENT_ID, clientID},
                { AsicInit._.IS_RUNNING, isRunning},
                { AsicInit._.MODEL_NAME1, modelName1}, 
                { AsicInit._.MODEL_NAME2, modelName2}, 
                { AsicInit._.MODEL_NAME3, modelName3},
                { AsicInit._.MODEL_POWER, modelPower},
                { AsicInit._.SN1, SN1}, { AsicInit._.SN2, SN2}, { AsicInit._.SN3, SN3},
                { AsicInit._.MAC1, MAC1}, { AsicInit._.MAC2, MAC2}, { AsicInit._.MAC3, MAC3},
                { AsicInit._.LOCATION_NAME, locationName},
                { AsicInit._.LOCATION_STAND_NUMBER, locationStandNumber},
                { AsicInit._.LOCATION_SLOT_INDEX, locationSlotIndex},
                { AsicInit._.POOL_ADDR_1, poolAddr1},
                { AsicInit._.POOL_NAME_1, poolName1},
                { AsicInit._.POOL_PASSWORD_1, poolPassword1},
                { AsicInit._.POOL_ADDR_2, poolAddr2},
                { AsicInit._.POOL_NAME_2, poolName2},
                { AsicInit._.POOL_PASSWORD_2, poolPassword2},
                { AsicInit._.POOL_ADDR_3, poolAddr3},
                { AsicInit._.POOL_NAME_3, poolName3},
                { AsicInit._.POOL_PASSWORD_3, poolPassword3},
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

}