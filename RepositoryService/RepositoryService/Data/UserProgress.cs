using Amazon.DynamoDBv2.DataModel;

namespace RepositoryService.Data;

public class UserProgress
{
    [DynamoDBHashKey]  // Primary Key
    public int UserId { get; set; }

    [DynamoDBRangeKey] // Sort Key
    public string DeviceId { get; set; }

    public string ProgressJson { get; set; }

    public string AppVersion { get; set; }

    public DateTime LastUpdated { get; set; }
}