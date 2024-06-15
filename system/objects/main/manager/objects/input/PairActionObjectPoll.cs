namespace Butterfly.system.objects.main
{
    public sealed class PairActionObjectPoll_0_1<R>
        : Redirect<R>, IInput, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<IReturn<R>> _action;
        private readonly System.Action _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_0_1(ref IInput Input,
            System.Action<IReturn<R>> action, IInformation information, System.Action safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_0_2<R1, R2>
        : Redirect<R1, R2>, IInput, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<IReturn<R1, R2>> _action;
        private readonly System.Action _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_0_2(ref IInput Input, System.Action<IReturn<R1, R2>> action,
            IInformation information, System.Action safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_0_3<R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<IReturn<R1, R2, R3>> _action;
        private readonly System.Action _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_0_3(ref IInput Input, System.Action<IReturn<R1, R2, R3>> action,
            IInformation information, System.Action safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_0_4<R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_0_4(ref IInput Input, System.Action<IReturn<R1, R2, R3, R4>> action,
            IInformation information, System.Action safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_0_5(ref IInput Input, System.Action<IReturn<R1, R2, R3, R4, R5>> action,
            IInformation information, System.Action safe)
            : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_1_1<T, R>
        : Redirect<R>, IInput<T>, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T, IReturn<R>> _action;
        private readonly System.Action<T> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_1_1(ref IInput<T> Input, System.Action<T, IReturn<R>> action,
            IInformation information, System.Action<T> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_2_1<T1, T2, R>
        : Redirect<R>, IInput<T1, T2>, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, IReturn<R>> _action;
        private readonly System.Action<T1, T2> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_2_1(ref IInput<T1, T2> Input, System.Action<T1, T2, IReturn<R>> action,
            IInformation information, System.Action<T1, T2> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_3_1<T1, T2, T3, R>
        : Redirect<R>, IInput<T1, T2, T3>, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, IReturn<R>> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_3_1(ref IInput<T1, T2, T3> Input, System.Action<T1, T2, T3, IReturn<R>> action,
            IInformation information, System.Action<T1, T2, T3> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_4_1<T1, T2, T3, T4, R>
        : Redirect<R>, IInput<T1, T2, T3, T4>, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, IReturn<R>> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_4_1(ref IInput<T1, T2, T3, T4> Input, System.Action<T1, T2, T3, T4,
            IReturn<R>> action, IInformation information, System.Action<T1, T2, T3, T4> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5>, IReturn<R>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R>> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_5_1(ref IInput<T1, T2, T3, T4, T5> Input,
            System.Action<T1, T2, T3, T4, T5, IReturn<R>> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_1_2<T, R1, R2>
        : Redirect<R1, R2>, IInput<T>, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T, IReturn<R1, R2>> _action;
        private readonly System.Action<T> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_1_2(ref IInput<T> Input, System.Action<T, IReturn<R1, R2>> action,
            IInformation information, System.Action<T> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_2_2<T1, T2, R1, R2>
        : Redirect<R1, R2>, IInput<T1, T2>, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, IReturn<R1, R2>> _action;
        private readonly System.Action<T1, T2> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_2_2(ref IInput<T1, T2> Input, System.Action<T1, T2, IReturn<R1, R2>> action,
            IInformation information, System.Action<T1, T2> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_3_2<T1, T2, T3, R1, R2>
        : Redirect<R1, R2>, IInput<T1, T2, T3>, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, IReturn<R1, R2>> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_3_2(ref IInput<T1, T2, T3> Input, System.Action<T1, T2, T3, IReturn<R1, R2>> action,
            IInformation information, System.Action<T1, T2, T3> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_4_2<T1, T2, T3, T4, R1, R2>
        : Redirect<R1, R2>, IInput<T1, T2, T3, T4>, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2>> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_4_2(ref IInput<T1, T2, T3, T4> Input, System.Action<T1, T2, T3, T4, IReturn<R1, R2>> action,
            IInformation information, System.Action<T1, T2, T3, T4> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>
        : Redirect<R1, R2>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_5_2(ref IInput<T1, T2, T3, T4, T5> Input,
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_1_3<T, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T>, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T, IReturn<R1, R2, R3>> _action;
        private readonly System.Action<T> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_1_3(ref IInput<T> Input, System.Action<T, IReturn<R1, R2, R3>> action,
            IInformation information, System.Action<T> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_2_3<T1, T2, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2>, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, IReturn<R1, R2, R3>> _action;
        private readonly System.Action<T1, T2> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_2_3(ref IInput<T1, T2> Input, System.Action<T1, T2, IReturn<R1, R2, R3>> action,
            IInformation information, System.Action<T1, T2> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_3_3<T1, T2, T3, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3>, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3>> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_3_3(ref IInput<T1, T2, T3> Input, System.Action<T1, T2, T3, IReturn<R1, R2, R3>> action,
            IInformation information, System.Action<T1, T2, T3> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_4_3<T1, T2, T3, T4, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_4_3(ref IInput<T1, T2, T3, T4> Input, System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> action,
            IInformation information, System.Action<T1, T2, T3, T4> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_5_3(ref IInput<T1, T2, T3, T4, T5> Input,
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_1_4<T, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T, IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action<T> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_1_4(ref IInput<T> Input, System.Action<T, IReturn<R1, R2, R3, R4>> action,
            IInformation information, System.Action<T> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_2_4<T1, T2, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action<T1, T2> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_2_4(ref IInput<T1, T2> Input, System.Action<T1, T2, IReturn<R1, R2, R3, R4>> action,
            IInformation information, System.Action<T1, T2> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_3_4<T1, T2, T3, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_3_4(ref IInput<T1, T2, T3> Input, System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> action,
            IInformation information, System.Action<T1, T2, T3> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_4_4<T1, T2, T3, T4, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_4_4(ref IInput<T1, T2, T3, T4> Input,
            System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> action, IInformation information,
                System.Action<T1, T2, T3, T4> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_5_4(ref IInput<T1, T2, T3, T4, T5> Input,
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T, IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action<T> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_1_5(ref IInput<T> Input, System.Action<T, IReturn<R1, R2, R3, R4, R5>> action,
            IInformation information, System.Action<T> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_2_5<T1, T2, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action<T1, T2> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_2_5(ref IInput<T1, T2> Input, System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> action,
            IInformation information, System.Action<T1, T2> safe)
                : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_3_5<T1, T2, T3, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action<T1, T2, T3> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_3_5(ref IInput<T1, T2, T3> Input,
            System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> action, IInformation information,
                System.Action<T1, T2, T3> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_4_5(ref IInput<T1, T2, T3, T4> Input,
            System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> action, IInformation information,
                System.Action<T1, T2, T3, T4> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> _action;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObjectPoll_5_5(ref IInput<T1, T2, T3, T4, T5> Input,
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action, IInformation information,
                System.Action<T1, T2, T3, T4, T5> safe)
                    : base(information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;

            _safe = safe;
        }

        public ulong GetUnieueID()
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    _action.Invoke(value1, value2, value3, value4, value5, this);

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4, value5);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
}