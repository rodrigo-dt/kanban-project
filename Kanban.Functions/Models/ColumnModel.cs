using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kanban_Functions.Models;

public class ColumnModel
{
    [BsonElement("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("cards")]
    public List<CardModel> Cards { get; set; } = new List<CardModel>();
}