using DentalApp.Models;
using Newtonsoft.Json;
using System.Net;

namespace DentalApp.Services
{
    public class TipoCitaServices
    {
        public async Task<List<TipoCita?>?>? ObtenerTiposCitaAsync()
        {
            try
            {
                var apiUrl = Constants.apiUrl + $"/api/TipoCita/ListarTipoCita";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var tipocitas = JsonConvert.DeserializeObject<List<TipoCita>>(jsonResponse);

                        return tipocitas;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else
                    {
                        return null;
                    }
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
