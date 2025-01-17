using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcAllinRent.Models;
using MvcAllinRent.Repositories;
using System.Security.Claims;
using MvcAllinRent.Utils;
using MvcAllinRent.Interfaces;

namespace MvcAllinRent.Controllers
{
    public class AuthController : Controller
    {

        private readonly AuthUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AuthController(AuthUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmail(model.Email);
                if(user == null)
                {
                    await _emailService.SendEmailAsync(model.Email, "All-In-Rent: Création compte", "Confirmer votre email");
                    var result = await _userRepository.Save(new AuthUser
                    {
                        Id = 0,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailAddress = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        IdNumber =  model.IdNumber,
                        Password =  model.Password,
                        Status = 2
                    });
                    if (result) {
                        return RedirectToAction("Login");
                    }
                    return View(result);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmail(model.Email);
                if (user != null && _userRepository.VerifyPassword(model.Password, user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.EmailAddress),
                        new Claim(ClaimTypes.Name, user.FirstName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
