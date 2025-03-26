using Microsoft.AspNetCore.Identity;
namespace Nhom4_QLBA_API.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
