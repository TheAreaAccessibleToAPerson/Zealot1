namespace Butterfly.system.objects.main
{
    public abstract class Redirect<T1, T2, T3, T4, T5> : InformationGlobalObject, IRedirect<T1, T2, T3, T4, T5>
    {
        protected IInput<T1, T2, T3, T4, T5> Input;

        public Redirect(IInformation information)
            : base(information) { }

        public void output_to(System.Action<T1, T2, T3, T4, T5> action)
            => new ActionObject<T1, T2, T3, T4, T5>(ref Input, action);

        public IRedirect<R> output_to<R>(System.Func<T1, T2, T3, T4, T5, R> func)
            => new FuncObject<T1, T2, T3, T4, T5, R>(ref Input, func, this);

        public void send_message_to(string name)
            => GlobalObjectsManager().Get<ListenMessage<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>>(name, ref Input);

        public void output_to(System.Action<T1, T2, T3, T4, T5> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>>
                (name, ref Input, new ActionObjectPoll<T1, T2, T3, T4, T5>(ref Input, action, this, null));

        public void output_to(System.Action<T1, T2, T3, T4, T5> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll<T1, T2, T3, T4, T5>, IInput<T1, T2, T3, T4, T5>>
                (name, ref Input, new ActionObjectPoll<T1, T2, T3, T4, T5>(ref Input, action, this, safe));

        public IRedirect<R> output_to<R>(System.Func<T1, T2, T3, T4, T5, R> func, string name)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<T1, T2, T3, T4, T5, R>(ref Input, func, this, null));

        public IRedirect<R> output_to<R>(System.Func<T1, T2, T3, T4, T5, R> func, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<T1, T2, T3, T4, T5, R>(ref Input, func, this, safe));

        public IRedirect<R> send_echo_to<R>(string name)
            => GlobalObjectsManager().Get<ListenEcho_5_1<T1, T2, T3, T4, T5, R>, SendEcho_5_1<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                    (name, ref Input, new SendEcho_5_1<T1, T2, T3, T4, T5, R>(this));

        public IRedirect<R1, R2> send_echo_to<R1, R2>(string name)
            => GlobalObjectsManager().Get<ListenEcho_5_2<T1, T2, T3, T4, T5, R1, R2>, SendEcho_5_2<T1, T2, T3, T4, T5, R1, R2>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2>>
                    (name, ref Input, new SendEcho_5_2<T1, T2, T3, T4, T5, R1, R2>(this));

        public IRedirect<R1, R2, R3> send_echo_to<R1, R2, R3>(string name)
            => GlobalObjectsManager().Get<ListenEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, SendEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3>>
                    (name, ref Input, new SendEcho_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(this));

        public IRedirect<R1, R2, R3, R4> send_echo_to<R1, R2, R3, R4>(string name)
            => GlobalObjectsManager().Get<ListenEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, SendEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4>>
                    (name, ref Input, new SendEcho_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(this));

        public IRedirect<R1, R2, R3, R4, R5> send_echo_to<R1, R2, R3, R4, R5>(string name)
            => GlobalObjectsManager().Get<ListenEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, SendEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref Input, new SendEcho_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(this));

        public IRedirect<R> output_to<R>(System.Action<T1, T2, T3, T4, T5, IReturn<R>> action)
            => new PairActionObject_5_1<T1, T2, T3, T4, T5, R>(ref Input, action, this);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action)
            => new PairActionObject_5_2<T1, T2, T3, T4, T5, R1, R2>(ref Input, action, this);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action)
            => new PairActionObject_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action)
            => new PairActionObject_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action)
            => new PairActionObject_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref Input, action, this);

        public IRedirect<R> output_to<R>(System.Action<T1, T2, T3, T4, T5, IReturn<R>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>(ref Input, action, this, null));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref Input, action, this, null));

        public IRedirect<R> output_to<R>(System.Action<T1, T2, T3, T4, T5, IReturn<R>> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>, IInput<T1, T2, T3, T4, T5>, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_5_1<T1, T2, T3, T4, T5, R>(ref Input, action, this, safe));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2>> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_5_2<T1, T2, T3, T4, T5, R1, R2>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3>> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_5_3<T1, T2, T3, T4, T5, R1, R2, R3>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4>> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_5_4<T1, T2, T3, T4, T5, R1, R2, R3, R4>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T1, T2, T3, T4, T5, IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action<T1, T2, T3, T4, T5> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>, IInput<T1, T2, T3, T4, T5>, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_5_5<T1, T2, T3, T4, T5, R1, R2, R3, R4, R5>(ref Input, action, this, safe));
    }
}