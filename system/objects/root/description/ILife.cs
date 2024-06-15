namespace Butterfly.system.objects.root.description
{
    /// <summary>
    /// Описывает методы для работы с жизнью системы. 
    /// </summary>
    public interface ILife<InformationType>
        where InformationType : class
    {
        /// <summary>
        /// Запускает систему. 
        /// </summary>
        public void Run(Butterfly.Settings settings, InformationType information);
    }
}