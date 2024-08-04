using System.Net.Sockets;
using System.Text.Json;
using Butterfly;

namespace Zealot.manager
{
    public abstract class ClientController : MainClient
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
        }

        protected string CurrentState { set; get; } = State.NONE;

        /// <summary>
        /// Запрашиваем все машинки которые пренадлежат клиенту.
        /// </summary> <summary>
        protected IInput<string> I_getAsics;

        protected void ISetState(string nextState)
        {
            if (IsRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart && !StateInformation.IsDestroy)
                {
                    if (nextState == State.GET_ASICS_INFORMATION)
                    {
                        if (CurrentState == State.AUTHORIZATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.GET_ASICS_INFORMATION;

                            I_getAsics.To(ClientInitialize.ID);
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {State.AUTHORIZATION}");
                    }
                    else if (nextState == State.END_AUTHORIZATION)
                    {
                        if (CurrentState == State.GET_ASICS_INFORMATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = State.END_AUTHORIZATION;

                            if (ClientInitialize != null)
                            {
                                SendSslMessage(ServerMessage.GetMessageArray(ServerMessage.SSLType.CLIENT_DATA,
                                    JsonSerializer.SerializeToUtf8Bytes(ClientInitialize)));
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
    }

}