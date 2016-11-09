using Icon.Common.DataAccess;
using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using System.Data.SqlClient;
using System.Data;
using Icon.Common.Context;
using Infor.Services.NewItem.Infrastructure;
using Icon.Logging;

namespace Infor.Services.NewItem.Queries
{
    public class GetNewItemsQueryHandler : IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>
    {
        private IRenewableContext<IrmaContext> context;
        private ILogger<GetNewItemsQueryHandler> logger;

        public GetNewItemsQueryHandler(IRenewableContext<IrmaContext> context, ILogger<GetNewItemsQueryHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<NewItemModel> Search(GetNewItemsQuery parameters)
        {
            SqlParameter instanceIdParameter = new SqlParameter("@instanceId", SqlDbType.Int)
            {
                Value = parameters.Instance
            };
            SqlParameter regionCodeParameter = new SqlParameter("@regionCode", SqlDbType.NVarChar)
            {
                Value = parameters.Region
            };
            SqlParameter numberOfItems = new SqlParameter("@numberOfItems", SqlDbType.Int)
            {
                Value = parameters.NumberOfItemsInMessage
            };

            var models = context.Context.Database.SqlQuery<NewItemModel>(
                "EXEC infor.GetNewItems @instanceId, @regionCode, @numberOfItems", 
                instanceIdParameter, 
                regionCodeParameter, 
                numberOfItems)
                .ToList();

            LogResults(models, parameters.Region);

            return models;
        }

        private void LogResults(IEnumerable<NewItemModel> models, string region)
        {
            if(models.Any())
            {
                logger.Info(string.Format("GetNewItemsQuery retrieved {0} items. Region: {1}, ScanCodes: {2}", models.Count(), region, string.Join(",", models.Select(m => m.ScanCode))));
            }
            else
            {
                logger.Info(string.Format("GetNewItemsQuery retrieved {0} items. Region: {1}", 0, region));
            }
        }
    }
}