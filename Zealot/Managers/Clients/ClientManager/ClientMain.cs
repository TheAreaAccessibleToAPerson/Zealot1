using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Butterfly;
using MongoDB.Bson;

namespace Zealot.manager
{
    public abstract class ClientMain : Controller.Board.LocalField<TcpClient>, Devices.IClientConnect, Clients.IClientConnect
    {
        public const string NAME = "Client";

        protected string CurrentState { set; get; } = Client.State.NONE;

        private TcpClient _tcpClient { set; get; }

        private string _login, _password, _email, _accessRights, _fullName, _organizationName;

        protected void SetTcpClient(TcpClient client)
        {
            if (_tcpClient == null)
            {
                _tcpClient = client;
            }
            else
            {
                Logger.S_E.To(this, $"Вы пытаетесь произвести инициализацию tcp client'a, но он уже проиницилизирован.");

                destroy();

                return;
            }
        }

        protected bool IsRunning { set; get; } = true;

        protected IInput<string> i_setState;

        protected IInput<string> I_sendSSLStringMessage;
        protected IInput<byte[]> I_sendSSLBytesMessage;

        protected IInput<string> I_sendTCPStringMessage;
        protected IInput<byte[]> I_sendTCPBytesMessage;

        // Добавляет новые асики. Отправлет задачу на выполнение в DeviceManager.
        // После получает результат.
        protected IInput<List<AddNewAsicJson>, Clients.IClientConnect> I_addNewAsics;

        /// <summary>
        /// Добовляет нового клиента в ClientsManager.
        /// </summary>
        protected IInput<AddNewClient, Clients.IClientConnect> I_addNewClient;

        /// <summary>
        /// После того как был добавлен новый клиент разошлем его всем админам.
        /// </summary>
        protected IInput<ClientData> I_sendNewClient;

        public void SendTcpMessage(string message)
        {
            if (IsRunning)
            {
                I_sendTCPStringMessage.To(message);
            }
        }

        public void SendTcpMessage(byte[] message)
        {
            if (IsRunning)
            {
                I_sendTCPBytesMessage.To(message);
            }
        }

        public void SendSslMessage(string message)
        {
            if (IsRunning)
            {
                I_sendSSLStringMessage.To(message);
            }
        }

        public void SendSslMessage<JsonType>(JsonType json, int type)
        {
            if (IsRunning)
            {
                // Отправляет клиенту результат добавление нового клиeнта.
                byte[] message = ServerMessage.GetMessageArray(type, JsonSerializer.SerializeToUtf8Bytes(json));

                I_sendSSLBytesMessage.To(message);
            }
        }

        public void SendSslMessage(byte[] message)
        {
            if (IsRunning)
            {
                I_sendSSLBytesMessage.To(message);
            }
        }

        public void SendMessage(byte[] message)
        {
            if (CurrentState == Client.State.RUNNING)
            {
                SendSslMessage(message);
            }
        }

        public void SendMessage(string message)
        {
            if (CurrentState == Client.State.RUNNING)
            {
                SendSslMessage(message);
            }
        }

        public void SendMessage<JsonType>(JsonType json, int type)
        {
            if (CurrentState == Client.State.RUNNING)
            {
                SendSslMessage(json, type);
            }
        }

        public string RemoteAddress { set; get; }
        public int RemotePort { set; get; }

        public ClientInitialize ClientInitialize { private set; get; }

        void ReceiveTcpMessage(int length, byte[] buffer)
        {
        }

        void ReceiveSSLMessage(int length, byte[] buffer)
        {
            try
            {
                if (IsRunning == false) return;

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

                            Console(str);

                            LoginAndPasswordJson j = JsonSerializer.Deserialize<LoginAndPasswordJson>(str);

                            Logger.I.To(this, $"Login:{j.login}, Password:{j.password}");

                            invoke_event(() => CheckLoginAndPassword(j), Header.Events.MONGO_DB);
                        }
                        // Добавить новый машинки.
                        else if (type == 1)
                        {
                            Logger.I.To(this, "MessageType:1:AddNewAsics");

                            string str = Encoding.UTF8.GetString(buffer, index + 7, messageLength);

                            Console(str);

                            List<AddNewAsicJson> j = JsonSerializer.Deserialize<List<AddNewAsicJson>>(str);
                        }
                        else if (type == 2)
                        {
                            Logger.I.To(this, "MessageType:2:AddNewClient");

                            string str = Encoding.UTF8.GetString(buffer, index + 7, messageLength);

                            Console(str);

                            AddNewClient j = JsonSerializer.Deserialize<AddNewClient>(str);

                            I_addNewClient.To(j, this);
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Неизвестный тип:[{type}] сообщения.");

                            destroy();

                            return;
                        }

                        index += headerLength + messageLength;
                        if (index >= length) return;
                    }
                    else
                    {
                        Logger.S_W.To(this, $"Пришло сообщение в котором длина заголовка меньше минимально возможной.");

                        destroy();

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.I.To(this, $"{ex.ToString()}");

                destroy();

                return;
            }
        }

        protected void ISendSSLStringMessage(string message)
            => ISendSSLByteMessage(Encoding.ASCII.GetBytes(message));

        protected void ISendSSLByteMessage(byte[] message)
        {
            if (message.Length == 0) return;

            try
            {
                Console($"Send bytes ssl message. Length:{message.Length}");

                Field.Client.Send(message);
            }
            catch (Exception ex)
            {
                Logger.I.To(this, $"{ex}");

                destroy();

                return;
            }
        }

        protected void ISendTCPStringMessage(string message)
            => ISendTCPByteMessage(Encoding.ASCII.GetBytes(message));

        protected void ISendTCPByteMessage(byte[] message)
        {
            if (message.Length == 0) return;

            if (_tcpClient != null)
            {
                try
                {
                    _tcpClient.Client.Send(message);
                }
                catch (Exception ex)
                {
                    Logger.I.To(this, $"{ex}");

                    destroy();

                    return;
                }
            }
        }

        protected void ReceiveMessageFromClient()
        {
            if (IsRunning)
            {
                try
                {
                    if (Field.Available > 0)
                    {
                        byte[] buffer = new byte[65536];
                        int length = Field.Client.Receive(buffer);

                        Console("New ssl packet length:" + length);

                        ReceiveSSLMessage(length, buffer);
                    }

                    if (_tcpClient != null)
                    {
                        if (_tcpClient.Available > 0)
                        {
                            byte[] buffer = new byte[65536];
                            int length = Field.Client.Receive(buffer);

                            Console("New tcp packet length:" + length);

                            ReceiveTcpMessage(length, buffer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.I.To(this, $"{ex.ToString()}");

                    destroy();

                    return;
                }
            }
        }

        public bool IsAdmin() => true;
        public bool IsRoot() => true;

        protected void EAddNewAsics(List<AddNewAsicJson> value)
        {
            if (IsAdmin() == false)
            {
                Logger.W.To(this, $"Клиент не являющийся Admin пытается добавить новые устройсва.");

                destroy();

                return;
            }
            else
            {
                Logger.I.To(this, $"Отправляем запрос на добавление {value.Count} асиков.");

                I_addNewAsics.To(value, this);
            }
        }

        protected void EAddNewAsicsResult(List<AddNewAsicsResultJson> value)
        {
        }

        protected void EAddNewClientResult(AddNewClientResult result, ClientData data)
        {
            if (IsAdmin())
            {
                Logger.I.To(this,
                    $"New client result:\n" +
                $"{Clients.DB.Client.Collection.Key.LOGIN}:{result.LoginResult}\n" +
                $"{Clients.DB.Client.Collection.Key.PASSWORD}:{result.PasswordResult}\n" +
                $"{Clients.DB.Client.Collection.Key.EMAIL}:{result.EmailResult}\n" +
                $"{Clients.DB.Client.Collection.Key.FULL_NAME}:{result.FullNameResult}\n" +
                $"{Clients.DB.Client.Collection.Key.ORGANIZATION_NAME}:{result.OrganizationNameResult}\n"
                );

                SendSslMessage(result, ServerMessage.SSLType.ADD_NEW_CLIENT_RESULT);

                if (result.IsSuccess && data != null)
                {
                    Logger.I.To(this, "Успешно добавление нового клиента, приступаем к рассылки всем администраторам данных о нем.");

                    I_sendNewClient.To(data);
                }
                else Logger.I.To(this, "Неудалось добавить нового клиента.");
            }
            else
            {
                Logger.S_E.To(this, $"Вам пришел результат добавления нового клинта, но данный клиент не является админом.");

                destroy();

                return;
            }
        }

        /// <summary>
        /// По данному логину.
        /// </summary>
        public string GetClientLogin()
        {
            if (ClientInitialize != null)
            {
                return ClientInitialize.Login;
            }
            else
            {
                Logger.S_E.To(this, $"Вы запросили логин клиента(в компании), но к этому моменту еще небыл проинициализирован обьект " +
                    $" в котором хранится данное значение.(CurrentState:{CurrentState})");

                destroy();

                return "";
            }
        }

        public void CheckLoginAndPassword(LoginAndPasswordJson data)
        {
            if (data.login.Length < 4)
            {
                Logger.I.To(this, $"Полученый логин слишком короткий [{data.login}].");

                destroy();

                return;
            }
            else if (data.password.Length < 3)
            {
                Logger.I.To(this, $"Полученый пароль слишком короткий [{data.password}].");

                destroy();

                return;
            }
            else
            {
                // Проверяем наличие документа хранящего адреса и диопазоны адресов.
                if (MongoDB.TryFind(Clients.DB.Client.NAME, Clients.DB.Client.Collection.NAME, out string findInfo,
                    out List<BsonDocument> clients))
                {
                    try
                    {
                        foreach (BsonDocument client in clients)
                        {
                            if (data.login == client[Clients.DB.Client.Collection.Key.LOGIN])
                            {
                                if (data.password == client[Clients.DB.Client.Collection.Key.PASSWORD])
                                {
                                    if (ClientInitialize == null)
                                    {
                                        ClientInitialize = new ClientInitialize()
                                        {
                                            Login = data.login,
                                            Password = data.password,
                                            FullName = client[Clients.DB.Client.Collection.Key.FULL_NAME].ToString(),
                                            Email = client[Clients.DB.Client.Collection.Key.EMAIL].ToString(),
                                            OrganizationName = client[Clients.DB.Client.Collection.Key.ORGANIZATION_NAME].ToString(),
                                            AccessRights = client[Clients.DB.Client.Collection.Key.ACCESS_RIGHTS].ToString(),
                                            AsicsCount = client[Clients.DB.Client.Collection.Key.ASICS_COUNT].ToInt32(),
                                            CreatingDate = client[Clients.DB.Client.Collection.Key.CREATING_DATE].ToString(),
                                            WorkUntilWhatDate = client[Clients.DB.Client.Collection.Key.WORK_UNTIL_WHAT_DATE].ToString(),
                                            IsRunning = client[Clients.DB.Client.Collection.Key.IS_RUNNING].ToBoolean(),
                                        };

                                        if (ClientInitialize.IsInitialize)
                                        {
                                            i_setState.To(Client.State.AUTHORIZATION);

                                            return;
                                        }
                                        else
                                        {
                                            Logger.S_W.To(this, ClientInitialize.Error);

                                            destroy();

                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Logger.S_E.To(this, $"Вы пытаетесь дважды проинициализировать поле " +
                                            "хранящее информацию из базы данных о клинте.");

                                        destroy();

                                        return;
                                    }
                                }
                                else
                                {
                                    // Если пароль не подходит, то запишим попытку,

                                    // Оповестим клинта.

                                    // Удалим обьект.

                                    Logger.I.To(this, $"Поступил пароль [{data.password}] не соответвующий логину:[{data.login}].");

                                    destroy();

                                    return;
                                }
                            }
                        }

                        // Такого логина не сущесвует, оповестим клинта, и удалиим текущий обьект.
                        Logger.I.To(this, $"Поступил несущесвующий логин:[{data.login}].");

                        destroy();

                        return;
                    }
                    catch (Exception ex)
                    {
                        Logger.W.To(this, ex.ToString());

                        destroy();

                        return;
                    }
                }
            }
        }

        public string GetFullNameClient()
        {
            throw new NotImplementedException();
        }

        public string GetOrganizationName()
        {
            throw new NotImplementedException();
        }

        public string GetLogin()
        {
            throw new NotImplementedException();
        }

        public string GetPassword()
        {
            throw new NotImplementedException();
        }

        public string GetEmail()
        {
            throw new NotImplementedException();
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
