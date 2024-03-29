﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajTakip.Business.Abstract;
using StajTakip.Entities.DTOs;
using System.Security.Claims;
using StajTakip.MVC.Models;

namespace StajTakip.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IAdminUserService _adminUserService;

        public AuthController(IAuthService authService, IAdminUserService adminUserService, IUserService userService)
        {
            _authService = authService;
            _adminUserService = adminUserService;
            _userService = userService;
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
                {
                    ModelState.AddModelError("", user.Message ?? "Bir hata ile karşılaşıldı!");
                    return View();
                }
                    

                var operationClaims = _authService.GetClaims(user.Data.Id);
                foreach (var item in operationClaims.Data)
                {
                    if (item.Name.StartsWith("student"))
                    {
                        return Redirect("/Auth/Login");
                    }
                }
                var userId = _adminUserService.GetByUserId(user.Data.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userId.Data.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Data.Email),
                    new Claim("userId",  user.Data.Id.ToString())
                };
                foreach (var role in operationClaims.Data)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var userIdentity = new ClaimsIdentity(claims, "a");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Inbox", "Message");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AdminUserForRegisterDto model)
        {
            if (ModelState.IsValid)
            {
                model.IsCompany = false;
                var result = _authService.RegisterAdmin(model);
                if (result.Success)
                    return RedirectToAction("Login");

                ModelState.AddModelError("", result.Message ?? "Bir hata ile karşılaşıldı!");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="admin, admin.teacher")]
        public IActionResult RegisterCompany(int? studentId)
        {
            if (studentId.HasValue)
            {
                ViewBag.StudentId = studentId;
            }
            ViewBag.StudentId = 0;
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="admin, admin.teacher")]
        public IActionResult RegisterCompany(AdminUserForRegisterDto model)
        {
            if (ModelState.IsValid)
            {
                model.IsCompany = true;
                var result = _authService.RegisterAdmin(model);
                if (result.Success)
                    return RedirectToAction("StudentList","Student");

                ModelState.AddModelError("", result.Message ?? "Bir hata ile karşılaşıldı!");
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles ="admin, admin.teacher, admin.company")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, admin.teacher, admin.company")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.ChangePassword(model);
                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message ?? "Hata! Lütfen alanları kontrol ediniz!");
                    return View();
                }
                await Logout();
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (ModelState.IsValid) 
            {
                var result = _userService.ForgotPassword(model);
                if(!result.Success)
                    ModelState.AddModelError("", result.Message ?? "Lütfen daha sonra yeniden deneyiniz!");

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
