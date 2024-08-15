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
    }
}