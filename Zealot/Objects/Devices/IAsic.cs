namespace Zealot
{
    public interface IAsic 
    {
        /// <summary>
        /// Модель. 
        /// </summary>
        /// <returns></returns>
        public string GetModel();

        /// <summary>
        /// Серийный номер.
        /// </summary>
        /// <returns></returns>
        public string GetSN();
    }
}