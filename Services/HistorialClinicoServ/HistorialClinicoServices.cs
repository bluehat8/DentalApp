using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DentalApp.Models;


namespace DentalApp.Services.HistorialClinicoServ
{
    public class HistorialClinicoService
    {

        public HistorialClinicoService()
        {
        }

        public async Task<HistorialClinico?> ObtenerHistorialClinico(int idUsuario)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{Constants.apiUrl}/api/HistorialClinico/obtenerHistorialClinico/{idUsuario}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        HistorialClinico? historialClinico = JsonConvert.DeserializeObject<HistorialClinico?>(content);

                        return historialClinico;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // El recurso no fue encontrado (código 404)
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
