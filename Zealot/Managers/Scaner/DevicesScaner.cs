using Butterfly;
using System.Net;
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

        /// <summary>
        /// Сдесь записаны все адреса из диопазонов.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private readonly Dictionary<string, string[]> _diopozoneAddressesList = new();

        IInput<string> i_setState;

        IInput<string> i_addAddress;
        IInput<string> i_deleteAddress;
        IInput<string, string> i_addDiopozoneOfAddresses;
        IInput<string, string> i_deleteDiopozoneOfAddresses;
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
            command = command.TrimStart().TrimEnd();

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
                    else if (command == "delete addresses")
                    {
                        i_setState.To(State.DELETE_ADDRESSES);
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
                        else if (_currentState == State.DELETE_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer != "")
                                Logger.S_E.To(this, $"На момент получение начала диопозона " +
                                    $"буффер хронящий временое значение начала диопозона не был пустым.");

                            _startDiopozoneBuffer = ip.ToString();

                            Logger.I.To(this, $"Получено начало диопозона {_startDiopozoneBuffer}.");

                            i_setState.To(State.INPUT_END_DELETE_ADDRESSES);
                        }
                        else if (_currentState == State.INPUT_END_DELETE_ADDRESSES)
                        {
                            if (_startDiopozoneBuffer == "")
                                Logger.S_E.To(this, $"На момент получение конца диопозона " +
                                    $"буффер хронящий временое значение начала диопозона был пустым.");

                            Logger.I.To(this, $"Получен конец диопозона {ip.ToString()}.");

                            i_deleteDiopozoneOfAddresses.To(_startDiopozoneBuffer, ip.ToString());

                            _startDiopozoneBuffer = "";

                            i_setState.To(State.WAIT_COMMAND);
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

                            _startDiopozoneBuffer = "";

                            i_setState.To(State.WAIT_COMMAND);
                        }
                        else i_setState.To(State.WAIT_COMMAND);
                        //else Logger.W.To(this, $"Ввод ip адреса должен быть в контексте с командой.");
                    }
                    else if (command == "info")
                    {
                        SystemInformation($"Доступные операции для {NAME}:\n" +
                            $"1)add address - добавить новый аддресс для сканирования(add address 192.168.0.1).\n" +
                            $"2)add addresses - добавить новые адреса, введите диопозон(add addresses 192.168.0.1 192.168.2.255).\n" +
                            $"3)show - показать все адресса и допозоны адрессов",
                                ConsoleColor.Yellow);

                        ReadLine.Input();
                    }
                    else
                    {
                        if (_currentState == State.INPUT_END_DELETE_ADDRESSES ||
                            _currentState == State.INPUT_END_ADD_ADDRESSES)
                        {
                            Logger.I.To(this, $"Ввод диопазона адрессов был прерван," +
                                $" очитим буффер хранящий начало диопазона {_startDiopozoneBuffer}");

                            _startDiopozoneBuffer = "";
                        }

                        i_setState.To(State.WAIT_COMMAND);
                    }
                }
                else Logger.CommandStateException.To(this, command, "Start", StateInformation.GetString());
            }
        }

        IInput i_runUpdate;
        IInput i_stoppingUpdate;
        bool _isUpdate;

        void Construction()
        {
            add_event(Header.Events.SCAN_DEVICES, 60000, () =>
            {
                if (_isUpdate && _isStart == false)
                {
                    i_start.To();
                }
            });

            input_to(ref i_runUpdate, Header.Events.SCAN_DEVICES, () =>
            {
                lock (StateInformation.Locker)
                {
                    if (!StateInformation.IsDestroy && StateInformation.IsStart)
                    {
                        if (_isStart == false && _isUpdate == false)
                        {
                            Logger.I.To(this, "Получена комманда на начало скана сети.");

                            _isUpdate = true;
                        }
                    }
                }
            });

            input_to_0_1<string>(ref i_start, Header.Events.SCAN_DEVICES, (@return) =>
            {
                lock (StateInformation.Locker)
                {
                    if (!StateInformation.IsDestroy && StateInformation.IsStart)
                    {
                        if (_isStart == false)
                        {
                            Logger.I.To(this, "Получена комманда на начало скана сети.");

                            _isStart = true;

                            @return.To(NAME);
                        }
                    }
                }
            }).send_echo_to<string[]>(Devices.BUS.GET_ADDRESSES_CONNECTION_DEVICES)
                .output_to((connectionAddresses) =>
                {
                    SystemInformation("start scan", ConsoleColor.Green);

                    string[] buffer;
                    lock (StateInformation.Locker)
                        buffer = _scanAddresses.ToArray();

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        bool isConnection = false;
                        {
                            for (int u = 0; u < connectionAddresses.Length; u++)
                            {
                                if (connectionAddresses[u] == buffer[i])
                                {
                                    // Данное устройсво уже обрабатывается.
                                    isConnection = true;
                                    break;
                                }
                            }
                        }
                        if (isConnection) break;

                        if (_isStart && StateInformation.IsStart)
                        {
                            obj<NetRequest>(buffer[i], buffer[i]);
                        }
                        else
                        {
                            SystemInformation("stop scan", ConsoleColor.Yellow);

                            return;
                        }
                    }

                    _isStart = false;

                    ReadLine.Input();

                },
                Header.Events.SCAN_DEVICES);

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

                                    break;
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
                            _addresses.Remove(address);

                            // Проверим есть ли этот адресс где либо в диопозоне аддрессов.
                            bool y = false;
                            foreach (string[] ad in _diopozoneAddressesList.Values)
                            {
                                if (y) break;

                                for (int i = 0; i < ad.Length; i++)
                                {
                                    if (address == ad[i])
                                    {
                                        y = true;
                                        break;
                                    }
                                }
                            }

                            if (y)
                            {
                                Logger.S_I.To(this, $"Данный адреес {address} испозуется также в диопозоне " +
                                    $"поэтому его не нужно удалять из коллекции _scanAddress.");
                            }
                            else
                            {
                                Logger.S_I.To(this, $"Данный адреес {address} не используется ни одним диопозоном " +
                                    $"поэтому его не можно удалять из коллекции _scanAddress.");

                                _scanAddresses.Remove(address);
                            }

                            foreach (string t in _scanAddresses)
                            {
                                if (t == address)
                                {
                                    Logger.S_E.To(this, $"Вы удалили адресс [{address}], но он попрежнему " +
                                        "остался в колекции [_scanAddresses] скорее всего он был продублирован.");

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

                                i_setState.To(State.WAIT_COMMAND);

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

            input_to(ref i_deleteDiopozoneOfAddresses, Header.Events.SCAN_DEVICES, (firstAddress, lastAddress) =>
            {
                List<string> m = Address.GetAddresses(new Address.Values()
                {
                    DiopozonesOfAddresses = new Address.DiopozoneOfAddresses[1]
                    {
                        new Address.DiopozoneOfAddresses(firstAddress, lastAddress)
                    }
                });

                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsStart)
                    {
                        // Был найден.
                        bool isEmpty = true;
                        foreach (string[] k in _diopozoneAddresses)
                        {
                            if (k[0] == firstAddress && k[1] == lastAddress)
                            {
                                Logger.I.To(this, $"Искомый диопозон адрессов");

                                isEmpty = false;
                            }
                        }

                        if (isEmpty)
                        {
                            Logger.I.To(this, $"Попытка удалить несущесвующий диопазон адрессов " +
                                $"{firstAddress}-{lastAddress}");

                            return;
                        }

                        int countRemove = _diopozoneAddresses.RemoveAll((r) =>
                        {
                            return (r[0] == firstAddress && r[1] == lastAddress);
                        });

                        if (countRemove == 1)
                        {
                            Logger.S_I.To(this, $"Диопозон аддрессов {firstAddress}-{lastAddress}" +
                                $"был удален из коллекции _diopozoneAddresses.");
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Не удалось удалить диопозон аддресов из _diopozoneAddresses " +
                                $" {firstAddress}-{lastAddress}. Такой диопозон должен быть лишь один, но их {countRemove}.");

                            destroy();

                            return;
                        }


                        if (_diopozoneAddressesList.Remove($"{firstAddress}-{lastAddress}"))
                        {
                            Logger.S_I.To(this, $"Диопазон аддрессов {firstAddress}-{lastAddress} был удален из словаря" +
                                $" _diopozoneAddressesList.");
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Диопазон аддрессов {firstAddress}-{lastAddress} не удалось удалить из словаря" +
                                $" _diopozoneAddressesList, так как его там нету.");

                            destroy();

                            return;
                        }


                        List<string> buffer = new List<string>();
                        // Берем все адресса из данного диопозона.
                        foreach (string s in m)
                        {
                            Logger.S_I.To(this, $"Решаем нужно ли удалить адрес {s} удаляемого диопазона " +
                                $"адрессов {firstAddress}-{lastAddress} из коллекции _scanAddresses");

                            // Сравниваем с адрессами сканирования
                            // И если есть совподения, то записываем текущий адресс
                            // на удаление, за исключением адрессов котороые
                            foreach (string t in _scanAddresses)
                            {
                                // Адресс найден.
                                if (s == t)
                                {
                                    // Теперь проверим если ли данный адресс в 
                                    // коллескии _addresses
                                    bool g = true;
                                    foreach (string b in _addresses)
                                    {
                                        // Данный аддресс остается в коллекции _scanAddresses
                                        if (s == b)
                                        {
                                            g = false;

                                            break;
                                        }
                                    }

                                    // Если true значить данный адресс не был найден и его 
                                    // нужно удалить.
                                    if (g)
                                    {
                                        Logger.S_I.To(this, $"Адресс {s} из диопозона [{firstAddress}-{lastAddress}]" +
                                            $"записан в буффер для дальнейшего удаления.");

                                        buffer.Add(s);
                                    }
                                    else
                                    {
                                        Logger.S_I.To(this, $"Адресс {s} остается в коллекции, " +
                                            $"так как этот адрес так же находится в коллекции _addresses.");
                                    }

                                    break;
                                }
                            }
                        }

                        foreach (string u in buffer)
                        {
                            Logger.S_I.To(this, $"Удаляем из коллекции _scanAddresses адрес [{u}]");

                            _scanAddresses.Remove(u);
                        }

                        if (MongoDB.TryDeleteOne(DB.NAME, DB.AddressCollection.NAME,
                            BsonDocumentType.DIOPOZONE_ADDRESSES, $"{firstAddress}-{lastAddress}", out string info))
                        {
                            Logger.I.To(this, info);
                        }
                        else
                        {
                            Logger.S_E.To(this, info);

                            destroy();

                            return;
                        }
                    }
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

                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsStart)
                    {
                        _diopozoneAddresses.Add(new string[] { firstAddress, lastAddress });
                        _diopozoneAddressesList.Add(
                                $"{firstAddress}-{lastAddress}", m.ToArray());

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
                }
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
                            else if (nextState == State.ADD_ADDRESSES || nextState == State.DELETE_ADDRESSES)
                                ReadLine.Input(NAME, "Введите начало диапозона адресов:");
                            else
                                ReadLine.Input();
                        }
                        else
                        {
                            Logger.S_E.To(this, $"Сменить состояние выполнения команды c " +
                                $"{State.WAIT_COMMAND} можно только на:" +
                                $"\n1){State.ADD_ADDRESS}" +
                                $"\n2){State.ADD_ADDRESSES}" +
                                $"\n3){State.DELETE_ADDRESS}" +
                                $"\n4){State.DELETE_ADDRESSES}" +
                                $",но вы попытались сменить на {nextState}.");

                            ReadLine.Input();
                        }
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
                    else if (_currentState == State.INPUT_END_DELETE_ADDRESSES)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.WAIT_COMMAND;

                            ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"С [{State.INPUT_END_DELETE_ADDRESSES}] можно " +
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

                            ReadLine.Input(NAME, "Введите конец диопозона:");
                        }
                        else Logger.S_E.To(this, $"Вы пытаетесь сменить состояние c [{State.ADD_ADDRESSES}] " +
                            $"на [{nextState}. Но ожидается что вы смените на [{State.INPUT_END_ADD_ADDRESSES}].");
                    }
                    else if (_currentState == State.DELETE_ADDRESS)
                    {
                        if (nextState == State.WAIT_COMMAND)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = State.WAIT_COMMAND;

                            ReadLine.Input();
                        }
                        else Logger.S_E.To(this, $"Произвести смену состояния ввода на {State.DELETE_ADDRESS}" +
                            $"можно только из состояния {State.WAIT_COMMAND}. Была произведена попытка смены " +
                            $" на {nextState}.");
                    }
                    else if (_currentState == State.DELETE_ADDRESSES)
                    {
                        if (nextState == State.INPUT_END_DELETE_ADDRESSES)
                        {
                            Logger.I.To(this, $"State:{_currentState}->{nextState}");

                            _currentState = nextState;

                            ReadLine.Input(NAME, "Введите конец диопозона:");
                        }
                        else Logger.S_E.To(this, $"Произвести смену состояния ввода на {State.DELETE_ADDRESSES}" +
                            $"можно только из состояния {State.INPUT_END_DELETE_ADDRESSES}. Была произведена попытка смены " +
                            $" на {nextState}.");
                    }
                    else if (nextState == State.WAIT_COMMAND)
                    {
                        Logger.I.To(this, $"Вы передали в nextState [{State.WAIT_COMMAND}], значит что то пошло нетак, " +
                            $"с точки зрения логически пользовательской последовательности. Текущее состояние {_currentState}.");
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
                                            bool f = true;
                                            foreach (string r in _scanAddresses)
                                            {
                                                if (r == t)
                                                {
                                                    f = false;
                                                    break;
                                                }
                                            }

                                            if (f) _scanAddresses.Add(t);
                                        }

                                        _diopozoneAddressesList.Add(a, resultConvert);
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
            public const string INPUT_END_DELETE_ADDRESSES = "Ввод удоляемых адрессов, конец диапозона включительно";
            public const string DELETE_ADDRESSES = "Удалить адресса";

            public const string SHOW_ADDRESSES = "Проказать адресса для скана";
        }
    }
}