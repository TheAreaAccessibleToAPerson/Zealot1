using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Butterfly.system.objects.root.manager
{
    public sealed class ActionInvoke : Controller.LocalField<string>
    {
        private readonly collection.Value<Action> _values
            = new collection.Value<Action>();

        private readonly ConcurrentQueue<Action> _values2 = new ConcurrentQueue<Action>();
        private static int count = 0;

        public const string TEG = "ActionInvokeTeg";

        public bool IsDestroy = false;

        public void Add(System.Action value)
        {
            if (StateInformation.IsStart && IsDestroy == false)
            {
                _values2.Enqueue(value);
                Interlocked.Increment(ref count);
                //_values.Add(value);
            }
            else
            {
                _values2.Enqueue(value);
                Interlocked.Increment(ref count);

                Process();
            }
        }

        void Construction()
        {
            add_teg(TEG);

            add_event(Field, Process);
        }

        void Process()
        {
            /*
            if (_values.TryExtractAll(out Action[] values))
                foreach (Action action in values)
                    action.Invoke();
                    */


            for (int i = 0; i < count; i++)
            {
                if (_values2.TryDequeue(out Action value))
                {
                    Interlocked.Decrement(ref count);
                    value.Invoke();
                }
            }
        }
    }
}