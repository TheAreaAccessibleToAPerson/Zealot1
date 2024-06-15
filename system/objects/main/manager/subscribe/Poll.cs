namespace Butterfly.system.objects.main.controller
{
    public class Poll : Informing
    {
        private readonly manager.description.ISubscribe _subscribeManager;

        private readonly information.State _stateInformation;
        private readonly information.Header _headerInformation;
        private readonly information.DOM _DOMInformation;

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        private byte _state = StateType.CREATING_TICKET;

        /// <summary>
        /// Мы выставили билеты на регистрацию. 
        /// </summary>
        /// <value></value>
        public bool IsRegisterSubscribe
        {
            get
            {
                return _state == StateType.REGISTER_SUBSCRIBE;
            }
        }

        /// <summary>
        /// Регистрация окончена. 
        /// </summary>
        /// <value></value>
        public bool IsSubscribe
        {
            get
            {
                return _state == StateType.END_SUBSCRIBE;
            }
        }

        private SubscribePollTicket[] _subscribeTickets = new SubscribePollTicket[0];

        /// <summary>
        /// Сдесь хранятся индексы пуллов в которых работают наши Action.
        /// </summary>
        private ulong[] _pointerToThePollID = new ulong[0];

        /// <summary>
        /// Указан индекс в массиве в менеджере пуллов где хранится наш обрабатываемый Action.
        /// </summary>
        private int[] _pointerIndexInArrayToThePoll = new int[0];

        /// <summary>
        /// Количесво подписаных билетов. Это значение нужно для того что бы при уничтожении
        /// обьекта дождатся пока мы отключимся от всех пулов на которые подписались.
        /// </summary>
        private int _subscribeTicketCount = 0;

        private readonly object _locker = new object();

        /// <summary>
        /// Имеются ли у нас регистрационые билеты. 
        /// </summary>
        /// <value></value>
        public bool IsRegisterTicket { private set; get; } = false;

        public Poll(informing.IMain mainInforming, information.State stateInformation,
            information.Header headerInformation, information.DOM DOMInformation,
                manager.description.ISubscribe subscribeManager)
            : base("SubscribePollManager", mainInforming)
        {
            _stateInformation = stateInformation;
            _headerInformation = headerInformation;
            _DOMInformation = DOMInformation;

            _subscribeManager = subscribeManager;
        }

        /// <summary>
        /// Ссылка на метод в PollManager с помощью которого пулл будет общатся со своим подписоным обьектом. 
        /// 1) Тип информирования.
        /// 2) Номер индекса в массиве SubsribePollManager.
        /// 3) ID пулла.
        /// 4) Номер индекса в массиве RootPollClients.
        /// </summary>
        public void ToInforming(root.poll.InformingType informingType, int indexSubsribePollManager,
            ulong idPoll, int indexRootPollClients)
        {
            lock (_locker)
            {
                // Сообщает что мы отовсюду отписались;
                if (informingType.HasFlag(root.poll.InformingType.EndUnsubscribe))
                {
                    if (_state == StateType.REGISTER_UNSUBSCRIBE)
                    {
                        if (_pointerToThePollID[indexSubsribePollManager] == idPoll &&
                            _pointerIndexInArrayToThePoll[indexSubsribePollManager] == indexRootPollClients)
                        {
                            if (--_subscribeTicketCount == 0)
                            {
                                _state = StateType.END_UNSUBSCRIBE;

                                _subscribeManager.EndUnsubscribe();
                            }
                            else if (_subscribeTicketCount < 0) throw new System.Exception();
                        }
                        else throw new System.Exception();
                    }
                    else throw new System.Exception();
                }
                else if (informingType.HasFlag(root.poll.InformingType.ChangeOfIndex))
                {
                    _pointerToThePollID[indexSubsribePollManager] = idPoll;
                    _pointerIndexInArrayToThePoll[indexSubsribePollManager] = indexRootPollClients;

                    // В момент отписки наше место положение изменилось, повторное информирование
                    // с новым местом лежит на нас.
                    if (_state == StateType.REGISTER_UNSUBSCRIBE)
                    {
                        _DOMInformation.RootManager.ActionInvoke(() =>
                        {
                            _DOMInformation.RootManager.AddUnsubscribeTickets(new UnsubscribePollTicket[]
                            {
                                new UnsubscribePollTicket()
                                {
                                    Name = _subscribeTickets[indexSubsribePollManager].Name,
                                    PollID = idPoll,
                                    IDObject = _DOMInformation.ID,
                                    IndexInRootPoll = indexRootPollClients,
                                    IndexInSubscribePollManager = indexSubsribePollManager
                                }
                            });
                        });
                    }
                }
                else if (informingType.HasFlag(root.poll.InformingType.EndSubscribe))
                {
                    if (_state == StateType.REGISTER_SUBSCRIBE)
                    {
                        // Запишем куда зарегестрировался наш тикет.
                        _pointerToThePollID[indexSubsribePollManager] = idPoll;
                        _pointerIndexInArrayToThePoll[indexSubsribePollManager] = indexRootPollClients;

                        // Дожидаемся пока все билеты откликрутся о завершении регистрации.
                        if (++_subscribeTicketCount == _pointerToThePollID.Length)
                        {
                            _state = StateType.END_SUBSCRIBE;

                            _subscribeManager.EndSubscribe();
                        }
                        else if (_subscribeTicketCount > _pointerToThePollID.Length) throw new System.Exception();
                    }
                    else throw new System.Exception();
                }
            }
        }

        /// <summary>
        /// Добавляет регистриационый билет для пулла потоков. 
        /// </summary>
        /// <param name="name">Имя пулла потоков куда нам будет неоходимо произвести подписку.</param>
        /// <param name="action">Action который нам предстоит обрабатывать.</param>
        public void Add(string name, System.Action action, byte type, uint timeDelay)
        {
            lock (_locker)
            {
                if (_headerInformation.IsNodeObject())
                {
                    IsRegisterTicket = true;

                    SubscribePollTicket ticket = new SubscribePollTicket()
                    {
                        Name = name,
                        Action = action,
                        Informing = ToInforming,
                        IDObject = _DOMInformation.ID,
                        IndexInSubscribePollManager = _subscribeTickets.Length,
                        TimeDelay = timeDelay,
                        Directory = _headerInformation.Directory
                    };

                    Hellper.ExpendArray(ref _subscribeTickets, ticket);
                }
                else ((main.description.IPoll)_DOMInformation.NodeObject).Add(name, action, type, timeDelay);
            }
        }

        public void Subscribe()
        {
            lock (_locker)
            {
                if (_state == StateType.CREATING_TICKET)
                {
                    _state = StateType.REGISTER_SUBSCRIBE;

                    // Сюда запишутся данные о том в каком пуле зарегестрирован наш билет.
                    // Если в последсвии билет будет передан в другой пулл, то
                    // он обязан будет сообщить о том где он в данный момент работает.
                    _pointerToThePollID = new ulong[_subscribeTickets.Length];

                    _pointerIndexInArrayToThePoll = new int[_subscribeTickets.Length];

                    _DOMInformation.RootManager.AddSubscribeTickets(_subscribeTickets);

                }
                else throw new System.Exception();
            }
        }

        /// <summary>
        /// Регистрируемся на отписку из пуллов.
        /// </summary>
        public void Unsubscribe()
        {
            lock (_locker)
            {
                if (_state == StateType.END_SUBSCRIBE)
                {
                    if (IsRegisterTicket)
                    {
                        _state = StateType.REGISTER_UNSUBSCRIBE;

                        UnsubscribePollTicket[] unsubscribePollTicket =
                            new UnsubscribePollTicket[_subscribeTickets.Length];

                        for (int i = 0; i < unsubscribePollTicket.Length; i++)
                        {
                            unsubscribePollTicket[i] = new UnsubscribePollTicket()
                            {
                                Name = _subscribeTickets[i].Name,
                                PollID = _pointerToThePollID[i],
                                IDObject = _DOMInformation.ID,
                                IndexInRootPoll = _pointerIndexInArrayToThePoll[i],
                                IndexInSubscribePollManager = i
                            };
                        }

                        _DOMInformation.RootManager.AddUnsubscribeTickets(unsubscribePollTicket);
                    }
                    else throw new System.Exception();
                }
                else throw new System.Exception();
            }
        }
    }
}