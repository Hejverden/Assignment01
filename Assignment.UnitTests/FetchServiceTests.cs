using Assignment.Models;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;

/// <summary>
/// Unit tests: Mock-testing the logic and functionality of the FetchService class.
///     The only logic inside the FetchService class is the SearchPhotosAsync method, which fetches photos from the Flickr API.
///     And that logic is dependent on IHttpClientWrapper.
///     Thus, the IHttpClientWrapper needs to be mocked in order to test the FetchService class.
/// </summary>
public class FetchServiceTests
{
    [Fact]
    public async Task SearchPhotosAsync_ReturnsPhotos()
    {
        // Arrange
        // Create a mock of IHttpClientWrapper
        var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();

        // Create an instance of IOptions<FlickrApiSettings>
        var flickrApiSettings = Options.Create(new FlickrApiSettings 
        { 
            ApiKey = "test_key", 
            ApiSecret = "test_secret",
            BaseUrl = "https://api.flickr.com",
            EndpointPath = "/services/rest",
            Method = "flickr.photos.search",
            Format = "json"
        });

        // Create an instance of FetchService with the mock of IHttpClientWrapper and the instance of IOptions<FlickrApiSettings>
        var fetchService = new FetchService(mockHttpClientWrapper.Object, flickrApiSettings);

        // Mock the response content from the Flickr API
        var mockResponseContent = new JObject(
            new JProperty("photos", new JObject(
                new JProperty("photo", new JArray(
                    new JObject(
                        new JProperty("id", "53853127416"),
                        new JProperty("owner", "201033373@N05"),
                        new JProperty("secret", "0f26b237a0"),
                        new JProperty("server", "65535"),
                        new JProperty("farm", 66),
                        new JProperty("title", "Panchita hdr"),
                        new JProperty("ispublic", 1),
                        new JProperty("isfriend", 0),
                        new JProperty("isfamily", 0)
                    )
                ))
            ))
        ).ToString();

        // Mock the GetStringAsync method of IHttpClientWrapper to return the mock response content
        var searchTerm = "cat";
        var page = 1;
        var flickrApiUrl = $"https://api.flickr.com/services/rest?method=flickr.photos.search&api_key=test_key&text={searchTerm}&sort=relevance&format=json&nojsoncallback=1&page={page}";

        mockHttpClientWrapper.Setup(wrapper => wrapper.GetStringAsync(It.Is<string>(url => url == flickrApiUrl)))
                             .ReturnsAsync(mockResponseContent);

        // Act : Call the SearchPhotosAsync method of FetchService
        var result = await fetchService.SearchPhotosAsync(searchTerm, page, "Relevant");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Panchita hdr", result[0].Title);
        Assert.Equal("53853127416", result[0].Id);
        Assert.Equal("65535", result[0].Server);
        Assert.Equal("0f26b237a0", result[0].Secret);
    }
}