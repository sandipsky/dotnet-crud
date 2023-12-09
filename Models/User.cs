using Microsoft.AspNetCore.Identity;

namespace crud.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}