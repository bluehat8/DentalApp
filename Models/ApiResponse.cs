namespace DentalApp.Models
{
    public class ApiResponse<T>
    {
        public string? message { get; set; }
        public T? response { get; set; }
    }
}
