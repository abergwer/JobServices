using JobServices.Models;

namespace JobServices.Services
{
    public class JobSchedulerService : BackgroundService
    {
        private readonly ILogger<JobSchedulerService> _logger;
        private readonly IJobService _jobService;
        public JobSchedulerService(ILogger<JobSchedulerService> logger, JobService jobService)
        {
            _logger = logger;
            _jobService = jobService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Job Scheduler Service is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var jobs = await _jobService.GetAllJobs();
                var now = DateTime.Now.ToUniversalTime();
                foreach (var job in jobs.Where(job => job.Status == "Active"))
                {
                    if (job.nextRun <= now && await _jobService.AtomicOperation(job) != null)
                    {
                        _logger.LogInformation($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        Console.WriteLine($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        await _jobService.UpdateJob(job.Id, job);
                    }
                    else
                    {
                        _logger.LogInformation($"Job {job.Name} is scheduled to run at {job.nextRun}, current time is {now}.");
                        Console.WriteLine($"Job {job.Name} is scheduled to run at {job.nextRun}, current time is {now}.");
                    }
                }
                await Task.Delay(1000*3, stoppingToken); // Check every second
            }
            _logger.LogInformation("Job Scheduler Service is stopping.");
        }
    }
}
