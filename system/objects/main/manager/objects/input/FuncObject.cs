namespace Butterfly.system.objects.main
{
    public sealed class FuncObject<R> : Redirect<R>, IInput
    {
        private readonly System.Func<R> _func;

        public FuncObject(ref IInput input, System.Func<R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To()
            => Input.To(_func());
    }
    public sealed class FuncObject<T, R> : Redirect<R>, IInput<T>
    {
        private readonly System.Func<T, R> _func;

        public FuncObject(ref IInput<T> input, System.Func<T, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T value)
            => Input.To(_func(value));
    }

    public sealed class FuncObject<T1, T2, R> : Redirect<R>, IInput<T1, T2>
    {
        private readonly System.Func<T1, T2, R> _func;

        public FuncObject(ref IInput<T1, T2> input, System.Func<T1, T2, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2)
            => Input.To(_func(value1, value2));
    }

    public sealed class FuncObject<T1, T2, T3, R> : Redirect<R>, IInput<T1, T2, T3>
    {
        private readonly System.Func<T1, T2, T3, R> _func;

        public FuncObject(ref IInput<T1, T2, T3> input, System.Func<T1, T2, T3, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3)
            => Input.To(_func(value1, value2, value3));
    }

    public sealed class FuncObject<T1, T2, T3, T4, R>
        : Redirect<R>, IInput<T1, T2, T3, T4>
    {
        private readonly System.Func<T1, T2, T3, T4, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4> input, System.Func<T1, T2, T3, T4, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4)
            => Input.To(_func(value1, value2, value3, value4));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5> input, System.Func<T1, T2, T3, T4, T5, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => Input.To(_func(value1, value2, value3, value4, value5));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, T6, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6> input,
            System.Func<T1, T2, T3, T4, T5, T6, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
            => Input.To(_func(value1, value2, value3, value4, value5, value6));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, T6, T7, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7> input,
            System.Func<T1, T2, T3, T4, T5, T6, T7, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
            => Input.To(_func(value1, value2, value3, value4, value5, value6, value7));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, T6, T7, T8, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8> input,
            System.Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
            => Input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9> input,
            System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
            => Input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private readonly System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> input,
            System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> func, IInformation information)
            : base(information)
        {
            _func = func;
            input = this;
        }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
            => Input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
    }
}