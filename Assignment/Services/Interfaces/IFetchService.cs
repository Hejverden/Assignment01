using Assignment.Models;

/// <summary>
/// Interface for FetchService: the reason for this interface is to allow for mocking the FetchService in unit tests.
/// </summary>
public interface IFetchService
{
    Task<List<Photo>> SearchPhotosAsync(string searchTerm, int page, string filter);
}