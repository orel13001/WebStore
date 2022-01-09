using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;


namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User()
            {
                UserName = model.UserName,
            };

            var registration_result = await _userManager.CreateAsync(user, model.Password);
            if (registration_result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View (model);

        }

        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel() { ReturnUrl = ReturnUrl});

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var login_result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                true
                );

            if (login_result.Succeeded)
            {
                //return RedirectToAction(model.ReturnUrl); // Не безопасно!!!

                //длинная запись
                //if (Url.IsLocalUrl(model.ReturnUrl))
                //{
                //    return Redirect(model.ReturnUrl);
                //}
                //return RedirectToAction("Index", "Home");

                //Короткая запись
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            return View(model);
        }

        public IActionResult Logout() => RedirectToAction("Index","Home");

        public IActionResult AccessDenied() => View();
    }
}
