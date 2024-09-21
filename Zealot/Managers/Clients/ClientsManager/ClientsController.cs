using System.Net;
using System.Net.Sockets;
using Butterfly;

namespace Zealot.manager
{
    public abstract class ClientsController : ClientsMain
    {

        protected void AddConnectionClient(TcpClient client)
        {
            string key = $"{((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()}:" +
                $"{((IPEndPoint)client.Client.RemoteEndPoint).Port}";

            if (StateInformation.IsStart && !StateInformation.IsDestroy)
            {
                if (try_obj(key, out Client obj))
                {
                    Logger.I.To(this, $"Клиент с ключом {key} уже подключон.");
                }
                else
                {
                    Logger.I.To(this, $"Connection new client:{key}");

                    Clients.IClientConnect newClient = obj<Client>(key, client);

                    Clients.Add(key, newClient);
                }
            }
            else
            {
                Logger.W.To(this, $"Неудалось поключить нового клинта {key}, " +
                    " так как ClientsManager завершает свою работу.");
            }
        }

        protected void RemoveDisconnectionClient(Clients.IClientConnect client)
        {
            if (Clients.Remove(client.GetKey()))
            {
                Logger.I.To(this, $"Client remove from ClientCollection.");
            }
            else
            {
                Logger.S_E.To(this, $"Client not remove from ClientCollection.");

                destroy();

                return;
            }
        }
    }
}