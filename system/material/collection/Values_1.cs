namespace Butterfly.system.collection
{
    public sealed class Value<ValueType> 
    {
        private readonly System.Collections.Generic.List<ValueType> _values 
            = new System.Collections.Generic.List<ValueType>();

        private readonly object _locker = new object();

        public void Add(ValueType value)
        {
            lock(_locker)
            {
                _values.Add(value);
            }
        }

        public int Count {get{lock(_locker) return _values.Count;}}

        public bool TryExtractAll(out ValueType[] values)
        {
            values = null;

            if (_values.Count == 0)
            {
                return false;
            }
            else 
            {
                lock(_locker)
                {
                    if (_values.Count > 0) 
                    {
                        values = _values.ToArray();

                        _values.Clear();

                        return true;
                    }
                    else return false;
                }
            }
        }
    }
}