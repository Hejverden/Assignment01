using Microsoft.AspNetCore.Mvc.Testing;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project01.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Integration (End-to-End) test: testing requests against the application as if it were running in a real server environment
/// Use Program as the entry point
/// </summary>
[TestClass]
public class IntegrationTest
{
    private static WebApplicationFactory<Program> _factory;
    private static HttpClient _client;

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        // Load environment variables from the .env file
        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
        var envFilePath = projectDir != null ? Path.Combine(projectDir, ".env") : null;
        if (File.Exists(envFilePath))
        {
            Env.Load(envFilePath);
        }

        // Create the WebApplicationFactory
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "FlickrApiSettings:ApiKey", Env.GetString("FLICKR_API_KEY") },
                        { "FlickrApiSettings:ApiSecret", Env.GetString("FLICKR_API_SECRET") }
                    });
                });
            });

        // Create the HttpClient
        _client = _factory.CreateClient();

        // Apply migrations and ensure the database is up-to-date
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<Project01DbContext>();
            dbContext.Database.Migrate();
        }
    }

    /// <summary>
    /// Performing the test: testing the application from the client's perspective, interacting with it through HTTP requests and validating the responses.
    ///     Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
    ///     Check the response for success status, content type, and non-empty content. 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task SearchPhotos()
    {
        // Arrange
        var url = "/api/photos/search?searchTerm=test&page=1&sort=Relevant";

        // Act : Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
        var response = await _client.GetAsync(url);

        // Assert : Validate the response
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode); // Ensure success status code

        var content = await response.Content.ReadAsStringAsync();
        Assert.IsFalse(string.IsNullOrEmpty(content), "Response content is empty");

        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    /// <summary>
    /// Performing the test: testing the application from the client's perspective, interacting with it through HTTP requests and validating the responses.
    ///     Send an HTTP GET request to the /api/photos/getRecent endpoint with a query parameter
    ///     Check the response for success status, content type, and non-empty content. 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task SearchPhotosWithNoSearchTerm()
    {
        // Arrange
        var url = "/api/photos/search?searchTerm=NULL&page=1&sort=Relevant";

        // Act : Send an HTTP GET request to the /api/photos/search endpoint with a query parameter
        var response = await _client.GetAsync(url);

        // Assert : Validate the response
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode); // Ensure success status code

        var content = await response.Content.ReadAsStringAsync();
        Assert.IsFalse(string.IsNullOrEmpty(content), "Response content is empty");

        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
}
