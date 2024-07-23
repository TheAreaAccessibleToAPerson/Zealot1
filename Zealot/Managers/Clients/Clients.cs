using System.Net;
using System.Net.Sockets;
using System.Text;
using Butterfly;

namespace Zealot.manager
{
    public sealed class Clients : Controller
    {
        public const string NAME = "ClientsManager";

        private Devices _devicesManager;

        public readonly Dictionary<string, Client> _clients = new();
        public readonly Dictionary<string, IDevice> _asics = new();

        IInput i_getDevicesInformation;

        /// <summary>
        /// Отпавляет админу информацию обо всех машинах. 
        /// </summary>
        IInput i_sendAllAsicsInformationToAdmin;

        void Construction()
        {
            _devicesManager = obj<Devices>(Devices.NAME);

            {
                input_to_0_1<byte[]>(ref i_sendAllAsicsInformationToAdmin, Header.Events.WORK_DEVICE, (@return) =>
                {
                    @return.To(_devicesManager.GetDevicesInforamtionMessage());

                }).output_to((value) =>
                {
                    var result = System.Text.Encoding.Default.GetString(value);

                    foreach (Client client in _clients.Values)
                        if (client.IsAdmin()) client.I_sendBytesMessage.To(value);
                },
                Header.Events.LISTEN_CLIENT);

                //add_event(Header.Events.LISTEN_CLIENT, 200000, i_sendAllAsicsInformationToAdmin.To);
            }

            // Оповещаем администратором о новых машинах
            listen_message<byte[]>(BUS.ADMIN_LISTEN_JSON_ASICS)
                .output_to((json) =>
                {
                    byte[] message = ServerMessage.GetMessageArray(ServerMessage.Type.ADD_NEW_SCAN_ASIC, json);

                    foreach (Client client in _clients.Values)
                    {
                        if (client.IsAdmin())
                        {
                            client.I_sendBytesMessage.To(message);
                        }
                    }
                },
                Header.Events.LISTEN_CLIENT);

            // Сюда приходит сообщение обрабатываемое событием ListenClients
            listen_message<TcpClient>(BUS.LISTEN_NEW_CLIENT)
                .output_to((client) =>
                {
                    IPEndPoint endPoint = client.Client.LocalEndPoint as IPEndPoint;
                    string addr = endPoint.Address.ToString();

                    if (_clients.TryGetValue(addr, out Client c))
                    {
                        c.destroy();
                    }

                    Logger.I.To(this, $"Connection new client:[Address-{addr}]");

                    Client newClient = obj<Client>(addr, client);
                    _clients.Add(addr, newClient);
                });
        }

        void Start()
        {
            obj<manager.network.Server>(manager.network.Server.NAME);
        }

        public struct BUS
        {
            public const string LISTEN_NEW_CLIENT = NAME + ":ListenNewClient";
            public const string ADMIN_LISTEN_JSON_ASICS = NAME + ":AdminListenJSONAsics";

            // Добовляем новый в коллекцию.
            public const string ADD_ASIC = NAME + ":AddAsic";
            public const string DELETE_ASIC = NAME + ":DeleteAsic";
        }
    }

    public class ServerMessage
    {
        public static byte[] GetMessageArray(int type, byte[] message)
        {
            int messageLength = message.Length;

            byte[] result = new byte[Header.LENGHT + messageLength];
            result[Header.LENGTH_BYTE_INDEX] = Header.LENGHT;
            result[Header.TYPE_1BYTE_INDEX] = (byte)(type >> 8);
            result[Header.TYPE_2BYTE_INDEX] = (byte)type;
            result[Header.MESSAGE_LENGTH_1BYTE_INDEX] = (byte)(messageLength >> 24);
            result[Header.MESSAGE_LENGTH_2BYTE_INDEX] = (byte)(messageLength >> 16);
            result[Header.MESSAGE_LENGTH_3BYTE_INDEX] = (byte)(messageLength >> 8);
            result[Header.MESSAGE_LENGTH_4BYTE_INDEX] = (byte)messageLength;

            //result.Concat(message);

            for (int i = 0; i < messageLength; i++)
            {
                result[Header.LENGHT + i] = message[i];
            }

            return result;
        }

        public struct Header
        {
            public const int LENGHT = 7;

            public const int LENGTH_BYTE_INDEX = 0;

            public const int TYPE_1BYTE_INDEX = 1;
            public const int TYPE_2BYTE_INDEX = 2;

            public const int MESSAGE_LENGTH_1BYTE_INDEX = 3;
            public const int MESSAGE_LENGTH_2BYTE_INDEX = 4;
            public const int MESSAGE_LENGTH_3BYTE_INDEX = 5;
            public const int MESSAGE_LENGTH_4BYTE_INDEX = 6;
        }

        public struct Type
        {
            /// <summary>
            /// Добавить новый отсканированый асик.
            /// </summary> <summary>
            public const int ADD_NEW_SCAN_ASIC = 0;

            /// <summary>
            /// Добавляет все отсканированые асик.
            /// </summary> <summary>
            public const int ADD_ALL_ASIC = 1;
        }
    }

    public sealed class Client : Controller.Board.LocalField<TcpClient>
    {
        private bool _isRunning = true;

        public IInput<string> I_sendStringMessage;
        public IInput<byte[]> I_sendBytesMessage;

        void Construction()
        {
            input_to(ref I_sendStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, (message) =>
            {
                if (message.Length == 0) return;

                if (_isRunning)
                {
                    try
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(message);

                        Field.Client.Send(bytes);
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"{ex}");

                        destroy();
                    }
                }
            });

            input_to(ref I_sendBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, (message) =>
            {
                if (message.Length == 0) return;

                if (_isRunning)
                {
                    try
                    {
                        Field.Client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"{ex}");

                        destroy();
                    }
                }
            });
        }

        public bool IsAdmin() => true;

        void Destroyed()
        {
            _isRunning = false;
        }

        public struct Status
        {
            public const string ADMIN = "Admin";
        }
    }
}