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
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);

            var closed = model.ModifiedStores.FirstOrDefault(x => x.tlc.Equals("DDD"));
            closed.status = "closed";

            model.Compare();

            Assert.AreEqual("DDD", model.UpdateList[0].STORE_ABBREVIATION);
        }


        [TestMethod]
        public void StoreUpdater_UpdatedName()
        {
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);

            var UPDATED  = model.ModifiedStores.FirstOrDefault(x => x.tlc.Equals("CCC"));
            UPDATED.name = "CANTANKEROUSCOOT";

            model.Compare();

            Assert.AreEqual("CCC", model.UpdateList[0].STORE_ABBREVIATION);
        }

        [TestMethod]
        public void StoreUpdater_NewSageStore()
        {
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);
            model.ModifiedStores.Add(new SageStore { tlc = "EEE", region = "EE", status = "new", name = "extraEvent" });

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
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);
            model.ModifiedStores.Add(new SageStore { tlc = "EEE", region = "DD", status = "new", name = "extraEvent" });

            var newStatus =
                model.StoreStatuses.FirstOrDefault(
                    x => x.STATUS.Equals("new", StringComparison.CurrentCultureIgnoreCase));

            var newish = model.Existing.FirstOrDefault(x => x.STORE_ABBREVIATION == "CCC");
            newish.STATUS_ID = newStatus.ID;

            var soonish = model.ModifiedStores.FirstOrDefault(x => x.tlc == "CCC");
            soonish.status = "soon";
            
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
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);
            model.ModifiedStores.Add(new SageStore { tlc = "EEE", region = "FF", status = "new", name = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            ;
        }

        [TestMethod]
        public void StoreUpdater_CantFindStatus_ShouldntGetToDatabase()
        {
            var sage = new FakeSage();
            var repo = new FakeOosRepo();

            var model = new StoreUpdaterSage(repo, sage);
            model.ModifiedStores.Add(new SageStore { tlc = "EEE", region = "FF", status = "Hanging", name = "extraEvent" });
            model.Compare();
            Assert.IsFalse(model.InsertList.Any(x => x.STORE_ABBREVIATION == "EEE"));
            
        }
    }
}
