namespace Buttler.Infrastructure.Identity
{
    public class JwtHandler
    {
        public string Key { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
