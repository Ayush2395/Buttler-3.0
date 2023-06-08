using System.Text.Json.Serialization;

namespace Buttler.Application.DTO
{
    public class UserDetailsDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public int? Age { get; set; }

        [JsonIgnore]
        public string? Gender { get; set; }

        [JsonIgnore]
        public string? UserName { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public DateTime? JoiningDate { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
