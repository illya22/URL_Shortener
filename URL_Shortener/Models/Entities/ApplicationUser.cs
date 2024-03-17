using Microsoft.AspNetCore.Identity;

namespace URL_Shortener.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Link> Links { get; set; }
    }
}
