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

        public enum DentalSolicitudCitaStatus
        {
            pendiente = 1,
            aceptada = 2,
            cancelada = 4,
            rechazada = 5,
        }

        public enum DentalTipoBusqueda
        {
            PorIdUsuario,
            PorCedula
        }

        public static Usuario? actualuser {  get; set; }

        public static string apiUrl = "https://localhost:7247";
        public static string loginEndpoint = "/api/Usuarios/api/Login";


    }
}
