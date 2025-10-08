using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Infrastructure.Agent.Agents
{
    public class DispatcherAgent
    {
        public AIAgent Agent { get; init; }
        public DispatcherAgent(IChatClient chatClient)
        {
            Agent = new ChatClientAgent(chatClient,
                name: "DispatcherAgent",
                description: "分派客服代理",
                instructions: @"只判斷問題應交誰：
甜點名稱、種類、介紹、價格、熱銷商品相關→DessertInfoAgent,
訂單狀態、查訂單等相關→OrderSupportAgent。
若無相關代理→直接回『此問題不在客服服務範圍內』，不得呼叫任何代理或 handoff。
不得回答內容或生成回覆。");
        }
    }
}