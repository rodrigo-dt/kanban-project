using MongoDB.Bson.Serialization.Attributes;

namespace Kanban_Functions.Models;

public class CardModel
{
    [BsonElement("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
    [BsonElement("priority")]
    public int Priority { get; set; }
    
    [BsonElement("due_date")]
    public DateTime? DueDate { get; set; }
    
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("color")]
    public string Color { get; set; }
    
    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new List<string>();
}