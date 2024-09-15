using System.Net;
using System.Text;
using System.Text.Json;
using Butterfly;

namespace Zealot.device
{
    /// <summary>
    /// Стандартная прошика. 
    /// </summary>
    public abstract class AntminerDefaultController : Controller.Board.LocalField<Setting>, IDevice
    {
        public struct State
        {
            public const string NONE = "None";
            public const string GET_SYSTEM_INFO = "GetSystemInfo";
            public const string GET_POOL = "GetPool";
            public const string GET_STATS = "GetStats";

            public const string UPDATE = "Update";
            public const string WAIT = "WaitState";
        }

        public readonly AntminerStatus Status = new ();

        private CookieContainer _cookeis = new ();

        protected string CurrentState { set; get; } = State.NONE;

        protected bool IsRun { set; get; } = true;

        protected IInput<string> I_requestInformation;
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

                if (nextState == State.GET_SYSTEM_INFO)
                {
                    if (CurrentState == State.NONE || CurrentState == State.UPDATE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.GET_SYSTEM_INFO;

                        I_requestInformation.To(URL.GET_SYSTEM_INFO);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE}] или [{State.UPDATE}]");
                }
                else if (nextState == State.GET_POOL)
                {
                    if (CurrentState == State.GET_SYSTEM_INFO)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

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
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.GET_STATS;

                        I_requestInformation.To(URL.GET_STATS);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.GET_POOL}]");
                }
                else if (nextState == State.WAIT)
                {
                    if (CurrentState == State.GET_STATS)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.WAIT;

                        I_setState.To(State.UPDATE);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.GET_STATS}] ");
                }
                else if (nextState == State.UPDATE)
                {
                    if (CurrentState == State.WAIT)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.UPDATE;

                        I_setState.To(State.GET_SYSTEM_INFO);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.UPDATE}] ");
                }
                else
                {
                    Logger.S_E.To(this, $"Вы попытались установить неизветное состняие [{nextState}] при текущем состоянии [{CurrentState}].");

                    destroy();

                    return;
                }
            }
        }

        protected void IRequestInformation(string url)
        {
            if (IsRun == false) return;

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

                                return;
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
            if (IsRun == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                try
                {
                    if (CurrentState == State.GET_SYSTEM_INFO)
                    {
                        AntminerSystemInfo systemInfo = JsonSerializer.Deserialize<AntminerSystemInfo>(str);

                        if (systemInfo != null)
                        {
                            Status.MinerType = systemInfo.minertype;
                            Status.model = systemInfo.minertype;
                            Status.mac = systemInfo.macaddr;
                            _MAC = Status.mac;

                            if (systemInfo.serinum != "")
                                Status.sn1 = systemInfo.serinum;

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
                                    // Если информация об асике получена.
                                    I_setState.To(State.GET_POOL);
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
                            Logger.W.To(this, "Неудалось получить системную информацию");

                            destroy();

                            return;
                        }
                    }
                    else if (CurrentState == State.GET_POOL)
                    {
                        AntminerPoolInfo poolInfo = JsonSerializer.Deserialize<AntminerPoolInfo>(str);

                        if (poolInfo != null && poolInfo.POOLS.Count > 0)
                        {
                            Status.pool1addr = poolInfo.POOLS[0].url;
                            Status.pool1name = poolInfo.POOLS[0].user;
                            Status.pool1IsAlive = poolInfo.POOLS[0].status;
                            Status.pool1accepted = poolInfo.POOLS[0].accepted.ToString();

                            if (poolInfo.POOLS.Count > 1)
                            {
                                Status.pool2addr = poolInfo.POOLS[1].url;
                                Status.pool2name = poolInfo.POOLS[1].user;
                                Status.pool2IsAlive = poolInfo.POOLS[1].status;
                                Status.pool2accepted = poolInfo.POOLS[1].accepted.ToString();

                                if (poolInfo.POOLS.Count > 2)
                                {
                                    Status.pool2addr = poolInfo.POOLS[2].url;
                                    Status.pool2name = poolInfo.POOLS[2].user;
                                    Status.pool2IsAlive = poolInfo.POOLS[2].status;
                                    Status.pool2accepted = poolInfo.POOLS[2].accepted.ToString();
                                }
                            }
                        }
                        else
                        {
                            Logger.W.To(this, $"Неудалось получить ");

                            destroy();

                            return;
                        }

                        I_setState.To(State.GET_STATS);
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
                                Status.elapsed = $"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}s";

                                Status.rateIdeal = s.rate_ideal.ToString();
                                Status.rate = s.rate_5s.ToString();
                                Status.rateName = s.rate_unit;

                                int[] fan = s.fan.ToArray();
                                if (fan.Length == 4)
                                {
                                    Status.fan1 = fan[0].ToString();
                                    Status.fan2 = fan[1].ToString();
                                    Status.fan3 = fan[2].ToString();
                                    Status.fan4 = fan[3].ToString();
                                }
                                else if (fan.Length == 2)
                                {
                                    Status.fan1 = fan[0].ToString();
                                    Status.fan2 = fan[1].ToString();
                                    Status.fan3 = "";
                                    Status.fan4 = "";
                                }

                                List<StatsInformation.Chain> c = s.chain;

                                if (c != null && c.Count > 0)
                                {
                                    Status.rate1 = c[0].rate_real.ToString();
                                    Status.rate1Ideal = c[0].rate_ideal.ToString();

                                    int[] temp_pic = c[0].temp_pic.ToArray();
                                    if (temp_pic != null && temp_pic.Length >= 2)
                                        Status.chipTemp1 = c[0].temp_chip[1].ToString();

                                    if (c.Count > 1)
                                    {
                                        Status.rate2 = c[1].rate_real.ToString();
                                        Status.rate2Ideal = c[1].rate_ideal.ToString();

                                        temp_pic = c[1].temp_pic.ToArray();
                                        if (temp_pic != null && temp_pic.Length >= 2)
                                            Status.chipTemp2 = c[1].temp_chip[1].ToString();

                                        if (c.Count > 2)
                                        {
                                            Status.rate3 = c[2].rate_real.ToString();
                                            Status.rate3Ideal = c[2].rate_ideal.ToString();

                                            temp_pic = c[2].temp_pic.ToArray();
                                            if (temp_pic != null && temp_pic.Length >= 2)
                                                Status.chipTemp3 = c[2].temp_chip[1].ToString();
                                        }
                                    }

                                    string miningPowerName = "GH";
                                    if (Status.model.ToLower().Contains("z15"))
                                        miningPowerName = "Ksol";

                                    OutputDataJson data = new OutputDataJson()
                                    {
                                        UniqueNumber = AsicInit.UniqueNumber,

                                        Culler1_power = Status.fan1,
                                        Culler2_power = Status.fan2,
                                        Culler3_power = Status.fan3,
                                        Culler4_power = Status.fan4,

                                        WorkTime = Status.elapsed,
                                        //Mode = Status.rate
                                        IPAddress = Field.IPAddress,

                                        //MiningPowerSize = Status.rate_avg.Replace(",", ""),
                                        //MiningPower1Size = Status.rate1.Replace(",", ""),
                                        //MiningPower2Size = Status.rate2.Replace(",", ""),
                                        //MiningPower3Size = Status.rate3.Replace(",", ""),
                                        MiningPowerSize = Status.rate.Contains(",") ?
                                            Status.rate.Split(',')[0] : Status.rate,

                                        MiningPower1Size = Status.rate1,
                                        MiningPower2Size = Status.rate2,
                                        MiningPower3Size = Status.rate3,

                                        MiningPowerName = miningPowerName,

                                        Temp1 = Status.chipTemp1,
                                        Temp2 = Status.chipTemp2,
                                        Temp3 = Status.chipTemp3,
                                    };

                                    /*
                                    if (Status.Pool1_Active == "true")
                                        data.PoolActiveURL = "1)" + Status.Pool1_URL;
                                    else if (Status.Pool2_Active == "true")
                                        data.PoolActiveURL = "2)" + Status.Pool2_URL;
                                    else if (Status.Pool2_Active == "true")
                                        data.PoolActiveURL = "3" + Status.Pool2_URL;
                                    else
                                        data.PoolActiveURL = "N/A";
                                        */

                                    AsicInit.SendDataMessage(JsonSerializer.SerializeToUtf8Bytes(data));

                                    I_setState3sDelay.To(State.WAIT);
                                }
                                else
                                {
                                    Logger.W.To(this, $"Неудалось получить STAT из Json обьеата.");

                                    destroy();

                                    return;
                                }
                            }
                            else
                            {
                                Logger.W.To(this, $"Неудалось извлечь STAT из Json обьеата.");

                                destroy();

                                return;
                            }
                        }
                        else
                        {
                            Logger.W.To(this, $"State information is null");

                            destroy();

                            return;
                        }

                        //Console("\n" + Status.GetShow());
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

        public string GetMAC() => _MAC;

        public void SetMAC()
        {
            throw new NotImplementedException();
        }

        public string GetAddress() => Status.Address;

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

        public WhatsMinerStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public void Destroy(string destroyInfo)
        {
            Logger.I.To(this, $"Внешний вызов destroy(). Причина:{destroyInfo}.");

            destroy();
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
            public long diffr { get; set; }
            public long diffs { get; set; }
            public long lsdiff { get; set; }
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

    public class AntminerStatus
    {
        public string MinerType { set; get; }
        public string Address { set; get; }

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
                result += $"RateAvg:{rate}\n";
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
        public string rate { get; set; } = "";
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