namespace Butterfly
{
    /// <summary>
    /// Общая информация об обьекте отправившего импульс.
    /// </summary>
    public interface IImpulsInformation
    {
        /// <summary>
        /// Хранит адрес обьекта в системе.
        /// </summary>
        /// <returns></returns>
        public string GetExplorer();

        /// <summary>
        /// Хранит ID обьекта в нутри которого был создан.
        /// </summary>
        /// <returns></returns>
        public ulong GetID();

        /// <summary>
        /// Хранит ID узла в нутри которого был создан. 
        /// </summary>
        /// <returns></returns>
        public ulong GetNodeID();

        /// <summary>
        /// Возращает ключ по которому был создан обьект. 
        /// </summary>
        /// <returns></returns>
        public string GetKey();
    }
}

namespace Butterfly.system.objects.main
{
    public class ListenImpuls : Redirect<IImpulsInformation>, IInputConnect, IInput<IImpulsInformation>
    {
        public ListenImpuls(IInformation information) 
            : base(information) { }

        public void To(IImpulsInformation impulsImformation)
            => Input.To(impulsImformation);

        object IInputConnect.GetConnect() => this;
    }

    public sealed class SendImpuls : InformationGlobalObject, IInputConnected, IInput, IImpulsInformation
    {
        private IInput<IImpulsInformation> _connect;

        public SendImpuls(IInformation information) 
            : base(information) 
                => _uniqueID = s_uniqueID++;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<IImpulsInformation>>
                (inputConnect, ref _connect, GetType());

        public void To()
            => _connect.To(this);

        private static ulong s_uniqueID = 0;
    }
}