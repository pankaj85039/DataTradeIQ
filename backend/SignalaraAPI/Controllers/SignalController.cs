using Microsoft.AspNetCore.Mvc;

namespace SignalaraAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalController : ControllerBase
    {
        private readonly ILogger<SignalController> _logger;

        public SignalController(ILogger<SignalController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get a welcome message from the API
        /// </summary>
        [HttpGet("welcome")]
        public ActionResult<object> GetWelcome()
        {
            return Ok(new
            {
                message = "Welcome to Signalara API!",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                status = "running"
            });
        }

        /// <summary>
        /// Get all signals (placeholder)
        /// </summary>
        [HttpGet]
        public ActionResult<object> GetSignals()
        {
            return Ok(new
            {
                signals = new object[]
                {
                    new { id = 1, name = "Signal 1", value = 100 },
                    new { id = 2, name = "Signal 2", value = 200 },
                    new { id = 3, name = "Signal 3", value = 300 }
                },
                total = 3
            });
        }

        /// <summary>
        /// Get a specific signal by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<object> GetSignalById(int id)
        {
            return Ok(new
            {
                id = id,
                name = $"Signal {id}",
                value = 100 * id,
                createdAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Create a new signal
        /// </summary>
        [HttpPost]
        public ActionResult<object> CreateSignal([FromBody] SignalRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new { error = "Signal name is required" });
            }

            return Created("", new
            {
                id = new Random().Next(1, 1000),
                name = request.Name,
                value = request.Value,
                createdAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("health")]
        public ActionResult<object> HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                uptime = "API is running"
            });
        }
    }

    /// <summary>
    /// Request model for creating a signal
    /// </summary>
    public class SignalRequest
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
