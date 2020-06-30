using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.Common;
using System;

namespace Icon.Web.DataAccess.Commands
{
    class AddFeatureFlagCommandHandler : ICommandHandler<AddFeatureFlagCommand>
    {
        private IDbProvider db;

        /// <summary>
        /// Intializes an instance of AddFeatureFlagCommandHandler.
        /// </summary>
        /// <param name="db"></param>
        public AddFeatureFlagCommandHandler(IDbProvider db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }

            this.db = db;
        }

        /// <summary>
        /// Execute.
        /// </summary>
        /// <param name="data">Add Feature Flag Command.</param>
        public void Execute(AddFeatureFlagCommand data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var rowsAdded = db.Connection.Execute(@"
                INSERT INTO [dbo].[FeatureFlag]
                           ([FlagName]
                           ,[Enabled]
                           ,[Description]
                           ,[CreatedDate]
                           ,[LastModifiedDate]
                           ,[LastModifiedBy])
                     VALUES
                           (@FlagName
                           ,@Enabled
                           ,@Description
                           ,GETDATE()
                           ,GETDATE()
                           ,@LastModifiedBy);",
                         data.FeatureFlag);

            if (rowsAdded == 0)
            {
                throw new CommandException("Feature flag was not added");
            }

        }
    }
}
