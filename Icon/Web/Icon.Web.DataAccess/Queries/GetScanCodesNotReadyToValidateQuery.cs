using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetScanCodesNotReadyToValidateQuery : IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>
    {
        private IconContext context;

        public GetScanCodesNotReadyToValidateQuery(IconContext context)
        {
            this.context = context;
        }

        public List<string> Search(GetScanCodesNotReadyToValidateParameters parameters)
        {
            if (!parameters.Items.Any())
            {
                return new List<string>();
            }

            var scanCodes = new SqlParameter("Items", SqlDbType.Structured)
                {
                    TypeName = "app.ItemCanonicalDataType",
                    Value = parameters.Items.Select(item => new
                        {
                            ScanCode = item.ScanCode,
                            ProductDescription = item.ProductDescription,
                            PosDescription = item.PosDescription,
                            PackageUnit = item.PackageUnit,
                            FoodStampEligible = item.FoodStampEligible,
                            PosScaleTare = item.PosScaleTare,
                            RetailSize = item.RetailSize,
                            RetailUom = item.RetailUom,
                            BrandId = item.BrandId,
                            BrowsingId = item.BrowsingId,
                            MerchandiseId = item.MerchandiseId,
                            TaxClassId = item.TaxId,
                            NationalClassId = item.NationalId
                        }).ToList().ToDataTable()
                };

            var scanCodesNotReadyToValidate = context.Database.SqlQuery<string>("exec app.GetScanCodesNotReadyToValidate @Items", scanCodes);

            return scanCodesNotReadyToValidate.ToList();
        }
    }
}
