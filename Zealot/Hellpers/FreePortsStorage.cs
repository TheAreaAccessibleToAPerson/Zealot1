using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class FreePortsStorage
{
    private static bool _isInitialize = false;
    private static Dictionary<int, string> _ports = new();
    private static object _locker = new();

    /// <summary>
    /// Получить свободный порт.
    /// </summary>
    /// <param name="name">Имя получателя.</param>
    /// <param name="port">Номер порта.</param>
    /// <param name="info">Дополнительная информация.</param>
    /// <returns></returns>
    public static bool GetFreePort(string name, out int port, out string info)
    {
        info = "";

        lock (_locker)
        {
            Initialize();

            foreach (var pair in _ports)
            {
                IPGlobalProperties igp = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tinfo = igp.GetActiveTcpConnections();
                foreach (TcpConnectionInformation tcpi in tinfo)
                {
                    if (tcpi.LocalEndPoint.Port == pair.Key)
                    {
                        continue;
                    }
                    else
                    {
                        if (pair.Value == "")
                        {

                            port = pair.Key;

                            info = $"{name} получил порт {port} на временое пользование.";

                            _ports[port] = name;

                            return true;
                        }
                    }
                }

            }

            info = $"{name} не смог получить порт, так как все порты уже заняты.";
            port = -1;

            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Имя того кто возращает.</param>
    /// <param name="port">Номер порта.</param>
    /// <param name="info">Дополнительная информация.</param>
    /// <returns></returns> <summary>
    public static bool SetFreePort(string name, int port, out string info)
    {
        info = "";

        lock (_locker)
        {
            Initialize();

            if (_ports.ContainsKey(port))
            {
                if (_ports[port] == name)
                {
                    info = $"{name} освободил порт {port}. Теперь данный порт свободен.";

                    _ports[port] = "";

                    return true;
                }
                else
                {
                    info = $"{name} попытался освободить порт {port}, но данный порт выделялся для [{_ports[port]}].";

                    return false;
                }
            }
            else
            {
                info = $"Клиент {name} попытался освободить порт {port}, но такой порт не выделялся.";
                return false;
            }
        }
    }

    private static void Initialize()
    {
        if (_isInitialize == false)
        {
            for (int i = 0; i < 1000; i++)
            {
                _ports.Add(35000 + i, "");
            }

            _isInitialize = true;
        }
    }
}