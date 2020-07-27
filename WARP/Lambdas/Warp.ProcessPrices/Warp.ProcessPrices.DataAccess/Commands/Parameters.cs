using System.Data;
using Dapper;

namespace Warp.ProcessPrices.DataAccess.Commands
{
    public class Parameters : DynamicParameters
    {
        public new void Add(string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            if (dbType == null && value is string)
            {
                dbType = size == null ? DbType.AnsiString : DbType.AnsiStringFixedLength;
            }
            base.Add(name, value, dbType, direction, size);
        }
    }
}