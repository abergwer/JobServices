using JobServices.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace JobServices.Services
{
    public class JobService : IJobService
    {
        public readonly IMongoCollection<Job> _jobs;

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
                    nextRun = DateTime.Now.ToUniversalTime().AddSeconds(secondsTimer)
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

        public async Task UpdateJob(string id, Job job)
        {
            try
            {
                var filter = Builders<Job>.Filter.Eq(j => j.Id, id);
                var update = Builders<Job>.Update
                    .Set(j => j.nextRun, DateTime.Now.ToUniversalTime().AddSeconds(secondsTimer))
                    .Set(j => j.Status, "Active");

                await _jobs.UpdateOneAsync(filter, update);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to update job in database", ex);
            }
        }

        /// <summary>
        /// Atomically updates a job status from "Active" to "Running".
        /// </summary>
        /// <param name="job">The job to update.</param>
        /// <returns>The updated job document, or null if the job was not found or not in Active status.</returns>
        public async Task<Job> AtomicOperation(Job job)
        {
            var filter = Builders<Job>.Filter.And(
                Builders<Job>.Filter.Eq(j => j.Id, job.Id),
                Builders<Job>.Filter.Eq(j => j.Status, "Active")
            );

            var update = Builders<Job>.Update.Set(j => j.Status, "Running");

            var options = new FindOneAndUpdateOptions<Job> { ReturnDocument = ReturnDocument.After };

            var updatedJob = await _jobs.FindOneAndUpdateAsync(filter, update, options);

            return updatedJob;
        }
    }
}
