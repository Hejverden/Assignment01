public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> GetStringAsync(string requestUri)
    {
        return _httpClient.GetStringAsync(requestUri);
    }

    // Additional wrapper methods for PostAsync, PutAsync, DeleteAsync, etc., can be added here.
}