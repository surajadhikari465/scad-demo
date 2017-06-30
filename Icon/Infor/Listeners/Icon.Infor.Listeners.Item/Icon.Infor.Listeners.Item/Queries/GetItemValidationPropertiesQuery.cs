using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Queries
{
    public class GetItemValidationPropertiesQuery : IQueryHandler<GetItemValidationPropertiesParameters, List<GetItemValidationPropertiesResultModel>>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public GetItemValidationPropertiesQuery(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<GetItemValidationPropertiesResultModel> Search(GetItemValidationPropertiesParameters parameters)
        {
            var items = parameters.Items
                .Where(i => i.ErrorCode == null)
                .Select(i => new ValidateItemModel(i))
                .ToTvp("items", "infor.ValidateItemType");

            using (var context = contextFactory.CreateContext())
            {
                var itemValidationPropertyResults = context.Database.SqlQuery<GetItemValidationPropertiesResultModel>("exec infor.GetItemValidationProperties @items", items).ToList();
                return itemValidationPropertyResults;
            }
        }
    }
}
