using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobServices.Models
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string Payload { get; set; }
        public required string Schedule { get; set; }
        public required string Status { get; set; }
        public required DateTime nextRun { get; set; } 
    }
}
