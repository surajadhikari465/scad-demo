using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using Warp.ProcessPrices.Common;
using Warp.ProcessPrices.DataAccess.Commands;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

// Warp.ProcessPrices::Warp.ProcessPrices.Lambda.Function::FunctionHandler
namespace Warp.ProcessPrices.Lambda
{
    public class Function
    {
        private static ServiceProvider serviceProvider;

        public Function()
        {
            LambdaLogger.Log("call constructor");
        }

        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Func<SQSEvent, ILambdaContext, string> func = FunctionHandler;
            using var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer());
            using var bootstrap = new LambdaBootstrap(handlerWrapper);
            await bootstrap.RunAsync();
        }

        /// <summary>
        /// 
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="inputEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FunctionHandler(SQSEvent inputEvent, ILambdaContext context)
        {
            if (serviceProvider == null)
            {
                var serviceCollection = new ServiceCollection();
                serviceProvider = ConfigureServiceProvider(serviceCollection);
            }

            var processor = serviceProvider.GetService<ISqsMessageProcessor>();
             
            foreach (var record in inputEvent.Records)
                processor.Process(record, context);

            Extensions.DebugInformation(context, inputEvent);

            return "done!";
        }
        private static ServiceProvider ConfigureServiceProvider(ServiceCollection serviceCollection)
        {
            var databaseSecretName = Environment.GetEnvironmentVariable("DatabaseSecretName");
            if (string.IsNullOrEmpty(databaseSecretName)) throw new ArgumentNullException("DatabaseSecretName", "DatabaseSecretName environment variable not found");
            
            DbSecretManager.Initialize();
            var connectionString = DbSecretManager.GetConnectionString(databaseSecretName);
            
            return serviceCollection
                .AddCommandQueryHandlers(typeof(ICommandHandler<>))
                .AddScoped<IDbConnection>(provider => new NpgsqlConnection(connectionString))
                .AddTransient<ISqsToPriceMapper, SqsToPriceMapper>()
                .AddTransient<ISqsMessageProcessor, SqsMessageProcessor>()
                .BuildServiceProvider();
        }
    }


 

  


}
