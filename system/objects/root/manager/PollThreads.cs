using System.Linq;

namespace Butterfly
{
    public class Event
    {
        public const string REPLACE_TIME_DELAY = "SYSTEM_EVENTS_REPLACE_TIME_DELAY";
        public const string REPLACE_IS_DESTROY = "SYSTEM_EVENT_REPLAYCE_IS_DESTROY";
    }
}

namespace Butterfly.system.objects.root.manager
{
    public sealed class PollThreads : Controller.LocalField<EventSetting[]>
    {
        public const string TEG = "PollThreadsTeg";

        private readonly System.Collections.Generic.Dictionary<string, Poll> _values
            = new System.Collections.Generic.Dictionary<string, Poll>();
        private readonly object _locker = new object();

        private readonly System.Collections.Generic.Dictionary<string, EventSetting> _eventSettings
            = new System.Collections.Generic.Dictionary<string, EventSetting>();

        void Construction()
        {
            foreach (EventSetting eventSetting in Field)
            {
                if (_eventSettings.Keys.Contains(eventSetting.Name))
                {
                    Exception($"Вы дважды указали настройки для одного и тогоже события {eventSetting.Name}");
                }
                else _eventSettings.Add(eventSetting.Name, eventSetting);
            }
        }

        /// <summary>
        /// Сменяет TimeDelay для события. 
        /// </summary>
        /// <param name="name">Имя события</param>
        /// <param name="value">Значение</param>
        public void ReplaceTimeDelay(string name, uint value)
        {
            if (_eventSettings.TryGetValue(name, out EventSetting eventSetting))
            {
                eventSetting.TimeDelay = value;

                try
                {
                    if (_values.TryGetValue(name, out Poll poll))
                        poll.ReplaceTimeDelay(name, value);
                }
                catch (System.NullReferenceException ex)
                {
                    //...
                }
            }
            else Exception($"Вы попытались сменить timeDelay для события {name}, но его не сущесвует.");
        }

        /// <summary>
        /// Меняет значение пула "Уничтожить в случае отсутвия учасников".
        /// </summary>
        /// <param name="name">Имя события</param>
        /// <param name="value">Значение</param>
        public void ReplaceIsDestroy(string name, bool value)
        {
            if (_eventSettings.TryGetValue(name, out EventSetting eventSetting))
            {
                try
                {
                    eventSetting.IsDestroy = value;
                }
                catch (System.NullReferenceException ex)
                {
                    SystemInformation($"События с именем {name} не сущесвует.");
                }
            }
            else Exception($"Вы попытались для события {name} сменить значение которое" +
                "указывает нужно ли его уничтожить в случае отсутвия учасников, но события с таким именем нету.");
        }

        /// <summary>
        /// Передаем в нужные потоки регистрационые билеты.
        /// </summary>
        public void Add(main.SubscribePollTicket[] tickets)
        {
            lock (_locker)
            {
                for (int i = 0; i < tickets.Length; i++)
                {
                    if (_values.TryGetValue(tickets[i].Name, out Poll poll))
                    {
                        poll.Add(tickets[i]);
                    }
                    else
                    {
                        if (_eventSettings.TryGetValue(tickets[i].Name, out EventSetting eventSetting))
                        {
                            Poll newPoll = obj<Poll>(eventSetting.Name,
                                    new poll.Fields()
                                    {
                                        ID = UniqueID++,
                                        Name = eventSetting.Name,
                                        Size = eventSetting.Size,
                                        TimeDelay = eventSetting.TimeDelay,
                                        ThreadPriority = eventSetting.Priority,
                                        IsDestroy = eventSetting.IsDestroy,
                                        Destroy = Destroy
                                    });

                            newPoll.DefineSize(eventSetting.Size);

                            newPoll.Add(tickets[i]);

                            _values.Add(tickets[i].Name, newPoll);
                        }
                        else Exception($"Вы не задали настройки для события {tickets[i].Name}.");
                    }
                }
            }
        }

        /// <summary>
        /// Передаем в нужные потоки регистрационые билеты.
        /// </summary>
        public void Add(main.UnsubscribePollTicket[] tickets)
        {
            lock (_locker)
            {
                for (int i = 0; i < tickets.Length; i++)
                {
                    if (_values.TryGetValue(tickets[i].Name, out Poll poll))
                    {
                        poll.AddTicket(tickets[i]);
                    }
                    else throw new System.Exception();
                }
            }
        }

        private bool Destroy(Poll poll)
        {
            if (System.Threading.Monitor.TryEnter(_locker))
            {
                if (_values.Remove(poll.Name))
                {
                    poll.destroy();
                }

                System.Threading.Monitor.Exit(_locker);

                return true;
            }

            return false;
        }

        private ulong UniqueID = 0;
    }
}