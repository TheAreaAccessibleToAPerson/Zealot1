namespace Zealot.device
{
    /// <summary>
    /// Данные о машинки передающиеся по Tcp.
    /// </summary> <summary>
    public class OutputDataJson
    {
        /// <summary>
        /// Уникальный номер асика по которому он хранится в нутри компании.
        /// </summary> 
        public string UniqueNumber { set; get; }

        public string Culler1_power { set; get; }
        public string Culler2_power { set; get; }
        public string Culler3_power { set; get; }
        public string Culler4_power { set; get; }

        /// <summary>
        /// Общая можность майнинга.
        /// </summary> 
        public string MiningPowerSize { set; get; }
        /// <summary>
        /// Мощность майнинга на первой плате.
        /// </summary>
        public string MiningPower1Size { set; get; }
        /// <summary>
        /// Мощность майнинга на второй плате.
        /// </summary>
        public string MiningPower2Size { set; get; }
        /// <summary>
        /// Мощность майнинга на третьей плате.
        /// </summary>
        public string MiningPower3Size { set; get; }
        /// <summary
        /// Имя значения майнинга.
        /// </summary>
        public string MiningPowerName { set; get; }
    }
}