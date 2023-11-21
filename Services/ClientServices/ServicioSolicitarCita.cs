using DentalApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DentalApp.Services.ClientServices
{
    public class ServicioSolicitarCita
    {

      
        public static async Task<List<SolicitudCita>> ObtenerSolicitudesCitasPorUsuario(int usuarioId)
        {
            string apiUrl = Constants.apiUrl+ "/api/citas/solicitudes-cita/" + usuarioId;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string jsonResponse = await httpClient.GetStringAsync(apiUrl);

                    List<SolicitudCita>? solicitudes = JsonConvert.DeserializeObject<List<SolicitudCita>>(jsonResponse);

                    return solicitudes;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<SolicitudCita>();

                }
            }
        }

    }
}
