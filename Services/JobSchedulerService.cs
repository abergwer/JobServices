using JobServices.Models;
using MongoDB.Driver;

namespace JobServices.Services
{
    public class JobSchedulerService : BackgroundService
    {
        private readonly IJobService _jobService;
        private readonly IMongoCollection<JobExecution> _executions;

        private readonly string _instanceId;
        public JobSchedulerService(MongoDbContext mongoDbContext, JobService jobService)
        {
            _jobService = jobService;
            _executions = mongoDbContext.GetCollection<JobExecution>("JobExecutions");

            _instanceId = Guid.NewGuid().ToString();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int secondsTimer = 5;
            Console.WriteLine("Job Scheduler Service is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var jobs = await _jobService.GetAllJobs();
                var now = DateTime.Now.ToUniversalTime();
                foreach (var job in jobs.Where(job => job.Status == "Active"))
                {
                    if (job.Payload.Contains("failed"))
                    {
                        await LogExecutionAsync(job, "Failed", "Simulated failure due to 'fail' in payload.");
                        await _jobService.UpdateJob(job.Id, job);
                    }
                    else if (job.nextRun <= now && await _jobService.AtomicOperation(job) != null)
                    {
                        Console.WriteLine($"Executing Job: {job.Name} at {now.ToLocalTime()}");
                        await LogExecutionAsync(job, "Completed");
                        await _jobService.UpdateJob(job.Id, job);
                    }
                    else
                    {
                        Console.WriteLine($"Job {job.Name} is scheduled to run at {job.nextRun.ToLocalTime()}, current time is {now.ToLocalTime()}.");
                    }
                }
                await Task.Delay(1000*secondsTimer, stoppingToken); // Check every second
            }
            Console.WriteLine("Job Scheduler Service is stopping.");
        }

        public async Task LogExecutionAsync(Job job, string status, string? errorMessage = null)
        {
            var execution = new JobExecution
            {
                JobName = job.Name,
                JobType = job.Type,
                Payload = job.Payload,
                ExecutedAt = DateTime.Now.ToLocalTime(),
                InstanceId = _instanceId,
                Status = status,
                ErrorMessage = errorMessage
            };

            await _executions.InsertOneAsync(execution);
        }
    }
}
