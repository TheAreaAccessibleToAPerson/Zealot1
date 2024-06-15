namespace Butterfly.system.objects.main.description
{
    /// <summary>
    /// Описывает методы для работы с DOM обьека. 
    /// </summary>
    public interface IDOM
    {
        /// <summary>
        /// Запускает создание узла. 
        /// </summary>
        public void CreatingNode();

        /// <summary>
        /// Инициализируем все необходимые данные DOM для ветки. 
        /// </summary>
        /// <param name="keyObject">Ключ обьекта по которому он был создан в родители.</param>
        /// <param name="nodeID">Уникальный ID узла в котором находится ветка.</param>
        /// <param name="nestingNodeNamberInTheSystem">Номер вложености текущего обьекта в системе.</param>
        /// <param name="nestingNodeNamberInTheNode">Номер вложености ветки в нутри узла.</param>
        /// <param name="parentObjectsID">Массив всех родителей.</param>
        /// <param name="parentObject">Ссылка на родительский обьект.</param>
        /// <param name="nodeObject">Ссылка на Node обьект.</param>
        /// <param name="nearBoardNodeObject">Ссылка на ближайший индивидуальный обьект.</param>
        /// <param name="rootManager">Ссылка на необходимые методы root обьекта.</param>
        /// <param name="globalObjects">Ссылка на все глобальные обьекты.</param>
        public void BranchDefine(string keyObject, ulong nodeID, ulong nestingNodeNamberInTheSystem, ulong nestingNodeNamberInTheNode, 
            ulong[] parentObjectsID, main.Object parentObject, main.Object nodeObject, main.Object nearBoardNodeObject, 
                root.IManager rootManager, System.Collections.Generic.Dictionary<string, object> globalObjects);


        /// <summary>
        /// Инициализируем все необходимые данные DOM для узла. 
        /// </summary>
        /// <param name="keyObject">Ключ обьекта по которому он был создан в родители.</param>
        /// <param name="nestingNodeNamberInTheSystem">Номер вложености текущего обьекта в системе.</param>
        /// <param name="parentObjectsID">Массив ID всех родителей.</param>
        /// <param name="parentObject">Ссылка на родительский обьект.</param>
        /// <param name="nearBoardNodeObject">Ссылка на ближайший индивидуальный обьект.</param>
        /// <param name="rootManager">Ссылка на необходимые методы root обьекта.</param>
        /// <param name="globalObjects">Ссылка на все глобальные обьекты.</param>
        public void NodeDefine(string keyObject, ulong nestingNodeNamberInTheSystem, ulong[] parentObjectsID, 
            main.Object parentObject, main.Object nearBoardNodeObject, root.IManager rootManager, 
                System.Collections.Generic.Dictionary<string, object> globalObjects);

        /// <summary>
        /// Описывает способ получения ссылки на родительский обьект.
        /// </summary>
        /// <returns></returns>
        public main.Object GetParent();

        /// <summary>
        /// Добавляет имя глобального обьекта созданого в дочернем обьекте,
        /// для того что бы переложить ответсвеность за его уничтожение.
        /// </summary>
        /// <param name="name">Имя глобального обьекта.</param>
        public string AddControlToGlobalObject(string name);

        public bool IsBoard();
    }
}