using System.Data;
using System.Data.SqlClient;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Interfaces;
using BrandUploadProcessor.DataAccess.Commands;
using BrandUploadProcessor.DataAccess.Queries;
using BrandUploadProcessor.Service.ExcelParsing;
using BrandUploadProcessor.Service.ExcelParsing.Interfaces;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Mappers.Interfaces;
using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Logging;
using SimpleInjector;
using NLog;

namespace BrandUploadProcessor.Service
{
    public class SimpleInjectorInitializer
    {
        public Container Initialize()
        {
            var container = new Container();

            container.Register<IServiceConfiguration>(ServiceConfiguration.LoadFromAppSettings);
            container.Register<IService, BrandUploadProcessorService>();
            container.Register<IBrandUploadManager, BrandUploadManager>();
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));


            container.Register<IDbConnection>(() => new SqlConnection(container.GetInstance<IServiceConfiguration>().IconConnectionString), Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), typeof(GetBrandUploadsQueryHandler).Assembly, Lifestyle.Singleton);
            container.Register(typeof(ICommandHandler<>), typeof(SetStatusCommandHandler).Assembly);

            container.Register<IExcelWorksheetParser, ExcelWorksheetParser>();
            container.Register<IExcelWorksheetHeadersParser, ExcelWorksheetHeadersParser>();
            container.Register<IExcelRowParser, ExcelRowParser>();
            container.Register<IExcelWorkbookValidator,ExcelWorkbookValidator>();
            
            container.Register<IAddUpdateBrandManager, AddUpdateBrandManager>();
            container.Register<IColumnHeadersValidator, ColumnHeadersValidator>();
            container.Register<IErrorMessageBuilder, ErrorMessageBuilder>();
            container.Register<IRowObjectToAddBrandModelMapper, RowObjectToAddBrandModelMapper>();
            container.Register<IRowObjectToUpdateBrandModelMapper, RowObjectToUpdateBrandModelMapper>();
            container.Register<IAttributeErrorMessageMapper, AttributeErrorMessageMapper>();
            container.Register<IRegexTextValidator, RegexTextValidator>();
            container.Register<IRowObjectsValidator, RowObjectsValidator>();

            container.Register<IBrandsCache, BrandsCache>(Lifestyle.Singleton);

            container.Verify();
            return container;
        }
    }
}