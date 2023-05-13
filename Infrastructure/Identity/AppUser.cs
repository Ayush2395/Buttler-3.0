using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Buttler.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string? LastName { get; set; }
        public int? Age { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(10)]
        public string? Gender { get; set; }


        [Column(TypeName = "nvarchar")]
        public string? ProfilePic { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
