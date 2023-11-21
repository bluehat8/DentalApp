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

namespace DentalApp.Controllers
{
    public class ClientController : Controller
    {
        private UserService userService;
        private ServicioSolicitarCita cita = new ServicioSolicitarCita();



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
                SolicitudesCita = solicitudCita,
               // SolicitudCita = new SolicitudCita()
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

        [HttpPost]
        public async Task<IActionResult> SolicitarCita(SolicitudCita solicitudDto)
        {
            try
            {
                if (TimeSpan.TryParseExact(solicitudDto.Fecha.ToString(), "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan horaTimeSpan))
                {
                    solicitudDto.Hora = horaTimeSpan.ToString();
                }
                else
                {
                    solicitudDto.Hora = TimeSpan.MinValue.ToString();
                }

                DateTime fecha = DateTime.ParseExact(
                  solicitudDto.Fecha.ToString(),
                  "dd/MM/yyyy HH:mm:ss",
                  CultureInfo.InvariantCulture); 

                TimeSpan hora = fecha.TimeOfDay;
                solicitudDto.Hora = hora.ToString(@"hh\:mm");


                var usuarioJson = HttpContext.Session.GetString("Usuario");
                Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                solicitudDto.Userid = u.Id;

                await cita.EnviarSolicitudCita(solicitudDto);

                // Puedes redirigir a una página de éxito o hacer cualquier otra cosa según tus necesidades
                return RedirectToAction("ClientHome", "Client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Manejar el error de alguna manera, posiblemente mostrar un mensaje de error en la vista
                return RedirectToAction("ClientHome", "Client");
            }
        }


    }
}
