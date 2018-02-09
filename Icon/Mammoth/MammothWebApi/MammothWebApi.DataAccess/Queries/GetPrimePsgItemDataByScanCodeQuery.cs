using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetPrimePsgItemDataByScanCodeQuery : IQuery<IEnumerable<PrimePsgItemStoreDataModel>>
    {
        public IEnumerable<StoreScanCode> StoreScanCodes { get; set; }
    }
}
