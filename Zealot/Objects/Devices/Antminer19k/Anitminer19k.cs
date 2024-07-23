using System.Net;
using System.Text;
using Butterfly;
using System.Text.Json;

namespace Zealot.device
{
    public sealed class Antminer19K : Controller.Board.LocalField<Setting>, IDevice
    {
        public const string NAME = "Antminer19k";

        public bool IsRunning = true;

        CookieContainer cookeis = new CookieContainer();

        IInput I_requestInformation;

        public class Root1
        {
            public string minertype { get; set; }
            public string nettype { get; set; }
            public string netdevice { get; set; }
            public string macaddr { get; set; }
            public string hostname { get; set; }
            public string ipaddress { get; set; }
            public string netmask { get; set; }
            public string gateway { get; set; }
            public string dnsservers { get; set; }
            public string system_mode { get; set; }
            public string system_kernel_version { get; set; }
            public string system_filesystem_version { get; set; }
            public string firmware_type { get; set; }
            public string serinum { get; set; }
        }

        void Start()
        {
            I_requestInformation.To();
        }

        void Construction()
        {
            input_to(ref I_requestInformation, Header.Events.SCAN_DEVICES, () =>
            {
                if (IsRunning == false) return;

                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                    try
                    {
                        string url = $"http://{Field.IPAddress}/cgi-bin/get_system_info.cgi";

                        HttpWebRequest request = (HttpWebRequest)
                            WebRequest.Create(url);

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

                                        Root1 r = JsonSerializer.Deserialize<Root1>(str.Trim());

                                        string m = r.serinum;

                                        if (m == "NGSBD4EBBJCJC3121")
                                        {
                                            Console(Field.IPAddress);
                                        }

                                        if (m == "HXXYDWEBBJCBE063A")
                                        {
                                            Console(Field.IPAddress);
                                        }

                                        if (m == "JYZZAEABCJHCA0ECD")
                                        {
                                            Console(Field.IPAddress);
                                        }

                                        if (m == null || m == "")
                                        {
                                            if (IsRunning == false) return;

                                            lock (StateInformation.Locker)
                                            {
                                                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                                                try
                                                {
                                                    string url = $"http://{Field.IPAddress}/cgi-bin/stats.cgi";

                                                    HttpWebRequest request = (HttpWebRequest)
                                                        WebRequest.Create(url);

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

                                                                    Root r = JsonSerializer.Deserialize<Root>(str.Trim());

                                                                    List<STAT> t = r.STATS;

                                                                    foreach (STAT g in t)
                                                                    {
                                                                        foreach (Chain c in g.chain)
                                                                        {
                                                                            string a = c.sn.Trim();

                                                                            if (a == "NGSBD4EBBJCJC3121")
                                                                            {
                                                                                Console(Field.IPAddress);
                                                                            }

                                                                            if (a == "HXXYDWEBBJCBE063A")
                                                                            {
                                                                                Console(Field.IPAddress);
                                                                            }

                                                                            if (a == "JYZZAEABCJHCA0ECD")
                                                                            {
                                                                                Console(Field.IPAddress);
                                                                            }

                                                                            //Console(c.sn);
                                                                        }
                                                                    }

                                                                    if (m == "NGSBD4EBBJCJC3121")
                                                                    {
                                                                        Console(Field.IPAddress);
                                                                    }

                                                                    if (m == "HXXYDWEBBJCBE063A")
                                                                    {
                                                                        Console(Field.IPAddress);
                                                                    }

                                                                    if (m == "JYZZAEABCJHCA0ECD")
                                                                    {
                                                                        Console(Field.IPAddress);
                                                                    }

                                                                    if (m == null || m == "")
                                                                    {
                                                                    }

                                                                    //Console(m);

                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console(ex.ToString());
                                                                /*
                                                                // Если мы получаем html страницу с пулами
                                                                // то на некоторых машинах итой адррес ... 
                                                                if (CurrentState == State.DOWNLOAD_POOL)
                                                                {
                                                                    // В случае если не удалось получить данные по первой ссылке
                                                                    // пробуем получить по второй.
                                                                    i_setState.To(State.ANOTHER_DOWNLOAD_POOL);
                                                                }
                                                                else
                                                                {
                                                                    Logger.W.To(this, $"Неудалось получить данные у {WhatsMiner.NAME}.[{CurrentState}] {ex}");

                                                                    destroy();
                                                                }
                                                                */
                                                            }
                                                        }

                                                    }), request);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Logger.W.To(this, $"Неудалось запросить станицу . {ex}");

                                                    destroy();
                                                }
                                            }

                                            //Console(m);

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console(ex.ToString());
                                    /*
                                    // Если мы получаем html страницу с пулами
                                    // то на некоторых машинах итой адррес ... 
                                    if (CurrentState == State.DOWNLOAD_POOL)
                                    {
                                        // В случае если не удалось получить данные по первой ссылке
                                        // пробуем получить по второй.
                                        i_setState.To(State.ANOTHER_DOWNLOAD_POOL);
                                    }
                                    else
                                    {
                                        Logger.W.To(this, $"Неудалось получить данные у {WhatsMiner.NAME}.[{CurrentState}] {ex}");

                                        destroy();
                                    }
                                    */
                                }
                            }

                        }), request);
                    }
                    catch (Exception ex)
                    {
                        Logger.W.To(this, $"Неудалось запросить станицу . {ex}");

                        destroy();
                    }
                }

            });
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

    public class INFO
    {
        public string miner_version { get; set; }
        public string CompileTime { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public STATUS1 STATUS { get; set; }
        public INFO INFO { get; set; }
        public List<STAT> STATS { get; set; }
    }

    public class STAT
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

    public class STATUS1
    {
        public string STATUS { get; set; }
        public int when { get; set; }
        public string Msg { get; set; }
        public string api_version { get; set; }
    }
}