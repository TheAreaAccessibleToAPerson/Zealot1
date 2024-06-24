using Butterfly;
using Butterfly.system.objects.main;

namespace Zealot 
{
    public static class Logger 
    {
        public static IInput<IInformation, string> S_I;
        public static IInput<IInformation, string, IEnumerable<string>> S_Is;
        public static IInput<IInformation, string> S_W;
        public static IInput<IInformation, string, IEnumerable<string>> S_Ws;
        public static IInput<IInformation, string> S_E;
        public static IInput<IInformation, string, IEnumerable<string>> S_Es;

        public static IInput<IInformation, string> I;
        public static IInput<IInformation, string, IEnumerable<string>> Is;
        public static IInput<IInformation, string> W;
        public static IInput<IInformation, string, IEnumerable<string>> Ws;
        public static IInput<IInformation, string> E;
        public static IInput<IInformation, string, IEnumerable<string>> Es;

        /// <summary>
        /// Неверное состояние обьекта при получении команды.
        /// this, command, expectState, currentState. 
        /// </summary>
        public static IInput<IInformation, string, string, string> CommandStateException;

        /// <summary>
        /// Неверное состяние обьекта при смене состояния выполнения.
        /// this, , expectState, currentState. 
        /// </summary>
        public static IInput<IInformation, string, string, string> SwapStateException;
    }

    public class WritingText 
    {
        public void SystemInformation(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }
        public void SystemInformations(IInformation info, string message, IEnumerable<string> messages)
        {

        }

        public void SystemWarning(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }

        public void SystemWarnings(IInformation info, string message, IEnumerable<string> messages)
        {
            //string message = ""; int index = 1;
            //foreach(string m in messages)
            //    message += $"\n{index}){m}";
        }

        public void SystemError(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }

        public void SystemErrors(IInformation info, string message, IEnumerable<string> messages)
        {
        }

        public void Information(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }

        public void Informations(IInformation info, string message, IEnumerable<string> messages)
        {
        }

        public void Warning(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }

        public void Warnings(IInformation info, string message, IEnumerable<string> messages)
        {
        }

        public void Error(IInformation info, string message)
        {
            System.Console.WriteLine(
                $"{info.GetExplorer()}/{info.GetKey()}:{message}");
        }
        public void Errors(IInformation info, string message, IEnumerable<string> messages)
        {
        }


        public void StateException(IInformation info, string command, string expectState, 
            string currentState)
        {
            System.Console.WriteLine($"{info.GetExplorer()}/{info.GetKey()}: Команда {command} не будет обработана." +
                    $"(StateException[ExpectState->{expectState} CurrentState->{currentState})");
        }
    }
}