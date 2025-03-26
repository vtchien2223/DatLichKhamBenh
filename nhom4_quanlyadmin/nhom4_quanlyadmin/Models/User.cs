using Microsoft.AspNetCore.Identity;
namespace nhom4_quanlyadmin.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
