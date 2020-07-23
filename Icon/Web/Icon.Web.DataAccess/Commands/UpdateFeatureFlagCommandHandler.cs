using Icon.Common.DataAccess;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.Common;
using System;

namespace Icon.Web.DataAccess.Commands
{
    class UpdateFeatureFlagCommandHandler : ICommandHandler<UpdateFeatureFlagCommand>
    {
        private IDbProvider db;

        public UpdateFeatureFlagCommandHandler(IDbProvider db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }

            this.db = db;
        }

        public void Execute(UpdateFeatureFlagCommand data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var rowsUpdated = db.Connection.Execute(@"
                UPDATE [dbo].[FeatureFlag]
                   SET [Enabled] = @Enabled
                      ,[Description] = @Description
                      ,[LastModifiedDateUtc] = GETUTCDATE()  
                      ,[LastModifiedBy] = @LastModifiedBy
                 WHERE [FeatureFlagId] = @FeatureFlagId;",
                data.FeatureFlag);

            if (rowsUpdated == 0)
            {
                throw new CommandException("Feature flag was not updated");
            }
        }
    }
}
