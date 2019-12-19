using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using OOSCommon.DataContext;

namespace OutOfStock.Models
{
    public class SummaryReportViewModel
    {
        public enum TeamEnum : int { Grocery, Whole_Body }

        public class SummaryReportViewModelByTeam
        {
            public int storeOOSCount { get; set; }
            public int storeUPCCount { get; set; }
            public decimal storeUPCPercentage
            {
                get
                {
                    return storeUPCCount == 0 ? 0 : (decimal)storeOOSCount / (decimal)storeUPCCount;
                }
            }
            public SummaryReportViewModelByTeam(int storeOOSCount, int storeUPCCount)
            {
                this.storeOOSCount = storeOOSCount;
                this.storeUPCCount = storeUPCCount;
            }
        }

        public string storeName { get; set; }
        public int storeTimesScanned { get; set; }
        public Dictionary<TeamEnum, SummaryReportViewModelByTeam> storeByTeam { get; set; }

        public SummaryReportViewModel(string storeName, int storeTimesScanned,
            Dictionary<TeamEnum, SummaryReportViewModelByTeam> storeByTeam)
        {
            this.storeName = storeName;
            this.storeTimesScanned = storeTimesScanned;
            this.storeByTeam = storeByTeam;
        }

        public static IEnumerable<SummaryReportViewModel> RunQuery(string region,
            string storeAbbreviation,
            DateTime? startDate, DateTime? endDate, bool isClosedExcluded, 
            bool isHideStoresNotReporting)
        {
            OutOfStock.MvcApplication.oosLog.Trace("Enter");
            // Preparing SQL to get the values needed and calculations for the Store OOS count,
            // Stores OOS Percentage and Timescanned etc
            List<SummaryReportViewModel> results = new List<SummaryReportViewModel>();
            OOSEntities ef = new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString);

            int idStoreClosedStatus = GetStoreClosedStatus(ef);

            IQueryable<STORE> queryStore = 
                from st in ef.STORE 
                join reg in ef.REGION.DefaultIfEmpty() on st.REGION_ID equals reg.ID
                where reg.REGION_ABBR == region && st.STATUS_ID != idStoreClosedStatus
                orderby st.STORE_NAME
                select st;
            if (!string.IsNullOrWhiteSpace(storeAbbreviation))
                queryStore = queryStore
                    .Where(st => st.STORE_ABBREVIATION.Equals(storeAbbreviation, StringComparison.OrdinalIgnoreCase))
                    .Select(st => st);
            if (isClosedExcluded)
                queryStore = queryStore
                    .Where(st => st.STORE_NAME.ToLower().IndexOf("closed") < 0)
                    .Select(st => st);

            foreach (STORE itemStore in queryStore)
            {
                //looping through Stores to do the calculations and get the values out
                Dictionary<TeamEnum, SummaryReportViewModelByTeam> storeByTeam =
                    new Dictionary<TeamEnum, SummaryReportViewModelByTeam>();
                foreach (TeamEnum teamEnum in Enum.GetValues(typeof(TeamEnum)))
                    storeByTeam.Add(teamEnum, new SummaryReportViewModelByTeam(0, 0));

                // Basis for several queries
                IQueryable<REPORT_HEADER> qReportHeader =
                    from rh in ef.REPORT_HEADER where rh.STORE_ID == itemStore.ID select rh;
                if (startDate.HasValue)
                    qReportHeader = qReportHeader.Where(rh => rh.CREATED_DATE >= startDate.Value).Select(rh => rh);
                if (endDate.HasValue)
                    qReportHeader = qReportHeader.Where(rh => rh.CREATED_DATE <= endDate.Value).Select(rh => rh);
                List<REPORT_HEADER> listReportHeader = qReportHeader.ToList();

                // Number of scans for store
                int storeTimesScanned = listReportHeader.Count();
                if (!isHideStoresNotReporting || storeTimesScanned > 0)
                {
                    // Get OOS and UPC counts
                    var qrd = listReportHeader
                        .Join(ef.REPORT_DETAIL, rh => rh.ID, rd => rd.REPORT_HEADER_ID, (rh, rd) => rd)
                        .Where(rd => !string.IsNullOrEmpty(rd.UPC))
                        .GroupBy(rd => rd.PS_TEAM)
                        .Select(g => new { team = g.Key, count = g.Count() });
                    foreach (var qItem in qrd)
                    {
                        if (!string.IsNullOrWhiteSpace(qItem.team))
                        {
                            switch (qItem.team.ToLower())
                            {
                                case "grocery":
                                    storeByTeam[TeamEnum.Grocery].storeOOSCount = qItem.count;
                                    storeByTeam[TeamEnum.Grocery].storeUPCCount =
                                        SKUCountModel.GetSKUCount(itemStore.STORE_ABBREVIATION, qItem.team, string.Empty);
                                    break;
                                case "whole body":
                                    storeByTeam[TeamEnum.Whole_Body].storeOOSCount = qItem.count;
                                    storeByTeam[TeamEnum.Whole_Body].storeUPCCount =
                                        SKUCountModel.GetSKUCount(itemStore.STORE_ABBREVIATION, qItem.team, string.Empty);
                                    break;
                            }
                        }
                    }
                    // Put it together into the results
                    results.Add(new SummaryReportViewModel(itemStore.STORE_NAME,
                        storeTimesScanned, storeByTeam));
                }
            }
            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return results;
        }

        /// <summary>
        /// Return the status indicating that a store is closed
        /// </summary>
        /// <returns></returns>
        protected static int GetStoreClosedStatus(OOSEntities db)
        {
            if (!idStoreClosedStatus.HasValue)
            {
                int? idStatus =
                    (from s in db.STATUS
                     where s.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                     select s.ID).FirstOrDefault();
                idStoreClosedStatus = idStatus.GetValueOrDefault(0);
            }
            return idStoreClosedStatus.GetValueOrDefault(0);
        }
        protected static int? idStoreClosedStatus = null;

    }
}