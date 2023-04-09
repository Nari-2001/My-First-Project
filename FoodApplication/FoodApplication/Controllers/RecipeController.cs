using FoodApplication.Models;
using FoodApplication.Models.DataContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    public class RecipeController : Controller
    {
        public readonly UserManager<UserInApplication> userManager;
        public readonly FoodDbContext context;
        public RecipeController(UserManager<UserInApplication> usermanager,FoodDbContext context)
        {
            this.userManager = usermanager;
            this.context=context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]  //post request comes from api server,then this method executes,list of recipes comes,so capture those we use collection type
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recipes)   //from API Body we can get data
        {
            //int items=recipes.Count;
            //TempData["lengthOfItems"] = items;
            return PartialView("_GetRecipeCard", recipes);
        }
        public IActionResult SearchItemRecipes(string recipe)  //Note: if we don't get any value then use 'FromQuery' attribute(this attribute is used to read from the URL in querystring)
        {
            ViewBag.title = recipe;
            return View();
        }
        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public IActionResult Order([FromForm] Order orderdetails)
        {
            orderdetails.OrderDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                context.Orders.Add(orderdetails);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Order", "Recipe", new {id=orderdetails.RecipeId});
        }
        public async Task<IActionResult> ShowOrderDetails(OrderRecipeDetails orderdetails)
        {
            Random random = new Random();
            ViewBag.Price=Math.Round(random.Next(150, 500)/5.0)*5;  //Note:simple 'random.Next(150,500)' also get same value

            var user=await userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Id;
            ViewBag.Address = user.Address;
            ViewBag.Id = orderdetails.Id;
            return PartialView("_ShowOrderDetails", orderdetails);
        }


    }
}
