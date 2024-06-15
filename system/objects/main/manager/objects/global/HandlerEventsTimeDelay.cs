using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Butterfly.system.objects.main
{

    public class HandlerEventsTimeDelay : InformationGlobalObject, IInput<System.Action, uint>, 
        IUpdate, IInputConnect
    {
        private readonly ConcurrentBag<EventTimeDelay> _values
            = new ConcurrentBag<EventTimeDelay>();

        private readonly List<EventTimeDelay> _awaitValue
            = new List<EventTimeDelay>();

        /// <summary>
        /// Таймер для ближайшего вызова.
        /// </summary>
        private int _nearestTimerCall = 0;
        private System.DateTime d_localDateTime = System.DateTime.Now;

        public HandlerEventsTimeDelay(IInformation information)
            : base(information) { }

        public  void To(System.Action value, uint timeDelay)
            => _values.Add(new EventTimeDelay(value, timeDelay));

        public async void Update()
        {
            d_localDateTime = System.DateTime.Now;

            int awaitValueCount = _awaitValue.Count;
            int valueCount = _values.Count;

            if (awaitValueCount > 0)
            {
                int step = (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000)
                    + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

                if ((_nearestTimerCall - step) <= 0)
                {
                    _nearestTimerCall = 0;

                    for (int i = 0; i < awaitValueCount; i++)
                    {
                        if (_awaitValue[i].IsCall(d_localDateTime, out int time))
                        {
                            _awaitValue.Remove(_awaitValue[i]);
                        }
                        else if (_nearestTimerCall > time) _nearestTimerCall = time;
                    }
                }
            }

            if (valueCount > 0)
            {
                do
                {
                    if (_values.TryTake(out EventTimeDelay eventTimeDelay))
                    {
                        if (eventTimeDelay.IsCall(d_localDateTime, out int time))
                        {
                            // Он просто вызывается.
                        }
                        else
                        {
                            // Иначе перезаписываем в лист ожидаения.
                            _awaitValue.Add(eventTimeDelay);

                            if (_nearestTimerCall > time) _nearestTimerCall = time;
                        }
                    }
                }
                while ((--valueCount) > 0);
            }
        }

        object IInputConnect.GetConnect() => this;
    }

}