using Microsoft.AspNetCore.Identity;

namespace TestD.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Name { get; set; }
    }
}
