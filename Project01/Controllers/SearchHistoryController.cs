using Microsoft.AspNetCore.Mvc;
using Project01.Data;
using Microsoft.EntityFrameworkCore;


namespace Project01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchHistoryController: ControllerBase
{
    private readonly Project01DbContext _context;

    public SearchHistoryController(Project01DbContext context)
    {
        _context = context;
    }


    [HttpGet("show")]
    public async Task<IActionResult> ShowSearchHistory()
    {
        var searchQueries = await _context.SearchQueries
                                            .OrderByDescending(sq => sq.SearchTime)
                                            .ToListAsync();
        return Ok(searchQueries);
    }
}