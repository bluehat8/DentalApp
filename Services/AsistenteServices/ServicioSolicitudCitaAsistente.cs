using DentalApp.Models;

namespace DentalApp.Services.AsistenteServices
{
    public class ServicioSolicitudCitaAsistente
    {
        public async Task<List<SolicitudCita>?> ListarTodasLasSolicitudes()
        {
            string apiUrl = Constants.apiUrl + "/api/citas/getSolicitudesCita";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    List<SolicitudCita>? solicitudes = await response.Content.ReadFromJsonAsync<List<SolicitudCita>>();

                    return solicitudes;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<SolicitudCita>();
                }
            }
        }

        public async Task<string> AceptarSolicitudCita(int solicitudId)
        {
            var httpClient = new HttpClient();

            var url = Constants.apiUrl + "/api/citas/aceptarCita/" + solicitudId;

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

        public async Task<string> RechazarSolicitudCita(int solicitudId)
        {
            var httpClient = new HttpClient();

            var url = Constants.apiUrl + "/api/citas/RechazarCita/" + solicitudId;

            var response = await httpClient.PutAsJsonAsync(url, solicitudId);

            if (response.IsSuccessStatusCode)
            {
                return "Solicitud rechazada correctamente correctamente";
            }
            else
            {
                return "Ocurrió un error al actualizar la solicitud";
            }
        }
    }
}
