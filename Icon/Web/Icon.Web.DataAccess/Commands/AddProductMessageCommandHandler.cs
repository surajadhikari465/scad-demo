using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Commands
{
    public class AddProductMessageCommandHandler : ICommandHandler<AddProductMessageCommand>
    {
        private IconContext context;

        public AddProductMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddProductMessageCommand data)
        {
            SqlParameter inputType = new SqlParameter("updatedItemIDs", SqlDbType.Structured);
            inputType.TypeName = "app.UpdatedItemIDsType";

            var itemIdList = new List<int> { data.ItemId }.ConvertAll(i => new
                {
                    itemID = i
                });

            inputType.Value = itemIdList.ToDataTable();

            string sql = "EXEC app.GenerateItemUpdateMessages @updatedItemIDs";

            context.Database.ExecuteSqlCommand(sql, inputType);
        }
    }
}
