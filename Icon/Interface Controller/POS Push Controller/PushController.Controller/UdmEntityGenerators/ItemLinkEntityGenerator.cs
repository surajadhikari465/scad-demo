using Icon.Framework;
using PushController.Common;
using PushController.Controller.UdmEntityBuilders;
using PushController.Controller.UdmUpdateServices;
using System;
using System.Collections.Generic;

namespace PushController.Controller.UdmEntityGenerators
{
    public class ItemLinkEntityGenerator : IUdmEntityGenerator<ItemLinkModel>
    {
        private IUdmEntityBuilder<ItemLinkModel> entityBuilder;
        private IUdmUpdateService<ItemLinkModel> updateService;

        public ItemLinkEntityGenerator(
            IUdmEntityBuilder<ItemLinkModel> entityBuilder,
            IUdmUpdateService<ItemLinkModel> updateService)
        {
            this.entityBuilder = entityBuilder;
            this.updateService = updateService;
        }

        public List<ItemLinkModel> BuildEntities(List<IRMAPush> posDataReadyForUdm)
        {
            return entityBuilder.BuildEntities(posDataReadyForUdm);
        }

        public void SaveEntities(List<ItemLinkModel> entitiesToSave)
        {
            try
            {
                updateService.SaveEntitiesBulk(entitiesToSave);
            }
            catch (Exception)
            {
                SaveEntitiesRowByRow(entitiesToSave);
            }
        }

        private void SaveEntitiesRowByRow(List<ItemLinkModel> entitiesToSave)
        {
            updateService.SaveEntitiesRowByRow(entitiesToSave);
        }
    }
}
