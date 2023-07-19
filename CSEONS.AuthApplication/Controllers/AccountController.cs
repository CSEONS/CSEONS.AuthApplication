using CSEONS.AuthApplication.Domain;
using CSEONS.AuthApplication.Extensions;
using CSEONS.AuthApplication.Models;
using CSEONS.AuthApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSEONS.AuthApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly GroupManager _groupManager;
        private readonly DataManager _dataManager;

        public AccountController(GroupManager groupManager, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DataManager dataManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _groupManager = groupManager;
            _dataManager = dataManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _dataManager.DefaultContext.Users.FirstOrDefault(u => u.Login == model.Login);

                if (user != null)
                {

                    var signinResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                    if (!signinResult.Succeeded)
                    {
                        ModelState.AddModelError(nameof(LoginViewModel.Login), "Неверный логин или пароль");
                        return View(model);
                    }
                }

                return Redirect(returnUrl ?? "/");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = nameof(ApplicationUser.Roles.Admin))]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [Authorize(Roles = nameof(ApplicationUser.Roles.Admin))]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? newUser = await _userManager.FindByNameAsync(model.FirstName);

                if (newUser is not null)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.FirstName), "Пользователь уже существует");
                    return View(model);
                }

                newUser = new ApplicationUser()
                {
                    Login = model.Login,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SecondName = model.SecondName,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(newUser, model.Password)
                };

                newUser.UserName = newUser.GetUserName();

                var createUserResult = await _userManager.CreateAsync(newUser);

                if (createUserResult.Succeeded)
                {
                    newUser = await _userManager.FindByNameAsync(newUser.GetUserName());
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(nameof(IdentityResult), error.Description);
                    }
                }

                if (await _groupManager.ExistAsync(model.Group))
                {
                    await _groupManager.AddToGroupAsync(newUser, model.Group);
                }
                else
                {
                    await _groupManager.CreateGroupAsync(model.Group);
                    await _groupManager.SaveAsync();
                    await _groupManager.AddToGroupAsync(newUser, model.Group);
                }

                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _userManager.AddToRoleAsync(newUser, model.Role);
                }
                else
                {
                    await _roleManager.CreateRoleAsync(model.Role);
                    await _userManager.AddToRoleAsync(newUser, model.Role);
                }

                ModelState.AddModelError("UserCreated", "Пользователь успешно создан");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return RedirectToAction(nameof(HomeController.Profile), nameof(HomeController).CutController());
        }
    }
}
