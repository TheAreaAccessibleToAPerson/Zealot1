using System.Net;
using System.Text;
using System.Text.Json;
using Butterfly;

namespace Zealot.device
{
    public class JavascriptCallback
    {
        // событие, которое срабатывает, когда мы получаем результат из Ajax-вызова
        public event Action OnResult;

        // возвращает объект результата вызова Ajax
        public object Result { get; private set; }

        // метод, который будет вызываться из JavaScript
        public void SetResult(object result)
        {
            Result = result;

            OnResult?.Invoke();
        }
    }

    public abstract class IceRiverController : Controller.Board.LocalField<Setting>, IDevice
    {
        public struct State
        {
            public const string NONE = "None";
            public const string GET_INFO = "GetInformation";
        }

        private readonly JavascriptCallback _javascriptCallback = new JavascriptCallback();


        protected string CurrentState { set; get; } = State.NONE;

        protected IceRiverStatus Status = new();

        private CookieContainer _cookeis = new();

        protected bool IsRun { set; get; } = true;

        protected IInput<string, string> I_requestInformation;
        protected IInput<string> I_setState;
        protected IInput<string> I_setState3sDelay;

        //************ASIC INIT****************
        // Зарпашивает данные об асике.
        protected AsicInit AsicInit;
        protected IInput<string> I_asicInit;
        protected IInput<byte[]> I_sendBytesMessageToClients;
        protected IInput<string> I_sendStringMessageToClients;

        //**********ADD NEW ASIC TO DEVICES MANAGER***********
        protected IInput I_addAsicToDictionary;
        private bool isAddAsicToDictionary = false;

        protected string _MAC = "";

        protected void ISetState(string nextState)
        {
            if (IsRun == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy) return;

                if (nextState == State.GET_INFO)
                {
                    if (CurrentState == State.NONE)
                    {
                        CurrentState = nextState;

                        I_requestInformation.To(URL.GET_INFO, JSON.GET_INFO);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE}]");
                }
            }
        }

        protected void IRequestInformation(string url, string json)
        {
            if (IsRun == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy) return;

                try
                {
                    string urll = $"http://{Field.IPAddress}/{url}";

                    HttpWebRequest request = (HttpWebRequest)
                        WebRequest.Create(urll);

                    request.ContentType = "application/json";
                    request.Method = "POST";

                    request.ServerCertificateValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => { return true; };

                    request.Credentials = new NetworkCredential("admin", "admin");

                    request.CookieContainer = _cookeis;

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }

                    request.BeginGetResponse(new AsyncCallback((result) =>
                    {
                        lock (StateInformation.Locker)
                        {
                            if (StateInformation.IsDestroy) return;

                            try
                            {
                                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                                using (var streamReader = new StreamReader(response.GetResponseStream()))
                                {
                                    Result(streamReader.ReadToEnd());
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.W.To(this, $"Неудалось получить данные у IceRiver.[{CurrentState}] {ex}");

                                destroy();

                                return;
                            }
                        }

                    }), request);
                }
                catch (Exception ex)
                {
                    Logger.W.To(this, $"Неудалось запросить станицу *******. {ex}");

                    destroy();

                    return;
                }
            }
        }

        void Result(string str)
        {
            if (IsRun == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy) return;

                try
                {
                    if (CurrentState == State.GET_INFO)
                    {
                        IceRiverJson systemInfo = JsonSerializer.Deserialize<IceRiverJson>(str);
                        if (systemInfo != null)
                        {
                            
                        }
                        else
                        {
                            Logger.W.To(this, $"Stats information is null");

                            destroy();

                            return;
                        }
                    }
                    else
                    {
                        Logger.S_E.To(this, $"Неизвестный CurrentState:[{CurrentState}].");

                        destroy();

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.W.To(this, $"{ex}");

                    destroy();

                    return;
                }
            }
        }

        public void Destroy(string destroyInfo)
        {
            throw new NotImplementedException();
        }

        public string GetAddress() => Field.IPAddress;

        public float GetHashrate()
        {
            throw new NotImplementedException();
        }

        public byte[] GetJsonBytes()
        {
            throw new NotImplementedException();
        }

        public string GetJsonString()
        {
            throw new NotImplementedException();
        }

        public string GetMAC()
        {
            return "";
        }

        public string GetModel()
        {
            throw new NotImplementedException();
        }

        public string GetNormalHashrate()
        {
            throw new NotImplementedException();
        }

        public string GetPassword1()
        {
            throw new NotImplementedException();
        }

        public string GetPassword2()
        {
            throw new NotImplementedException();
        }

        public string GetPassword3()
        {
            throw new NotImplementedException();
        }

        public string GetPool1()
        {
            throw new NotImplementedException();
        }

        public string GetPool2()
        {
            throw new NotImplementedException();
        }

        public string GetPool3()
        {
            throw new NotImplementedException();
        }

        public string GetSN()
        {
            throw new NotImplementedException();
        }

        public string GetStandNumber()
        {
            throw new NotImplementedException();
        }

        public string GetStandPosition()
        {
            throw new NotImplementedException();
        }

        public WhatsMinerStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public string GetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public string GetWorker1()
        {
            throw new NotImplementedException();
        }

        public string GetWorker2()
        {
            throw new NotImplementedException();
        }

        public string GetWorker3()
        {
            throw new NotImplementedException();
        }

        public string GetWorkMode()
        {
            throw new NotImplementedException();
        }

        public bool IsOnline()
        {
            throw new NotImplementedException();
        }

        public void SetMAC()
        {
            throw new NotImplementedException();
        }

        public void SetModel()
        {
            throw new NotImplementedException();
        }

        public void SetPassword1()
        {
            throw new NotImplementedException();
        }

        public void SetPassword2()
        {
            throw new NotImplementedException();
        }

        public void SetPassword3()
        {
            throw new NotImplementedException();
        }

        public void SetPool1()
        {
            throw new NotImplementedException();
        }

        public void SetPool2()
        {
            throw new NotImplementedException();
        }

        public void SetPool3()
        {
            throw new NotImplementedException();
        }

        public void SetSN()
        {
            throw new NotImplementedException();
        }

        public void SetStandNumber()
        {
            throw new NotImplementedException();
        }

        public void SetStandPosition()
        {
            throw new NotImplementedException();
        }

        public void SetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public void SetWorker1()
        {
            throw new NotImplementedException();
        }

        public void SetWorker2()
        {
            throw new NotImplementedException();
        }

        public void SetWorker3()
        {
            throw new NotImplementedException();
        }

        public string SetWorkMode()
        {
            throw new NotImplementedException();
        }

        public bool TryRestart(out string info)
        {
            throw new NotImplementedException();
        }

        public struct URL
        {
            public const string GET_INFO = "user/userpanel";
        }

        public struct JSON
        {
            public const string GET_INFO = "{\"post\":4}";
        }
    }

    public class Board
    {
        public double no { get; set; }
        public double chipnum { get; set; }
        public double chipsuc { get; set; }
        public double error { get; set; }
        public double freq { get; set; }
        public string rtpow { get; set; }
        public string avgpow { get; set; }
        public string idealpow { get; set; }
        public string tempnum { get; set; }
        public string pcbtemp { get; set; }
        public double intmp { get; set; }
        public double outtmp { get; set; }
        public bool state { get; set; }
        public List<int> @false { get; set; }
    }

    public class Data
    {
        public string model { get; set; }
        public string algo { get; set; }
        public bool online { get; set; }
        public string firmver1 { get; set; }
        public string firmver2 { get; set; }
        public string softver1 { get; set; }
        public string softver2 { get; set; }
        public string firmtype { get; set; }
        public string nic { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
        public string netmask { get; set; }
        public string host { get; set; }
        public bool dhcp { get; set; }
        public string gateway { get; set; }
        public string dns { get; set; }
        public bool locate { get; set; }
        public string rtpow { get; set; }
        public string avgpow { get; set; }
        public double reject { get; set; }
        public string runtime { get; set; }
        public string unit { get; set; }
        public Pows pows { get; set; }
        public List<string> pows_x { get; set; }
        public bool powstate { get; set; }
        public bool netstate { get; set; }
        public bool fanstate { get; set; }
        public bool tempstate { get; set; }
        public List<int> fans { get; set; }
        public List<Pool> pools { get; set; }
        public List<Board> boards { get; set; }
        public string refTime { get; set; }
    }

    public class Pool
    {
        public double no { get; set; }
        public string addr { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public bool connect { get; set; }
        public string diff { get; set; }
        public double priority { get; set; }
        public double accepted { get; set; }
        public double rejected { get; set; }
        public double diffa { get; set; }
        public double diffr { get; set; }
        public double state { get; set; }
        public double lsdiff { get; set; }
        public string lstime { get; set; }
    }

    public class Pows
    {
        public List<int> board1 { get; set; }
    }

    public class IceRiverJson
    {
        public int error { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }

    public class IceRiverStatus
    {
        public string Address { set; get; }
    }
}