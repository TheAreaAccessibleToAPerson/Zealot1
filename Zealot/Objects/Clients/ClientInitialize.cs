using System.Text.Json;

namespace Zealot
{
    public class LoginAndPassword
    {
        public string login { set; get; }
        public string password { set; get; }
    }

    public class ClientTCPPort
    {
        public int port { set; get; }
    }

    public class ClientInitialize
    {
        public string ID { set; get; } = "";
        public string Name { set; get; } = "";
        public string Email { set; get; } = "";

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

                if (ID == "")
                {
                    Error += "В обьекте ClientInitialize не проинициализировано св-во ID. ";
                    result = false;
                }
                if (Name == "")
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
                if (Asics == null)
                {
                    Error += "В обьекте ClientInitialize не проинициализировано св-во Asics. ";
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
            info = $"Клиент ID:[{ID}], Name:[{Name}], Email:[{Email}], AccessRights[{AccessRights}], ";

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
        public PoolInformation Pool { set; get; }

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
            public string MAC { set; get; } = "";
            // Мак указаный при отправки.
            public string MAC2 { set; get; } = "";
            // Мак указаный на наклейки.
            public string MAC3 { set; get; } = "";
        }

        public class ClientInformation
        {
            // ID Клиента.
            public string ID { set; get; } = "";
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
            public const string IS_RUNNING = "Is running";
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
        }

    }
}