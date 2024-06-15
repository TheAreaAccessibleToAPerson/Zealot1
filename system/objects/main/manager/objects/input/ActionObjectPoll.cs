namespace Butterfly.system.objects.main
{
    public class ActionObjectPoll
        : InformationGlobalObject, IInput, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action _action;

        private readonly System.Action _safe;

        public ActionObjectPoll(ref IInput input, System.Action action, 
            IInformation information, System.Action safe = null) 
            : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke();
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke();

                    DecrementEvent();
                });
            } 
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T> 
        : InformationGlobalObject, IInput<T>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T> _action;

        private readonly System.Action<T> _safe;

        public ActionObjectPoll(ref IInput<T> input, System.Action<T> action, 
            IInformation information, System.Action<T> safe = null) 
            : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2> 
        : InformationGlobalObject, IInput<T1, T2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2> _action;

        private readonly System.Action<T1, T2> _safe;

        public ActionObjectPoll(ref IInput<T1, T2> input, System.Action<T1, T2> action, 
            IInformation information, System.Action<T1, T2> safe = null) 
            : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3> 
        : InformationGlobalObject, IInput<T1, T2, T3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        public ActionObjectPoll(ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3> action, 
            IInformation information, System.Action<T1, T2, T3> safe = null) 
            : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3, T4> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4> action, 
            IInformation information, System.Action<T1, T2, T3, T4> safe = null) 
            : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5> input, 
            System.Action<T1, T2, T3, T4, T5> action, IInformation information, 
                System.Action<T1, T2, T3, T4, T5> safe = null) 
                    : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3, T4, T5>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6> _action;
        private readonly System.Action<T1, T2, T3, T4, T5, T6> _safe;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6> input, 
            System.Action<T1, T2, T3, T4, T5, T6> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5, T6> safe = null) 
                    : base (information)
        {
            _action = action;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3, T4, T5, T6>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            if (_safe == null)
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6);

                    DecrementEvent();
                });
            } 
            else _safe.Invoke(value1, value2, value3, value4, value5, value6);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7> action, IInformation information)
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11, T12 value12)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11, value12);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11, value12, value13);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11, value12, value13, value14);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11, value12, value13, value14, value15);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public class ActionObjectPoll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> 
        : InformationGlobalObject, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> _action;

        public ActionObjectPoll(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, IInformation information) 
            : base (information)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
            T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)
        {
            if (TryIncrementEvent())
            {
                _connect.To(() => 
                {   
                    _action.Invoke(value1, value2, value3, value4, value5, value6, 
                        value7, value8, value9, value10, value11, value12, value13, value14, value15, value16);

                    DecrementEvent();
                });
            }
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
}