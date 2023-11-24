using DentalApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DentalApp.Services.TratamientoService
{
    public class TratamientoService
    {
        public async Task<List<Tratamiento?>?>? ListarTratamientos()
        {
            string apiUrl = Constants.apiUrl + "/api/Tratamientos/ListarTratamientos";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(apiUrl);

                    List<Tratamiento>? tratamientos = await response.Content.ReadFromJsonAsync<List<Tratamiento>>();

                    return tratamientos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return new List<Tratamiento?>();
                }
            }
        }

        public async Task<List<Tratamiento?>?>? IngresarTratamiento(Tratamiento tratamientoDto)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                string apiUrl = Constants.apiUrl + "/api/Tratamientos/Ingresar";

                string jsonContent = JsonConvert.SerializeObject(tratamientoDto);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tratamientos = JsonConvert.DeserializeObject<List<Tratamiento>>(responseContent);

                    return tratamientos;
                }
                else
                {
                    Console.WriteLine($"Error al enviar la solicitud. Código: {response.StatusCode}");

                    return new List<Tratamiento?>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Tratamiento?>();
            }
        }


        public async Task<List<Tratamiento>> EliminarTratamiento(int id)
        {
            List<Tratamiento> tratamientosActualizados = new List<Tratamiento>();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = Constants.apiUrl + $"/api/Tratamientos/Delete/{id}";

                    var response = await httpClient.DeleteAsync(apiUrl);

                    // Verifica si la solicitud fue exitosa (código 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Tratamiento eliminado correctamente");
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var tratamientos = JsonConvert.DeserializeObject<List<Tratamiento>>(responseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Error al enviar la solicitud. Código: {response.StatusCode}");

                        tratamientosActualizados = new List<Tratamiento>();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al realizar la solicitud HTTP: {ex.Message}");

                tratamientosActualizados = new List<Tratamiento>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar la respuesta JSON: {ex.Message}");

                tratamientosActualizados = new List<Tratamiento>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");

                tratamientosActualizados = new List<Tratamiento>();
            }

            return tratamientosActualizados;
        }
    }
}
