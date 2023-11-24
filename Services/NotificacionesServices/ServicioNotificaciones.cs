using DentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace DentalApp.Services.NotificacionesServices
{
    public class ServicioNotificaciones
    {

        public async Task<List<Notificaciones>?> ListarNotificaciones()
        {
            string apiUrl = Constants.apiUrl + "/api/Notificaciones/ObtenerNotificaciones";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    List<Notificaciones>? notificaciones = await response.Content.ReadFromJsonAsync<List<Notificaciones>>();

                    return notificaciones;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<Notificaciones>(); 
                }
            }
        }

        public async Task<List<Notificaciones?>?>? ObtenerNotificacionesPorCliente(int usuarioId)
        {
            string apiUrl = Constants.apiUrl + "/api/Notificaciones/ListarNotificaciones/" + usuarioId;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonResponse = await httpClient.GetStringAsync(apiUrl);

                    List<Notificaciones?>? notificaciones = JsonConvert.DeserializeObject<List<Notificaciones>>(jsonResponse);

                    return notificaciones;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<Notificaciones?>();

                }
            }
        }


    }
}
