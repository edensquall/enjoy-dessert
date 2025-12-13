using System.Text.Json;
using Core.Interfaces;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class ChatHistoryRepository<T> : IChatHistoryRepository<T> where T : new()
    {
        private readonly IDatabase _database;
        private readonly int _shortTermMinutes;
        public ChatHistoryRepository(IConnectionMultiplexer redis, IConfiguration config)
        {
            _database = redis.GetDatabase();
            _shortTermMinutes = config.GetValue<int>("ChatSettings:MemorySettings:ShortTermMinutes", 10);
        }

        public async Task<bool> DeleteChatHistoryAsync(string chatToken)
        {
            return await _database.KeyDeleteAsync(chatToken);
        }

        public async Task<T> GetChatHistoryAsync(string chatToken)
        {
            var data = await _database.StringGetAsync(chatToken);

            if (data.IsNullOrEmpty)
                return new T();

            var result = JsonSerializer.Deserialize<T>((string)data!);
            return result ?? new T();
        }

        public async Task<T> UpdateChatHistoryAsync(string chatToken, T chatHistory)
        {
            var created = await _database.StringSetAsync(chatToken,
                JsonSerializer.Serialize(chatHistory), TimeSpan.FromMinutes(_shortTermMinutes));

            if (!created) return new T();

            return await GetChatHistoryAsync(chatToken);
        }
    }
}