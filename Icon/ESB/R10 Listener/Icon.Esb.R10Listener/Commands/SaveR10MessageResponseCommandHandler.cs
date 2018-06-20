using Dapper;
using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Infrastructure.DataAccess;
using System;

namespace Icon.Esb.R10Listener.Commands
{
    public class SaveR10MessageResponseCommandHandler : ICommandHandler<SaveR10MessageResponseCommand>
    {
        private IDbFactory dbFactory;

        public SaveR10MessageResponseCommandHandler(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void Execute(SaveR10MessageResponseCommand data)
        {
            using (var connection = dbFactory.CreateConnection("Icon"))
            {
                connection.Execute(
                    @"INSERT INTO app.MessageResponseR10
                    (
                        MessageId,
                        RequestSuccess,
                        SystemError,
                        FailureReasonCode,
                        ResponseText,
                        InsertDate
                    )
                    VALUES
                    (
                        @MessageId,
                        @RequestSuccess,
                        @SystemError,
                        @FailureReasonCode,
                        @ResponseText,
                        @InsertDate
                    )",
                    new
                    {
                        data.R10MessageResponseModel.MessageId,
                        data.R10MessageResponseModel.RequestSuccess,
                        data.R10MessageResponseModel.SystemError,
                        data.R10MessageResponseModel.FailureReasonCode,
                        data.R10MessageResponseModel.ResponseText,
                        InsertDate = DateTime.Now
                    });
            }
            
        }
    }
}
