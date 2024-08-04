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
            public const string GET_POOL = "GetPool";
            public const string GET_STATS = "GetStats";
        }

        private readonly Status _status = new Status();

        private CookieContainer _cookeis = new CookieContainer();

        protected string CurrentState { set; get; } = State.NONE;

        protected bool IsRunning { set; get; } = true;

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
                else if (nextState == State.GET_POOL)
                {
                    if (CurrentState == State.GET_SYSTEM_INFO)
                    {
                        Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.GET_POOL;

                        I_requestInformation.To(URL.GET_POOL);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.GET_SYSTEM_INFO}]");
                }
                else if (nextState == State.GET_STATS)
                {
                    if (CurrentState == State.GET_POOL)
                    {
                        Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.GET_STATS;

                        I_requestInformation.To(URL.GET_STATS);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.GET_POOL}]");
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

                    request.CookieContainer = _cookeis;

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
            if (IsRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                if (CurrentState == State.GET_SYSTEM_INFO)
                {
                    AntminerSystemInfo systemInfo = JsonSerializer.Deserialize<AntminerSystemInfo>(str);

                    if (systemInfo != null)
                    {
                        _status.model = systemInfo.minertype;
                        _status.mac = systemInfo.macaddr;

                        if (systemInfo.serinum != "")
                            _status.sn1 = systemInfo.serinum;
                    }

                    i_setState.To(State.GET_POOL);
                }
                else if (CurrentState == State.GET_POOL)
                {
                    AntminerPoolInfo poolInfo = JsonSerializer.Deserialize<AntminerPoolInfo>(str);

                    if (poolInfo != null && poolInfo.POOLS.Count > 0)
                    {
                        _status.pool1addr = poolInfo.POOLS[0].url;
                        _status.pool1name = poolInfo.POOLS[0].user;
                        _status.pool1IsAlive = poolInfo.POOLS[0].status;
                        _status.pool1accepted = poolInfo.POOLS[0].accepted.ToString();

                        if (poolInfo.POOLS.Count > 1)
                        {
                            _status.pool2addr = poolInfo.POOLS[1].url;
                            _status.pool2name = poolInfo.POOLS[1].user;
                            _status.pool2IsAlive = poolInfo.POOLS[1].status;
                            _status.pool2accepted = poolInfo.POOLS[1].accepted.ToString();

                            if (poolInfo.POOLS.Count > 2)
                            {
                                _status.pool2addr = poolInfo.POOLS[2].url;
                                _status.pool2name = poolInfo.POOLS[2].user;
                                _status.pool2IsAlive = poolInfo.POOLS[2].status;
                                _status.pool2accepted = poolInfo.POOLS[2].accepted.ToString();
                            }
                        }
                    }

                    i_setState.To(State.GET_STATS);
                }
                else if (CurrentState == State.GET_STATS)
                {
                    StatsInformation statsInformation = JsonSerializer.Deserialize<StatsInformation>(str);
                    if (statsInformation != null)
                    {
                        List<StatsInformation.Stat> stat = statsInformation.STATS;
                        if (stat != null && stat.Count > 0)
                        {
                            StatsInformation.Stat s = stat[0];

                            TimeSpan ts = new TimeSpan(0, 0, s.elapsed);
                            _status.elapsed = $"day:{ts.Days}, hour:{ts.Hours} min:{ts.Minutes} sec:{ts.Seconds}";

                            _status.rateIdeal = s.rate_ideal.ToString();
                            _status.rate_avg = s.rate_avg.ToString();
                            _status.rateName = s.rate_unit;

                            int[] fan = s.fan.ToArray();
                            if (fan.Length == 4)
                            {
                                _status.fan1 = fan[0].ToString();
                                _status.fan2 = fan[1].ToString();
                                _status.fan3 = fan[2].ToString();
                                _status.fan4 = fan[3].ToString();
                            }

                            List<StatsInformation.Chain> c = s.chain;
                            if (c != null && c.Count > 0)
                            {
                                _status.rate1 = c[0].rate_real.ToString();
                                _status.rate1Ideal = c[0].rate_ideal.ToString();

                                int[] temp_pic = c[0].temp_pic.ToArray();
                                if (temp_pic != null && temp_pic.Length == 4)
                                    _status.chipTemp1 = c[0].temp_pic[3].ToString();

                                if (c.Count > 1)
                                {
                                    _status.rate2 = c[1].rate_real.ToString();
                                    _status.rate2Ideal = c[1].rate_ideal.ToString();

                                    temp_pic = c[1].temp_pic.ToArray();
                                    if (temp_pic != null && temp_pic.Length == 4)
                                        _status.chipTemp1 = c[1].temp_pic[3].ToString();

                                    if (c.Count > 2)
                                    {
                                        _status.rate3 = c[2].rate_real.ToString();
                                        _status.rate3Ideal = c[2].rate_ideal.ToString();

                                        temp_pic = c[2].temp_pic.ToArray();
                                        if (temp_pic != null && temp_pic.Length == 4)
                                            _status.chipTemp1 = c[2].temp_pic[3].ToString();
                                    }
                                }
                            }
                        }
                    }

                    Console("\n" + _status.GetShow());
                }
            }
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
            public const string GET_POOL = "/cgi-bin/pools.cgi";
            public const string GET_STATS = "/cgi-bin/stats.cgi";
        }
    }

    //S19, S19 Pro, D9, E9Pro
    public class AntminerPoolInfo
    {
        public Status STATUS { get; set; }
        public Info INFO { get; set; }
        public List<Pool> POOLS { get; set; }

        public class Info
        {
            public string miner_version { get; set; }
            public string CompileTime { get; set; }
            public string type { get; set; }
        }

        public class Pool
        {
            public int index { get; set; }
            public string url { get; set; }
            public string user { get; set; }
            public string status { get; set; }
            public int priority { get; set; }
            public int getworks { get; set; }
            public int accepted { get; set; }
            public int rejected { get; set; }
            public int discarded { get; set; }
            public int stale { get; set; }
            public string diff { get; set; }
            public int diff1 { get; set; }
            public double diffa { get; set; }
            public int diffr { get; set; }
            public int diffs { get; set; }
            public int lsdiff { get; set; }
            public string lstime { get; set; }
        }

        public class Status
        {
            public string STATUS { get; set; }
            public int when { get; set; }
            public string Msg { get; set; }
            public string api_version { get; set; }
        }
    }

    public class StatsInformation
    {
        public Status STATUS { get; set; }
        public Info INFO { get; set; }
        public List<Stat> STATS { get; set; }

        public class Stat
        {
            public int elapsed { get; set; }
            public double rate_5s { get; set; }
            public double rate_30m { get; set; }
            public double rate_avg { get; set; }
            public double rate_ideal { get; set; }
            public string rate_unit { get; set; }
            public int chain_num { get; set; }
            public int fan_num { get; set; }
            public List<int> fan { get; set; }
            public double hwp_total { get; set; }

            public int minermode { get; set; }

            public int freqlevel { get; set; }
            public List<Chain> chain { get; set; }
        }

        public class Status
        {
            public string STATUS { get; set; }
            public int when { get; set; }
            public string Msg { get; set; }
            public string api_version { get; set; }
        }

        public class Chain
        {
            public int index { get; set; }
            public int freq_avg { get; set; }
            public double rate_ideal { get; set; }
            public double rate_real { get; set; }
            public int asic_num { get; set; }
            public string asic { get; set; }
            public List<int> temp_pic { get; set; }
            public List<int> temp_pcb { get; set; }
            public List<int> temp_chip { get; set; }
            public int hw { get; set; }
            public bool eeprom_loaded { get; set; }
            public string sn { get; set; }
            public double hwp { get; set; }
            public List<List<int>> tpl { get; set; }
        }

        public class Info
        {
            public string miner_version { get; set; }
            public string CompileTime { get; set; }
            public string type { get; set; }
        }
    }

    public class Status
    {
        public string GetShow()
        {
            string result = "";
            {
                result += $"Model:{model}\n";
                result += $"RateName:{rateName}\n";
                result += $"RateIdeal:{rateIdeal}\n";
                result += $"MAC:{mac}\n";
                result += $"SN1:{sn1}\n";
                result += $"SN2:{sn2}\n";
                result += $"RateAvg:{rate_avg}\n";
                result += $"Elapsed:{elapsed}\n";

                result += $"Pool1Addr:{pool1addr}\n";
                result += $"Pool1IsAlive:{pool1IsAlive}\n";
                result += $"Pool1Name:{pool1name}\n";
                result += $"Pool1Accepted:{pool1accepted}\n";

                result += $"Pool2Addr:{pool2addr}\n";
                result += $"Pool2IsAlive:{pool2IsAlive}\n";
                result += $"Pool2Name:{pool2name}\n";
                result += $"Pool2Accepted:{pool2accepted}\n";

                result += $"Pool2Addr:{pool3addr}\n";
                result += $"Pool2IsAlive:{pool3IsAlive}\n";
                result += $"Pool2Name:{pool3name}\n";
                result += $"Pool2Accepted:{pool3accepted}\n";

                result += $"Rate1:{rate1}\n";
                result += $"Rate1Ideal:{rate1Ideal}\n";
                result += $"Chip1Temp:{chipTemp1}\n";

                result += $"Rate2:{rate2}\n";
                result += $"Rate2Ideal:{rate2Ideal}\n";
                result += $"Chip2Temp:{chipTemp2}\n";

                result += $"Rate3:{rate3}\n";
                result += $"Rate3Ideal:{rate3Ideal}\n";
                result += $"Chip3Temp:{chipTemp3}\n";

                result += $"Fan1:{fan1}\n";
                result += $"Fan2:{fan2}\n";
                result += $"Fan3:{fan3}\n";
                result += $"Fan4:{fan4}\n";
            }
            return result;
        }

        // Модель машины.
        public string model { get; set; } = "";
        //GH/s
        public string rateName { get; set; } = "";
        // Сколько должена выдовать машинка хешей.
        public string rateIdeal { get; set; } = "";
        // мак
        public string mac { get; set; } = "";
        // серийный номер из устройсва.
        public string sn1 { get; set; } = "";
        // назначеный серийный номер из устройсва.
        public string sn2 { get; set; } = "";
        // Общий хершейт машины.
        public string rate_avg { get; set; } = "";
        // Время работы.
        public string elapsed { get; set; } = "";

        public string pool1addr { get; set; } = "";
        // Alive, Dead
        public string pool1IsAlive { get; set; } = "";
        public string pool1name { get; set; } = "";
        public string pool1accepted { get; set; } = "";

        public string pool2addr { get; set; } = "";
        public string pool2IsAlive { get; set; } = "";
        public string pool2name { get; set; } = "";
        public string pool2accepted { get; set; } = "";

        public string pool3addr { get; set; } = "";
        public string pool3IsAlive { get; set; } = "";
        public string pool3name { get; set; } = "";
        public string pool3accepted { get; set; } = "";

        public string rate1 { get; set; } = "";
        public string rate1Ideal { get; set; } = "";
        public string chipTemp1 { get; set; } = "";

        public string rate2 { get; set; } = "";
        public string rate2Ideal { get; set; } = "";
        public string chipTemp2 { get; set; } = "";

        public string rate3 { get; set; } = "";
        public string rate3Ideal { get; set; } = "";
        public string chipTemp3 { get; set; } = "";

        public string fan1 { get; set; } = "";
        public string fan2 { get; set; } = "";
        public string fan3 { get; set; } = "";
        public string fan4 { get; set; } = "";
    }

    //S19, 
    public class AntminerSystemInfo
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