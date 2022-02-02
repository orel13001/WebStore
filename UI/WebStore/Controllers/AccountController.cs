using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;


namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> Logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = Logger;   

        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model, [FromServices] IMapper Mapper)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            _logger.LogInformation("Начало процедуры регистрации пользователя {0}", model.UserName);

            using(_logger.BeginScope("Регистрация {0}", model.UserName))
            {
                var user = Mapper.Map<User>(model);
                var registration_result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(true);
                if (registration_result.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} зарегистрирован", model.UserName);
                    await _userManager.AddToRoleAsync(user, Role.Users).ConfigureAwait(false);
                    
                    _logger.LogInformation("Пользователь {0} наделён ролью {1}", model.UserName, Role.Users);

                    await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(true);

                    _logger.LogInformation("Пользователь {0} временно вошёл в систему после регистрации", model.UserName);

                    return RedirectToAction("Index", "Home");
                }

                var errors = string.Join(", ", registration_result.Errors.Select(e => e.Description));
                _logger.LogWarning("Во время регистрации {0} возникли ошибки: {1}", model.UserName, errors);

                foreach (var error in registration_result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View (model);

        }

        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel() { ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Попытка входа в систему {0}", model.UserName);

            var login_result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                true
                ).ConfigureAwait(true);

            if (login_result.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} вошёл в систему", model.UserName);

                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            _logger.LogWarning("ошибка входа пользователя {0} в систему", model.UserName);

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity!.Name;
            await _signInManager.SignOutAsync().ConfigureAwait(true);
            _logger.LogInformation("Пользователь {0} вышел из системы", user_name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Ошбка доступа к {0}", ControllerContext.HttpContext.Request.Path);
            return View();
        }
    }
}
