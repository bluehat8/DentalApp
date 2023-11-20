using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DentalApp
{
    public class TokenAuthentication
    {
        public static async Task<HttpClient> AutenticarConTokenAsync()
        {
            string apiUrl = "http://tutorialpruebasomee.somee.com/api/Usuario";
            string username = "Walger";
            string password = "1234";

            HttpClient httpClient = new HttpClient();

            var credentials = new
            {
                username,
                password
            };

            string jsonCredentials = JsonConvert.SerializeObject(credentials);

            HttpContent content = new StringContent(jsonCredentials, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string token = tokenObject.token;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return httpClient;
            }
            else
            {
                return httpClient;
            }
        }
    }
}
