using Core.Interfaces;

namespace Infrastructure.Services
{
  public class AgentService : IAgentService
  {
    private readonly IChatService _chatService;
    public AgentService(IChatService chatService)
    {
      _chatService = chatService;
    }

    public async Task<string> HandleUserInput(string input, string token)
    {
      return await _chatService.AnswerWithProductPlugin(input, token);
    }
  }
}