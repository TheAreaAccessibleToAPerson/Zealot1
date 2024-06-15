namespace Butterfly.system.objects.main
{
    public struct SubscribePollTicket
    {
        /// <summary>
        /// Настройка для пулла. Имя.
        /// </summary>
        public string Name {get;init;}

        /// <summary>
        /// Ссылка на Action которую будет обрабатывать пулл.
        /// </summary>
        public System.Action Action {get;init;}

        /// <summary>
        /// TimeDelay для события.
        /// </summary>
        public uint TimeDelay{ get; init;}

        /// <summary>
        /// Дириктория обьекта.
        /// </summary>
        public string Directory { get; init;}

        /// <summary>
        /// Ссылка на метод в PollManager с помощью которого пулл будет общатся со своим подписоным обьектом. 
        /// 1) Тип информирования.
        /// 2) Номер индекса в массиве SubsribePollManager.
        /// 3) ID пулла.
        /// 4) Номер индекса в массиве RootPollClients.
        /// </summary>
        public System.Action<root.poll.InformingType, int, ulong, int> Informing {get;init;}

        /// <summary>
        /// ID обьекта(узла) который регистрирует свои подписки и своих веток в пулл.
        /// </summary>
        public ulong IDObject {get;init;}

        /// <summary>
        /// Индекс в массиве где хранится информация о регистрации в PollManager.  
        /// </summary>
        public int IndexInSubscribePollManager {get;init;}
    }

    public struct UnsubscribePollTicket
    {
        /// <summary>
        /// Имя пулла. 
        /// </summary>
        /// <value></value>
        public string Name {get;init;}
        /// <summary>
        /// ID пулла на который мы подписаны. 
        /// </summary>
        public ulong PollID {get;init;}

        /// <summary>
        /// ID обьекта(узла) который регистрирует свои отписик и своих веток в пулл.
        /// </summary>
        public ulong IDObject {get;init;}

        /// <summary>
        /// Индекс в пулле на который мы подписались. 
        /// </summary>
        public int IndexInRootPoll {get;init;}

        /// <summary>
        /// Индекс в массиве subsribePollMangaer где хранится 
        /// вся информация о пуле в котором мы работаем.
        /// </summary>
        public int IndexInSubscribePollManager {get;init;}
    }

    public struct StateType
    {
        /// <summary>
        /// Создание билетов.
        /// </summary>
        public const byte CREATING_TICKET = 0;

        /// <summary>
        /// Регистрируем подписки.
        /// </summary>
        public const byte REGISTER_SUBSCRIBE = 1;

        /// <summary>
        /// Все подписки зарегистрировались.
        /// </summary>
        public const byte END_SUBSCRIBE = 2;

        /// <summary>
        /// Регистрируем все отписки.
        /// </summary>
        public const byte REGISTER_UNSUBSCRIBE = 3;

        /// <summary>
        /// Мы отписались ото всех отписок.
        /// </summary>
        public const byte END_UNSUBSCRIBE = 4;
    }
}