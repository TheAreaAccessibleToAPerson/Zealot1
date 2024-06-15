namespace Butterfly.system.objects.main
{
    /// <summary>
    /// Реализует методы для получения информации об владельце. 
    /// </summary>
    public abstract class InformationGlobalObject : IInformation
    {
        /// <summary>
        /// Описываем способ смены информации владельца. 
        /// </summary>
        public interface IReplace
        { public void Process(IInformation information); }

        protected IInformation Information;

        public InformationGlobalObject(IInformation information) 
            => Information = information;

        public string GetExplorer() => Information.GetExplorer();
        public string GetKey() => Information.GetKey();
        public ulong GetID() => Information.GetID();
        public ulong GetNodeID() => Information.GetNodeID();

        public bool TryIncrementEvent() => Information.TryIncrementEvent();
        public void DecrementEvent() => Information.DecrementEvent();

        public manager.IGlobalObjects GlobalObjectsManager() 
            => Information.GlobalObjectsManager();

    }
}