using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kanban_Functions.Models;

public class BoardModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("columns")]
    public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
}