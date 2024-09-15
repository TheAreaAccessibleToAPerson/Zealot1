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
        protected IInput<List<AddNewAsic>, Clients.IClientConnect> I_addNewAsics;

        /// <summary>
        /// Добовляет нового клиента в ClientsManager.
        /// </summary>
        protected IInput<AddNewClient, Clients.IClientConnect> I_addNewClient;

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

        public void SendSslMessage(byte[] message)
        {
            if (IsRunning)
            {
                I_sendSSLBytesMessage.To(message);
            }
        }

        public void SendMessage(byte[] message)
        {
            if (IsRunning && CurrentState == Client.State.RUNNING)
            {
                SendTcpMessage(message);
            }
        }

        public void SendMessage(string message)
        {
            if (IsRunning && CurrentState == Client.State.RUNNING)
            {
                SendTcpMessage(message);
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

                            LoginAndPassword j = JsonSerializer.Deserialize<LoginAndPassword>(str);

                            Logger.I.To(this, $"Login:{j.login}, Password:{j.password}");

                            invoke_event(() => CheckLoginAndPassword(j), Header.Events.MONGO_DB);
                        }
                        // Добавить новый машинки.
                        else if (type == 1)
                        {
                            Logger.I.To(this, "MessageType:1:AddNewAsics");

                            string str = Encoding.UTF8.GetString(buffer, index + 7, messageLength);

                            Console(str);

                            List<AddNewAsic> j = JsonSerializer.Deserialize<List<AddNewAsic>>(str);
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
                    else Logger.S_W.To(this, $"Пришло сообщение в котором длина заголовка меньше минимально возможной.");
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

        protected void EAddNewAsics(List<AddNewAsic> value)
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

        protected void EAddNewAsicsResult(List<AddNewAsicsResult> value)
        {

        }

        protected void EAddNewClientResult(AddNewClientResult value)
        {
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

        public struct DB
        {
            public const string NAME = "Clients";

            public struct AsicsCollection
            {
                public const string NAME = "AsicsCollection";

                // ID клиента которому принадлежит асик.
                public const string CLIENT_ID = "ClientID";
                public const string DATA_JSON = "DataJson";
            }

            public struct ClientsCollection
            {
                public const string NAME = "ClientsCollection";

                public struct Client
                {
                    // Активирован ли данный аккаунт.
                    public const string IS_ACITVATED = "IsActivated";

                    // Логин
                    public const string LOGIN = "Login";

                    // Пароль.
                    public const string PASSWORD = "Password";

                    public const string ID = "ID";
                    public const string NAME = "Name";
                    public const string EMAIL = "Email";
                    public const string ASICS_JSON = "AsicsJson";

                    // Права доступа.
                    public struct ACCESS_RIGHTS
                    {
                        public const string STR = "AccessRigths";

                        public const string ROOT = "Root";
                    }
                }
            }
        }

        public void CheckLoginAndPassword(LoginAndPassword data)
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
                if (MongoDB.TryFind(DB.NAME, DB.ClientsCollection.NAME, out string findInfo,
                    out List<BsonDocument> clients))
                {
                    try
                    {
                        foreach (BsonDocument client in clients)
                        {
                            if (data.login == client[DB.ClientsCollection.Client.LOGIN])
                            {
                                if (data.password == client[DB.ClientsCollection.Client.PASSWORD])
                                {
                                    if (ClientInitialize == null)
                                    {
                                        ClientInitialize = new ClientInitialize()
                                        {
                                            ID = client[DB.ClientsCollection.Client.ID].ToString(),
                                            Name = client[DB.ClientsCollection.Client.NAME].ToString(),
                                            Email = client[DB.ClientsCollection.Client.EMAIL].ToString(),
                                            AccessRights = client[DB.ClientsCollection.Client.ACCESS_RIGHTS.STR].ToString(),
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
