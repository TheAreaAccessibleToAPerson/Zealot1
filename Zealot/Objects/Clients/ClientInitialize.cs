using System.Text.Json;
using Zealot.manager;

namespace Zealot
{
    public class LoginAndPasswordJson
    {
        public string login { set; get; }
        public string password { set; get; }
    }

    public class ClientTCPPortJson
    {
        public int Port { set; get; }
    }

    public class ClientJson
    {
    }

    public class ClientInitialize
    {
        public string FullName { set; get; } = "";
        public string Email { set; get; } = "";
        public string Login { set; get; } = "";
        public string Password { set; get; } = "";
        public string OrganizationName { set; get; } = "";

        /// <summary>
        /// Кол-во машин у клиента.
        /// </summary>
        public int AsicsCount {set;get;} = 0;

        /// <summary>
        /// Дата создания клиента.
        /// </summary>
        public string CreatingDate {set;get;} = "";

        /// <summary>
        /// До какого числа работает данный клиент.
        /// </summary>
        public string WorkUntilWhatDate {set;get;} = "";

        /// <summary>
        /// Разрешина ли работа машин у клиента.
        /// </summary>
        public bool IsRunning {set;get;} = false;

        // 0 - полный доступ.(root)
        // 1 - дежурный(не доступна полная информация о клинтах).
        // 2 - аператор(не доступен перенос машин).
        // 4 - клиент.
        public string AccessRights { set; get; } = "";

        public bool IsInitialize
        {
            get
            {
                bool result = true;

                if (FullName == "")
                {
                    Error += "В обьекте ClientInitialize не проинициализировано св-во Name. ";
                    result = false;
                }
                if (Email == "")
                {
                    Error += "В обьекте ClientInitialize не проинициализировано св-во Email. ";
                    result = false;
                }
                if (AccessRights == "")
                {
                    Error += "В обьекте ClientInitialize не проинициализировано св-во AccessRights. ";
                    result = false;
                }

                return result;
            }
        }
        public string Error { set; get; } = "";

        public string AsicsJSON
        {
            set
            {
                if (Asics != null)
                {
                    Error = "Попытка повторно проинициализировать св-во Asics в обьекте ClientInitialize.";
                }
                else
                {
                    try
                    {
                        Asics = JsonSerializer.Deserialize<AsicInit[]>(value);
                    }
                    catch (Exception ex)
                    {
                        Error = ex.ToString();
                    }
                }
            }
        }

        public AsicInit[] Asics { private set; get; } = null;

        public bool SetAsics(List<AsicInit> asics, out string info)
        {
            info = $"Name:[{FullName}], Email:[{Email}], AccessRights[{AccessRights}], ";

            if (Asics == null)
            {
                info += $" получил информацию о своих {asics.Count} машинах.";

                Asics = asics.ToArray();

                return true;
            }
            else
            {
                info += $"не смог получить информацию о своих {asics.Count} машинах, так как эта информация была получена ранее.";

                return false;
            }
        }
    }

    public class AsicInit
    {
        // Уникальный номер в нутри компании.
        public string UniqueNumber { set; get; } = "";

        // Разрешение на работу.
        public bool IsRunning { set; get; } = false;

        public MACInformation MAC { set; get; }
        public SNInformation SN { set; get; }
        public LocationInformation Location { set; get; }
        public ClientInformation Client { set; get; }
        public ModelInformation Model { set; get; }
        public PoolInformation Pool { set; get; }
        public CompanyInformation Company { set; get; }

        /// <summary>
        /// 
        /// </summary>
        private Devices.IClientConnect[] _clientsConnect;
        private int _clientConnectCount = 0;

        /// <summary>
        /// Отправить сообщение клиенту.
        /// Должно выполнятся в потоке который обрабатывает работу девайсов.
        /// </summary>
        public void SendToMessage(byte[] message)
        {
            for (int i = 0; i < _clientConnectCount; i++)
            {
                _clientsConnect[i].SendMessage(message);
            }
        }

        /// <summary>
        /// Отправить сообщение клиенту.
        /// Должно выполнятся в потоке который обрабатывает работу девайсов.
        /// </summary>
        public void SendToMessage(string message)
        {
            for (int i = 0; i < _clientConnectCount; i++)
            {
                _clientsConnect[i].SendMessage(message);
            }
        }

        /// <summary>
        /// Данный метод рассылает всем клиентам данные о машинках по tcp соединению.
        /// Скорость куллера и тд.
        /// </summary>
        public void SendDataMessage(byte[] jsonUtf8Bytes)
        {
            byte[] m = ServerMessage.GetMessageArray(ServerMessage.TCPType.ASIC_DATA, jsonUtf8Bytes);
            for (int i = 0; i < _clientConnectCount; i++)
                _clientsConnect[i].SendMessage(m);
        }

        public void ClientSubscribeToReceiveMessage(Devices.IClientConnect client)
        {
            if (_clientsConnect == null)
                _clientsConnect = new Devices.IClientConnect[1];
            else
            {
                Devices.IClientConnect[] buffer = new Devices.IClientConnect[_clientConnectCount + 1];
                for (int i = 0; i < _clientConnectCount; i++)
                    buffer[i] = _clientsConnect[i];

                _clientsConnect = buffer;
            }

            _clientsConnect[_clientConnectCount++] = client;
        }

        public void ClientUnsubscribeToReceiveMessage(Devices.IClientConnect client)
        {
            if (_clientsConnect != null)
            {
                for (int i = 0; i < _clientConnectCount; i++)
                {
                    if (client.GetKey() == _clientsConnect[i].GetKey())
                    {
                        Devices.IClientConnect[] buffer = new Devices.IClientConnect[_clientConnectCount - 1];
                        {
                            for (int u = 0; u < _clientConnectCount; u++)
                            {
                                int index = 0;
                                if (u != i)
                                {
                                    buffer[index] = _clientsConnect[index];
                                    ++index;
                                }
                            }
                        }
                        _clientsConnect = buffer;

                        return;
                    }
                }
            }
        }

        public class DateTimeInformation
        {
            /// <summary>
            /// Дата добавление асика.
            /// </summary>
            public string AddDateTime { set; get; }
        }
        public class CompanyInformation
        {
            // Имя в нутри системы.
            public string Name1 { set; get; } = "";
            // Имя указаное при отправки
            public string Name2 { set; get; } = "";
            // Имя по факту.
            public string Name3 { set; get; } = "";

            // Заявленая мощность модели.
            public string Power { set; get; } = "";
        }

        public class ModelInformation
        {
            // Имя в нутри системы.
            public string Name1 { set; get; } = "";
            // Имя указаное при отправки
            public string Name2 { set; get; } = "";
            // Имя по факту.
            public string Name3 { set; get; } = "";

            // Заявленая мощность модели.
            public string Power { set; get; } = "";
        }

        public class PoolInformation
        {
            public string Addr1 { set; get; } = "";
            public string Name1 { set; get; } = "";
            public string Password1 { set; get; } = "";

            public string Addr2 { set; get; } = "";
            public string Name2 { set; get; } = "";
            public string Password2 { set; get; } = "";

            public string Addr3 { set; get; } = "";
            public string Name3 { set; get; } = "";
            public string Password3 { set; get; } = "";
        }

        public class MACInformation
        {
            // Программный мак.
            public string MAC1 { set; get; } = "";
            // Мак указаный при отправки.
            public string MAC2 { set; get; } = "";
            // Мак указаный на наклейки.
            public string MAC3 { set; get; } = "";
        }

        public class ClientInformation
        {
            // ID Клиента.
            public string ID { set; get; } = "";
            public string NAME { set; get; } = "";
        }

        public class LocationInformation
        {
            // Имя локации (цех, контейнер).
            public string Name { set; get; } = "";
            // Номер стойки.
            public string StandNumber { set; get; } = "";
            // Индекс позции на стройки.
            public string SlotIndex { set; get; } = "";
        }

        public class SNInformation
        {
            // Программный сериный номер.
            public string SN1 { set; get; } = "";
            // Серийный номер указаный при отправки.
            public string SN2 { set; get; } = "";
            // Серийный номер на наклейки.
            public string SN3 { set; get; } = "";
        }

        public string ToJson() => JsonSerializer.Serialize(this);

        public struct _
        {
            public const string UNIQUE_NUMBER = "UniqueNumber";
            public const string CLIENT_ID = "ClientID";
            public const string CLIENT_NAME = "ClientName";
            public const string IS_RUNNING = "Is running";
            public const string COMPANY_NAME1 = "CompanyName1";
            public const string COMPANY_NAME2 = "CompanyName1";
            public const string COMPANY_NAME3 = "CompanyName1";
            public const string MODEL_NAME1 = "ModelName1";
            public const string MODEL_NAME2 = "ModelName2";
            public const string MODEL_NAME3 = "ModelName3";
            public const string POWER = "Power";
            public const string SN1 = "SN1";
            public const string SN2 = "SN2";
            public const string SN3 = "SN3";
            public const string MAC1 = "MAC1";
            public const string MAC2 = "MAC2";
            public const string MAC3 = "MAC3";
            public const string LOCATION_NAME = "Location name";
            public const string LOCATION_STAND_NUMBER = "Location stand number";
            public const string LOCATION_SLOT_INDEX = "Location slot index";
            public const string POOL_ADDR_1 = "Pool addr 1";
            public const string POOL_NAME_1 = "Pool name 1";
            public const string POOL_PASSWORD_1 = "Pool password 1";
            public const string POOL_ADDR_2 = "Pool addr 2";
            public const string POOL_NAME_2 = "Pool name 2";
            public const string POOL_PASSWORD_2 = "Pool password 2";
            public const string POOL_ADDR_3 = "Pool addr 3";
            public const string POOL_NAME_3 = "Pool name 3";
            public const string POOL_PASSWORD_3 = "Pool password 3";
            public const string ADD_ASIC_DATE_TIME = "Add asic dateTime";
        }

    }
}