using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DentalApp.Models;
using static DentalApp.Constants;
using System.Text;


namespace DentalApp.Services.HistorialClinicoServ
{
    public class HistorialClinicoService
    {

        public HistorialClinicoService()
        {
        }

        public async Task<HistorialClinico?> ObtenerHistorialClinico(string identificador, DentalTipoBusqueda tipoBusqueda)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string endpoint = tipoBusqueda == DentalTipoBusqueda.PorIdUsuario
                        ? $"obtenerHistorialClinico/{Convert.ToInt32(identificador)}"
                        : $"obtenerHistorialClinicoPorCedula/{identificador}";

                    string url = $"{Constants.apiUrl}/api/HistorialClinico/{endpoint}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<HistorialClinico>(content);
                    }
                    else
                    {
                        return new HistorialClinico();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new HistorialClinico();
                }
            }

        }


        public async Task CrearModificarHistorialClinico(HistorialClinico historialClinicoDto)
        {
            HttpClient client = new HttpClient();
            string url = $"{Constants.apiUrl}/api/HistorialClinico/crearModificarHistorialClinico";

            try
            {
                var jsonContent = JsonConvert.SerializeObject(historialClinicoDto);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Operación exitosa");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


    }
}
