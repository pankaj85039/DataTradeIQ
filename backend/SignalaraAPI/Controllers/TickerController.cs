using Microsoft.AspNetCore.Mvc;
using SignalaraAPI.Models;
using SignalaraAPI.Services;

namespace SignalaraAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TickerController : ControllerBase
{
    private readonly TickerService _tickerService;
    private readonly ILogger<TickerController> _logger;
    
    public TickerController(TickerService tickerService, ILogger<TickerController> logger)
    {
        _tickerService = tickerService;
        _logger = logger;
    }
    
    /// <summary>
    /// Get live ticker data for all indices
    /// Updates every 2 seconds
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<TickerData>>> GetTickers()
    {
        try
        {
            var tickers = await _tickerService.GetTickersAsync();
            return Ok(tickers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch tickers");
            return StatusCode(500, new { error = "Failed to fetch ticker data", message = ex.Message });
        }
    }
}
