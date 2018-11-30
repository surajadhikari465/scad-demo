using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Models.DataMonster;
using MoreLinq.Extensions;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemsQueryHandler : IQueryHandler<GetItemsQuery, ItemComposite>
    {
        private IDbProvider db;

        public GetItemsQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public ItemComposite Search(GetItemsQuery parameters)
        {
            var values = parameters.ScanCodes.Select(v => new { Value = v }).ToDataTable();
            var multiResults  = this.db.Connection.QueryMultiple(
                "[dbo].[GetItems]",
                new
                {
                    ScanCodes = values
                },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);

            var itemInformation = multiResults.Read<ItemInformation>().ToList();
            var itemRegionalInformation = multiResults.Read<ItemLocaleInformation>().ToList();

            return new ItemComposite()
            {
                ItemInformation= itemInformation,
                ItemLocaleInformation = itemRegionalInformation
            };
        }
    }
}
