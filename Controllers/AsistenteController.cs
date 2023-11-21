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
using DentalApp.Services.DoctorServices;
using DentalApp.Services.AsistenteServices;

namespace DentalApp.Controllers
{
    public class AsistenteController : Controller
    {
        private UserService userService;
        private AsistenteServices Asistente;

        public AsistenteController()
        {

        }

        public async Task<IActionResult> AsistenteHomeAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();

            if (string.IsNullOrEmpty(usuarioJson))
            {
                return await signOut();
            }

            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            userService = new UserService(new ApiClient(httpClient));
            Asistente = new AsistenteServices(new ApiClient(httpClient));


            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            Asistente? A = await Asistente.ObtenerAsistenteAsync(u.Id);


            // Crear el modelo de vista y asignar valores
            var viewModel = new AsistenteHomeViewModel
            {
                Usuario = usuarioactual,
                _Asistente = A
            };

            // Devolver el modelo de vista a la vista
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
            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();
            userService = new UserService(new ApiClient(httpClient));

            try
            {
                // Llamada a la función de UsuarioService
                var mensaje = await userService.ActualizarUsuarioAsync(u.Id, usuario);

                if (mensaje)
                {
                    ViewData["ErrorMessage"] = $"Error interno del servidor";

                }
                return RedirectToAction("DoctorHome", "Doctor");


            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                ViewData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return RedirectToAction("DoctorHome", "Doctor");
            }
        }



    }
}
