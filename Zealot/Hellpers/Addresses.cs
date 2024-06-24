namespace Zealot.hellper
{
    public static class Address
    {
        public class Values
        {
            public string[] Addresses { set; get; }

            public DiopozoneOfAddresses[] DiopozonesOfAddresses { set; get; }
        }

        public class DiopozoneOfAddresses
        {
            public string Start { set; get; }
            public string End { set; get; }

            public DiopozoneOfAddresses(string start, string end)
            {
                Start = start;
                End = end;
            }
        }

        public static bool ConvertDioposoneAddresses(string value, out string[] result, 
            out string error)
        {
            error = "";
            {
                result = value.Split("-");

                if (result.Length == 2) return true;

                error = $"Неверный формат диопозона адрресов [{value}], ожидается что значение должно иметь " + 
                    $"вот такой вид [127.0.0.1-127.0.0.255]";

            }
            return false;
        }

        public static List<string> GetAddresses(Values settings)
        {
            List<string> result = new List<string>();
            {
                if (settings.Addresses != null)
                {
                    for (int i = 0; i < settings.Addresses.Length; i++)
                        result.Add(settings.Addresses[i]);
                }

                if (settings.DiopozonesOfAddresses != null)
                {
                    for (int i = 0; i < settings.DiopozonesOfAddresses.Length; i++)
                    {
                        string[] start = settings.DiopozonesOfAddresses[i].Start.Split(".");
                        string[] end = settings.DiopozonesOfAddresses[i].End.Split(".");

                        int start1octet = Convert.ToInt32(start[0]); int start2octet = Convert.ToInt32(start[1]);
                        int start3octet = Convert.ToInt32(start[2]); int start4octet = Convert.ToInt32(start[3]);

                        int end1octet = Convert.ToInt32(end[0]); int end2octet = Convert.ToInt32(end[1]);
                        int end3octet = Convert.ToInt32(end[2]); int end4octet = Convert.ToInt32(end[3]);

                        //System.Console.WriteLine($"Start octet:{start1octet}.{start2octet}.{start3octet}.{start4octet}");
                        //System.Console.WriteLine($"End octet:{end1octet}.{end2octet}.{end3octet}.{end4octet}");

                        while (true)
                        {
                            if (start1octet <= end1octet)
                            {
                                if (start2octet <= end2octet)
                                {
                                    if (start3octet <= end3octet)
                                    {
                                        //System.Console.WriteLine($"Current octet:{start1octet}.{start2octet}.{start3octet}.{start4octet}");

                                        result.Add($"{start1octet}.{start2octet}.{start3octet}.{start4octet}");

                                        if (start1octet == end1octet && start2octet == end2octet && start3octet == end3octet && start4octet == end4octet)
                                            break;

                                        if ((++start4octet) == 256)
                                        {
                                            start4octet = 0;
                                            start3octet += 1;
                                        }
                                    }
                                }
                            }
                            else break;
                        }
                    }
                }
            }
            return result;
        }
    }
}