namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username;
        public string Password;
        public string Email;
        public int ModuleId;
        public int SettingId;
    }
}
