using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Amazon.S3;
using Amazon.SQS;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Shared.DataAccess.Dapper.Decorators;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.MessageParsers;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using Mammoth.Esb.HierarchyClassListener.Services;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container Initialize()
        {
            var container = new Container();

            var listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();

            container.Register<MammothHierarchyClassListener>();
            container.RegisterSingleton(() => listenerSettings);
            container.RegisterSingleton(() => DvsClientUtil.GetS3Client(listenerSettings));
            container.RegisterSingleton(() => DvsClientUtil.GetSqsClient(listenerSettings));
            container.RegisterSingleton<IDvsSubscriber, DvsSqsSubscriber>();

            container.RegisterSingleton(() => EmailClientSettings.CreateFromConfig());
            container.RegisterSingleton<IEmailClient, EmailClient>();
            container.RegisterSingleton<ILogger<MammothHierarchyClassListener>, NLogLogger<MammothHierarchyClassListener>>();
            container.RegisterSingleton<IMessageParser<List<HierarchyClassModel>>, HierarchyClassMessageParser>();
            container.Register<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>, AddOrUpdateHierarchyClassService>();
            container.Register<IHierarchyClassService<DeleteBrandRequest>, DeleteBrandService>();
            container.Register<IHierarchyClassService<DeleteMerchandiseClassRequest>, DeleteMerchandiseClassService>();
            container.Register<IHierarchyClassService<DeleteNationalClassRequest>, DeleteNationalClassService>();
            container.Register<IHierarchyClassService<DeleteManufacturerRequest>, DeleteManufacturerService>();

            container.Register<ICommandHandler<AddOrUpdateHierarchyClassesCommand>, AddOrUpdateHierarchyClassesCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>, AddOrUpdateMerchandiseHierarchyLineageCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateNationalHierarchyLineageCommand>, AddOrUpdateNationalHierarchyLineageCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>, AddOrUpdateFinancialHierarchyClassCommandHandler>();
            container.Register<ICommandHandler<DeleteBrandsParameter>, DeleteBrandsCommandHandler>();
            container.Register<ICommandHandler<DeleteMerchandiseClassParameter>, DeleteMerchandiseClassCommandHandler>();
            container.Register<ICommandHandler<DeleteNationalClassParameter>, DeleteNationalClassCommandHandler>();
            container.Register<ICommandHandler<DeleteManufacturerParameter>, DeleteManufacturerCommandHandler>();
            container.Register<IQueryHandler<GetItemsByBrandIdParameter, IEnumerable<Item>>, GetItemsByBrandIdQueryHandler>();
            container.Register<IQueryHandler<GetItemsByMerchandiseHierarchyIdParameter, IEnumerable<Item>>, GetItemsByMerchandiseClassIdQueryHandler>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(IHierarchyClassService<IHierarchyClassRequest>), typeof(ValidateItemAssociationForDeleteBrandDecorator));
            container.RegisterDecorator(typeof(IHierarchyClassService<IHierarchyClassRequest>), typeof(ValidateItemAssociationForDeleteMerchandiseHierarchyDecorator));

            Registration amazonS3Registration = container.GetRegistration(typeof(IAmazonS3)).Registration;
            amazonS3Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonS3 is taken care of by the application.");
            Registration amazonSqsRegistration = container.GetRegistration(typeof(IAmazonSQS)).Registration;
            amazonSqsRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonSQS is taken care of by the application.");

            container.Verify();
            return container;
        }
    }
}
