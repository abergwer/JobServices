using JobServices.Models;

namespace JobServices.Services
{
    public interface IJobService
    {
        Task<Job> CreateJob(CreateJobRequest createJob);
        Task<IEnumerable<Job>> GetAllJobs();
        void CheckJobStatus(Guid jobId);
        Task<bool> StopJob(string jobId);
        Task<bool> ResumeJob(string jobId);
        Task<bool> DeleteJob(string jobName);
    }
}
