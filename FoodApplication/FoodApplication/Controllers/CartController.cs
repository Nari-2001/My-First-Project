using FoodApplication.Models;
using FoodApplication.Models.DataContexts;
using FoodApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{

    public class CartController : Controller
    {
        private readonly IData data;
        private readonly FoodDbContext context;

        //constructor for accessing context class AND repository class
        public CartController(IData data, FoodDbContext context)
        {
            this.data = data;
            this.context = context;
        }
        
        //display cart items in view
        public async Task<IActionResult> Index()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartitems = context.Carts.Where(c => c.UserId == user.Id).ToList();
            return View(cartitems);
        }

        
        [HttpPost]
        public IActionResult SaveCart(Cart cartdetails)
        {
            var user = data.GetUser(HttpContext.User);
            cartdetails.UserId = user.Result.Id;
            if (ModelState.IsValid)
            {
                context.Carts.Add(cartdetails);
                context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        
        public IActionResult GetCartItems()
        {
            var user = data.GetUser(HttpContext.User);
            List<string> cartitems = context.Carts.Where(c => c.UserId == user.Result.Id).Select(c => c.RecipeId).ToList();
            return Ok(cartitems);
        }
    
        public async Task<IActionResult> GetCartDetails()
        {
            var user=await data.GetUser(HttpContext.User);
            var cart3items=context.Carts.Where(c => c.UserId == user.Id).Take(3).ToList();
            return PartialView("_CartList", cart3items);
        }


        [HttpPost]
        public IActionResult RemoveCartItem(string Id)
        {
            if (Id != null)
            {
                var cartitem = context.Carts.Where(c => c.RecipeId == Id).FirstOrDefault();
                if (cartitem != null)
                {
                    context.Carts.Remove(cartitem);
                    context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }

    }
}
