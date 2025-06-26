using BookStoresWebAPI.Models.Identity;
using BookStoresWebAPI.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System.Threading.Tasks;

namespace BookStoresWebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager; //for register
            _signInManager = signInManager; //login
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IsUserNameAvailable(string username) 
        {
            //this function is backend of the ajax 

            var user = await _userManager.FindByNameAsync(username);
            return Json(user == null); // if the name available, then true
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Login form model state is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("Error: {Error}", error.ErrorMessage);
                }
                return View(model);
            }
           
            var user = new AppUser
            {
                UserName = model.UserName, // Username field in User model
                Email = model.Email // Email field in User model
            };

            var result = await _userManager.CreateAsync(user, model.Password); // CreateAsync method from UserManager to create a new user

            if (result.Succeeded)
            {
                Log.Information("User {UserName} registered successfully.", model.UserName); // Log successful registration
                return RedirectToAction("Overview", "Items"); //on succesfull register, redirect to overview
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Log.Error($"Register failed. Error: {error} has occured.");
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError("Register failed: {Error}", error.Description);
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()// Action to display the login page
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // Action to handle login form submission
        {
            Console.WriteLine($"Username: {model.UserName}, Password: {model.Password}");

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Model error in '{key}': {error.ErrorMessage}");
                    }
                }

                _logger.LogWarning("Login failed due to invalid ModelState.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                Log.Information("Login is successfull...");
                return RedirectToAction("Overview", "Items");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                Log.Error("Failed to login.");
                return View(model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Msg"] = "Logouted succesfully!";

            return RedirectToAction("Index", "Home"); //the page we want direct the user to
        }
    }

}
