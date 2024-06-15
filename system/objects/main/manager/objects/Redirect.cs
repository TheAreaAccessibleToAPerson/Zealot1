namespace Butterfly.system.objects.main
{
    public abstract class Redirect : InformationGlobalObject, IRedirect
    {
        protected IInput Input;

        public Redirect(IInformation information)
            : base(information) { }

        public void output_to(System.Action action)
            => new ActionObject(ref Input, action);

        public IRedirect<R> output_to<R>(System.Func<R> func)
            => new FuncObject<R>(ref Input, func, this);

        public IRedirect send_echo_to(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_0, SendEcho_0_0, IInput, IRedirect>
                (name, ref Input, new SendEcho_0_0(this));

        public void output_to(System.Action action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll, IInput>
                (name, ref Input, new ActionObjectPoll(ref Input, action, this, null));

        public void output_to(System.Action action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, ActionObjectPoll, IInput>
                (name, ref Input, new ActionObjectPoll(ref Input, action, this, safe));

        public IRedirect<R> output_to<R>(System.Func<R> func, string name)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<R>, IInput, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<R>(ref Input, func, this, null));

        public IRedirect<R> output_to<R>(System.Func<R> func, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, FuncObjectPoll<R>, IInput, IRedirect<R>>
                (name, ref Input, new FuncObjectPoll<R>(ref Input, func, this, safe));

        public IRedirect<R> send_echo_to<R>(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_1<R>, SendEcho_0_1<R>, IInput, IRedirect<R>>
                    (name, ref Input, new SendEcho_0_1<R>(this));

        public IRedirect<R1, R2> send_echo_to<R1, R2>(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_2<R1, R2>, SendEcho_0_2<R1, R2>, IInput, IRedirect<R1, R2>>
                    (name, ref Input, new SendEcho_0_2<R1, R2>(this));

        public IRedirect<R1, R2, R3> send_echo_to<R1, R2, R3>(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_3<R1, R2, R3>, SendEcho_0_3<R1, R2, R3>, IInput, IRedirect<R1, R2, R3>>
                    (name, ref Input, new SendEcho_0_3<R1, R2, R3>(this));

        public IRedirect<R1, R2, R3, R4> send_echo_to<R1, R2, R3, R4>(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_4<R1, R2, R3, R4>, SendEcho_0_4<R1, R2, R3, R4>, IInput, IRedirect<R1, R2, R3, R4>>
                    (name, ref Input, new SendEcho_0_4<R1, R2, R3, R4>(this));

        public IRedirect<R1, R2, R3, R4, R5> send_echo_to<R1, R2, R3, R4, R5>(string name)
            => GlobalObjectsManager().Get<ListenEcho_0_5<R1, R2, R3, R4, R5>, SendEcho_0_5<R1, R2, R3, R4, R5>, IInput, IRedirect<R1, R2, R3, R4, R5>>
                    (name, ref Input, new SendEcho_0_5<R1, R2, R3, R4, R5>(this));

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action)
            => new PairActionObject_0_1<R>(ref Input, action, this);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action)
            => new PairActionObject_0_2<R1, R2>(ref Input, action, this);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action)
            => new PairActionObject_0_3<R1, R2, R3>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action)
            => new PairActionObject_0_4<R1, R2, R3, R4>(ref Input, action, this);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action)
            => new PairActionObject_0_5<R1, R2, R3, R4, R5>(ref Input, action, this);

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_1<R>, IInput, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_0_1<R>(ref Input, action, this, null));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_2<R1, R2>, IInput, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_0_2<R1, R2>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_3<R1, R2, R3>, IInput, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_0_3<R1, R2, R3>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_4<R1, R2, R3, R4>, IInput, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_0_4<R1, R2, R3, R4>(ref Input, action, this, null));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action, string name)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>, IInput, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>(ref Input, action, this, null));

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_1<R>, IInput, IRedirect<R>>
                (name, ref Input, new PairActionObjectPoll_0_1<R>(ref Input, action, this, safe));

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_2<R1, R2>, IInput, IRedirect<R1, R2>>
                (name, ref Input, new PairActionObjectPoll_0_2<R1, R2>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_3<R1, R2, R3>, IInput, IRedirect<R1, R2, R3>>
                (name, ref Input, new PairActionObjectPoll_0_3<R1, R2, R3>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_4<R1, R2, R3, R4>, IInput, IRedirect<R1, R2, R3, R4>>
                (name, ref Input, new PairActionObjectPoll_0_4<R1, R2, R3, R4>(ref Input, action, this, safe));

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action safe)
            => GlobalObjectsManager().Get<HandlerEvents, PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>, IInput, IRedirect<R1, R2, R3, R4, R5>>
                (name, ref Input, new PairActionObjectPoll_0_5<R1, R2, R3, R4, R5>(ref Input, action, this, safe));
    }
}