using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Butterfly;
using Butterfly.system.objects.main.controller;

namespace Zealot.device
{
    // 1)Выгружаем мак.
    // 2)Настройки(пулы, воркеры.)
    // 3)
    public sealed class WhatsMiner : Controller.Board.LocalField<Setting>,
        IDevice
    {
        public struct State
        {
            public const string NONE = "None";
            public const string DOWNLOAD_MAC_AND_UPLOAD = "DownloadMacAndUpload";
            public const string DOWNLOAD_POOL = "DowloadPool";
            // Пуллы загружается по 2 ссылкам, данное состояние будет выставленно если 
            // не удалось загрузить по первой ссылке.
            public const string ANOTHER_DOWNLOAD_POOL = "AnotherDowloadPool";
            public const string DOWNLOAD_POWER_MODE = "DowloadPowerMode";
            public const string DOWNLOAD_STATE = "DowloadState";
        }

        CookieContainer cookeis = new CookieContainer();

        IInput<string> i_setState;
        IInput<string> i_extractResult;

        /// <summary>
        /// Запрашивает интерфейс для того что бы получить MAC и время работы. 
        /// </summary>
        IInput<string> i_requestInformation;
        IInput i_requestConfiguration;

        public string _currentState = State.NONE;

        public const string NAME = "WhatsMiner";

        private static int index = 0;
        private static object locker = new();
        void Construction()
        {
            input_to(ref i_setState, Header.Events.SCAN_DEVICES, (nextState) =>
            {
                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                    if (nextState == State.DOWNLOAD_MAC_AND_UPLOAD)
                    {
                        if (_currentState == State.NONE)
                        {
                            //Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                            _currentState = nextState;
                            lock (locker)
                            {
                                index++;
                                Logger.I.To(this, $"COUNT:{index}");
                            }

                            i_requestInformation.To(URL.INTERFACE);
                        }
                        else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                            $" состояние равно [{State.NONE}]");
                    }
                    else if (nextState == State.DOWNLOAD_POOL)
                    {
                        if (_currentState == State.DOWNLOAD_MAC_AND_UPLOAD)
                        {
                            //Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                            _currentState = nextState;

                            i_requestInformation.To(URL.POOL1);
                        }
                        else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                            $" состояние равно [{State.DOWNLOAD_MAC_AND_UPLOAD}]");
                    }
                    else if (nextState == State.ANOTHER_DOWNLOAD_POOL)
                    {
                        if (_currentState == State.DOWNLOAD_POOL)
                        {
                            //Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                            _currentState = nextState;

                            i_requestInformation.To(URL.POOL2);
                        }
                        else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                            $" состояние равно [{State.DOWNLOAD_POOL}]");
                    }
                    else if (nextState == State.DOWNLOAD_POWER_MODE)
                    {
                        if (_currentState == State.DOWNLOAD_POOL || _currentState == State.ANOTHER_DOWNLOAD_POOL)
                        {
                            //Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                            _currentState = nextState;

                            i_requestInformation.To(URL.POWER_MODE);
                        }
                        else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                            $" состояние равно [{State.DOWNLOAD_POOL}] или [{State.ANOTHER_DOWNLOAD_POOL}]");
                    }
                    else if (nextState == State.DOWNLOAD_STATE)
                    {
                        if (_currentState == State.DOWNLOAD_POOL)
                        {
                            //Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                            _currentState = nextState;
                        }
                        else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                            $" состояние равно [{State.DOWNLOAD_POOL}]");
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                            $"[{nextState}]. [{nextState}] неизвестное состояние.");

                }
            });

            input_to(ref i_requestInformation, (url) =>
            {
                lock (StateInformation.Locker)
                {
                    if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                    try
                    {
                        string urll = $"https://{Field.IPAddress}/{url}";

                        HttpWebRequest request = (HttpWebRequest)
                            WebRequest.Create($"https://{Field.IPAddress}/{url}");

                        request.ServerCertificateValidationCallback =
                            (sender, cert, chain, sslPolicyErrors) => { return true; };

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
                                    // Если мы получаем html страницу с пулами
                                    // то на некоторых машинах итой адррес ... 
                                    if (_currentState == State.DOWNLOAD_POOL)
                                    {
                                        // В случае если не удалось получить данные по первой ссылке
                                        // пробуем получить по второй.
                                        i_setState.To(State.ANOTHER_DOWNLOAD_POOL);
                                    }
                                    else
                                    {
                                        Logger.W.To(this, $"Неудалось получить данные у {NAME}.[{_currentState}] {ex}");
                                    }

                                    destroy();
                                }
                            }

                        }), request);
                    }
                    catch (Exception ex)
                    {
                        Logger.W.To(this, $"Неудалось запросить станицу {URL.INTERFACE}. {ex}");

                        destroy();
                    }
                }
            });
        }

        void Start()
        {
            i_setState.To(State.DOWNLOAD_MAC_AND_UPLOAD);
        }

        void Result(string str)
        {
            if (_currentState == State.DOWNLOAD_MAC_AND_UPLOAD)
            {
                int result = hellpers.WhatsMiner.ExtractMACAndUpload(str, out string error, out _MAC, out _uptime);
                if (result == 0)
                {
                    i_setState.To(State.DOWNLOAD_POOL);
                }
                else if (result == 1)
                {
                    Logger.W.To(this, error);

                    destroy();
                }
                else 
                {
                    Logger.S_E.To(this, $"Вы получили неизвестный тип ошибки во время извлечение мака и время работы из ватсмайнера.");

                    destroy();
                }
            }
            else if (_currentState == State.DOWNLOAD_POOL || _currentState == State.ANOTHER_DOWNLOAD_POOL)
            {
                int poolExtractResult = hellpers.WhatsMiner.ExtractPool(str, out string poolExtractResultInfo, out _pools);
                if (poolExtractResult == 0)
                {
                    Logger.I.To(this, poolExtractResultInfo);
                }
                else if (poolExtractResult == 1)
                {
                    Logger.W.To(this, poolExtractResultInfo);
                    destroy();
                    return;
                }
                else 
                {
                    Logger.S_E.To(this, $"Вы получили неизвестный тип ошибки во время извлечение пуллов из ватсмайнера.");
                    destroy();
                    return;
                }

                int workerExtractResult = hellpers.WhatsMiner.ExtractWorker(str, out string workerExtractResultInfo, out _pools);
                if (workerExtractResult == 0)
                {
                    Logger.I.To(this, workerExtractResultInfo);
                }
                else if (poolExtractResult == 1)
                {
                    Logger.W.To(this, workerExtractResultInfo);
                    destroy();
                    return;
                }
                else 
                {
                    Logger.S_E.To(this, $"Вы получили неизвестный тип ошибки во время извлечение воркеров из ватсмайнера.");
                    destroy();
                    return;
                }

                int passwordExtractResult = hellpers.WhatsMiner.ExtractPassword(str, out string passwordExtractResultInfo, out _passwords);
                if (passwordExtractResult == 0)
                {
                    Logger.I.To(this, passwordExtractResultInfo);
                }
                else if (passwordExtractResult == 1)
                {
                    Logger.W.To(this, passwordExtractResultInfo);
                    destroy();
                    return;
                }
                else 
                {
                    Logger.S_E.To(this, $"Вы получили неизвестный тип ошибки во время извлечение паролей из ватсмайнера.");
                    destroy();
                    return;
                }

                i_setState.To(State.DOWNLOAD_POWER_MODE);
            }
            else if (_currentState == State.DOWNLOAD_POWER_MODE)
            {
                int powerExtractResult = hellpers.WhatsMiner.ExtractPowerMode(str, out string powerInfo, out _powerMode);
                if (powerExtractResult == 0)
                {
                    Logger.I.To(this, powerInfo);
                }
                else if (powerExtractResult == 1)
                {
                    Logger.W.To(this, powerInfo);

                    destroy();
                    return;
                }
                else 
                {
                    Logger.S_E.To(this, $"Вы получили неизвестный тип ошибки во время извлечение power mode из ватсмайнера.");
                    destroy();
                    return;
                }
            }
        }


        private float _hashrate = 0.0f;
        private string _MAC = "";
        private string _coinType = "";
        private string[] _pools = new string[3];
        private string[] _workers = new string[3];
        private string[] _passwords = new string[3];
        private string _powerMode = "";

        ///  <summary>
        /// Время работы.
        /// </summary>
        private long _uptime = 0;

        public float GetHashrate() => _hashrate;
        public string GetMAC() => _MAC;

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

        public string IsRunning()
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

        private struct URL
        {
            public const string STATUS = "cgi-bin/luci?luci_username=admin&luci_password=admin";
            public const string INTERFACE1 = "cgi-bin/luci/admin/network/network?luci_username=admin&luci_password=admin";
            public const string INTERFACE = "cgi-bin/luci/admin/network/iface_status/lan?luci_username=admin&luci_password=admin";

            // Если не удалось получить html по этой ссылке
            public const string POOL1 = "cgi-bin/luci/admin/network/btminer/pool?luci_username=admin&luci_password=admin";
            // то получаем по этой.
            public const string POOL2 = "cgi-bin/luci/admin/network/btminer/cgminer?luci_username=admin&luci_password=admin";
            public const string POWER_MODE = "cgi-bin/luci/admin/network/btminer/power?luci_username=admin&luci_password=admin";
        }
    }

    public class WhatsMinerInterface
    {
        public long rx_bytes { get; set; }
        public string ifname { get; set; }
        public long tx_bytes { get; set; }
        public List<string> ipaddrs { get; set; }
        public string gwaddr { get; set; }
        public long tx_packets { get; set; }
        public List<string> dnsaddrs { get; set; }
        public long rx_packets { get; set; }
        public string proto { get; set; }
        public string id { get; set; }
        public List<object> ip6addrs { get; set; }
        public long uptime { get; set; }
        public List<object> subdevices { get; set; }
        public bool is_up { get; set; }
        public string macaddr { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }
}