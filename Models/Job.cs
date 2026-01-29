namespace JobServices.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string Payload { get; set; }
        public required string Schedule { get; set; }
        public required string Status { get; set; }
    }
}
