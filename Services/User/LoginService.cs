using DentalApp.Interfaces;
using DentalApp.Models;
using NuGet.Protocol.Plugins;

namespace DentalApp.Services.User
{
    public class LoginService
    {
        private readonly ApiClient _apiClient;

        public LoginService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<(Usuario? user, string message)> Login(string username, string password)
        {
            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = username,
                Password = password
            };

            var loginResponse = await _apiClient.PostAsync<LoginRequest, ApiResponse<Usuario>>
                (Constants.apiUrl + Constants.loginEndpoint, loginRequest);

            var user = loginResponse?.response;
            var message = loginResponse?.message;

            return (user, message ?? "");
        }

    }

    public class LoginRequest
    {
        public string? UsernameOrEmail { get; set; }
        public string? Password { get; set; }
    }

}
