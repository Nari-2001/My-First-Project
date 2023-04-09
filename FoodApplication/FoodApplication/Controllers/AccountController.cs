using FoodApplication.Models;
using FoodApplication.Models.DataContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public readonly UserManager<UserInApplication> userManager;
        public readonly SignInManager<UserInApplication> signInManager;
        public AccountController(UserManager<UserInApplication> usermanager, SignInManager<UserInApplication> signinmanager)
        {
            this.userManager = usermanager;
            this.signInManager = signinmanager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginDetails,string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result=await signInManager.PasswordSignInAsync(loginDetails.Email, loginDetails.Password,false,false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name & Password");
                }
            }
            return View(loginDetails);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerDetails)
        {
            if (ModelState.IsValid)
            {
                var user = new UserInApplication
                {
                    Name = registerDetails.Name,
                    Email = registerDetails.Email,
                    Address = registerDetails.Address,
                    UserName=registerDetails.Email
                };
                IdentityResult? result=await userManager.CreateAsync(user, registerDetails.Password);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerDetails);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
