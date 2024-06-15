namespace Butterfly.system.objects.main
{
    /// <summary>
    /// Описывает способ подключения к обьекту реализующего данный интерфейс.
    /// реализующего данный интерфейс.
    /// </summary>
    public interface IInputConnect
    {
        /// <summary>
        /// Возращает ссылку на текущий обьект. 
        /// </summary>
        /// <returns></returns>
        public object GetConnect();
    }

    /// <summary>
    /// Описывает способ подлючения к удаленному обьекту.
    /// </summary>
    public interface IInputConnected
    {
        /// <summary>
        /// Получает параметром обьект с которым нужно установить соединение.
        /// </summary>
        /// <param name="inputConnect">Обьект к которму нужно подключиться.</param>
        public void SetConnected(object inputConnect);
    }
}