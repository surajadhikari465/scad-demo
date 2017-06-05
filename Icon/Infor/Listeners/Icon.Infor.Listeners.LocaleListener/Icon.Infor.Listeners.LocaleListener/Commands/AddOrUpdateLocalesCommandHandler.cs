using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.LocaleListener.Models;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Commands
{
    public class AddOrUpdateLocalesCommandHandler : ICommandHandler<AddOrUpdateLocalesCommand>
    {
        private IDbProvider dbProvider;
        private string sql;

        public AddOrUpdateLocalesCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }
        public void Execute(AddOrUpdateLocalesCommand data)
        {
            var organization = data.Locale;
            var chains = organization.Locales;
            var regions = chains.SelectMany(p => p.Locales);
            var metros = regions.SelectMany(t => t.Locales);
            var stores = metros.SelectMany(s => s.Locales);
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();
            try
            {
                AddOrUpdateLocales(chains, regions, metros);

                AddOrUpdateStores(stores);
                AddOrUpdateLocaleTraits(stores);
                AddOrUpdateLocaleAddress(stores);
                dbProvider.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dbProvider.Transaction.Rollback();
            }
        }
        public void AddOrUpdateStores(IEnumerable<LocaleModel> stores)
        {
            var storesTableType = GetLocaleTableType(stores.Where(c => c.Action == ActionEnum.AddOrUpdate), "infor.LocaleAddOrUpdateType", true);
            sql = @"infor.AddOrUpdateStores  @localeStores";
            dbProvider.Connection.Execute
                (
                    sql,
                    new
                    {
                        localeStores = storesTableType
                    },
                    transaction: dbProvider.Transaction
                );
        }
        public void AddOrUpdateLocales(IEnumerable<LocaleModel> chains, IEnumerable<LocaleModel> regions,
                                       IEnumerable<LocaleModel> metros)
        {
            var chainsTableType = GetLocaleTableType(chains.Where(c => c.Action == ActionEnum.AddOrUpdate), "infor.LocaleAddOrUpdateType", false);
            var regionsTableType = GetLocaleTableType(regions.Where(c => c.Action == ActionEnum.AddOrUpdate), "infor.LocaleAddOrUpdateType", false);
            var metrosTableType = GetLocaleTableType(metros.Where(c => c.Action == ActionEnum.AddOrUpdate), "infor.LocaleAddOrUpdateType", false);      
           
            sql = @"infor.AddOrUpdateLocales @localeChains, @localeRegions, @localeMetros";
            dbProvider.Connection.Execute
                (
                    sql,
                    new
                    {
                        localeChains = chainsTableType,
                        localeRegions = regionsTableType,
                        localeMetros = metrosTableType
                    },
                    transaction: dbProvider.Transaction
                );
        }

        public void AddOrUpdateLocaleTraits(IEnumerable<LocaleModel> stores)
        {
            var traits = stores.SelectMany(s => s.LocaleTraits).ToDataTable();
            traits.SetTypeName("infor.LocaleTraitAddOrUpdateType");

            sql = @"infor.AddOrUpdateLocaleTraits @traits";
            dbProvider.Connection.Execute
                (
                    sql,
                    new
                    {
                        traits = traits
                    },
                    transaction: dbProvider.Transaction
                );
        }

        public void AddOrUpdateLocaleAddress(IEnumerable<LocaleModel> stores)
        {
            var address = stores.Select(s => s.Address).ToDataTable();
            address.SetTypeName("infor.LocaleAddressAddOrUpdateType");

            sql = @"infor.AddOrUpdateLocaleAddress @address";
            dbProvider.Connection.Execute
                (
                    sql,
                    new
                    {
                        address = address
                    },
                    transaction: dbProvider.Transaction
                );
        }

        public DataTable GetLocaleTableType(IEnumerable<LocaleModel> models, string parameterTypeName, bool isStore)
        {
            var dataTable = models.Select(or => new
            {
                or.LocaleId,
                or.Name,
                OpenDate = isStore == true ? or.OpenDate : (DateTime?)null,
                CloseDate = isStore == true?or.CloseDate:(DateTime?)null,
                or.TypeCode,
                or.ParentLocaleId,
                or.BusinessUnitId,
                or.EwicAgency,
            }).ToDataTable();

            dataTable.SetTypeName(parameterTypeName);
            return dataTable;
        }
    }
}
