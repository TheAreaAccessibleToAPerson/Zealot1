namespace Butterfly.system.objects.main
{
    public sealed class ListenMessage<T> : Redirect<T>, IInput<T>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T value) => Input.To(value);
    }
    public sealed class ListenMessage<T1, T2> : Redirect<T1, T2>, IInput<T1, T2>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T1 value1, T2 value2) 
            => Input.To(value1, value2);
    }
    public sealed class ListenMessage<T1, T2, T3> : Redirect<T1, T2, T3>, IInput<T1, T2, T3>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T1 value1, T2 value2, T3 value3) 
            => Input.To(value1, value2, value3);
    }
    public sealed class ListenMessage<T1, T2, T3, T4> : Redirect<T1, T2, T3, T4>, IInput<T1, T2, T3, T4>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => Input.To(value1, value2, value3, value4);
    }
    public sealed class ListenMessage<T1, T2, T3, T4, T5> : Redirect<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => Input.To(value1, value2, value3, value4, value5);
    }
    public sealed class ListenMessage<T1, T2, T3, T4, T5, T6> 
        : Redirect<T1, T2, T3, T4, T5, T6>, IInput<T1, T2, T3, T4, T5, T6>
    {
        public ListenMessage(IInformation information) 
            : base (information){}

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) 
            => Input.To(value1, value2, value3, value4, value5, value6);
    }
}