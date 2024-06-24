using System.Net;
using Butterfly;
using Zealot.hellper;
using MongoDB.Bson;

namespace Zealot.manager
{
    /// <summary>
    /// Сканируем сеть. 
    /// </summary>
    public sealed class ScanerDevices : Controller, ReadLine.IInformation
    {
        public struct BsonDocumentType
        {
            public const string HEADER = "header";

            public const string ADDRESS = "Address";
            public const string DIOPOZONE_ADDRESSES = "Diopozone addresses";
        }

        public const string NAME = "scan";

        public struct DB
        {
            public const string NAME = "ScanerDevices";
            public struct AddressCollection
            {
                public const string NAME = "AddressCollection";
                public const string KEY = "AddressKey";
            }
        }

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

        IInput<string> i_setState;

        IInput<string> i_addAddress;
        IInput<string> i_deleteAddress;
        IInput<string, string> i_addDiopozoneOfAddresses;
        IInput i_showAddresses;

        /// <summary>
        /// Запущен процесс сканирования?
        /// </summary> <summary>
        bool _isStart = false;
        IInput i_start;

        /// <summary>
        /// Начало диопозона. 
        /// </summary>
        /// <value></value>
        private string _startDiopozoneBuffer { set; get; } = "";

        private string _currentState = State.NONE;

        void Start()
        {
            Logger.S_I.To(this, "starting ...");
            {
                _currentState = State.WAIT_COMMAND;

                ReadLine.Start(this);
            }
            Logger.S_I.To(this, "start");
        }

        public void Command(string command)
        {
            Logger.I.To(this, $"Новая команда:{command}, current state:{_currentState}");

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStart)
                {
                    if (command == "stop")
                    {
                        _isStart = false;
                    }
                    else if (command == "start")
                    {
                        i_start.To();
                    }
                    else if (command == "show")
                    {
                        i_showAddresses.To();
                    }
                    else if (command == "add address")
                    {
                        i_setState.To(State.ADD_ADDRESS);
                    }
                    else if (command == "add addresses")
                    {
                        i_setState.To(State.ADD_ADDRESSES);
                    }
                    else if (command == "delete address")
                    {
                        i_setState.To(State.DELETE_ADDRESS);
                    }
                    else if (IPAddress.TryParse(command, out IPAddress ip))
                    {
                        if (_currentState == State.DELETE_ADDRESS)
                        {
                            Logger.I.To(this, $"Удаляем адрес {ip.ToString()}");

                            i_deleteAddress.To(ip.ToString());
                        }
                        else if (_currentState == State.ADD_ADDRESS)
                        {
                            Logger.I.To(this, $"Добавлен новый адрес {ip.ToString()}");

                            i_addAddress.To(ip.ToString());
                        }
                        else if (_currentState == State.ADD_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer != "")
                                Logger.S_E.To(this, $"На момент получение начала диопозона " +
                                    $"буффер хронящий временое значение начала диопозона не был пустым.");

                            _startDiopozoneBuffer = ip.ToString();

                            Logger.I.To(this, $"Получено начало диопозона {_startDiopozoneBuffer}.");

                            i_setState.To(State.INPUT_END_ADD_ADDRESSES);
                        }
                        else if (_currentState == State.INPUT_END_ADD_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer == "")
                                Logger.S_E.To(this, $"На момент получение конца диопозона " +
                                    $"буффер хронящий временое значение начала диопозона был пустым.");

                            Logger.I.To(this, $"Получен конец диопозона {ip.ToString()}.");

                            i_addDiopozoneOfAddresses.To(_startDiopozoneBuffer, ip.ToString());

                            i_setState.To(State.WAIT_COMMAND);

                            _startDiopozoneBuffer = "";
                        }
                        else Logger.W.To(this, $"Ввод ip адреса должен быть в контексте с командой.");
                    }
                    else if (command == "")
                    {
                        ReadLine.Input();
                    }
                    else
                    {
                        SystemInformation($"Доступные операции для {NAME}:\n" +
                            $"1)add address - добавить новый аддресс для сканирования(add address 192.168.0.1).\n" +
                            $"2)add addresses - добавить новые адреса, введите диопозон(add addresses 192.168.0.1 192.168.2.255).\n" +
                            $"3)show - показать все адресса и допозоны адрессов",
                                ConsoleColor.Yellow);

                        ReadLine.Input();
                    }
                }
                else Logger.CommandStateException.To(this, command, "Start", StateInformation.GetString());
            }
        }

        void Construction()
        {
            input_to(ref i_start, Header.Events.SCAN_DEVICES, () =>
            {
                SystemInformation("start scan", ConsoleColor.Green);

                if (_isStart == false)
                {
                    _isStart = true;

                    foreach (string address in _scanAddresses)
                    {
                        if (_isStart && StateInformation.IsStart)
                        {
                            Console("SCAN");
                            sleep(100);
                        }
                        else
                        {
                            SystemInformation("stop scan", ConsoleColor.Yellow);
                            return;
                        }
                    }

                    _isStart = false;
                    ReadLine.Input();
                }

                SystemInformation("end scan", ConsoleColor.Green);
            });

            input_to(ref i_showAddresses, Header.Events.SCAN_DEVICES, () =>
            {
                string a = "\nAddresses:\n";

                for (int i = 0; i < _addresses.Count; i++)
                    a += $"{i + 1}){_addresses[i]}\n";

                if (_diopozoneAddresses.Count > 0)
                {
                    a += "\nDiopozone addresses:\n";

                    for (int i = 0; i < _diopozoneAddresses.Count; i++)
                        a += $"{i + 1}){_diopozoneAddresses[i][0]}-{_diopozoneAddresses[i][1]}\n";
                }

                SystemInformation(a);

                ReadLine.Input();
            });

            input_to(ref i_deleteAddress, Header.Events.SCAN_DEVICES, (address) =>
            {
                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsStart)
                    {
                        // Данный адресс был найден.
                        bool m = false;
                        foreach (string a in _addresses)
                        {
                            if (a == address)
                            {
                                if (MongoDB.TryDeleteOne(DB.NAME, DB.AddressCollection.NAME,
                                    BsonDocumentType.ADDRESS, address, out string info))
                                {
                                    Logger.I.To(this, info);

                                    m = true;
                                }
                                else
                                {
                                    Logger.S_E.To(this, info);

                                    destroy();

                                    return;
                                }
                            }
                        }

                        if (m)
                        {
                            _scanAddresses.Remove(address);
                            _addresses.Remove(address);

                            foreach (string t in _scanAddresses)
                            {
                                if (t == address)
                                {
                                    Logger.S_E.To(this, $"Вы удалили адресс [{address}], но он попрежнему " +
                                        "остался в колекции [_scanAddresses]  в скорее всего он был продублирован.");

                                    destroy();

                                    return;
                                }
                            }

                            foreach (string t in _addresses)
                            {
                                if (t == address)
                                {
                                    Logger.S_E.To(this, $"Вы удалили адресс [{address}], но он попрежнему " +
                                        "остался в колекции [_addresses]  в скорее всего он был продублирован.");

                                    destroy();

                                    return;
                                }
                            }
                        }
                        else Logger.W.To(this, $"Вы пытаетесь удалить аддрес [{address}] " +
                            $"который у вас не хранится.");

                        i_setState.To(State.WAIT_COMMAND);
                    }
                }
            });

            input_to(ref i_addAddress, Header.Events.SCAN_DEVICES, (address) =>
            {
                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsStart)
                    {
                        foreach (string a in _addresses)
                        {
                            if (a == address)
                            {
                                Logger.W.To(this, $"Вы попытались дважды добавить ip {address}.");

                                return;
                            }
                        }

                        bool isNone = true;
                        {
                            foreach (string a in _scanAddresses)
                            {
                                if (a == address)
                                {
                                    // Если такой адресс уже есть то не добавляем его.
                                    isNone = false;
                                    break;
                                }
                            }
                        }
                        if (isNone)
                        {
                            Logger.I.To(this, $"Вы добавили новый аддесс [{address}] в коллекцию [_scanAddresses]");

                            _scanAddresses.Add(address);
                        }
                        else Logger.I.To(this, $"Данный аддресс уже [{address}] добавлен в [_scanAddresses]");

                        // Сначала запишим ...
                        _addresses.Add(address);

                        // Получаем все документы.
                        if (MongoDB.TryFind(DB.NAME, DB.AddressCollection.NAME, out string infoFindAllDoc,
                            out List<BsonDocument> allDoc))
                        {
                            Logger.I.To(this, infoFindAllDoc);

                            foreach (BsonDocument doc in allDoc)
                            {
                                if (doc[BsonDocumentType.HEADER] == BsonDocumentType.ADDRESS)
                                {
                                    if (doc[BsonDocumentType.ADDRESS] == address)
                                    {
                                        Logger.W.To(this, "Вы ввели ip аддресс который уже был добавлен.");

                                        i_setState.To(State.WAIT_COMMAND);

                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Logger.S_E.To(this, infoFindAllDoc);

                            destroy();

                            return;
                        }

                        // Затем данное значение передадим в базу данных.
                        if (MongoDB.TryInsertOne(DB.NAME, DB.AddressCollection.NAME,
                            out string info, new BsonDocument()
                            {
                                { BsonDocumentType.HEADER, BsonDocumentType.ADDRESS},
                                { BsonDocumentType.ADDRESS, address}
                            }))
                        {
                            Logger.S_I.To(this, info);

                            i_setState.To(State.WAIT_COMMAND);
                        }
                        else
                        {
                            Logger.S_E.To(this, info);

                            destroy();

                            return;
                        }
                        Logger.I.To(this, $"Вы добавили новый адрес для скана ip {address}.");
                    }
                    else Logger.W.To(this, $"Не удалось добавить ip для скана {address}.");
                }
            });

            input_to(ref i_addDiopozoneOfAddresses, Header.Events.SCAN_DEVICES, (firstAddress, lastAddress) =>
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
                    _diopozoneAddresses.Add(new string[] { firstAddress, lastAddress });

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

                    // Затем данное значение передадим в базу данных.
                    if (MongoDB.TryInsertOne(DB.NAME, DB.AddressCollection.NAME,
                        out string info, new BsonDocument()
                        {
                            { BsonDocumentType.HEADER, BsonDocumentType.DIOPOZONE_ADDRESSES},
                            { BsonDocumentType.DIOPOZONE_ADDRESSES, $"{firstAddress}-{lastAddress}" }
                        }))
                    {
                        Logger.S_I.To(this, info);
                    }
                    else
                    {
                        Logger.S_E.To(this, info);

                        destroy();

                        return;
                    }
                }
                else Logger.Ws.To(this, $"Не удалось добавить ip адресса для скана:", m);
            });

            input_to(ref i_setState, Header.Events.SYSTEM, (nextState) =>
            {
                Logger.I.To(this, $"Попытка сменить текущее состояние [{_currentState}] на [{nextState}].");

                if (StateInformation.IsStart || StateInformation.IsStarting)
                {
                    if (_currentState == State.WAIT_COMMAND)
                    {
                        if (hellper.State.Contains(nextState, new string[]
                        {
                            State.ADD_ADDRESS, State.ADD_ADDRESSES,
                            State.DELETE_ADDRESS, State.DELETE_ADDRESSES,
                        }
                        ))
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");
                            _currentState = nextState;

                            if (nextState == State.ADD_ADDRESS || nextState == State.DELETE_ADDRESS)
                                ReadLine.Input(NAME, "Введите адрес:");
                            else if (nextState == State.ADD_ADDRESSES)
                                ReadLine.Input(NAME, "Введите начало диапозона адресов:");
                            else
                                ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"Сменить состояние выполнения команды c " +
                            $"{State.WAIT_COMMAND} можно только на:" +
                            $"\n1){State.ADD_ADDRESS}" +
                            $"\n2){State.ADD_ADDRESSES}" +
                            $"\n3){State.DELETE_ADDRESS}" +
                            $"\n4){State.DELETE_ADDRESSES}" +
                            $",но вы попытались сменить на {nextState}.");
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

                            ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"Сменить состояние выполнения команды на " +
                            $"{State.ADD_ADDRESS} можно только c:" +
                            $"\n4){State.WAIT_COMMAND}, вы попытались сменить c {nextState}.");
                    }
                    else if (_currentState == State.INPUT_END_ADD_ADDRESSES)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.WAIT_COMMAND;

                            ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"С [{State.INPUT_END_ADD_ADDRESSES}] можно " +
                            $" перейти только в состояние [{State.WAIT_COMMAND}]. " +
                            $"Но вы попытались перейти в состояние {nextState}.");
                    }
                    else if (_currentState == State.ADD_ADDRESS)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.WAIT_COMMAND;

                            ReadLine.Input();
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

                            ReadLine.Input(NAME, "Введите конец диопозона.");
                        }
                        else Logger.S_E.To(this, $"Вы пытаетесь сменить состояние c [{State.ADD_ADDRESSES}] " +
                            $"на [{nextState}. Но ожидается что вы смените на [{State.INPUT_END_ADD_ADDRESSES}].");
                    }
                    else if (_currentState == State.DELETE_ADDRESS)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;

                            ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"Произвести смену состояния ввода на {State.DELETE_ADDRESS}" +
                            $"можно только из состояния {State.WAIT_COMMAND}. Была произведена попытка смены " +
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
                    else
                    {
                        Logger.S_E.To(this, $"Вы пытаетесь установить состояния о котором ничего не известно.");

                        destroy();

                        return;
                    }
                }
                else Logger.W.To(this, "Неудалось сменить стояние выполнение команды." +
                    $"Сменить состояние команды можно только если обьект находится в состоянии " +
                        $"Star или Starting. В текущий момент обьект находится в состоянии {StateInformation.GetString()}");
            });
        }

        void Configurate()
        {
            Logger.S_I.To(this, "start configurate ...");
            {
                // Проверяем сущесвует ли база данных DeviceScaner
                // Если нет то создадим ее.
                if (MongoDB.ContainsDatabase(DB.NAME, out string containsDBerror))
                {
                    Logger.S_I.To(this, $"База данныx {DB.NAME} уже создана.");
                }
                else
                {
                    if (containsDBerror != "")
                    {
                        Logger.S_E.To(this, containsDBerror);

                        destroy();

                        return;
                    }
                    else
                    {
                        Logger.S_I.To(this, $"Создаем базу данных {DB.NAME}.");

                        if (MongoDB.TryCreatingDatabase(DB.NAME, out string info))
                        {
                            Logger.S_I.To(this, info);
                        }
                        else
                        {
                            Logger.S_E.To(this, info);

                            destroy();

                            return;
                        }
                    }
                }

                // Проверяем наличие коллекции.
                if (MongoDB.ContainsCollection<BsonDocument>(DB.NAME, DB.AddressCollection.NAME,
                    out string error))
                {
                    Logger.S_I.To(this, $"Коллекция [{DB.AddressCollection.NAME}] в базе данных " +
                        $" [{DB.NAME}] уже создана.");
                }
                else
                {
                    // Коллекции нету, создадим ее.
                    if (error == "")
                    {
                        if (MongoDB.TryCreatingCollection(DB.NAME, DB.AddressCollection.NAME,
                            out string info))
                        {
                            Logger.S_I.To(this, info);
                        }
                        else
                        {
                            Logger.S_E.To(this, info);

                            destroy();

                            return;
                        }
                    }
                    else
                    {
                        Logger.S_E.To(this, error);

                        destroy();

                        return;
                    }
                }

                /*
                BsonDocument
                {
                    string[] { "127.0.0.1", "...", "...", }
                    string[] { "127.0.0.1-127.0.0.5", "...", "...", }
                }
                */

                // Проверяем наличие документа хранящего адреса и диопазоны адресов.
                if (MongoDB.TryFind(DB.NAME, DB.AddressCollection.NAME, out string findInfo,
                    out List<BsonDocument> addresses))
                {
                    Logger.S_I.To(this, findInfo);

                    if (addresses != null)
                    {
                        try
                        {
                            foreach (BsonDocument doc in addresses)
                            {
                                if (doc[BsonDocumentType.HEADER] == BsonDocumentType.ADDRESS)
                                {
                                    string a = doc[BsonDocumentType.ADDRESS].ToString();

                                    Logger.S_I.To(this, $"Получили ip аддресс из базы данных [{a}]");

                                    _addresses.Add(a);
                                    _scanAddresses.Add(a);
                                }
                            }

                            foreach (BsonDocument doc in addresses)
                            {
                                if (doc[BsonDocumentType.HEADER] == BsonDocumentType.DIOPOZONE_ADDRESSES)
                                {
                                    if (Address.ConvertDioposoneAddresses
                                        (doc[BsonDocumentType.DIOPOZONE_ADDRESSES].ToString(),
                                            out string[] resultConvert,
                                                out string errorConvertDioposoneAddresses))
                                    {
                                        string a = doc[BsonDocumentType.DIOPOZONE_ADDRESSES].ToString();

                                        Logger.S_I.To(this, $"Получили диопозон ip аддрессов из базы данных [{a}]");

                                        List<string> diopozoneAddresses = Address.GetAddresses(new Address.Values()
                                        {
                                            DiopozonesOfAddresses = new Address.DiopozoneOfAddresses[1]
                                            {
                                                new Address.DiopozoneOfAddresses(resultConvert[0], resultConvert[1])
                                            }
                                        });

                                        foreach (string t in diopozoneAddresses)
                                        {
                                            foreach (string r in _addresses)
                                            {
                                                if (r == t) break;
                                            }

                                            _scanAddresses.Add(t);
                                        }

                                        _diopozoneAddresses.Add(resultConvert);
                                    }
                                    else
                                    {
                                        Logger.S_E.To(this, errorConvertDioposoneAddresses);

                                        destroy();

                                        return;
                                    }
                                }
                                else
                                {
                                    Logger.S_E.To(this, "Неизветный тип заголовка BsonDocument " +
                                        $"который должен хранить адресс или диапоозон адрессов.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.S_E.To(this, ex.ToString());

                            destroy();

                            return;
                        }
                    }
                }
                else
                {
                    Logger.S_E.To(this, findInfo);

                    destroy();

                    return;
                }
            }
            Logger.S_I.To(this, "end configurate");
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

            public const string SHOW_ADDRESSES = "Проказать адресса для скана";
        }
    }
}