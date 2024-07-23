using System.Net;
using System.Text;
using System.Text.Json;
using Butterfly;

namespace Zealot.device
{
    /// <summary>
    /// Стандартная прошика. 
    /// </summary>
    public sealed class AntminerDefault : Controller.Board.LocalField<Setting>, IDevice
    {
        public struct State 
        {
            public const string NONE = "None";
            public const string GET_SYSTEM_INFO = "GetSystemInfo";
        }

        CookieContainer cookeis = new CookieContainer();

        protected string CurrentState = State.NONE;

        protected bool IsRunning = true;

        private IInput<string> I_requestInformation;
        private IInput<string> i_setState;

        void Construction()
        {
            input_to(ref i_setState, Header.Events.SCAN_DEVICES, ISetState);
            input_to(ref I_requestInformation, Header.Events.SCAN_DEVICES, IRequestInformation);
        }

        void Start()
        {
            Logger.I.To(this, $"run starting ...");
            {
                i_setState.To(State.GET_SYSTEM_INFO);
            }
            Logger.I.To(this, $"end starting.");
        }

        protected void ISetState(string nextState)
        {
            if (IsRunning == false) return; 

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                if (nextState == State.GET_SYSTEM_INFO)
                {
                    if (CurrentState == State.NONE)
                    {
                        Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.GET_SYSTEM_INFO;

                        I_requestInformation.To(URL.GET_SYSTEM_INFO);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE}]");
                }
            }

        }

        protected void IRequestInformation(string url)
        {
            if (IsRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                try
                {
                    string urll = $"http://{Field.IPAddress}/{url}";

                    HttpWebRequest request = (HttpWebRequest)
                        WebRequest.Create(urll);

                    request.ServerCertificateValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => { return true; };

                    request.Credentials = new NetworkCredential("root", "root");

                    request.CookieContainer = cookeis;

                    request.BeginGetResponse(new AsyncCallback((result) =>
                    {
                        lock (StateInformation.Locker)
                        {
                            if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                            try
                            {
                                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                {
                                    string str = stream.ReadToEnd();

                                    Result(str);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.W.To(this, $"Неудалось получить данные у ANMAINER.[{CurrentState}] {ex}");

                                destroy();
                            }
                        }

                    }), request);
                }
                catch (Exception ex)
                {
                    Logger.W.To(this, $"Неудалось запросить станицу *******. {ex}");

                    destroy();
                }
            }
        }

        void Result(string str)
        {
            AntminerSystemInfoType1 f = JsonSerializer.Deserialize<AntminerSystemInfoType1>(str);
            Console(f.macaddr);
        }


        void Configurate()
        {
        }

        void Destroyed()
        {
        }

        void Stop()
        {
        }

        public bool IsOnline()
        {
            throw new NotImplementedException();
        }

        public string GetNormalHashrate()
        {
            throw new NotImplementedException();
        }

        public float GetHashrate()
        {
            throw new NotImplementedException();
        }

        public bool TryRestart(out string info)
        {
            throw new NotImplementedException();
        }

        public string GetWorkMode()
        {
            throw new NotImplementedException();
        }

        public string SetWorkMode()
        {
            throw new NotImplementedException();
        }

        public string GetMAC()
        {
            throw new NotImplementedException();
        }

        public void SetMAC()
        {
            throw new NotImplementedException();
        }

        public string GetAddress()
        {
            throw new NotImplementedException();
        }

        public string GetModel()
        {
            throw new NotImplementedException();
        }

        public void SetModel()
        {
            throw new NotImplementedException();
        }

        public string GetSN()
        {
            throw new NotImplementedException();
        }

        public void SetSN()
        {
            throw new NotImplementedException();
        }

        public string GetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public void SetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public string GetStandNumber()
        {
            throw new NotImplementedException();
        }

        public void SetStandNumber()
        {
            throw new NotImplementedException();
        }

        public string GetStandPosition()
        {
            throw new NotImplementedException();
        }

        public void SetStandPosition()
        {
            throw new NotImplementedException();
        }

        public string GetPool1()
        {
            throw new NotImplementedException();
        }

        public void SetPool1()
        {
            throw new NotImplementedException();
        }

        public string GetWorker1()
        {
            throw new NotImplementedException();
        }

        public void SetWorker1()
        {
            throw new NotImplementedException();
        }

        public string GetPassword1()
        {
            throw new NotImplementedException();
        }

        public void SetPassword1()
        {
            throw new NotImplementedException();
        }

        public string GetPool2()
        {
            throw new NotImplementedException();
        }

        public void SetPool2()
        {
            throw new NotImplementedException();
        }

        public string GetWorker2()
        {
            throw new NotImplementedException();
        }

        public void SetWorker2()
        {
            throw new NotImplementedException();
        }

        public string GetPassword2()
        {
            throw new NotImplementedException();
        }

        public void SetPassword2()
        {
            throw new NotImplementedException();
        }

        public string GetPool3()
        {
            throw new NotImplementedException();
        }

        public void SetPool3()
        {
            throw new NotImplementedException();
        }

        public string GetWorker3()
        {
            throw new NotImplementedException();
        }

        public void SetWorker3()
        {
            throw new NotImplementedException();
        }

        public string GetPassword3()
        {
            throw new NotImplementedException();
        }

        public void SetPassword3()
        {
            throw new NotImplementedException();
        }

        public string GetJsonString()
        {
            throw new NotImplementedException();
        }

        public byte[] GetJsonBytes()
        {
            throw new NotImplementedException();
        }

        public AsicStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public void Destroy(string destroyInfo)
        {
            throw new NotImplementedException();
        }

        public struct URL 
        {
            public const string GET_SYSTEM_INFO = "/cgi-bin/get_system_info.cgi";
        }
    }


    //S19, 
    public class AntminerSystemInfoType1
    {
        public string minertype { get; set; } = "";
        public string nettype { get; set; } = "";
        public string netdevice { get; set; } = "";
        public string macaddr { get; set; } = "";
        public string hostname { get; set; } = "";
        public string ipaddress { get; set; } = "";
        public string netmask { get; set; } = "";
        public string gateway { get; set; } = "";
        public string dnsservers { get; set; } = "";
        public string system_mode { get; set; } = "";
        public string system_kernel_version { get; set; } = "";
        public string system_filesystem_version { get; set; } = "";
        public string firmware_type { get; set; } = "";
        public string serinum { get; set; } = "";
    }
}