using DentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DentalApp.Services.DoctorServices
{
    public class DoctorServices
    {
        private readonly ApiClient _apiClient;

        public DoctorServices(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> ActualizarUsuarioAsync(int id, Usuario usuario)
        {
            var apiUrl = Constants.apiUrl + $"/api/Usuarios/{id}";
            HttpClient client = new HttpClient();

            try
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(jsonResponse);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<Doctor?> ObtenerDoctorAsync(int id)
        {
            try
            {
                var apiUrl = Constants.apiUrl + $"/api/Dentista/{id}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var Doctor = JsonConvert.DeserializeObject<Doctor>(jsonResponse);

                        return Doctor;
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


        public async Task <List<Especialidades>?> ObtenerEspecialidadAsync()
        {
            try
            {
                var apiUrl = Constants.apiUrl + $"/api/Especialidad/ListarEspecialidades";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                          var especialidades = JsonConvert.DeserializeObject<List<Especialidades>>(jsonResponse);

                        return especialidades;
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
