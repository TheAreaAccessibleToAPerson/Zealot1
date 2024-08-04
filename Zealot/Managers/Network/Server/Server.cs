using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager.network
{
    public class Server : Controller.Board
    {
        public const string NAME = "Server[Listen new clients]";

        public bool _isRunning = false;

        private TcpListener _listener;

        private IInput<TcpClient> i_listenClients;

        void Construction()
        {
            send_message(ref i_listenClients, Clients.BUS.ADD_CLIENT);

            add_event(Header.Events.CLIENT_WORK, () =>
            {
                if (_isRunning)
                {
                    try
                    {
                        while (_listener.Pending())
                        {
                            TcpClient client = _listener.AcceptTcpClient();

                            i_listenClients.To(client);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.W.To(this, $"Listen client exception.{ex.ToString()}");

                        destroy();
                    }
                }
            });
        }

        void Configurate()
        {
            Logger.I.To(this, "start configuration");
            {
                string address = Butterfly.Program.ADDRESS;
                int port = Butterfly.Program.PORT;

                _listener = new TcpListener(new IPEndPoint(IPAddress.Parse(address), port));

                Logger.I.To(this, $"Bind address:{address}, port:{port}");
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
                }
                catch (Exception ex)
                {
                    Logger.W.To(this, $"{ex}");

                    destroy();
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
                if (StateInformation.IsConfigurate)
                {

                    try
                    {
                        _listener.Stop();
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, ex.Message);
                    }
                }
            }
            Logger.I.To(this, "end stopping");
        }
    }
}