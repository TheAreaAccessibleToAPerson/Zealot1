using System.Threading;
namespace Butterfly
{
    public sealed class CollectorCollection<T>
    {
        private readonly System.Collections.Generic.List<T> _values 
            = new System.Collections.Generic.List<T>();

        private readonly System.Action<T[]> _action;

        private object _1 = new object();
        private object _2 = new object();
        private object _3 = new object();

        private bool _is1 = false;

        public CollectorCollection(System.Action<T[]> callAction)
            => _action = callAction;

        public void Subscribe(T value)
        {
        }

        private void Process(T[] values)
        {
            //...
        }
    }
}