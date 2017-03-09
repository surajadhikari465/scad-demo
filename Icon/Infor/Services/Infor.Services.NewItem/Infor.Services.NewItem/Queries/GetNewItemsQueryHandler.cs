﻿using Icon.Common.DataAccess;
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
        private const string sql = @"
                                    DECLARE @ids table (Id nvarchar(13))

                                    DECLARE @instanceIdChar varchar(max) = CAST(@instanceId as varchar)


                                    UPDATE TOP(@numberOfItems) dbo.IconItemChangeQueue
                                   SET InProcessBy = @instanceIdChar
                                       OUTPUT inserted.QID INTO @ids
                                   WHERE(InProcessBy = @instanceIdChar OR InProcessBy IS NULL)

                                        AND ProcessFailedDate IS NULL


                                    SELECT
                                        @regionCode AS Region,
                                        q.QID AS QueueId,
                                        q.Item_Key AS ItemKey,
		                                q.Identifier AS ScanCode,
		                                i.Retail_Sale AS IsRetailSale,
                                        CAST(ii.Default_Identifier AS bit) AS IsDefaultIdentifier,
                                        i.Item_Description AS ItemDescription,
		                                i.POS_Description AS PosDescription,
		                                ib.Brand_Name AS BrandName,
		                                vb.IconBrandId AS IconBrandId,
		                                i.Package_Desc1 AS PackageUnit,
		                                i.Package_Desc2 AS RetailSize,
		                                iu.Unit_Abbreviation AS RetailUom,
		                                i.Food_Stamps AS FoodStampEligible,	
		                                st.SubTeam_Name AS SubTeamName,
                                        CAST(st.Dept_No AS nvarchar(max)) AS SubTeamNumber,
                                        CAST(CASE
                                            WHEN nic.ClassID IS NULL THEN 99999
                                            ELSE nic.ClassID
                                        END AS nvarchar(5)) AS NationalClassCode,
                                        LEFT(tc.TaxClassDesc, 7) AS TaxClassCode,
                                        q.InsertDate AS QueueInsertDate
                                    FROM dbo.IconItemChangeQueue q
                                    JOIN dbo.ItemIdentifier ii on q.Identifier = ii.Identifier
                                    JOIN dbo.Item i on ii.Item_Key = i.Item_Key
                                        and q.Item_Key = i.Item_Key
                                    JOIN dbo.ItemUnit iu on i.Package_Unit_ID = iu.Unit_ID
                                    JOIN dbo.SubTeam st on i.SubTeam_No = st.SubTeam_No
                                    LEFT JOIN dbo.NatItemClass nic on i.ClassID = nic.ClassID
                                    JOIN dbo.ItemBrand ib on i.Brand_ID = ib.Brand_ID
                                    LEFT JOIN dbo.ValidatedBrand vb on i.Brand_ID = vb.IrmaBrandId
                                    JOIN TaxClass tc on i.TaxClassID = tc.TaxClassID
                                    WHERE q.InProcessBy = @instanceIdChar
                                        AND q.ProcessFailedDate IS NULL
                                        AND q.QID in 
			                                (
                                                SELECT Id FROM @ids
                                            )";

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
                sql, 
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