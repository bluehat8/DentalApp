using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using DentalApp.Models;
using DentalApp.Services;
using DentalApp.Interfaces;
using Newtonsoft.Json;

namespace DentalApp.Controllers
{
    public class AccountController : Controller
    {

        public AccountController()
        {
           
        }
        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("ClientHome", "Client");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();
            LoginService login = new LoginService(new ApiClient(httpClient));
            var response = await login.Login(u.Username, u.Contraseña);

            try
            { 

                if (response.user != null)
                {
                    List<Claim> c = new()
                    {
                        new Claim(ClaimTypes.NameIdentifier, u.Username)
                    };

                    ClaimsIdentity ci = new ClaimsIdentity(c, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties p = new AuthenticationProperties();
                    p.AllowRefresh = true;
                    p.IsPersistent = u.keepActive;

                    p.ExpiresUtc = u.keepActive == false ? DateTimeOffset.UtcNow.AddMinutes(1) : DateTimeOffset.UtcNow.AddDays(1);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);

                    TempData["Usuario"] = JsonConvert.SerializeObject(response.user);
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(response.user));



                    return RedirectToAction("ClientHome", "Client");
                }
                else
                {
                    ViewBag.Error = response.message;
                }


                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
