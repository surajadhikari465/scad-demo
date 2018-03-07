using Dapper;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class MassInsertToEPlumQueueCommandHandler : ICommandHandler<MassInsertToEPlumQueueCommand>
    {
        private MammothContext context;

        public MassInsertToEPlumQueueCommandHandler(MammothContext context)
        {
            this.context = context;
        }

        public void Execute(MassInsertToEPlumQueueCommand data)
        {
            var regionCode = data.Region;
            var businessUnitIds = data.BusinessUnitIds;

            var table = new DataTable();
            table.Columns.Add("BusinessUnitId");
            businessUnitIds.ToList().ForEach(c => table.Rows.Add(c));

            var stores = new SqlParameter
            {
                ParameterName = "BusinessUnitIds",
                Value = table,
                TypeName = "gpm.BusinessUnitIdsType"
            };

            var region = new SqlParameter("Region", SqlDbType.VarChar)
            {
                Value = regionCode
            };

            context.Database.ExecuteSqlCommand(
                "EXEC stage.MassInsertToItemStoreKeysEPlum @Region, @BusinessUnitIds",
                region,
                stores);
        }
    }
}
