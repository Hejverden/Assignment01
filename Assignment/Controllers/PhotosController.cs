using Microsoft.AspNetCore.Mvc;
using Assignment.Models;

[ApiController]
[Route("api/[controller]")]
public class PhotosController(IFetchService fetchService, ILogger<PhotosController> logger) : ControllerBase
{
    private readonly IFetchService _fetchService = fetchService;
    private readonly ILogger<PhotosController> _logger = logger;

    /// <summary>
    /// Endpoint to search photos from Flickr: api/photos/search?searchTerm={searchTerm}&page={page}&sort={sort}
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <param name="page"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("search")]
    public async Task<ActionResult<List<Photo>>> SearchPhotos([FromQuery] string searchTerm, [FromQuery] int page, [FromQuery] string sort)
    {
        try
        {
            var photos = await _fetchService.SearchPhotosAsync(searchTerm == "NULL" ? "" : searchTerm, page, sort);
            return Ok(photos);
        }
        catch (Exception ex)
        {
            // Log the exception details
            _logger.LogError(ex, "Failed to search photos with term {SearchTerm} and page {Page}", searchTerm, page);

            // Return an appropriate error response
            var message = $"Start{Environment.NewLine} Oops! {Environment.NewLine} {ex.Message} {Environment.NewLine} End";
            throw new Exception(message.Replace(Environment.NewLine, "<br>"));
        }
    }
}
