using Microsoft.AspNet.Identity.EntityFramework;

namespace CAS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class AppUser : IdentityUser
    {        
        public int? IsActive { get; set; }
    }    
}