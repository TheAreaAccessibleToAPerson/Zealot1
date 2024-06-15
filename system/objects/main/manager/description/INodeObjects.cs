namespace Butterfly.system.objects.main.manager
{
    public interface INodeObjects
    {
        /// <summary>
        /// Удалить обьект. 
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Информирует о том что один из обьектов окончил свою сборку. 
        /// Сборка обьекта может прерваться на любом этапе.
        /// </summary>
        void InformingCollected();
    }
}