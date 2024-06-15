namespace Thread
{
    public enum Priority
    {
        Lowest = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        Highest = 4
    }
}

namespace Butterfly.system.objects.main.manager
{

    public sealed class Threads : Informing, dispatcher.IThreads
    {
        private readonly information.State _stateInformation;

        public Threads(informing.IMain mainInforming, information.State stateInformation)
            : base("ThreadsManager", mainInforming)
        {
            _stateInformation = stateInformation;
        }

        private global::System.Threading.Thread[] _threads = new global::System.Threading.Thread[0];
        private string[] _names = new string[0];
        private Int[] _timesDelay = new Int[0];
        private string[] _prioritys = new string[0];

        private Bool[] _isRuns = new Bool[0];

        private bool _isStopping = false;
        private readonly object _locker = new object();

        public void ReplaceTimeDelay(string name, uint value)
        {
            for (int i = 0; i < _threads.Length; i++)
            {
                if (_names[i] == name)
                {
                    _timesDelay[i].Value = (int)value;
                    return;
                }
            }

            Exception("Вы пытаетесь изменить timeDelay для несущесвующего потока.");
        }

        public void Add(string name, global::System.Action action, uint timeDelay, Thread.Priority priority)
        {
            if (_stateInformation.IsContruction)
            {
                Hellper.ExpendArray(ref _names, name);
                Hellper.ExpendArray(ref _isRuns, new Bool(true));
                Hellper.ExpendArray(ref _prioritys, priority.ToString());
                Hellper.ExpendArray(ref _timesDelay, (new Int(timeDelay)));

                Hellper.ExpendArray(ref _threads, new global::System.Threading.Thread(() =>
                {
                    int index = _names.Length - 1;
                    Int timeDelay = _timesDelay[index];
                    Bool isRun = _isRuns[index];

                    while (true)
                    {
                        if (isRun.Value)
                        {
                            action.Invoke();

                            global::System.Threading.Thread.Sleep(timeDelay.Value);
                        }
                        else return;
                    }
                }));

                _threads[_threads.Length - 1].Name = name;
                _threads[_threads.Length - 1].Priority = (global::System.Threading.ThreadPriority)priority;
            }
            else
                Exception(data.ThreadManager.x100001, name);
        }

        void dispatcher.IThreads.Start()
        {
            foreach (var thread in _threads)
                thread.Start();
        }

        void dispatcher.IThreads.TaskStop()
        {
            System.Threading.Tasks.Task.Run(((dispatcher.IThreads)this).Stop);
        }

        void dispatcher.IThreads.Stop()
        {
            lock (_locker)
            {
                if (_threads.Length == 0) return;

                _isStopping = true;

                string nameThreads = "";
                foreach (var nameThread in _threads)
                    nameThreads += nameThread.Name + " ";

                for (int i = 0; i < _isRuns.Length; i++)
                    _isRuns[i].False();

                bool[] isStopThreads = new bool[_threads.Length];

                int stopThreadsCount = 0;

                while (true)
                {
                    for (int i = 0; i < _threads.Length; i++)
                    {
                        if (isStopThreads[i] == false)
                        {
                            if (_threads[i].IsAlive)
                            {
                                //...
                            }
                            else
                            {
                                isStopThreads[i] = true;
                                stopThreadsCount++;

                                //SystemInformation($"StopThread:" + _threads[i].Name, System.ConsoleColor.Green);

                                if (stopThreadsCount == _threads.Length) break;
                            }
                        }
                    }

                    if (stopThreadsCount == _threads.Length) break;
                }

                for (int i = 0; i < _threads.Length; i++)
                {
                   SystemInformation($"StopThread:{_names[i]}", System.ConsoleColor.Green);
                }
            }
        }
    }
}