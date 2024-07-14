using Microsoft.AspNetCore.Mvc.Testing;
using DotNetEnv;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Integration (End-to-End) test: testing requests against the application as if it were running in a real server environment
/// Use Program as the entry point
/// </summary>
public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>> 
{
    private readonly HttpClient _client;

    /// <summary>
    /// Preparing the test setup:
    ///     Loading environment variables
    ///     Configuring application settings by setting up an instance of HttpClient
    /// </summary>
    /// <param name="factory"></param>
    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        #region Load environment variables from the .env file in the solution directory

        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        var envFilePath = projectDir != null ? Path.Combine(projectDir, ".env") : null;
        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        #endregion

        #region Setting up an instance of HttpClient

        _client = factory.WithWebHostBuilder(builder =>
        {
            _ = builder.ConfigureAppConfiguration((context, config) =>
            {
                _ = config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "FlickrApiSettings:ApiKey", Env.GetString("FLICKR_API_KEY") },
                    { "FlickrApiSettings:ApiSecret", Env.GetString("FLICKR_API_SECRET") }
                });
            });
        }).CreateClient();

        #endregion
    }

    /// <summary>
    /// Performing the test: testing the application from the client's perspective, interacting with it through HTTP requests and validating the responses.
    ///     Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
    ///     Check the response for success status, content type, and non-empty content. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SearchPhotos_EndpointReturnsSuccessAndCorrectContentType()
    {
        try
        { 
            // Arrange
            var url = "/api/photos/search?searchTerm=test&page=1&sort=Relevant";

            // Act : Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
            var response = await _client.GetAsync(url);

            // Assert : Validate the response
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode); // Ensure success status code

            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content is empty");

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"HTTP request failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception ($"Unexpected error: {ex.Message}");
        }
    }


    /// <summary>
    /// Performing the test: testing the application from the client's perspective, interacting with it through HTTP requests and validating the responses.
    ///     Send an HTTP GET request to the /api/photos/getRecent endpoint with a query parameter
    ///     Check the response for success status, content type, and non-empty content. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SearchPhotosWithNoSearTerm_EndpointReturnsSuccessAndCorrectContentType()
    {
        try
        { 
            // Arrange
            var url = "/api/photos/search?searchTerm=NULL&page=1&sort=Relevant";

            // Act : Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
            var response = await _client.GetAsync(url);

            // Assert : Validate the response
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode); // Ensure success status code

            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content is empty");

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"HTTP request failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception ($"Unexpected error: {ex.Message}");
        }
    }
}