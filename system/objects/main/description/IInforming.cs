namespace Butterfly.system.objects.main.informing
{
    /// <summary>
    /// Описывает методы для вывода необходимой информации через базовый обьект Main. 
    /// </summary>
    public interface IMain 
    {
        public void Console(System.IConvertible message);

        /// <summary>
        /// Вызывает исключение времени сборки обьекта и выводит причину в консоль.
        /// </summary>
        /// <param name="message">Сообщение содержащее описание причины сбоя.</param>
        /// <param name="arg">Необходимые аргументы для того что бы точно понять причину сбоя.</param>
        public System.Exception Exception(string message, params System.IConvertible[] arg);

        /// <summary>
        /// Выводит в консоль системное сообщение.
        /// </summary>
        /// <param name="message">Системное сообщение для вывода в консоль.</param>
        public void SystemInformation(string message, System.ConsoleColor color);
    }
}