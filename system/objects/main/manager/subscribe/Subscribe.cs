namespace Butterfly.system.objects.main.manager
{
    namespace description
    {
        /// <summary>
        /// Описывает метод который будет вызываться в менеджерах подписак
        /// после того как придет ответ о подписании в нужное место.
        /// /// </summary>
        public interface ISubscribe
        {
            /// <summary>
            /// Сообщим менеджеру отвечаещего за все подписки о том что нас подписали.
            /// в нужные места.
            /// /// </summary>
            public void EndSubscribe();

            /// <summary>
            /// Сообщим менеджеру отвечающего за все подписки о том что на описали. 
            /// </summary>
            public void EndUnsubscribe();
        }
    }

    public class Subscribe : Informing, dispatcher.ISubscribe, description.ISubscribe
    {
        public struct Type
        {
            public const byte POLL = 0;
        }

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        private readonly controller.Poll _pollController;

        private readonly manager.IDispatcher _dispatcherManager;

        private readonly object _locker = new object();

        /// <summary>
        /// Когда нас подпишут, нам об этом сообщат, и мы инкременириует данное значение.
        /// Если оно станет равно количесву регистраци, то мы сообщим диспетчеру.
        /// </summary>
        private uint SubscribeCount = 0;

        public Subscribe(informing.IMain mainInforming, information.State stateInformation,
            information.Header headerInformation, information.DOM DOMInformation,
                manager.Dispatcher dispatcherManager)
            : base("SubscribeManager", mainInforming)
        {
            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;
            _dispatcherManager = dispatcherManager;

            _pollController = new controller.Poll
                (mainInforming, stateInformation, headerInformation,
                    DOMInformation, this);
        }

        /// <summary>
        /// Добавляется данные для регистрации в пулл потоков.
        /// </summary>
        /// <param name="name">Имя пулла на который нужно подписаться.</param>
        /// <param name="action">Action который мы выставляем в пулл потоков для обработки.</param>
        public main.Object Add(string name, System.Action action, byte type, uint timeDelay)
        {
            lock (_locker)
            {
                if (_stateInformation.IsContruction)
                {
                    if (Type.POLL == type)
                        _pollController.Add(name, action, type, timeDelay);
                }
                else
                    Exception($"Вы пытатесь добавить событие {name} не в методe Contruction().");
            }

            return _DOMInformation.CurrentObject;
        }

        /// <summary>
        /// Подписывает метод описаный в IUpdate на события.
        /// </summary>
        /// <param name="name">Имя пулла на который нужно подписаться.</param>
        /// <param name="action">Action который мы выставляем в пулл потоков для обработки.</param>
        public GlobalObjectType Add<GlobalObjectType>(string name, GlobalObjectType globalObject, 
            byte type, uint timeDelay)
                where GlobalObjectType : IUpdate
        {
            lock (_locker)
            {
                if (_stateInformation.IsContruction)
                {
                    if (Type.POLL == type)
                    {
                        _pollController.Add(name, globalObject.Update, type, timeDelay);
                    }
                }
                else
                    Exception($"Вы пытатесь добавить событие {name} не в методe Contruction().");
            }

            return globalObject;
        }

        void dispatcher.ISubscribe.StartSubscribe()
        {
            lock (_locker)
            {
                // Проверяем умеются ли у нас регистрационые билеты.
                if (_pollController.IsRegisterTicket)
                {
                    // Данный метод сначало выставит флаг о том что мы начали подписку.
                    // После отправит билет. Данный метод должен вызываться синхронно.
                    _pollController.Subscribe();
                }
                // Если регистрационых билетов нету, то продолжим создание обьекта.
                else _dispatcherManager.Process(manager.Dispatcher.Command.START_OBJECT);
            }
        }

        void dispatcher.ISubscribe.StartUnsubscribe()
        {
            lock (_locker)
            {
                //Console("UNSUBSCRIBE");
                // Если данный обьект отправлял регистрационые билеты, и ему пришол 
                // ответ о том что регистрация окончилась, то мы отправим запрос на отписку.
                if (_pollController.IsRegisterTicket && _pollController.IsSubscribe)
                {
                    _pollController.Unsubscribe();
                }
                // Если у нас имеются регистрационые билеты, но мы не подписывались и 
                // не ожидаем пока регистрация окончена, значит продолжаем остановку обьекта.
                else if (_pollController.IsRegisterTicket && _pollController.IsSubscribe == false
                    && _pollController.IsRegisterSubscribe == false)
                {
                    _dispatcherManager.Process(manager.Dispatcher.Command.CONTINUE_STOPPING);
                }
                // Если обьект не имел регистрационых билетов и неначто небыл подписан,
                // то продожим его уничтжение.
                else _dispatcherManager.Process(manager.Dispatcher.Command.CONTINUE_STOPPING);
            }
        }

        void description.ISubscribe.EndSubscribe()
        {
            //lock (_locker)
            {
                // Нас оповестили что подписка окончена продолжим создание обьекта.
                _DOMInformation.RootManager.ActionInvoke(() 
                    => _dispatcherManager.Process(manager.Dispatcher.Command.START_OBJECT));
            }
        }

        void description.ISubscribe.EndUnsubscribe()
        {
            //lock (_locker)
            {
                // Нас оповетили что отписка закончена, продолжит уничтожение обьекта.
                _DOMInformation.RootManager.ActionInvoke(()
                    => _dispatcherManager.Process(manager.Dispatcher.Command.CONTINUE_STOPPING));
            }
        }
    }
}