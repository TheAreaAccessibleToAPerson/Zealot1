namespace Butterfly.system.objects.main
{
    /// <summary>
    /// Главный обьект.
    /// </summary>
    public abstract class Object : informing.IMain, manager.INodeObjects, manager.IBranchObjects,
        description.IDOM, description.IPoll, manager.IDispatcher, manager.ILifeCyrcle,
            IInformation, IController
    {
        public readonly information.State StateInformation;

        private readonly information.State.Manager _stateInformationManager;
        private readonly information.Header _headerInformation;
        private readonly information.Tegs _tegsInformation;

        protected void add_teg(string teg) => _tegsInformation.Add(teg);
        public bool contains_teg(string teg) => _tegsInformation.Contains(teg);

        protected void add_teg(byte teg) => _tegsInformation.Add(teg);
        public bool contains_teg(byte teg) => _tegsInformation.Contains(teg);

        public Object(byte type)
        {
            StateInformation = new information.State();
            _stateInformationManager = new information.State.Manager(this, StateInformation);

            _headerInformation = new information.Header(this, type);
            _tegsInformation = new information.Tegs(StateInformation, this);
        }

        #region LifeCyrcleManager
        private manager.LifeCyrcle _lifeCyrcleManager;

        public void destroy()
            => _lifeCyrcleManager.Destroy();

        void manager.ILifeCyrcle.ContinueDestroy()
            => _lifeCyrcleManager.ContinueDestroy();

        #endregion

        #region Dispatcher

        private manager.Dispatcher _dispatcherManager;

        void manager.IDispatcher.Process(byte command)
            => _dispatcherManager.Process(command);

        #endregion

        #region DOM

        private information.DOM _DOMInformation;

        main.Object description.IDOM.GetParent()
            => _DOMInformation.ParentObject;

        void description.IDOM.CreatingNode()
            => _dispatcherManager.Start();

        bool description.IDOM.IsBoard()
            => _headerInformation.IsBoard();

        string description.IDOM.AddControlToGlobalObject(string name)
            => _globalObjectsManager.AddControlToGlobalObject(name);

        void description.IDOM.BranchDefine(string keyObject, ulong nodeID, ulong nestingNodeNamberInTheSystem,
            ulong nestingNodeNamberInTheNode, ulong[] parentObjectsID, main.Object parentObject,
                main.Object nodeObject, main.Object nearIndependentNodeObject, root.IManager rootManager,
                    System.Collections.Generic.Dictionary<string, object> globalObjects)
        {
            _headerInformation.BranchDefine
                (parentObject._headerInformation.Directory, GetType(),
                    parentObject._DOMInformation, keyObject);

            _DOMInformation = new information.DOM
                (keyObject, nodeID, nestingNodeNamberInTheSystem, nestingNodeNamberInTheNode,
                    parentObjectsID, this, parentObject, nodeObject, nearIndependentNodeObject, rootManager);

            Define(globalObjects);
        }

        void description.IDOM.NodeDefine(string keyObject, ulong nestingNodeNamberInTheSystem,
            ulong[] parentObjectsID, main.Object parentObject, main.Object nearIndependentNodeObject,
                root.IManager rootManager, System.Collections.Generic.Dictionary<string, object> globalObjects)
        {
            _headerInformation.NodeDefine
                (parentObject._headerInformation.Directory, GetType(),
                    parentObject._DOMInformation, keyObject);

            _DOMInformation = new information.DOM
                (keyObject, nestingNodeNamberInTheSystem, parentObjectsID, this, parentObject,
                    _headerInformation.IsBoard() ? this : nearIndependentNodeObject, rootManager);

            Define(globalObjects);
        }

        private void Define(System.Collections.Generic.Dictionary<string, object> globalObjects)
        {
            _dispatcherManager = new manager.Dispatcher(this, _headerInformation, _tegsInformation);
            {
                _globalObjectsManager = new manager.GlobalObjects
                    (this, globalObjects, _headerInformation, StateInformation, _DOMInformation);

                _branchObjectsManager = new manager.BranchObjects
                    (this, _headerInformation, StateInformation, _DOMInformation, globalObjects);

                _nodeObjectsManager = new manager.NodeObjects
                    (this, _headerInformation, StateInformation, _DOMInformation, globalObjects);

                _threadsManager = new manager.Threads
                    (this, StateInformation);

                _subscribeManager = new manager.Subscribe
                    (this, StateInformation, _headerInformation, _DOMInformation, _dispatcherManager);

                _lifeCyrcleManager = new manager.LifeCyrcle
                    (this, _headerInformation, StateInformation, _stateInformationManager, _DOMInformation,
                        _tegsInformation, _branchObjectsManager, _nodeObjectsManager, _dispatcherManager);
            }
            _dispatcherManager.Initialize
                (_globalObjectsManager, _lifeCyrcleManager, _subscribeManager,
                    _threadsManager);
        }

        #endregion

        #region GlobalObjectsManager

        private manager.GlobalObjects _globalObjectsManager;

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящий импульс от дочернего обьекта. 
        /// В качесве сообщения придет инфомация об обьекте которые его отправил.
        /// </summary>
        /// <param name="name">Имя по которому можно будет установить связь.</param>
        /// <returns></returns>
        protected IRedirect<IImpulsInformation> listen_impuls(string name)
            => _globalObjectsManager.Add<ListenImpuls, IRedirect<IImpulsInformation>>
                (name, new ListenImpuls(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушавает поступающие события от дочерних обьектов.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты будут делигировать выполнение событий.</param>
        /// <param name="eventName">Имя пулла события который будет обрабатывать поступающие события.</param>
        protected void listen_events(string name, string eventName)
            => _subscribeManager.Add<HandlerEvents>(eventName, _globalObjectsManager.Add
                (name, new HandlerEvents(this)), manager.Subscribe.Type.POLL, 0);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушавает поступающие события от дочерних обьектов.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты будут делигировать выполнение событий.</param>
        /// <param name="eventName">Имя пулла события который будет обрабатывать поступающие события.</param>
        /// <param name="timeDelay">Временой промежуток для пулла события.</param>
        protected void listen_events(string name, string eventName, uint timeDelay)
            => _subscribeManager.Add<HandlerEvents>(eventName, _globalObjectsManager.Add
                (name, new HandlerEvents(this)), manager.Subscribe.Type.POLL, timeDelay);


        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Отправляет импульс родительскому обьекту по имени. В качесве сообщения
        /// будет доставлена общая информация о текущем обьектe.
        /// </summary>
        /// <param name="input">Способ вызова импульса.</param>
        /// <param name="name">Имя глобального обьекта который прослушивает данный ипульс.</param>
        protected void send_impuls(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenImpuls, SendImpuls, IInput>
                (name, ref input, new SendImpuls(this));

        /// <summary>
        /// Делегирует родительскому обьекту в котором создан listen_events, выполнения события.
        /// </summary>
        /// <param name="action">Событие</param>
        /// <param name="timeDelay">Через сколько нужно произвести вызов.</param>
        /// <param name="name">Имя по которому создан listen_events в родительском обьектe.</param>
        protected void invoke_event(System.Action action, uint timeDelay, string name)
            => _globalObjectsManager.Get<HandlerEvents>(name).To(action, timeDelay);

        /// <summary>
        /// Делегирует родительскому обьекту в котором создан listen_events, выполнения события.
        /// </summary>
        /// <param name="action">Событие</param>
        /// <param name="name">Имя по которому создан listen_events в родительском обьектe.</param>
        protected void invoke_event(System.Action action, string name)
            => _globalObjectsManager.Get<HandlerEvents>(name).To(action);

        /// <summary>
        /// Получаем ссылку на listen_events созданый в родительском обьекте с помощью которой 
        /// мы сможем производить вызов событий.
        /// </summary>
        /// <param name="input">Ссылка на listen_events</param>
        /// <param name="name">Имя по которому создан listen_events в родительском обьектe.</param>
        protected void invoke_event(ref IInput<System.Action> input, string name)
            => _globalObjectsManager.Get<HandlerEvents, IInput<System.Action>>(name, ref input);

        /// <summary>
        /// Получаем ссылку на listen_events созданый в родительском обьекте с помощью которой 
        /// мы сможем производить отложеный вызов событий.
        /// </summary>
        /// <param name="input">Ссылка на listen_events</param>
        /// <param name="name">Имя по которому создан listen_events в родительском обьектe.</param>
        protected void invoke_event(ref IInput<System.Action, uint> input, string name)
            => _globalObjectsManager.Get<HandlerEvents, IInput<System.Action, uint>>(name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Безопасный прием входящих сообщений от дочерних обьектов. 
        /// При пересоздании обьекта он заново получит доступ к данному случателю.
        /// На выход c помощью .output_to будет получено сообщение, и метод вызвав который вы подтвертиде что сообщение было принято.
        /// В parentName нужно задать имя родительского обьекта который с помощью метода extract_values(string, System.Action)
        /// получит в указаный метод массив сообщений за прием которых вы не отчитались.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <param name="eventName">Имя события на которое нужно подписаться.</param>
        /// <param name="parentName">Имя родительского обьекта которые с помощью метода extract_values извлечет не обработаные данные.</param>
        /// <typeparam name="T">Тип получаемых сообщений.</typeparam>
        /// <returns></returns>
        protected IRedirect<T, System.Action> safe_listen_message<T>(string name, string eventName, string parentName)
            => _subscribeManager.Add<SafeListenMessagePoll<T>>(eventName, _globalObjectsManager.Add
                (((description.IDOM)(_DOMInformation).GetParent(parentName)).AddControlToGlobalObject(name),
                    new SafeListenMessagePoll<T>(this)), manager.Subscribe.Type.POLL, 0);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Безопасный прием входящих сообщений от дочерних обьектов. 
        /// При пересоздании обьекта он заново получит доступ к данному случателю.
        /// На выход c помощью .output_to будет получено сообщение, и метод вызвав который вы подтвертиде что сообщение было принято.
        /// В parentName нужно задать имя родительского обьекта который с помощью метода extract_values(string, System.Action)
        /// получит в указаный метод массив сообщений за прием которых вы не отчитались.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <param name="eventName">Имя события на которое нужно подписаться.</param>
        /// <param name="parentName">Имя родительского обьекта которые с помощью метода extract_values извлечет не обработаные данные.</param>
        /// <param name="timeDelay">TimeDelay для события.</param>
        /// <typeparam name="T">Тип получаемых сообщений.</typeparam>
        /// <returns></returns>
        protected IRedirect<T, System.Action> safe_listen_message<T>(string name, string eventName, string parentName, uint timeDelay)
            => _subscribeManager.Add<SafeListenMessagePoll<T>>(eventName, _globalObjectsManager.Add
                (((description.IDOM)(_DOMInformation).GetParent(parentName)).AddControlToGlobalObject(name),
                    new SafeListenMessagePoll<T>(this)), manager.Subscribe.Type.POLL, timeDelay);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Отправит сообщение в safe_listen_message который определен в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки данных.</param>
        /// <param name="name">Имя safe_listen_message в который будут отправляться сообщения.</param>
        /// <typeparam name="T"></typeparam>
        protected void safe_send_message<T>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<SafeListenMessagePoll<T>, IInput<T>>
                (name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Извлекает необработаные сообщения из safe_listen_message в метод.
        /// </summary>
        /// <param name="name">Имя safe_listen_message созданого в дочернем обьекте из которого нужно получить не обработаные сообщения.</param>
        /// <param name="safe">Метод в который нужно вывести массив необработаных сообщений.</param>
        /// <typeparam name="R"></typeparam>
        protected void extract_values<R>(string name, System.Action<R[]> safe)
             => _globalObjectsManager.Extract<SafeListenMessagePoll<R>, R>(name, safe);

        protected IRedirect<IReturn> listen_echo(string name)
            => _globalObjectsManager.Add<ListenEcho_0_0, IRedirect<IReturn>>
                (name, new ListenEcho_0_0(this));

        protected IRedirect<T, IReturn> listen_echo_1_0<T>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_0<T>, IRedirect<T, IReturn>>
                (name, new ListenEcho_1_0<T>(this));

        protected IRedirect<T1, T2, IReturn> listen_echo_2_0<T1, T2>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_0<T1, T2>, IRedirect<T1, T2, IReturn>>
                (name, new ListenEcho_2_0<T1, T2>(this));

        protected IRedirect<T1, T2, T3, IReturn> listen_echo_3_0<T1, T2, T3>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_0<T1, T2, T3>, IRedirect<T1, T2, T3, IReturn>>
                (name, new ListenEcho_3_0<T1, T2, T3>(this));

        protected IRedirect<T1, T2, T3, T4, IReturn> listen_echo_4_0<T1, T2, T3, T4>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_0<T1, T2, T3, T4>, IRedirect<T1, T2, T3, T4, IReturn>>
                (name, new ListenEcho_4_0<T1, T2, T3, T4>(this));

        protected IRedirect<T1, T2, T3, T4, T5, IReturn> listen_echo_5_0<T1, T2, T3, T4, T5>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_0<T1, T2, T3, T4, T5>, IRedirect<T1, T2, T3, T4, T5, IReturn>>
                (name, new ListenEcho_5_0<T1, T2, T3, T4, T5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите способ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<IReturn<R>> listen_echo<R>(string name)
            => _globalObjectsManager.Add<ListenEcho_0_1<R>, IRedirect<IReturn<R>>>
                (name, new ListenEcho_0_1<R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите способ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<IReturn<R1, R2>> listen_echo<R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_0_2<R1, R2>, IRedirect<IReturn<R1, R2>>>
                (name, new ListenEcho_0_2<R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите способ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<IReturn<R1, R2, R3>> listen_echo<R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_0_3<R1, R2, R3>, IRedirect<IReturn<R1, R2, R3>>>
                (name, new ListenEcho_0_3<R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите cпособ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<IReturn<R1, R2, R3, R4>> listen_echo<R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_0_4<R1, R2, R3, R4>, IRedirect<IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_0_4<R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите cпособ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<IReturn<R1, R2, R3, R4, R5>> listen_echo<R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_0_5<R1, R2, R3, R4, R5>, IRedirect<IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_0_5<R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает echo сообщения от дочерних обьектов. 
        /// С помощью .output_to вы получите входящее сообщение от дочернего обьекта и способ обратной связки с ним.
        /// Если вам нужно отправить или получить более одного сообщения, то явно укажите количесво получаемых сообщений
        /// и количесво отправляемых обратно:
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T, IReturn<R>> listen_echo_1_1<T, R>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_1<T, R>, IRedirect<T, IReturn<R>>>
                (name, new ListenEcho_1_1<T, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, IReturn<R>> listen_echo_2_1<T1, T2, R>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_1<T1, T2, R>, IRedirect<T1, T2, IReturn<R>>>
                (name, new ListenEcho_2_1<T1, T2, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, IReturn<R>> listen_echo_3_1<T1, T2, T3, R>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_1<T1, T2, T3, R>, IRedirect<T1, T2, T3, IReturn<R>>>
                (name, new ListenEcho_3_1<T1, T2, T3, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, IReturn<R>> listen_echo_4_1<T1, T2, T3, T4, R>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_1<T1, T2, T3, T4, R>, IRedirect<T1, T2, T3, T4, IReturn<R>>>
                (name, new ListenEcho_4_1<T1, T2, T3, T4, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5, IReturn<R>> listen_echo_5_1<T1, T2, T3, T4, T5, R>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_1<T1, T2, T3, T4, T5, R>, IRedirect<T1, T2, T3, T4, T5, IReturn<R>>>
                (name, new ListenEcho_5_1<T1, T2, T3, T4, T5, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T, IReturn<R1, R2>> listen_echo_1_2<T, R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_2<T, R1, R2>, IRedirect<T, IReturn<R1, R2>>>
                (name, new ListenEcho_1_2<T, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, IReturn<R1, R2>> listen_echo_2_2<T1, T2, R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_2<T1, T2, R1, R2>, IRedirect<T1, T2, IReturn<R1, R2>>>
                (name, new ListenEcho_2_2<T1, T2, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, IReturn<R1, R2>> listen_echo_3_2<T1, T2, T3, R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_2<T1, T2, T3, R1, R2>, IRedirect<T1, T2, T3, IReturn<R1, R2>>>
                (name, new ListenEcho_3_2<T1, T2, T3, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, IReturn<R1, R2>> listen_echo_4_2<T1, T2, T3, T4, R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_2<T1, T2, T3, T4, R1, R2>, IRedirect<T1, T2, T3, T4, IReturn<R1, R2>>>
                (name, new ListenEcho_4_2<T1, T2, T3, T4, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2>> listen_echo_5_2<T1, T2, T3, T4, T5, R1, R2>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_2<T1, T2, T3, T4, T5, R1, R2>, IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2>>>
                (name, new ListenEcho_5_2<T1, T2, T3, T4, T5, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T, IReturn<R1, R2, R3>> listen_echo_1_3<T, R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_3<T, R1, R2, R3>, IRedirect<T, IReturn<R1, R2, R3>>>
                (name, new ListenEcho_1_3<T, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, IReturn<R1, R2, R3>> listen_echo_2_3<T1, T2, R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_3<T1, T2, R1, R2, R3>, IRedirect<T1, T2, IReturn<R1, R2, R3>>>
                (name, new ListenEcho_2_3<T1, T2, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, IReturn<R1, R2, R3>> listen_echo_3_3<T1, T2, T3, R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_3<T1, T2, T3, R1, R2, R3>, IRedirect<T1, T2, T3, IReturn<R1, R2, R3>>>
                (name, new ListenEcho_3_3<T1, T2, T3, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3>> listen_echo_4_3<T1, T2, T3, T4, R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_3<T1, T2, T3, T4, R1, R2, R3>, IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3>>>
                (name, new ListenEcho_4_3<T1, T2, T3, T4, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> listen_echo_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>>>
                (name, new ListenEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T, IReturn<R1, R2, R3, R4>> listen_echo_1_4<T, R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_4<T, R1, R2, R3, R4>, IRedirect<T, IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_1_4<T, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, IReturn<R1, R2, R3, R4>> listen_echo_2_4<T1, T2, R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_4<T1, T2, R1, R2, R3, R4>, IRedirect<T1, T2, IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_2_4<T1, T2, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, IReturn<R1, R2, R3, R4>> listen_echo_3_4<T1, T2, T3, R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_4<T1, T2, T3, R1, R2, R3, R4>, IRedirect<T1, T2, T3, IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_3_4<T1, T2, T3, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> listen_echo_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>, IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> listen_echo_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>>>
                (name, new ListenEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T, IReturn<R1, R2, R3, R4, R5>> listen_echo_1_5<T, R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_1_5<T, R1, R2, R3, R4, R5>, IRedirect<T, IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_1_5<T, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, IReturn<R1, R2, R3, R4, R5>> listen_echo_2_5<T1, T2, R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_2_5<T1, T2, R1, R2, R3, R4, R5>, IRedirect<T1, T2, IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_2_5<T1, T2, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> listen_echo_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>, IRedirect<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> listen_echo_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>, IRedirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [listen_echo]_[количесво получаемых сообщений]_[количесво отправляемых обратно сообщений](string)
        /// Прослушивает echo сообщения от дочерних обьектов.
        /// С помощью .output_to вы получите входящее сообщени[е/я] от дочернего обьекта и способ обратной связки с ним.
        /// Данный способ связи не гарантирует что к моменту отправки сообщения дочернему обьекту он все еще будет активен.
        /// </summary>
        /// <param name="name">Имя по которому дочерние обьекты могут получить доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> listen_echo_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(string name)
            => _globalObjectsManager.Add<ListenEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IRedirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>>>
                (name, new ListenEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(this));

        protected IRedirect send_echo(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_0, SendEcho_0_0, IInput, IRedirect>
                    (name, ref input, new SendEcho_0_0(this));

        protected IRedirect send_echo_1_0<T>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_0<T>, SendEcho_1_0<T>, IInput<T>, IRedirect>
                    (name, ref input, new SendEcho_1_0<T>(this));

        protected IRedirect send_echo_2_0<T1, T2>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_0<T1, T2>, SendEcho_2_0<T1, T2>, IInput<T1, T2>, IRedirect>
                    (name, ref input, new SendEcho_2_0<T1, T2>(this));

        protected IRedirect send_echo_3_0<T1, T2, T3>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_0<T1, T2, T3>, SendEcho_3_0<T1, T2, T3>, IInput<T1, T2, T3>, IRedirect>
                    (name, ref input, new SendEcho_3_0<T1, T2, T3>(this));

        protected IRedirect send_echo_4_0<T1, T2, T3, T4>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_0<T1, T2, T3, T4>, SendEcho_4_0<T1, T2, T3, T4>, IInput<T1, T2, T3, T4>, IRedirect>
                    (name, ref input, new SendEcho_4_0<T1, T2, T3, T4>(this));

        protected IRedirect send_echo_5_0<T1, T2, T3, T4, T5>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_0<T1, T2, T3, T4, T5>, SendEcho_5_0<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>, IRedirect>
                    (name, ref input, new SendEcho_5_0<T1, T2, T3, T4, T5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ echo оповещения.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo<R>(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_1<R>, SendEcho_0_1<R>, IInput, IRedirect<R>>
                    (name, ref input, new SendEcho_0_1<R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ echo оповещения.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo<R1, R2, R3, R4, R5>(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_5<R1, R2, R3, R4, R5>, SendEcho_0_5<R1, R2, R3, R4, R5>, IInput, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_0_5<R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ echo оповещения.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo<R1, R2>(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_2<R1, R2>, SendEcho_0_2<R1, R2>, IInput, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_0_2<R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ echo оповещения.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo<R1, R2, R3>(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_3<R1, R2, R3>, SendEcho_0_3<R1, R2, R3>, IInput, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_0_3<R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ echo оповещения.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo<R1, R2, R3, R4>(ref IInput input, string name)
            => _globalObjectsManager.Get<ListenEcho_0_4<R1, R2, R3, R4>, SendEcho_0_4<R1, R2, R3, R4>, IInput, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_0_4<R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo_1_1<T, R>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_1<T, R>, SendEcho_1_1<T, R>, IInput<T>, IRedirect<R>>
                    (name, ref input, new SendEcho_1_1<T, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo_2_1<T1, T2, R>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_1<T1, T2, R>, SendEcho_2_1<T1, T2, R>, IInput<T1, T2>, IRedirect<R>>
                    (name, ref input, new SendEcho_2_1<T1, T2, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo_3_1<T1, T2, T3, R>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_1<T1, T2, T3, R>, SendEcho_3_1<T1, T2, T3, R>, IInput<T1, T2, T3>, IRedirect<R>>
                (name, ref input, new SendEcho_3_1<T1, T2, T3, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo_4_1<T1, T2, T3, T4, R>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_1<T1, T2, T3, T4, R>, SendEcho_4_1<T1, T2, T3, T4, R>, IInput<T1, T2, T3, T4>, IRedirect<R>>
                (name, ref input, new SendEcho_4_1<T1, T2, T3, T4, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R> send_echo_5_1<T1, T2, T3, T4, T5, R>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_1<T1, T2, T3, T4, T5, R>, SendEcho_5_1<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                (name, ref input, new SendEcho_5_1<T1, T2, T3, T4, T5, R>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo_1_2<T, R1, R2>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_2<T, R1, R2>, SendEcho_1_2<T, R1, R2>, IInput<T>, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_1_2<T, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo_2_2<T1, T2, R1, R2>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_2<T1, T2, R1, R2>, SendEcho_2_2<T1, T2, R1, R2>, IInput<T1, T2>, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_2_2<T1, T2, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo_3_2<T1, T2, T3, R1, R2>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_2<T1, T2, T3, R1, R2>, SendEcho_3_2<T1, T2, T3, R1, R2>, IInput<T1, T2, T3>, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_3_2<T1, T2, T3, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo_4_2<T1, T2, T3, T4, R1, R2>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_2<T1, T2, T3, T4, R1, R2>, SendEcho_4_2<T1, T2, T3, T4, R1, R2>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_4_2<T1, T2, T3, T4, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> send_echo_5_2<T1, T2, T3, T4, T5, R1, R2>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_2<T1, T2, T3, T4, T5, R1, R2>, SendEcho_5_2<T1, T2, T3, T4, T5, R1, R2>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2>>
                    (name, ref input, new SendEcho_5_2<T1, T2, T3, T4, T5, R1, R2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo_1_3<T, R1, R2, R3>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_3<T, R1, R2, R3>, SendEcho_1_3<T, R1, R2, R3>, IInput<T>, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_1_3<T, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo_2_3<T1, T2, R1, R2, R3>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_3<T1, T2, R1, R2, R3>, SendEcho_2_3<T1, T2, R1, R2, R3>, IInput<T1, T2>, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_2_3<T1, T2, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo_3_3<T1, T2, T3, R1, R2, R3>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_3<T1, T2, T3, R1, R2, R3>, SendEcho_3_3<T1, T2, T3, R1, R2, R3>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_3_3<T1, T2, T3, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo_4_3<T1, T2, T3, T4, R1, R2, R3>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_3<T1, T2, T3, T4, R1, R2, R3>, SendEcho_4_3<T1, T2, T3, T4, R1, R2, R3>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_4_3<T1, T2, T3, T4, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> send_echo_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, SendEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3>>
                    (name, ref input, new SendEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo_1_4<T, R1, R2, R3, R4>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_4<T, R1, R2, R3, R4>, SendEcho_1_4<T, R1, R2, R3, R4>, IInput<T>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_1_4<T, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo_2_4<T1, T2, R1, R2, R3, R4>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_4<T1, T2, R1, R2, R3, R4>, SendEcho_2_4<T1, T2, R1, R2, R3, R4>, IInput<T1, T2>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_2_4<T1, T2, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo_3_4<T1, T2, T3, R1, R2, R3, R4>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_4<T1, T2, T3, R1, R2, R3, R4>, SendEcho_3_4<T1, T2, T3, R1, R2, R3, R4>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_3_4<T1, T2, T3, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>, SendEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> send_echo_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, SendEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new SendEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo_1_5<T, R1, R2, R3, R4, R5>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenEcho_1_5<T, R1, R2, R3, R4, R5>, SendEcho_1_5<T, R1, R2, R3, R4, R5>, IInput<T>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_1_5<T, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo_2_5<T1, T2, R1, R2, R3, R4, R5>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenEcho_2_5<T1, T2, R1, R2, R3, R4, R5>, SendEcho_2_5<T1, T2, R1, R2, R3, R4, R5>, IInput<T1, T2>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_2_5<T1, T2, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>, SendEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>, SendEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво отправляемых сообщений]_[количесво ожидаемых сообщений](IInput, string)
        /// Оправляет echo сообщение в родительский обьект прослушивающий echo сообщения по имени name и идентичном типом принимаемых
        /// и оправляемых обратно данных. 
        /// Данный способ связи не гарантирует что к моменту получения данных обратно этот обьект не будет остановлен.
        /// При отправке данных вы можете заблокировать обьект и он не сможет начать процес отановки.  
        /// Сделать это можно помощью встроеного метода TryIncrementEvent(). Если данный метод вернет true
        /// обьект отключит возможность начала процесса остановки и будет ожидать вызова DecrementEvent().
        /// false в свою очередь будет означать что обьект уже прекращает свою работу.
        /// </summary>
        /// <param name="input">Способ отправки echo сообщений.</param>
        /// <param name="name">Имя listen_echo(string); в родительском обьекте которые принимает echo сообщения.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> send_echo_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, SendEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new SendEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящие сообщения. 
        /// </summary>
        /// <param name="name">Имя по которому осущесвляется доступ.</param>
        /// <returns></returns>
        protected IRedirect<T> listen_message<T>(string name)
            => _globalObjectsManager.Add<ListenMessage<T>, IRedirect<T>>
                (name, new ListenMessage<T>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящие сообщения. 
        /// </summary>
        /// <param name="name">Имя по которому осущесвляется доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2> listen_message<T1, T2>(string name)
            => _globalObjectsManager.Add<ListenMessage<T1, T2>, IRedirect<T1, T2>>
                (name, new ListenMessage<T1, T2>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящие сообщения. 
        /// </summary>
        /// <param name="name">Имя по которому осущесвляется доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3> listen_message<T1, T2, T3>(string name)
            => _globalObjectsManager.Add<ListenMessage<T1, T2, T3>, IRedirect<T1, T2, T3>>
                (name, new ListenMessage<T1, T2, T3>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящие сообщения. 
        /// </summary>
        /// <param name="name">Имя по которому осущесвляется доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4> listen_message<T1, T2, T3, T4>(string name)
            => _globalObjectsManager.Add<ListenMessage<T1, T2, T3, T4>, IRedirect<T1, T2, T3, T4>>
                (name, new ListenMessage<T1, T2, T3, T4>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Прослушивает входящие сообщения. 
        /// </summary>
        /// <param name="name">Имя по которому осущесвляется доступ.</param>
        /// <returns></returns>
        protected IRedirect<T1, T2, T3, T4, T5> listen_message<T1, T2, T3, T4, T5>(string name)
            => _globalObjectsManager.Add<ListenMessage<T1, T2, T3, T4, T5>, IRedirect<T1, T2, T3, T4, T5>>
                (name, new ListenMessage<T1, T2, T3, T4, T5>(this));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает сообщение в listen_message(string) определеный в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки сообщений</param>
        /// <param name="name">Имя listen_message который принимает сообщение.</param>
        protected void send_message<T>(ref IInput<T> input, string name)
            => _globalObjectsManager.Get<ListenMessage<T>, IInput<T>>
                (name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает сообщение в listen_message(string) определеный в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки сообщений</param>
        /// <param name="name">Имя listen_message который принимает сообщение.</param>
        protected void send_message<T1, T2>(ref IInput<T1, T2> input, string name)
            => _globalObjectsManager.Get<ListenMessage<T1, T2>, IInput<T1, T2>>
                (name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает сообщение в listen_message(string) определеный в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки сообщений</param>
        /// <param name="name">Имя listen_message который принимает сообщение.</param>
        protected void send_message<T1, T2, T3>(ref IInput<T1, T2, T3> input, string name)
            => _globalObjectsManager.Get<ListenMessage<T1, T2, T3>, IInput<T1, T2, T3>>
                (name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает сообщение в listen_message(string) определеный в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки сообщений</param>
        /// <param name="name">Имя listen_message который принимает сообщение.</param>
        protected void send_message<T1, T2, T3, T4>(ref IInput<T1, T2, T3, T4> input, string name)
            => _globalObjectsManager.Get<ListenMessage<T1, T2, T3, T4>, IInput<T1, T2, T3, T4>>
                (name, ref input);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает сообщение в listen_message(string) определеный в родительском обьекте.
        /// </summary>
        /// <param name="input">Способ отправки сообщений</param>
        /// <param name="name">Имя listen_message который принимает сообщение.</param>
        protected void send_message<T1, T2, T3, T4, T5>(ref IInput<T1, T2, T3, T4, T5> input, string name)
            => _globalObjectsManager.Get<ListenMessage<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>>
                (name, ref input);

        #endregion

        #region Input

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для вызова
        /// метода Branch обьект использую конструкцию: input_to(ref input, obj<TestObject>("TestObjectName").Process());
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        protected void input_to
            (ref IInput input, System.Action action)
                => new ActionObject(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает данные в метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который данные будут переданы.</param>
        protected void input_to<T>
            (ref IInput<T> input, System.Action<T> action)
                => new ActionObject<T>(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает данные в метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который данные будут переданы.</param>
        protected void input_to<T1, T2>
            (ref IInput<T1, T2> input, System.Action<T1, T2> action)
                => new ActionObject<T1, T2>(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает данные в метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который данные будут переданы.</param>
        protected void input_to<T1, T2, T3>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3> action)
                => new ActionObject<T1, T2, T3>(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает данные в метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который данные будут переданы.</param>
        protected void input_to<T1, T2, T3, T4>
            (ref IInput<T1, T2, T3, T4> input,
                System.Action<T1, T2, T3, T4> action)
                    => new ActionObject<T1, T2, T3, T4>(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает данные в метод. Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который данные будут переданы.</param>
        protected void input_to<T1, T2, T3, T4, T5>
            (ref IInput<T1, T2, T3, T4, T5> input,
                System.Action<T1, T2, T3, T4, T5> action)
                    => new ActionObject<T1, T2, T3, T4, T5>(ref input, action);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод и получает результат выполнения с помощью .output_to(). 
        /// Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Get(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова метода.</param>
        /// <param name="action">Метод в который будет вызван.</param>
        protected IRedirect<R> input_to<R>
            (ref IInput input, System.Func<R> func)
                => new FuncObject<R>(ref input, func, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод передает данные и получает результат выполнения с помощью .output_to(). 
        /// Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который будет вызван.</param>
        protected IRedirect<R> input_to<T, R>
            (ref IInput<T> input, System.Func<T, R> func)
                => new FuncObject<T, R>(ref input, func, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод передает данные и получает результат выполнения с помощью .output_to(). 
        /// Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который будет вызван.</param>
        protected IRedirect<R> input_to<T1, T2, R>
            (ref IInput<T1, T2> input, System.Func<T1, T2, R> func)
                => new FuncObject<T1, T2, R>(ref input, func, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод передает данные и получает результат выполнения с помощью .output_to(). 
        /// Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который будет вызван.</param>
        protected IRedirect<R> input_to<T1, T2, T3, R>
            (ref IInput<T1, T2, T3> input, System.Func<T1, T2, T3, R> func)
                => new FuncObject<T1, T2, T3, R>(ref input, func, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Вызывает метод передает данные и получает результат выполнения с помощью .output_to(). 
        /// Выполняемую работу в методе можно переложить на событие, 
        /// указав третьим параметром его имя. Так же можно использовать данный способ для передачи
        /// данных в Branch обьект использую конструкцию: input_to(ref input, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// Метод можно обернуть в конструкцию TryIncrementEvent() который в случае возрата true защитит обьект от преждевременной
        /// остановки что позволит методу выполнится до того как обьект начнет останавливаться. 
        /// Возрат false будет означать что обьект начал процесс остановки. Так же нужно будет разблакировать обьект с помощью
        /// метода DecrementEvent();
        /// </summary>
        /// <param name="input">Способ передачи данных.</param>
        /// <param name="action">Метод в который будет вызван.</param>
        protected IRedirect<R> input_to<T1, T2, T3, T4, T5, R>
            (ref IInput<T1, T2, T3, T4, T5> input,
                System.Func<T1, T2, T3, T4, T5, R> func)
                    => new FuncObject<T1, T2, T3, T4, T5, R>(ref input, func, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to
            (ref IInput input, string name, System.Action action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll, IInput>
                    (name, ref input, new ActionObjectPoll(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он и получит на вход параметр[ы] переданые в метод который небыл вызван. 
        /// В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to<T>
            (ref IInput<T> input, string name, System.Action<T> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll<T>, IInput<T>>
                    (name, ref input, new ActionObjectPoll<T>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он и получит на вход параметр[ы] переданые в метод который небыл вызван. 
        /// В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to<T1, T2>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll<T1, T2>, IInput<T1, T2>>
                    (name, ref input, new ActionObjectPoll<T1, T2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он и получит на вход параметр[ы] переданые в метод который небыл вызван. 
        /// В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to<T1, T2, T3>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3> action, System.Action<T1, T2, T3> safe = null)
                    => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll<T1, T2, T3>, IInput<T1, T2, T3>>
                            (name, ref input, new ActionObjectPoll<T1, T2, T3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to<T1, T2, T3, T4>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4> action, System.Action<T1, T2, T3, T4> safe = null)
                    => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll<T1, T2, T3, T4>, IInput<T1, T2, T3, T4>>
                            (name, ref input, new ActionObjectPoll<T1, T2, T3, T4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected void input_to<T1, T2, T3, T4, T5>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                    => _globalObjectsManager.Get<HandlerEvents, ActionObjectPoll<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>>
                            (name, ref input, new ActionObjectPoll<T1, T2, T3, T4, T5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<R>
            (ref IInput input, string name, System.Func<R> func, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<R>, IInput, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<T, R>
            (ref IInput<T> input, string name, System.Func<T, R> func, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<T, R>, IInput<T>, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<T, R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<T1, T2, R>
            (ref IInput<T1, T2> input, string name, System.Func<T1, T2, R> func, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<T1, T2, R>, IInput<T1, T2>, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<T1, T2, R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<T1, T2, T3, R>
            (ref IInput<T1, T2, T3> input, string name, System.Func<T1, T2, T3, R> func, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<T1, T2, T3, R>, IInput<T1, T2, T3>, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<T1, T2, T3, R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<T1, T2, T3, T4, R>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Func<T1, T2, T3, T4, R> func, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<T1, T2, T3, T4, R>, IInput<T1, T2, T3, T4>, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<T1, T2, T3, T4, R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// Передает вызов метода в событие с именем указаным в параметре name.
        /// Если к моменту вызова метода событием обьект начал прекращать свою работу,
        /// то данный метод не будет вызван, но если вы передали четвертым параметом safeметод, то вызовется 
        /// именно он. В случае если обьект продолжает свою работу, то в начале выполнения метода в событии
        /// выставится флаг о том что обьект не может приступить к остановки до тех пор пока метод не закончит свою работу.
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(type));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи метода в событие.</param>
        /// <param name="name">Имя события.</param>
        /// <param name="action">Метод который должен вызваться.</param>
        /// <param name="safe">Метод который вызовется в случае если обьект приступил к остановки.</param>
        protected IRedirect<R> input_to<T1, T2, T3, T4, T5, R>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Func<T1, T2, T3, T4, T5, R> func, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, FuncObjectPoll<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                    (name, ref input, new FuncObjectPoll<T1, T2, T3, T4, T5, R>(ref input, func, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_0_1<R>
            (ref IInput input, System.Action<IReturn<R>> action)
                => new PairActionObject_0_1<R>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_0_2<R1, R2>
            (ref IInput input, System.Action<IReturn<R1, R2>> action)
                => new PairActionObject_0_2<R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_0_3<R1, R2, R3>
            (ref IInput input, System.Action<IReturn<R1, R2, R3>> action)
                => new PairActionObject_0_3<R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_0_4<R1, R2, R3, R4>
            (ref IInput input, System.Action<IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_0_4<R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_0_5<R1, R2, R3, R4, R5>
            (ref IInput input, System.Action<IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_0_5<R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1> input_to_1_1<T1, R1>
            (ref IInput<T1> input, System.Action<T1, IReturn<R1>> action)
                => new PairActionObject_1_1<T1, R1>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_1_2<T1, R1, R2>
            (ref IInput<T1> input, System.Action<T1, IReturn<R1, R2>> action)
                => new PairActionObject_1_2<T1, R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_2_2<T1, T2, R1, R2>
            (ref IInput<T1, T2> input, System.Action<T1, T2, IReturn<R1, R2>> action)
                => new PairActionObject_2_2<T1, T2, R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_3_2<T1, T2, T3, R1, R2>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3, IReturn<R1, R2>> action)
                => new PairActionObject_3_2<T1, T2, T3, R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_4_2<T1, T2, T3, T4, R1, R2>
            (ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4, IReturn<R1, R2>> action)
                => new PairActionObject_4_2<T1, T2, T3, T4, R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_5_2<T1, T2, T3, T4, T5, R1, R2>
            (ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action)
                => new PairActionObject_5_2<T1, T2, T3, T4, T5, R1, R2>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_1_3<T1, R1, R2, R3>
            (ref IInput<T1> input, System.Action<T1, IReturn<R1, R2, R3>> action)
                => new PairActionObject_1_3<T1, R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_2_3<T1, T2, R1, R2, R3>
            (ref IInput<T1, T2> input, System.Action<T1, T2, IReturn<R1, R2, R3>> action)
                => new PairActionObject_2_3<T1, T2, R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_3_3<T1, T2, T3, R1, R2, R3>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3, IReturn<R1, R2, R3>> action)
                => new PairActionObject_3_3<T1, T2, T3, R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_4_3<T1, T2, T3, T4, R1, R2, R3>
            (ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> action)
                => new PairActionObject_4_3<T1, T2, T3, T4, R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_5_3<T1, T2, T3, T4, T5, R1, R2, R3>
            (ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action)
                => new PairActionObject_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_1_4<T1, R1, R2, R3, R4>
            (ref IInput<T1> input, System.Action<T1, IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_1_4<T1, R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_2_4<T1, T2, R1, R2, R3, R4>
            (ref IInput<T1, T2> input, System.Action<T1, T2, IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_2_4<T1, T2, R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_3_4<T1, T2, T3, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_3_4<T1, T2, T3, R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_4_4<T1, T2, T3, T4, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action)
                => new PairActionObject_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_1_5<T1, R1, R2, R3, R4, R5>
            (ref IInput<T1> input, System.Action<T1, IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_1_5<T1, R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_2_5<T1, T2, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2> input, System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_2_5<T1, T2, R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_3_5<T1, T2, T3, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action)
                => new PairActionObject_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1> input_to_2_1<T1, T2, R1>
            (ref IInput<T1, T2> input, System.Action<T1, T2, IReturn<R1>> action)
                => new PairActionObject_2_1<T1, T2, R1>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1> input_to_3_1<T1, T2, T3, R1>
            (ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3, IReturn<R1>> action)
                => new PairActionObject_3_1<T1, T2, T3, R1>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1> input_to_4_1<T1, T2, T3, T4, R1>
            (ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4, IReturn<R1>> action)
                => new PairActionObject_4_1<T1, T2, T3, T4, R1>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Вызывает метод. Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().Вызов IReturn можно обернуть в конструкцию
        /// TryIncrementEvents() что гарантирует что обьект не начнет процесс уничтожения до того как цепочка методов
        /// закончит свое выполнение. Не забудьте вызвать DecrementEvents().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ вызова.</param>
        /// <param name="action">Вызываемый метод.</param>
        /// <returns></returns>
        protected IRedirect<R1> input_to_5_1<T1, T2, T3, T4, T5, R1>
            (ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5, IReturn<R1>> action)
                => new PairActionObject_5_1<T1, T2, T3, T4, T5, R1>(ref input, action, this);

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_0_1<R>
            (ref IInput input, string name, System.Action<IReturn<R>> action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_0_1<R>, IInput, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_0_1<R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_0_2<R1, R2>
            (ref IInput input, string name, System.Action<IReturn<R1, R2>> action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_0_2<R1, R2>, IInput, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_0_2<R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_0_3<R1, R2, R3>
            (ref IInput input, string name, System.Action<IReturn<R1, R2, R3>> action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_0_3<R1, R2, R3>, IInput, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_0_3<R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_0_4<R1, R2, R3, R4>
            (ref IInput input, string name, System.Action<IReturn<R1, R2, R3, R4>> action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_0_4<R1, R2, R3, R4>, IInput, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_0_4<R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_0_5<R1, R2, R3, R4, R5>
            (ref IInput input, string name, System.Action<IReturn<R1, R2, R3, R4, R5>> action, System.Action safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>, IInput, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_1_1<T, R>
            (ref IInput<T> input, string name, System.Action<T, IReturn<R>> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_1_1<T, R>, IInput<T>, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_1_1<T, R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_1_2<T, R1, R2>
            (ref IInput<T> input, string name, System.Action<T, IReturn<R1, R2>> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_1_2<T, R1, R2>, IInput<T>, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_1_2<T, R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_1_3<T, R1, R2, R3>
            (ref IInput<T> input, string name, System.Action<T, IReturn<R1, R2, R3>> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_1_3<T, R1, R2, R3>, IInput<T>, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_1_3<T, R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_1_4<T, R1, R2, R3, R4>
            (ref IInput<T> input, string name, System.Action<T, IReturn<R1, R2, R3, R4>> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_1_4<T, R1, R2, R3, R4>, IInput<T>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_1_4<T, R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_1_5<T, R1, R2, R3, R4, R5>
            (ref IInput<T> input, string name, System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, System.Action<T> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>, IInput<T>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_2_1<T1, T2, R>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2, IReturn<R>> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_2_1<T1, T2, R>, IInput<T1, T2>, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_2_1<T1, T2, R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_2_2<T1, T2, R1, R2>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2, IReturn<R1, R2>> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_2_2<T1, T2, R1, R2>, IInput<T1, T2>, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_2_2<T1, T2, R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_2_3<T1, T2, R1, R2, R3>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2, IReturn<R1, R2, R3>> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_2_3<T1, T2, R1, R2, R3>, IInput<T1, T2>, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_2_3<T1, T2, R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_2_4<T1, T2, R1, R2, R3, R4>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2, IReturn<R1, R2, R3, R4>> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_2_4<T1, T2, R1, R2, R3, R4>, IInput<T1, T2>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_2_4<T1, T2, R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_2_5<T1, T2, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2> input, string name, System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> action, System.Action<T1, T2> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_2_5<T1, T2, R1, R2, R3, R4, R5>, IInput<T1, T2>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_2_5<T1, T2, R1, R2, R3, R4, R5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_3_1<T1, T2, T3, R>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3, IReturn<R>> action, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_3_1<T1, T2, T3, R>, IInput<T1, T2, T3>, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_3_1<T1, T2, T3, R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_3_2<T1, T2, T3, R1, R2>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3, IReturn<R1, R2>> action, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_3_2<T1, T2, T3, R1, R2>, IInput<T1, T2, T3>, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_3_2<T1, T2, T3, R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_3_3<T1, T2, T3, R1, R2, R3>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3, IReturn<R1, R2, R3>> action, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_3_3<T1, T2, T3, R1, R2, R3>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_3_3<T1, T2, T3, R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_3_4<T1, T2, T3, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> action, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_3_4<T1, T2, T3, R1, R2, R3, R4>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_3_4<T1, T2, T3, R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_3_5<T1, T2, T3, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3> input, string name, System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> action, System.Action<T1, T2, T3> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_3_5<T1, T2, T3, R1, R2, R3, R4, R5>, IInput<T1, T2, T3>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_3_5<T1, T2, T3, R1, R2, R3, R4, R5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_4_1<T1, T2, T3, T4, R>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4, IReturn<R>> action, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_4_1<T1, T2, T3, T4, R>, IInput<T1, T2, T3, T4>, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_4_1<T1, T2, T3, T4, R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_4_2<T1, T2, T3, T4, R1, R2>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4, IReturn<R1, R2>> action, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_4_2<T1, T2, T3, T4, R1, R2>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_4_2<T1, T2, T3, T4, R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_4_3<T1, T2, T3, T4, R1, R2, R3>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> action, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_4_3<T1, T2, T3, T4, R1, R2, R3>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_4_3<T1, T2, T3, T4, R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_4_4<T1, T2, T3, T4, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> action, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_4_4<T1, T2, T3, T4, R1, R2, R3, R4>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_4_4<T1, T2, T3, T4, R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3, T4> input, string name, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> action, System.Action<T1, T2, T3, T4> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R> input_to_5_1<T1, T2, T3, T4, T5, R>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5, IReturn<R>> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                    (name, ref input, new PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2> input_to_5_2<T1, T2, T3, T4, T5, R1, R2>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2>>
                    (name, ref input, new PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3> input_to_5_3<T1, T2, T3, T4, T5, R1, R2, R3>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3>>
                    (name, ref input, new PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4> input_to_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4>>
                    (name, ref input, new PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref input, action, this, safe));

        /// <summary>
        /// [Предназначен только для определения в методе void Construction();] 
        /// [send_echo]_[количесво передоваемых параметров]_[количесво возращаемых параметров](IInput, string)
        /// Передает вызов метода в событие. Если к моменту вызова метода обьект начнет остановку, то данный метод
        /// не будет вызван, а вызовется метод переданый как дополнительный и в него будут переданы параметры которые 
        /// предназначались для основного метода.
        /// В случае успешного вызова, пока данный метод не закончит свое выполнение обьект не начнет процесс остановки.
        /// Передача результата выполнения происходит с помощью крайнего параметра IReturn.
        /// В качесве первого параметра принимает способ вызова. В качесве второго параметра принимает метод
        /// который должен в качесве крайнего параметра принимать IReturn, с помощью которого передаст результат
        /// выполнения в следующий метод указаный с помощью .output_to().
        /// Так же можно использовать данный способ для вызова метода из 
        /// Branch обьектa использую конструкцию: input_to(ref input, name, obj("TestObjectName").Add(..., IReturn));
        /// Branch обьект будет автоматически создан и доступен для других монипуляций по этому же имени.
        /// </summary>
        /// <param name="input">Способ передачи в событие</param>
        /// <param name="name">Имя события</param>
        /// <param name="action">Метод</param>
        /// <param name="safe">Метод который вызовется в случае если обьект начнет остановку до того как событие начнет вызов.</param>
        /// <returns></returns>
        protected IRedirect<R1, R2, R3, R4, R5> input_to_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>
            (ref IInput<T1, T2, T3, T4, T5> input, string name, System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action, System.Action<T1, T2, T3, T4, T5> safe = null)
                => _globalObjectsManager.Get<HandlerEvents, PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref input, new PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref input, action, this, safe));

        #endregion

        #region ObjectsManager

        private manager.BranchObjects _branchObjectsManager;
        private manager.NodeObjects _nodeObjectsManager;

        protected IRedirect<R> obj<ObjectType, R>(string key, object localFields = null)
            where ObjectType : main.Object, IRedirect<R>, new()
                => obj<ObjectType>(key, localFields);

        /// <summary>
        /// Создание обьектов: Node и Branch. Создание обьекта происходит в системном событии по умолчанию с пустым именем "". 
        ///     Cоздание/Получение Branch обьектa реализуется в системном методе void Construction().
        /// После вызова данного метода вы несможете больше создать или получить доступ к Branch обьекту.
        /// Если будет нужен доступ к данному типу обьекта, сохраните ссылку на него в данном методе.
        /// Отныне этот обьект будет являться неразрывной частью своего родителя. Все дочерние обьекты родителя
        /// могут установить связь с определеными listen_****. методами в его Branch обьектах.
        /// У Branch обьектов могут быть свои дочерние Branch обьекты они так же будут связаны с ближайшим Node обьектом,
        /// и являться ответвлением его ветки. Branch обьекты прекратят свою работу в самую последнюю очередь, 
        /// после того как будут остановлены и уничтожены все дочерние Node обьекты Node родителя Branch обьекта. 
        /// После чего уничтожение продолжится начиная с самого первого созданого Branch обьекта. В котором так же 
        /// изначально будут остановлены и уничтожены все дочерние Node обьекты после чего если у Branch обьекта
        /// имеется свой Branch обьект, то продолжится его остановка и тд. Хоть остановка начинается с первого обьявленого
        /// Branch обьекта нету горантий что он прекратит свою работу быстрее чем самый крайний Branch обьект.
        ///     Создание/Получение Node обьектов начинается с системного метода void Start().
        /// Node обьекты бывают двух видов обычный и Board. Обычный Node обьект связан со своим родителем и при его
        /// уничтожении уничтожется и его родитель. В системном методе void Start() можно создать только обычные Node обьекты.
        /// Второй вид обьектов называется Board. Этот вид Node обьекта олицетворяет черту до которой в случае вызова метода
        /// destroy() в одном из дочерних обьектов Board обьекта уничтожится все что находится ниже данного обьекта.
        /// P.S если обьект не дошел до стадии вызова метода void Construction(), 
        /// то и системный метод void Stop() не будет вызван. Получить полную информацию о жизни обьекта можно
        /// обратившить к полю StateInformation.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="localFields"></param>
        /// <typeparam name="ObjectType"></typeparam>
        /// <returns></returns>
        protected ObjectType obj<ObjectType>(string key, object localFields = null)
            where ObjectType : main.Object, new()
        {
            if (StateInformation.IsContruction)
            {
                return _branchObjectsManager.Add<ObjectType>
                    (key, localFields);
            }
            else
                return _nodeObjectsManager.Add<ObjectType>
                    (key, localFields);
        }

        /// <summary>
        /// Пытаемся получить обьект по ключу.
        /// </summary>
        protected bool try_obj<ObjectType>(string key, out ObjectType value)
            where ObjectType : main.Object, new()
                => _nodeObjectsManager.TryGet<ObjectType>(key, out value);

        /// <summary>
        /// Проверяем наличие обьекта по ключу. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool try_obj(string key)
            => _nodeObjectsManager.Contains(key);

        ObjectType IController.obj<ObjectType>(string key, object localFields)
            => _nodeObjectsManager.Add<ObjectType>(key, localFields);

        ObjectType IController.obj<ObjectType>(string key)
            => _nodeObjectsManager.Add<ObjectType>(key, null);

        bool IController.try_obj<ObjectType>(string key, out ObjectType value)
            => _nodeObjectsManager.TryGet<ObjectType>(key, out value);

        bool IController.try_obj(string key)
            => _nodeObjectsManager.Contains(key);

        void manager.INodeObjects.InformingCollected()
            => _nodeObjectsManager.IncrementCollectedCount();

        void manager.INodeObjects.Remove(string key)
            => _nodeObjectsManager.Remove(key);

        void manager.IBranchObjects.Remove(string key)
            => _branchObjectsManager.Remove(key);

        #endregion

        #region Events

        protected void add_event(string name, System.Action action)
            => _subscribeManager.Add(name, action, manager.Subscribe.Type.POLL, 0);

        protected void add_event(string name, uint timeDelay, System.Action action)
            => _subscribeManager.Add(name, action, manager.Subscribe.Type.POLL, timeDelay);

        #endregion

        #region Thread

        private manager.Threads _threadsManager;

        protected void add_thread
            (string name, System.Action action, uint timeDelay, Thread.Priority priority)
                => _threadsManager.Add(name, action, timeDelay, priority);

        protected void replace_time_delay(string name, uint value)
            => _threadsManager.ReplaceTimeDelay(name, value);

        #endregion

        #region Informing

        protected void Console(System.IConvertible message)
            => global::System.Console.WriteLine
                ($"{_headerInformation.Explorer}:{GetKey()}:{message}");

        protected void ConsoleLine(System.IConvertible message)
            => global::System.Console.Write
                ($"{_headerInformation.Explorer}:{GetKey()}:{message}");

        void informing.IMain.Console(System.IConvertible message) => Console(message);
        void IController.Console<T>(T message) => Console(message);

        public System.Exception Exception(string message, params System.IConvertible[] arg)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;

            System.Console.WriteLine($"{_headerInformation.Explorer}:{message}", arg);

            sleep(10000000);

            throw new System.Exception(message);
        }

        protected void SystemInformation(string message, System.ConsoleColor color = System.ConsoleColor.Green)
        {
            System.Console.ForegroundColor = color;

            global::System.Console.WriteLine
                ($"{_headerInformation.Explorer}:{GetKey()}:{message}");

            System.Console.ForegroundColor = System.ConsoleColor.White;
        }

        void informing.IMain.SystemInformation(string message, System.ConsoleColor color)
            => SystemInformation(message, color);

        void IController.SystemInformation(string message, System.ConsoleColor color)
            => SystemInformation(message, color);

        #endregion

        #region SuscribeManager

        private manager.Subscribe _subscribeManager;

        void description.IPoll.Add(string name, System.Action action, byte type, uint timeDelay)
            => _subscribeManager.Add(name, action, type, timeDelay);

        #endregion

        #region Information

        public ulong GetID() => _DOMInformation.ID;
        public string GetKey() => _DOMInformation.KeyObject;
        public ulong GetNodeID() => _DOMInformation.NodeID;
        public string GetExplorer() => _headerInformation.Explorer;

        public bool TryIncrementEvent() => _lifeCyrcleManager.TryIncrement();
        public void DecrementEvent() => _lifeCyrcleManager.Decrement();

        manager.IGlobalObjects IInformation.GlobalObjectsManager()
            => _globalObjectsManager;

        #endregion

        #region Hellpers

        private System.DateTime d_localDateTime = System.DateTime.Now;
        private static System.DateTime d_globalStartTime = System.DateTime.Now;

        protected void start_timer()
            => d_localDateTime = System.DateTime.Now;

        protected int current_timer()
             => (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

        protected int step_timer()
        {
            int result = (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

            d_localDateTime = System.DateTime.Now;

            return result;
        }

        protected static void global_start_timer()
            => d_globalStartTime = System.DateTime.Now;

        protected static int global_current_timer()
            => (System.DateTime.Now.Subtract(d_globalStartTime).Seconds * 1000)
                + System.DateTime.Now.Subtract(d_globalStartTime).Milliseconds;

        protected void sleep(int pTimeSpeep)
            => global::System.Threading.Thread.Sleep(pTimeSpeep);


        #endregion
    }
}