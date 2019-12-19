using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class AddAttributeMessageCommandHandler : ICommandHandler<AddAttributeMessageCommand>
    {
        private IDbConnection dbConnection;

        public AddAttributeMessageCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(AddAttributeMessageCommand data)
        {
            dbConnection.Execute("INSERT INTO esb.MessageQueueAttribute(AttributeId) VALUES (@AttributeId)", data.AttributeModel);
        }
    }
}
