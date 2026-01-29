using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobConroller : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetJobStatus()
        {
            return Ok(new { status = "Job service is running." });
        }
    }
}
