using DentalApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DentalApp.Services;
using System.Net.Http;
using DentalApp.Services.ClientServices;
using DentalApp.Models.View;
using DentalApp.Services.User;
using System.Globalization;
using System.Drawing.Printing;
using System.Reflection;
using DentalApp.Services.NotificacionesServices;

namespace DentalApp.Controllers
{
    public class AdminController : Controller
    {
        private UserService userService = new UserService();
        private ServicioNotificaciones? notificacionesServ;

        public AdminController()
        {

        }

        public async Task<IActionResult> AdminHomeAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();

            if (string.IsNullOrEmpty(usuarioJson))
            {
                return await signOut();
            }

            notificacionesServ = new ServicioNotificaciones();
            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
          

            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            List<Notificaciones>? notificaciones = await notificacionesServ.ListarNotificaciones();



            var viewModel = new AdminViewModel
            {
               Usuario = usuarioactual,
               notificaciones= notificaciones
            };

            return View(viewModel);
        }

        public async Task<IActionResult> signOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ActualizarUsuario(Usuario usuario)
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            Usuario? u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();
            userService = new UserService(new ApiClient(httpClient));

            try
            {
                var mensaje = await userService.ActualizarUsuarioAsync(u.Id, usuario);

                if (mensaje)
                {
                    ViewData["ErrorMessage"] = $"Error interno del servidor";

                }
                return RedirectToAction("AdminHome", "Admin");


            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return RedirectToAction("AdminHome", "Admin");
            }
        }

    }
}
