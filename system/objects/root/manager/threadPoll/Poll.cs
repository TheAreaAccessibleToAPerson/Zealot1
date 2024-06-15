using System.Collections.Concurrent;
namespace Butterfly.system.objects.root
{
    public class Poll : manager.Clients
    {
        /// <summary>
        /// Локер блокирующийся на время выполнения action.
        /// </summary>
        /// <returns></returns>
        private readonly object _actionRunLocker = new object();

        /// <summary>
        /// Oбьект начал процесс подписания полученых билетов?
        /// </summary>
        private bool _isProcessSubscribeAndUnsubscribe = false;
        private readonly object _processSubscribeAndUnsubscribeLocker = new object();

        /// <summary>
        /// Блокирует доступ к регистрации. 
        /// </summary>
        /// <returns></returns>
        private readonly object _subsribeAndUnsubscribeLocker = new object();

        /// <summary>
        /// Сдесь будут хранится билеты. 
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        //private readonly collection.Value<main.SubscribePollTicket, main.UnsubscribePollTicket> _values
        //= new collection.Value<main.SubscribePollTicket, main.UnsubscribePollTicket>();

        /*
                private readonly collection.Value<main.UnsubscribePollTicket> _values
                   = new collection.Value<main.UnsubscribePollTicket>();
                   */

        private readonly BlockingCollection<main.UnsubscribePollTicket> _values
           = new BlockingCollection<main.UnsubscribePollTicket>();


        /// <summary>
        /// Получаем билет. 
        /// </summary>
        /// <param name="ticket"></param>
        public void AddTicket(main.UnsubscribePollTicket ticket)
        {
            _values.Add(ticket);
            /*
            if (global::System.Threading.Monitor.TryEnter(_actionRunLocker))
            {
                if (global::System.Threading.Monitor.TryEnter(_subsribeAndUnsubscribeLocker))
                {
                    Add(ticket);

                    global::System.Threading.Monitor.Exit(_subsribeAndUnsubscribeLocker);
                }
                else lock (_subsribeAndUnsubscribeLocker) Add(ticket);

                global::System.Threading.Monitor.Exit(_actionRunLocker);
            }
            else if (global::System.Threading.Monitor.TryEnter(_subsribeAndUnsubscribeLocker))
            {
                lock (_processSubscribeAndUnsubscribeLocker)
                {
                    if (_isProcessSubscribeAndUnsubscribe == false)
                    {
                        _values.Add(ticket);
                    }
                    else Add(ticket);
                }

                global::System.Threading.Monitor.Exit(_subsribeAndUnsubscribeLocker);
            }
            else lock (_subsribeAndUnsubscribeLocker) Add(ticket);
            */
        }

        public void ReplaceTimeDelay(string name, uint value)
            => replace_time_delay(name, value);

        void Construction()
        {
            add_teg(root.manager.ActionInvoke.TEG);

            add_thread(Field.Name, () =>
            {
                //lock (_processSubscribeAndUnsubscribeLocker) _isProcessSubscribeAndUnsubscribe = false;

                //lock (_actionRunLocker) Run();
                Run();

                //lock (_subsribeAndUnsubscribeLocker)
                {
                    //lock (_processSubscribeAndUnsubscribeLocker) _isProcessSubscribeAndUnsubscribe = true;

                    int count = _values.Count;
                    if (count > 0)
                    {
                        do
                        {
                            if (_values.TryTake(out main.UnsubscribePollTicket ticket))
                                Add(ticket);
                        }
                        while ((--count) > 0);
                    }

                    /*
                    if (_values.TryExtractAll(out main.UnsubscribePollTicket[] tickets))
                        foreach (main.UnsubscribePollTicket ticket in tickets)
                            Add(ticket);
                            */

                    /*
                                        if (_values.TryExtractAll(out bool isSubscribeTickets, out main.SubscribePollTicket[] subscribeTickets,
                                            out bool isUnsubscribeTickets, out main.UnsubscribePollTicket[] unsubscribeTickets))
                                            {
                                                if (isSubscribeTickets)
                                                    foreach(main.SubscribePollTicket ticket in subscribeTickets)
                                                        Add(ticket);

                                                if (isUnsubscribeTickets)
                                                    foreach(main.UnsubscribePollTicket ticket in unsubscribeTickets)
                                                        Add(ticket);
                                            }
                                            */

                    if (Field.IsDestroy)
                        if (Count == 0)
                        {
                            Field.Destroy(this);
                        }

                }
            },
            Field.TimeDelay, Field.ThreadPriority);
        }
    }
}