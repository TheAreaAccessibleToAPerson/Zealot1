using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager
{
    public sealed class Clients : ClientsController
    {
        public const string NAME = "ClientsManager";


        void Construction()
        {
            DevicesManager = obj<Devices>(Devices.NAME);

            // Сюда приходит сообщение содеждащее новое SSL подключение,
            // обрабатываемое событием Work Client
            listen_message<TcpClient>(BUS.ADD_CONNECTION_CLIENT)
                .output_to(AddConnectionClient, Header.Events.CLIENT_WORK);

            listen_message<IClientConnect>(BUS.REMOVE_DISCONNECTION_CLIENT)
                .output_to(RemoveDisconnectionClient, Header.Events.CLIENT_WORK);

            listen_echo_2_2<AddNewClient, IClientConnect, AddNewClientResult, ClientData>(BUS.ADD_NEW_CLIENT)
                .output_to(EAddNewClient, Header.Events.MONGO_DB);

            listen_message<ClientData>(BUS.SEND_NEW_CLIENT_TO_ADMINS)
                .output_to(SendClientsToAdmins, Header.Events.CLIENT_WORK);
        }

        void Start()
        {
            SystemInformation("starting ...");
            {
                obj<network.Server>(network.Server.NAME);
            }
        }

        void Configurate()
        {
        }

        public struct BUS
        {
            // Добовляет нового подключенного клинта.
            public const string REMOVE_DISCONNECTION_CLIENT = NAME + ":Delete client";
            public const string ADD_CONNECTION_CLIENT = NAME + ":Add client";

            /// <summary>
            /// Добавляет нового клиента.
            /// </summary>
            public const string ADD_NEW_CLIENT = NAME + ":Add new client";

            /// <summary>
            /// Был добавлен новый клиент, отправим данные всем клинтам
            /// (админам) в том числе админу который его добавил.
            /// </summary>
            public const string SEND_NEW_CLIENT_TO_ADMINS = NAME + "Send new clinet to admins";

            /// <summary>
            /// Получить общую информацию всех клиeнтов(Логин, парль, полное имя, уровент доступа.)
            /// </summary> 
            public const string GET_CLIENTS_DATA = NAME + "Get clinets data";
        }

        public struct DB
        {
            public struct Client
            {
                public const string NAME = "ClientsDB";

                public struct Collection
                {
                    public const string NAME = "ClientsCollection";

                    public struct Key
                    {
                        /// <summary>
                        /// Логин клиента.
                        /// </summary>
                        public const string LOGIN = "Login";

                        /// <summary>
                        /// Пароль клиента.
                        /// </summary> 
                        public const string PASSWORD = "Password";

                        /// <summary>
                        /// Емайл клиента.
                        /// </summary>
                        public const string EMAIL = "Email";

                        /// <summary>
                        /// Полное имя клиента.
                        /// </summary> 
                        public const string FULL_NAME = "FullName";

                        /// <summary>
                        /// Разрешона ли работа машин клиента.
                        /// </summary> 
                        public const string IS_RUNNING = "IsRunning";

                        /// <summary>
                        /// Наименование организации.
                        /// </summary> 
                        public const string ORGANIZATION_NAME = "OrganizationName";

                        /// <summary>
                        /// Уровень доступа.
                        /// </summary> 
                        public const string ACCESS_RIGHTS = "AccessRights";

                        /// <summary>
                        /// Сколько всего машин у клиента.
                        /// </summary> <summary>
                        public const string ASICS_COUNT = "AsicsCount";

                        /// <summary>
                        /// Дата добавления клиeнта.
                        /// </summary> <summary>
                        public const string CREATING_DATE = "AddDate";

                        /// <summary>
                        /// До какой даты работают машины клинта.
                        /// </summary> <summary>
                        public const string WORK_UNTIL_WHAT_DATE = "WorkUntilWhatDate";
                    }
                }
            }
        }

        public interface IClientConnect
        {
            /// <summary>
            /// Отправляет SSL сообщение клиенту.
            /// </summary>
            public void SendMessage(byte[] message);

            /// <summary>
            /// Отправляет SSL сообщение клиенту.
            /// </summary>
            public void SendMessage(string message);

            /// <summary>
            /// Отправляет SSL сообщение клиенту.
            /// </summary>
            public void SendMessage<JsonType>(JsonType json, int type);

            /// <summary>
            /// Ключ по которому создается обьект в нутри библиотеки.
            /// </summary>
            public string GetKey();

            /// <summary>
            /// Является ли клиент админом?
            /// </summary>
            public bool IsAdmin();

            /// <summary>
            /// Возращает уникальный идентификатор клиeнта.
            /// </summary>
            public string GetClientLogin();

            /// <summary>
            /// Возращает полное имя клиента.
            /// </summary>
            public string GetFullNameClient();

            /// <summary>
            /// Возращает наименование организации.
            /// </summary>
            public string GetOrganizationName();

            /// <summary>
            /// Возращает логин клиента.
            /// </summary>
            public string GetLogin();

            /// <summary>
            /// Возращает пароль клиeнта.
            /// </summary>
            public string GetPassword();

            /// <summary>
            /// Возращает email клиeнта.
            /// </summary>
            public string GetEmail();
        }
    }
}