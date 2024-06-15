namespace Butterfly.system.objects.main
{
    public abstract class Redirect<T> : InformationGlobalObject, IRedirect<T>
    {
        protected IInput<T> Input;

        public Redirect(IInformation information)
            : base(information) { }

        public void output_to(System.Action<T> action)
            => new ActionObject<T>(ref Input, action);

        public IRedirect<R> output_to<R>(System.Func<T, R> func)
            => new FuncObject<T, R>(ref Input, func, this);

        public void send_message_to(string name)
            => GlobalObjectsManager().Get<ListenMessage<T>, IInput<T>>(name, ref Input);

        public IRedirect send_echo_to(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_0<T>, SendEcho_1_0<T>, IInput<T>, IRedirect>
                (name, ref Input, new SendEcho_1_0<T>(this));

        public void output_to(System.Action<T> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll<T>, IInput<T>>
                (name, ref Input, new ActionObjectPoll<T>(ref Input, action, this, null));

        public void output_to(System.Action<T> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll<T>, IInput<T>>
                (name, ref Input, new ActionObjectPoll<T>(ref Input, action, this, safe));

        public IRedirect<R> output_to<R>(System.Func<T, R> func, string name)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<T, R>, IInput<T>, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<T, R>(ref Input, func, this, null));

        public IRedirect<R> output_to<R>(System.Func<T, R> func, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<T, R>, IInput<T>, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<T, R>(ref Input, func, this, safe));

        public void safe_send_message(string name)
            => GlobalObjectsManager().Get<SafeListenMessagePoll<T>, IInput<T>>
                (name, ref Input);

        public IRedirect<R> send_echo_to<R>(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_1<T, R>, SendEcho_1_1<T, R>, IInput<T>, IRedirect<R>>
                    (name, ref Input, new SendEcho_1_1<T, R>(this));

        public IRedirect<R1, R2> send_echo_to<R1, R2>(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_2<T, R1, R2>, SendEcho_1_2<T, R1, R2>, IInput<T>, IRedirect<R1, R2>>
                    (name, ref Input, new SendEcho_1_2<T, R1, R2>(this));

        public IRedirect<R1, R2, R3> send_echo_to<R1, R2, R3>(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_3<T, R1, R2, R3>, SendEcho_1_3<T, R1, R2, R3>, IInput<T>, IRedirect<R1, R2, R3>>
                    (name, ref Input, new SendEcho_1_3<T, R1, R2, R3>(this));

        public IRedirect<R1, R2, R3, R4> send_echo_to<R1, R2, R3, R4>(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_4<T, R1, R2, R3, R4>, SendEcho_1_4<T, R1, R2, R3, R4>, IInput<T>, IRedirect<R1, R2, R3, R4>>
                    (name, ref Input, new SendEcho_1_4<T, R1, R2, R3, R4>(this));

        public IRedirect<R1, R2, R3, R4, R5> send_echo_to<R1, R2, R3, R4, R5>(string name)
            => GlobalObjectsManager().Get<ListenEcho_1_5<T, R1, R2, R3, R4, R5>, SendEcho_1_5<T, R1, R2, R3, R4, R5>, IInput<T>, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref Input, new SendEcho_1_5<T, R1, R2, R3, R4, R5>(this));

        public IRedirect<R> output_to<R>(System.Action<T, IReturn<R>> action)
            => new PairActionObject_1_1<T, R>(ref Input, action, this);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action)
            => new PairActionObject_1_2<T, R1, R2>(ref Input, action, this);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action)
            => new PairActionObject_1_3<T, R1, R2, R3>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action)
            => new PairActionObject_1_4<T, R1, R2, R3, R4>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action)
            => new PairActionObject_1_5<T, R1, R2, R3, R4, R5>(ref Input, action, this);

        public IRedirect<R> output_to<R>(System.Action<T, IReturn<R>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_1<T, R>, IInput<T>, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_1_1<T, R>(ref Input, action, this, null));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_2<T, R1, R2>, IInput<T>, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_1_2<T, R1, R2>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_3<T, R1, R2, R3>, IInput<T>, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_1_3<T, R1, R2, R3>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_4<T, R1, R2, R3, R4>, IInput<T>, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_1_4<T, R1, R2, R3, R4>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>, IInput<T>, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>(ref Input, action, this, null));

        public IRedirect<R> output_to<R>(System.Action<T, IReturn<R>> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_1<T, R>, IInput<T>, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_1_1<T, R>(ref Input, action, this, safe));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<T, IReturn<R1, R2>> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_2<T, R1, R2>, IInput<T>, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_1_2<T, R1, R2>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<T, IReturn<R1, R2, R3>> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_3<T, R1, R2, R3>, IInput<T>, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_1_3<T, R1, R2, R3>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<T, IReturn<R1, R2, R3, R4>> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_4<T, R1, R2, R3, R4>, IInput<T>, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_1_4<T, R1, R2, R3, R4>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<T, IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action<T> safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>, IInput<T>, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_1_5<T, R1, R2, R3, R4, R5>(ref Input, action, this, safe));
    }
}