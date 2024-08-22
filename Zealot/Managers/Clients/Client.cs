using System.Net;
using Butterfly;
using MongoDB.Bson;

namespace Zealot.manager
{
    // КЛИЕНТ АДМИН ЧЕРЕЗ ОПРЕДЕЛЕННЫЙ ПРОМЕЖУТОК ВРЕМЕНИ ДОЛЖЕН ЗАПРАШИВАТЬ
    // МАШИНЫ НЕ СОХРАНЕНЫЕ В БАЗЕ ДАННЫХ.
    public sealed class Client : ClientController
    {
        private IInput<Client> i_removeFromClientsCollection;

        void Start()
        {
            Logger.I.To(this, "starting ...");
            {
                i_setState.To(State.AUTHORIZATION);
            }
        }

        void Construction()
        {
            RemoteAddress = ((IPEndPoint)Field.Client.RemoteEndPoint).Address.ToString();
            RemotePort = ((IPEndPoint)Field.Client.RemoteEndPoint).Port;

            input_to(ref i_setState, Header.Events.SYSTEM, ISetState);
            input_to(ref I_sendSSLStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendSSLStringMessage);
            input_to(ref I_sendSSLBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendSSLByteMessage);
            input_to(ref I_sendTCPStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendTCPStringMessage);
            input_to(ref I_sendTCPBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendTCPByteMessage);

            add_event(Header.Events.RECEIVE_MESSAGE_FROM_CLIENT, ReceiveMessageFromClient);

            send_message(ref i_removeFromClientsCollection, Clients.BUS.DELETE_CLIENT);
            send_echo_1_1<Devices.IClientConnect, List<AsicInit>>(ref I_getAsics, Devices.BUS.Client.GET_CLIENT_ASICS)
                .output_to((asics) =>
                {
                    if (ClientInitialize.SetAsics(asics, out string info))
                    {
                        Logger.I.To(this, info);

                        i_setState.To(State.END_AUTHORIZATION);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();
                    }
                },
                Header.Events.SYSTEM);

            send_echo_1_1<Devices.IClientConnect, List<string[]>>(ref I_subscribeToAsicsMessage, Devices.BUS.Client.SUBSCRIBE_TO_MESSAGE)
                .output_to((subAsics) =>
                {
                    DecrementEvent();

                    string u = "Subscribe to asics:";
                    for (int i = 0; i < subAsics.Count; i++)
                        u += $"\n{i + 1})MAC:[{subAsics[i][0]}], SN:[{subAsics[i][1]}]";

                    Logger.I.To(this, u);

                    i_setState.To(State.RUNNING);
                },
                Header.Events.SYSTEM);

            send_message(ref I_unsubscribeToAsicsMessage, Devices.BUS.Client.UNSUBSCRIBE_TO_MESSAGE);
        }

        void Stop()
        {
            Logger.I.To(this, "stopping ...");
            {
                if (StateInformation.IsCallConstruction)
                {
                    // Отпишимся от получение сообщений.
                    // Из машинок.
                    I_unsubscribeToAsicsMessage.To(this);
                }
            }
        }

        void Destruction()
        {
            // Stop может быть не вызван.
            // Поэтому удаляем обьект из коллекции вот от сюда.
            Logger.I.To(this, "destruction call ...");
            {
                if (StateInformation.IsCallConstruction)
                {
                    i_removeFromClientsCollection.To(this);
                }
            }
        }

        void Destroyed()
        {
            IsRunning = false;
        }

        void Configurate()
        {
            if (MongoDB.ContainsDatabase(DB.NAME, out string containsDBerror))
            {
                Logger.S_I.To(this, $"База данныx {DB.NAME} уже создана.");
            }
            else
            {
                if (containsDBerror != "")
                {
                    Logger.S_E.To(this, containsDBerror);

                    destroy();

                    return;
                }
                else
                {
                    Logger.S_I.To(this, $"Создаем базу данных {DB.NAME}.");

                    if (MongoDB.TryCreatingDatabase(DB.NAME, out string info))
                    {
                        Logger.S_I.To(this, info);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();

                        return;
                    }
                }
            }

            // Проверяем наличие коллекции.
            if (MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.ClientsCollection.NAME,
                out string error))
            {
                Logger.S_I.To(this, $"Коллекция [{DB.ClientsCollection.NAME}] в базе данных " +
                    $" [{DB.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (MongoDB.TryCreatingCollection(DB.NAME, DB.ClientsCollection.NAME,
                        out string info))
                    {
                        Logger.S_I.To(this, info);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();

                        return;
                    }
                }
                else
                {
                    Logger.S_E.To(this, error);

                    destroy();

                    return;
                }
            }
        }
    }
}