namespace Butterfly.system.objects.main
{
    public abstract class Redirect<T1, T2, T3, T4, T5, T6> : InformationGlobalObject, IRedirect<T1, T2, T3, T4, T5, T6>
    {
        protected IInput<T1, T2, T3, T4, T5, T6> Input;

        public Redirect(IInformation information)
            : base(information) { }

        public void output_to(System.Action<T1, T2, T3, T4, T5, T6> action)
            => new ActionObject<T1, T2, T3, T4, T5, T6>(ref Input, action);

    }
}