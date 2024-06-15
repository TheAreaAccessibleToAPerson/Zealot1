namespace Butterfly.system.objects.root.poll
{
    public class Fields
    {
        /// <summary>
        /// ID пулла реализующего работу потока. 
        /// </summary>
        /// <value></value>
        public ulong ID { get; init; }

        /// <summary>
        /// Имя пулла реализующего работу потока. 
        /// </summary>
        /// <value></value>
        public string Name { get; init; }

        /// <summary>
        /// Максимальный размер для пулла. 
        /// </summary>
        /// <value></value>
        public uint Size{ get;init; }

        /// <summary>
        /// Sleep для потока.
        /// </summary>
        /// <value></value>
        public uint TimeDelay { get; init; }

        /// <summary>
        /// Приоритет потока.
        /// </summary>
        /// <value></value>
        public Thread.Priority ThreadPriority { get; init; }

        /// <summary>
        /// Нужно ли удалять пулл если все клинты отпишутся от него. 
        /// </summary>
        /// <value></value>
        public bool IsDestroy { get; init; }

        /// <summary>
        /// Метод предназначеный для удаления из пулла потоков. 
        /// </summary>
        /// <value></value>
        public System.Func<Poll, bool> Destroy { get; init; }
    }
}