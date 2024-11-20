using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaimSystem.Models
{
    public class User : IdentityUser
    {
        // Custom properties for additional user information
        public string FullName { get; set; } // User's full name
        public string Role { get; set; }     // User role: Lecturer, Coordinator, Academic Manager
       
    }
}
