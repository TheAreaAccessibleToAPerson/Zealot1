namespace Butterfly.system.objects.main
{
    public interface IInformation
    {
        /// <summary>
        /// Хранит адрес обьекта в системе.
        /// </summary>
        /// <returns></returns>
        public string GetExplorer();

        /// <summary>
        /// Хранит ID обьекта в нутри которого был создан.
        /// </summary>
        /// <returns></returns>
        public ulong GetID();

        /// <summary>
        /// Хранит ID узла в нутри которого был создан. 
        /// </summary>
        /// <returns></returns>
        public ulong GetNodeID();

        /// <summary>
        /// Возращает ключ по которому был создан обьект. 
        /// </summary>
        /// <returns></returns>
        public string GetKey();

        /// <summary>
        /// Препренимает попытку инкрементировать количесво выполняемых событий.
        /// Если вернулось значение false значит обьект находится на стадии уничтожения.
        /// Обьект не начнет уничтожаться пока инкрементируемое значение с помощью
        /// метода void DecrementEvent() не вернется на отметку 0.
        /// </summary>
        /// <returns></returns>
        public bool TryIncrementEvent();

        /// <summary>
        /// Декриментирует количесво выполняемых событий в обработчике событий. 
        /// </summary>
        /// <returns></returns>
        public void DecrementEvent();

        /// <summary>
        /// Получает доступ к менеджеру глобальных обьектов. 
        /// </summary>
        /// <returns></returns>
        public manager.IGlobalObjects GlobalObjectsManager();
    }
}