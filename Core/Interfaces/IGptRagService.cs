namespace Core.Interfaces
{
    public interface IGptRagService
    {
        Task<string> AnswerWithRag(string question, string token);
    }
}