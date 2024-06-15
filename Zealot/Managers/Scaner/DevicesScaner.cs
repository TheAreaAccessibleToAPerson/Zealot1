using System.Net;
using Butterfly;
using Zealot.hellper;

namespace Zealot.manager
{
    /// <summary>
    /// Сканируем сеть. 
    /// </summary>
    public sealed class ScanerDevices : Controller, ReadLine.IInformation
    {
        public const string NAME = "scan";

        /// <summary>
        /// Адреса которые сканируются. 
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private readonly List<string> _scanAddresses = new List<string>();

        /// <summary>
        /// Одиночно добавленые адреса.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private readonly List<string> _addresses = new List<string>();
        /// <summary>
        /// Диапозоны адресов.
        /// </summary>
        /// <typeparam name="string[]"></typeparam>
        /// <returns></returns>
        private readonly List<string[]> _diopozoneAddresses = new List<string[]>();

        IInput<string> i_addAddress;
        IInput<string, string> i_addDiopozoneOfAddresses;

        /// <summary>
        /// Начало диопозона. 
        /// </summary>
        /// <value></value>
        private string _startDiopozoneBuffer { set; get; } = "";

        private string _currentState = State.NONE;

        void Start()
        {
            _currentState = State.WAIT_COMMAND;
            ReadLine.Start(this);
        }

        bool SetState(string nextState)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart || StateInformation.IsStarting)
                {
                    if (_currentState == State.WAIT_COMMAND)
                    {
                        if (hellper.State.Contains(nextState, new string[]
                            {
                                State.ADD_ADDRESS, State.ADD_ADDRESSES,
                                State.DELETE_ADDRESS, State.DELETE_ADDRESSES
                            }
                        ))
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;
                        }
                        else Logger.S_E.To(this, $"Сменить состояние выполнения команды c " +
                            $"{State.WAIT_COMMAND} можно только на:" +
                            $"\n1){State.ADD_ADDRESS}" +
                            $"\n2){State.ADD_ADDRESSES}" +
                            $"\n3){State.DELETE_ADDRESS}" +
                            $"\n4){State.DELETE_ADDRESSES}, вы попытались сменить на {nextState}.");
                    }
                    else if (hellper.State.Contains(nextState, new string[]
                        {
                           // State.INPUT_ADD_ADDRESS, State.INPUT_DELETE_ADDRESS,
                           // State.INPUT_ADD_ADDRESSES, State.INPUT_DELETE_ADDRESSES
                        }
                    ))
                    {
                        if (nextState == State.ADD_ADDRESS)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;
                        }
                        else Logger.S_E.To(this, $"Сменить состояние выполнения команды на " +
                            $"{State.ADD_ADDRESS} можно только c:" +
                            $"\n4){State.WAIT_COMMAND}, вы попытались сменить c {nextState}.");
                    }
                    else if (_currentState == State.ADD_ADDRESS)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.WAIT_COMMAND;
                        }
                        else Logger.S_E.To(this, $"С [{State.ADD_ADDRESS}] можно " + 
                            $" перейти только в состояние [{State.WAIT_COMMAND}]. " + 
                            $"Но вы попытались перейти в состояние {nextState}.");
                    }
                    else if (_currentState == State.ADD_ADDRESSES)
                    {
                        if (nextState == State.INPUT_END_ADD_ADDRESSES)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.INPUT_END_ADD_ADDRESSES;
                        }
                        else Logger.S_E.To(this, $"Вы пытаетесь сменить состояние c [{State.ADD_ADDRESSES}] " + 
                            $"на [{nextState}. Но ожидается что вы смените на [{State.INPUT_END_ADD_ADDRESSES}].");
                    }
                    else if (_currentState == State.DELETE_ADDRESS)
                    {
                        if (nextState == State.INPUT_DELETE_ADDRESS)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;
                        }
                        else Logger.S_E.To(this, $"Произвести смену состояния ввода на {State.DELETE_ADDRESS}" +
                            $"можно только из состояния {State.INPUT_DELETE_ADDRESS}. Была произведена попытка смены " +
                            $" на {nextState}.");
                    }
                    else if (_currentState == State.DELETE_ADDRESSES)
                    {
                        if (nextState == State.INPUT_DELETE_ADDRESSES)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;
                        }
                        else Logger.S_E.To(this, $"Произвести смену состояния ввода на {State.DELETE_ADDRESSES}" +
                            $"можно только из состояния {State.INPUT_DELETE_ADDRESSES}. Была произведена попытка смены " +
                            $" на {nextState}.");
                    }
                }
                else Logger.W.To(this, "Неудалось сменить стояние выполнение команды." +
                    $"Сменить состояние команды можно только если обьект находится в состоянии " +
                        $"Star или Starting. В текущий момент обьект находится в состоянии {StateInformation.GetString()}");

                return true;
            }
        }

        public void Command(string command)
        {
            Logger.I.To(this, $"Новая команда:{command}");

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart)
                {
                    if (command == "add address")
                    {
                        SetState(State.ADD_ADDRESS);

                        ReadLine.Input(NAME, "Введите адрес:");
                    }
                    else if (command == "add addresses")
                    {
                        SetState(State.ADD_ADDRESSES);

                        ReadLine.Input(NAME, "Введите начало диапозона адресов:");
                    }
                    else if (IPAddress.TryParse(command, out IPAddress ip))
                    {
                        if (_currentState == State.ADD_ADDRESS)
                        {
                            Logger.I.To(this, $"Добавлен новый адрес {ip.ToString()}");

                            i_addAddress.To(ip.ToString());

                            SetState(State.WAIT_COMMAND);

                            ReadLine.Input();
                        }
                        else if (_currentState == State.ADD_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer != "")
                                Logger.S_E.To(this, $"На момент получение начала диопозона " + 
                                    $"буффер хронящий временое значение начала диопозона не был пустым.");

                            _startDiopozoneBuffer = ip.ToString();

                            Logger.I.To(this, $"Получено начало диопозона {_startDiopozoneBuffer}.");

                            SetState(State.INPUT_END_ADD_ADDRESSES);

                            ReadLine.Input(NAME, "Введите конец диапозона адресов:");
                        }
                        else if (_currentState == State.INPUT_END_ADD_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer == "")
                                Logger.S_E.To(this, $"На момент получение конца диопозона " + 
                                    $"буффер хронящий временое значение начала диопозона был пустым.");

                            Logger.I.To(this, $"Получен конец диопозона {ip.ToString()}.");

                            i_addDiopozoneOfAddresses.To(_startDiopozoneBuffer, ip.ToString());

                            SetState(State.WAIT_COMMAND);

                            _startDiopozoneBuffer = "";

                            ReadLine.Input(NAME, "Введите конец диапозона адресов:");
                        }
                        else Logger.W.To(this, $"Ввод ip адреса должен быть в контексте с командой.");
                    }
                    else if (command == "")
                    {
                        //...
                    }
                    else
                    {
                        SystemInformation($"Доступные операции для {NAME}:\n" +
                            $"1)add address - добавить новый аддресс для сканирования(add address 192.168.0.1).\n" +
                            $"2)add addresses - добавить новые адреса, введите диопозон(add addresses 192.168.0.1 192.168.2.255).\n",
                                ConsoleColor.Yellow);

                        ReadLine.Input();
                    }
                }
                else Logger.CommandStateException.To(this, command, "Start", StateInformation.GetString());
            }
        }

        void Construction()
        {
            input_to(ref i_addAddress, Header.Events.SYSTEM, (address) =>
            {
                if (StateInformation.IsStart)
                {
                    foreach (string a in _scanAddresses)
                    {
                        if (a == address)
                        {
                            Logger.W.To(this, $"Вы попытались дважды добавить ip {address}.");

                            return;
                        }
                    }

                    Logger.I.To(this, $"Вы добавили новый адрес для скана ip {address}.");

                    _scanAddresses.Add(address);
                    _addresses.Add(address);
                }
                else Logger.W.To(this, $"Не удалось добавить ip для скана {address}.");
            });

            input_to(ref i_addDiopozoneOfAddresses, Header.Events.SYSTEM, (firstAddress, lastAddress) =>
            {
                List<string> m = Address.GetAddresses(new Address.Values()
                {
                    DiopozonesOfAddresses = new Address.DiopozoneOfAddresses[1]
                    {
                        new Address.DiopozoneOfAddresses(firstAddress, lastAddress)
                    }
                });

                if (StateInformation.IsStart)
                {
                    foreach (string s in m)
                    {
                        // Совподений нету.
                        bool result = true;
                        foreach (string t in _scanAddresses)
                        {
                            if (t == s)
                            {
                                Logger.W.To(this, $"Вы попытались дважды добавить ip {s}.");

                                result = false;

                                break;
                            }
                        }

                        Logger.I.To(this, $"Вы добавили новый ip для скана:{s}");

                        if (result) _scanAddresses.Add(s);
                    }
                }
                else Logger.Ws.To(this, $"Не удалось добавить ip адресса для скана:", m);
            });
        }

        public struct State
        {
            public const string NONE = "NONE";
            public const string WAIT_COMMAND = "Ожидает команды";
            public const string ADD_ADDRESS = "Добавить новый адрес";
            public const string DELETE_ADDRESS = "Удалить адрес";
            public const string INPUT_DELETE_ADDRESS = "Ввод удаляемого адреса";
            public const string ADD_ADDRESSES = "Дабавить новые адресса";
            public const string INPUT_START_ADD_ADDRESSES = "Ввод новых адресов, начальный диапозон.";
            public const string INPUT_END_ADD_ADDRESSES = "Ввод новых адресов, конец диапозона включительно.";
            public const string DELETE_ADDRESSES = "Удалить адресса";
            public const string INPUT_DELETE_ADDRESSES = "Ввод удаляемых адрессов";
        }
    }
}