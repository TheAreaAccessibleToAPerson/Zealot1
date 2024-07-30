using Zealot.manager;

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

        public AsicInitialize[] Asics { set; get; }

        public void II()
        {
            Asics = new AsicInitialize[1];

            AsicInitialize j = new AsicInitialize()
            {
                UniqueNumber = "JJJJJ",

                MAC = new AsicInitialize.MACInformation()
                {
                    MAC = "MAC_1",
                    MAC2 = "MAC_2",
                    MAC3 = "MAC_3"
                },

                SN = new AsicInitialize.SNInformation()
                {
                    SN1 = "SN_1",
                    SN2 = "SN_2",
                    SN3 = "SN_3",
                },

                Location = new AsicInitialize.LocationInformation()
                {
                    Name = "ЦЕХ",
                    StandNumber = "1-3",
                    SlotIndex = "1",
                },

                Client = new AsicInitialize.ClientInformation()
                {
                },
            };

            Asics[0] = j;
        }
    }

    public class AsicInitialize
    {
        // Уникальный номер в нутри компании.
        public string UniqueNumber { set; get; } = "";

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
            // Сколько клиент платит на киловат.
            public string OwnerPriceKilowatt { set; get; } = "";

            public string OwnerName { set; get; } = "";
            public string OwnerFamily { set; get; } = "";
            public string OwnerSurname { set; get; } = "";
            public string OwnerEmail { set; get; } = "";
            public string OwnerTelephon { set; get; } = "";
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
    }
}