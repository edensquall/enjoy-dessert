using Infrastructure.Agent.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Infrastructure.Agent.Agents
{
    public class DessertInfoAgent
    {
        public AIAgent Agent { get; init; }

        public DessertInfoAgent(IChatClient chatClient, ProductTool productTool)
        {
            Agent = new ChatClientAgent(chatClient,
                name: "DessertInfoAgent",
                description: "甜點資訊客服",
                instructions: @"只回答甜點資訊。依 ResultContract<T>:
Success=true→Data,
Success=false→Message。
查無資料→回『查無相關甜點資訊』。
不得臆測、不得回答非甜點問題。
不得提供或建議訂購，回答只包含甜點資訊本身。",
                tools: [AIFunctionFactory.Create(productTool.GetProducts),
                AIFunctionFactory.Create(productTool.GetProduct),
                AIFunctionFactory.Create(productTool.GetBestseller),
                AIFunctionFactory.Create(productTool.GetProductTypes)]
                );
        }
    }
}