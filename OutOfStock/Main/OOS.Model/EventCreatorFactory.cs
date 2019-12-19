using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;
using OOSCommon.DataContext;
using OOSCommon.Import;
using OOSCommon.Movement;
using OOSCommon.VIM;

namespace OOS.Model
{
    public class EventCreatorFactory : IEventCreatorFactory
    {
        private IOOSLog logger;
        private IConfigurator config;
        private IOOSEntitiesFactory dbFactory;

        public EventCreatorFactory(ILogService logService, IConfigurator config, IOOSEntitiesFactory dbFactory)
        {
            this.config = config;
            this.logger = logService.GetLogger();
            this.dbFactory = dbFactory;
        }

        public IOOSUpdateReported ForStore(string name)
        {
            STORE store = GetStoreByName(name);
            return GetCreator(store);
        }

        private IOOSUpdateReported GetCreator(STORE store)
        {
            if (store == null)
            {
                var message = string.Format("Store: '{0}' does not exist or has been closed", store);
                logger.Error(message);
                throw new InvalidStoreException(message);
            }

            return new OOSUpdateReported(store, GetValidiationMode(), logger,
                GetVIMRepository(), GetEFConnectionString(), GetMovementRepository());
        }

        private bool GetValidiationMode()
        {
            return config.GetValidationMode();
        }

        private string GetEFConnectionString()
        {
            return config.GetEFConnectionString();
        }

        private IMovementRepository GetMovementRepository()
        {
            string oosMovementServiceName = config.GetMovementServiceName();
            string oosConnectionString = config.GetOOSConnectionString();
            return new MovementRepository(oosConnectionString, oosMovementServiceName, logger);

        }

        private IVIMRepository GetVIMRepository()
        {
            string oosVIMServiceName = config.GetVIMServiceName();
            string oosConnectionString = config.GetOOSConnectionString();

            return new VIMRepository(oosConnectionString, oosVIMServiceName, logger);

        }

        private STORE GetStoreByName(string storeName)
        {
            using (var db = dbFactory.New())
            {
                var openStores = (from s in db.STORE join ss in db.STATUS on s.STATUS_ID equals ss.ID where !ss.STATUS1.Equals("Closed", StringComparison.OrdinalIgnoreCase) select s);
                return openStores.FirstOrDefault(p => p.STORE_NAME == storeName);
            }
        }

        public IOOSUpdateReported ForStoreInRegion(string region, string name)
        {
            STORE store = GetStoreInRegion(region, name);
            return GetCreator(store);
        }


        private STORE GetStoreInRegion(string region, string name)
        {
            using (var db = dbFactory.New())
            {
                var openStoresInRegion = (from s in db.STORE 
                                  join r in db.REGION on s.REGION_ID equals  r.ID
                                  join ss in db.STATUS on s.STATUS_ID equals ss.ID
                                  where !ss.STATUS1.Equals("Closed", StringComparison.OrdinalIgnoreCase) && r.REGION_NAME.Equals(region, StringComparison.OrdinalIgnoreCase)
                                  select s);
                var store = openStoresInRegion.FirstOrDefault(p => p.STORE_NAME == name);
                return store;
            }
        }

        public IOOSUpdateReported ForStoreByAbbreviation(string storeAbbrev)
        {
            var store = GetStoreByAbbreviation(storeAbbrev);
            return GetCreator(store);
        }

        private STORE GetStoreByAbbreviation(string storeAbbrev)
        {
            using (var db = dbFactory.New())
            {
                var openStores = (from s in db.STORE join ss in db.STATUS on s.STATUS_ID equals ss.ID where !ss.STATUS1.Equals("Closed", StringComparison.OrdinalIgnoreCase) select s);
                return openStores.FirstOrDefault(p => p.STORE_ABBREVIATION == storeAbbrev);
            }

        }
    }
}
