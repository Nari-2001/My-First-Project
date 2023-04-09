using FoodApplication.Models.DataContexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodApplication.Repositories
{
    public class Data : IData
    {
        private readonly UserManager<UserInApplication> user;

        public Data(UserManager<UserInApplication> user)
        {
            this.user = user;
        }
        public async Task<UserInApplication> GetUser(ClaimsPrincipal claims)
        {
             return await user.GetUserAsync(claims);
        }
    }
}
