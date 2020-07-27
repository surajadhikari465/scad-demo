using System;
using System.Linq;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Warp.ProcessPrices.Lambda
{
    public static class Extensions
    {
        public static IServiceCollection AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(Warp.ProcessPrices.DataAccess.Commands.ICommandHandler<>).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
                );

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
            }

            return services;
        }

        public static void DebugInformation(ILambdaContext context, SQSEvent inputEvent)
        {
            LambdaLogger.Log("ENVIRONMENT VARIABLES: " + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariables(), Formatting.Indented));
            LambdaLogger.Log("CONTEXT: " + JsonConvert.SerializeObject(context, Formatting.Indented));
            LambdaLogger.Log("EVENT: " + JsonConvert.SerializeObject(inputEvent, Formatting.Indented));
        }
    }
}
