using Butterfly;

namespace Zealot 
{
    public class Header : Controller 
    {
        public struct Events 
        {
            public const string SYSTEM = "System";
            public const int SYSTEM_TIME_DELAY = 5;

            public const string LOGGER = "Logger";
            public const int LOGGER_TIME_DELAY = 3;

            public const string MONGO_DB = "MongoDB";
            public const int MONGO_DB_TIME_DELAY = 5;

            /// <summary>
            /// Взаимодейсвие, работа с девайсами. 
            /// Запись, удаление, получение.
            /// </summary>
            public const string WORK_DEVICE = "WorkDevice";
            public const int WORK_DEVICE_TIME_DELAY = 5;

            /// <summary>
            /// Сканирование устройсв из сети.
            /// Работа с диопазоном сети.
            /// </summary> <summary>
            public const string SCAN_DEVICES = "ScanDevices";
            public const int SCAN_DEVICES_TIME_DELAY = 5;

            /// <summary>
            /// Запросы устройсв для полуения информации (MAC, Uptime, pool )
            /// </summary>
            public const string REQUEST_DEVICES_INFORMATION = "RequestInformation";
            public const int REQUEST_DEVICES_INFORMATION_TIME_DELAY = 5;

            /// <summary>
            /// Извлечение данных из полученых ответов(html, json ...)/
            /// </summary>
            public const string EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION = "ExtractFromResultRequestDevicesInformation";
            public const int EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION_TIME_DELAY = 5;

            /// <summary>
            /// Прослушивает новых клинтов, переподключает их, прослушивает сообщения от них.
            /// Работает с коллекциями клинтов.
            /// </summary> <summary>
            public const string LISTEN_CLIENT = "ListenClient";
            public const int LISTEN_CLIENT_TIME_DELAY = 5;

            /// <summary>
            /// Отправляет сообщение клиeнтам.
            /// </summary> 
            public const string SEND_MESSAGE_TO_CLIENT = "SendMessageToClient";
            public const int SEND_MESSAGE_TO_CLIENT_TIME_DELAY = 5;

            /// <summary>
            /// Прослушивает сообщения от клинтов.
            /// </summary> 
            public const string RECEIVE_MESSAGE_FROM_CLIENT = "ReceiveMessageFromClient";
            public const int RECEIVE_MESSAGE_FROM_CLIENT_TIME_DELAY = 5;
        }

        private readonly WritingText _writingText = new WritingText();

        void Construction()
        {
            listen_events(Events.SYSTEM, Events.SYSTEM);
            listen_events(Events.LOGGER, Events.LOGGER);
            listen_events(Events.MONGO_DB, Events.MONGO_DB);
            listen_events(Events.LISTEN_CLIENT, Events.LISTEN_CLIENT);
            listen_events(Events.SEND_MESSAGE_TO_CLIENT, Events.SEND_MESSAGE_TO_CLIENT);
            listen_events(Events.RECEIVE_MESSAGE_FROM_CLIENT, Events.RECEIVE_MESSAGE_FROM_CLIENT);
            listen_events(Events.SCAN_DEVICES, Events.SCAN_DEVICES);
            listen_events(Events.WORK_DEVICE, Events.WORK_DEVICE);
            listen_events(Events.REQUEST_DEVICES_INFORMATION, Events.REQUEST_DEVICES_INFORMATION);
            listen_events(Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION, Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION);
            
            input_to(ref Logger.S_I, Events.LOGGER, _writingText.SystemInformation);
            input_to(ref Logger.S_Is, Events.LOGGER, _writingText.SystemInformations);
            input_to(ref Logger.S_W, Events.LOGGER, _writingText.SystemWarning);
            input_to(ref Logger.S_Ws, Events.LOGGER, _writingText.SystemWarnings);
            input_to(ref Logger.S_E, Events.LOGGER, _writingText.SystemError);
            input_to(ref Logger.S_Es, Events.LOGGER, _writingText.SystemErrors);

            input_to(ref Logger.I, Events.LOGGER, _writingText.Information);
            input_to(ref Logger.Is, Events.LOGGER, _writingText.Informations);
            input_to(ref Logger.W, Events.LOGGER, _writingText.Warning);
            input_to(ref Logger.Ws, Events.LOGGER, _writingText.Warnings);
            input_to(ref Logger.E, Events.LOGGER, _writingText.Error);
            input_to(ref Logger.Es, Events.LOGGER, _writingText.Errors);

            input_to(ref Logger.CommandStateException, Events.LOGGER, _writingText.StateException);
        }

        void Start()
        {
            Logger.S_I.To(this, "starting...");
            {
                obj<Program>("program");
            }
            Logger.S_I.To(this, "start...");
        }
    }
}