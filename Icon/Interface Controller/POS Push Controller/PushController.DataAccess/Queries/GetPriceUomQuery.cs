using Icon.Framework;
using PushController.DataAccess.Interfaces;

namespace PushController.DataAccess.Queries
{
    public class GetPriceUomQuery : IQuery<UOM>
    {
        public int PriceUomId { get; set; }
    }
}
