namespace Butterfly.system.objects.main.manager
{
    public interface IDispatcher
    {
        public void Process(byte command);
    }

    namespace dispatcher
    {
        /// <summary>
        /// Описывает сборос работы диспетчера с LifeCyrcleManager'ом.
        /// </summary>
        public interface ILifeCyrcle
        {
            public void Contruction();
            public void Configurate();
            public void Starting();
            public void Start();
            public void Stopping();
            public void ContinueStopping();
        }

        /// <summary>
        /// Описывает способ работы диспетчера с SubscribeManager'ом.
        /// </summary>
        public interface ISubscribe
        {
            /// <summary>
            /// Запускает процесс отписки. 
            /// </summary>
            void StartSubscribe();

            /// <summary>
            /// Запускает процесс подписки. 
            /// </summary>
            void StartUnsubscribe();
        }

        public interface IThreads
        {
            void Start();
            void Stop();
            void TaskStop();
        }

        public interface IGlobalObjects
        {
            /// <summary>
            /// Удаляем все глобальные обьекты созданые в нем. 
            /// </summary>
            void RemoveObjects();

            /// <summary>
            /// Вызывает все обьекты которые извлекают данные из безопасных глобальных обьектов. 
            /// </summary>
            void ExtractObjects();
        }
    }

    public sealed class Dispatcher : main.Informing, IDispatcher
    {
        public struct Command
        {
            /// <summary>
            /// Базовое состояние. 
            /// </summary>
            public const byte NONE = 0;

            /// <summary>
            /// Создание Node обьекта:
            ///     Вызывается метод LifeCyrcleManager.Construction в котором запустится системный метод
            /// void Construction(). В данном методе Node обьект узнает о своих Branch обьектах, 
            /// о событиях на которые нужно будет подписаться. Так же установит связь с глобальными
            /// обьектами в своих родителях.
            /// Создание Branch обьекта: 
            ///     Данная команда будет доставлена из BranchObjectsManager родительского обьекта.
            /// Произадет все тоже самое что и с Node обьектом, за исключением того что события
            /// на которое нужно пописаться будут записаны в ближайший Node обьект. Именно он 
            /// произведет регистрацию.
            /// summary>
            public const byte CONSTRUCTION_OBJECT = 1;

            /// <summary>
            /// Node обьект сконструирован. Branch обьекты сконструированы, установлена связь с глобальными
            /// обьектами, все билеты для подписки на события созданы.
            /// Node обьект отвечает и за свои подписки на события и за подписки своих Branch обьектов.
            /// </summary>
            public const byte START_SUBSCRIBE = 2;

            /// <summary>
            /// После того как Node обьект получен ответ от пулла, о том что все его регистрационые билеты
            /// для регистрации на события были обработаны, запустится метод LifeCyrcleManager.Starting. 
            /// </summary>
            public const byte CONFIGURATE_OBJECT = 3;

            public const byte STARTING_OBJECT = 4;

            /// <summary>
            /// Первая стадия запуска подошла к концу.
            /// В ней мы вызвали метод Configurate, и если в данном методе обьект не был 
            /// выставлен на уничтожение, то запустился метод Start(). 
            /// </summary>
            public const byte START_OBJECT = 5;

            /// <summary>
            /// Запускаем потоки.
            /// </summary>
            public const byte STARTING_THREAD = 6;

            /// <summary>
            /// Создать отложеные узлы, которые были добавлены в методе Start().
            /// </summary>
            public const byte CREATING_DEFERRED_NODE_OBJECT = 7;

            /// <summary>
            /// Переводит обьект в состояния остановки. Первым делом останавливает поток.
            /// </summary>
            public const byte STOPPING_OBJECT = 8;

            /// <summary>
            /// Запускат обьекты которые должны извлеч данные из безопасных глобыльных обьектов. 
            /// </summary>
            public const byte EXTRACT_OBJECTS_INVOKE = 9;

            /// <summary>
            /// Запускает процесс отписки в Node обьектах.
            /// </summary>
            public const byte START_UNSUBSCRIBE = 10;

            /// <summary>
            /// Прекращает процесс остановки обьектов. 
            /// </summary>
            public const byte CONTINUE_STOPPING = 11;

            /// <summary>
            /// Процесс отписки окончен. 
            /// </summary>
            public const byte END_UNSUBSCRIBE = 12;

            /// <summary>
            /// Запускает в текущем обьекты процесс удаления всех созданых в нем глобальных обьектов. 
            /// </summary>
            public const byte REMOVE_GLOBAL_OBJECTS = 13;
        }

        private readonly information.Header _headerInformation;

        /// <summary>
        /// Текущая команда которую выполняет диспетчер. 
        /// </summary>
        public byte CurrentProcess = Command.NONE;

        private dispatcher.IGlobalObjects _globalObjectsDispatcher;
        private dispatcher.ILifeCyrcle _lifeCyrcleDispatcher;
        private dispatcher.ISubscribe _subscribeDispatcher;
        private dispatcher.IThreads _threadsDispatcher;

        private readonly information.Tegs _tegsInformation;

        public Dispatcher(informing.IMain mainInforming, information.Header headerInformation, information.Tegs tegsInformation)
                : base("DispatcherManager", mainInforming)
        {
            _headerInformation = headerInformation;

            _tegsInformation = tegsInformation;
        }

        public void Initialize(dispatcher.IGlobalObjects globalObjectsDispatcher,
            dispatcher.ILifeCyrcle lifeCyrcleDispathcer, dispatcher.ISubscribe subscribeDispatcher,
                dispatcher.IThreads threadsDispatcher)
        {
            _globalObjectsDispatcher = globalObjectsDispatcher;
            _lifeCyrcleDispatcher = lifeCyrcleDispathcer;
            _subscribeDispatcher = subscribeDispatcher;
            _threadsDispatcher = threadsDispatcher;
        }
        /***************************************************************************************


        ***************************************************************************************/
        public void Process(byte command)
        {
            switch (command)
            {
                case Command.CONSTRUCTION_OBJECT:

                    CurrentProcess = Command.CONSTRUCTION_OBJECT;

                    _lifeCyrcleDispatcher.Contruction();

                    break;

                case Command.START_SUBSCRIBE:

                    CurrentProcess =  Command.START_SUBSCRIBE;

                    _subscribeDispatcher.StartSubscribe();

                    break;

                case Command.CONFIGURATE_OBJECT:

                    CurrentProcess = Command.CONFIGURATE_OBJECT;

                    _lifeCyrcleDispatcher.Configurate();

                    break;
                
                case Command.STARTING_OBJECT:

                    CurrentProcess = Command.STARTING_OBJECT;

                    _lifeCyrcleDispatcher.Starting();

                break;

                case Command.START_OBJECT:

                    CurrentProcess = Command.START_OBJECT;

                    _lifeCyrcleDispatcher.Start();

                    break;

                case Command.STARTING_THREAD:

                    CurrentProcess = Command.STARTING_THREAD;

                    _threadsDispatcher.Start();

                    break;

                case Command.STOPPING_OBJECT:

                    CurrentProcess = Command.STOPPING_OBJECT;

                    if (_tegsInformation.Contains(root.manager.ActionInvoke.TEG))
                    {
                        _threadsDispatcher.TaskStop();
                    }
                    else _threadsDispatcher.Stop();

                    _lifeCyrcleDispatcher.Stopping();

                    break;

                case Command.EXTRACT_OBJECTS_INVOKE:

                _globalObjectsDispatcher.ExtractObjects();

                break;

                case Command.START_UNSUBSCRIBE:

                    CurrentProcess = Command.START_UNSUBSCRIBE;

                    _subscribeDispatcher.StartUnsubscribe();

                    break;

                case Command.CONTINUE_STOPPING:

                    CurrentProcess = Command.CONTINUE_STOPPING;

                    _lifeCyrcleDispatcher.ContinueStopping();

                    break;

                case Command.REMOVE_GLOBAL_OBJECTS:

                    CurrentProcess = Command.REMOVE_GLOBAL_OBJECTS;

                    _globalObjectsDispatcher.RemoveObjects();

                    break;

                default: 
                    Console(command);
                    throw new System.Exception();
            }
        }

        /// <summary>
        /// Запустить диспетчер, начать сборку Node обьекта.
        /// </summary>
        public void Start()
        {
            if (_headerInformation.IsNodeObject())
            {
                ((IDispatcher)this).Process(Command.CONSTRUCTION_OBJECT);
            }
            else 
                Exception("Метод Start вызывается в Node обьекте.");
        }
    }
}