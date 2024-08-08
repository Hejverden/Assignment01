using Microsoft.AspNetCore.Mvc;
using Moq;
using Project01.Models;
using Project01.Data;
using Project01.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Unit tests: Mock-testing the logic and functionality of the PhotosController class.
///     The only logic inside the PhotosController class is the SearchPhotos endpoint method.
///     And the logic of this endpoint method is dependent on the functionality (logic) of the IFetchService class,
///     Thus, the IFetchService class needs to be mocked in order to test the SearchPhotos functionality.
/// </summary>
[TestClass]
public class PhotosControllerTests
{
    [TestMethod]
    public async Task SearchPhotos_ReturnsOkObjectResult_WithListOfPhotos()
    {
        // Arrange
        // Create a mock of IFetchService, ILogger and DbContext:
        var mockFetchService = new Mock<IFetchService>();
        var mockLogger = new Mock<ILogger<PhotosController>>();
        
        var options = new DbContextOptionsBuilder<Project01DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new Project01DbContext(options))
        {
            var photosController = new PhotosController(mockFetchService.Object, context, mockLogger.Object);

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
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult, "The result should be of type OkObjectResult.");
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult), "The result should be of type OkObjectResult.");

            Assert.IsNotNull(okResult.Value, "The Value of OkObjectResult should not be null.");
            var returnedPhotos = okResult.Value as List<Photo>;
            Assert.IsNotNull(returnedPhotos, "The Value of OkObjectResult should be of type List<Photo>.");

            Assert.AreEqual(1, returnedPhotos.Count, "The list should contain exactly one item.");
            Assert.AreEqual("Test Photo", returnedPhotos[0].Title, "The title of the photo should be 'Test Photo'.");
        }
    }
}