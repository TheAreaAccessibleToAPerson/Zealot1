using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager
{
    public sealed class Clients : Controller
    {
        public const string NAME = "ClientsManager";

        private Devices _devicesManager;

        public readonly Dictionary<string, MainClient> _admins = new();
        public readonly Dictionary<string, MainClient> _clients = new();

        void Construction()
        {
            _devicesManager = obj<Devices>(Devices.NAME);

            // Сюда приходит сообщение содеждащее новое SSL подключение,
            // обрабатываемое событием Work Client
            listen_message<TcpClient>(BUS.ADD_CLIENT)
                .output_to((client) =>
                {
                    string key = $"{((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()}:" +
                        $"{((IPEndPoint)client.Client.RemoteEndPoint).Port}";

                    if (StateInformation.IsStart && !StateInformation.IsDestroy)
                    {
                        if (try_obj(key, out Client obj))
                        {
                            Logger.I.To(this, $"Клиент с ключом {key} уже подключон.");
                        }
                        else
                        {
                            Logger.I.To(this, $"Connection new client:{key}");

                            Client newClient = obj<Client>(key, client);

                            _clients.Add(key, newClient);
                        }

                    }
                    else
                    {
                        Logger.W.To(this, $"Неудалось поключить нового клинта {key}, " +
                            " так как ClientsManager завершает свою работу.");
                    }
                });

            listen_message<Client>(BUS.DELETE_CLIENT)
                .output_to((client) =>
                {
                    if (_clients.Remove(client.GetKey()))
                    {
                        Logger.I.To(this, $"Client remove from ClientCollection.");
                    }
                    else
                    {
                        Logger.S_E.To(this, $"Client not remove from ClientCollection.");

                        destroy();

                        return;
                    }
                });
        }

        void Start()
        {
            SystemInformation("starting ...");
            {
                obj<network.Server>(network.Server.NAME);
            }
        }

        void Configurate()
        {
        }

        public struct BUS
        {
            // Добовляет нового подключенного клинта.
            public const string DELETE_CLIENT = NAME + ":Delete client";
            public const string ADD_CLIENT = NAME + ":Add client";
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

        public struct TCPType
        {
            /// <summary>
            /// Поступили новые данные для асика.
            /// </summary> <summary>
            public const int ASIC_DATA = 0;
        }

        public struct SSLType
        {
            /// <summary>
            /// Авторизаци прошла усмешна. 
            /// </summary> <summary>
            public const int SUCCSESS_AUTHORIZATION = 0;

            /// <summary>
            /// Неверный логин или пароль. 
            /// </summary>
            public const int ERROR_AUTHORIZATION = 1;

            /// <summary>
            /// Данные клиeнта.
            /// </summary> 
            public const int CLIENT_DATA = 2;
        }
    }
}