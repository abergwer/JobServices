namespace JobServices.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public string scedule { get; set; }
        public string Status { get; set; }
    }
}
