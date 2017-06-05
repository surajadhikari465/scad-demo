using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.LocaleListener.Commands
{
    public class GenerateLocaleMessagesCommandHandler : ICommandHandler<GenerateLocaleMessagesCommand>
    {
        private readonly string connectionString;

        public GenerateLocaleMessagesCommandHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Execute(GenerateLocaleMessagesCommand data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                List<LocaleModel> addOrUpdateLocales = new List<LocaleModel>();
                List<LocaleModel> addOrUpdateStores = new List<LocaleModel>();
                
                foreach (var chain in data.Locale.Locales.Where(l => l.ErrorCode == null))
                {
                    bool isParentAddOrUpdate = data.Locale.Action == Contracts.ActionEnum.AddOrUpdate;

                    if(chain.Action == Contracts.ActionEnum.AddOrUpdate || (isParentAddOrUpdate && chain.Action == Contracts.ActionEnum.Inherit))
                    {
                        addOrUpdateLocales.Add(chain);
                        isParentAddOrUpdate = true;
                    }
                    foreach (var region in chain.Locales.Where(l => l.ErrorCode == null))
                    {
                        if (region.Action == Contracts.ActionEnum.AddOrUpdate || (isParentAddOrUpdate && region.Action == Contracts.ActionEnum.Inherit))
                        {
                            addOrUpdateLocales.Add(region);
                            isParentAddOrUpdate = true;
                        }
                        foreach (var metro in region.Locales.Where(l => l.ErrorCode == null))
                        {
                            if (metro.Action == Contracts.ActionEnum.AddOrUpdate || (isParentAddOrUpdate && metro.Action == Contracts.ActionEnum.Inherit))
                            {
                                addOrUpdateLocales.Add(metro);
                                isParentAddOrUpdate = true;
                            }
                            foreach (var store in metro.Locales.Where(l => l.ErrorCode == null))
                            {
                                if (store.Action == Contracts.ActionEnum.AddOrUpdate || (isParentAddOrUpdate && store.Action == Contracts.ActionEnum.Inherit))
                                {
                                    addOrUpdateStores.Add(store);
                                }
                            }
                        }
                    }
                }

                var locales = addOrUpdateLocales.Select(l => new { l.LocaleId }).ToDataTable();
                locales.SetTypeName("infor.GenerateLocaleMessageType");
                var stores = addOrUpdateStores.Select(l => new { l.BusinessUnitId }).ToDataTable();
                stores.SetTypeName("infor.GenerateStoreMessageType");

                string sql = @"infor.GenerateLocaleMessages @locales, @stores";
                connection.Execute(
                    sql,
                    new
                    {
                        locales = locales,
                        stores = stores
                    });
            }
        }
    }
}
