using MongoDB.Driver;

namespace Butterfly
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            Butterfly.fly<Zealot.Header>(new Butterfly.Settings()
            {
                Name = "Program",

                SystemEvent = new EventSetting(Zealot.Header.Events.SYSTEM, 
                    Zealot.Header.Events.SYSTEM_TIME_DELAY),

                EventsSetting = new EventSetting[] 
                {
                    new EventSetting(Zealot.Header.Events.LOGGER, 
                        Zealot.Header.Events.LOGGER_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.MONGO_DB, 
                        Zealot.Header.Events.MONGO_DB_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.SCAN_DEVICES, 
                        Zealot.Header.Events.SCAN_DEVICES_TIME_DELAY)
                }
            });
        }
    }
}