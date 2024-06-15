namespace Butterfly.system.objects.main
{
    public sealed class FuncObjectPoll<R> : Redirect<R>, IInput, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<R> _func;
        private readonly System.Action _safe;

        public FuncObjectPoll(ref IInput input, System.Func<R> func, IInformation information, System.Action safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput.To()
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func());
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func());

                    DecrementEvent();
                });
            }
            else _safe.Invoke();
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }

    public sealed class FuncObjectPoll<T, R> : Redirect<R>, IInput<T>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<T, R> _func;
        private readonly System.Action<T> _safe;

        public FuncObjectPoll(ref IInput<T> input, System.Func<T, R> func,
            IInformation information, System.Action<T> safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput<T>.To(T value)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func(value));
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func(value));

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class FuncObjectPoll<T1, T2, R> : Redirect<R>, IInput<T1, T2>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<T1, T2, R> _func;
        private readonly System.Action<T1, T2> _safe;

        public FuncObjectPoll(ref IInput<T1, T2> input, System.Func<T1, T2, R> func,
            IInformation information, System.Action<T1, T2> safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2>.To(T1 value1, T2 value2)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2));
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2));

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class FuncObjectPoll<T1, T2, T3, R> : Redirect<R>, IInput<T1, T2, T3>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<T1, T2, T3, R> _func;
        private readonly System.Action<T1, T2, T3> _safe;

        public FuncObjectPoll(ref IInput<T1, T2, T3> input, System.Func<T1, T2, T3, R> func,
            IInformation information, System.Action<T1, T2, T3> safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3));
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3));

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class FuncObjectPoll<T1, T2, T3, T4, R> : Redirect<R>, IInput<T1, T2, T3, T4>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<T1, T2, T3, T4, R> _func;
        private readonly System.Action<T1, T2, T3, T4> _safe;

        public FuncObjectPoll(ref IInput<T1, T2, T3, T4> input, System.Func<T1, T2, T3, T4, R> func,
            IInformation information, System.Action<T1, T2, T3, T4> safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3, value4));
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3, value4));

                    DecrementEvent();
                });
            }
            else _safe.Invoke(value1, value2, value3, value4);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action>>
                (inputConnect, ref _connect, GetType());
    }
    public sealed class FuncObjectPoll<T1, T2, T3, T4, T5, R> : Redirect<R>, IInput<T1, T2, T3, T4, T5>, IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action> _connect;

        private readonly System.Func<T1, T2, T3, T4, T5, R> _func;
        private readonly System.Action<T1, T2, T3, T4, T5> _safe;

        public FuncObjectPoll(ref IInput<T1, T2, T3, T4, T5> input, System.Func<T1, T2, T3, T4, T5, R> func,
            IInformation information, System.Action<T1, T2, T3, T4, T5> safe)
            : base(information)
        {
            _func = func;
            input = this;

            _safe = safe;
        }

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (_safe == null)
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3, value4, value5));
                });
            }
            else if (TryIncrementEvent())
            {
                _connect.To(() =>
                {
                    Input.To(_func(value1, value2, value3, value4, value5));

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