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

            public const string SCAN_DEVICES = "ScanDevices";
            public const int SCAN_DEVICES_TIME_DELAY = 5;
        }

        private readonly WritingText _writingText = new WritingText();

        void Construction()
        {
            listen_events(Events.SYSTEM, Events.SYSTEM);
            listen_events(Events.LOGGER, Events.LOGGER);
            listen_events(Events.MONGO_DB, Events.MONGO_DB);
            listen_events(Events.SCAN_DEVICES, Events.SCAN_DEVICES);
            
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