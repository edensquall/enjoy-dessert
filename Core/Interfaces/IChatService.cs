namespace Core.Interfaces
{
    public interface IChatService
    {
        Task<string> AnswerWithProductPlugin(string question, string token);
    }
}