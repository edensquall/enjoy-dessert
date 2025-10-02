using System.Text.Json;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class ChatHistoryRepository<T> : IChatHistoryRepository<T> where T : new()
    {
        private readonly IDatabase _database;
        public ChatHistoryRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteChatHistoryAsync(string chatToken)
        {
            return await _database.KeyDeleteAsync(chatToken);
        }

        public async Task<T?> GetChatHistoryAsync(string chatToken)
        {
            var data = await _database.StringGetAsync(chatToken);

            return data.IsNullOrEmpty ? new T() : JsonSerializer.Deserialize<T>(data);
        }

        public async Task<T?> UpdateChatHistoryAsync(string chatToken, T chatHistory)
        {
            var created = await _database.StringSetAsync(chatToken,
                JsonSerializer.Serialize(chatHistory), TimeSpan.FromMinutes(30));

            if (!created) return new T();

            return await GetChatHistoryAsync(chatToken);
        }
    }
}