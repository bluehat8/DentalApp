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

namespace DentalApp.Controllers
{
    public class DoctorController : Controller
    {
        private UserService userService;
        private DoctorServices Doctor;
        private List<Especialidades>? Listespecialidades = new List<Especialidades>();

        public DoctorController()
        {

        }

        public async Task<IActionResult> DoctorHomeAsync()
        {
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            HttpClient httpClient = await TokenAuthentication.AutenticarConTokenAsync();

            if (string.IsNullOrEmpty(usuarioJson))
            {
                return await signOut();
            }

            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            userService = new UserService(new ApiClient(httpClient));
            Doctor = new DoctorServices(new ApiClient(httpClient));
            

            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            Doctor? D = await Doctor.ObtenerDoctorAsync(u.Id);
            Listespecialidades = await Doctor.ObtenerEspecialidadAsync();


            // Crear el modelo de vista y asignar valores
            var viewModel = new DoctorHomeViewModel
            {
                Usuario = usuarioactual,
                _Doctor = D,
                Especialidades = Listespecialidades
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
            Usuario? u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
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
