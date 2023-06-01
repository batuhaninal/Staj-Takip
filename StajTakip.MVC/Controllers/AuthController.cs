using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.DTOs;
using System.Security.Claims;

namespace StajTakip.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IStudentUserService _studentUserService;

        public AuthController(IAuthService authService, IStudentUserService studentUserService)
        {
            _authService = authService;
            _studentUserService = studentUserService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Login(model);
                if (!user.Success)
                    return View();

                var operationClaims = _authService.GetClaims(user.Data.Id);
                var studentId = _studentUserService.GetByUserId(user.Data.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, studentId.Data.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Data.Email),

                    new Claim("userId", user.Data.Id.ToString())

                };
                foreach (var role in operationClaims.Data)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var userIdentity = new ClaimsIdentity(claims, "a");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "InternshipsBook");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(StudentUserForRegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var result = _authService.RegisterStudent(model);
                if (result.Success)
                    return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
