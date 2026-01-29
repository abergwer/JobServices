using JobServices.Models;

namespace JobServices.Services
{
    public class JobService : IJobService
    {
        public List<Job> Jobs = new List<Job>();
        public void CheckJobStatus(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public Task<Job> CreateJob(CreateJobRequest createJob)
        {
            Job newJob = new Job
            {
                Id = Guid.NewGuid(),
                Name = createJob.Name,
                Type = createJob.Type,
                Payload = createJob.Payload,
                Schedule = createJob.Schedule,
                Status = "Active"
            };
            Jobs.Add(newJob);
            return Task.FromResult(newJob);
        }

        public Task<bool> DeleteJob(string jobName)
        {
            var job = Jobs.FirstOrDefault(job => job.Name == jobName);
            if (job != null)
            {
                Jobs.Remove(job);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);

        }

        public Task<IEnumerable<Job>> GetAllJobs()
        {
            if (Jobs.Count == 0)
            {
                return Task.FromResult(Enumerable.Empty<Job>());
            }
            return Task.FromResult(Jobs.AsEnumerable());
        }

        public Task<bool> ResumeJob(string jobId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopJob(string jobId)
        {
            throw new NotImplementedException();
        }
    }
}
