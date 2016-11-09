using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.Common;
using Icon.Framework;
using InterfaceController.Common;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkAddMammothPriceEventsCommandHandler: ICommandHandler<BulkAddMammothPriceEventsCommand>
    {
        private IrmaContext context;
        private bool configValueSet;
        private bool mammothEventsEnabled;

        public BulkAddMammothPriceEventsCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(BulkAddMammothPriceEventsCommand command)
        {
            if (IsMammothEventGenerationEnabled())
            {
                var itemsToGenerateEventsFor = command.ValidatedItems.Where(i => i.EventTypeId == EventTypes.NewIrmaItem || i.EventTypeId == EventTypes.ItemValidation);

                DataTable table = new DataTable();
                table.Columns.Add("Identifier");
                foreach (var item in itemsToGenerateEventsFor)
                {
                    table.Rows.Add(item.ScanCode);
                }

                SqlParameter identifiers = new SqlParameter
                {
                    ParameterName = "IdentifiersType",
                    Value = table,
                    TypeName = "dbo.IdentifiersType"
                };

                SqlParameter eventTypeName = new SqlParameter("EventTypeName", SqlDbType.VarChar);
                eventTypeName.Value = EventConstants.MammothItemPriceEvent;

                context.Database.ExecuteSqlCommand("EXEC mammoth.IconGenerateMammothEvents @IdentifiersType, @EventTypeName", identifiers, eventTypeName);
            }
        }

        private bool IsMammothEventGenerationEnabled()
        {
            if (!configValueSet)
            {
                string environmentCode = (from v in context.Version
                                          select v.Environment.Substring(0, 1))
                                               .FirstOrDefault().ToString();

                var configValue = (from v in context.AppConfigValue
                                   join a in context.AppConfigApp on v.ApplicationID equals a.ApplicationID
                                   join e in context.AppConfigEnv on v.EnvironmentID equals e.EnvironmentID
                                   join k in context.AppConfigKey on v.KeyID equals k.KeyID
                                   where (!v.Deleted
                                      && a.Name == IrmaConstants.IrmaClientApplicationName
                                      && k.Name == IrmaConstants.MammothPriceEventGenerationConfigKey
                                      && e.ShortName.Substring(0, 1) == environmentCode)
                                   select v.Value)
                                    .FirstOrDefault();
                int enabled = 0;
                if(configValue != null && Int32.TryParse(configValue, out enabled))
                {
                    mammothEventsEnabled = Convert.ToBoolean(enabled);
                }
                else
                {
                    mammothEventsEnabled = false;
                }

                configValueSet = true;
                return mammothEventsEnabled;
            }
            else
            {
                return mammothEventsEnabled;
            }
        }
    }
}
