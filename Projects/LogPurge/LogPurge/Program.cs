using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Reflection;
//using WholeFoods.Common.IRMALib;
using System.Xml.Linq;
namespace LogPurge
{
    class LogPurge
    {
        //private static ConfigRepository cr = new ConfigRepository(Utility.GetConnectionString());
        private static LogLevels LL = new LogLevels();
        private static Database mydb = null; 
      /// <summary>
      /// Application entry
      /// </summary>
      /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {                               

                mydb = new SqlDatabase(Utility.GetConnectionString());                
                Readconfig();
                PurgeLog();
                
            }
            catch (Exception ex)
            {
                Utility.logger.Error("Error in main execution ", ex);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
      /// <summary>
        /// Setting up  LogeLevel object with values from custom config file.
      /// </summary>
      /// <param name="Path">Path for custom config file</param>

        private static void Readconfig()
        {
            try
            {
                string _config = "";
                string valueResult = ""; ;
               Guid _appID = new Guid(ConfigurationManager.AppSettings["ApplicationGUID"].ToString());
               Guid _envID = new Guid(ConfigurationManager.AppSettings["EnvironmentGUID"].ToString());
              _config =  cr.GetConfigDocument(_appID, _envID);
              {
                  LL.Debug = Convert.ToInt32((cr.ConfigurationGetValue(_config,enumLogLevels.Debug.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.DebugArchive = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.DebugArchive.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.Info = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.Info.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.InfoArchive = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.InfoArchive.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.Warn = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.Warn.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.WarnArchive = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.WarnArchive.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.Error = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.Error.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.ErrorArchive = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.ErrorArchive.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.Fatal = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.Fatal.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.FatalArchive = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.FatalArchive.ToString(), ref valueResult)) ? valueResult : "-1");
                  LL.TimeOut = Convert.ToInt32((cr.ConfigurationGetValue(_config, enumLogLevels.TimeOut.ToString(), ref valueResult)) ? valueResult : "-1");
                if (LL.Debug == -1 || LL.DebugArchive == -1 || LL.Info == -1 || LL.InfoArchive == -1 || LL.Warn == -1
                    || LL.WarnArchive == -1 || LL.Error == -1 || LL.ErrorArchive == -1 || LL.Fatal == -1 || LL.FatalArchive == -1 || LL.TimeOut == -1)
                  {
                      Utility.logger.Error("Error Reading config Vaues");
                      Environment.Exit(0);
                  }
              }
            }
            catch (Exception ex)
            {
                Utility.logger.Error("Error Reading config Vaues", ex);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Execute PurgeLog Stored Procedure to purge Applog table
        /// </summary>
        private static void PurgeLog()
        {
            try
            {
                DbCommand PurgeLogCommand = mydb.GetStoredProcCommand("PurgeLog");
                mydb.AddInParameter(PurgeLogCommand, "@Debug", DbType.Int32, LL.Debug);
                mydb.AddInParameter(PurgeLogCommand, "@DebugArchive", DbType.Int32, LL.DebugArchive);
                mydb.AddInParameter(PurgeLogCommand, "@Info", DbType.Int32, LL.Info);
                mydb.AddInParameter(PurgeLogCommand, "@InfoArchive", DbType.Int32, LL.InfoArchive);
                mydb.AddInParameter(PurgeLogCommand, "@Warn", DbType.Int32, LL.Warn);
                mydb.AddInParameter(PurgeLogCommand, "@WarnArchive", DbType.Int32, LL.WarnArchive);
                mydb.AddInParameter(PurgeLogCommand, "@Error", DbType.Int32, LL.Error);
                mydb.AddInParameter(PurgeLogCommand, "@ErrorArchive", DbType.Int32, LL.ErrorArchive);
                mydb.AddInParameter(PurgeLogCommand, "@Fatal", DbType.Int32, LL.Fatal);
                mydb.AddInParameter(PurgeLogCommand, "@FatalArchive", DbType.Int32, LL.FatalArchive);
                PurgeLogCommand.CommandTimeout = LL.TimeOut;
               DataTable dt =  mydb.ExecuteDataSet(PurgeLogCommand).Tables[0];
               Utility.logger.Info("Number of archived records " + dt.Rows[0]["applog"].ToString());
               Utility.logger.Info("Number of  deleted archive records " + dt.Rows[0]["applogarchive"].ToString());
            }
            catch (Exception ex)
            {
                Utility.logger.Error("Error Purging log ", ex);
                Environment.Exit(0);
            }
        }     
    }
}
