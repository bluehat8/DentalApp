using DentalApp.Models;
using static System.Net.WebRequestMethods;

namespace DentalApp
{
    public class Constants
    {
        public static Usuario? actualuser {  get; set; }

        public static string apiUrl = "https://localhost:7247";
        public static string loginEndpoint = "/api/Usuarios/api/Login";


    }
}
