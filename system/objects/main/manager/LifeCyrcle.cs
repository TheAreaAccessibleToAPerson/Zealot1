namespace Butterfly.system.objects.main.manager
{
    public class LifeCyrcle : Informing, dispatcher.ILifeCyrcle
    {
        public struct Data
        {
            /// <summary>
            /// Вызываем команду в диспетчере которая произведет конструирование обьекта
            /// c помощью метода LifeCyrcleManager.Construction.
            /// </summary>
            public const byte BEGIN_BRANCH_OBJECT_CONTRUCTION = 0;

            /// <summary>
            /// Приступаем к запуску обьектов.
            /// </summary>
            public const byte BEGIN_CONFIGURATE = 1;

            public const byte BEGIN_STARTING = 2;

            /// <summary>
            /// Продолжаем запуск обьекта. 
            /// </summary>
            public const byte BEGIN_START = 3;

            /// <summary>
            /// Приступает к остановки обьектов. 
            /// </summary>
            public const byte BEGIN_STOPPING = 4;
        }

        private readonly information.State.Manager _stateInformationManager;
        private readonly information.State _stateInformation;
        private readonly information.Header _headerInformation;
        private readonly information.DOM _DOMInformation;
        private readonly information.Tegs _tegsInformation;

        private readonly manager.BranchObjects _branchObjectsManager;
        private readonly manager.NodeObjects _nodeObjectsManager;

        private readonly IDispatcher _dispatcher;


        /// <summary>
        /// Как только обьект приступает к уничтожению запускаем системный метод Destruction.
        /// </summary>
        private bool _isCallDesctructionMethod = false;

        /// <summary>
        /// Обьекты могут апосредовано запустить данный метод, поэтому
        /// во избежания повторного вызова используем данный флаг. 
        ///</summary>
        bool _isStoppingRun = false;

        public LifeCyrcle(informing.IMain mainInforming, information.Header headerInformation,
            information.State stateInformation, information.State.Manager stateInformationManager,
                information.DOM DOMInformation, information.Tegs tegsInformation,
                    manager.BranchObjects branchObjectsManager, manager.NodeObjects nodeObjectsManager,
                        IDispatcher dispatcher)
            : base("LifeCyrcleManager", mainInforming)
        {
            _stateInformationManager = stateInformationManager;
            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _tegsInformation = tegsInformation;
            _DOMInformation = DOMInformation;

            _branchObjectsManager = branchObjectsManager;
            _nodeObjectsManager = nodeObjectsManager;

            _dispatcher = dispatcher;
        }


        void dispatcher.ILifeCyrcle.Contruction()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroying == false &&
                    _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                {
                    if (_stateInformationManager.Replace(information.State.Data.CONSTRUCTION, information.State.Data.OCCUPERENCE))
                    {
                        if (Hellper.GetSystemMethod("Construction", _DOMInformation.CurrentObject.GetType(),
                        out System.Reflection.MethodInfo oSystemMethodConstruction))
                            oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);

                        if (_stateInformation.IsDestroying == false &&
                            _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                        {
                            _branchObjectsManager.LifeCyrcle(Data.BEGIN_BRANCH_OBJECT_CONTRUCTION);

                            if (_stateInformation.IsDestroying == false &&
                                _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                            {
                                if (_headerInformation.IsNodeObject())
                                    _dispatcher.Process(manager.Dispatcher.Command.CONFIGURATE_OBJECT);

                                return;
                            }
                            else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECTS_CONTRUCTION);
                        }
                        else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_CONTRUCTION);
                    }
                    else throw new System.Exception();
                }
                else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CONSTRUCTION);
            }

            if (_headerInformation.IsNodeObject())
                ((manager.INodeObjects)_DOMInformation.ParentObject).InformingCollected();
        }

        void dispatcher.ILifeCyrcle.Configurate()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroying == false &&
                    _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                {
                    if (_stateInformationManager.Replace(information.State.Data.CONFIGURATE, information.State.Data.CONSTRUCTION))
                    {
                        _branchObjectsManager.LifeCyrcle(Data.BEGIN_CONFIGURATE);

                        if (_stateInformation.IsDestroying == false &&
                            _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                        {
                            if (Hellper.GetSystemMethod("Configurate", _DOMInformation.CurrentObject.GetType(),
                            out System.Reflection.MethodInfo oSystemMethodConfigurate))
                                oSystemMethodConfigurate.Invoke(_DOMInformation.CurrentObject, null);

                            if (_stateInformation.IsDestroying == false &&
                                _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                            {
                                if (_headerInformation.IsNodeObject())
                                    _dispatcher.Process(manager.Dispatcher.Command.STARTING_OBJECT);

                                return;
                            }
                            else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_CONFIGURATE);
                        }
                        else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECT_CONFIGURATE);
                    }
                    else throw new System.Exception();
                }
                else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CONFIGURATE);
            }

            if (_headerInformation.IsNodeObject())
                ((manager.INodeObjects)_DOMInformation.ParentObject).InformingCollected();
        }

        void dispatcher.ILifeCyrcle.Starting()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroying == false &&
                    _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                {
                    if (_stateInformationManager.Replace(information.State.Data.STARTING, information.State.Data.CONFIGURATE))
                    {
                        _branchObjectsManager.LifeCyrcle(Data.BEGIN_STARTING);

                        if (_stateInformation.IsDestroying == false &&
                            _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                        {
                            if (Hellper.GetSystemMethod("Start", _DOMInformation.CurrentObject.GetType(),
                            out System.Reflection.MethodInfo oSystemMethodConfigurate))
                                oSystemMethodConfigurate.Invoke(_DOMInformation.CurrentObject, null);

                            if (_stateInformation.IsDestroying == false &&
                                _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                            {
                                if (_stateInformationManager.Replace(information.State.Data.SUBSCRIBE, information.State.Data.STARTING))
                                {
                                    if (_headerInformation.IsNodeObject())
                                        _dispatcher.Process(manager.Dispatcher.Command.START_SUBSCRIBE);

                                    return;
                                }
                                else throw new System.Exception();
                            }
                            else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_START);
                        }
                        else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_CALL_DESTROY_IN_BRANCH_OBJECTS_STARTING);
                    }
                    else throw new System.Exception();
                }
                else _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_START);
            }

            if (_headerInformation.IsNodeObject())
                ((manager.INodeObjects)_DOMInformation.ParentObject).InformingCollected();
        }

        void dispatcher.ILifeCyrcle.Start()
        {
            lock (_stateInformation.Locker)
            {
                _branchObjectsManager.LifeCyrcle(Data.BEGIN_START);

                if (_stateInformation.IsDestroying == false && _stateInformation.IsDeferredDestroying == false &&
                    _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroying == false)
                {
                    if (_stateInformationManager.Replace(information.State.Data.START, information.State.Data.SUBSCRIBE))
                    {
                        _dispatcher.Process(manager.Dispatcher.Command.STARTING_THREAD);
                    }
                    else throw new System.Exception();
                }
                else
                {
                    _stateInformationManager.SetInterrupted(information.State.Data.INTERRUPTED_START_THREAD_OBJECT);
                    _stateInformationManager.Set(information.State.Data.DESTROYING);
                }
            }

            if (_headerInformation.IsNodeObject())
                ((manager.INodeObjects)_DOMInformation.ParentObject).InformingCollected();


            if (_stateInformation.IsDeferredDestroying)
                ((manager.ILifeCyrcle)_DOMInformation.CurrentObject).ContinueDestroy();
        }

        void dispatcher.ILifeCyrcle.Stopping()
        {
            lock (_stateInformation.Locker)
            {
                _stateInformationManager.Set(information.State.Data.STOPPING);

                if (_index <= 0)
                {
                    if (_isStoppingRun == false && _nodeObjectsManager.Count == 0 && _nodeObjectsManager.DublicatCount == 0)
                    {
                        _isStoppingRun = true;

                        _dispatcher.Process(manager.Dispatcher.Command.EXTRACT_OBJECTS_INVOKE);

                        //if (_stateInformation.IsInterrupted == false || _stateInformation.Interrupted > 2)
                        {
                            if (Hellper.GetSystemMethod("Stop", _DOMInformation.CurrentObject.GetType(),
                                out System.Reflection.MethodInfo oSystemMethodStart))
                                oSystemMethodStart.Invoke(_DOMInformation.CurrentObject, null);
                        }

                        if (_headerInformation.IsNodeObject())
                        {
                            _dispatcher.Process(manager.Dispatcher.Command.START_UNSUBSCRIBE);
                        }
                        else _dispatcher.Process(manager.Dispatcher.Command.CONTINUE_STOPPING);
                    }
                    else _nodeObjectsManager.StoppingAllObject();
                }
            }
        }

        void dispatcher.ILifeCyrcle.ContinueStopping()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsStop) return;

                if (_nodeObjectsManager.Count == 0 && _branchObjectsManager.Count == 0 && _nodeObjectsManager.DublicatCount == 0)
                {
                    _dispatcher.Process(manager.Dispatcher.Command.REMOVE_GLOBAL_OBJECTS);

                    _stateInformationManager.Set(information.State.Data.STOP);

                    if (Hellper.GetSystemMethod("Destroyed", _DOMInformation.CurrentObject.GetType(),
                    out System.Reflection.MethodInfo oSystemMethodConstruction))
                        oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);

                    if (_headerInformation.IsNodeObject())
                    {
                        ((manager.INodeObjects)_DOMInformation.ParentObject).
                            Remove(_DOMInformation.KeyObject);
                    }
                    else ((manager.IBranchObjects)_DOMInformation.ParentObject).
                            Remove(_DOMInformation.KeyObject);
                }
                else
                {
                    if (_nodeObjectsManager.Count == 0 && _nodeObjectsManager.DublicatCount == 0)
                    {
                        _branchObjectsManager.LifeCyrcle(Data.BEGIN_STOPPING);
                    }
                    else throw new System.Exception();
                }
            }
        }

        /// <summary>
        /// Запускает процесс уничтожения или информирует что обьект нужно уничтожить
        /// позже если в данный момент он находится в состоянии которое нельзя прервать,
        /// например подписка. Уничтожение всегда начинается с Board обьекта. В процессе
        /// уничтожения всех обьектов которые прихотятся уничтожаемому Board обьекту дочерними
        /// уже могут уничтожатся Node обьекты(которые были выставлены на отложеное уничтожие), 
        /// но процесс уничтжения прервется так как в начале нужно
        /// проинформировать Board обьект, а он уже в свою очередь сам остановит все дочерние Node Обьекты.
        /// Перед тем как уничтожаемый Node обьект проинформирует Board обьект он выставит флаг IsDestroy в true.
        /// тем самым запретит самому себе создания дочерних Node обьектов. 
        /// </summary>
        public void Destroy()
        {
            /*
            if (_stateInformation.IsStop)
                Exception($"Вы вызвали метод destroy() у обьекта {_headerInformation.Explorer}" +
                    "который уже был удален из системы.");
                    */
            if (_stateInformation.IsStop) return;

            //************************Грязная проверка**********************

            // Данный обьект может быть выставлен на отложеное уничтожение если 
            // в данный момент он находится в состоянии Subscribe.
            // После того как подписка будет окончена обьект начнет уничтожатся.
            if (_stateInformation.IsDeferredDestroying == false)
            {
                //...
            }
            // 1) Обьект уже начал свое уничтожние.
            // 2) Обьект уже начал процесс остановки.
            // 3) Обьект уже значет что после создания ему нужно начать свое уничтожение.
            else if (_stateInformation.IsDestroying || _stateInformation.IsStopping ||
                _stateInformation.IsDeferredDestroying)
                return;

            //**************************************************************

            lock (_stateInformation.Locker)
            {
                // Являлся ли текущий обьект запущеным.
                bool isStart = _stateInformation.IsStart;
                // запишим его текущее состояние.
                byte currentState = _stateInformation.CurrentState;

                // Обьект находится в состоянии подписки и мы в первый раз сообщаем
                // ему о том что после ее окончания нужно преступить к уничтожению.
                if (_stateInformation.IsSubscribe &&
                    _stateInformation.IsDeferredDestroying == false)
                {
                    if (_isCallDesctructionMethod == false)
                    {
                        _isCallDesctructionMethod = true;

                        if (Hellper.GetSystemMethod("Destruction", _DOMInformation.CurrentObject.GetType(),
                        out System.Reflection.MethodInfo oSystemMethodConstruction))
                            oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);
                    }

                    _stateInformationManager.DeferredDestroy();

                    return;
                }

                // Если была предпринята попытка уничтожения обьекта в момент
                // подписания, то ранее мы уже указали обьекту что нужно приступить
                // к уничтожению после того как придет увидомление об подписании.
                if (_stateInformation.IsSubscribe) return;

                //************************Чистая проверка****************************
                if (_stateInformation.IsDeferredDestroying == false)
                {
                    //...
                }
                else if (_stateInformation.IsDestroying || _stateInformation.IsStopping
                    || _stateInformation.IsDeferredDestroying)
                    return;
                //********************************************************************

                // Обьект либо уже указан как уничтожаемый, либо его уничтожение было отложено 
                // в связи с тем что он подписывался, и позже приступит к уничтожению.
                if (_stateInformation.IsDestroy == false)
                {
                    // Указывает обьекту, что мы находимя в процессе уничтожения и
                    // создовать новые node обьекты нельзя.
                    _stateInformationManager.Destroy();

                    if (_isCallDesctructionMethod == false)
                    {
                        _isCallDesctructionMethod = true;

                        if (Hellper.GetSystemMethod("Destruction", _DOMInformation.CurrentObject.GetType(),
                        out System.Reflection.MethodInfo oSystemMethodConstruction))
                            oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);
                    }

                    // Обьект уничтожается в системном потоке.
                    _DOMInformation.RootManager.ActionInvoke(() =>
                    {
                        if (isStart == false &&
                            _stateInformation.Interrupted == 0)
                        {
                            if (currentState == information.State.Data.OCCUPERENCE)
                            {
                                _stateInformationManager.SetInterrupted
                                    (information.State.Data.INTERRUPTED_CALL_DESTROY_IN_CONTRUCTION);
                            }
                            else if (currentState == information.State.Data.CONSTRUCTION)
                            {
                                _stateInformationManager.SetInterrupted
                                    (information.State.Data.INTERRUPTED_CALL_DESTROY_IN_CONTRUCTION);
                            }
                            else if (currentState == information.State.Data.CONFIGURATE)
                            {
                                _stateInformationManager.SetInterrupted
                                    (information.State.Data.INTERRUPTED_CALL_DESTROY_IN_CONFIGURATE);
                            }
                            else if (currentState == information.State.Data.START)
                            {
                                _stateInformationManager.SetInterrupted
                                    (information.State.Data.INTERRUPTED_CALL_DESTROY_IN_START);
                            }
                        }

                        // Если это Board обьект, то приступим к его уничтожению.
                        if (_headerInformation.IsBoard())
                        {
                            ((manager.IDispatcher)_DOMInformation.CurrentObject).
                                Process(manager.Dispatcher.Command.STOPPING_OBJECT);
                        }
                        // Процесс уничтожения может быть вызвано в Branch обьекте.
                        // Поэтому если его Node обьект еще не приступил к уничтожению,
                        // то переложим дальнейший процесс на него.
                        else if (_DOMInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            _DOMInformation.NodeObject.destroy();
                        }
                        // В данном месте можно окозатся как при первом вызове из Node обьекта,
                        // который в случае если Board обьект находится в рабочем состоянии продолжит
                        // уничтожение через него, так войти в данное место из Branch обьекта.
                        else if (_DOMInformation.NearBoardNodeObject.StateInformation.IsDestroy == false)
                        {
                            _DOMInformation.NearBoardNodeObject.destroy();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Продолжает процесс уничтожения.
        /// </summary>
        public void ContinueDestroy()
        {
            lock (_stateInformation.Locker)
            {
                if (_isCallDesctructionMethod == false)
                {
                    _isCallDesctructionMethod = true;

                    if (Hellper.GetSystemMethod("Destruction", _DOMInformation.CurrentObject.GetType(),
                    out System.Reflection.MethodInfo oSystemMethodConstruction))
                        oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);
                }

                if (_headerInformation.IsBranchObject())
                {
                    // Указываем обьект как Destroy, если он еще не был указан такавым.
                    // Что бы запретить создание узлов.
                    _stateInformationManager.Destroy();

                    // В данный блок кода мы можем попать только в Branch обьекте до того
                    // как его состояние сменилось c Subscribe на Start был вызван процесс уничтожения.
                    // Данный обьект был помечен как DefferedDestroy, поэтому когда запустится
                    // метод dispathcer.LifeCyrcle.Start бранч обьект в конце метода вызовет Destroying().
                    // В данный мемент Node обьект имеет состояния IsSubscribe. Поэтому если
                    // в Node обьект во время подписки так же не был выставлен флаг на отложеное 
                    // уничтожение, то вызовем для него destroy(), как было сказано ранее
                    // обьект Node в данный момент находится в состоянии Subscribe поэтому его нельзя начать 
                    // уничтожать. Когда обьект Node в методе dispathcer.LifeCyrcle.Start запустит этот же 
                    // метод для всех своих веток в конце он начнет процесс собсвеного уничтожения.
                    if (_DOMInformation.NodeObject.StateInformation.IsSubscribe)
                    {
                        if (_DOMInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            _DOMInformation.NodeObject.destroy();
                        }
                    }
                    // Branch обьект может уничтожиться только в составе своего Node обьекта.
                    // Тоесть Node обьект вызвет Destroying() для каждого Branch обьект,
                    // в этот момент он будет находится в состоянии 
                    else if (_DOMInformation.NodeObject.StateInformation.IsStopping)
                    {
                        // Данный обьект уже начал процесс остановки.
                        //if (_stateInformation.IsStopping) return;

                        ((manager.IDispatcher)_DOMInformation.CurrentObject).
                            Process(manager.Dispatcher.Command.STOPPING_OBJECT);
                    }
                }
                else if (_headerInformation.IsNodeObject())
                {
                    // Выставим узлу Destroy. Запретим создание новых узлов.
                    _stateInformationManager.Destroy();

                    // Если узел подписывается, то обьект начнет уничтожатся после того
                    if (_stateInformation.IsSubscribe)
                    {
                        _stateInformationManager.DeferredDestroy();

                        return;
                    }
                    else if (_headerInformation.IsBoard() == false &&
                        _DOMInformation.NearBoardNodeObject.StateInformation.IsDestroy == false)
                    {
                        _DOMInformation.NearBoardNodeObject.destroy();
                    }
                    else ((manager.IDispatcher)_DOMInformation.CurrentObject).
                            Process(manager.Dispatcher.Command.STOPPING_OBJECT);
                }
            }
        }
        private int _index = 0;

        public bool TryIncrement()
        {
            if (_stateInformation.IsDestroying || _stateInformation.IsDeferredDestroying || _stateInformation.IsDestroy) return false;

            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroying || _stateInformation.IsDeferredDestroying || _stateInformation.IsDestroy) return false;

                SystemInformation("Инкрементируем(закрываем возможность уничтожения)", ConsoleColor.Yellow);
                _index++;

                return true;
            }
        }

        public void Decrement()
        {
            lock (_stateInformation.Locker)
            {
                SystemInformation("Декриментируем(открываем возможность уничтожения)", ConsoleColor.Yellow);
                if ((--_index) <= 0 && _stateInformation.IsDestroying)
                {
                    SystemInformation("Обьект уничтожается(открываем возомжность долнейшего уничтожения.)", ConsoleColor.Yellow);
                    ((manager.IDispatcher)_DOMInformation.CurrentObject).
                        Process(manager.Dispatcher.Command.STOPPING_OBJECT);
                }
            }
        }
    }
}