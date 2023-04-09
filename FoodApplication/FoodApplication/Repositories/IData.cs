using FoodApplication.Models.DataContexts;
using System.Security.Claims;

namespace FoodApplication.Repositories
{
    public interface IData
    {
        Task<UserInApplication> GetUser(ClaimsPrincipal claims);
    }
}
