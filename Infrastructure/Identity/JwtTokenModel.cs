namespace Buttler.Infrastructure.Identity
{
    public class JwtTokenModel
    {
        public string AccessKey { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsTwoFactorAuthentication { get; set; }
        public DateTime DurationInMinutes { get; set; }
    }
}
