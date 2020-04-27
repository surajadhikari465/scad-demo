using System.Data;
using System.Linq;
using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Logging;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class PublishBrandUpdatesCommandHandler : ICommandHandler<PublishBrandUpdatesCommand>
    {
        private readonly IDbConnection dbConnection;
        private readonly ILogger<PublishBrandUpdatesCommandHandler> logger;
        

        public PublishBrandUpdatesCommandHandler(IDbConnection dbConnection, ILogger<PublishBrandUpdatesCommandHandler> logger)
        {
            this.dbConnection = dbConnection;
            this.logger = logger;
        }

        public void Execute(PublishBrandUpdatesCommand data)
        {
            logger.Info($"Refreshing {data.BrandIds.Count} brand(s) for regions: [ { string.Join(",",data.Regions)} ]");
            var ids = data.BrandIds.Select(brandId => new
            {
                I = brandId
            }).ToDataTable().AsTableValuedParameter("[app].[IntList]");

            var regions = data.Regions.Select(region => new
            {
                RegionAbbr = region
            }).ToDataTable().AsTableValuedParameter("app.RegionAbbrType");

            dbConnection.Execute("app.RefreshBrands @ids, @regions", new {ids, regions});

        }
    }
}