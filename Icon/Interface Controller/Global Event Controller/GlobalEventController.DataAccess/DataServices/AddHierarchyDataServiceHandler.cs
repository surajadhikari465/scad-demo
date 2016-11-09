using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.DataServices
{
    public class AddHierarchyDataServiceHandler : IDataService<AddHierarchyDataService>
    {
        private ICommandHandler<BulkAddBrandCommand> bulkBrandHandler;

        public AddHierarchyDataServiceHandler(ICommandHandler<BulkAddBrandCommand> bulkBrandHandler)
        {
            this.bulkBrandHandler = bulkBrandHandler;
        }

        public void Process(AddHierarchyDataService service)
        {
            BulkAddBrandCommand bulkBrandCommand = service.BulkBrandCommand;
            bulkBrandHandler.Handle(bulkBrandCommand);
        }
    }
}
