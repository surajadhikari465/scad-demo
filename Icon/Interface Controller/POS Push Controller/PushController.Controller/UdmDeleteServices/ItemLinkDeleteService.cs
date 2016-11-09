using Icon.Common.DataAccess;
using PushController.Common;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushController.Controller.UdmDeleteServices
{
    public class ItemLinkDeleteService : IUdmDeleteService<ItemLinkModel>
    {
        private ICommandHandler<DeleteItemLinksCommand> deleteItemLinksCommandHandler;

        public ItemLinkDeleteService(ICommandHandler<DeleteItemLinksCommand> deleteItemLinksCommandHandler)
        {
            this.deleteItemLinksCommandHandler = deleteItemLinksCommandHandler;
        }

        public void DeleteEntitiesBulk(List<ItemLinkModel> entities)
        {
            deleteItemLinksCommandHandler.Execute(new DeleteItemLinksCommand { ItemLinks = entities });
        }

        public void DeleteEntitiesRowByRow(List<ItemLinkModel> entities)
        {
            foreach (var itemLink in entities)
            {
                deleteItemLinksCommandHandler.Execute(new DeleteItemLinksCommand { ItemLinks = new List<ItemLinkModel> { itemLink } });
            }
        }
    }
}
