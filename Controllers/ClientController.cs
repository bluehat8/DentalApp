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

namespace DentalApp.Controllers
{
    public class ClientController : Controller
    {
        private UserService userService;


        public ClientController() { 

        }

        public async Task<IActionResult> ClientHomeAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();

            if (string.IsNullOrEmpty(usuarioJson))
            {
                return await signOut();
            }

            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            userService = new UserService(new ApiClient(httpClient));

            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            List<SolicitudCita> solicitudCita = await ServicioSolicitarCita.ObtenerSolicitudesCitasPorUsuario(u.Id);

            // Crear el modelo de vista y asignar valores
            var viewModel = new ClientHomeViewModel
            {
                Usuario = usuarioactual,
                SolicitudesCita = solicitudCita
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
                return RedirectToAction("ClientHome", "Client");


            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                ViewData["ErrorMessage"] = $"Error interno del servidor: {ex.Message}";
                return RedirectToAction("ClientHome", "Client");
            }
        }


    }
}
