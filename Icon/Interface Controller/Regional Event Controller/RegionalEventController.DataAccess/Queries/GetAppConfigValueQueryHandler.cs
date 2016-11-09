using Irma.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetAppConfigValueQueryHandler : IQueryHandler<GetAppConfigValueQuery, string>
    {
        private IrmaContext context;
        public GetAppConfigValueQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public string Execute(GetAppConfigValueQuery parameters)
        {
            string environmentCode = (from v in context.Version
                                        select v.Environment.Substring(0, 1))
                                        .FirstOrDefault().ToString();

            return (from v in context.AppConfigValue
                    join a in context.AppConfigApp on v.ApplicationID equals a.ApplicationID
                    join e in context.AppConfigEnv on v.EnvironmentID equals e.EnvironmentID
                    join k in context.AppConfigKey on v.KeyID equals k.KeyID
                    where (!v.Deleted
                       && a.Name == parameters.applicationName
                       && k.Name == parameters.configurationKey
                       && e.ShortName.Substring(0, 1) == environmentCode)
                    select v.Value)
                    .FirstOrDefault()
                    .ToString();
        }
    }
}
