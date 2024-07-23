using System.Text.Json;
using System.Text.RegularExpressions;
using DnsClient.Protocol;
using Zealot.device.whatsminer;

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

            Regex poolRegex = new Regex(@"class=""cbi-input-text"" value=""[A-Za-z0-9="". -:]*"" data-type");
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

        private static bool I(string str, string reg, string startString, string endString, string name,
            ref string result, ref string info)
        {
            // Elapsed
            Regex regex = new Regex(reg);
            int substringStartLength = startString.Length;
            int substringEndLength = endString.Length;
            MatchCollection elapsedMatches = regex.Matches(str);
            if (elapsedMatches.Count > 0)
            {
                string elapsed = elapsedMatches[0].Value.Substring(substringStartLength);
                elapsed = elapsed.Substring(0, elapsed.Length - substringEndLength).Trim();
                result = elapsed;

                info += $"{name}:[{elapsed}]";
            }
            /*
            else if (elapsedMatches.Count > 1)
            {
                info = $"Неудалось извлеч {name}, проблемы с парсингом.";

                return false;
            }
            */
            else
            {
                info += $"Неудалось получить {name}.";
            }

            return true;
        }


        // Нарежем стриницу на блоки
        private static string[] M(string str)
        {
            string[] result = new string[6];

            // Elapsed
            Regex regexSummaru = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Summary[\w\W]*<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Devices");
            MatchCollection elapsedSummaruMatches = regexSummaru.Matches(str);
            if (elapsedSummaruMatches.Count > 0)
            {
                result[0] = elapsedSummaruMatches[0].Value;
            }

            //Regex regexDevices = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Devices[\w\W]*<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Device</th><th class=""cbi-section-table-cell"">Status");
            Regex regexDevices = new Regex(@"GHSav</th><th class=""cbi-section-table-cell"">GHS5s[\w\W]*Device</th><th class=""cbi-section-table-cell"">Status</th>");
            MatchCollection elapsedDevicesMatches = regexDevices.Matches(str);
            if (elapsedDevicesMatches.Count > 0)
            {
                result[1] = elapsedDevicesMatches[0].Value;
            }

            Regex regexDevice = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Device[\w\W]*<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Pools");
            MatchCollection elapsedDeviceMatches = regexDevice.Matches(str);
            if (elapsedDeviceMatches.Count > 0)
            {
                result[2] = elapsedDeviceMatches[0].Value;
            }

            Regex regexPools = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Pools[\w\W]*<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Errors");
            MatchCollection elapsedPoolsMatches = regexPools.Matches(str);
            if (elapsedPoolsMatches.Count > 0)
            {
                result[3] = elapsedPoolsMatches[0].Value;
            }

            Regex regexErrors = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Errors[\w\W]*<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Events");
            MatchCollection elapsedErrorsMatches = regexErrors.Matches(str);
            if (elapsedErrorsMatches.Count > 0)
            {
                result[4] = elapsedErrorsMatches[0].Value;
            }

            Regex regexEvents = new Regex(@"<fieldset class=""cbi-section"" id=""cbi-table-table"">\n[\s]+<legend>Events[\w\W]*");
            MatchCollection elapsedEventsMatches = regexEvents.Matches(str);
            if (elapsedEventsMatches.Count > 0)
            {
                result[5] = elapsedEventsMatches[0].Value;
            }

            return result;
        }


        // Наименования могут повторяться.
        // имя статус может использоваться два и более раз, поэтому в number нужно передать какой
        // по счету он.
        private static int U(string str, int index, string name, ref string result, ref string info)
        {
            Regex regex = new Regex(@"<input type=""hidden"" id=""cbid.table." + index + "." + name + @""" value=""[A-Za-z0-9 &#; ':+,.-/ \[\] ]*"" \/>\n<\/div>");
            int substringStartLength = $"<input type=\"hidden\" id=\"cbid.table.1.{name}\" value=\"".Length;
            int substringEndLength = "\" />\n</div>".Length;
            MatchCollection elapsedMatches = regex.Matches(str);
            if (elapsedMatches.Count > 0)
            {
                string elapsed = elapsedMatches[0].Value.Substring(substringStartLength);
                elapsed = elapsed.Substring(0, elapsed.Length - substringEndLength).Trim();
                result = elapsed;

                info += $"{name}:[{elapsed}]";
            }
            /*
            else if (elapsedMatches.Count > 1)
            {
                info = $"Неудалось извлеч {name}, проблемы с парсингом.";

                return false;
            }
            */
            else
            {
                Regex regex2 = new Regex(@"<input id=""cbid.table." + index + "." + name + @""" type=""hidden"" value=""[A-Z a-z 0-9 &#;':+,.-/ \[\] ]*"">\n<div>");
                int substringStartLength2 = $"<input type=\"hidden\" id=\"cbid.table.1.{name}\" value=\"".Length;
                int substringEndLength2 = "\">\n<div>".Length;
                MatchCollection elapsedMatches2 = regex2.Matches(str);
                if (elapsedMatches2.Count > 0)
                {
                    string elapsed = elapsedMatches2[0].Value.Substring(substringStartLength2);
                    elapsed = elapsed.Substring(0, elapsed.Length - substringEndLength2).Trim();
                    result = elapsed;

                    info += $"{name}:[{elapsed}]";
                }
                else
                {
                    Regex regex3 = new Regex(@"<input type=""hidden"" id=""cbid.table." + index + "." + name + @""" value=""[A-Z a-z 0-9 &#;':+,.-/ \[\] ]*"">\n<\/div>");
                    int substringStartLength3 = $"<input type=\"hidden\" id=\"cbid.table.1.{name}\" value=\"".Length;
                    int substringEndLength3 = "\">\n</div>".Length;
                    MatchCollection elapsedMatches3 = regex3.Matches(str);
                    if (elapsedMatches3.Count > 0)
                    {
                        string elapsed = elapsedMatches3[0].Value.Substring(substringStartLength3);
                        elapsed = elapsed.Substring(0, elapsed.Length - substringEndLength3).Trim();
                        result = elapsed;

                        info += $"{name}:[{elapsed}]";
                    }
                    else
                    {
                        //info += $"Неудалось получить {name}. ";

                        return 0;
                    }
                }
            }

            return 0;
        }

        public static int ExtractMainPage(string html, out string info, ref AsicStatus w)
        {
            info = "MainPage -";

            string[] t = M(html);

            string buffer = "";

            string extract_buffer()
            {
                string t = buffer;
                buffer = "";
                return t;
            }

            // Elapses
            string str = t[0];
            if (str != null)
            {
                if (U(str, 1, "elapsed", ref buffer, ref info) == 0) { w.Elapsed = extract_buffer(); } else return 1;
                if (U(str, 1, "accepted", ref buffer, ref info) == 0) { w.Accepted = extract_buffer(); } else return 1;
                if (U(str, 1, "rejected", ref buffer, ref info) == 0) { w.Rejected = extract_buffer(); } else return 1;
                if (U(str, 1, "fanspeedin", ref buffer, ref info) == 0) { w.FanSpeedIn = extract_buffer(); } else return 1;
                if (U(str, 1, "fanspeedout", ref buffer, ref info) == 0) { w.FanSpeedOut = extract_buffer(); } else return 1;
                if (U(str, 1, "voltage", ref buffer, ref info) == 0) { w.Voltage = extract_buffer(); } else return 1;
                if (U(str, 1, "power", ref buffer, ref info) == 0) { w.Power = extract_buffer(); } else return 1;
                if (U(str, 1, "workmode", ref buffer, ref info) == 0) { w.PowerMode = extract_buffer(); } else return 1;
            }

            // Devices
            str = t[1];
            if (str != null)
            {
                if (U(str, 1, "name", ref buffer, ref info) == 0) { w.SM0_Name = extract_buffer(); } else return 1;

                // Плпта 1 
                if (U(str, 1, "freqs_avg", ref buffer, ref info) == 0) { w.SM0_Frequency = extract_buffer(); } else return 1;
                if (U(str, 1, "mhs5s", ref buffer, ref info) == 0) { w.SM0_GHS5s = extract_buffer(); } else return 1;

                // Плата 2
                if (U(str, 2, "freqs_avg", ref buffer, ref info) == 0) { w.SM1_Frequency = extract_buffer(); } else return 1;
                if (U(str, 2, "mhs5s", ref buffer, ref info) == 0) { w.SM1_GHS5s = extract_buffer(); } else return 1;

                // Плата 3
                if (U(str, 3, "freqs_avg", ref buffer, ref info) == 0) { w.SM2_Frequency = extract_buffer(); } else return 1;
                if (U(str, 3, "mhs5s", ref buffer, ref info) == 0) { w.SM2_GHS5s = extract_buffer(); } else return 1;

                // Общий результат плат.
                if (U(str, 4, "freqs_avg", ref buffer, ref info) == 0) { w.SM_Frequency = extract_buffer(); } else return 1;
                if (U(str, 4, "mhs5s", ref buffer, ref info) == 0) { w.SM_GHS5s = extract_buffer(); } else return 1;
            }


            // Device
            str = t[2];
            if (str != null)
            {
                // Плата1
                if (U(str, 1, "status", ref buffer, ref info) == 0) { w.SM0_Alive = extract_buffer(); } else return 1;
                if (U(str, 1, "upfreq_complete", ref buffer, ref info) == 0) { w.SM0_UpfreqCompleted = extract_buffer(); } else return 1;
                if (U(str, 1, "effective_chips", ref buffer, ref info) == 0) { w.SM0_EffectiveChips = extract_buffer(); } else return 1;
                if (U(str, 1, "temp", ref buffer, ref info) == 0) { w.SM0_EffectiveChips = extract_buffer(); } else return 1;

                // Плата2
                if (U(str, 2, "status", ref buffer, ref info) == 0) { w.SM1_Alive = extract_buffer(); } else return 1;
                if (U(str, 2, "upfreq_complete", ref buffer, ref info) == 0) { w.SM1_UpfreqCompleted = extract_buffer(); } else return 1;
                if (U(str, 2, "effective_chips", ref buffer, ref info) == 0) { w.SM1_EffectiveChips = extract_buffer(); } else return 1;
                if (U(str, 2, "temp", ref buffer, ref info) == 0) { w.SM1_EffectiveChips = extract_buffer(); } else return 1;

                // Плата3
                if (U(str, 3, "status", ref buffer, ref info) == 0) { w.SM2_Alive = extract_buffer(); } else return 1;
                if (U(str, 3, "upfreq_complete", ref buffer, ref info) == 0) { w.SM2_UpfreqCompleted = extract_buffer(); } else return 1;
                if (U(str, 3, "effective_chips", ref buffer, ref info) == 0) { w.SM2_EffectiveChips = extract_buffer(); } else return 1;
                if (U(str, 3, "temp", ref buffer, ref info) == 0) { w.SM2_EffectiveChips = extract_buffer(); } else return 1;
            }


            // Pools
            str = t[3];
            if (str != null)
            {
                // Пулл 1
                if (U(str, 1, "url", ref buffer, ref info) == 0) { w.Pool1_URL = extract_buffer(); } else return 1;
                if (U(str, 1, "stratumactive", ref buffer, ref info) == 0) { w.Pool1_Active = extract_buffer(); } else return 1;
                if (U(str, 1, "user", ref buffer, ref info) == 0) { w.Pool1_User = extract_buffer(); } else return 1;
                if (U(str, 1, "status", ref buffer, ref info) == 0) { w.Pool1_Status = extract_buffer(); } else return 1;
                if (U(str, 1, "stratumdifficulty", ref buffer, ref info) == 0) { w.Pool1_Difficulty = extract_buffer(); } else return 1;
                if (U(str, 1, "getworks", ref buffer, ref info) == 0) { w.Pool1_GetWorks = extract_buffer(); } else return 1;
                if (U(str, 1, "accepted", ref buffer, ref info) == 0) { w.Pool1_Accepted = extract_buffer(); } else return 1;
                if (U(str, 1, "rejected", ref buffer, ref info) == 0) { w.Pool1_Rejected = extract_buffer(); } else return 1;
                if (U(str, 1, "stale", ref buffer, ref info) == 0) { w.Pool1_Stale = extract_buffer(); } else return 1;
                if (U(str, 1, "lastsharetime", ref buffer, ref info) == 0) { w.Pool1_LST = extract_buffer(); } else return 1;

                // Пулл 2
                if (U(str, 2, "url", ref buffer, ref info) == 0) { w.Pool2_URL = extract_buffer(); } else return 1;
                if (U(str, 2, "stratumactive", ref buffer, ref info) == 0) { w.Pool2_Active = extract_buffer(); } else return 1;
                if (U(str, 2, "user", ref buffer, ref info) == 0) { w.Pool2_User = extract_buffer(); } else return 1;
                if (U(str, 2, "status", ref buffer, ref info) == 0) { w.Pool2_Status = extract_buffer(); } else return 1;
                if (U(str, 2, "stratumdifficulty", ref buffer, ref info) == 0) { w.Pool2_Difficulty = extract_buffer(); } else return 1;
                if (U(str, 2, "getworks", ref buffer, ref info) == 0) { w.Pool2_GetWorks = extract_buffer(); } else return 1;
                if (U(str, 2, "accepted", ref buffer, ref info) == 0) { w.Pool2_Accepted = extract_buffer(); } else return 1;
                if (U(str, 2, "rejected", ref buffer, ref info) == 0) { w.Pool2_Rejected = extract_buffer(); } else return 1;
                if (U(str, 2, "stale", ref buffer, ref info) == 0) { w.Pool2_Stale = extract_buffer(); } else return 1;
                if (U(str, 2, "lastsharetime", ref buffer, ref info) == 0) { w.Pool2_LST = extract_buffer(); } else return 1;

                // Пулл 3
                if (U(str, 3, "url", ref buffer, ref info) == 0) { w.Pool3_URL = extract_buffer(); } else return 1;
                if (U(str, 3, "stratumactive", ref buffer, ref info) == 0) { w.Pool3_Active = extract_buffer(); } else return 1;
                if (U(str, 3, "user", ref buffer, ref info) == 0) { w.Pool3_User = extract_buffer(); } else return 1;
                if (U(str, 3, "status", ref buffer, ref info) == 0) { w.Pool3_Status = extract_buffer(); } else return 1;
                if (U(str, 3, "stratumdifficulty", ref buffer, ref info) == 0) { w.Pool3_Difficulty = extract_buffer(); } else return 1;
                if (U(str, 3, "getworks", ref buffer, ref info) == 0) { w.Pool3_GetWorks = extract_buffer(); } else return 1;
                if (U(str, 3, "accepted", ref buffer, ref info) == 0) { w.Pool3_Accepted = extract_buffer(); } else return 1;
                if (U(str, 3, "rejected", ref buffer, ref info) == 0) { w.Pool3_Rejected = extract_buffer(); } else return 1;
                if (U(str, 3, "stale", ref buffer, ref info) == 0) { w.Pool3_Stale = extract_buffer(); } else return 1;
                if (U(str, 3, "lastsharetime", ref buffer, ref info) == 0) { w.Pool3_LST = extract_buffer(); } else return 1;
            }


            str = t[4];
            if (str != null)
            {
                // ERROR
                // Элементы коорые дублируются могут быть и в единичном виде.
                if (U(str, 1, "code", ref buffer, ref info) == 0) { w.ErrorCode = extract_buffer(); } else return 1;
                if (U(str, 1, "cause", ref buffer, ref info) == 0) { w.ErrorCause = extract_buffer(); } else return 1;
                if (U(str, 1, "time", ref buffer, ref info) == 0) { w.ErrorTime = extract_buffer(); } else return 1;
            }


            str = t[5];
            if (str != null)
            {
                // EVENTS
                // event code
                if (U(str, 1, "id", ref buffer, ref info) == 0) { w.EventCode = extract_buffer(); } else return 1;
                if (U(str, 1, "cause", ref buffer, ref info) == 0) { w.EventCouse = extract_buffer(); } else return 1;
                // pool change
                if (U(str, 1, "action", ref buffer, ref info) == 0) { w.EventAction = extract_buffer(); } else return 1;
                if (U(str, 1, "times", ref buffer, ref info) == 0) { w.EventCount = extract_buffer(); } else return 1;
                if (U(str, 1, "lasttime", ref buffer, ref info) == 0) { w.EventLastTime = extract_buffer(); } else return 1;
                if (U(str, 1, "source", ref buffer, ref info) == 0) { w.EventSource = extract_buffer(); } else return 1;
            }

            return 0;
        }
    }
}
