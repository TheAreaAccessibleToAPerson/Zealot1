using Butterfly;
using Zealot.manager;

namespace Zealot
{
    public sealed class Program : Controller, ReadLine.IInformation
    {
        ScanerDevices _scanerDevices;

        void Construction()
        {
            _scanerDevices = obj<ScanerDevices>(ScanerDevices.NAME);
        }

        void Start()
        {
            Logger.S_I.To(this, "starting ...");
            {
                ReadLine.Start(this);
            }
            Logger.S_I.To(this, "start");
        }

        void Destruction()
        {
            Logger.S_I.To(this, "destruction ...");
            {
                //...
            }
        }

        void Destroyed()
        {
            {
                //...
            }
            Logger.S_I.To(this, "destroyed");
        }

        void Stop()
        {
            Logger.S_I.To(this, "stopping ...");
            {
                ReadLine.Stop(this);
            }
            Logger.S_I.To(this, "stop");
        }

        void Configurate()
        {
            Logger.S_I.To(this, "start configurate ...");
            {
                if (MongoDB.DefineConnection("mongodb://localhost:27017", out string defineInfo))
                {
                    Logger.S_I.To(this, defineInfo);

                    if (MongoDB.StartConnection(out string startConnectionInfo))
                    {
                        Logger.S_I.To(this, startConnectionInfo);
                    }
                    else 
                    {
                        Logger.S_E.To(this, startConnectionInfo);

                        destroy();
                    }
                }
                else 
                {
                    Logger.S_E.To(this, defineInfo);

                    destroy();
                }
            }
            Logger.S_I.To(this, "end configurate");
        }

        public void Command(string command)
        {
            if (command == "exit")
            {
                destroy();
            }
        }
    }
}