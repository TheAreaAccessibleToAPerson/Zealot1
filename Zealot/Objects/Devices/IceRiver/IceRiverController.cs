using System.Net;
using System.Text;
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
        }

        private readonly JavascriptCallback _javascriptCallback = new JavascriptCallback();

        /*

        public Task<T> EvaluateScriptWithCallback<T>(string script, bool conditionalAjax = false)
        {
            // используется для ожидания вызова SetResult (класса JavascriptCallback) из JavaScript
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            Action onResultCallback = null;

            onResultCallback = () =>
            {
                _javascriptCallback.OnResult -= onResultCallback;
                tcs.SetResult(ConvertHelper.ToTypedVariable<T>(_javascriptCallback.Result));
            };

            _javascriptCallback.OnResult += onResultCallback;

            T scriptResult = EvaluateJavascript<T>(script).Result;

            if (conditionalAjax && scriptResult != null)
            {
                tcs.SetResult(scriptResult);
            }

            return tcs.Task;
        }
        */


        protected string CurrentState { set; get; } = State.NONE;

        protected IceRiverStatus Status = new();

        private CookieContainer _cookeis = new();

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
                if (StateInformation.IsDestroy || StateInformation.IsStart == false) return;
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

                                    //Result(str);
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
            }
        }

        public void Destroy(string destroyInfo)
        {
            throw new NotImplementedException();
        }

        public string GetAddress()
        {
            throw new NotImplementedException();
        }

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
    }

    public class IceRiverStatus
    {
        public string Address { set; get; }
    }
}