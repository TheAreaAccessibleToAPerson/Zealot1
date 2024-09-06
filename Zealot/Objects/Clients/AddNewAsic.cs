
namespace Zealot
{
    public class AddNewAsic
    {
        /// <summary>
        /// Индекс в строке в которой создается 
        /// запрос на добавление новых асиков.
        /// </summary> 
        public string IndexLine { get; set; } = "";
        public string LocationName { get; set; } = "";
        public string LocationNumber { get; set; } = "";
        public string IndexPosition { get; set; } = "";
        public string Company { get; set; } = "";
        public string Model { get; set; } = "";
        public string Hash { get; set; } = "";
        public string Sn { get; set; } = "";
        public string Mac { get; set; } = "";
        public string ClientName { get; set; } = "";
    }

    /// <summary>
    /// Хранит результат добавления новых асиков. 
    /// </summary>
    public class AddNewAsicsResult
    {
        public const string SUCCESS = "Success";
        public const string ERROR = "Error";

        public string IndexLine { get; set; } = "";
        public string LocationName { get; set; } = "";
        public string LocationNameResult { get; set; } = "";
        public string LocationNumber { get; set; } = "";
        public string LocationNumberResult { get; set; } = "";
        public string IndexPosition { get; set; } = "";
        public string IndexPositionResult { get; set; } = "";
        public string Company { get; set; } = "";
        public string CompanyResult { get; set; } = "";
        public string Model { get; set; } = "";
        public string ModelResult { get; set; } = "";
        public string Hash { get; set; } = "";
        public string HashResult { get; set; } = "";
        public string Sn { get; set; } = "";
        public string SnResult { get; set; } = "";
        public string Mac { get; set; } = "";
        public string MacResult { get; set; } = "";
        public string ClientName { get; set; } = "";
        public string ClientNameResult { get; set; } = "";
    }
}