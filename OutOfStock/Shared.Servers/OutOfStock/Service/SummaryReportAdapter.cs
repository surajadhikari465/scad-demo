using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OOS.Model;
using OutOfStock.Models;

namespace OutOfStock.Service
{
    public class SummaryReportAdapter : ISummaryReportAdapter
    {
        public List<SummaryReportViewModel> Adapt(OOSSummary oosSummary)
        {
            List<SummaryReportViewModel> summary = new List<SummaryReportViewModel>();

            var stores = oosSummary.GetStores();
            foreach (var store in stores)
            {
                int timesScanned = oosSummary.NumberOfScansFor(store);
                var storeByTeam = CreateStoreByTeamMap();

                foreach (SummaryReportViewModel.TeamEnum teamEnum in Enum.GetValues(typeof(SummaryReportViewModel.TeamEnum)))
                {
                    string team = teamEnum.ToString().Replace('_', ' ');
                    storeByTeam[teamEnum].storeOOSCount = oosSummary.OOSCountFor(store, team);
                    storeByTeam[teamEnum].storeUPCCount = oosSummary.NumberOfSKUsFor(store, team);
                }

                SummaryReportViewModel report = new SummaryReportViewModel(store, timesScanned, storeByTeam);

                summary.Add(report);
            }
            return summary;
        }

        private Dictionary<SummaryReportViewModel.TeamEnum, SummaryReportViewModel.SummaryReportViewModelByTeam> CreateStoreByTeamMap()
        {
            var storeByTeam = new Dictionary<SummaryReportViewModel.TeamEnum, SummaryReportViewModel.SummaryReportViewModelByTeam>();
            foreach (SummaryReportViewModel.TeamEnum teamEnum in Enum.GetValues(typeof(SummaryReportViewModel.TeamEnum)))
                storeByTeam.Add(teamEnum, new SummaryReportViewModel.SummaryReportViewModelByTeam(0, 0));

            return storeByTeam;
        }

    }
}