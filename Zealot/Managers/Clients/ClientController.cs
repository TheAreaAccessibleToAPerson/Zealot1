using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Butterfly;
using MongoDB.Bson;

namespace Zealot.manager
{
    public sealed class Client : Controller.Board.LocalField<TcpClient>
    {
        public const string NAME = "Client";

        public struct State
        {
            public const string NONE = "None";
            public const string AUTHORIZATION = "Authorization";
            // Конец авторизации отправляет данные о клинте.
            public const string END_AUTHORIZATION = "EndAuthorization";
        }

        private TcpClient _tcpClient { set; get; }

        private bool _isRunning = true;

        private string _currentState = State.NONE;

        private IInput<string> i_setState;

        public IInput<string> I_sendSSLStringMessage;
        public IInput<byte[]> I_sendSSLBytesMessage;

        public IInput<string> I_sendTCPStringMessage;
        public IInput<byte[]> I_sendTCPBytesMessage;

        public string RemoteAddress { set; get; }
        public int RemotePort { set; get; }

        void Start()
        {
            // Приходит подключение.
            // Создается SSL клиент.
            // Принимаем данные логин, пароль.
            // Если верно, то
            // Проверяем не авторизованы ли мы, если да то завершаем прошлую сессию.
            // Если нет, то авторизуемся.
            // Далее создаем новую TCP сессию, отправляем уникальный id и порт клиенту по SSL, по которому нужно подключиться.
            // После клиент устанавливает TCP соединение.
            // После того как соединение установлено, сервер сообщает клинту о начале работы.
            // И высылает ему необходимые данные.
        }

        void ReceiveSSLMessage(int length, byte[] buffer)
        {
            try
            {
                if (_isRunning == false) return;

                if (length < 7) return;

                int index = 0;
                int currentStep = 0; int maxStepCount = 100;
                while (true)
                {
                    if (currentStep++ > maxStepCount)
                    {
                        Logger.W.To(this, $"Превышено одновеменое количесво SSL сообщений принятых из сети.");

                        return;
                    }

                    int headerLength = buffer[index + 0];

                    if (headerLength == 7)
                    {
                        int type = (int)buffer[index + 1] << 8;
                        type += (int)buffer[index + 2];

                        int messageLength = buffer[index + 3] << 24;
                        messageLength += buffer[index + 4] << 16;
                        messageLength += buffer[index + 5] << 8;
                        messageLength += buffer[index + 6];

                        if (index + 7 + messageLength > length)
                        {
                            Logger.S_W.To(this, $"Привышена длина сообщения.[Length:{messageLength}]");

                            destroy();

                            return;
                        }

                        // Сообщение с логином и паролем.
                        if (type == 0)
                        {
                            Logger.I.To(this, "MessageType:0:LoginAndPassword");

                            string str = Encoding.UTF8.GetString(buffer, index + 7, messageLength);

                            LoginAndPassword j = JsonSerializer.Deserialize<LoginAndPassword>(str);

                            Logger.I.To(this, $"Login:{j.login}, Password:{j.password}");

                            CheckLoginAndPassword(j);
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Неизвестный тип:[{type}] сообщения.");
                            destroy();
                        }

                        index += headerLength + messageLength;
                        if (index >= length) return;
                    }
                    else Logger.S_W.To(this, $"Пришло сообщение в котором длина заголовка меньше минимально возможной.");
                }

            }
            catch (Exception ex)
            {
                Logger.I.To(this, $"{ex.ToString()}");

                destroy();
            }
        }

        void Construction()
        {
            RemoteAddress = ((IPEndPoint)Field.Client.RemoteEndPoint).Address.ToString();
            RemotePort = ((IPEndPoint)Field.Client.RemoteEndPoint).Port;

            input_to(ref i_setState, Header.Events.SYSTEM, ISetState);

            add_event(Header.Events.RECEIVE_MESSAGE_FROM_CLIENT, () =>
            {
                if (_isRunning)
                {
                    try
                    {
                        if (Field.Available > 0)
                        {
                            byte[] buffer = new byte[65536];
                            int length = Field.Client.Receive(buffer);

                            Console("New packet length:" + length);

                            ReceiveSSLMessage(length, buffer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"{ex.ToString()}");

                        destroy();
                    }
                }
            });


            input_to(ref I_sendSSLStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, (message) =>
            {
                if (message.Length == 0) return;

                if (_isRunning)
                {
                    try
                    {
                        Console("Send string message");

                        byte[] bytes = Encoding.ASCII.GetBytes(message);

                        Field.Client.Send(bytes);
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"{ex}");

                        destroy();
                    }
                }
            });

            input_to(ref I_sendSSLBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, (message) =>
            {
                if (message.Length == 0) return;

                if (_isRunning)
                {
                    try
                    {
                        Console("Send bytes message");

                        Field.Client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"{ex}");

                        destroy();
                    }
                }
            });
        }

        public void ISetState(string nextState)
        {
            if (_isRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart && !StateInformation.IsDestroy)
                {
                    if (nextState == State.END_AUTHORIZATION)
                    {
                        if (_currentState == State.AUTHORIZATION)
                        {
                            Logger.I.To(this, $"NextState:{_currentState}->{nextState}");

                            _currentState = State.END_AUTHORIZATION;

                            ClientInitialize c = new ClientInitialize()
                            {
                                ID = "1",
                                Name = "TestName",
                                Email = "test@main.ru"
                            };

                            c.II();

                            I_sendSSLBytesMessage.To(ServerMessage.GetMessageArray(ServerMessage.SSLType.CLIENT_DATA,
                                JsonSerializer.SerializeToUtf8Bytes(c)));
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.AUTHORIZATION}");
                    }
                    else if (nextState == State.AUTHORIZATION)
                    {
                        if (_currentState == State.NONE)
                        {
                            Logger.I.To(this, $"NextState:{_currentState}->{nextState}");

                            _currentState = State.AUTHORIZATION;

                            string listenStartingListenConnectionName = $"{BUS.STARTING_LISTEN_TCP_CONNECTION}[Addr:{RemoteAddress}, Port:{RemotePort}]";
                            listen_message<bool, int>(listenStartingListenConnectionName)
                                .output_to((isStarting, port) =>
                                {
                                    if (_isRunning == false) return;

                                    lock (StateInformation.Locker)
                                    {
                                        if (StateInformation.IsStart && !StateInformation.IsDestroy)
                                        {
                                            if (isStarting)
                                            {
                                                I_sendSSLBytesMessage.To(ServerMessage.GetMessageArray(ServerMessage.SSLType.SUCCSESS_AUTHORIZATION,
                                                    JsonSerializer.SerializeToUtf8Bytes(new ClientTCPPort() { port = port })));
                                            }
                                        }
                                    }
                                },
                                Header.Events.SYSTEM);

                            string listenResultConnectionName = $"{BUS.RECEIVE_RESULT_TCP_CONNECTION}[Addr:{RemoteAddress}, Port:{RemotePort}]";
                            listen_message<bool, TcpClient>(listenResultConnectionName)
                                .output_to((isSuccsess, client) =>
                                {
                                    if (_isRunning == false) return;

                                    lock (StateInformation.Locker)
                                    {
                                        if (StateInformation.IsStart && !StateInformation.IsDestroy)
                                        {
                                            if (isSuccsess)
                                            {
                                                Logger.I.To(this, $"Получен tcp клиент.");

                                                _tcpClient = client;

                                                i_setState.To(State.END_AUTHORIZATION);
                                            }
                                            else
                                            {
                                                Logger.I.To(this, $"Неудалось установить tpc соединение.");

                                                // НУЖНО ОПОВЕСТИТЬ КЛИНТА ПО SSL

                                                destroy();
                                            }
                                        }
                                    }
                                },
                                Header.Events.SYSTEM);

                            obj<ReceiveTCPConnection>($"ReceiveTcpConnection[Addr:{RemoteAddress}, Port:{RemotePort}]",
                                new ReceiveTCPConnection.Setting()
                                {
                                    StartingReturn = listenStartingListenConnectionName,
                                    ResultReturn = listenResultConnectionName
                                });
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.NONE}");
                    }
                }
            }

        }

        public bool IsAdmin() => true;

        void Destroyed()
        {
            _isRunning = false;
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

        public struct DB
        {
            public const string NAME = "Clients";

            public struct ClientsCollection 
            {
                public const string NAME = "ClientsCollection";
            }
        }

        public void CheckLoginAndPassword(LoginAndPassword data)
        {
            // Проверяем в базе данных.
            bool result = true;

            if (result)
            {
                i_setState.To(State.AUTHORIZATION);
            }
            else
            {
                //Оповестим что пароль неверный.
            }
        }

        public struct Status
        {
            public const string ADMIN = "Admin";
        }

        public struct BUS
        {
            /// <summary>
            /// Готовы к подключению клинта по TCP
            /// </summary> <summary>
            public const string STARTING_LISTEN_TCP_CONNECTION = NAME + ":StartingReceiveTcpConnection";

            /// <summary>
            /// Результат TCP подключения.
            /// </summary> <summary>
            public const string RECEIVE_RESULT_TCP_CONNECTION = NAME + ":ReceiveResultTcpConnection";
        }
    }
}