using Microsoft.AspNetCore.Identity;

namespace dreamStayHotel.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
