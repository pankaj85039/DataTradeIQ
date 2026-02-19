using Microsoft.AspNetCore.Mvc;

namespace SignalaraAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet]
        public ActionResult<object> Check()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "Signalara API",
                version = "1.0.0"
            });
        }
    }
}
