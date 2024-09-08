using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager.network
{
    public class Server : Controller
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
                            Logger.I.To(this, "Новый клиент.");

                            TcpClient client = _listener.AcceptTcpClient();

                            i_listenClients.To(client);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.I.To(this, $"Listen client exception.{ex.ToString()}");

                        destroy();

                        return;
                    }
                }
            });
        }

        void Configurate()
        {
            SystemInformation("start configuration");
            {
                string address = Butterfly.Program.ADDRESS;
                int port = Butterfly.Program.PORT;

                _listener = new TcpListener(new IPEndPoint(IPAddress.Parse(address), port));

                SystemInformation($"Bind address:{address}, port:{port}");
            }
            SystemInformation("end configuration");
        }

        void Start()
        {
            SystemInformation($"starting ...");
            {
                try
                {
                    _listener.Start();
                    _isRunning = true;
                }
                catch (Exception ex)
                {
                    SystemInformation(ex.ToString(), ConsoleColor.Red);

                    destroy();

                    return;
                }
            }
            SystemInformation($"star");
        }

        void Destroyed()
        {
            SystemInformation("destroyed");

            _isRunning = false;
        }

        void Stop()
        {
            SystemInformation("start stopping");
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
            SystemInformation("end stopping");
        }
    }
}