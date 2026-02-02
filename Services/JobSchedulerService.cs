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
                foreach (var job in jobs)
                {
                    if (job.Status == "Active" && job.nextRun <= now)
                    {
                        _logger.LogInformation($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        Console.WriteLine($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        
                        await _jobService.UpdateJob(job.Id, job);
                    }
                }
                await Task.Delay(1000, stoppingToken); // Check every second
            }
            _logger.LogInformation("Job Scheduler Service is stopping.");
        }
    }
}
