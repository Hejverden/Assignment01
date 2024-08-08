using Microsoft.AspNetCore.Mvc;
using Project01.Models;
using Project01.Data;
using Microsoft.EntityFrameworkCore;


namespace Project01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController : ControllerBase
{
    private readonly IFetchService _fetchService;
    private readonly Project01DbContext _context;
    private readonly ILogger<PhotosController> _logger;

    public PhotosController(IFetchService fetchService, Project01DbContext context, ILogger<PhotosController> logger)
    {
        _fetchService = fetchService;
        _context = context;
        _logger = logger;
    }

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
        // Check if the search term already exists in the database
        var existingSearchQuery = await _context.SearchQueries
                                                .AnyAsync(sq => sq.QueryText.ToLower() == searchTerm.ToLower());

        if (!existingSearchQuery)
        {
            // The search term does not exist, so add it to the database
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchQuery = new SearchQuery
                {
                    QueryText = searchTerm,
                    SearchTime = DateTime.Now
                };

                _context.SearchQueries.Add(searchQuery);
                await _context.SaveChangesAsync();
            }
        }

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
