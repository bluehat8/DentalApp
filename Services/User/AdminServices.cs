using DentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DentalApp.Services.User
{
    public class AdminServices
    {

        private readonly ApiClient _apiClient;

        public AdminServices(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public AdminServices()
        {

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


        public async Task<Usuario?> ObtenerUsuarioAsync(int id)
        {
            try
            {
                var apiUrl = Constants.apiUrl + $"/api/Usuarios/{id}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse);

                        return usuario;
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


        public async Task<(int id, string message)> CreateUsuarioAsync(Usuario usuario)
        {
            HttpClient client = new HttpClient();
            var apiUrl = Constants.apiUrl + $"/api/Usuarios/crearUsuario";


            try
            {
                usuario.Telefono = "1";

                // Serializar el objeto Usuario a formato JSON
                string jsonUsuario = JsonConvert.SerializeObject(usuario);

                // Configurar la solicitud HTTP
                var content = new StringContent(jsonUsuario, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                // Verificar si la solicitud fue exitosa
                response.EnsureSuccessStatusCode();

                // Leer la respuesta JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeAnonymousType(jsonResponse, new { id = 0, message = "" });

                return (result.id, result.message);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante la solicitud
                Console.WriteLine($"Error: {ex.Message}");
                return (0, "Error al realizar la solicitud");
            }
        }


    }
}
