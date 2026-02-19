using Microsoft.AspNetCore.Mvc;
using SignalaraAPI.Models;
using SignalaraAPI.Services;

namespace SignalaraAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignalsController : ControllerBase
{
    private readonly SignalService _signalService;
    private readonly ILogger<SignalsController> _logger;
    
    public SignalsController(SignalService signalService, ILogger<SignalsController> logger)
    {
        _signalService = signalService;
        _logger = logger;
    }
    
    /// <summary>
    /// Get trading signals for a given symbol
    /// </summary>
    /// <param name="symbol">Symbol like ^NSEI, ^NSEBANK, ^BSESN</param>
    /// <param name="barsCount">Number of historical bars to analyze (default: 100)</param>
    /// <returns>SignalsResponse with consensus and individual strategy signals</returns>
    [HttpGet]
    public async Task<ActionResult<SignalsResponse>> GetSignals(
        [FromQuery] string symbol = "^NSEI",
        [FromQuery] int barsCount = 100)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest(new { error = "Symbol is required" });
            }
            
            var response = await _signalService.CalculateSignalsAsync(symbol, barsCount);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, $"Invalid symbol: {symbol}");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate signals");
            return StatusCode(500, new { error = "Failed to calculate signals", message = ex.Message });
        }
    }
    
    /// <summary>
    /// Get list of supported indices
    /// </summary>
    [HttpGet("indices")]
    public ActionResult<Dictionary<string, string>> GetIndices()
    {
        var indices = _signalService.GetSupportedIndices();
        return Ok(indices);
    }
}
