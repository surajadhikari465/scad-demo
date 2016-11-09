using Controller.Core.Filters;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.ConsoleExample.Filters
{
    public class GetNewItemsFilter : FilterBase<RegionalControllerPipelineParameters>
    {
        private IQueryHandler<GetNewItemsParameters, NewItemModel> getNewItemsQueryHandler;

        public override void Execute(RegionalControllerPipelineParameters parameters)
        {
            
        }
    }
}
