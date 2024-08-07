using System.Net.Sockets;
using System.Text.Json;
using Butterfly;

namespace Zealot.manager
{
    public abstract class ClientController : MainClient, Devices.IClientConnect
    {
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

        protected string CurrentState { set; get; } = State.NONE;

        /// <summary>
        /// Запрашиваем все машинки которые пренадлежат клиенту.
        /// </summary> <summary>
        protected IInput<Devices.IClientConnect> I_getAsics;

        // Подписываемся на полуение сообщение от наших асиков.
        protected IInput<Devices.IClientConnect> I_subscribeToAsicsMessage;

        // Отписываемся от полуение сообщенй от наших асиков.
        protected IInput<Devices.IClientConnect> I_unsubscribeToAsicsMessage;

        protected void ISetState(string nextState)
        {
            if (IsRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart && !StateInformation.IsDestroy)
                {
                    if (nextState == State.RUNNING)
                    {
                        if (CurrentState == State.START_SUBSCRIBE_TO_ASICS)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.RUNNING;
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.END_AUTHORIZATION}");
                    }
                    else if (nextState == State.START_SUBSCRIBE_TO_ASICS)
                    {
                        if (TryIncrementEvent())
                        {
                            if (CurrentState == State.END_AUTHORIZATION)
                            {
                                Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                                CurrentState = State.START_SUBSCRIBE_TO_ASICS;

                                I_subscribeToAsicsMessage.To(this);
                            }
                            else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                                $" $только если текущее состояние {State.END_AUTHORIZATION}");

                            invoke_event(() =>
                            {
                                if (CurrentState == State.START_SUBSCRIBE_TO_ASICS)
                                {
                                    Logger.S_E.To(this, $"Обьект начал подписку на получение сообщений от асиков, " +
                                        $"ему заблокировали возможность уничтжения, далее состояние было выставлено на [{State.START_SUBSCRIBE_TO_ASICS}]" +
                                            $" и ожидалось что в ответном сообщении снимится возможность уничтожения, " +
                                               $"а так же смениться состояние на [{State.RUNNING}]");

                                    DecrementEvent();

                                    destroy();
                                }
                            },
                            10000, Header.Events.SYSTEM);
                        }
                        else Logger.I.To(this, $"Неудалось начать подписку на прослушивание сообщений от асиков, " +
                                "так как обьект приступил к своему уничтожению.");
                    }
                    else if (nextState == State.END_AUTHORIZATION)
                    {
                        if (CurrentState == State.GET_ASICS_INFORMATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.END_AUTHORIZATION;

                            if (ClientInitialize != null)
                            {
                                // Отправляем клиенту данные.
                                SendSslMessage(ServerMessage.GetMessageArray(ServerMessage.SSLType.CLIENT_DATA,
                                    JsonSerializer.SerializeToUtf8Bytes(ClientInitialize)));

                                i_setState.To(State.START_SUBSCRIBE_TO_ASICS);
                            }
                            else
                            {
                                Logger.S_E.To(this, $"В момент смены состояния на {State.END_AUTHORIZATION} уже должно быть " +
                                    "проинициализировано поле ClientInitialize");

                                destroy();
                            }
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.GET_ASICS_INFORMATION}");
                    }
                    else if (nextState == State.GET_ASICS_INFORMATION)
                    {
                        if (CurrentState == State.AUTHORIZATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.GET_ASICS_INFORMATION;

                            I_getAsics.To(this);
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.AUTHORIZATION}");
                    }
                    else if (nextState == State.AUTHORIZATION)
                    {
                        if (CurrentState == State.NONE)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.AUTHORIZATION;

                            string listenStartingListenConnectionName = $"{BUS.STARTING_LISTEN_TCP_CONNECTION}[Addr:{RemoteAddress}, Port:{RemotePort}]";
                            listen_message<bool, int>(listenStartingListenConnectionName)
                                .output_to((isStarting, port) =>
                                {
                                    if (IsRunning == false) return;

                                    lock (StateInformation.Locker)
                                    {
                                        if (StateInformation.IsStart && !StateInformation.IsDestroy)
                                        {
                                            if (isStarting)
                                            {
                                                Logger.I.To(this, $"Отправляем клиенту порт [{port}] для TCP подключения.");

                                                SendSslMessage(ServerMessage.GetMessageArray(ServerMessage.SSLType.SUCCSESS_AUTHORIZATION,
                                                    JsonSerializer.SerializeToUtf8Bytes(new ClientTCPPort() { port = port })));
                                            }
                                            else
                                            {
                                                Logger.W.To(this, $"Неудалось запустить прослушку TCP соединения.");

                                                destroy();
                                            }
                                        }
                                    }
                                },
                                Header.Events.SYSTEM);

                            string listenResultConnectionName = $"{BUS.RECEIVE_RESULT_TCP_CONNECTION}[Addr:{RemoteAddress}, Port:{RemotePort}]";
                            listen_message<bool, TcpClient>(listenResultConnectionName)
                                .output_to((isSuccsess, client) =>
                                {
                                    if (IsRunning == false) return;

                                    lock (StateInformation.Locker)
                                    {
                                        if (StateInformation.IsStart && !StateInformation.IsDestroy)
                                        {
                                            if (isSuccsess)
                                            {
                                                Logger.I.To(this, $"Получен tcp клиент.");

                                                SetTcpClient(client);

                                                i_setState.To(State.GET_ASICS_INFORMATION);
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

        public void SendMessage(byte[] message)
        {
            if (IsRunning && CurrentState == State.RUNNING)
            {
            }
        }

        public void SendMessage(string message)
        {
            if (IsRunning && CurrentState == State.RUNNING)
            {
            }
        }

        /// <summary>
        /// По данному ID клиент хранится в нутри компании.
        /// </summary>
        public string GetClientID()
        {
            if (ClientInitialize != null)
            {
                return ClientInitialize.ID;
            }
            else
            {
                Logger.S_E.To(this, $"Вы запросили id клиента(в компании), но к этому моменту еще небыло проинициализировано поле" +
                    $" в котором хранится данное значение.(CurrentState:{CurrentState})");

                destroy();

                return "";
            }
        }
    }
}