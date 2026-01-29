using JobServices.Models;

namespace JobServices.Services
{
    public interface IJobService
    {
        Task<Job> CreateJob(CreateJobRequest createJob);
        Task<IEnumerable<Job>> GetAllJobs();
        void CheckJobStatus(Guid jobId);
        Task<bool> StopJob(string jobName);
        Task<bool> ResumeJob(string jobName);
        Task<bool> DeleteJob(string jobName);
    }
}
