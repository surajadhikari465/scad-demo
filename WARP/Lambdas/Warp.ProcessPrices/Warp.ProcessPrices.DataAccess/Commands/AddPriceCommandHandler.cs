using System.Data;
using Dapper;

namespace Warp.ProcessPrices.DataAccess.Commands
{
    public class AddPriceCommandHandler : ICommandHandler<AddPriceCommand>
    {
        private readonly IDbConnection connection;

        public AddPriceCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(AddPriceCommand data)
        {
            const string sql = @"gpm.addprice";

            var parameters = new Parameters();
            parameters.Add("@gpm_id", data.Price.GpmId);
            parameters.Add("@region", data.Price.Region);
            parameters.Add("@scan_code", data.Price.ScanCode);
            parameters.Add("@item_id", data.Price.ItemId);
            parameters.Add("@business_unit_id", data.Price.BusinessUnitId);
            parameters.Add("@start_date", data.Price.StartDate);
            parameters.Add("@end_date", data.Price.EndDate);
            parameters.Add("@price", data.Price.Price);
            parameters.Add("@percent_off", data.Price.PercentOff);
            parameters.Add("@price_type", data.Price.PriceType);
            parameters.Add("@price_type_attribute", data.Price.PriceTypeAttribute);
            parameters.Add("@sellable_uom", data.Price.SellableUom);
            parameters.Add("@currency_code", data.Price.CurrencyCode);
            parameters.Add("@multiple", data.Price.Multiple);
            parameters.Add("@tag_expiration_date", data.Price.TagExpirationDate);

            var values = connection.Query<int>(sql, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}