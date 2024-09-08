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

            //AddAsic.StartScript2();
            //AddAsic.StartScript3();
            //AddAsic.StartScript4();
            //AddAsic.StartScript5();
            //AddAsic.StartScript6();
            //AddAsic.StartScript7();
            //AddAsic.StartScript8();
            //AddAsic.StartScript9();
            //AddAsic.StartScript10();
            //AddAsic.StartScript11();
            //AddAsic.StartScript12();
            //AddAsic.StartScript13();
            //AddAsic.StartScript14();
            //AddAsic.StartScript15();
            //AddAsic.StartScript16();
            //AddAsic.StartScript17();
            //AddAsic.StartScript18();
            //AddAsic.StartScript19();
            //AddAsic.StartScript20();
            //AddAsic.RemoveDataBase();
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
            SystemInformation("destruction...");
            {
                //...
            }
        }

        void Destroyed()
        {
            {
                //...
            }
            SystemInformation("destroyed");
        }

        void Stop()
        {
            SystemInformation("stopping ...");
            {
                if (StateInformation.IsCallStart)
                {
                    ReadLine.Stop(this);
                }
            }
            SystemInformation("stop");
        }

        void StartConfigurate()
        {
            SystemInformation("start configurate ...");
            {
                if (MongoDB.DefineConnection("mongodb://localhost:27017", out string defineInfo))
                {
                    Logger.S_I.To(this, defineInfo);

                    if (MongoDB.StartConnection(out string startConnectionInfo))
                    {
                        SystemInformation(startConnectionInfo);
                    }
                    else
                    {
                        SystemInformation(startConnectionInfo);

                        destroy();

                        return;
                    }
                }
                else
                {
                    SystemInformation(defineInfo);

                    destroy();

                    return;
                }
            }
            SystemInformation("end configurate");
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