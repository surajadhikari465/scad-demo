using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model.Repository
{
    public class StoreDataDisposer : IStoreDataDisposer
    {
        private IConfigurator config;
        private IStoreRepository storeRepository;

        public StoreDataDisposer(IConfigurator config, IStoreRepository storeRepository)
        {
            this.config = config;
            this.storeRepository = storeRepository;
        }

        public int DeleteStoreData(string storeAbbrev, DateTime startDate, DateTime endDate)
        {
            if (storeRepository.ForName(storeAbbrev) == null) return -1;

            var n = DeleteStoreDetail(storeAbbrev, startDate, endDate);
            var m = DeleteStoreHeader(storeAbbrev, startDate, endDate);
            return n + m;
        }

        internal int DeleteStoreDetail(string storeAbbrev, DateTime startDate, DateTime endDate)
        {
            var sql = CleanReportDetailSQL();
            var command = string.Format(sql, FormStoreAbbreviation(storeAbbrev), FormDateClause(startDate, endDate));

            return ExecuteCommand(command);
        }

        private string CleanReportDetailSQL()
        {
            return "delete from REPORT_DETAIL "
                   + "where REPORT_HEADER_ID in ( "
                   + "select rh.ID from REPORT_HEADER rh "
                   + "where rh.STORE_ID=(select ID from STORE where STORE_ABBREVIATION in ({0})) "
                   + "{1} "
                   + ")";
        }

        private string FormStoreAbbreviation(string storeAbbrev)
        {
            return new StringBuilder("'").Append(storeAbbrev).Append("'").ToString();
        }

        private string FormDateClause(DateTime startDate, DateTime endDate)
        {
            const string clause = "and CREATED_DATE between '{0}' and '{1}'";
            return string.Format(clause, startDate.ToShortDateString(), endDate.ToShortDateString());
        }



        private int ExecuteCommand(string command)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var result = oosDataContext.ExecuteCommand(command, new object[] { });
                return result;
            }

        }

        internal int DeleteAllStoreDetail(string storeAbbrev)
        {
            var sql = CleanReportDetailSQL();
            var command = string.Format(sql, FormStoreAbbreviation(storeAbbrev), string.Empty);
            return ExecuteCommand(command);
        }

        internal int DeleteStoreHeader(string storeAbbrev, DateTime startDate, DateTime endDate)
        {
            var sql = CleanReportHeaderSQL();
            var command = string.Format(sql, FormStoreAbbreviation(storeAbbrev), FormDateClause(startDate, endDate));
            return ExecuteCommand(command);
        }

        private string CleanReportHeaderSQL()
        {
            return "delete from REPORT_HEADER "
                + "where STORE_ID=(select ID from STORE where STORE_ABBREVIATION in ({0})) "
                + "{1}";
        }

        internal int DeleteAllStoreHeader(string storeAbbrev)
        {
            var sql = CleanReportHeaderSQL();
            var command = string.Format(sql, FormStoreAbbreviation(storeAbbrev), string.Empty);
            return ExecuteCommand(command);
        }

        public int DeleteAllStoreData(string storeAbbrev)
        {
            if (storeRepository.ForAbbrev(storeAbbrev) == null) return -1;

            var n = DeleteAllStoreDetail(storeAbbrev);
            var m = DeleteAllStoreHeader(storeAbbrev);
            return n + m;
        }
    }
}
