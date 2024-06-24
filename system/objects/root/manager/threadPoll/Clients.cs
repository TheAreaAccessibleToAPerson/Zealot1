namespace Butterfly.system.objects.root.manager
{
    public abstract class Clients : Controller.Board.LocalField<poll.Fields>
    {
        /// <summary>
        /// Текущее количесво клиентов. 
        /// </summary>
        /// <value></value>
        public int Count { private set; get; } = 0;

        /// <summary>
        /// ID текущего пулла. 
        /// </summary>
        /// <value></value>
        public ulong ID { get { return Field.ID; } }

        /// <summary>
        /// Имя текущего пулла. 
        /// </summary>
        /// <value></value>
        public string Name { get { return Field.Name; } }

        /// <summary>
        /// Xранятся исполняемые Actions клинтов.
        /// </summary>
        private System.Action[] _runs;

        /// <summary>
        /// Хранит уникальные id клинтов. 
        /// </summary>
        private ulong[] _idClients;

        /// <summary>
        /// Временые промежутки для клиeнтов.
        /// </summary>
        private int[] _timeDelays;

        /// <summary>
        /// Временные промежутки которые клинты уже ожидают.
        /// </summary>
        private int[] _stepsTime;

        /// <summary>
        /// Хранятся методы для информирования клинта.
        /// </summary>
        private System.Action<root.poll.InformingType, int, ulong, int>[] _toInformingClients;

        /// <summary>
        /// Индекс в массиве в PollSubscribeManager. 
        /// </summary>
        private int[] _indexInSubscribePollManager;

        private object locker = new object();

        /// <summary>
        /// Задает максимальный размер учасников пулла.
        /// </summary>
        /// <param name="size">Mаксимальное число учасников пулла.</param>
        public void DefineSize(uint size)
        {
            _runs = new System.Action[size];
            _idClients = new ulong[size];
            _toInformingClients = new System.Action<root.poll.InformingType, int, ulong, int>[size];
            _indexInSubscribePollManager = new int[size];
            _timeDelays = new int[size];
            _stepsTime = new int[size];
        }

        /// <summary>
        /// Интекс пустого слота под клинта. 
        /// </summary>
        private int _indexEmptySlot = 0;

        public void Run()
        {
            int stepTime = 0;
            if (current_timer() > 0)
                stepTime = step_timer();

            //Console(stepTime);

            int[] timeDelays = _timeDelays;
            int[] stepsTime = _stepsTime;

            for (int i = 0; i < _indexEmptySlot; i++)
            {
                int timeDelay = timeDelays[i];

                if (timeDelay > 0) 
                {
                    if (stepTime == 0) continue;

                    stepsTime[i] -= stepTime;
                    int delay = stepsTime[i];

                    if (delay <= 0)
                    {
                        _stepsTime[i] = timeDelay;
                    }
                    else
                    {
                        _stepsTime[i] = delay;

                        continue;
                    }
                }

                _runs[i].Invoke();
            }
        }


        public void Add(main.SubscribePollTicket ticket)
        {
            lock (locker)
            {
                if (_indexEmptySlot == Field.Size)
                    Exception($"Превышено количесво учасников для события {Name}.");

                _runs[_indexEmptySlot] = ticket.Action;
                _toInformingClients[_indexEmptySlot] = ticket.Informing;
                _idClients[_indexEmptySlot] = ticket.IDObject;
                _indexInSubscribePollManager[_indexEmptySlot] = ticket.IndexInSubscribePollManager;
                _timeDelays[_indexEmptySlot] = (int)ticket.TimeDelay;

                ticket.Informing.Invoke(root.poll.InformingType.EndSubscribe,
                    ticket.IndexInSubscribePollManager, ID, _indexEmptySlot);

                Count++;
                _indexEmptySlot++;
            }
        }

        protected void Add(main.UnsubscribePollTicket ticket)
        {
            lock (locker)
            {
                // Индекс по которому преположительно хранится наш клиент.
                int index = ticket.IndexInRootPoll;

                // Проверяем не переместили ли клинта в другой слот.
                if (_idClients[index] == ticket.IDObject)
                {
                    // Один и тот же клиент может несколько раз подписаться в один и тот же пулл.
                    // проверяем номер билета на совподение.
                    if (_indexInSubscribePollManager[index]
                        == ticket.IndexInSubscribePollManager)
                    {
                        Count--;

                        //_runs[index].Invoke();

                        // Проинформирем клинта что он отключон.
                        _toInformingClients[index].Invoke(root.poll.InformingType.EndUnsubscribe,
                            ticket.IndexInSubscribePollManager, ID, index);

                        _runs[index] = null;
                        _toInformingClients[index] = null;
                        _idClients[index] = ulong.MaxValue;
                        _indexInSubscribePollManager[index] = -1;
                        _timeDelays[index] = -1;
                        _stepsTime[index] = -1;

                        if (_indexEmptySlot == 0 || ((_indexEmptySlot + 1) == index))
                        {
                            return;
                        }
                        else if ((_indexEmptySlot == 1) || ((_indexEmptySlot - 1) == index))
                        {
                            --_indexEmptySlot;
                            return;
                        }

                        // Декриментируем крайнее значение. Так как крайний клиент переедит 
                        // в освободившийся слот.
                        _indexEmptySlot--;

                        _runs[index] = _runs[_indexEmptySlot];
                        _toInformingClients[index] = _toInformingClients[_indexEmptySlot];
                        _idClients[index] = _idClients[_indexEmptySlot];
                        _indexInSubscribePollManager[index] = _indexInSubscribePollManager[_indexEmptySlot];
                        _timeDelays[index] = _timeDelays[_indexEmptySlot];
                        _stepsTime[index] = _stepsTime[_indexEmptySlot];


                        _toInformingClients[index].Invoke(root.poll.InformingType.ChangeOfIndex,
                                _indexInSubscribePollManager[index], ID, index);

                        _runs[_indexEmptySlot] = null;
                        _toInformingClients[_indexEmptySlot] = null;
                        _idClients[_indexEmptySlot] = ulong.MaxValue;
                        _indexInSubscribePollManager[_indexEmptySlot] = -1;
                        _timeDelays[_indexEmptySlot] = -1;
                        _stepsTime[_indexEmptySlot] = -1;
                    }
                }

            }
        }
    }
}