using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace RepositoryService.Data;
using System.Text.Json;

public class SaveProgressRequest
{
    public string DeviceId { get; set; }
    public JsonElement Progress { get; set; } 
    public string AppVersion { get; set; }
}


