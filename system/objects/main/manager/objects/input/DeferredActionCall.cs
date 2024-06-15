using System;

namespace Butterfly.system.objects.main
{
    public sealed class DeferredActionCall : InformationGlobalObject, IInput<System.Action, uint>,
        IInputConnected
    {
        // Подключается к handler_events
        private IInput<System.Action, uint> _connect;

        public DeferredActionCall(ref IInput<System.Action, uint> input, IInformation information)
            : base(information)
        {
            input = this;
        }

        void IInput<System.Action, uint>.To(Action value1, uint value2)
        {
            _connect.To(value1, value2);
        }

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<System.Action, uint>>
                (inputConnect, ref _connect, GetType());
    }
}
