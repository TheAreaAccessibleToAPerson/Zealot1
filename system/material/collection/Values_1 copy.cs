namespace Butterfly.system.collection
{
    public sealed class Value<ValueType1, ValueType2>
    {
        private readonly System.Collections.Generic.List<ValueType1> _values1 
            = new System.Collections.Generic.List<ValueType1>();
        private readonly System.Collections.Generic.List<ValueType2> _values2 
            = new System.Collections.Generic.List<ValueType2>();

        private readonly object _locker = new object();

        public void Add(ValueType1 value)
        {
            lock (_locker)
                _values1.Add(value);
        }

        public void Add(ValueType2 value)
        {
            lock (_locker)
                _values2.Add(value);
        }

        public void Add<ValueType>(ValueType value)
        {
            lock(_locker)
            {
                if (value is ValueType1 valueReduse1) _values1.Add(valueReduse1);
                if (value is ValueType2 valueReduse2) _values2.Add(valueReduse2);
            }
        }

        public int Count { get { lock (_locker) return _values1.Count + _values2.Count; } }

        public bool TryExtractAll(out bool isAvailableValue1, out ValueType1[] values1,
            out bool isAvailableValue2, out ValueType2[] values2)
        {
            isAvailableValue1 = isAvailableValue2 = false;

            values1 = null;
            values2 = null;

            if (_values1.Count == 0 && _values2.Count == 0)
            {
                return false;
            }
            else
            {
                if (_values1.Count > 0) isAvailableValue1 = true;
                if (_values2.Count > 0) isAvailableValue2 = true;

                if (isAvailableValue1 && isAvailableValue2)
                {
                    lock (_locker)
                    {
                        if (_values1.Count > 0)
                        {
                            values1 = _values1.ToArray();

                            _values1.Clear();
                        }
                        else
                            isAvailableValue1 = false;

                        if (_values2.Count > 0)
                        {
                            values2 = _values2.ToArray();

                            _values2.Clear();
                        }
                        else
                            isAvailableValue2 = false;

                        if (isAvailableValue1 || isAvailableValue2) 
                            return true;
                    }
                }
                else if (isAvailableValue1)
                {
                    lock (_locker)
                    {
                        if (_values1.Count > 0)
                        {
                            values1 = _values1.ToArray();

                            _values1.Clear();

                            return true;
                        }
                    }
                }
                else if (isAvailableValue2)
                {
                    lock (_locker)
                    {
                        if (_values2.Count > 0)
                        {
                            values2 = _values2.ToArray();

                            _values2.Clear();

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}