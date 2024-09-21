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
                    if (CurrentState == State.NONE || CurrentState == State.GET_INFO)
                    {
                        CurrentState = nextState;

                        I_requestInformation.To(URL.GET_INFO, JSON.GET_INFO);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE} или {State.GET_INFO}]");
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
                        Status = JsonSerializer.Deserialize<IceRiverStatus>(str);
                        if (Status != null)
                        {
                            if (_MAC == "") _MAC = Status.data.mac;

                            if (isAddAsicToDictionary == false)
                            {
                                isAddAsicToDictionary = true;

                                I_addAsicToDictionary.To();
                            }
                            else
                            {
                                // Проверим получили ли мы данные по этой машинки из быза данных.
                                if (AsicInit != null)
                                {
                                    int[] pows = Status.data.pows.board1.ToArray();
                                    int currentPow = 0;
                                    {
                                        if (pows != null && pows.Length > 0) currentPow = pows[0];
                                    }

                                    int[] fans = new int[4] { 0, 0, 0, 0 };
                                    {
                                        if (Status.data != null && Status.data.fans != null && fans.Length == 4)
                                            fans = Status.data.fans.ToArray();
                                    }

                                    int[] inTemp = new int[3] { 0, 0, 0 };
                                    int[] outTemp = new int[3] { 0, 0, 0 };
                                    string[] rtpows = new string[3] { "", "", "" };
                                    {
                                        Board board1 = null; Board board2 = null; Board board3 = null;
                                        if (Status.data.boards.Count == 3)
                                        {
                                            board1 = Status.data.boards[0];
                                            board2 = Status.data.boards[1];
                                            board3 = Status.data.boards[2];
                                        }
                                        else if (Status.data.boards.Count == 2)
                                        {
                                            board1 = Status.data.boards[0];
                                            board2 = Status.data.boards[1];
                                        }
                                        else if (Status.data.boards.Count == 1)
                                        {
                                            board1 = Status.data.boards[0];
                                        }

                                        if (board1 != null)
                                        {
                                            inTemp[0] = Convert.ToInt32(board1.intmp);
                                            outTemp[0] = Convert.ToInt32(board1.outtmp);

                                            string[] rtpow = board1.rtpow.Split(".");
                                            if (rtpow.Length == 2)
                                                rtpows[0] = rtpow[0];
                                        }

                                        if (board2 != null)
                                        {
                                            inTemp[1] = Convert.ToInt32(board2.intmp);
                                            outTemp[1] = Convert.ToInt32(board2.outtmp);

                                            string[] rtpow = board1.rtpow.Split(".");
                                            if (rtpow.Length == 2)
                                                rtpows[1] = rtpow[1];
                                        }

                                        if (board3 != null)
                                        {
                                            inTemp[2] = Convert.ToInt32(board3.intmp);
                                            outTemp[2] = Convert.ToInt32(board3.outtmp);

                                            string[] rtpow = board1.rtpow.Split(".");
                                            if (rtpow.Length == 2)
                                                rtpows[1] = rtpow[1];
                                        }
                                    }

                                    OutputDataJson data = new OutputDataJson()
                                    {
                                        UniqueNumber = AsicInit.UniqueNumber,

                                        Culler1_power = fans[0].ToString(),
                                        Culler2_power = fans[1].ToString(),
                                        Culler3_power = fans[2].ToString(),
                                        Culler4_power = fans[3].ToString(),

                                        //WorkTime = Status.Elapsed,
                                        //Mode = Status.PowerMode,

                                        IPAddress = Field.IPAddress,

                                        MiningPowerSize = currentPow.ToString(),
                                        MiningPower1Size = rtpows[0],
                                        MiningPower2Size = rtpows[1],
                                        MiningPower3Size = rtpows[2],
                                        MiningPowerName = "GH",

                                        InTemp1 = inTemp[0].ToString(),
                                        InTemp2 = inTemp[1].ToString(),
                                        InTemp3 = inTemp[2].ToString(),

                                        OutTemp1 = outTemp[0].ToString(),
                                        OutTemp2 = outTemp[1].ToString(),
                                        OutTemp3 = outTemp[2].ToString(),
                                    };

                                    AsicInit.SendDataMessage(JsonSerializer.SerializeToUtf8Bytes(data));

                                    I_setState3sDelay.To(State.GET_INFO);
                                }
                                else
                                {
                                    Logger.I.To(this, $"В базе данных нету информации о {_MAC}.");

                                    destroy();

                                    return;
                                }
                            }
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
            SystemInformation(destroyInfo, ConsoleColor.Red);
            Logger.E.To(this, destroyInfo);
            destroy();
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
            return _MAC;
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
        public double no { get; set; } = 0.0d;
        public double chipnum { get; set; } = 0.0d;
        public double chipsuc { get; set; } = 0.0d;
        public double error { get; set; } = 0.0d;
        public double freq { get; set; } = 0.0d;
        public string rtpow { get; set; } = "";
        public string avgpow { get; set; } = "";
        public string idealpow { get; set; } = "";
        public string tempnum { get; set; } = "";
        public string pcbtemp { get; set; } = "";
        public double intmp { get; set; } = 0.0d;
        public double outtmp { get; set; } = 0.0d;
        public bool state { get; set; } = false;
        public List<int> @false { get; set; } = new List<int>(0);
    }

    public class Data
    {
        public string model { get; set; } = "";
        public string algo { get; set; } = "";
        public bool online { get; set; } = false;
        public string firmver1 { get; set; } = "";
        public string firmver2 { get; set; } = "";
        public string softver1 { get; set; } = "";
        public string softver2 { get; set; } = "";
        public string firmtype { get; set; } = "";
        public string nic { get; set; } = "";
        public string mac { get; set; } = "";
        public string ip { get; set; } = "";
        public string netmask { get; set; } = "";
        public string host { get; set; } = "";
        public bool dhcp { get; set; } = false;
        public string gateway { get; set; } = "";
        public string dns { get; set; } = "";
        public bool locate { get; set; } = false;
        public string rtpow { get; set; } = "";
        public string avgpow { get; set; } = "";
        public double reject { get; set; } = 0.0d;
        public string runtime { get; set; } = "";
        public string unit { get; set; } = "";
        public Pows pows { get; set; } = new Pows();
        public List<string> pows_x { get; set; } = new List<string>(0);
        public bool powstate { get; set; } = false;
        public bool netstate { get; set; } = false;
        public bool fanstate { get; set; } = false;
        public bool tempstate { get; set; } = false;
        public List<int> fans { get; set; } = new List<int>(0);
        public List<Pool> pools { get; set; } = new List<Pool>(0);
        public List<Board> boards { get; set; } = new List<Board>(0);
        public string refTime { get; set; } = "";
    }

    public class Pool
    {
        public double no { get; set; } = 0.0d;
        public string addr { get; set; } = "";
        public string user { get; set; } = "";
        public string pass { get; set; } = "";
        public bool connect { get; set; } = false;
        public string diff { get; set; } = "";
        public double priority { get; set; } = 0.0d;
        public double accepted { get; set; } = 0.0d;
        public double rejected { get; set; } = 0.0d;
        public double diffa { get; set; } = 0.0d;
        public double diffr { get; set; } = 0.0d;
        public double state { get; set; } = 0.0d;
        public double lsdiff { get; set; } = 0.0d;
        public string lstime { get; set; } = "";
    }

    public class Pows
    {
        public List<int> board1 { get; set; } = new List<int>(0);
    }

    public class IceRiverStatus
    {
        public string Address { get; set; } = "";

        public int error { get; set; } = 1;
        public Data data { get; set; } = new Data();
        public string message { get; set; } = "";
    }
}