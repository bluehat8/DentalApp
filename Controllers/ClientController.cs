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
    public class ClientController : Controller
    {
        private UserService userService;
        private ServicioSolicitarCita cita = new ServicioSolicitarCita();
        private ServicioNotificaciones notificacionesServ;
        private TipoCitaServices? tipocitaServ;



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
            notificacionesServ = new ServicioNotificaciones();
            tipocitaServ = new TipoCitaServices();


            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            List<SolicitudCita>? solicitudCita = await ServicioSolicitarCita.ObtenerSolicitudesCitasPorUsuario(u.Id);
            List<SolicitudCita>? citasFilter = solicitudCita.Where(x => x.Estado != (Int32)Constants.DentalSolicitudCitaStatus.cancelada).ToList();
            List<Notificaciones?>? notificaciones = await notificacionesServ.ObtenerNotificacionesPorCliente(usuarioactual.Id);
            List<TipoCita?>? tiposcita = await tipocitaServ.ObtenerTiposCitaAsync();

            int TotalRecords = citasFilter.Count();

            int TotalPages = (int)Math.Ceiling((decimal)TotalRecords / 7);


            // Aplicar paginación
            citasFilter = citasFilter
              .Skip((1 - 1) * 7)
            .Take(7)
              .ToList();

            var viewModel = new ClientHomeViewModel
            {
                Usuario = usuarioactual,
                SolicitudesCita = citasFilter,
                TotalRecords = TotalRecords, 
                TotalPages = TotalPages,
                PageNumber = 1,
                notificaciones = notificaciones,
                tipoCitas = tiposcita
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
                return RedirectToAction("ClientHome", "Client");


            }
            catch (Exception ex)
            {
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
                Usuario? u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                solicitudDto.Userid = u.Id;

                await cita.EnviarSolicitudCita(solicitudDto);

                return RedirectToAction("ClientHome", "Client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("ClientHome", "Client");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ActualizarSolicitudCita(SolicitudCita solicitudDto)
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
                Usuario? u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                solicitudDto.Userid = u.Id;

                await cita.UpdateSolicitudCita(solicitudDto.Id,solicitudDto);

                return RedirectToAction("ClientHome", "Client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("ClientHome", "Client");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelarSolicitudCita(int id)
        {
            try
            {
                await cita.CancelarSolicitudCita(id);

                return RedirectToAction("ClientHome", "Client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("ClientHome", "Client");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelarCita([FromForm] int[] ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    await cita.CancelarSolicitudCita(id);
                }

                return RedirectToAction("ClientHome", "Client");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("ClientHome", "Client");
            }
        }



    }
}
