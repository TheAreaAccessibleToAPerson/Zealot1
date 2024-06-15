using System.Collections.Concurrent;
using System.Linq;

namespace Butterfly.system.objects.main
{
    /// <summary>
    /// Описывает способ извлечения данных. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtract<T>
    {
        void To(System.Action<T[]> action);
    }

    /// <summary>
    /// Описывает способ сброса хранимых даных в безопасном обьекте. 
    /// </summary>
    public interface ISafeListenPollRestart
    {
        void Process();
    }

    public class SafeListenMessagePoll<T> : Redirect<T, System.Action>, IInput<T>,
        IExtract<T>, ISafeListenPollRestart, InformationGlobalObject.IReplace, IUpdate, IInputConnect
    {
        private readonly ConcurrentBag<T> _values = new ConcurrentBag<T>();

        private readonly System.Collections.Generic.Dictionary<uint, T> _bufferValues
            = new System.Collections.Generic.Dictionary<uint, T>();

        private uint _bufferIndex = 0;
        private uint _bufferIndexReport = 0;

        public SafeListenMessagePoll(IInformation information)
            : base(information) { }

        void IInput<T>.To(T value)
            => _values.Add(value);

        public void Update()
        {
            int count = _values.Count;

            if (count == 0) return;

            do
            {
                if (_bufferIndex == uint.MaxValue)
                    _bufferIndex = 0;

                if (_values.TryTake(out T value))
                {
                    _bufferIndexReport = _bufferIndex;
                    _bufferValues.Add(_bufferIndex++, value);
                    Input.To(value, Report);
                }
            }
            while ((--count) > 0);
        }

        void Report()
        {
            if (_bufferValues.Remove(_bufferIndexReport) == false)
                throw new System.Exception();
        }

        object IInputConnect.GetConnect() => this;

        void ISafeListenPollRestart.Process()
        {
            foreach (T value in _bufferValues.Values)
                _values.Add(value);

            _bufferIndex = 0;

            _bufferValues.Clear();
        }

        void IExtract<T>.To(System.Action<T[]> action)
        {
            T[] buffer = new T[_values.Count + _bufferValues.Count];

            _bufferValues.Values.ToArray().CopyTo(buffer, 0);

            for (int i = _bufferValues.Count; i < buffer.Length; i++)
            {
                if (_values.TryTake(out T value))
                {
                    buffer[i] = value;
                }
            }

            action.Invoke(buffer);

            _bufferValues.Clear();
        }

        void InformationGlobalObject.IReplace.Process(IInformation information)
            => Information = information;
    }
}