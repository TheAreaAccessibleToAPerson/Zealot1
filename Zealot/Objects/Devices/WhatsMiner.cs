using System.Net;
using System.Text;
using Butterfly;

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
            public const string DOWNLOAD_MAC = "DownloadMac";
        }

        IInput<string> i_setState;

        public string _currentState = State.NONE;

        public const string NAME = "WhatsMiner";

        void Construction()
        {
            input_to(ref i_setState, Header.Events.SCAN_DEVICES, (nextState) =>
            {
                if (nextState == State.DOWNLOAD_MAC)
                {
                    if (_currentState == State.NONE)
                    {
                        Logger.I.To(this, $"Сменил состояние [{_currentState}]->[{nextState}]");

                        _currentState = nextState;
                    }
                    else Logger.S_E.To(this, $"Попытка сменить состояние с [{_currentState}] на " +
                        $"[{nextState}]. Данную операцию можно произвести только если текущее " +
                        $" состояние равно [{State.NONE}]");
                }
                else
                {
                }
            });
        }

        void Start()
        {
            //HttpsAuthorizationRequest();
        }

        CookieContainer cookeis = new CookieContainer();

        // Загружаем страницу с которой нужно получить мак.
        public void HttpRequestInterface()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://10.90.29.200/{URL.INTERFACE}");

                request.ServerCertificateValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => { return true; };

                request.CookieContainer = cookeis;

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                Logger.W.To(this, $"Неудалось запросить станицу {URL.INTERFACE}. {ex}");

                destroy();
            }
        }

        private void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();

                    Console(str);
                    // полная перезапись файла 

                    using (StreamWriter writer = new StreamWriter("whatsMinerInterface.txt", false))
                    {
                        writer.WriteLine(str);
                    }
                }
            }
            catch (Exception ex)
            {
                Console(ex.ToString());
            }
        }

        public string GetHashrate()
        {
            throw new NotImplementedException();
        }

        public string GetMAC()
        {
            throw new NotImplementedException();
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
            public const string STATUS = "/cgi-bin/luci?luci_username=admin&luci_password=admin";
            public const string INTERFACE = "/cgi-bin/luci/admin/network/network?luci_username=admin&luci_password=admin";
            public const string CONFIGURATION = "/cgi-bin/luci/admin/network/btminer?luci_username=admin&luci_password=admin";
        }
    }
}