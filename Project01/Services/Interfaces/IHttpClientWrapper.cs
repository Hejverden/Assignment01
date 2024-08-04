/// <summary>
/// Interface for HttpClientWrapper: the reason for this interface is to allow for mocking HttpClient in unit tests.
/// Since HttpClient.GetStringAsync is a non-virtual method, Moq cannot override it.
/// </summary>
public interface IHttpClientWrapper
{
    Task<string> GetStringAsync(string requestUri);

    // Signatures for PostAsync, PutAsync, DeleteAsync, etc.
}

