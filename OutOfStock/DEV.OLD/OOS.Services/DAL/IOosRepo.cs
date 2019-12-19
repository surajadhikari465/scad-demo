using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NLog;
using OOS.Services.DataModels;

namespace OOS.Services.DAL
{
    public interface IOosRepo : IDisposable
    {
        IEnumerable<StoreDb> GetExistingStores();
        IEnumerable<StoreStatus> GetStoreStatuses();
        IEnumerable<Region> GetRegions();
        void SaveNewStores(List<StoreDb> stores);

        void UpdateStores(List<StoreDb> stores);
    }

    public class OosRepository : IOosRepo
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region repoMethods

        private readonly DbConnection _connection;
        public OosRepository()
        {
            _connection = GetOpenConnection();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private DbConnection GetOpenConnection()
        {
            var connex = ConfigurationManager.ConnectionStrings["OOSConnectionString"].ConnectionString;
            var connection = new SqlConnection(connex);
            connection.Open();
            return connection;
        }


        #endregion


        public IEnumerable<StoreDb> GetExistingStores()
        {
            var sql = "SELECT * FROM Store";
            return _connection.Query<StoreDb>(sql);
        }

        public IEnumerable<StoreStatus> GetStoreStatuses()
        {
            var sql = "SELECT * FROM [STATUS]";
            return _connection.Query<StoreStatus>(sql);
        }

        public IEnumerable<Region> GetRegions()
        {
            var sql = "SELECT ID, REGION_ABBR FROM [Region]";
            return _connection.Query<Region>(sql);
        }

        public void SaveNewStores(List<StoreDb> stores)
        {
            var rightNow = DateTime.Now;

            var statement =
                "Insert into Store (PS_BU, STORE_ABBREVIATION, STORE_NAME, REGION_ID, STATUS_ID, LAST_UPDATED_BY, LAST_UPDATED_DATE, CREATED_BY, CREATED_DATE) values "
                + "(@PS_BU, @STORE_ABBREVIATION, @STORE_NAME, @REGION_ID, @STATUS_ID, 'Automagical', @rightNow, 'Automagical', @rightNow)";

            logger.Info("inserting {0} stores", stores.Count);
            foreach (var store in stores)
            {
                _connection.Execute(statement,
                    new
                    {
                        store.PS_BU,
                        store.STORE_ABBREVIATION,
                        store.STORE_NAME,
                        store.REGION_ID,
                        store.STATUS_ID,
                        rightNow
                    });
            }

        }

        public void UpdateStores(List<StoreDb> stores)
        {
            var rightNow = DateTime.Now;
            var statement = "UPDATE STORE Set STORE_NAME = @STORE_NAME, STATUS_ID = @STATUS_ID, " +
                            "LAST_UPDATED_BY = 'Automagical', LAST_UPDATED_DATE = @rightNow where ID = @ID";
            logger.Info("Updating {0} stores", stores.Count);

            foreach (var store in stores)
            {
                try
                {
                    _connection.Execute(statement, new
                    {
                        store.STORE_NAME,
                        store.STATUS_ID,
                        rightNow,
                        store.ID
                    });
                    logger.Info("Updating PS_BU {0}", store.PS_BU);
                }
                catch (Exception ex)
                {
                    logger.Error("PSBU {0} - {1}", store.PS_BU, ex.Message);
                }
                
            }
        }
    }




}
