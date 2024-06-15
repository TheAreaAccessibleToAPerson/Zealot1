namespace Butterfly
{
    public class Butterfly
    {
        /// <summary>
        /// Запускает систему.
        /// </summary>
        /// <typeparam name="ObjectType">Обьект преставляющий систему. 
        /// Обьект должен быть унаследован от абстрактного класса Controller.
        /// </typeparam>
        public static void fly<ObjectType, InformationType>(Butterfly.Settings settings, InformationType information)
            where ObjectType : system.objects.main.Object, new()
            where InformationType : class
        {
            ((system.objects.root.description.ILife<InformationType>)
                new system.objects.root.Object<ObjectType, InformationType>()).Run(settings, information);
        }
        /// <summary>
        /// Запускает систему.
        /// </summary>
        /// <typeparam name="ObjectType">Обьект преставляющий систему. 
        /// Обьект должен быть унаследован от абстрактного класса Controller.
        /// </typeparam>
        public static void fly<ObjectType>(Butterfly.Settings settings)
            where ObjectType : system.objects.main.Object, new()
        {
            ((system.objects.root.description.ILife<NullInformation>)
                new system.objects.root.Object<ObjectType, NullInformation>()).Run(settings, null);
        }

        public class NullInformation {}

        public class Settings
        {
            /// <summary>
            /// Имя проекта. По умолчанию имя будет пустым.
            /// </summary>
            /// <value></value>
            public string Name { get; init; } = "";

            /// <summary>
            /// Настройки для системного события.
            /// По истечению времени заданому в полe StartingTime, дальнейшую
            /// работу будет обеспечивать системное событие с именем заданым в поле Name.
            /// Вы можете использовать системное событие указав его имя.
            /// </summary>
            /// <value></value>
            public EventSetting SystemEvent { get; init; }
                = new EventSetting("", 0, 1024, false, Thread.Priority.Normal);

            /// <summary>
            /// Настройки для events которые будут использоваться. 
            /// </summary>
            /// <value></value>
            public EventSetting[] EventsSetting = new EventSetting[0];

            /// <summary>
            /// Получает доступ к настройкам событий. 
            /// </summary>
            /// <value></value>
            public EventsController EventsController { get; init; } = null;
        }
    }

    public class EventSetting
    {
        /// <summary>
        /// Имя события. 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// TimeDelay для события.
        /// </summary>
        public uint TimeDelay;

        /// <summary>
        /// Максимальный размер учаснтиков для события. 
        /// </summary>
        public readonly uint Size;

        /// <summary>
        /// Нужно ли уничтожать поток который обрабатывает 
        /// событие если все учаники отпишутся от него.
        /// </summary>
        public bool IsDestroy;

        /// <summary>
        /// Приоритет для потока который будет обрабатывать данное событие. 
        /// </summary>
        public readonly Thread.Priority Priority;


        /// <summary>
        /// Создает настройки для событий которые будут использоваться в проекте. 
        /// </summary>
        /// <param name="name">Имя события.</param>
        /// <param name="timeDelay">TimeDelay для события.</param>
        /// <param name="size">Максимальный размер учасников.</param>
        /// <param name="isDestroy">Нужно ли уничтожить поток обрабатывающий событие если учасников не станет.</param>
        /// <param name="priority">Приоритет для потока.</param>
        public EventSetting(string name, uint timeDelay, uint size = 1024, bool isDestroy = false,
            Thread.Priority priority = Thread.Priority.Normal)
        {
            Name = name;
            TimeDelay = timeDelay;
            Size = size;
            IsDestroy = isDestroy;
            Priority = priority;
        }
    }

    public sealed class EventsController : EventsController.IEventsController
    {
        /// <summary>
        /// Хранит ссылку на action который может сменить timeDelay для события. 
        /// </summary>
        public System.Action<string, uint> ReplaceTimeDelay { private set; get; }

        /// <summary>
        /// Хранит ссылку на action который может запретить/разренить уничтожения потока обрабатывающего
        /// событие в случае отсутсвия учасников. 
        ///</summary>
        public System.Action<string, bool> ReplaceIsDestroy { private set; get; }

        void IEventsController.Set(System.Action<string, uint> replaceTimeDelay, System.Action<string, bool> replaceIsDestroy)
        {
            ReplaceIsDestroy = replaceIsDestroy;
            ReplaceTimeDelay = replaceTimeDelay;
        }

        public interface IEventsController
        {
            void Set(System.Action<string, uint> replaceTimeDelay, System.Action<string, bool> replaceIsDestroy);
        }
    }
}