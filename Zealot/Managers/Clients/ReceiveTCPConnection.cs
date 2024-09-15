using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager
{
    public sealed class ReceiveTCPConnection : Controller.Board.LocalField<ReceiveTCPConnection.Setting>
    {
        public bool _isRunning = false;

        private TcpListener _listener;

        private int _port;

        int timer = 0;

        IInput<bool, int> i_returnStartingReceiveClient;
        IInput<bool, TcpClient> i_returnResult;

        void Construction()
        {
            send_message(ref i_returnStartingReceiveClient, Field.StartingReturn);
            send_message(ref i_returnResult, Field.ResultReturn);

            add_event(Header.Events.CLIENT_WORK, 100, () =>
            {
                if (_isRunning)
                {
                    try
                    {
                        while (_listener.Pending())
                        {
                            TcpClient client = _listener.AcceptTcpClient();

                            i_returnResult.To(true, client);

                            destroy();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.W.To(this, $"Listen client exception.{ex.ToString()}");

                        destroy();

                        return;
                    }
                }

                timer += 100;

                if (timer > 5000)
                {
                    i_returnResult.To(false, null);

                    destroy();

                    return;
                }
            });
        }

        void Configurate()
        {
            Logger.I.To(this, "start configuration");
            {
                if (FreePortsStorage.GetFreePort(GetNodeID().ToString(), out _port, out string info))
                {
                    Logger.I.To(this, info);

                    string address = Butterfly.Program.ADDRESS;
                    //_listener = new TcpListener(new IPEndPoint(IPAddress.Parse(address), _port));
                    _listener = new TcpListener(new IPEndPoint(IPAddress.Parse(address), _port));

                    Logger.I.To(this, $"Bind address:{address}, port:{_port}");
                }
                else
                {
                    Logger.S_E.To(this, info);

                    destroy();

                    return;
                }
            }
            Logger.I.To(this, "end configuration");
        }


        void Start()
        {
            Logger.I.To(this, $"starting ...");
            {
                try
                {
                    _listener.Start();
                    _isRunning = true;

                    i_returnStartingReceiveClient.To(true, _port);
                }
                catch (Exception ex)
                {
                    i_returnStartingReceiveClient.To(false, _port);

                    Logger.W.To(this, $"{ex}");

                    destroy();

                    return;
                }
            }
            Logger.I.To(this, $"star");
        }

        void Destroyed()
        {
            _isRunning = false;
        }

        void Stop()
        {
            Logger.I.To(this, "start stopping");
            {
                if (StateInformation.IsCallConfigurate)
                {
                    try
                    {
                        _listener.Stop();
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, ex.Message);
                    }

                    if (FreePortsStorage.SetFreePort(GetKey(), _port, out string info))
                    {
                        Logger.I.To(this, info);
                    }
                    else Logger.S_E.To(this, info);
                }
            }
            Logger.I.To(this, "end stopping");
        }



        public class Setting
        {
            /// <summary>
            /// Сообщенить о том что прослушивание запущено.
            /// </summary> <summary>
            public string StartingReturn { set; get; }
            // Место куда нужно вернуть результат(TCP клинта.)
            public string ResultReturn { set; get; }
        }
    }
}