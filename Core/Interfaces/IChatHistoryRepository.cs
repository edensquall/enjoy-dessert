namespace Core.Interfaces
{
    public interface IChatHistoryRepository<T>
    {
        Task<T?> GetChatHistoryAsync(string chatToken);
        Task<T?> UpdateChatHistoryAsync(string chatToken, T chatHistory);
        Task<bool> DeleteChatHistoryAsync(string chatToken);
    }
}