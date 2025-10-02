using Core.Interfaces;

namespace Infrastructure.Services
{
  public class AgentService : IAgentService
  {
    private readonly IGptRagService _gptRagService;
    public AgentService(IGptRagService gptRagService)
    {
      _gptRagService = gptRagService;
    }

    public async Task<string> HandleUserInput(string input, string token)
    {
      return await _gptRagService.AnswerWithRag(input, token);
    }
  }
}