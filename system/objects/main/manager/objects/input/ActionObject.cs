namespace Butterfly.system.objects.main
{
    public class ActionObject : IInput
    {
        private readonly System.Action _action;

        public ActionObject(ref IInput input, System.Action action) 
        {
            _action = action;
            input = this;
        }

        void IInput.To() => _action.Invoke();
    }

    public class ActionObject<T> : IInput<T>
    {
        private readonly System.Action<T> _action;

        public ActionObject(ref IInput<T> input, System.Action<T> action) 
        {
            _action = action;
            input = this;
        }

        void IInput<T>.To(T value) => _action.Invoke(value);
    }

    public sealed class ActionObject<T1, T2> : IInput<T1, T2>
    {
        private readonly System.Action<T1, T2> _action;

        public ActionObject(ref IInput<T1, T2> input, System.Action<T1, T2> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2>.To(T1 value1, T2 value2) => _action.Invoke(value1, value2);
    }

    public sealed class ActionObject<T1, T2, T3> : IInput<T1, T2, T3>
    {
        private readonly System.Action<T1, T2, T3> _action;

        public ActionObject(ref IInput<T1, T2, T3> input, System.Action<T1, T2, T3> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3>.To
            (T1 value1, T2 value2, T3 value3)
                => _action.Invoke(value1, value2, value3);
    }
    public sealed class ActionObject<T1, T2, T3, T4> : IInput<T1, T2, T3, T4>
    {
        private readonly System.Action<T1, T2, T3, T4> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4> input, System.Action<T1, T2, T3, T4> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4>.To
            (T1 value1, T2 value2, T3 value3, T4 value4)
                => _action.Invoke(value1, value2, value3, value4);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5> : IInput<T1, T2, T3, T4, T5>
    {
        private readonly System.Action<T1, T2, T3, T4, T5> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5> input, System.Action<T1, T2, T3, T4, T5> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
                => _action.Invoke(value1, value2, value3, value4, value5);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6> : IInput<T1, T2, T3, T4, T5, T6>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6> input, System.Action<T1, T2, T3, T4, T5, T6> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
                => _action.Invoke(value1, value2, value3, value4, value5, value6);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7> 
        : IInput<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, 
                    value7);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, 
                    value7, value8);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, 
                    value8, value9);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, 
                    value9, value10);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11, T12 value12)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11, value12);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11, value12, value13);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11, value12, value13, value14);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11, value12, value13, value14, value15);
    }
    public sealed class ActionObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> 
        : IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> _action;

        public ActionObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> input, 
            System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, 
                T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)
                => _action.Invoke(value1, value2, value3, value4, value5, value6, value7, value8, value9, 
                    value10, value11, value12, value13, value14, value15, value16);
    }
}