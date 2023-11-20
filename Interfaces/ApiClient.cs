using DentalApp.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace DentalApp.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error al realizar la solicitud HTTP", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("Error al deserializar la respuesta JSON", ex);
            }
            catch (Exception ex)
            {
                throw new ApiException("Error inesperado", ex);
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error al realizar la solicitud HTTP", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("Error al deserializar la respuesta JSON", ex);
            }
            catch (Exception ex)
            {
                throw new ApiException("Error inesperado", ex);
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error al realizar la solicitud HTTP", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("Error al deserializar la respuesta JSON", ex);
            }
            catch (Exception ex)
            {
                throw new ApiException("Error inesperado", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error al realizar la solicitud HTTP", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("Error al deserializar la respuesta JSON", ex);
            }
            catch (Exception ex)
            {
                throw new ApiException("Error inesperado", ex);
            }
        }
    }

    public class ApiException : Exception
    {
        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
