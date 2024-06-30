using System.Collections.Generic;

namespace Butterfly
{
    public static class ReadLine
    {
        /// <summary>
        /// Описывает необходимую информацию в обьекте для работы с ReadLine. 
        /// </summary>
        public interface IInformation
        {
            /// <summary>
            /// Метод который реализуем выполнение комманд. 
            /// </summary>
            /// <param name="command"></param>
            void Command(string command);

            /// <summary>
            /// Метод через который можно получить ключ/имя обьекта.
            /// </summary>
            /// <returns></returns>
            string GetKey();
        }

        private static System.Threading.CancellationTokenSource s_cancelTokenSource
            = new System.Threading.CancellationTokenSource();

        private static System.Threading.CancellationToken s_token;

        private static Dictionary<string, System.Action<string>> s_values = new Dictionary<string, System.Action<string>>();
        private static object s_locker = new object();

        private static async void Work(string objName = "", string text = "")
        {
            lock (s_locker)
                if (s_values.Count == 0) return;

            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    System.Threading.Thread.Sleep(200);

                    string name = "";
                    if (objName == "")
                    {
                        System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                        System.Console.Write("Enter:");
                        System.Console.ForegroundColor = System.ConsoleColor.White;
                        name = System.Console.ReadLine();
                    }
                    else name = objName;

                    if (name == "-all")
                    {
                        string str = "Objects:\n";
                        lock (s_locker)
                        {
                            int i = 0;
                            foreach (string nameInfo in s_values.Keys)
                                str += $"{i++}){nameInfo}\n";
                        }

                        System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                        System.Console.WriteLine(str);
                        System.Console.ForegroundColor = System.ConsoleColor.White;

                        Work();
                        return;
                    }

                    System.Action<string> action;

                    lock (s_locker)
                    {
                        if (s_values.TryGetValue(name, out action))
                        {
                            //
                        }
                        else
                        {
                            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                            System.Console.WriteLine($"Обьекта использующего ReadLine с именем {name} не сущесвует.");
                            System.Console.ForegroundColor = System.ConsoleColor.White;

                            if (s_values.Count > 0)
                                Work();

                            return;
                        }
                    }

                    System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                    string u = "Command:";
                    if (text != "") u = text;
                    System.Console.Write(u);
                    System.Console.ForegroundColor = System.ConsoleColor.White;

                    string command = System.Console.ReadLine();

                    lock (s_locker)
                    {
                        if (s_values.TryGetValue(name, out action))
                        {
                            action.Invoke(command);

                            _isInput = false;
                        }
                        else
                        {
                            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                            System.Console.WriteLine($"Комманда {command} не была выполнена так " +
                                $"как обьект {name} прекратил свою работу.");
                            System.Console.ForegroundColor = System.ConsoleColor.White;
                        }
                    }
                },
                s_token);
            }
            catch (System.Exception ex)
            {
            }
        }

        public static bool _isInput = false;

        public static void Input(string objName = "", string text = "")
        {
            lock (s_locker)
            {
                if (s_values.Count > 0)
                Work(objName, text);
            }
        }

        /// <summary>
        /// Запускает прослушивание ввода в консоль. 
        /// </summary>
        public async static void Start(ReadLine.IInformation information)
        {
            if (s_token == null) s_token = s_cancelTokenSource.Token;

            lock (s_locker)
            {
                if (s_values.ContainsKey(information.GetKey()))
                {
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    System.Console.WriteLine($"Вы уже используете ReadLine для обьекта с ключом/именем {information.GetKey()}");
                    System.Console.ForegroundColor = System.ConsoleColor.White;

                    return;
                }
                else
                {
                    s_values.Add(information.GetKey(), information.Command);

                    if (s_values.Count == 1)
                        Work();
                }
            }
        }

        /// <summary>
        /// Останавливает прослушивания ввода в консоль. 
        /// </summary>
        public static void Stop(ReadLine.IInformation information)
        {
            lock (s_locker)
            {
                if (s_values.Remove(information.GetKey()))
                {
                    if (s_values.Count == 0)
                        if (s_token != null)
                            s_cancelTokenSource.Cancel();
                }
            }
        }
    }
}