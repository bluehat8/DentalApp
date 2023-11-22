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

        public async Task<string> UpdateSolicitudCita(int solicitudId, SolicitudCita solicitud)
        {
            var httpClient = new HttpClient();

            var url = Constants.apiUrl+"/api/citas/updateSolicitud/" + solicitudId;

            var response = await httpClient.PutAsJsonAsync(url, solicitud);

            if (response.IsSuccessStatusCode)
            {
                return "Solicitud actualizada correctamente";
            }
            else
            {
                return "Ocurrió un error al actualizar la solicitud";
            }

        }

        public async Task<string> CancelarSolicitudCita(int solicitudId)
        {
            var httpClient = new HttpClient();

            var url = Constants.apiUrl + "/api/citas/cancelarCita/" + solicitudId;

            var response = await httpClient.PutAsJsonAsync(url, solicitudId);

            if (response.IsSuccessStatusCode)
            {
                return "Solicitud actualizada correctamente";
            }
            else
            {
                return "Ocurrió un error al actualizar la solicitud";
            }

        }

    }
}
