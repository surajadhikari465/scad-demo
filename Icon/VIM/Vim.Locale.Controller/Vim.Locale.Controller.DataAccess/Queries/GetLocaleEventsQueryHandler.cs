using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Vim.Common.DataAccess;
using Vim.Locale.Controller.DataAccess.Models;

namespace Vim.Locale.Controller.DataAccess.Queries
{
    public class GetLocaleEventsQueryHandler : IQueryHandler<GetLocaleEventsQuery, List<LocaleEventModel>>
    {
        private IDbProvider dbProvider;

        public GetLocaleEventsQueryHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public List<LocaleEventModel> Search(GetLocaleEventsQuery parameters)
        {
            List<LocaleEventModel> localeEvents = dbProvider.Connection.Query<LocaleEventModel, VimStoreModel, LocaleEventModel>(
                "vim.GetStagedStoresForVIM", 
                (eventModel, storeModel) => { eventModel.StoreModel = storeModel; return eventModel; },
                new
                {
                    Instance = parameters.Instance,
                    FirstAttemptWaitTimeInMinute = parameters.FirstAttemptWaitTimeInMinute,
                    SecondAttemptWaitTimeInMinute = parameters.SecondAttemptWaitTimeInMinute,
                    ThirdAttemptWaitTimeInMinute = parameters.ThirdAttemptWaitTimeInMinute
                },
                commandType: CommandType.StoredProcedure, 
                splitOn: "PSBU").ToList();

            return localeEvents;
        }
    }
}