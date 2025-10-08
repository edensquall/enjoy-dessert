using Core.Interfaces;
using Infrastructure.Agent.Agents;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
  public class ChatService : IChatService
  {
    private readonly int _shortTermRounds;
    private readonly IChatHistoryRepository<List<ChatMessage>> _chatHistoryRepository;
    private readonly DispatcherAgent _dispatcherAgent;
    private readonly DessertInfoAgent _dessertInfoAgent;
    private readonly OrderSupportAgent _orderSupportAgent;
    public ChatService(IConfiguration config, IChatHistoryRepository<List<ChatMessage>> chatHistoryRepository, DispatcherAgent dispatcherAgent, DessertInfoAgent dessertInfoAgent, OrderSupportAgent orderSupportAgent)
    {
      _orderSupportAgent = orderSupportAgent;
      _dessertInfoAgent = dessertInfoAgent;
      _dispatcherAgent = dispatcherAgent;
      _shortTermRounds = config.GetValue<int>("ChatSettings:MemorySettings:ShortTermRounds");
      _chatHistoryRepository = chatHistoryRepository;
    }

    public async Task<string> AnswerWithHandoffAsync(string question, string token, string userName)
    {
      var messages = await _chatHistoryRepository.GetChatHistoryAsync(token);

      messages.Add(new ChatMessage(ChatRole.User, question));
      messages.Add(new ChatMessage(ChatRole.System,

    string.IsNullOrWhiteSpace(userName)
        ? "User is not logged in"
        : $"User is logged in as {userName}"));

      var workflow = AgentWorkflowBuilder.CreateHandoffBuilderWith(_dispatcherAgent.Agent)
                      .WithHandoffs(_dispatcherAgent.Agent, [_dessertInfoAgent.Agent, _orderSupportAgent.Agent])
                      .WithHandoff(_dessertInfoAgent.Agent, _dispatcherAgent.Agent, "非甜點相關→DispatcherAgent")
                      .WithHandoff(_orderSupportAgent.Agent, _dispatcherAgent.Agent, "非訂單相關→DispatcherAgent")
                      .Build();

      StreamingRun run = await InProcessExecution.StreamAsync(workflow, messages);
      await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

      await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
      {
        if (evt is WorkflowOutputEvent outputEvent)
        {
          messages = (List<ChatMessage>)outputEvent.Data!;
          break;
        }
      }

      TrimChatHistory(messages, _shortTermRounds);

      await _chatHistoryRepository.UpdateChatHistoryAsync(token, messages);

      return messages.Last(m => m.Role == ChatRole.Assistant).Text;
    }

    private void TrimChatHistory(List<ChatMessage> chatMessages, int maxUserRounds)
    {
      int userRoundCount = 0;
      int cutoffIndex = 0;

      for (int i = chatMessages.Count - 1; i >= 0 && userRoundCount < maxUserRounds; i--)
      {
        if (chatMessages[i].Role == ChatRole.User)
        {
          cutoffIndex = i;
          userRoundCount++;
        }
      }

      if (cutoffIndex > 0)
      {
        chatMessages.RemoveRange(0, cutoffIndex);
      }
    }
  }
}