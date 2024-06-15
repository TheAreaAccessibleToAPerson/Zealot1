namespace Butterfly.system.objects.main
{
    public sealed class ListenEcho_0_0 : Redirect<IReturn>, IInput<IReturn>, IInputConnect
    {
        public ListenEcho_0_0(IInformation information) 
            : base (information) {}

        void IInput<IReturn>.To(IReturn returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_0<T> : Redirect<T, IReturn>, IInput<T, IReturn>, IInputConnect
    {
        public ListenEcho_1_0(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn>.To(T value, IReturn returnValue)   
            => Input.To(value, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_0<T1, T2> : Redirect<T1, T2, IReturn>, IInput<T1, T2, IReturn>, IInputConnect
    {
        public ListenEcho_2_0(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn>.To(T1 value1, T2 value2, IReturn returnValue)   
            => Input.To(value1, value2, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_0<T1, T2, T3> : Redirect<T1, T2, T3, IReturn>, IInput<T1, T2, T3, IReturn>, IInputConnect
    {
        public ListenEcho_3_0(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn>.To(T1 value1, T2 value2, T3 value3, IReturn returnValue)   
            => Input.To(value1, value2, value3, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_0<T1, T2, T3, T4> : Redirect<T1, T2, T3, T4, IReturn>, IInput<T1, T2, T3, T4, IReturn>, IInputConnect
    {
        public ListenEcho_4_0(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn>.To(T1 value1, T2 value2, T3 value3, T4 value4, IReturn returnValue)   
            => Input.To(value1, value2, value3, value4, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_0<T1, T2, T3, T4, T5> : Redirect<T1, T2, T3, T4, T5, IReturn>, IInput<T1, T2, T3, T4, T5, IReturn>, IInputConnect
    {
        public ListenEcho_5_0(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn returnValue)   
            => Input.To(value1, value2, value3, value4, value5, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_0_1<R> : Redirect<IReturn<R>>, IInput<IReturn<R>>, IInputConnect
    {
        public ListenEcho_0_1(IInformation information) 
            : base (information) {}

        void IInput<IReturn<R>>.To(IReturn<R> returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_0_2<R1, R2> : Redirect<IReturn<R1, R2>>, IInput<IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_0_2(IInformation information) 
            : base (information) {}

        void IInput<IReturn<R1, R2>>.To(IReturn<R1, R2> returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_0_3<R1, R2, R3> : Redirect<IReturn<R1, R2, R3>>, IInput<IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_0_3(IInformation information) 
            : base (information) {}

        void IInput<IReturn<R1, R2, R3>>.To(IReturn<R1, R2, R3> returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_0_4<R1, R2, R3, R4> : Redirect<IReturn<R1, R2, R3, R4>>, IInput<IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_0_4(IInformation information) 
            : base (information) {}

        void IInput<IReturn<R1, R2, R3, R4>>.To(IReturn<R1, R2, R3, R4> returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_0_5<R1, R2, R3, R4, R5> : Redirect<IReturn<R1, R2, R3, R4, R5>>, IInput<IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_0_5(IInformation information) 
            : base (information) {}

        void IInput<IReturn<R1, R2, R3, R4, R5>>.To(IReturn<R1, R2, R3, R4, R5> returnValue)   
            => Input.To(returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_1<T, R> : Redirect<T, IReturn<R>>, IInput<T, IReturn<R>>, IInputConnect
    {
        public ListenEcho_1_1(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn<R>>.To(T value1, IReturn<R> returnValue)   
            => Input.To(value1, returnValue);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_1<T1, T2, R> 
        : Redirect<T1, T2, IReturn<R>>, IInput<T1, T2, IReturn<R>>, IInputConnect
    {
        public ListenEcho_2_1(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn<R>>.To
            (T1 value1, T2 value2, IReturn<R> returnValues)   
                => Input.To(value1, value2, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_2<T1, T2, R1, R2> 
        : Redirect<T1, T2, IReturn<R1, R2>>, IInput<T1, T2, IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_2_2(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn<R1, R2>>.To
            (T1 value1, T2 value2, IReturn<R1, R2> returnValues)   
                => Input.To(value1, value2, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_3<T1, T2, R1, R2, R3> 
        : Redirect<T1, T2, IReturn<R1, R2, R3>>, IInput<T1, T2, IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_2_3(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn<R1, R2, R3>>.To
            (T1 value1, T2 value2, IReturn<R1, R2, R3> returnValues)   
                => Input.To(value1, value2, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_4<T1, T2, R1, R2, R3, R4> 
        : Redirect<T1, T2, IReturn<R1, R2, R3, R4>>, IInput<T1, T2, IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_2_4(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn<R1, R2, R3, R4>>.To
            (T1 value1, T2 value2, IReturn<R1, R2, R3, R4> returnValues)   
                => Input.To(value1, value2, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_2_5<T1, T2, R1, R2, R3, R4, R5> 
        : Redirect<T1, T2, IReturn<R1, R2, R3, R4, R5>>, IInput<T1, T2, IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_2_5(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, IReturn<R1, R2, R3, R4, R5>>.To
            (T1 value1, T2 value2, IReturn<R1, R2, R3, R4, R5> returnValues)   
                => Input.To(value1, value2, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_1<T1, T2, T3, R> 
        : Redirect<T1, T2, T3, IReturn<R>>, IInput<T1, T2, T3, IReturn<R>>, IInputConnect
    {
        public ListenEcho_3_1(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn<R>>.To
            (T1 value1, T2 value2, T3 value3, IReturn<R> returnValues)   
                => Input.To(value1, value2, value3, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_2<T1, T2, T3, R1, R2> 
        : Redirect<T1, T2, T3, IReturn<R1, R2>>, IInput<T1, T2, T3, IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_3_2(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn<R1, R2>>.To
            (T1 value1, T2 value2, T3 value3, IReturn<R1, R2> returnValues)   
                => Input.To(value1, value2, value3, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_3<T1, T2, T3, R1, R2, R3> 
        : Redirect<T1, T2, T3, IReturn<R1, R2, R3>>, IInput<T1, T2, T3, IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_3_3(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn<R1, R2, R3>>.To
            (T1 value1, T2 value2, T3 value3, IReturn<R1, R2, R3> returnValues)   
                => Input.To(value1, value2, value3, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_4<T1, T2, T3, R1, R2, R3, R4> 
        : Redirect<T1, T2, T3, IReturn<R1, R2, R3, R4>>, IInput<T1, T2, T3, IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_3_4(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn<R1, R2, R3, R4>>.To
            (T1 value1, T2 value2, T3 value3, IReturn<R1, R2, R3, R4> returnValues)   
                => Input.To(value1, value2, value3, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5> 
        : Redirect<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>>, IInput<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_3_5(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>>.To
            (T1 value1, T2 value2, T3 value3, IReturn<R1, R2, R3, R4, R5> returnValues)   
                => Input.To(value1, value2, value3, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_1<T1, T2, T3, T4, R> 
        : Redirect<T1, T2, T3, T4, IReturn<R>>, IInput<T1, T2, T3, T4, IReturn<R>>, IInputConnect
    {
        public ListenEcho_4_1(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn<R>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, IReturn<R> returnValues)   
                => Input.To(value1, value2, value3, value4, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_2<T1, T2, T3, T4, R1, R2> 
        : Redirect<T1, T2, T3, T4, IReturn<R1, R2>>, IInput<T1, T2, T3, T4, IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_4_2(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn<R1, R2>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, IReturn<R1, R2> returnValues)   
                => Input.To(value1, value2, value3, value4, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_3<T1, T2, T3, T4, R1, R2, R3> 
        : Redirect<T1, T2, T3, T4, IReturn<R1, R2, R3>>, IInput<T1, T2, T3, T4, IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_4_3(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn<R1, R2, R3>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, IReturn<R1, R2, R3> returnValues)   
                => Input.To(value1, value2, value3, value4, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4> 
        : Redirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>>, IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_4_4(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, IReturn<R1, R2, R3, R4> returnValues)   
                => Input.To(value1, value2, value3, value4, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5> 
        : Redirect<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>>, IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_4_5(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, IReturn<R1, R2, R3, R4, R5> returnValues)   
                => Input.To(value1, value2, value3, value4, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_1<T1, T2, T3, T4, T5, R> 
        : Redirect<T1, T2, T3, T4, T5, IReturn<R>>, IInput<T1, T2, T3, T4, T5, IReturn<R>>, IInputConnect
    {
        public ListenEcho_5_1(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn<R>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn<R> returnValues)   
                => Input.To(value1, value2, value3, value4, value5, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_2<T1, T2, T3, T4, T5, R1, R2> 
        : Redirect<T1, T2, T3, T4, T5, IReturn<R1, R2>>, IInput<T1, T2, T3, T4, T5, IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_5_2(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn<R1, R2>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn<R1, R2> returnValues)   
                => Input.To(value1, value2, value3, value4, value5, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3> 
        : Redirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>>, IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_5_3(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn<R1, R2, R3> returnValues)   
                => Input.To(value1, value2, value3, value4, value5, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4> 
        : Redirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>>, IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_5_4(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn<R1, R2, R3, R4> returnValues)   
                => Input.To(value1, value2, value3, value4, value5, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5> 
        : Redirect<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>>, IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_5_5(IInformation information) 
            : base (information) {}

        void IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>>.To
            (T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IReturn<R1, R2, R3, R4, R5> returnValues)   
                => Input.To(value1, value2, value3, value4, value5, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_2<T, R1, R2> 
        : Redirect<T, IReturn<R1, R2>>, IInput<T, IReturn<R1, R2>>, IInputConnect
    {
        public ListenEcho_1_2(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn<R1, R2>>.To(T value1, IReturn<R1, R2> returnValues)   
            => Input.To(value1, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_3<T, R1, R2, R3> 
        : Redirect<T, IReturn<R1, R2, R3>>, IInput<T, IReturn<R1, R2, R3>>, IInputConnect
    {
        public ListenEcho_1_3(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn<R1, R2, R3>>.To(T value1, IReturn<R1, R2, R3> returnValues)   
            => Input.To(value1, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_4<T, R1, R2, R3, R4> 
        : Redirect<T, IReturn<R1, R2, R3, R4>>, IInput<T, IReturn<R1, R2, R3, R4>>, IInputConnect
    {
        public ListenEcho_1_4(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn<R1, R2, R3, R4>>.To(T value1, IReturn<R1, R2, R3, R4> returnValues)   
            => Input.To(value1, returnValues);

        object IInputConnect.GetConnect() => this;
    }
    public sealed class ListenEcho_1_5<T, R1, R2, R3, R4, R5> 
        : Redirect<T, IReturn<R1, R2, R3, R4, R5>>, IInput<T, IReturn<R1, R2, R3, R4, R5>>, IInputConnect
    {
        public ListenEcho_1_5(IInformation information) 
            : base (information) {}

        void IInput<T, IReturn<R1, R2, R3, R4, R5>>.To(T value1, IReturn<R1, R2, R3, R4, R5> returnValues)   
            => Input.To(value1, returnValues);

        object IInputConnect.GetConnect() => this;
    }
}