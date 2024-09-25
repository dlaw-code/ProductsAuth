using Microsoft.AspNetCore.Identity;

namespace Assignment.API.Entity
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    
    }
}
