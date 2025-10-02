namespace Core.Interfaces
{
    public interface IAgentService
    {
        Task<string> HandleUserInput(string input, string token);
    }
}