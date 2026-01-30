using JobServices.Models;
using MongoDB.Driver;

namespace JobServices.Services
{
    public class JobService : IJobService
    {
        private readonly IMongoCollection<Job> _jobs;

        public JobService(MongoDbContext mongoDbContext)
        {
            _jobs = mongoDbContext.GetCollection<Job>("Jobs");
        }
        public void CheckJobStatus(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public Task<Job> CreateJob(CreateJobRequest createJob)
        {
            int secondsTimer;
            if (int.TryParse(createJob.Schedule, out secondsTimer))
            {
                Job newJob = new Job
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createJob.Name,
                    Type = createJob.Type,
                    Payload = createJob.Payload,
                    Schedule = createJob.Schedule,
                    Status = "Active",
                    nextRun = DateTime.Now.AddSeconds(secondsTimer)
                };
                try
                {
                    _jobs.InsertOne(newJob);
                    return Task.FromResult(newJob);
                }
                catch (Exception)
                {
                    return Task.FromException<Job>(new Exception("Failed to create job in database"));
                }
            }
            else
            {
                return Task.FromException<Job>(new Exception("Schedule must be number of seconds"));
            }
        }

        public async Task<bool> DeleteJob(string jobName)
        {
            var result = await _jobs.DeleteOneAsync(job => job.Name == jobName);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Job>> GetAllJobs()
        {
            return await _jobs.Find(_ => true).ToListAsync();
        }

        public async Task<bool> ResumeJob(string jobName)
        {
            var filter = Builders<Job>.Filter.Eq(job => job.Name, jobName);
            var update = Builders<Job>.Update.Set(job => job.Status, "Active");

            var result = await _jobs.FindOneAndUpdateAsync(filter, update);
            return result != null;
        }

        public async Task<bool> StopJob(string jobName)
        {
            var filter = Builders<Job>.Filter.Eq(job => job.Name, jobName);
            var update = Builders<Job>.Update.Set(job => job.Status, "Paused");

            var result = await _jobs.FindOneAndUpdateAsync(filter, update);
            return result != null;
        }
    }
}
