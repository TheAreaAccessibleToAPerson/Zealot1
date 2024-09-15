using System.Net.Sockets;
using System.Text.Json;
using Butterfly;

namespace Zealot.manager
{
    public abstract class ClientController : ClientMain
    {


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
                    if (nextState == Client.State.RUNNING)
                    {
                        if (CurrentState == Client.State.START_SUBSCRIBE_TO_ASICS)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = Client.State.RUNNING;
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                                $" $только если текущее состояние {Client.State.END_AUTHORIZATION}");

                            destroy();

                            return;
                        }
                    }
                    else if (nextState == Client.State.START_SUBSCRIBE_TO_ASICS)
                    {
                        if (TryIncrementEvent())
                        {
                            if (CurrentState == Client.State.END_AUTHORIZATION)
                            {
                                Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                                CurrentState = Client.State.START_SUBSCRIBE_TO_ASICS;

                                I_subscribeToAsicsMessage.To(this);
                            }
                            else
                            {
                                Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                                    $" $только если текущее состояние {Client.State.END_AUTHORIZATION}");

                                destroy();

                                return;
                            }

                            invoke_event(() =>
                            {
                                lock (StateInformation.Locker)
                                {
                                    if (CurrentState == Client.State.START_SUBSCRIBE_TO_ASICS)
                                    {
                                        Logger.S_E.To(this, $"Обьект начал подписку на получение сообщений от асиков, " +
                                            $"ему заблокировали возможность уничтжения, далее состояние было выставлено на [{Client.State.START_SUBSCRIBE_TO_ASICS}]" +
                                                $" и ожидалось что в ответном сообщении снимится возможность уничтожения, " +
                                                   $"а так же смениться состояние на [{Client.State.RUNNING}]");

                                        DecrementEvent();

                                        destroy();
                                    }
                                }
                            },
                            10000, Header.Events.SYSTEM);
                        }
                        else
                        {
                            Logger.I.To(this, $"Неудалось начать подписку на прослушивание сообщений от асиков, " +
                                    "так как обьект приступил к своему уничтожению.");

                            destroy();

                            return;
                        }
                    }
                    else if (nextState == Client.State.END_AUTHORIZATION)
                    {
                        if (CurrentState == Client.State.GET_ASICS_INFORMATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = Client.State.END_AUTHORIZATION;

                            if (ClientInitialize != null)
                            {
                                // Отправляем клиенту данные.
                                SendSslMessage(ServerMessage.GetMessageArray(ServerMessage.SSLType.CLIENT_DATA,
                                    JsonSerializer.SerializeToUtf8Bytes(ClientInitialize)));

                                i_setState.To(Client.State.START_SUBSCRIBE_TO_ASICS);
                            }
                            else
                            {
                                Logger.S_E.To(this, $"В момент смены состояния на {Client.State.END_AUTHORIZATION} уже должно быть " +
                                    "проинициализировано поле ClientInitialize");

                                destroy();
                            }
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {Client.State.GET_ASICS_INFORMATION}");
                    }
                    else if (nextState == Client.State.GET_ASICS_INFORMATION)
                    {
                        if (CurrentState == Client.State.AUTHORIZATION)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = Client.State.GET_ASICS_INFORMATION;

                            I_getAsics.To(this);
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                                $" $только если текущее состояние {Client.State.AUTHORIZATION}");

                            destroy();

                            return;
                        }
                    }
                    else if (nextState == Client.State.AUTHORIZATION)
                    {
                        if (CurrentState == Client.State.NONE)
                        {
                            Logger.I.To(this, $"NextState:{CurrentState}->{nextState}");

                            CurrentState = Client.State.AUTHORIZATION;

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

                                                return;
                                            }
                                        }
                                    }
                                },
                                Header.Events.SYSTEM);

                            string listenResultConnectionName = $"{BUS.RECEIVE_RESULT_TCP_CONNECTION}[Addr:{RemoteAddress}, Port:{RemotePort}]";
                            listen_message<bool, TcpClient>(listenResultConnectionName)
                                .output_to((isSuccess, client) =>
                                {
                                    if (IsRunning == false) return;

                                    lock (StateInformation.Locker)
                                    {
                                        if (StateInformation.IsStart && !StateInformation.IsDestroy)
                                        {
                                            if (isSuccess)
                                            {
                                                Logger.I.To(this, $"Получен tcp клиент.");

                                                SetTcpClient(client);

                                                i_setState.To(Client.State.GET_ASICS_INFORMATION);
                                            }
                                            else
                                            {
                                                Logger.I.To(this, $"Неудалось установить tpc соединение.");

                                                // НУЖНО ОПОВЕСТИТЬ КЛИНТА ПО SSL

                                                destroy();

                                                return;
                                            }
                                        }
                                    }
                                } ,
                                Header.Events.SYSTEM);

                            obj<ReceiveTCPConnection>($"ReceiveTcpConnection[Addr:{RemoteAddress}, Port:{RemotePort}]",
                                new ReceiveTCPConnection.Setting()
                                {
                                    StartingReturn = listenStartingListenConnectionName,
                                    ResultReturn = listenResultConnectionName
                                });
                        }
                        else Logger.S_E.To(this, $"Вы можете сменить состояние обьекта на {nextState}, " +
                            $" $только если текущее состояние {Client.State.NONE}");
                    }
                }
            }

        }

    }
}