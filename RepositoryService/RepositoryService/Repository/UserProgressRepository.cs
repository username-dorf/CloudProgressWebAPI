using RepositoryService.Data;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace RepositoryService.Repository;

public class UserProgressRepository
{
    private readonly DynamoDBContext _context;
    private readonly string _tableName;

    public UserProgressRepository(IAmazonDynamoDB client, IConfiguration configuration)
    {
        _tableName = configuration["DynamoDB:TableName"] ?? "UserProgress";
        _context = new DynamoDBContext(client);
    }

    public async Task SaveProgressAsync(UserProgress progress)
    {
        var config = new DynamoDBOperationConfig { OverrideTableName = _tableName };
        await _context.SaveAsync(progress, config);
    }

    public async Task<UserProgress> GetProgressAsync(int userId, string deviceId)
    {
        var config = new DynamoDBOperationConfig { OverrideTableName = _tableName };
        return await _context.LoadAsync<UserProgress>(userId, deviceId, config);
    }
}



