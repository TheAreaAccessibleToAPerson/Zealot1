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
                        Zealot.Header.Events.LOGGER_TIME_DELAY)
                }
            });

            /*
            if (Zealot.MongoDB.DefineConnection("mongodb://localhost:27017", out string defineInfo))
            {
                Console.WriteLine(defineInfo);

                if (Zealot.MongoDB.StartConnection(out string startConnectionInfo))
                {
                    Console.WriteLine(startConnectionInfo);

                    if (Zealot.MongoDB.TryCreatingDatabase("1", out string info1))
                    {
                        Console.WriteLine(info1);
                    }
                    else Console.WriteLine(info1);

                    if (Zealot.MongoDB.TryGetDatabase("1", out IMongoDatabase db, out string info))
                    {
                        Console.WriteLine(info);
                        Person p = new Person();
                    }
                    else Console.WriteLine(info);
                }
                else Console.WriteLine(startConnectionInfo);
            }
            else Console.WriteLine(defineInfo);
            */
        }
    }
}