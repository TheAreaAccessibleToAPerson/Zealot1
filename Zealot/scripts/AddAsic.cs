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

        public static void StartScript7()
        {
            AddToDB("id000009", "5", true, 
            "", "", "Antminer K7",
            "63.5T",
            // SN
            "", "", "JYZZEGABCJBAA0190",
            // MAC
            "", "", "E8:E7:74:CF:3D:1A",
            "ЦЕХ", "2-11", "19", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript8()
        {
            AddToDB("id000010", "5", true, 
            "", "", "Antminer L7",
            "9050M",
            // SN
            "", "", "FXDZDCBBBJBBJ037Y",
            // MAC
            "", "", "60:CD:2E:12:60:AF",
            "ЦЕХ", "1-5", "8", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript9()
        {
            AddToDB("id000011", "5", true, 
            "", "", "Antminer S19",
            "86T",
            // SN
            "", "", "PIEME5BBBJHJI0197",
            // MAC
            "", "", "02:39:C1:BD:AD:D7",
            "ЦЕХ", "4-11", "13", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript10()
        {
            AddToDB("id000012", "5", true, 
            "", "", "Antminer S19 Pro",
            "100T",
            // SN
            "", "", "HKYQEMBBCJAJI03D9",
            // MAC
            "", "", "02:FC:AC:CB:2C:DA",
            "ЦЕХ", "1-6", "12", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript11()
        {
            AddToDB("id000013", "5", true, 
            "", "", "Antminer S19 XP",
            "141T",
            // SN
            "", "", "JYZZEABBCJBBG043B",
            // MAC
            "", "", "02:F4:F4:8B:9E:D2",
            "ЦЕХ", "4-11", "11", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript12()
        {
            AddToDB("id000014", "5", true, 
            "", "", "Antminer S19j Pro",
            "104T",
            // SN
            "", "", "SMTTD4DBBAJAB0002",
            // MAC
            "", "", "B4:10:7B:A1:AF:F7",
            "ЦЕХ", "5-1", "11", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript13()
        {
            AddToDB("id000015", "5", true, 
            "", "", "Antminer S19j Pro+",
            "120T",
            // SN
            "", "", "YNAHADABCJCBG0A2V",
            // MAC
            "", "", "02:6E:A8:F2:F4:D5",
            "ЦЕХ", "8-3", "19", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript14()
        {
            AddToDB("id000016", "5", true, 
            "", "", "Antminer S19k Pro",
            "120T",
            // SN
            "", "", "YNAHAEUBCAABI0J00",
            // MAC
            "", "", "2A:65:59:54:06:EC",
            "ЦЕХ", "3-10", "22", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript15()
        {
            AddToDB("id000017", "5", true, 
            "", "", "Antminer S21",
            "195T",
            // SN
            "", "", "JYZZATUBCAABE01M7",
            // MAC
            "", "", "02:76:8A:E6:F0:C9",
            "ЦЕХ", "5-6", "8", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript16()
        {
            AddToDB("id000018", "5", true, 
            "", "", "Antminer S21 Pro",
            "234T",
            // SN
            "", "", "DGAHFKABDJFJE013A",
            // MAC
            "", "", "02:7E:BC:91:53:C6",
            "ЦЕХ", "8-8", "23", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript17()
        {
            AddToDB("id000019", "5", true, 
            "", "", "Antminer T21 190T",
            "190T",
            // SN
            "", "", "DGAHF9ABDJDAC042S",
            // MAC
            "", "", "02:6C:C4:AC:E8:61",
            "ЦЕХ", "4-6", "18", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript18()
        {
            AddToDB("id000020", "5", true, 
            "", "", "Antminer Z15 Pro",
            "840k/s",
            // SN
            "", "", "JYZZEVBBCAJJD0098",
            // MAC
            "", "", "78:D0:C5:6B:A1:5A",
            "ЦЕХ", "9-4", "11", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript19()
        {
            AddToDB("id000021", "5", true, 
            "", "", "Bluestar L1",
            "4900Mh/s",
            // SN
            "", "", "XM02LGF0121GHBSL10433",
            // MAC
            "", "", "F4:84:4C:12:11:A7",
            "ЦЕХ", "7-8", "1", 
            "", "", "",
            "", "", "",
            "", "", ""
            );
        }

        public static void StartScript20()
        {
            AddToDB("id000022", "5", true, 
            "", "", "IceRiver KS3M 6th",
            "6TH/s",
            // SN
            "", "", "M01IRKS3M0101813042B0100070",
            // MAC
            "", "", "00:0a:59:00:2e:bf",
            "ЦЕХ", "8-6", "10", 
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
            if (Zealot.MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.AsicsCollection.NAME,
                out string error))
            {
                System.Console.WriteLine($"Коллекция [{DB.AsicsCollection.NAME}] в базе данных " +
                    $" [{DB.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (Zealot.MongoDB.TryCreatingCollection(DB.NAME, DB.AsicsCollection.NAME,
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
            if (Zealot.MongoDB.TryFind(DB.NAME, DB.AsicsCollection.NAME, out string findInfo,
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
            if (Zealot.MongoDB.TryInsertOne(DB.NAME, DB.AsicsCollection.NAME,
            out string info, new BsonDocument()
            {
                { AsicInit._.UNIQUE_NUMBER, uniqueNumber},
                { AsicInit._.CLIENT_ID, clientID},
                { AsicInit._.IS_RUNNING, isRunning},
                { AsicInit._.MODEL_NAME1, modelName1}, 
                { AsicInit._.MODEL_NAME2, modelName2}, 
                { AsicInit._.MODEL_NAME3, modelName3},
                { AsicInit._.POWER, modelPower},
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