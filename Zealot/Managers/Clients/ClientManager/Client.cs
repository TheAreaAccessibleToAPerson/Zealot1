using System.Net;
using Butterfly;
using MongoDB.Bson;

namespace Zealot.manager
{
    // КЛИЕНТ АДМИН ЧЕРЕЗ ОПРЕДЕЛЕННЫЙ ПРОМЕЖУТОК ВРЕМЕНИ ДОЛЖЕН ЗАПРАШИВАТЬ
    // МАШИНЫ НЕ СОХРАНЕНЫЕ В БАЗЕ ДАННЫХ.
    public sealed class Client : ClientController
    {
        private IInput<Clients.IClientConnect> i_removeFromClientsCollection;

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

            /*
            send_echo_2_1<List<AddNewAsic>, Clients.IClientConnect, List<AddNewAsicsResult>>(ref I_addNewAsics, Devices.BUS.Asic.ADD_NEW_ASIC)
                .output_to(EAddNewAsicsResult, Header.Events.SYSTEM);
                */

            send_echo_2_2<AddNewClient, Clients.IClientConnect, AddNewClientResult, ClientData>(ref I_addNewClient, Clients.BUS.ADD_NEW_CLIENT)
                .output_to(EAddNewClientResult, Header.Events.SYSTEM);

            send_message(ref i_removeFromClientsCollection, Clients.BUS.REMOVE_DISCONNECTION_CLIENT);
            send_message(ref I_sendNewClient, Clients.BUS.SEND_NEW_CLIENT_TO_ADMINS);

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

                        return;
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
                if (StateInformation.IsCallStart)
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
            if (MongoDB.ContainsDatabase(Clients.DB.Client.NAME, out string containsDBerror))
            {
                Logger.S_I.To(this, $"База данныx {Clients.DB.Client.NAME} уже создана.");
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
                    Logger.S_I.To(this, $"Создаем базу данных {Clients.DB.Client.NAME}.");

                    if (MongoDB.TryCreatingDatabase(Clients.DB.Client.NAME, out string info))
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
            if (MongoDB.ContainsCollection<BsonDocument>(Clients.DB.Client.NAME, Clients.DB.Client.Collection.NAME,
                out string error))
            {
                Logger.S_I.To(this, $"Коллекция [{Clients.DB.Client.Collection.NAME}] в базе данных " +
                    $" [{Clients.DB.Client.NAME}] уже создана.");
            }
            else
            {
                // Коллекции нету, создадим ее.
                if (error == "")
                {
                    if (MongoDB.TryCreatingCollection(Clients.DB.Client.NAME, Clients.DB.Client.Collection.NAME,
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

        public struct State
        {
            public const string NONE = "None";

            // 1)Происзодит авторизацию.
            // Проверяет данные, создает TCP подключение.
            public const string AUTHORIZATION = "Authorization";

            // 2)Запрашивает данные своих машинок.
            public const string GET_ASICS_INFORMATION = "GetAsicsInformation";

            // 3)Конец авторизации отправляет данные о клинте.
            // Отправляет данные кленту.
            public const string END_AUTHORIZATION = "EndAuthorization";

            /// <summary>
            /// Начинаем подключение к машинам.
            /// </summary> 
            public const string START_SUBSCRIBE_TO_ASICS = "StartSubscribeToAsics";

            /// <summary>
            /// Все клиент в работе.
            /// </summary> 
            public const string RUNNING = "Running";
        }
    }
}