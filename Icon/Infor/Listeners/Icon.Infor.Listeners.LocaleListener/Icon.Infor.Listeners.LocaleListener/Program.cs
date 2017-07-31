using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Infor.Listeners.LocaleListener.MessageParsers;
using Icon.Infor.Listeners.LocaleListener.Queries;
using Icon.Logging;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Icon.Infor.Listeners.LocaleListener
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {               
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(c => new InforLocaleListener(
                        new LocaleMessageParser(new NLogLogger<LocaleMessageParser>()),
                        new AddOrUpdateLocalesCommandHandler(new SqlDbProvider() { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString) }),
                        new GenerateLocaleMessagesCommandHandler(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString),
                        new ArchiveLocaleMessageCommandHandler(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString),
                        ListenerApplicationSettings.CreateDefaultSettings("Infor Locale Listener"),
                        EsbConnectionSettings.CreateSettingsFromConfig(),
                        new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()),
                        EmailClient.CreateFromConfig(),
                        new NLogLogger<InforLocaleListener>(),
                        new GetSequenceIdFromLocaleIdQueryHandler(new SqlDbProvider() { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString) }),
                        new GetSequenceIdFromBusinessUnitIdQueryHandler(new SqlDbProvider() { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString) })
                        ));
                    s.WhenStarted(cm => cm.Run());
                    s.WhenStopped(cm => cm.Close());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }
    }
}
