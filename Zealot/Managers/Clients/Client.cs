using System.Net;

namespace Zealot.manager
{
    public sealed class Client : ClientController
    {
        void Start()
        {
            // Приходит подключение.
            // Создается SSL клиент.
            // Принимаем данные логин, пароль.
            // Если верно, то
            // Проверяем не авторизованы ли мы, если да то завершаем прошлую сессию.
            // Если нет, то авторизуемся.
            // Далее создаем новую TCP сессию, отправляем уникальный id и порт клиенту по SSL, по которому нужно подключиться.
            // После клиент устанавливает TCP соединение.
            // После того как соединение установлено, сервер сообщает клинту о начале работы.
            // И высылает ему необходимые данные.
        }

        void Construction()
        {
            RemoteAddress = ((IPEndPoint)Field.Client.RemoteEndPoint).Address.ToString();
            RemotePort = ((IPEndPoint)Field.Client.RemoteEndPoint).Port;

            input_to(ref i_setState, Header.Events.SYSTEM, ISetState);
            input_to(ref I_sendSSLStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendSSLStringMessage);
            input_to(ref I_sendSSLBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendSSLByteMessage);
            input_to(ref I_sendTCPStringMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendTCPStringMessage);
            input_to(ref I_sendTCPBytesMessage, Header.Events.SEND_MESSAGE_TO_CLIENT, ISendTCPByteMessage);

            add_event(Header.Events.RECEIVE_MESSAGE_FROM_CLIENT, ReceiveMessageFromClient);
        }
    }
}