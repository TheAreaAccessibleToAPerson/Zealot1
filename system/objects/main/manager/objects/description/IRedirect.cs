namespace Butterfly.system.objects.main
{
    public interface IRedirect
    {
        public void output_to(System.Action action);

        public void output_to(System.Action action, string name);

        public void output_to(System.Action action, string name, System.Action safe);

        public IRedirect<R> output_to<R>(System.Func<R> func);

        public IRedirect<R> output_to<R>(System.Func<R> func, string name);

        public IRedirect<R> output_to<R>(System.Func<R> func, string name, System.Action safe);

        public IRedirect send_echo_to(string name);

        public IRedirect<R> send_echo_to<R>(string name);

        public IRedirect<R1, R2> send_echo_to<R1, R2>(string name);

        public IRedirect<R1, R2, R3> send_echo_to<R1, R2, R3>(string name);

        public IRedirect<R1, R2, R3, R4> send_echo_to<R1, R2, R3, R4>(string name);

        public IRedirect<R1, R2, R3, R4, R5> send_echo_to<R1, R2, R3, R4, R5>(string name);

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action);

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action, string name);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action, string name);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action, string name);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action, string name);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action, string name);

        public IRedirect<R> output_to<R>(System.Action<IReturn<R>> action, string name, System.Action safe);

        public IRedirect<R1, R2> output_to<R1, R2>(System.Action<IReturn<R1, R2>> action, string name, System.Action safe);

        public IRedirect<R1, R2, R3> output_to<R1, R2, R3>(System.Action<IReturn<R1, R2, R3>> action, string name, System.Action safe);

        public IRedirect<R1, R2, R3, R4> output_to<R1, R2, R3, R4>(System.Action<IReturn<R1, R2, R3, R4>> action, string name, System.Action safe);

        public IRedirect<R1, R2, R3, R4, R5> output_to<R1, R2, R3, R4, R5>(System.Action<IReturn<R1, R2, R3, R4, R5>> action, string name, System.Action safe);
    }
}