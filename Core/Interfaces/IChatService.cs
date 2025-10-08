namespace Core.Interfaces
{
    public interface IChatService
    {
        Task<string> AnswerWithHandoffAsync(string question, string token, string userName);
    }
}