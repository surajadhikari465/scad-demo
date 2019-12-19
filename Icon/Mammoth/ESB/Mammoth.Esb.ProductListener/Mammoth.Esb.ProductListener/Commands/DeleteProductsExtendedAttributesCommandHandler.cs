using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MoreLinq;
using System.Data;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Commands
{
	public class DeleteProductsExtendedAttributesCommandHandler : ICommandHandler<DeleteProductsExtendedAttributesCommand>
	{
		IDbProvider db;

		public DeleteProductsExtendedAttributesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

		public void Execute(DeleteProductsExtendedAttributesCommand data)
        {
			var itemsWithoutExtAttributes = data.Items.Where(x => x.ExtendedAttributes == null || x.ExtendedAttributes.Traits == null || !x.ExtendedAttributes.Traits.Any())
                .Select(x => new {Value = x.GlobalAttributes.ItemID }).ToDataTable();
            
            if(itemsWithoutExtAttributes.Rows.Count > 0)
            {
                this.db.Connection.Execute(
                     sql: "dbo.DeleteItemAttributesExt",
                     param: new { extAttributesItemIds = itemsWithoutExtAttributes },
                     transaction: this.db.Transaction,
                     commandType: CommandType.StoredProcedure);
            }
		}
	}
}
