using System.Net;
using System.Text;
using System.Text.Json;
using Butterfly;

namespace Zealot.device.whatsminer
{
    public abstract class ControllerBoard : Butterfly.Controller.Board.LocalField<Setting>
    {
        public bool IsRunning = true;

        public struct State
        {
            public const string NONE = "None";
            public const string DOWNLOAD_MAC_AND_UPLOAD = "DownloadMacAndUpload";
            public const string DOWNLOAD_POOL = "DowloadPool";
            /// <summary>
            /// Пытаемся получить информацию о данном устройсве из базы устройсв. 
            /// </summary>
            public const string ASIC_INIT = "AsicInit";
            // Пуллы загружается по 2 ссылкам, данное состояние будет выставленно если 
            // не удалось загрузить по первой ссылке.
            public const string ANOTHER_DOWNLOAD_POOL = "AnotherDowloadPool";
            public const string DOWNLOAD_POWER_MODE = "DowloadPowerMode";
            public const string DOWNLOAD_STATE = "DowloadState";

            // Все данные выкачены, ожидаем запусука обновления.
            public const string WAIT_STATE = "WaitState";

            // Обновляет все данные кроме мака.
            public const string UPDATE = "Update";
        }

        protected AsicStatus Status = new AsicStatus();

        CookieContainer cookeis = new CookieContainer();

        protected IInput<string> i_setState;
        protected IInput<string> i_extractResult;

        private static int index = 0;
        private static object locker = new();


        ///  <summary>
        /// Время работы.
        /// </summary>
        protected long _uptime = 0;
        protected string _uptimeString = "";

        protected float _hashrate = 0.0f;

        protected string _MAC = "";
        protected string _coinType = "";
        protected string[] _pools = new string[3];
        protected string[] _workers = new string[3];
        protected string[] _passwords = new string[3];
        protected string _powerMode = "";

        /// <summary>
        /// Запрашивает интерфейс для того что бы получить MAC и время работы. 
        /// </summary>
        protected IInput<string> I_requestInformation;
        protected IInput I_requestConfiguration;

        protected string CurrentState = State.NONE;

        //************ASIC INIT****************
        // Зарпашивает данные об асике.
        protected AsicInit AsicInit;
        protected IInput<string> I_asicInit;
        protected IInput<byte[]> I_sendBytesMessageToClients;
        protected IInput<string> I_sendStringMessageToClients;


        //**********ADD NEW ASIC TO DEVICES MANAGER***********
        protected IInput I_addAsicToDictionary;
        private bool isAddAsicToDictionary = false;

        protected void ISetState(string nextState)
        {
            if (IsRunning == false) return;

            lock (StateInformation.Locker)
            {
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;

                if (nextState == State.DOWNLOAD_MAC_AND_UPLOAD)
                {
                    if (CurrentState == State.NONE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = nextState;

                        I_requestInformation.To(URL.INTERFACE);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE}]");
                }
                else if (nextState == State.ASIC_INIT)
                {
                    if (CurrentState == State.DOWNLOAD_MAC_AND_UPLOAD || CurrentState == State.UPDATE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = nextState;

                        I_asicInit.To(_MAC);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_MAC_AND_UPLOAD}]");
                }
                else if (nextState == State.DOWNLOAD_POOL)
                {
                    if (CurrentState == State.ASIC_INIT || CurrentState == State.DOWNLOAD_MAC_AND_UPLOAD ||
                        CurrentState == State.UPDATE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.DOWNLOAD_POOL;

                        I_requestInformation.To(URL.POOL1);
                    }
                    else if (CurrentState == State.DOWNLOAD_MAC_AND_UPLOAD) Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_MAC_AND_UPLOAD}]");
                }
                else if (nextState == State.ANOTHER_DOWNLOAD_POOL)
                {
                    if (CurrentState == State.DOWNLOAD_POOL)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.ANOTHER_DOWNLOAD_POOL;

                        I_requestInformation.To(URL.POOL2);
                    }
                    else if (CurrentState == State.DOWNLOAD_POOL) Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_POOL}]");
                }
                else if (nextState == State.DOWNLOAD_POWER_MODE)
                {
                    if (CurrentState == State.DOWNLOAD_POOL || CurrentState == State.ANOTHER_DOWNLOAD_POOL)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.DOWNLOAD_POWER_MODE;

                        I_requestInformation.To(URL.POWER_MODE);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_POOL}] или [{State.ANOTHER_DOWNLOAD_POOL}]");
                }
                else if (nextState == State.DOWNLOAD_STATE)
                {
                    if (CurrentState == State.DOWNLOAD_POWER_MODE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.DOWNLOAD_STATE;

                        I_requestInformation.To(URL.DOWNLOAD_STATE);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_POWER_MODE}] ");
                }
                else if (nextState == State.WAIT_STATE)
                {
                    if (CurrentState == State.DOWNLOAD_STATE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.WAIT_STATE;

                        i_setState.To(State.UPDATE);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.DOWNLOAD_STATE}] ");
                }
                else if (nextState == State.UPDATE)
                {
                    if (CurrentState == State.WAIT_STATE)
                    {
                        //Logger.I.To(this, $"Сменил состояние [{CurrentState}]->[{nextState}]");

                        CurrentState = State.UPDATE;

                        i_setState.To(State.DOWNLOAD_POOL);
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.UPDATE}] ");
                }
                else Logger.S_E.To(this, $"Попытка сменить состояние с [{CurrentState}] на " +
                        $"[{nextState}]. [{nextState}] неизвестное состояние.");
            }
        }

        public void Update()
        {
            if (IsRunning == false) return;

            if (CurrentState == State.WAIT_STATE)
            {
                i_setState.To(State.UPDATE);
            }
        }

        void Stop()
        {
        }

        protected void IRequestInformation(string url)
        {
            if (IsRunning == false) return;

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
        }

        void Result(string str)
        {
            if (IsRunning == false) return;

            if (CurrentState == State.DOWNLOAD_MAC_AND_UPLOAD)
            {
                int result = hellpers.WhatsMiner.ExtractMACAndUpload(str, out string error, out _MAC, out _uptime);
                if (result == 0)
                {
                    if (_MAC != "")
                    {
                        Status.MAC = _MAC;

                        //Result1.Reiceve(Field.IPAddress, _MAC);

                        // Проверим по маку имеется ли такая машинка в наличии.
                        if (isAddAsicToDictionary == false)
                        {
                            I_addAsicToDictionary.To();
                        }
                        else
                        {
                            // Проверим получили ли мы данные по этой машинки из быза данных.
                            if (AsicInit != null)
                            {
                                // Если информация об асике получена.
                                i_setState.To(State.DOWNLOAD_POOL);
                            }
                        }
                    }
                    else 
                    {
                        Logger.E.To(this, $"В результате парсинга был получен пустой мак.");

                        destroy();

                        return;
                    }
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
            else if (CurrentState == State.DOWNLOAD_POOL || CurrentState == State.ANOTHER_DOWNLOAD_POOL)
            {
                int poolExtractResult = hellpers.WhatsMiner.ExtractPool(str, out string poolExtractResultInfo, out _pools);
                if (poolExtractResult == 0)
                {
                    // Logger.I.To(this, poolExtractResultInfo);
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
                    //Logger.I.To(this, workerExtractResultInfo);
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
                    //Logger.I.To(this, passwordExtractResultInfo);
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
            else if (CurrentState == State.DOWNLOAD_POWER_MODE)
            {
                int powerExtractResult = hellpers.WhatsMiner.ExtractPowerMode(str, out string powerInfo, out _powerMode);
                if (powerExtractResult == 0)
                {
                    //Logger.I.To(this, powerInfo);

                    i_setState.To(State.DOWNLOAD_STATE);
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
            else if (CurrentState == State.DOWNLOAD_STATE)
            {
                int extractMainPageResult =
                    hellpers.WhatsMiner.ExtractMainPage(str, out string downloadStateInfo, ref Status);

                if (extractMainPageResult == 0)
                {
                    Logger.I.To(this, $"Отправляем сообщение клиeнтам.");

                    if (AsicInit != null)
                    {
                        OutputDataJson data = new OutputDataJson()
                        {
                            UniqueNumber = AsicInit.UniqueNumber,

                            Culler1_power = Status.FanSpeedIn,
                            Culler2_power = Status.FanSpeedOut,
                        };

                        AsicInit.SendDataMessage(JsonSerializer.SerializeToUtf8Bytes(data));

                        i_setState.To(State.WAIT_STATE);
                    }
                    else 
                    {
                        Logger.S_E.To(this, $"В момент отправки сообщения от асика клинтам поле AsicInit окозалось null.");

                        destroy();

                        return;
                    }
                }
                else if (extractMainPageResult == 1)
                {
                    // Удалось получить не все поля.
                    Logger.I.To(this, downloadStateInfo);
                }
                else if (extractMainPageResult == 2)
                {
                    Logger.W.To(this, downloadStateInfo);

                    destroy();

                    return;
                }
                else
                {
                    Logger.W.To(this, "Вы получили неизвестный тип ошибки во время извлечения данных с главной страницы с ватсмайнера.");

                    destroy();

                    return;
                }
            }
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
            public const string DOWNLOAD_STATE = "cgi-bin/luci?luci_username=admin&luci_password=admin";
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