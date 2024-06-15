using System.Threading.Tasks;
using Butterfly.system.objects.main;

namespace Butterfly.system.objects.root
{
    public class Object<ObjectType, InformationType> : Controller.Board, IManager, description.ILife<InformationType>
        where ObjectType : main.Object, new() 
            where InformationType : class
    {
        /// <summary>
        /// Имя системы. 
        /// </summary>
        private string _name;

        /// <summary>
        /// Имя системного события. 
        /// </summary>
        private string _systemEventName;

        /// <summary>
        /// Настройки для событий. 
        /// </summary>
        private EventSetting[] _eventSettings = new EventSetting[1];

        private InformationType _information;

        private manager.ActionInvoke ActionInvokeManager;
        private manager.PollThreads PollThreads;

        void Construction()
        {
            other._hEADErRRR年.SHdJdDkOkDoDd____FFodkjfdsodfOW();

            SystemInformation("starting...");

            PollThreads = obj<manager.PollThreads>
                ("PollThreads", _eventSettings);

            ActionInvokeManager = obj<manager.ActionInvoke>
                ("ActionInvoke", _systemEventName);

            listen_message<string, uint>(Event.REPLACE_TIME_DELAY)
                .output_to((name, value)
                    => ActionInvokeManager.Add(()
                        => PollThreads.ReplaceTimeDelay(name, value)));

            listen_message<string, bool>(Event.REPLACE_IS_DESTROY)
                .output_to((name, value)
                    => ActionInvokeManager.Add(()
                        => PollThreads.ReplaceIsDestroy(name, value)));
        }

        void Start()
        {
            if (_information == null)
                obj<ObjectType>(_name);
            else 
                obj<ObjectType>(_name, _information);

            SystemInformation("start");
        }

        void Stop()
        {
            SystemInformation("stop");

            ActionInvokeManager.IsDestroy = true;
        }

        void IManager.ActionInvoke(System.Action action)
            => ActionInvokeManager.Add(action);

        void IManager.AddSubscribeTickets(main.SubscribePollTicket[] tickets)
            => PollThreads.Add(tickets);

        void IManager.AddUnsubscribeTickets(main.UnsubscribePollTicket[] tickets)
            => PollThreads.Add(tickets);

        void description.ILife<InformationType>.Run(Butterfly.Settings s, InformationType information)
        {
            if (information != null)
                _information = information;

            Creating(s);
        }

        void Creating(Butterfly.Settings s)
        {
            _name = s.Name;
            _systemEventName = s.SystemEvent.Name;

            if (_eventSettings.Length > 0) 
                _eventSettings = s.EventsSetting;
            Hellper.ExpendArray(ref _eventSettings, s.SystemEvent);

            if (s.EventsController != null)
                ((EventsController.IEventsController)s.EventsController)
                    .Set((name, value) =>
                        ActionInvokeManager.Add(() => PollThreads.ReplaceTimeDelay(name, value)),
                        (name, value) =>
                        ActionInvokeManager.Add(() => PollThreads.ReplaceIsDestroy(name, value)));

            ((main.description.IDOM)this).NodeDefine
                ("", 0, new ulong[0], this, this, this, new System.Collections.Generic.Dictionary<string, object>());

            ((main.description.IDOM)this).CreatingNode();
        }
    }
}