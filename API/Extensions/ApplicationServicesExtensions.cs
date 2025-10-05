using API.Errors;
using Core.Interfaces;
using Infrastructure.AIPlugins;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped(typeof(IChatHistoryRepository<>), typeof(ChatHistoryRepository<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddSingleton<ProductPlugin>();
            services.AddSingleton<Kernel>(sp =>
            {
                var builder = Kernel.CreateBuilder();

                builder.AddOpenAIChatCompletion(
                    modelId: config["ChatSettings:OpenAI:ChatModelId"],
                    apiKey: config["ChatSettings:OpenAI:ApiKey"]
                );

#pragma warning disable SKEXP0010
                builder.AddOpenAIEmbeddingGenerator(
                    modelId: config["ChatSettings:OpenAI:EmbeddingModelId"],
                    apiKey: config["ChatSettings:OpenAI:ApiKey"]
                );
#pragma warning restore SKEXP0010

                builder.Plugins.AddFromObject(new ProductPlugin(sp));
                return builder.Build();
            });

            services.AddSingleton<IChatCompletionService>(sp =>
            {
                var kernel = sp.GetRequiredService<Kernel>();
                return kernel.GetRequiredService<IChatCompletionService>();
            });

            services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
            {
                var kernel = sp.GetRequiredService<Kernel>();
                return kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
            });

            return services;
        }
    }
}