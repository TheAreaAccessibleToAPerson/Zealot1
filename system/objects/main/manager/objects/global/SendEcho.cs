namespace Butterfly.system.objects.main
{
    public sealed class SendEcho
    {
        public static ulong s_uniqueID = 0;
    }
    public sealed class SendEcho_1_0<T> : Redirect, IInput<T>, IReturn, IInputConnected
    {
        private IInput<T, IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_2_0<T1, T2> : Redirect, IInput<T1, T2>, IReturn, IInputConnected
    {
        private IInput<T1, T2, IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_0<T1, T2, T3> : Redirect, IInput<T1, T2, T3>, IReturn, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_4_0<T1, T2, T3, T4> : Redirect, IInput<T1, T2, T3, T4>, IReturn, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_5_0<T1, T2, T3, T4, T5> : Redirect, IInput<T1, T2, T3, T4, T5>, IReturn, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_0 : Redirect, IInput, IReturn, IInputConnected
    {
        private IInput<IReturn> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_0(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn.To()
            => Input.To();

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_1<R> : Redirect<R>, IInput, IReturn<R>, IInputConnected
    {
        private IInput<IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_2<R1, R2> : Redirect<R1, R2>, IInput, IReturn<R1, R2>, IInputConnected
    {
        private IInput<IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_3<R1, R2, R3> : Redirect<R1, R2, R3>, IInput, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_3(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_4<R1, R2, R3, R4> : Redirect<R1, R2, R3, R4>, IInput, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_0_5<R1, R2, R3, R4, R5> : Redirect<R1, R2, R3, R4, R5>, IInput, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_0_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput.To()
            => _connect.To(this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }

    public sealed class SendEcho_1_1<T, R> : Redirect<R>, IInput<T>, IReturn<R>, IInputConnected
    {
        private IInput<T, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }

    public sealed class SendEcho_2_1<T1, T2, R> : Redirect<R>, IInput<T1, T2>, IReturn<R>, IInputConnected
    {
        private IInput<T1, T2, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_2_2<T1, T2, R1, R2> : Redirect<R1, R2>, IInput<T1, T2>, IReturn<R1, R2>, IInputConnected
    {
        private IInput<T1, T2, IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_2_3<T1, T2, R1, R2, R3> : Redirect<R1, R2, R3>, IInput<T1, T2>, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<T1, T2, IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_3(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_2_4<T1, T2, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<T1, T2, IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_2_5<T1, T2, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<T1, T2, IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_2_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2>.To(T1 value1, T2 value2)
            => _connect.To(value1, value2, this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_1<T1, T2, T3, R> : Redirect<R>, IInput<T1, T2, T3>, IReturn<R>, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_2<T1, T2, T3, R1, R2> : Redirect<R1, R2>, IInput<T1, T2, T3>, IReturn<R1, R2>, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_3<T1, T2, T3, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3>, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_3(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_4<T1, T2, T3, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_3_5<T1, T2, T3, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_3_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3)
            => _connect.To(value1, value2, value3, this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }

    public sealed class SendEcho_4_1<T1, T2, T3, T4, R> : Redirect<R>, IInput<T1, T2, T3, T4>, IReturn<R>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_4_2<T1, T2, T3, T4, R1, R2> : Redirect<R1, R2>, IInput<T1, T2, T3, T4>, IReturn<R1, R2>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_5_2<T1, T2, T3, T4, T5, R1, R2> : Redirect<R1, R2>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_4_3<T1, T2, T3, T4, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong s_UniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_3(IInformation information)
            : base(information)
                => s_UniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => s_UniqueID;
    }
    public sealed class SendEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_3(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_4_4<T1, T2, T3, T4, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_4_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4)
            => _connect.To(value1, value2, value3, value4, this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_5_1<T1, T2, T3, T4, T5, R> : Redirect<R>, IInput<T1, T2, T3, T4, T5>, IReturn<R>, IInputConnected
    {
        private IInput<T1, T2, T3, T4, T5, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T1, T2, T3, T4, T5, IReturn<R>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_5_1(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
            => _connect.To(value1, value2, value3, value4, value5, this);

        void IReturn<R>.To(R value)
            => Input.To(value);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_1_2<T, R1, R2> : Redirect<R1, R2>, IInput<T>, IReturn<R1, R2>, IInputConnected
    {
        private IInput<T, IReturn<R1, R2>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R1, R2>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_2(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_1_3<T, R1, R2, R3> : Redirect<R1, R2, R3>, IInput<T>, IReturn<R1, R2, R3>, IInputConnected
    {
        private IInput<T, IReturn<R1, R2, R3>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R1, R2, R3>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_3(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_1_4<T, R1, R2, R3, R4>
        : Redirect<R1, R2, R3, R4>, IInput<T>, IReturn<R1, R2, R3, R4>, IInputConnected
    {
        private IInput<T, IReturn<R1, R2, R3, R4>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R1, R2, R3, R4>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_4(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        public ulong GetUnieueID()
            => _uniqueID;
    }
    public sealed class SendEcho_1_5<T, R1, R2, R3, R4, R5>
        : Redirect<R1, R2, R3, R4, R5>, IInput<T>, IReturn<R1, R2, R3, R4, R5>, IInputConnected
    {
        private IInput<T, IReturn<R1, R2, R3, R4, R5>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R1, R2, R3, R4, R5>>>
                (inputConnect, ref _connect, GetType());

        public SendEcho_1_5(IInformation information)
            : base(information)
                => _uniqueID = SendEcho.s_uniqueID++;

        void IInput<T>.To(T value)
            => _connect.To(value, this);

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        public ulong GetUnieueID()
            => _uniqueID;
    }
}