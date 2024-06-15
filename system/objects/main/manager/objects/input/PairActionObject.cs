namespace Butterfly.system.objects.main
{
    public class PairActionObject
    {
        public static ulong s_UniqueID = 0;
    }
    public sealed class PairActionObject_0_1<R> 
        : Redirect<R>, IInput, IReturn<R>
    {
        private readonly System.Action<IReturn<R>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_0_1(ref IInput Input, 
            System.Action<IReturn<R>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R>.To(R value)
            => Input.To(value);

        void IInput.To() 
            => _action.Invoke(this);
    }
    public sealed class PairActionObject_0_2<R1, R2> 
        : Redirect<R1, R2>, IInput, IReturn<R1, R2>
    {
        private readonly System.Action<IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_0_2(ref IInput Input, 
            System.Action<IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput.To() 
            => _action.Invoke(this);
    }
    public sealed class PairActionObject_0_3<R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput, IReturn<R1, R2, R3>
    {
        private readonly System.Action<IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_0_3(ref IInput Input, 
            System.Action<IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput.To() 
            => _action.Invoke(this);
    }
    public sealed class PairActionObject_0_4<R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_0_4(ref IInput Input, 
            System.Action<IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput.To() 
            => _action.Invoke(this);
    }
    public sealed class PairActionObject_0_5<R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_0_5(ref IInput Input, 
            System.Action<IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput.To() 
            => _action.Invoke(this);
    }
    public sealed class PairActionObject_1_1<T1, R1> 
        : Redirect<R1>, IInput<T1>, IReturn<R1>
    {
        private readonly System.Action<T1, IReturn<R1>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_1_1(ref IInput<T1> Input, 
            System.Action<T1, IReturn<R1>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1>.To(R1 value)
            => Input.To(value);

        void IInput<T1>.To(T1 value) 
            => _action.Invoke(value, this);
    }
    public sealed class PairActionObject_1_2<T1, R1, R2> 
        : Redirect<R1, R2>, IInput<T1>, IReturn<R1, R2>
    {
        private readonly System.Action<T1, IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_1_2(ref IInput<T1> Input, 
            System.Action<T1, IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1>.To(T1 value) 
            => _action.Invoke(value, this);
    }
    public sealed class PairActionObject_2_2<T1, T2, R1, R2> 
        : Redirect<R1, R2>, IInput<T1, T2>, IReturn<R1, R2>
    {
        private readonly System.Action<T1, T2, IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_2_2(ref IInput<T1, T2> Input, 
            System.Action<T1, T2, IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2>.To(T1 value1, T2 value2) 
            => _action.Invoke(value1, value2, this);
    }
    public sealed class PairActionObject_3_2<T1, T2, T3, R1, R2> 
        : Redirect<R1, R2>, IInput<T1, T2, T3>, IReturn<R1, R2>
    {
        private readonly System.Action<T1, T2, T3, IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_3_2(ref IInput<T1, T2, T3> Input, 
            System.Action<T1, T2, T3, IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3) 
            => _action.Invoke(value1, value2, value3, this);
    }
    public sealed class PairActionObject_4_2<T1, T2, T3, T4, R1, R2> 
        : Redirect<R1, R2>, IInput<T1, T2, T3, T4>, IReturn<R1, R2>
    {
        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_4_2(ref IInput<T1, T2, T3, T4> Input, 
            System.Action<T1, T2, T3, T4, IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => _action.Invoke(value1, value2, value3, value4, this);
    }
    public sealed class PairActionObject_5_2<T1, T2, T3, T4, T5, R1, R2> 
        : Redirect<R1, R2>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_5_2(ref IInput<T1, T2, T3, T4, T5> Input, 
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2>.To(R1 value1, R2 value2)
            => Input.To(value1, value2);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => _action.Invoke(value1, value2, value3, value4, value5, this);
    }
    public sealed class PairActionObject_1_3<T1, R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput<T1>, IReturn<R1, R2, R3>
    {
        private readonly System.Action<T1, IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_1_3(ref IInput<T1> Input, 
            System.Action<T1, IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1>.To(T1 value) 
            => _action.Invoke(value, this);
    }
    public sealed class PairActionObject_2_3<T1, T2, R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput<T1, T2>, IReturn<R1, R2, R3>
    {
        private readonly System.Action<T1, T2, IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_2_3(ref IInput<T1, T2> Input, 
            System.Action<T1, T2, IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2>.To(T1 value1, T2 value2) 
            => _action.Invoke(value1, value2, this);
    }
    public sealed class PairActionObject_3_3<T1, T2, T3, R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3>, IReturn<R1, R2, R3>
    {
        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_3_3(ref IInput<T1, T2, T3> Input, 
            System.Action<T1, T2, T3, IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3) 
            => _action.Invoke(value1, value2, value3, this);
    }
    public sealed class PairActionObject_4_3<T1, T2, T3, T4, R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3>
    {
        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_4_3(ref IInput<T1, T2, T3, T4> Input, 
            System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => _action.Invoke(value1, value2, value3, value4, this);
    }
    public sealed class PairActionObject_5_3<T1, T2, T3, T4, T5, R1, R2, R3> 
        : Redirect<R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_5_3(ref IInput<T1, T2, T3, T4, T5> Input, 
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3>.To(R1 value1, R2 value2, R3 value3)
            => Input.To(value1, value2, value3);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => _action.Invoke(value1, value2, value3, value4, value5, this);
    }
    public sealed class PairActionObject_1_4<T1, R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput<T1>, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<T1, IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_1_4(ref IInput<T1> Input, 
            System.Action<T1, IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1>.To(T1 value) 
            => _action.Invoke(value, this);
    }
    public sealed class PairActionObject_2_4<T1, T2, R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2>, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<T1, T2, IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_2_4(ref IInput<T1, T2> Input, 
            System.Action<T1, T2, IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2>.To(T1 value1, T2 value2) 
            => _action.Invoke(value1, value2, this);
    }
    public sealed class PairActionObject_3_4<T1, T2, T3, R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_3_4(ref IInput<T1, T2, T3> Input, 
            System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3) 
            => _action.Invoke(value1, value2, value3, this);
    }
    public sealed class PairActionObject_4_4<T1, T2, T3, T4, R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_4_4(ref IInput<T1, T2, T3, T4> Input, 
            System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => _action.Invoke(value1, value2, value3, value4, this);
    }
    public sealed class PairActionObject_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4> 
        : Redirect<R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_5_4(ref IInput<T1, T2, T3, T4, T5> Input, 
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4>.To(R1 value1, R2 value2, R3 value3, R4 value4)
            => Input.To(value1, value2, value3, value4);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => _action.Invoke(value1, value2, value3, value4, value5, this);
    }
    public sealed class PairActionObject_1_5<T1, R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1>, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<T1, IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_1_5(ref IInput<T1> Input, 
            System.Action<T1, IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1>.To(T1 value) 
            => _action.Invoke(value, this);
    }
    public sealed class PairActionObject_2_5<T1, T2, R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2>, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_2_5(ref IInput<T1, T2> Input, 
            System.Action<T1, T2, IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2>.To(T1 value1, T2 value2) 
            => _action.Invoke(value1, value2, this);
    }
    public sealed class PairActionObject_3_5<T1, T2, T3, R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3>, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_3_5(ref IInput<T1, T2, T3> Input, 
            System.Action<T1, T2, T3, IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3) 
            => _action.Invoke(value1, value2, value3, this);
    }
    public sealed class PairActionObject_4_5<T1, T2, T3, T4, R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4>, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_4_5(ref IInput<T1, T2, T3, T4> Input, 
            System.Action<T1, T2, T3, T4, IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => _action.Invoke(value1, value2, value3, value4, this);
    }
    public sealed class PairActionObject_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5> 
        : Redirect<R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IReturn<R1, R2, R3, R4, R5>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_5_5(ref IInput<T1, T2, T3, T4, T5> Input, 
            System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1, R2, R3, R4, R5>.To(R1 value1, R2 value2, R3 value3, R4 value4, R5 value5)
            => Input.To(value1, value2, value3, value4, value5);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => _action.Invoke(value1, value2, value3, value4, value5, this);
    }
    public sealed class PairActionObject_2_1<T1, T2, R1> 
        : Redirect<R1>, IInput<T1, T2>, IReturn<R1>
    {
        private readonly System.Action<T1, T2, IReturn<R1>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_2_1(ref IInput<T1, T2> Input, 
            System.Action<T1, T2, IReturn<R1>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1>.To(R1 value)
            => Input.To(value);

        void IInput<T1, T2>.To(T1 value1, T2 value2) 
            => _action.Invoke(value1, value2, this);
    }
    public sealed class PairActionObject_3_1<T1, T2, T3, R1> 
        : Redirect<R1>, IInput<T1, T2, T3>, IReturn<R1>
    {
        private readonly System.Action<T1, T2, T3, IReturn<R1>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_3_1(ref IInput<T1, T2, T3> Input, 
            System.Action<T1, T2, T3, IReturn<R1>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1>.To(R1 value)
            => Input.To(value);

        void IInput<T1, T2, T3>.To(T1 value1, T2 value2, T3 value3) 
            => _action.Invoke(value1, value2, value3, this);
    }
    public sealed class PairActionObject_4_1<T1, T2, T3, T4, R1> 
        : Redirect<R1>, IInput<T1, T2, T3, T4>, IReturn<R1>
    {
        private readonly System.Action<T1, T2, T3, T4, IReturn<R1>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_4_1(ref IInput<T1, T2, T3, T4> Input, 
            System.Action<T1, T2, T3, T4, IReturn<R1>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1>.To(R1 value)
            => Input.To(value);

        void IInput<T1, T2, T3, T4>.To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => _action.Invoke(value1, value2, value3, value4, this);
    }
    public sealed class PairActionObject_5_1<T1, T2, T3, T4, T5, R1> 
        : Redirect<R1>, IInput<T1, T2, T3, T4, T5>, IReturn<R1>
    {
        private readonly System.Action<T1, T2, T3, T4, T5, IReturn<R1>> _action;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        public PairActionObject_5_1(ref IInput<T1, T2, T3, T4, T5> Input, 
            System.Action<T1, T2, T3, T4, T5, IReturn<R1>> action, IInformation information)
            : base (information)
        {
            _uniqueID = PairActionObject.s_UniqueID++;

            _action = action;
            Input = this;
        }

        public ulong GetUnieueID() 
            => _uniqueID;

        void IReturn<R1>.To(R1 value)
            => Input.To(value);

        void IInput<T1, T2, T3, T4, T5>.To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => _action.Invoke(value1, value2, value3, value4, value5, this);
    }
}