using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOS.Services.DAL;
using OOS.Services.DataModels;
using OOS.Services.Tests.Fakes;

namespace OOS.Services.Tests
{
    [TestClass]
    public class UpdateStoresTests
    {

       
        [TestMethod]
        public void StoreUpdater_ClosedStore()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);

            var closed = model.VimStores.FirstOrDefault(x => x.STORE_ABBR.Equals("DDD"));
            closed.STATUS = "CLOSED";

            model.Compare();

            Assert.AreEqual("DDD", model.UpdateList[0].STORE_ABBREVIATION);
        }


        [TestMethod]
        public void StoreUpdater_UpdatedName()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);

            var UPDATED  = model.VimStores.FirstOrDefault(x => x.STORE_ABBR.Equals("CCC"));
            UPDATED.STORE_NAME = "CANTANKEROUSCOOT";

            model.Compare();

            Assert.AreEqual("CCC", model.UpdateList[0].STORE_ABBREVIATION);
        }

        [TestMethod]
        public void StoreUpdater_NewSageStore()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "EE", STATUS = "new", STORE_NAME = "extraEvent" });

            model.Compare();

            Assert.AreEqual("EEE", model.InsertList[0].STORE_ABBREVIATION);

            var newStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("new", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(newStatus.ID, model.InsertList[0].STATUS_ID);

        }

        [TestMethod]
        public void StoreUpdater_UpdatedStatusFromNewToSoon()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "DD", STATUS = "new", STORE_NAME = "extraEvent" });

            var newStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("new", StringComparison.CurrentCultureIgnoreCase));

            var newish = model.Existing.FirstOrDefault(x => x.STORE_ABBREVIATION == "CCC");
            newish.STATUS_ID = newStatus.ID;

            var soonish = model.VimStores.FirstOrDefault(x => x.STORE_ABBR == "CCC");
            soonish.STATUS = "soon";
            
            model.Compare();



            Assert.AreEqual("CCC", model.UpdateList[0].STORE_ABBREVIATION);

            var soonStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("soon", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(soonStatus.ID, model.UpdateList[0].STATUS_ID);

        }

        [TestMethod]
        public void StoreUpdater_CantFindRegion_ShouldntGetToDatabase()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "FF", STATUS = "new", STORE_NAME = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            ;
        }

        [TestMethod]
        public void StoreUpdater_CantFindStatus_ShouldntGetToDatabase()
        {
            var vim = new FakeVim();
            var repo = new FakeOosRepo();

            var model = new StoreUpdater(repo, vim);
            model.VimStores.Add(new VimStore() { STORE_ABBR = "EEE", REGION = "FF", STATUS = "Hanging", STORE_NAME = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            
        }
    }
}
