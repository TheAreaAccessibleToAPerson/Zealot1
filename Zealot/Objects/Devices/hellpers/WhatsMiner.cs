using System.Text.Json;
using System.Text.RegularExpressions;
using SharpCompress.Common;

namespace Zealot.device.hellpers
{
    public static class WhatsMiner
    {
        public static int ExtractMACAndUpload(string str, out string error, out string MAC, out long uptime)
        {
            error = "";

            if (str == "")
            {
                error = "В ответ на запрос интерфейса пришло пустое сообщение.";
                MAC = ""; uptime = 0;
                return 1;
            }

            List<WhatsMinerInterface> i =
                JsonSerializer.Deserialize<List<WhatsMinerInterface>>(str);

            if (i == null || i[0] == null)
            {
                error = "Неудалось получить обьект из json";
                MAC = ""; uptime = 0;
                return 1;
            }

            MAC = i[0].macaddr;
            uptime = i[0].uptime;

            return 0;
        }

        public static int ExtractPassword(string str, out string info, out string[] passwords)
        {
            passwords = new string[3];

            Regex passwordRegex = new Regex(@"name=""cbid.pools.default.pool([0-9])pw"" type=""text"" class=""cbi-input-text"" value=""[A-Za-z0-9.]*");
            int substringPasswordStartLength = "name=\"cbid.pools.default.pool1pw\" type=\"text\" class=\"cbi-input-text\" value=\"".Length;
            int substringPasswordEndLength = "".Length;
            MatchCollection passwordMatches = passwordRegex.Matches(str);
            if (passwordMatches.Count > 0)
            {
                if (passwordMatches.Count > 3)
                {
                    info = $"WhatsMiner не может содержать более 3 паролей, было прочитано {passwordMatches.Count}.";

                    return 1;
                }
                else
                {
                    int index = 0;
                    info = $"Извлечены пароли:";
                    foreach (Match match in passwordMatches)
                    {
                        string password = match.Value.Substring(substringPasswordStartLength);
                        password = password.Substring(0, password.Length - substringPasswordEndLength).Trim();
                        info += $"[{index}){password}]";
                        passwords[index++] = password;
                    }
                }
            }
            else
            {
                info = $"На машине установлено 0 паролей.";
            }

            return 0;
        }

        public static int ExtractPowerMode(string str, out string info, out string powerMode)
        {
            info = ""; powerMode = "";

            for (int i = 0; i < 3; i++)
            {
                Regex powerRegex = new Regex(@"name=""cbid.btminer.default.miner_type"" value=""" + i + @""" checked=""checked"" />");
                MatchCollection powerMatches = powerRegex.Matches(str);
                if (powerMatches.Count == 1)
                {
                    if (i == 0)
                    {
                        info = "PowerMode:Low";
                        powerMode = "Low";

                        return 0;
                    }
                    else if (i == 1)
                    {
                        info = "PowerMode:Normal";
                        powerMode = "Normal";

                        return 0;
                    }
                    else if (i == 2)
                    {
                        info = "PowerMode:High";
                        powerMode = "High";

                        return 0;
                    }
                }
            }

            powerMode = "NONE";
            info = "Power mode is not found(parse warning)";

            return 0;
        }

        public static int ExtractWorker(string str, out string info, out string[] workers)
        {
            workers = new string[3];

            Regex workerRegex = new Regex(@"name=""cbid.pools.default.pool([0-9])user"" type=""text"" class=""cbi-input-text"" value=""[A-z0-9.]*");
            int substringWorkerStartLength = "name=\"cbid.pools.default.poolxuser\" type=\"text\" class=\"cbi-input-text\" value=\"".Length;
            int substringWorkerEndLength = "".Length;
            MatchCollection workerMatches = workerRegex.Matches(str);
            if (workerMatches.Count > 0)
            {
                if (workerMatches.Count > 3)
                {
                    info = $"WhatsMiner не может содержать более 3 воркеров, было прочитано {workerMatches.Count}.";

                    return 1;
                }
                else
                {
                    int index = 0;
                    info = $"Извлечены воркеры:";
                    foreach (Match match in workerMatches)
                    {
                        string worker = match.Value.Substring(substringWorkerStartLength);
                        worker = worker.Substring(0, worker.Length - substringWorkerEndLength).Trim();
                        info += $"[{index}){worker}]";
                        workers[index++] = worker;
                    }
                }
            }
            else
            {
                info = $"На машине установлено 0 воркеров.";
            }

            return 0;
        }

        public static int ExtractPool(string str, out string info, out string[] pools)
        {
            pools = new string[3];

            if (str == "")
            {
                info = $"В ответ на запрос конфигурации пришло пустое сообщение.";
                return 1;
            }

            Regex poolRegex = new Regex(@"class=""cbi-input-text"" value=""[A-Za-z1-9="". -:]*"" data-type");
            int substringPoolStartLength = "class=\"cbi-input-text\" value=\"".Length;
            int substringPoolEndLength = "\" data-type".Length;
            MatchCollection poolMatches = poolRegex.Matches(str);
            if (poolMatches.Count > 0)
            {
                if (poolMatches.Count > 3)
                {
                    info = $"WhatsMiner не может содержать более 3 пулов, было прочитано {poolMatches.Count}.";
                    return 1;
                }
                else
                {
                    int index = 0;
                    info = "Извлечены пуллы:";
                    foreach (Match match in poolMatches)
                    {
                        string pool = match.Value.Substring(substringPoolStartLength);
                        pool = pool.Substring(0, pool.Length - substringPoolEndLength).Trim();
                        info += $"[{index}){pool}]";

                        pools[index++] = pool;
                    }
                }
            }
            else
            {
                info = $"На машине установлено 0 пуллов.";
            }

            return 0;
        }

        public static int ExtractMainPage(string str, out string info)
        {
            info = "";

            return 1;
        }
    }
}