using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.DataAccess.Commands
{
    public class AddAttributesToMessageQueueCommandHandler : ICommandHandler<AddAttributesToMessageQueueCommand>
    {
        private IDbConnection dbConnection;

        public AddAttributesToMessageQueueCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(AddAttributesToMessageQueueCommand data)
        {
            foreach (var attribute in data.Attributes)
            {
                dbConnection.Execute("INSERT esb.MessageQueueAttribute(AttributeId) VALUES (@AttributeId)", attribute);
            }
        }
    }
}
