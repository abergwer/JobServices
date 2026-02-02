using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobServices.Models
{
    public class JobExecution
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string JobName { get; set; }
        public string JobType { get; set; }
        public string Payload { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.Now.ToLocalTime();
        public string InstanceId { get; set; } // Optional: identify which instance ran it
        public string Status { get; set; } // e.g., "Started", "Completed", "Failed"
        public string? ErrorMessage { get; set; } // Optional: error details
    }

}
