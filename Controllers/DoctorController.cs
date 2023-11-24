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
using DentalApp.Services.NotificacionesServices;
using DentalApp.Services.TratamientoService;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;

namespace DentalApp.Controllers
{
    public class DoctorController : Controller
    {
        private UserService? userService;
        private DoctorServices? Doctor;
        private List<Especialidades>? Listespecialidades = new List<Especialidades>();
        private ServicioNotificaciones? notificacionesServ;
        private TratamientoService tratamientoService;

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

            notificacionesServ = new ServicioNotificaciones();
            tratamientoService = new TratamientoService();

            Usuario u = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            userService = new UserService(new ApiClient(httpClient));
            Doctor = new DoctorServices(new ApiClient(httpClient));
            

            Usuario? usuarioactual = await userService.ObtenerUsuarioAsync(u.Id);
            Doctor? D = await Doctor.ObtenerDoctorAsync(u.Id);
            Listespecialidades = await Doctor.ObtenerEspecialidadAsync();
            List<Notificaciones>? notificaciones = await notificacionesServ.ListarNotificaciones();
            List<Tratamiento>? tratamientos = await tratamientoService.ListarTratamientos();



            // Crear el modelo de vista y asignar valores
            var viewModel = new DoctorHomeViewModel
            {
                Usuario = usuarioactual,
                _Doctor = D,
                Especialidades = Listespecialidades,
                notificaciones = notificaciones,
                Tratamientos = tratamientos,
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

        [HttpPost]
        public async Task<ActionResult> AgregarTratamientoAsync()
        {
            try
            {
                // Recibe los datos directamente desde el formulario
                var nombre = HttpContext.Request.Form["Nombre"];
                var descripcion = HttpContext.Request.Form["Descripcion"];
                var duracion = HttpContext.Request.Form["Duracion"];
                var restricciones = HttpContext.Request.Form["Restricciones"];
                var precio = Convert.ToDecimal(HttpContext.Request.Form["Precio"]);
                var imagen = HttpContext.Request.Form["Imagen"];

                Tratamiento tratamiento = new Tratamiento()
                {
                    Nombre = nombre,
                    Descripcion= descripcion,
                    Restricciones = restricciones,
                    TipoTratamiento = 1,
                    Estado = 1,
                    Precio = precio,
                    Imagen = Encoding.ASCII.GetBytes("datos_de_la_imagen"),
                    Duracion = duracion

                };

                tratamientoService = new TratamientoService();
                List<Tratamiento>? tratamientos = await tratamientoService.IngresarTratamiento(tratamiento);


                var TableViewModel = new TratamientoPartialViewModel
                {
                    Tratamientos = tratamientos,
                };

                // Renderizar la vista parcial y convertirla en una cadena
                var partialView = this.RenderPartialViewToString("TratamientoPartial", TableViewModel);

                // Retornar la cadena HTML como resultado
                return Content(partialView);
            }
            catch (Exception ex)
            {
                return Content($"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult> EliminarTratamientoAsync(int idtratamiento)
        {
            try
            {

                tratamientoService = new TratamientoService();
                List<Tratamiento>? tratamientos = await tratamientoService.EliminarTratamiento(idtratamiento);


                var TableViewModel = new TratamientoPartialViewModel
                {
                    Tratamientos = tratamientos,
                };

                var partialView = this.RenderPartialViewToString("TratamientoPartial", TableViewModel);

                return Content(partialView);
            }
            catch (Exception ex)
            {
                return Content($"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> FiltrarTratamientoPorNombre(string filtro)
        {
            try
            {
                tratamientoService = new TratamientoService();

                List<Tratamiento>? tratamientos = await tratamientoService.ListarTratamientos();
                List<Tratamiento>? listFiltro;


                if (string.IsNullOrEmpty(filtro))
                {
                    listFiltro = tratamientos;
                }
                else
                {
                    listFiltro = tratamientos.Where(x => x.Nombre.ToLower().Contains(filtro.ToLower())
                || x.Descripcion.ToLower().Contains(filtro.ToLower())).ToList();
                }


                var TableViewModel = new TratamientoPartialViewModel
                {
                    Tratamientos = listFiltro,
                };

                var partialView = this.RenderPartialViewToString("TratamientoPartial", TableViewModel);

                return Content(partialView);
            }
            catch (Exception ex)
            {
                return Content($"Error interno del servidor: {ex.Message}");
            }
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                try
                {
                    var engine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                    var viewResult = engine.FindView(ControllerContext, viewName, false);

                    var viewContext = new ViewContext(
                        ControllerContext,
                        viewResult.View,
                        ViewData,
                        TempData,
                        sw,
                        new HtmlHelperOptions()
                    );

                    viewResult.View.RenderAsync(viewContext).Wait();
                    return sw.GetStringBuilder().ToString();
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    // Por ejemplo, puedes imprimir el mensaje de error en la consola
                    Console.WriteLine("Error al renderizar la vista parcial: " + ex.Message);
                    return string.Empty; // Otra acción para manejar el error según tu necesidad
                }
            }
        }





    }
}
