namespace Zealot
{

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