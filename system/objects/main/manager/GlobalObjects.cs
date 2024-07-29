namespace Butterfly.system.objects.main.manager
{
    /// <summary>
    /// Связывает с глобальными родительскими обьектами. 
    /// </summary>
    public sealed class GlobalObjects : Informing, IGlobalObjects, dispatcher.IGlobalObjects
    {
        private readonly System.Collections.Generic.Dictionary<string, object> _values;

        /// <summary>
        /// Ключи обьектов созданых в текущем обьекте, 
        /// </summary>
        private string[] _creatingObjectKeys = new string[0];

        /// <summary>
        /// Ключи обьектов созданых в дочернем обьекте, но делегирующих
        /// Процесс уничтожение данному обьекту.
        /// </summary>
        private string[] _delegateControlObjectKey = new string[0];

        /// <summary>
        /// Извлечение данных из безопасных обьектов.
        /// </summary>
        private System.Action[] _extractInObjects = new System.Action[0];
        /// <summary>
        /// Ключи безопасных обьектов из которох нужно извлеч данные. 
        /// </summary>
        private string[] _extractKeys = new string[0];

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public GlobalObjects(informing.IMain mainInforming, System.Collections.Generic.Dictionary<string, object> globalObjects,
            information.Header headerInformation, information.State stateInformation, information.DOM DOMInformation)
            : base("GlobalObjectsManager", mainInforming)
        {
            _values = globalObjects;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;
        }

        public RedirectType Add<GlobalObjectType, RedirectType, InputType>
            (string key, ref InputType input, GlobalObjectType value)
                where GlobalObjectType : InputType, RedirectType, IInformation
                    => Hellper.GetInput<GlobalObjectType, InputType>
                        (ref input, Add(key, value));

        public RedirectType Add<GlobalObjectType, RedirectType>
            (string key, GlobalObjectType value)
                where GlobalObjectType : RedirectType
                    => Add(key, value);

        public RedirectType Get<GlobalObjectType, LocalObjectType, InputType, RedirectType>
            (string key, ref InputType input, LocalObjectType localObject)
                where LocalObjectType : InputType, RedirectType, IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect
                        => Hellper.SetConnected<LocalObjectType, InputType, GlobalObjectType>
                            (ref input, localObject, Get<GlobalObjectType>(key));

        public void Get<GlobalObjectType, LocalObjectType, InputType>
            (string key, ref InputType input, LocalObjectType localObject)
                where LocalObjectType : InputType, IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect
                        => Hellper.SetConnected<LocalObjectType, InputType, GlobalObjectType>
                            (ref input, localObject, Get<GlobalObjectType>(key));

        public void Get<GlobalObjectType, LocalObjectType>
            (string key, LocalObjectType localObject)
                where LocalObjectType : IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect
                        => localObject.SetConnected(Get<GlobalObjectType>(key).GetConnect());

        public void Get<GlobalObjectType, InputType>
            (string key, ref InputType input)
                where GlobalObjectType : InputType, IInformation
                    => input = Get<GlobalObjectType>(key);

        public GlobalObjectType Get<GlobalObjectType>(string key)
            where GlobalObjectType : IInformation
        {
            if (_values.TryGetValue(key, out object globalObject))
            {
                //if (_stateInformation.IsContruction)
                //{
                    if (globalObject is GlobalObjectType globalObjectReduse)
                    {
                        if (globalObject is InformationGlobalObject.IReplace)
                        {
                            return globalObjectReduse;
                        }
                        else
                        {
                            if (_DOMInformation.IsParentID(globalObjectReduse.GetNodeID()))
                            {
                                return globalObjectReduse;
                            }
                            else
                                Exception($"Глобальный обьект с именем {key} не определен не у одного из ваших родителей. " +
                                    $"Обьект с таким именем находится в {globalObjectReduse.GetExplorer()}");
                        }
                    }
                    else
                        throw new System.Exception($"Вы пытаетесь получить глобальный обьект типа {typeof(GlobalObjectType).FullName} по ключу {key}" +
                            $", но под данным ключом находится обьект типа {globalObject.GetType().FullName}.");
                //}
                //else
                    //Exception($"Вы можете установить ссылку на глобальный обьект {key} только в методе Contruction().");
            }
            else
                Exception($"Вы пытаетесь получить несущесвующий глобальный обьект по ключу {key}");

            return default;
        }

        public GlobalObjectType Add<GlobalObjectType>(string key, GlobalObjectType value)
        {
            //if (_stateInformation.IsContruction)
            {
                if (_values.TryGetValue(key, out object globalObject))
                {
                    if (globalObject is IInformation globalObjectInformation)
                    {
                        if (globalObject is GlobalObjectType globalObjectReduse && 
                            globalObject is InformationGlobalObject.IReplace globalObjectReplace
                            && globalObject is ISafeListenPollRestart globalObjectRestart)
                        {
                            // Проверяем данный обьект на соответсвие директории.
                            if (_DOMInformation.CurrentObject.GetExplorer()
                                == globalObjectInformation.GetExplorer())
                            {
                                globalObjectReplace.Process(_DOMInformation.CurrentObject);
                                globalObjectRestart.Process();

                                return globalObjectReduse;
                            }
                            else Exception($"Вы пытаетесь повторно получить ссылку на глобальный обьект {key}" +
                                $"из обьекта который которому данный глобальный обьект не пренадлежит." +
                                $"Глобальный обьект {key} первоночально был создан в {globalObjectInformation.GetExplorer()}, " +
                                $"а попытка получить осуществляется из {_DOMInformation.CurrentObject.GetExplorer()}.");
                        }
                        else Exception($"Вы уже создали глобальный обьект с именем {key} c типом " +
                            $"{globalObject.GetType().FullName} в {globalObjectInformation.GetExplorer()}.");
                    }
                    else throw new System.Exception($"Обьект типа {globalObject.GetType().FullName} не реализует интерфейс " +
                            $"{typeof(IInformation).FullName}");
                }
                else
                {
                    // Создание обьекта может быть отложеным.Например его создание должно произойти
                    // не в момент конструирования обьекта.
                    _values.Add(key, value);

                    // Обьект реализующий данный интерфейс не предпологает
                    // что он будет уничтожен стандартным способом.
                    // Метод описаный в данном интерфейсе позволяет сменить владельца.
                    // Удаление данного обьекта будет осуществленно отличным способом.
                    if (value is InformationGlobalObject.IReplace == false)
                        Hellper.ExpendArray(ref _creatingObjectKeys, key);

                    return value;
                }
            }
            //else Exception($"Вы можете добавить глобальный обьект" +
            //    $"глобальный обьект {key} только в методе Contruction().");

            return default;
        }

        /// <summary>
        /// Данный метод добавляет имя глобального обьекта, 
        /// за уничтожение которого данный обьект будет отвечать. 
        /// </summary>
        /// <param name="key"></param>
        public string AddControlToGlobalObject(string key)
        {
            Hellper.ExpendArray(ref _delegateControlObjectKey, key);

            return key;
        }

        void dispatcher.IGlobalObjects.RemoveObjects()
        {
            foreach (string key in _creatingObjectKeys)
                _values.Remove(key);
        }

        public void Extract<GlobalObjectType, ValueType>(string key, System.Action<ValueType[]> safe)
            where GlobalObjectType : IExtract<ValueType>
        {
            System.Action action = () =>
            {
                if (_values.TryGetValue(key, out object obj))
                {
                    if (obj is IExtract<ValueType> objExtract)
                    {
                        objExtract.To(safe);
                    }
                    else Exception($"Вы пытаетесь извлечь данные типа {typeof(ValueType).FullName} из обьекта" +
                        $" {key}, но данный обьект обрабатывает данные другого типа.");
                }
            };

            Hellper.ExpendArray(ref _extractInObjects, action);
            Hellper.ExpendArray(ref _extractKeys, key);
        }

        void dispatcher.IGlobalObjects.ExtractObjects()
        {
            for (int i = 0; i < _extractInObjects.Length; i++)
            {
                if (_delegateControlObjectKey.Length > 0)
                {
                    for (int u = 0; u < _delegateControlObjectKey.Length; u++)
                    {
                        if (_delegateControlObjectKey[u] == _extractKeys[i])
                        {
                            _extractInObjects[i].Invoke();

                            _values.Remove(_extractKeys[i]);
                        }
                    }
                }
                else
                {
                    if (_values.TryGetValue(_extractKeys[i], out object obj))
                    {
                        if (obj is IInformation globalObjectInformation)
                        {
                            Exception($"Вы пытаетесь получить данные из обьекта, но у вас нету доступа к нему." +
                                $"Необходимый вам обьект был создан в {globalObjectInformation.GetExplorer()}");
                        }
                        else throw new System.Exception();
                    }
                    else throw new System.Exception();
                }
            }
        }
    }
}