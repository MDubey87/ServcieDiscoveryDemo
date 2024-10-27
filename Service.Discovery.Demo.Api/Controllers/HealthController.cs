using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Discovery.Demo.Api.Controllers
{    
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult CheckHealth()
        {
            // Implement health check logic
            return Ok("Healthy");
        }
    }
}
