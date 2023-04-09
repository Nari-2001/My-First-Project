using Microsoft.AspNetCore.Identity;

namespace FoodApplication.Models.DataContexts
{
    public class UserInApplication:IdentityUser
    {
        public string? Name { get;set; }
        public string? Address { get;set; }
    }
}
