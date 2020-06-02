using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Esb.CchTax.Commands
{
    public class SaveTaxHierarchyClassesCommandHandler : ICommandHandler<SaveTaxHierarchyClassesCommand>
    {
        private CchTaxListenerApplicationSettings settings;

        public SaveTaxHierarchyClassesCommandHandler(CchTaxListenerApplicationSettings settings)
        {
            this.settings = settings;
        }

        public void Execute(SaveTaxHierarchyClassesCommand data)
        {
            #region SQL to process any updates per the CCH Tax message
            string updateTaxHierarchyClassSql = "EXEC app.UpdateTaxHierarchyClass @TaxHierarchyClasses, @Regions, @GenerateGlobalEvents";

            SqlParameter hierarchyClassListSql = new SqlParameter("TaxHierarchyClasses", SqlDbType.Structured)
            {
                TypeName = "app.TaxHierarchyClassUpdateType",
                Value = Utility.ToDataTable(data.TaxHierarchyClasses.Select(hc =>
                    new
                    {
                        HierarchyClassId = hc.TaxCode,
                        HierarchyClassName = hc.HierarchyClassName
                    }))
            };
            SqlParameter regionListSql = new SqlParameter("Regions", SqlDbType.Structured)
            {
                TypeName = "app.RegionAbbrType",
                Value = Utility.ToDataTable(data.Regions)
            };
            SqlParameter generateGlobalEvent = new SqlParameter("GenerateGlobalEvents", SqlDbType.Bit)
            {
                Value = this.settings.GenerateGlobalEvents
            };
            #endregion

            #region SQL to archive the CCH Tax Message
            string archiveCCHTaxMessageSql = @"INSERT INTO app.MessageHistory (MessageTypeId, MessageStatusId, Message) VALUES (@msgTypeId, @msgStatusId, @cchMsgXml)";

            SqlParameter msgTypeIdSql = new SqlParameter("msgTypeId", DbType.Int32);
            msgTypeIdSql.Value = MessageTypes.CchTaxUpdate;

            SqlParameter msgStatusIdSql = new SqlParameter("msgStatusId", DbType.Int32);
            msgStatusIdSql.Value = MessageStatusTypes.Consumed;

            SqlParameter cchMsgXmlSql = new SqlParameter("cchMsgXml", DbType.Xml);
            cchMsgXmlSql.Value = data.CchTaxMessage;
            #endregion

            using (var context = new IconContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Database.ExecuteSqlCommand(updateTaxHierarchyClassSql, hierarchyClassListSql, regionListSql, generateGlobalEvent);
                    context.Database.ExecuteSqlCommand(archiveCCHTaxMessageSql, msgTypeIdSql, msgStatusIdSql, cchMsgXmlSql);

                    transaction.Commit();
                }

                // Set HierarchyClassIDs for passing to Mammoth command handler
                foreach (var taxClass in data.TaxHierarchyClasses)
                {
                    taxClass.HierarchyClassId = context.HierarchyClass
                        .AsNoTracking()
                        .First(hc => hc.hierarchyClassName == taxClass.HierarchyClassName).hierarchyClassID;
                }
            }
        }
    }
}
