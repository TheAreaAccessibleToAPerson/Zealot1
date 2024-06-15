namespace Butterfly.system.objects.main.manager
{
    public sealed class BranchObjects : Informing
    {
        private readonly informing.IMain _mainInforming;

        private readonly System.Collections.Generic.Dictionary<string, object> _globalObjects;

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public BranchObjects(informing.IMain mainInforming, information.Header headerInformation, information.State stateInformation,
            information.DOM DOMInformation, System.Collections.Generic.Dictionary<string, object> globalObjects)
            : base("BranchObjectsManager", mainInforming)
        {
            _mainInforming = mainInforming;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;

            _globalObjects = globalObjects;
        }

        private main.Object[] _values = new main.Object[0];
        private string[] _keys = new string[0];

        /// <summary>
        /// Количесво Branch обьектов. 
        /// </summary>
        /// <value></value>
        public int Count { private set; get; } = 0;

        public void LifeCyrcle(byte state)
        {
            if (_values.Length == 0) return;

            switch (state)
            {
                case manager.LifeCyrcle.Data.BEGIN_BRANCH_OBJECT_CONTRUCTION:
                    foreach (main.Object value in _values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.CONSTRUCTION_OBJECT);
                    break;

                case manager.LifeCyrcle.Data.BEGIN_CONFIGURATE:
                    foreach (main.Object value in _values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.CONFIGURATE_OBJECT);
                    break;

                case manager.LifeCyrcle.Data.BEGIN_STARTING:
                    foreach (main.Object value in _values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.STARTING_OBJECT);
                    break;

                case manager.LifeCyrcle.Data.BEGIN_START:
                    foreach (main.Object value in _values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.START_OBJECT);
                    break;

                case manager.LifeCyrcle.Data.BEGIN_STOPPING:
                    foreach (main.Object value in _values)
                        ((manager.ILifeCyrcle)value).ContinueDestroy();
                    break;
            }
        }

        public BranchObjectType Add<BranchObjectType>(string key, object localFields)
            where BranchObjectType : main.Object, new()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                if (_keys[i] == key)
                {
                    if (_values[i] is BranchObjectType valueReduse)
                        return valueReduse;
                    else
                        Exception(data.BranchManager.x100002, typeof(BranchObjectType).FullName,
                            key, _values[i].GetType().FullName);
                }
            }

            BranchObjectType branchObject = new BranchObjectType();

            if (localFields != null)
            {
                if (branchObject is ILocalField objectLocalFieldReduse)
                {
                    objectLocalFieldReduse.SetField(localFields);
                }
                else
                    Exception("Вы передали значение для локального поля обьекта, не не подключили его.");
            }

            ((main.description.IDOM)branchObject).BranchDefine(key, _DOMInformation.NodeID, _DOMInformation.NestingNodeNamberInTheSystem + 1,
                _DOMInformation.NestingObjectNamberInTheNode + 1, _DOMInformation.ParentObjectsID, _DOMInformation.CurrentObject,
                   _DOMInformation.NodeObject, _DOMInformation.NearBoardNodeObject, _DOMInformation.RootManager, _globalObjects);
            {
                if (((main.description.IDOM)branchObject).IsBoard())
                    Exception($"Branch обьект {key} не может быть Board");

                Count = Hellper.ExpendArray(ref _values, branchObject);
                Hellper.ExpendArray(ref _keys, key);
            }

            return branchObject;
        }

        public void Remove(string key)
        {
            lock (_stateInformation.Locker)
            {
                for (int i = 0; i < _keys.Length; i++)
                    if (_keys[i] == key) Count--;

                if (Count == 0 && _stateInformation.IsStop == false)
                {
                    ((manager.IDispatcher)_DOMInformation.CurrentObject).
                        Process(manager.Dispatcher.Command.CONTINUE_STOPPING);
                }
            }
        }
    }
}