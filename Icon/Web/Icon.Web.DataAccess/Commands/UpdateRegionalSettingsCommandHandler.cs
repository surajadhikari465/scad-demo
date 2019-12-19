using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateRegionalSettingsCommandHandler : ICommandHandler<UpdateRegionalSettingsCommand>
    {
        private IconContext context;

        public UpdateRegionalSettingsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateRegionalSettingsCommand data)
        {
            RegionalSettings updatedSettings = context.RegionalSettings.Find(data.RegionalSettingId);
            updatedSettings.Value = data.SettingValue;          

            try
            {
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("Error updating RegionalSettingId {0}", data.RegionalSettingId), exception);
            }

        }
    }
}
