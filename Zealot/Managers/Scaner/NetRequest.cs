using System.Net;
using System.Text;
using Butterfly;
using Zealot.manager;

namespace Zealot.hellper
{
    public class NetRequest : Controller.Board.LocalField<string>
    {
        public struct State
        {
            public const string NONE = "None";
            public const string HTTP_REQUEST = "HttpRequest";
            public const string HTTP_AUTHORIZATION_REQUEST = "HttpAuthorizationRequest";
            public const string HTTPS_AUTHORIZATION_REQUEST = "HttpsAuthorizationRequest";
        }

        private string _currentState = State.NONE;

        public string Address { private set; get; } = "";

        IInput<string, string> i_send;
        IInput i_request;
        IInput<string> i_emptyAddressToDevicesScaner;

        void Construction()
        {
            Address = Field;

            input_to(ref i_request, Request);
            send_message(ref i_send, Devices.BUS.RECEIVE_SCAN_DEVICES);
            send_message(ref i_emptyAddressToDevicesScaner, ScanerDevices.BUS.EMPTY_ADDRESS_TO_SCAN);
        }

        /// <summary>
        /// Если во время сканирование устройсво было освобождено, то
        /// укажим что оно доступно для повторного сканирования.
        /// </summary> 
        public void Destroy()
        {
            if (StateInformation.IsCallConstruction)
            {
                Logger.I.To(this, $"В данный момент устросво не доступно, освободим его аддрес {Address} и позже попробуем получить к нему доступ.");

                i_emptyAddressToDevicesScaner.To(Address);

                destroy();

                return;
            }
        }

        void Stop()
        {
        }

        void Start()
        {
            i_request.To();
        }

        void Configurate()
        {
            if (IPAddress.TryParse(Field, out IPAddress a))
            {
                Address = Field;
            }
            else
            {
                Logger.S_E.To(this, $"Вы пытаетесь получить доступ к " +
                    $" синтаксически неверному ip аддресу.");

                destroy();

                return;
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

                    i_send.To(Field, str);

                    destroy();
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
                {
                    if (_currentState == State.HTTP_REQUEST)
                    {
                        HttpAuthorizationRequest();
                    }
                    else if (_currentState == State.HTTPS_AUTHORIZATION_REQUEST)
                    {
                        //Logger.S_E.To(this, "Неудалось авторизоваться по логину/паролю admin.");
                        Destroy();
                    }
                    else
                    {
                        //Logger.S_E.To(this, "1 Неудалось авторизоваться.");
                        Destroy();
                    }
                }
                else if (ex.ToString().Contains("The SSL connection could not be established, see inner exception"))
                {
                    HttpsAuthorizationRequest();
                }
                else
                {
                    Destroy();
                    //Logger.S_E.To(this, ex.ToString());
                }
            }
        }

        public void Request()
        {
            Http2Request();
        }

        CookieContainer cookeis = new CookieContainer();

        private void Http2Request()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Address);
                //request.CookieContainer = cookeis;
                request.BeginGetResponse(new AsyncCallback((result) =>
                {
                    try
                    {
                        HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                        using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string str = stream.ReadToEnd();

                            i_send.To(Field, str);

                            destroy();

                            return;
                        }
                    }
                    catch (Exception httpEx)
                    {
                        try
                        {
                            if (httpEx.ToString().Contains("The SSL connection could not be established, see inner exception"))
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + Address + "/cgi-bin/luci" + "?luci_username=admin&luci_password=admin");

                                request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                                request.CookieContainer = cookeis;

                                request.BeginGetResponse(new AsyncCallback((result) =>
                                {
                                    try
                                    {
                                        HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                                        using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                        {
                                            string str = stream.ReadToEnd();

                                            i_send.To(Field, str);

                                            destroy();

                                            return;
                                        }
                                    }
                                    catch (Exception httpsAdminException)
                                    {
                                        if (httpsAdminException.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
                                        {
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + Address + "?username=root&password=root");

                                            request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                                            request.UseDefaultCredentials = false;
                                            //request.CookieContainer = cookeis;

                                            request.BeginGetResponse(new AsyncCallback((result) =>
                                            {
                                                try
                                                {
                                                    HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                                                    using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                                    {
                                                        string str = stream.ReadToEnd();

                                                        i_send.To(Field, str);

                                                        destroy();

                                                        return;
                                                    }
                                                }
                                                catch (Exception eee)
                                                {
                                                    //Console(eee.ToString());
                                                    Destroy();

                                                    return;
                                                }
                                            }),
                                            request);
                                        } //874
                                        else
                                        {
                                            //Console(httpsAdminException.ToString());
                                            Destroy();

                                            return;
                                        }
                                    }
                                }),
                                request);
                            }
                            else if (httpEx.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Address + "/");

                                request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                                request.Credentials = new NetworkCredential("root", "root");
                                //request.UseDefaultCredentials = false;
                                //request.CookieContainer = cookeis;

                                request.BeginGetResponse(new AsyncCallback((result) =>
                                {
                                    try
                                    {
                                        HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                                        using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                        {
                                            string str = stream.ReadToEnd();

                                            i_send.To(Field, str);

                                            destroy();
                                        }
                                    }
                                    catch (Exception eee)
                                    {
                                        //Console(eee.ToString());
                                        Destroy();
                                    }
                                }),
                                request);
                            }
                            else 
                            {
                                Destroy();
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console(ex.ToString());
                            Destroy();
                        }
                    }

                }),
                request);
            }
            catch (Exception ex)
            {
                //Console(ex.ToString());
                Destroy();
            }
        }

        private void HttpAuthorizationRequest()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Address);

                _currentState = State.HTTP_AUTHORIZATION_REQUEST;

                request.Credentials = new NetworkCredential("root", "root");

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                Destroy();

                Console(ex.ToString());
            }
        }

        private static int index = 0;
        public static object locker = new();

        public void HttpsAuthorizationRequest()
        {
            try
            {
                lock (locker)
                {
                    index++;
                    Console(index);
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + Address + "?username=admin&password=admin");

                request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                _currentState = State.HTTPS_AUTHORIZATION_REQUEST;

                request.Credentials = new NetworkCredential("admin", "admin");

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                Destroy();
                Console(ex.ToString());
            }
        }

        private void Exception(Exception ex)
        {
            System.Console.WriteLine(ex.ToString());

            if (ex.ToString().Contains("The SSL connection could not be established, see inner exception"))
            {
                //Console("KDJFKJDF");
            }
            else if (ex.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
            {
            }
            else if (ex.ToString().Contains("Попытка установить соединение была безуспешной"))
            {
            }
            else if (ex.ToString().Contains("Сделана попытка выполнить операцию на сокете при отключенной сети"))
            {
            }
            else
            {
            }
        }
    }
}