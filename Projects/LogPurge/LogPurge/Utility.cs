using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Xml;
using log4net;
using log4net.Appender;

namespace LogPurge
{
   public static class Utility
    {
       public static AdoNetAppender ADOAppender = SetADOAppender(GetConnectionString());
       public static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       /// <summary>
       /// Setting up log4net uppender with dinamic connection string
       /// </summary>
       /// <param name="connectionString">Connection string</param>
       /// <returns></returns>
       public static AdoNetAppender SetADOAppender(string connectionString)
       {
               log4net.Config.XmlConfigurator.Configure();
               log4net.Repository.Hierarchy.Hierarchy log4netHierarchy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
               log4net.Appender.AdoNetAppender adoAppender;
               adoAppender = (AdoNetAppender)log4netHierarchy.Root.GetAppender("ADONetAppender");
               if (adoAppender != null)
               {
                   adoAppender.ConnectionString = connectionString;
                   adoAppender.ActivateOptions();
               }                       
         
           return adoAppender;
       }

       /// <summary>
       /// Getting connection string from config file or command line
       /// </summary>
       /// <returns></returns>
       public static string GetConnectionString()
       {
           string ConnectionString = string.Empty;
           try
           {
               if (ConfigurationManager.ConnectionStrings["IRMAServiceLibrary_Conn"] == null)
                   ConnectionString = Environment.GetCommandLineArgs().GetValue(2).ToString();
               else
                   ConnectionString = ConfigurationManager.ConnectionStrings["IRMAServiceLibrary_Conn"].ToString();
           }
           catch
           {
               Environment.Exit(0);
           }
           return ConnectionString;
       }

    

     
    }
}
