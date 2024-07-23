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

        private string _address;

        IInput<string, string> i_send;
        IInput i_request;

        void Construction()
        {
            input_to(ref i_request, Request);
            send_message(ref i_send, Devices.BUS.RECEIVE_SCAN_DEVICES);
        }

        void Start()
        {
            i_request.To();
        }

        void Configurate()
        {
            if (IPAddress.TryParse(Field, out IPAddress a))
            {
                _address = Field;
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
                        destroy();
                    }
                    else
                    {
                        //Logger.S_E.To(this, "1 Неудалось авторизоваться.");
                        destroy();
                    }
                }
                else if (ex.ToString().Contains("The SSL connection could not be established, see inner exception"))
                {
                    HttpsAuthorizationRequest();
                }
                else
                {
                    destroy();
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + _address);
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
                    catch (Exception httpEx)
                    {
                        try
                        {
                            if (httpEx.ToString().Contains("The SSL connection could not be established, see inner exception"))
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + _address + "/cgi-bin/luci" + "?luci_username=admin&luci_password=admin");

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
                                        }
                                    }
                                    catch (Exception httpsAdminException)
                                    {
                                        if (httpsAdminException.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
                                        {
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + _address + "?username=root&password=root");

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
                                                    }
                                                }
                                                catch (Exception eee)
                                                {
                                                    //Console(eee.ToString());
                                                    destroy();
                                                }
                                            }),
                                            request);
                                        } //874
                                        else
                                        {
                                            //Console(httpsAdminException.ToString());
                                            destroy();
                                        }
                                    }
                                }),
                                request);
                            }
                            else if (httpEx.ToString().Contains("The remote server returned an error: (401) Unauthorized"))
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + _address + "/");

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
                                        destroy();
                                    }
                                }),
                                request);

                            }
                            //else Logger.I.To(this, $"{httpEx.ToString()}");
                        }
                        catch(Exception ex)
                        {
                           //Console(ex.ToString());
                            destroy();
                        }
                    }

                }),
                request);
            }
            catch (Exception ex)
            {
                //Console(ex.ToString());
                destroy();
            }
        }

        private void HttpRequest()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + _address);

                _currentState = State.HTTP_REQUEST;

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                destroy();
                Console(ex.ToString());
            }
        }

        private void HttpAuthorizationRequest()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + _address);

                _currentState = State.HTTP_AUTHORIZATION_REQUEST;

                request.Credentials = new NetworkCredential("root", "root");

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                destroy();
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

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + _address + "?username=admin&password=admin");

                request.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                _currentState = State.HTTPS_AUTHORIZATION_REQUEST;

                request.Credentials = new NetworkCredential("admin", "admin");

                request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            }
            catch (Exception ex)
            {
                destroy();
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