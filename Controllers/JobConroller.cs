using JobServices.Models;
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

        [HttpPost("CreateJob")]
        public Task<IActionResult> CreateJob([FromBody] CreateJobRequest job)
        {
            // Here you would add logic to create the job based on the request data.
            return Task.FromResult<IActionResult>(Ok(new { message = "Job created successfully.", job }));
        }
    }
}
