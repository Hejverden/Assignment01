using Microsoft.AspNetCore.Mvc;
using Moq;
using Assignment.Models;
using Microsoft.Extensions.Logging;

/// <summary>
/// Unit tests: Mock-testing the logic and functionality of the PhotosController class.
///     The only logic inside the PhotosController class is the SearchPhotos endpoint method.
///     And the logic of this endpoint method is dependent on the functionality (logic) of the IFetchService class,
///     Thus, the IFetchService class needs to be mocked in order to test the SearchPhotos functionality.
/// </summary>
public class PhotosControllerTests
{
    [Fact]
    public async Task SearchPhotos_ReturnsOkObjectResult_WithListOfPhotos()
    {
        // Arrange
        // Create a mock of IFetchService
        var mockFetchService = new Mock<IFetchService>();

        // Create an instance of PhotosController with the mock of IFetchService
        var logger = new Mock<ILogger<PhotosController>>().Object;
        var photosController = new PhotosController(mockFetchService.Object, logger);
        
        // Mock the SearchPhotosAsync method of IFetchService to return a mocked list of photos
        var searchTerm = "test";
        var page = 1;
        var mockPhotos = new List<Photo> { new Photo 
        { Id = "1", 
        Owner = "test", 
        Secret = "secret", 
        Server = "server", 
        Title = "Test Photo",
        Farm = 1,
        IsPublic = 1,
        IsFriend = 0,
        IsFamily = 0
        } };
        mockFetchService.Setup(service => service.SearchPhotosAsync(searchTerm, page, "Relevant")).ReturnsAsync(mockPhotos);

        // Act : Call the SearchPhotos endpoint method of PhotosController
        var result = await photosController.SearchPhotos(searchTerm, page, "Relevant");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPhotos = Assert.IsType<List<Photo>>(okResult.Value);
        Assert.Single(returnedPhotos);
        Assert.Equal("Test Photo", returnedPhotos[0].Title);
    }
}