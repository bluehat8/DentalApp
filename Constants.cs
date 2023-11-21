using DentalApp.Models;
using static System.Net.WebRequestMethods;

namespace DentalApp
{
    public class Constants
    {
        public enum DentalRole
        {
            admin = 0,
            cliente = 1,
            doctor = 2,
            asistente =3
        }

        public static Usuario? actualuser {  get; set; }

        public static string apiUrl = "https://localhost:7247";
        public static string loginEndpoint = "/api/Usuarios/api/Login";


    }
}
