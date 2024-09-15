using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager
{
    public abstract class ClientsController : ClientsMain
    {

        protected void AddConnectionClient(TcpClient client)
        {
            string key = $"{((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()}:" +
                $"{((IPEndPoint)client.Client.RemoteEndPoint).Port}";

            if (StateInformation.IsStart && !StateInformation.IsDestroy)
            {
                if (try_obj(key, out Client obj))
                {
                    Logger.I.To(this, $"Клиент с ключом {key} уже подключон.");
                }
                else
                {
                    Logger.I.To(this, $"Connection new client:{key}");

                    Client newClient = obj<Client>(key, client);

                    Clients.Add(key, newClient);
                }
            }
            else
            {
                Logger.W.To(this, $"Неудалось поключить нового клинта {key}, " +
                    " так как ClientsManager завершает свою работу.");
            }
        }

        protected void RemoveDisconnectionClient(Clients.IClientConnect client)
        {
            if (Clients.Remove(client.GetKey()))
            {
                Logger.I.To(this, $"Client remove from ClientCollection.");
            }
            else
            {
                Logger.S_E.To(this, $"Client not remove from ClientCollection.");

                destroy();

                return;
            }
        }

        protected void AddNewClient(AddNewClient addNewClientData, Clients.IClientConnect client, 
            IReturn<AddNewClientResult> @return)
        {
            /*
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
            */
        }
    }
}