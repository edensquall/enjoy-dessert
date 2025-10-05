using Core.Interfaces;
using Infrastructure.AIPlugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Infrastructure.Services
{
  public class ChatService : IChatService
  {
    private readonly IConfiguration _config;
    private readonly IChatCompletionService _chat;
    private readonly Kernel _kernel;
    private readonly int _shortTermRounds;
    private readonly IChatHistoryRepository<ChatHistory> _chatHistoryRepository;
    public ChatService(IConfiguration config, IChatCompletionService chat, Kernel kernel, ProductPlugin productPlugin, IChatHistoryRepository<ChatHistory> chatHistoryRepository)
    {
      _kernel = kernel;
      _chat = chat;
      _config = config;
      _shortTermRounds = _config.GetValue<int>("ChatSettings:MemorySettings:ShortTermRounds");
      _chatHistoryRepository = chatHistoryRepository;

      if (!kernel.Plugins.Contains("ProductPlugin"))
      {
        kernel.Plugins.AddFromObject(productPlugin, "ProductPlugin");
      }
    }

    public async Task<string> AnswerWithProductPlugin(string question, string token)
    {
      var chatHistory = await _chatHistoryRepository.GetChatHistoryAsync(token);

      if (!chatHistory.Any())
      {
        chatHistory.AddSystemMessage(
            "你是Enjoy Dessert甜點店客服，提供甜點種類、熱銷商品及資訊。回答專業簡明禮貌，資訊必查工具，不可臆造。購買請至網頁，不說販售"
        );
      }

      chatHistory.AddUserMessage($"使用者問題:{question}");

      OpenAIPromptExecutionSettings promptExecutionSettings = new()
      {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
      };

      var response = await _chat.GetChatMessageContentAsync(chatHistory, promptExecutionSettings, _kernel);

      string answer = response.Content ?? string.Empty;

      chatHistory.AddAssistantMessage(answer);
      TrimChatHistory(chatHistory, _shortTermRounds);

      await _chatHistoryRepository.UpdateChatHistoryAsync(token, chatHistory);

      return answer;
    }

    private void TrimChatHistory(ChatHistory chatHistory, int maxUserRounds)
    {
      int userRoundCount = 0;
      int cutoffIndex = 1;

      for (int i = chatHistory.Count - 1; i > 0 && userRoundCount < maxUserRounds; i--)
      {
        if (chatHistory[i].Role == AuthorRole.User)
        {
          cutoffIndex = i;
          userRoundCount++;
        }
      }

      if (cutoffIndex > 1)
      {
        int removeCount = cutoffIndex - 1;
        chatHistory.RemoveRange(1, removeCount);
      }
    }
  }
}