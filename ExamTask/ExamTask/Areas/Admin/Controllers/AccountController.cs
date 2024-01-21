using ExamTask.Helpers;
using ExamTask.Models;
using ExamTask.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamTask.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName=registerVM.Username,
                Email = registerVM.Email,

            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(Index), "Home", new {area=""});

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var exist = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (exist == null)
            {
                exist = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(exist == null)
                {
                    ModelState.AddModelError("", "Username ve ya Passwordunuz sehvdir");
                    return View();
                }
            }
            var SignInCheck=await _signInManager.CheckPasswordSignInAsync(exist, loginVM.Password,true);
            if (!SignInCheck.Succeeded)
            {
                ModelState.AddModelError("", "Username ve ya Passwordunuz sehvdir");
                return View();
            }
            if(SignInCheck.IsLockedOut)
            {
                ModelState.AddModelError("", "Biraz sonra yeniden cehd edin");
                return View();
            }
            await _signInManager.SignInAsync(exist, loginVM.RememberMe);
            return RedirectToAction(nameof(Index), "Home", new { area = "" });

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
               if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString(),
                    });
                }
                    
            }

            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
