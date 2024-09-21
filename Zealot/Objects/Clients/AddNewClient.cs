namespace Zealot
{
    /// <summary>
    /// Данные клиeнта.
    /// </summary> 
    public sealed class ClientData
    {
        public ClientData(AddNewClient value)
        {
            FullName = value.FullName;
            Login = value.Login;
            Password = value.Password;
            Email = value.Email;
            AccessRight = value.AccessRight;
            OrganizationName = value.OrganizationName;
        }

        /// <summary>
        /// Полное имя клиента.
        /// </summary> 
        public string FullName { init; get; } = "";
        /// <summary>
        /// Логин клиента.
        /// </summary> <summary>
        public string Login { init; get; } = "";
        /// <summary>
        /// Пароль клиента.
        /// </summary>
        public string Password { init; get; } = "";
        /// <summary>
        /// Email клиента. 
        /// </summary>
        /// <value></value>
        public string Email { init; get; } = "";
        /// <summary>
        /// Уровень доступа к функционалу у клинта.
        /// </summary> 
        public string AccessRight { init; get; } = "";
        /// <summary>
        /// Имя организации.
        /// </summary> 
        public string OrganizationName { init; get; } = "";
        /// <summary>
        /// Количесво машин у клиента. 
        /// </summary>
        /// <value></value>
        public int AsicsCount { init; get; } = 0;

        /// <summary>
        /// Разрешон ли запуск.
        /// </summary> 
        public bool IsRunning { set; get; } = false;

        /// <summary>
        /// Дана добавления клинта.
        /// </summary> <summary>
        public string AddClientDate { set; get; } = "";
        /// <summary>
        /// Дата до которой работает клиент. 
        /// </summary>
        /// <value></value>
        public string WorkUntilWantDate { set; get; } = "";
    }

    public sealed class AddNewClient
    {
        public string FullName { set; get; } = "";
        public string Login { set; get; } = "";
        public string Password { set; get; } = "";
        public string Email { set; get; } = "";
        public string AccessRight { set; get; } = "";
        public string OrganizationName { set; get; } = "";
    }

    public sealed class AddNewClientResult
    {
        public bool IsSuccess { set; get; } = false;

        public string OtherResult { set; get; } = "";

        public string FullNameResult { set; get; } = "";
        public string LoginResult { set; get; } = "";
        public string PasswordResult { set; get; } = "";
        public string EmailResult { set; get; } = "";
        public string AccessRightResult { set; get; } = "";
        public string OrganizationNameResult { set; get; } = "";
    }
}