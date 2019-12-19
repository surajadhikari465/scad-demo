using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Commands;

namespace OOS.Model
{
    public class StoreSkuUpdater
    {
        private IStoreValidator storeValidator;
        private ISkuCountRepository skuCountRepository;

        public StoreSkuUpdater(ISkuCountRepository skuCountRepository, IStoreValidator storeValidator)
        {
            this.storeValidator = storeValidator;
            this.skuCountRepository = skuCountRepository;
        }

        public void Insert(UpdateStoreSkuCommand updateSkuCommand)
        {
            storeValidator.Validate(updateSkuCommand.StoreAbbreviation);
            var store = updateSkuCommand.StoreAbbreviation;
            var team = updateSkuCommand.Team;
            var count = updateSkuCommand.SKUCount;
            skuCountRepository.Insert(store, team, count);
        }

        
    }
}
