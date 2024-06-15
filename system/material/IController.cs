namespace Butterfly
{
    public interface IController
    {
        /// <summary>
        /// Начинает процесс уничтожения обьекта.
        /// </summary>
        void destroy();

        /// <summary>
        /// Выводит сообщение в консоль. 
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        void Console<T>(T message) where T : System.IConvertible;

        /// <summary>
        /// Выводит системное сообщение. 
        /// </summary>
        void SystemInformation(string message, System.ConsoleColor color);

        /// <summary>
        /// Создает/Получает Node обьект. 
        /// </summary>
        ObjectType obj<ObjectType>(string key)
            where ObjectType : system.objects.main.Object, new();

        /// <summary>
        /// Пытается получить обьект.
        /// </summary>
        bool try_obj<ObjectType>(string key, out ObjectType value)
            where ObjectType : system.objects.main.Object, new();

        /// <summary>
        /// Проверяет имеется ли обьект с данным ключом.
        /// </summary>
        bool try_obj(string key);

        /// <summary>
        /// Создает/Получает Node обьект. 
        /// </summary>
        ObjectType obj<ObjectType>(string key, object localFields)
            where ObjectType : system.objects.main.Object, new();
    }
}