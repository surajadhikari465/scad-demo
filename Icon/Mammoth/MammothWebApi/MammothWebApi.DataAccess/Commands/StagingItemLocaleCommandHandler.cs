using FastMember;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleCommandHandler : ICommandHandler<StagingItemLocaleCommand>
    {
        private IDbProvider db;

        public StagingItemLocaleCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingItemLocaleCommand data)
        {
            if (data.ItemLocales.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
                {
                    using (var reader = ObjectReader.Create(
                        data.ItemLocales,
                        nameof(StagingItemLocaleModel.Region),
                        nameof(StagingItemLocaleModel.BusinessUnitID),
                        nameof(StagingItemLocaleModel.ScanCode),
                        nameof(StagingItemLocaleModel.Discount_Case),
                        nameof(StagingItemLocaleModel.Discount_TM),
                        nameof(StagingItemLocaleModel.Restriction_Age),
                        nameof(StagingItemLocaleModel.Restriction_Hours),
                        nameof(StagingItemLocaleModel.Authorized),
                        nameof(StagingItemLocaleModel.Discontinued),
                        nameof(StagingItemLocaleModel.LabelTypeDesc),
                        nameof(StagingItemLocaleModel.LocalItem),
                        nameof(StagingItemLocaleModel.Product_Code),
                        nameof(StagingItemLocaleModel.RetailUnit),
                        nameof(StagingItemLocaleModel.Sign_Desc),
                        nameof(StagingItemLocaleModel.Locality),
                        nameof(StagingItemLocaleModel.Sign_RomanceText_Long),
                        nameof(StagingItemLocaleModel.Sign_RomanceText_Short),
                        nameof(StagingItemLocaleModel.Msrp),
                        nameof(StagingItemLocaleModel.Timestamp),
                        nameof(StagingItemLocaleModel.TransactionId)))
                    {
                        bulkCopy.DestinationTableName = "[stage].[ItemLocale]";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
