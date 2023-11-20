using DentalApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentalApp.Controllers
{
    public class ClientController : Controller
    {

        public async Task<IActionResult> ClientHomeAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuarioJson))
            {
                return await signOut();
            }

            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            return View(u);
        }

        public async Task<IActionResult> signOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
