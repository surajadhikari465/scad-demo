using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.TransferObjects;

namespace WebSupport.DataAccess.Queries
{
    public class GetStoresForRegionQuery : IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>
    {
        private MammothContext context;

        public GetStoresForRegionQuery(MammothContext context)
        {
            this.context = context;
        }

        public IList<StoreTransferObject> Search(GetStoresForRegionParameters parameters)
        {
            return context.Database
                .SqlQuery<StoreTransferObject>($@"
                    SELECT 
                        CAST(BusinessUnitID AS NVARCHAR(10)) AS BusinessUnit, 
                        StoreAbbrev AS Abbreviation, 
                        StoreName AS Name 
                    FROM dbo.Locale 
                    WHERE Region = '{parameters.Region}'
                        AND (LocaleCloseDate IS NULL OR LocaleCloseDate > CAST(GETDATE() as DATE))
                    ORDER BY BusinessUnit")
                .ToList();
        }
    }
}
