namespace JobServices.Models
{
    public class CreateJobRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public string Schedule { get; set; }
    }
}
