using Core.Interfaces;

namespace Infrastructure.Services
{
  public class AgentService : IAgentService
  {
    private readonly IChatService _chatService;
    public AgentService(IChatService chatRagService)
    {
      _chatService = chatRagService;
    }

    public async Task<string> HandleUserInput(string input, string token)
    {
      return await _chatService.AnswerWithProductPlugin(input, token);
    }
  }
}