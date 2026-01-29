using JobServices.Models;
using JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobConroller : ControllerBase
    {
        public readonly IJobService _jobService;

        public JobConroller(IJobService jobService)
        {
            _jobService = jobService;
        }   

        [HttpGet("status")]
        public IActionResult GetJobStatus()
        {
            return Ok(new { status = "Job service is running." });
        }

        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest job)
        {
            var newJob = await _jobService.CreateJob(job);
            if (newJob == null)
            {
                return BadRequest(new { message = "Failed to create job." });
            }
            return Ok(new { message = "Job created successfully.", job = newJob });
        }

        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            return Ok(new { jobs = await _jobService.GetAllJobs() });
            // Here you would add logic to retrieve all jobs.
        }

        [HttpPut("ResumeJob")]
        public async Task<IActionResult> ResumeJob([FromQuery] string jobName)
        {
            var result = await _jobService.ResumeJob(jobName);
            if (!result)
            {
                return NotFound(new { message = $"Job {jobName} not found." });
            }
            return Ok(new { message = $"Job {jobName} resumed successfully." });
        }

        [HttpPut("PauseJob")]
        public async Task<IActionResult> PauseJob([FromQuery] string jobName)
        {
            var result = await _jobService.StopJob(jobName);
            if (!result)
            {
                return NotFound(new { message = $"Job {jobName} not found." });
            }
            return Ok(new { message = $"Job {jobName} paused successfully." });
        }

        [HttpDelete("DeleteJob")]
        public async Task<IActionResult> DeleteJob([FromQuery] string jobName)
        {
            var result = await _jobService.DeleteJob(jobName);
            if (!result)
            {
                return NotFound(new { message = $"Job {jobName} not found." });
            }
            return Ok(new { message = $"Job {jobName} deleted successfully." });
            // Here you would add logic to delete the job with the given ID.
        }
    }
}
