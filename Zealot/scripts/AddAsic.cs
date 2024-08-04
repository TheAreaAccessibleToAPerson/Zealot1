using MongoDB.Bson;
using static Zealot.manager.Devices;

namespace Zealot.script
{
    /// <summary>
    /// Скрипт для добовления клинтов.
    /// </summary> <summary>
    public static class AddAsic
    {
        public static void StartScript()
        {
            //AddToDB(true, "root", "root", "00000000", "root", "root@main.ru", "root");
            AddToDB("a0000001", "00000000", true, 
            "SN001", "SN001", "SN001", 
            "MAC001", "MAC001", "MAC001",
            "ЦЕХ", "1-1", "3", 
            "addr1", "name1", "password1",
            "addr2", "name2", "password2",
            "addr3", "name3", "password3"
            );
        }

        private static void AddToDB(string uniqueNumber, string clientID, bool isRunning,
            string SN1, string SN2, string SN3, string MAC1, string MAC2, string MAC3,
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