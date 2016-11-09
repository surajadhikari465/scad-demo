using Controller.Core.ConsoleExample.Factories;
using Controller.Core.ConsoleExample.Models;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.ConsoleExample.Queries
{
    public class GetNewItemsQueryHandler : IQueryHandler<GetNewItemsParameters, IEnumerable<NewItemModel>>
    {
        private IrmaContextFactory factory;

        public IEnumerable<NewItemModel> Search(GetNewItemsParameters parameters)
        {
            using (var wrapper = factory.Create(parameters.Region))
            {
                return wrapper.Entity.Database.SqlQuery<NewItemModel>(
                    "",
                    parameters.Instance,
                    parameters.Region)
                    .ToList();
            }
        }
    }
}
