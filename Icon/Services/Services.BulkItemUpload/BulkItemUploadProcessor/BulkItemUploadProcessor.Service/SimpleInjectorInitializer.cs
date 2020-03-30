using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Builder;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.DataAccess.Decorators;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.BulkUpload;
using BulkItemUploadProcessor.Service.Cache;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.ExcelParsing;
using BulkItemUploadProcessor.Service.ExcelParsing.Interfaces;
using BulkItemUploadProcessor.Service.Interfaces;
using BulkItemUploadProcessor.Service.Mappers;
using BulkItemUploadProcessor.Service.Mappers.Interfaces;
using BulkItemUploadProcessor.Service.Validation;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Common.Validators.ItemAttributes;
using Icon.Logging;
using OfficeOpenXml;
using SimpleInjector;
using System.Data;
using System.Data.SqlClient;

namespace BulkItemUploadProcessor.Service
{
    public class SimpleInjectorInitializer
    {
        public Container Initialize()
        {
            var  container = new Container();

            container.Register<IServiceConfiguration>(ServiceConfiguration.LoadFromAppSettings);
            container.Register<IService, BulkUploadProcessorService>();
            container.Register<IBulkUploadManager, BulkUploadManager>();
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register<IDbConnection>(() => new SqlConnection(container.GetInstance<IServiceConfiguration>().IconConnectionString), Lifestyle.Singleton);

            container.Register(typeof(IQueryHandler<,>), typeof(GetBarcodeTypesQueryHandler).Assembly, Lifestyle.Singleton);
            container.Register(typeof(ICommandHandler<>), typeof(SetStatusCommandHandler).Assembly, Lifestyle.Singleton);

            container.Register(typeof(IHierarchyCache), typeof(HierarchyCache), Lifestyle.Singleton);

            container.Register(typeof(IMerchItemPropertiesCache), typeof(MerchItemPropertiesCache), Lifestyle.Singleton);

            container.Register<IValidatorFactory, SimpleInjectorValidatorFactory>(Lifestyle.Singleton);

            container.Register<IItemAttributesValidatorFactory, ItemAttributesValidatorFactory>();
            
            container.Register<IValidationManager, ValidationManager>();
            container.Register<IErrorMessageBuilder, ErrorMessageBuilder>();
            container.Register<IAddUpdateItemManager,AddUpdateItemManager>();
            container.Register<IValidator<ExcelPackage>, ExcelPackageValidator>();
            container.Register<IHierarchyValidator, HierarchyValidator>();
            container.Register<IRowObjectsValidator, RowObjectsValidator>();
            container.Register<IColumnHeadersValidator, ColumnHeadersValidator>();
            container.Register<IRowObjectToUpdateItemModelMapper, RowObjectToUpdateItemModelMapper>();
            container.Register<IExcelRowParser, ExcelRowParser>();
            container.Register<IExcelWorksheetParser, ExcelWorksheetParser>();
            container.Register<IExcelWorksheetHeadersParser, ExcelWorksheetHeadersParser>();
            container.Register<IRowObjectToAddItemModelMapper, RowObjectToAddItemModelMapper>();
            container.Register<IExcelWorkbookValidator, ExcelWorkbookValidator>();

            container.RegisterDecorator<ICommandHandler<AddItemsCommand>, TransactionCommandHandlerDecorator<AddItemsCommand>>();

            container.RegisterDecorator<ICommandHandler<UpdateItemsCommand>, TransactionCommandHandlerDecorator<UpdateItemsCommand>>();

            container.Register(() => BulkItemUploadProcessorSettings.Load());

            container.Verify();
            return container;
        }
    }
}