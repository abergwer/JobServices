namespace JobServices.Services
{
    public class JobSchedulerService : BackgroundService
    {
        private readonly ILogger<JobSchedulerService> _logger;
        private readonly IJobService _jobService;
        public JobSchedulerService(ILogger<JobSchedulerService> logger, IJobService jobService)
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
                var now = DateTime.Now;
                foreach (var job in jobs)
                {
                    if (job.Status == "Active" && job.nextRun <= now)
                    {
                        _logger.LogInformation($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        Console.WriteLine($"Executing job: {job.Name} of type {job.Type} with payload: {job.Payload}");
                        // Here you would add the logic to execute the job based on its type and payload.
                        // For demonstration, we just update the nextRun time.
                        if (int.TryParse(job.Schedule, out int secondsTimer))
                        {
                            job.nextRun = now.AddSeconds(secondsTimer);
                        }
                    }
                }
                await Task.Delay(1000, stoppingToken); // Check every second
            }
            _logger.LogInformation("Job Scheduler Service is stopping.");
        }
    }
}
