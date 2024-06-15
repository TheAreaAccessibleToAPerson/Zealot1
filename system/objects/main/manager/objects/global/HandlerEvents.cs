using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Butterfly.system.objects.main
{
    public struct EventTimeDelay
    {
        public readonly System.Action Event;
        private System.DateTime d_localDateTime = System.DateTime.Now;

        // Сколько времени до запуска.
        public int TimeLeft { private set; get; }

        public EventTimeDelay(System.Action action, uint timeDelay)
        {
            Event = action;
            TimeLeft = (int)timeDelay;
        }

        public bool IsCall(System.DateTime now, out int timeDelay)
        {
            timeDelay = (now.Subtract(d_localDateTime).Seconds * 1000)
                + now.Subtract(d_localDateTime).Milliseconds;

            TimeLeft -= timeDelay;

            if (TimeLeft <= 0)
            {
                Event.Invoke();

                return true;
            }
            else return false;
        }
    }

    public class HandlerEvents : InformationGlobalObject, IInput<System.Action>,
        IInput<System.Action, uint>, IUpdate, IInputConnect
    {
        private readonly ConcurrentQueue<System.Action> _values = new();
        private readonly ConcurrentQueue<EventTimeDelay> _deferrentCallValues = new();

        private readonly List<EventTimeDelay> _awaitDeferrentCallValue = new();

        /// <summary>
        /// Таймер для ближайшего вызова.
        /// </summary>
        private int _nearestTimerCall = 0;
        private System.DateTime d_localDateTime = System.DateTime.Now;

        public HandlerEvents(IInformation information)
            : base(information)
        { }

        public void To(System.Action value)
            => _values.Enqueue(value);

        public void To(System.Action value, uint timeDelay)
        {
            _deferrentCallValues.Enqueue(new EventTimeDelay(value, timeDelay));
        }

        object IInputConnect.GetConnect() => this;

        public void Update()
        {
            // Сколько нам поступило новых вызовов.
            int defferentCallValuesCount = _deferrentCallValues.Count;

            if (defferentCallValuesCount > 0)
            {
                if (_deferrentCallValues.TryDequeue(out EventTimeDelay eventTimeDelay))
                {
                    if (_nearestTimerCall > eventTimeDelay.TimeLeft) _nearestTimerCall = eventTimeDelay.TimeLeft;

                    _awaitDeferrentCallValue.Add(eventTimeDelay);
                }
            }

            if (_awaitDeferrentCallValue.Count > 0)
            {
                // Получаем новый timeDelay.
                _nearestTimerCall -= (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                    + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

                if (_nearestTimerCall <= 0)
                {
                    EventTimeDelay[] removeIndexes = null;
                    int removeCount = 0;
                    for (int i = 0; i < _awaitDeferrentCallValue.Count; i++)
                    {
                        if (_awaitDeferrentCallValue[i].IsCall(d_localDateTime, out int time))
                        {
                            if (removeIndexes == null) removeIndexes = new EventTimeDelay[_awaitDeferrentCallValue.Count];

                            removeIndexes[removeCount++] = _awaitDeferrentCallValue[i];
                        }
                        else
                        {
                            if (_nearestTimerCall > time) _nearestTimerCall = time;
                        }
                    }

                    if (removeCount > 0)
                    {
                        for (int u = 0; u < removeCount; u++)
                        {
                            _awaitDeferrentCallValue.Remove(removeIndexes[u]);
                        }

                        if (_awaitDeferrentCallValue.Count == 0) _nearestTimerCall = 0;
                    }
                }
            }

            d_localDateTime = System.DateTime.Now;

            int count = _values.Count;

            if (count > 0)
            {
                do
                {
                    if (_values.TryDequeue(out System.Action action))
                        action.Invoke();
                }
                while ((--count) > 0);
            }

            /*
            if (awaitDeferredValueCount > 0)
            {
                _nearestTimerCall -= (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                    + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

                EventTimeDelay[] removeIndexes = null; int removeCount = 0;
                for (int i = 0; i < awaitDeferredValueCount; i++)
                {
                    if (_awaitDeferrentCallValue[i].IsCall(d_localDateTime, out int time))
                    {
                        if (removeIndexes == null) removeIndexes
                            = new EventTimeDelay[awaitDeferredValueCount];

                        removeIndexes[removeCount++] = _awaitDeferrentCallValue[i];
                    }
                    else
                    {
                        _nearestTimerCall = time;

                        break;
                    }
                }

                if (removeIndexes != null)
                    for (int i = 0; i < removeCount; i++)
                        _awaitDeferrentCallValue.Remove(removeIndexes[i]);
            }

            d_localDateTime = System.DateTime.Now;

            if (deferrentValueCount > 0)
            {
                d_localDateTime = System.DateTime.Now;

                bool is_new = false;

                do
                {
                    if (_deferrentCallValues.TryDequeue(out EventTimeDelay eventTimeDelay))
                    {
                        if (eventTimeDelay.IsCall(d_localDateTime, out int time))
                        {
                            // ... 
                        }
                        else
                        {
                            if (is_new == false) is_new = true;

                            // Иначе перезаписываем в лист ожидаения.
                            _awaitDeferrentCallValue.Add(eventTimeDelay);

                            if (_nearestTimerCall > time) _nearestTimerCall = time;
                        }
                    }
                }
                while ((--deferrentValueCount) > 0);

                if (is_new)
                {
                    _awaitDeferrentCallValue.Sort((a, b) => a.TimeLeft.CompareTo(b.TimeLeft));
                }
            }

            int count = _values.Count;

            if (count > 0)
            {
                do
                {
                    if (_values.TryDequeue(out System.Action action))
                        action.Invoke();
                }
                while ((--count) > 0);
            }
            */
        }
    }
}