using Icon.Framework;
using PushController.Common;
using PushController.Controller.UdmEntityBuilders;
using PushController.Controller.UdmUpdateServices;
using System;
using System.Collections.Generic;

namespace PushController.Controller.UdmEntityGenerators
{
    public class ItemPriceEntityGenerator : IUdmEntityGenerator<ItemPriceModel>
    {
        private IUdmEntityBuilder<ItemPriceModel> entityBuilder;
        private IUdmUpdateService<ItemPriceModel> updateService;

        public ItemPriceEntityGenerator(
            IUdmEntityBuilder<ItemPriceModel> entityBuilder,
            IUdmUpdateService<ItemPriceModel> updateService)
        {
            this.entityBuilder = entityBuilder;
            this.updateService = updateService;
        }

        public List<ItemPriceModel> BuildEntities(List<IRMAPush> posDataReadyForUdm)
        {
            return entityBuilder.BuildEntities(posDataReadyForUdm);
        }

        public void SaveEntities(List<ItemPriceModel> entitiesToSave)
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

        private void SaveEntitiesRowByRow(List<ItemPriceModel> entitiesToSave)
        {
            updateService.SaveEntitiesRowByRow(entitiesToSave);
        }
    }
}
