using DentalApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DentalApp.Services.ClientServices
{
    public class ServicioSolicitarCita
    {
        private readonly HttpClient _httpClient = new HttpClient();


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


        public async Task EnviarSolicitudCita(SolicitudCita solicitudDto)
        {

            try
            {
                string apiUrl = Constants.apiUrl + "/api/citas";

                string jsonContent = JsonConvert.SerializeObject(solicitudDto);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                // Verifica si la solicitud fue exitosa (código 2xx)
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Solicitud enviada correctamente");
                }
                else
                {
                    Console.WriteLine($"Error al enviar la solicitud. Código: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
