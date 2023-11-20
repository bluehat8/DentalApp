namespace DentalApp.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(string url);
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request);
        Task<TResponse> DeleteAsync<TResponse>(string url);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request);
    }
}
