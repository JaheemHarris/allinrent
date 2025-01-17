using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAllinRent.Dto;
using System.Security.Claims;

namespace RazorAllinRent.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly RazorAllinRent.Database.DatabaseContext _context;

        public LoginModel(RazorAllinRent.Database.DatabaseContext context)
        {
            _context = context;
        }

        public ActionResult OnGet()
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Items/Index");
            }
            return Page();
        }

        [BindProperty]
        public LoginFormDto LoginFormData { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync() {
            if(ModelState.IsValid)
            {
                var user = await _context.AuthUsers.FirstOrDefaultAsync(users => users.EmailAddress == LoginFormData.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(LoginFormData.Password, user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.EmailAddress),
                        new Claim(ClaimTypes.Email, user.EmailAddress),
                        new Claim(ClaimTypes.Name, user.FirstName),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToPage("/Items/Index");
                }
            }
            return Page();
        }
    }
}
