using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Models;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Commands
{
    public class ValidateItemsCommandHandler : ICommandHandler<ValidateItemsCommand>
    {
        private IRenewableContext<IconContext> globalContext;

        public ValidateItemsCommandHandler(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(ValidateItemsCommand data)
        {
            var items = data.Items
                .Where(i => i.ErrorCode == null)
                .Select(i => new ValidateItemModel(i))
                .ToTvp("items", "infor.ValidateItemType");

            var validateItemsResult = globalContext.Context.Database.SqlQuery<ValidateItemResultModel>("exec infor.ValidateItems @items", items).ToList();

            var errorItems = data.Items.Join(
                validateItemsResult,
                i => i.ItemId,
                v => v.ItemId,
                (i, v) => new
                {
                    Item = i,
                    ValidationErrorCode = v.ErrorCode,
                    ValidationErrorDetails = v.ErrorDetails
                })
                .Where(i => i.ValidationErrorCode != null);

            foreach (var item in errorItems)
            {
                item.Item.ErrorCode = item.ValidationErrorCode;
                item.Item.ErrorDetails = item.ValidationErrorDetails;
            }
        }
    }
}
