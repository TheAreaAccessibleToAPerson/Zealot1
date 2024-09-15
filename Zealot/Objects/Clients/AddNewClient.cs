namespace Zealot
{
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
        public string AccessRight { set; get; } = "";
        public string OrganizationNameResult { set; get; } = "";
    }
}