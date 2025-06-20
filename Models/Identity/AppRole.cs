using Microsoft.AspNetCore.Identity;

namespace BookStoresWebAPI.Models.Identity
{
    public class AppRole: IdentityRole<int>
    {
        public string? RoleDesc { get; set; }
    }
}
