namespace Butterfly.system.objects.root.poll
{
    public enum InformingType
    {
        None = 0,

        /// <summary>
        /// Обьект подписан и начал свою работу.
        /// </summary>
        EndSubscribe = 1,

        /// <summary>
        /// Обькт окончил свою работу и был отписан.
        /// </summary>
        EndUnsubscribe = 2,

        /// <summary>
        /// Обьект изменил свою позицию.
        /// </summary>
        ChangeOfIndex = 4
    }
}