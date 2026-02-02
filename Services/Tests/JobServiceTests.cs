using JobServices.Models;
using JobServices.Services;
using MongoDB.Driver;
using Xunit;

namespace JobServices.Tests
{
    public class JobServiceTests : IDisposable
    {
        private readonly JobService _jobServiceTest;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public JobServiceTests()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("JobServiceTestsDb");
            _collectionName = "JobsTest"; // unique test collection

            // Pass a real collection to JobService
            _jobServiceTest = new JobService(_database.GetCollection<Job>(_collectionName));
        }

        public void Dispose()
        {
            // Cleanup after tests
            _client.DropDatabase("JobServiceTestsDb");
        }

        [Fact]
        public void RunningTests()
        {
            Console.WriteLine("Running Test Successed");
            Assert.True(true);
        }

        [Fact]
        public async Task CreateJobTest()
        {
            var jobRequest = new CreateJobRequest
            {
                Name = "Test Job",
                Type = "TypeA",
                Payload = "Payload Data",
                Schedule = "10"
            };
            var createdJob = await _jobServiceTest.CreateJob(jobRequest);
            Assert.NotNull(createdJob);
            Assert.Equal(jobRequest.Name, createdJob.Name);
            Assert.Equal(jobRequest.Type, createdJob.Type);
            Assert.Equal(jobRequest.Payload, createdJob.Payload);
            Assert.Equal(jobRequest.Schedule, createdJob.Schedule);
            Assert.Equal("Active", createdJob.Status);
        }

        [Fact]
        public async Task CreateJob_WithInvalidSchedule()
        {
            var jobRequest = new CreateJobRequest
            {
                Name = "Test Job",
                Type = "Email",
                Payload = "Payload Data",
                Schedule = "adf"
            };
            await Assert.ThrowsAnyAsync<Exception>(async () => await _jobServiceTest.CreateJob(jobRequest));
        }

        [Fact]
        public async Task StopJob()
        {
            var jobRequest = new CreateJobRequest
            {
                Name = "Test Job",
                Type = "Email",
                Payload = "Payload Data",
                Schedule = "10"
            };
            var createdJob = await _jobServiceTest.CreateJob(jobRequest);
            var stoppedJob = await _jobServiceTest.StopJob(createdJob.Name);
            Assert.NotNull(stoppedJob);
        }

        [Fact]
        public async Task AtomicOperation_OnlyOneInstanceUpdatesJob()
        {
            var job =  await _jobServiceTest.CreateJob(new CreateJobRequest
            {
                Name = "test",
                Type = "Email",
                Payload = "Payload",
                Schedule = "10"
            });

            // Act: run two tasks simulating two instances
            var task1 = _jobServiceTest.AtomicOperation(job);
            var task2 = _jobServiceTest.AtomicOperation(job);

            var results = await Task.WhenAll(task1, task2);

            // Assert: only one succeeded
            var updatedCount = results.Count(result => result != null && result.Status == "Running");
            Assert.Equal(1, updatedCount);

            // Assert: job in DB is Running
            var finalJob = (await _jobServiceTest.GetAllJobs()).First(j => j.Id == job.Id);
            Assert.Equal("Running", finalJob.Status);
        }


    }
}