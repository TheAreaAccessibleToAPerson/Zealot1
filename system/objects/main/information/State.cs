namespace Butterfly.system.objects.main.information
{
    namespace state
    {
        public interface IManager
        {
            /// <summary>
            /// Установить новое состояние обьекта. 
            /// </summary>
            /// <param name="state">Состояние обьекта.</param>
            public void Set(byte state);

            /// <summary>
            /// Указывает этап на котором обьект прекратил свою сборку. 
            /// </summary>
            /// <param name="state"></param>
            public void SetInterrupted(byte state);

            /// <summary>
            /// Получить текущее состояние обьекта. 
            /// </summary>
            /// <returns></returns>
            public byte Get();

            /// <summary>
            /// Возращает в текущее состояние обьекта в строковом представлении. 
            /// </summary>
            /// <returns></returns>
            public string GetString();

            public string GetString(byte state);

            /// <summary>
            /// Выставить обьект в состояние уничтожения. 
            /// </summary>
            public void Destroy();

            /// <summary>
            /// Выставить обьект в состояние отложеного уничтожения. 
            /// </summary>
            public void DeferredDestroy();
        }
    }

    public class State : state.IManager
    {
        public struct Data
        {
            /// <summary>
            /// Данное состояние означает что обьект еще не приступил к своей сборки. 
            /// Если обьект находясь в данном состоянии будет уничтожен, то он получет
            /// [Прерываение:InterruptedContruction]
            /// </summary>
            public const byte OCCUPERENCE = 0;

            /// <summary>
            /// В данное состояние обьект переходит из состояния Occuperence. 
            /// В данном состаянии обьект запускает системный метод void Construction().
            /// Если это Node обьект, то изначально метод Contruction() вызовется у него, в случае если
            /// в данном методе будет вызван destroy(), то обьект получет 
            /// [Прерываение:InterruptedCallDestroyInContruction].
            /// Если метод вызвался, то далее данное состояние получат все его Branch обьекты и
            /// их Branch обьекты, начиная с самого первого обьявленого в Branch обьекта в void Contruction()
            /// и заканчивая самым последним. Если у Branch обьекта есть свои бренч обьекты, то когда дойдет 
            /// очередь до него он выставит им состояние Contruction и вызовет им метод Contruction().
            /// В случае если в данной цепочке в одном из вызываемых методов или извне в данный момент будет вызван метод 
            /// destroy(), то обьект который в данный момент вызывает метод void Construction() получит
            /// [Прерываение:InterruptedCallDestroyInContruction]
            /// чепочка вызов прервется и все последующие обьекты до которых не дошла очередь
            /// получат [Прерываение:InterruptedContruction] и их метод Contriction() не будет вызван.
            /// В обьектах в которых произошло прерывание InterruptedContruction или InterruptedCallDestroyInContruction
            /// не будет вызван системный метод void Stop().
            /// </summary>
            public const byte CONSTRUCTION = 1;

            /// <summary>
            /// В данное состояние обьект переходит из состояния Construction. 
            /// В данном состаянии обьект запускает системный метод void Configurate().
            /// В начале запустятся системные методы void Confirurate() во всех Branch обьектах принадлежавших
            /// Node обьекту и только потом Node обьект запустит свой void Configurate().
            /// В случае если во время вызова void Configurate() был вызван метод destroy() в самом методе
            /// или внешне обьект получит [Прерываение:InterruptedCallDestroyInContruction]
            /// </summary>
            public const byte CONFIGURATE = 2;
            public const byte STARTING = 3;
            public const byte SUBSCRIBE = 4;

            public const byte START = 5;
            public const byte DESTROYING = 6;

            public const byte STOPPING = 7;
            public const byte STOP = 8;

            /// <summary>
            /// Обьект приступил к уничтожению до вызова метода Construction().
            /// </summary>
            public const byte INTERRUPTED_CONSTRUCTION = 1;

            /// <summary>
            /// Обьект приступил к уничтожению до того как запустил методы Construction(). 
            /// в своих Branch обьектов.
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_CONTRUCTION = 2;

            /// <summary>
            /// Обьект приступил к уничтожению в момент вызова метода Contruction() в своих
            /// Branch обьетах.
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECTS_CONTRUCTION = 3;

            /// <summary>
            /// Обьект приступил к уничтожению до вызова метода Configurate().
            /// </summary>
            public const byte INTERRUPTED_CONFIGURATE = 4;

            /// <summary>
            /// Обьект приступил к уничтожению во время вызова метода Configurate().
            /// в своих Branch обьектах.
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECT_CONFIGURATE = 5;

            /// <summary>
            /// Обьект приступил к уничтожению во время вызова метода Configurate().
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_CONFIGURATE = 6;

            /// <summary>
            /// Обьект приступил к уничтожению до вызова метода Start().
            /// </summary>
            public const byte INTERRUPTED_START = 7;

            /// <summary>
            /// Обьект приступил к уничтожению во время вызова метода Start().
            /// Обьект не подпишится на события.
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_START = 8;

            /// <summary>
            /// Обьект приступил к уничтожению во время вызова метода Start() в своих Branch обьектах.
            /// </summary>
            public const byte INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECTS_STARTING = 9;

            /// <summary>
            /// Обьект приступил к уничтожению до запуска своих потоков.
            /// </summary>
            public const byte INTERRUPTED_START_THREAD_OBJECT = 10;
        }

        /// <summary>
        /// Номер прерывания обьекта при создании.
        /// </summary>
        public byte Interrupted { private set; get; } = 0;

        /// <summary>
        /// Было ли прервано создание обьекта?
        /// </summary>
        public bool IsInterrupted { get { return Interrupted > 0; } }

        /// <summary>
        /// В обьекте был запущен системный метод Start();
        /// </summary>
        public bool IsCallStart { get => (Interrupted == 0 || Interrupted > 7); }

        /// <summary>
        /// В обьекте был запущен системный метод Configurate(). 
        /// </summary>
        public bool IsCallConfigurate { get => (Interrupted == 0 || Interrupted > 4); }

        /// <summary>
        /// В обьекте был запущен системный метод Construction().
        /// </summary>
        public bool IsCallConstruction { get => (Interrupted == 0 || Interrupted > 1); }

        public byte CurrentState { private set; get; } = Data.OCCUPERENCE;

        public readonly object Locker = new();

        /// <summary>
        /// Обьект уничтожается.
        /// </summary>
        private bool _isDestroying = false;

        /// <summary>
        /// Объект находится в процессе уничтожения?
        /// </summary>
        public bool IsDestroying
        {
            get { return _isDestroying; }
        }

        /// <summary>
        /// Отложеное уничтожение. Перед тем как начать уничтожение обьекта
        /// он дожен запустится так как от его работы возможно зависят другие объекты.
        /// </summary>
        private bool _isDeferredDestroying = false;

        /// <summary>
        /// Объект выставлен на отложеное уничтожение?
        /// </summary>
        public bool IsDeferredDestroying
        {
            get { return _isDeferredDestroying; }
        }

        /// <summary>
        /// Обьект будут уничтожен, или уже в процессе. 
        /// </summary>
        /// <value></value>
        public bool IsDestroy
        {
            get
            {
                return (_isDeferredDestroying || _isDestroying);
            }
        }

        /// <summary>
        /// Обьект находится в состоянии зародыша?
        /// </summary>
        public bool IsOccuperence
        {
            get { return CurrentState == Data.OCCUPERENCE; }
        }

        /// <summary>
        /// Обьект находится в состоянии сборки?
        /// </summary>
        public bool IsContruction
        {
            get { return CurrentState == Data.CONSTRUCTION; }
        }

        public bool IsSubscribe
        {
            get { return CurrentState == Data.SUBSCRIBE; }
        }

        public bool IsConfigurate
        {
            get { return CurrentState == Data.CONFIGURATE; }
        }

        /// <summary>
        /// Обьект находится в состоянии запуска.
        /// </summary>
        public bool IsStarting
        {
            get { return CurrentState == Data.STARTING; }
        }
        /// <summary>
        /// Обьект запущен?
        /// </summary>
        public bool IsStart
        {
            get { return CurrentState == Data.START; }
        }
        /// <summary>
        /// Обьект останавливается?
        /// </summary>
        public bool IsStopping
        {
            get { return CurrentState == Data.STOPPING; }
        }
        /// <summary>
        /// Обьект остановлен?
        /// </summary>
        public bool IsStop
        {
            get { return CurrentState == Data.STOP; }
        }

        public string GetString()
        {
            if (IsStart) return "Start";
            if (IsStop) return "Stop";
            if (IsDestroying) return "Destroying";
            if (IsSubscribe) return "Subscribe";
            if (IsOccuperence) return "Occuperence";
            if (IsContruction) return "Contruction";
            if (IsConfigurate) return "Configurate";
            if (IsStarting) return "Starting";
            if (IsStopping) return "Stopping";

            return "";
        }

        public string GetString(byte state)
        {
            if (Data.START == state) return "Start";
            if (Data.STOP == state) return "Stop";
            if (Data.OCCUPERENCE == state) return "Occuperence";
            if (Data.CONSTRUCTION == state) return "Contruction";
            if (Data.DESTROYING == state) return "Destroying";
            if (Data.SUBSCRIBE == state) return "Subscribe";
            if (Data.CONFIGURATE == state) return "Configurate";
            if (Data.STARTING == state) return "Starting";
            if (Data.STOPPING == state) return "Stopping";

            return "";
        }

        void state.IManager.Set(byte state)
            => CurrentState = state;

        void state.IManager.SetInterrupted(byte state)
            => Interrupted = state;

        byte state.IManager.Get()
            => CurrentState;

        void state.IManager.Destroy()
        {
            lock (Locker)
            {
                _isDestroying = true;
            }
        }

        void state.IManager.DeferredDestroy()
        {
            lock (Locker)
            {
                _isDeferredDestroying = true;
            }
        }

        /// <summary>
        /// Менеджер для управления состояние обьекта. 
        /// </summary>
        public class Manager : Informing
        {
            private readonly state.IManager _stateManager;

            public Manager(informing.IMain mainInforming, state.IManager stateManager)
                : base("StateManager", mainInforming)
                    => _stateManager = stateManager;

            /// <summary>
            /// Сменить состояние обьекта с проверкой на текущее состояние. 
            /// </summary>
            /// <param name="state">Состояние на которое нужно сменить.</param>
            /// <param name="currentState">Текущее состояние в котором должен пребывать обьект.</param>
            public bool Replace(byte state, byte currentState)
            {
                if (currentState == _stateManager.Get())
                {
                    _stateManager.Set(state);

                    //SystemInformation($"State replace:{_stateManager.GetString(currentState)}->{_stateManager.GetString(state)}.",
                    //    System.ConsoleColor.White);

                    return true;
                }
                else return false;
            }

            public void Set(byte state)
            {
                //SystemInformation($"State replace:{_stateManager.GetString()}->{_stateManager.GetString(state)}.",
                //    System.ConsoleColor.White);

                _stateManager.Set(state);
            }

            public void SetInterrupted(byte state)
            {
                _stateManager.SetInterrupted(state);
            }

            /// <summary>
            /// Выставить обьекту задачу получения состояния уничтожен.
            /// </summary>
            public void Destroy() => _stateManager.Destroy();

            /// <summary>
            /// Выставить обьекту задачу при первой возможности начать получение состояния уничтожен. 
            /// </summary>
            public void DeferredDestroy() => _stateManager.DeferredDestroy();
        }
    }
}