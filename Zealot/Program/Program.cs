using Butterfly;
using MongoDB.Bson;
using Zealot.script;

namespace Zealot
{
    public sealed class Program : Controller, ReadLine.IInformation
    {
        manager.ScanerDevices _scanerDevices;

        void Construction()
        {
            StartConfigurate();

            //AddClient.StartScript();

            obj<manager.Clients>(manager.Clients.NAME);
        }

        void Start()
        {
            Logger.S_I.To(this, "starting ...");
            {
                _scanerDevices = obj<manager.ScanerDevices>(manager.ScanerDevices.NAME);

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
                if (StateInformation.IsStart)
                {
                    ReadLine.Stop(this);
                }
            }
            Logger.S_I.To(this, "stop");
        }

        void StartConfigurate()
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

                        return;
                    }
                }
                else
                {
                    Logger.S_E.To(this, defineInfo);

                    destroy();

                    return;
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
            else
            {
                ReadLine.Input();
            }
        }
    }
}