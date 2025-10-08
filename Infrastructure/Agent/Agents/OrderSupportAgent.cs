using Infrastructure.Agent.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Infrastructure.Agent.Agents
{
    public class OrderSupportAgent
    {
        public AIAgent Agent { get; init; }
        public OrderSupportAgent(IChatClient chatClient, OrderTool orderTool)
        {
            Agent = new ChatClientAgent(chatClient,
                name: "OrderSupportAgent",
                description: "訂單查詢客服",
                instructions: @"只回答訂單查詢。
若 system message 顯示使用者未登入→回『請先登入以查詢訂單』，不得提供登入協助。
否則依 ResultContract<T>:
Success=true→Data, 
Success=false→Message。
不得臆測、不得回答非訂單問題。",
                tools: [AIFunctionFactory.Create(orderTool.GetCurrentUserOrders),
                AIFunctionFactory.Create(orderTool.GetCurrentUserOrder)]
                );
        }
    }
}