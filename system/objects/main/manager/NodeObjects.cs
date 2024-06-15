namespace Butterfly.system.objects.main.manager
{
    public sealed class NodeObjects : Informing
    {
        private readonly informing.IMain _mainInforming;

        private readonly System.Collections.Generic.Dictionary<string, object> _globalObjects;

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public NodeObjects(informing.IMain mainInforming, information.Header headerInformation, information.State stateInformation,
            information.DOM DOMInformation, System.Collections.Generic.Dictionary<string, object> globalObject)
            : base("NodeObjectsManager", mainInforming)
        {
            _mainInforming = mainInforming;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;

            _globalObjects = globalObject;
        }

        private System.Collections.Generic.Dictionary<string, main.Object> _values;
        private System.Collections.Generic.Dictionary<string, main.Object> _dublicatValues;

        /// <summary>
        /// Количесво Node обьектов. 
        /// </summary>
        /// <value></value>
        public int Count
        {
            get { lock (_stateInformation.Locker) return _values == null ? 0 : _values.Count; }
        }

        public int DublicatCount
        {
            get { lock (_stateInformation.Locker) return _dublicatValues == null ? 0 : _dublicatValues.Count; }
        }


        /// <summary>
        /// Текущее количесво собираемых обьектов.
        /// </summary>
        private int _collectedCount = 0;

        /// <summary>
        /// Инкрементируем количесво собраных объектов.
        /// </summary>
        public void IncrementCollectedCount()
        {
            lock (_stateInformation.Locker)
            {
                if (_values == null)
                {
                    //...
                }
                else
                {
                    if (_headerInformation.IsSystemController())
                    {
                        if ((--_collectedCount) == -1)
                        {
                            _collectedCount = 0;

                            if (_stateInformation.IsDestroying)
                            {
                                StoppingAllObject();
                            }
                        }
                    }
                    else
                    {
                        if ((--_collectedCount) == 0)
                        {
                            if (_stateInformation.IsDestroying)
                            {
                                StoppingAllObject();
                            }
                        }
                    }
                }
            }
        }

        public bool Contains(string key)
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroy) return false;

                if (Count == 0) return false;

                if (_values.TryGetValue(key, out main.Object valueReduse))
                {
                    lock (valueReduse.StateInformation.Locker)
                        if (valueReduse.StateInformation.IsDestroy == false)
                            return true;
                }

                if (DublicatCount > 0)
                {
                    if (_dublicatValues.ContainsKey(key))
                        return true;
                }

                return false;
            }
        }

        public bool TryGet<NodeObjectType>(string key, out NodeObjectType value)
            where NodeObjectType : main.Object, main.description.IDOM, new()
        {
            lock (_stateInformation.Locker)
            {
                value = null;

                if (_stateInformation.IsDestroy) return false;

                if (Count == 0) return false;

                if (_values.TryGetValue(key, out main.Object valueReduse))
                {
                    if (valueReduse is NodeObjectType nodeObjectReduse)
                    {
                        lock (valueReduse.StateInformation.Locker)
                        {
                            if (valueReduse.StateInformation.IsDestroy == false)
                            {
                                value = nodeObjectReduse;

                                return true;
                            }
                        }
                    }
                    else Exception($"Вы пытаетесь получить node обьект типа {typeof(NodeObjectType)} по ключу {key}, "
                        + $" но с данным ключом уже создан обьект типа {valueReduse.GetType()}");
                }

                if (DublicatCount > 0)
                {
                    if (_dublicatValues.TryGetValue(key, out main.Object dublicateValue))
                    {
                        if (dublicateValue is NodeObjectType dublicateValueReduse)
                        {
                            value = dublicateValueReduse;

                            return true;
                        }
                        else Exception($"Вы пытаетесь получить node обьект типа {typeof(NodeObjectType)} по ключу {key}, "
                            + $" но с данным ключом уже создан обьект типа {dublicateValue.GetType()}");
                    }
                }

                return false;
            }
        }

        public NodeObjectType Add<NodeObjectType>(string key, object localFields)
            where NodeObjectType : main.Object, main.description.IDOM, new()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsDestroy == false &&
                    (_stateInformation.IsStart || _stateInformation.IsStarting || _stateInformation.IsSubscribe))
                {
                    if (_values is null) _values = new System.Collections.Generic.Dictionary<string, Object>();

                    if (_values.TryGetValue(key, out main.Object value))
                    {
                        if (value is NodeObjectType valueReduse)
                        {
                            lock (valueReduse.StateInformation.Locker)
                            {
                                if (valueReduse.StateInformation.IsDestroy == false)
                                {
                                    return valueReduse;
                                }
                                else if (_dublicatValues is null)
                                    _dublicatValues = new System.Collections.Generic.Dictionary<string, main.Object>();

                                NodeObjectType nodeObject = Define<NodeObjectType>(key, localFields);

                                if (_dublicatValues.Remove(key))
                                {
                                    //...
                                }

                                _dublicatValues.Add(key, nodeObject);

                                return nodeObject;
                            }
                        }
                        else
                            Exception(data.BranchManager.x100002, typeof(NodeObjectType).FullName,
                                key, value.GetType().FullName);
                    }
                    else
                    {
                        NodeObjectType nodeObject = Define<NodeObjectType>(key, localFields);

                        _values.Add(key, nodeObject);

                        _collectedCount++;
                        _DOMInformation.RootManager.ActionInvoke(nodeObject.CreatingNode);

                        return nodeObject;
                    }
                }

                return default;
            }
        }

        private NodeObjectType Define<NodeObjectType>(string key, object localFields)
            where NodeObjectType : main.Object, main.description.IDOM, new()
        {
            NodeObjectType nodeObject = new NodeObjectType();

            if (localFields != null)
            {
                if (nodeObject is ILocalField objectLocalFieldReduse)
                {
                    objectLocalFieldReduse.SetField(localFields);
                }
                else Exception("Вы передали значение для локального поля обьекта, не не подключили его.");
            }

            nodeObject.NodeDefine(key, _DOMInformation.NestingNodeNamberInTheSystem + 1,
                    _DOMInformation.ParentObjectsID, _DOMInformation.CurrentObject,
                        _DOMInformation.NearBoardNodeObject, _DOMInformation.RootManager,
                            _globalObjects);

            return nodeObject;
        }

        private bool _isStopping = false;
        public void StoppingAllObject()
        {
            lock (_stateInformation.Locker)
            {
                // Он не может сюда зайти во второй разю
                if (_collectedCount == 0 && Count > 0 && _isStopping == false)
                {
                    _isStopping = true;

                    foreach (main.Object nodeObject in _values.Values)
                    {
                        _DOMInformation.RootManager.ActionInvoke
                            (((ILifeCyrcle)nodeObject).ContinueDestroy);
                    }
                }
            }
        }

        public void Remove(string key)
        {
            lock (_stateInformation.Locker)
            {
                if (_values.Remove(key, out main.Object mainObject))
                {
                    if (_dublicatValues != null && _dublicatValues.Count > 0)
                    {
                        if (_dublicatValues.Remove(key, out main.Object nodeObject))
                        {
                            _values.Add(key, nodeObject);

                            _collectedCount++;

                            ((main.description.IDOM)nodeObject).CreatingNode();

                            return;
                        }
                    }

                    //SystemInformation($"Count:{_values.Count} NAME {key}, _collectedCount:{_collectedCount}", System.ConsoleColor.White);

                    if (_collectedCount == 0 && _stateInformation.IsStop == false && _stateInformation.IsDestroying &&
                        _stateInformation.IsStopping && _values.Count == 0 && DublicatCount == 0)
                    {
                        _DOMInformation.RootManager.ActionInvoke(() =>
                        {
                            ((manager.IDispatcher)_DOMInformation.CurrentObject).
                                       Process(manager.Dispatcher.Command.STOPPING_OBJECT);

                        });
                    }
                }
                else
                {
                    if (_headerInformation.IsSystemController())
                    {
                        //...
                    }
                    else Exception("EXCEPTION:" + key);
                }
            }
        }
    }
}