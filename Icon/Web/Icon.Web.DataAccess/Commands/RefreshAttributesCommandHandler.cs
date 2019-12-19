using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshAttributesCommandHandler : ICommandHandler<RefreshAttributesCommand>
    {
        private readonly IDbProvider db;

        public RefreshAttributesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(RefreshAttributesCommand data)
        {
            DateTime now = DateTime.UtcNow;

            data.AttributeIds.ForEach(id => db.Connection.Execute($"INSERT esb.MessageQueueAttribute(AttributeId, InsertDateUtc) VALUES (@id, @now)", new { id, now }));
        }
    }
}
