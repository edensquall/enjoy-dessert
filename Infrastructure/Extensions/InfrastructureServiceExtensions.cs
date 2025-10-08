using System.Reflection;
using Core.Interfaces;
using Infrastructure.Agent.Agents;
using Infrastructure.Agent.Tools;
using Infrastructure.Data;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;

namespace Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var infraAssembly = Assembly.GetExecutingAssembly();
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(infraAssembly);
            services.AddSingleton(typeAdapterConfig);

            services.AddScoped<IMapper, ServiceMapper>();

            services.AddSingleton<IChatClient>(provider =>
            {
                return new OpenAIClient(config["ChatSettings:OpenAI:ApiKey"])
                    .GetChatClient(config["ChatSettings:OpenAI:ChatModelId"])
                    .AsIChatClient();
            });

            services.AddScoped(typeof(IChatHistoryRepository<>), typeof(ChatHistoryRepository<>));

            services.AddScoped<DispatcherAgent>();
            services.AddScoped<DessertInfoAgent>();
            services.AddScoped<OrderSupportAgent>();

            services.AddScoped<ProductTool>();
            services.AddScoped<OrderTool>();
            
            return services;
        }
    }
}