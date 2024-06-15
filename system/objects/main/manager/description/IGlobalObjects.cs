namespace Butterfly.system.objects.main.manager
{
    /// <summary>
    /// Описывает методы для работы с глобальными обьектами с проверкой на соответсвие:
    /// 1) Времени жизненого цикла.
    /// 2) Отсутвия схожего ключа.
    /// 3) Сохранения идеи DOM.
    /// /// </summary>
    public interface IGlobalObjects
    {
        /// <summary>
        /// Получает глобальный обьект у обьекта к которому приходится потомком.
        /// После чего с помощью описаного способа в IInputConnet получаем ссылку на 
        /// принятие входных данных и передаем ее локальному обьекту с помощью
        /// описаного способа в IInputConnected. 
        /// </summary>
        /// <param name="input">Способ передачи данных в локальный обьект.</param>
        /// <param name="key">Ключ по которому хранится глобальный обьект.</param>
        /// <param name="localObject">Локальный обьект через который мы будем общатся с глобальным обьектом.</param>
        /// <typeparam name="GlobalObjectType">Тип глобального обьекта.</typeparam>
        /// <typeparam name="LocalObjectType">Тип локального обьекта.</typeparam>
        /// <typeparam name="InputType">Тип с помощью которого мы будет передавать данные в локальный обьект.</typeparam>
        /// <typeparam name="RedirectType">Тип который примит ответ из глобального обьекта.</typeparam>
        /// <returns></returns>
        public RedirectType Get<GlobalObjectType, LocalObjectType, InputType, RedirectType>
            (string key, ref InputType input, LocalObjectType localObject)
                where LocalObjectType : InputType, RedirectType, IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect;

        /// <summary>
        /// По ключу получает глобаный обьект реализующий интерфейс IInputConnect в котором описывается
        /// способ получения доступа к нему, после переданый вторым параметром обьект релизующий интерфейс
        /// IInputConneted описывающий способ подключения, подключится к глобальному обьекту.
        /// </summary>
        /// <param name="key">Ключ по которому хранится глобальный обьект.</param>
        /// <param name="localObject">Обьект который необходимо подключить к глобальному обьекту.</param>
        /// <typeparam name="GlobalObjectType">Тип глобального обьекта.</typeparam>
        /// <typeparam name="LocalObjectType">Тип подключаемого обьекта.</typeparam>
        /// <returns></returns>
        public void Get<GlobalObjectType, LocalObjectType>
            (string key, LocalObjectType localObject)
                where LocalObjectType : IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect;

        /// <summary>
        /// Получает глобальный обьект по ключу и устанавливает в него
        /// </summary>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <typeparam name="GlobalObjectType"></typeparam>
        /// <typeparam name="InputType"></typeparam>
        /// <returns></returns>
        public void Get<GlobalObjectType, InputType>
            (string key, ref InputType input)
                where GlobalObjectType : InputType, IInformation;
        
        /// <summary>
        /// Добавляет глобальный обьект и возращает спобоб передачи данных из него.
        /// </summary>
        /// <param name="key">Ключ по которому будет создан глобаный обьект.</param>
        /// <param name="value">Глобальный обьект.</param>
        /// <typeparam name="GlobalObjectType">Тип глобального обьекта.</typeparam>
        /// <typeparam name="RedirectType">Тип перенаправляемых данных.</typeparam>
        /// <returns></returns>
        public RedirectType Add<GlobalObjectType, RedirectType>
            (string key, GlobalObjectType value)
                where GlobalObjectType : RedirectType;

        /// <summary>
        /// Получает глобальный обьект и устанавливает с ним связь.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <param name="localObject"></param>
        /// <typeparam name="GlobalObjectType"></typeparam>
        /// <typeparam name="LocalObjectType"></typeparam>
        /// <typeparam name="InputType"></typeparam>
        /// <returns></returns>
        public void Get<GlobalObjectType, LocalObjectType, InputType>
            (string key, ref InputType input, LocalObjectType localObject)
                where LocalObjectType : InputType, IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect;

        /// <summary>
        /// Получает глобальный обьект.
        /// <param name="key">Ключ глобального обьекта.</param>
        /// <typeparam name="GlobalObjectType">Тип получаемого глобального обьекта.</typeparam>
        /// <returns></returns>
        public GlobalObjectType Get<GlobalObjectType>
            (string key)
                where GlobalObjectType : IInformation;

        /// <summary>
        /// Добавляет глобальный обьек по ключу. 
        /// </summary>
        /// <param name="key">Ключ глобального обьекта.</param>
        /// <param name="value">Глобальный обьект.</param>
        /// <typeparam name="GlobalObjectType">Тип глобального обьекта.</typeparam>
        /// <returns></returns>
        public GlobalObjectType Add<GlobalObjectType>
            (string key, GlobalObjectType value);
    }
}