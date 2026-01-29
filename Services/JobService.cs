using JobServices.Models;

namespace JobServices.Services
{
    public class JobService : IJobService
    {
        public void CheckJobStatus(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public Task<Job> CreateJob(CreateJobRequest createJob)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteJob(string jobName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Job>> GetAllJobs()
        {
            throw new NotImplementedException();
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
