using Icon.Common.DataAccess;
using Dapper;
using System.Collections.Generic;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// GetFeatureFlagsQuery Handler
    /// </summary>
    public class GetFeatureFlagsQuery :
        IQueryHandler<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>,
            IEnumerable<FeatureFlagModel>>
    {
        private IDbProvider db;

        /// <summary>
        /// Intializes an instance of GetAllFeaturesQueryHandler
        /// </summary>
        /// <param name="dbConnection"></param>
        public GetFeatureFlagsQuery(IDbProvider db)
        {
            if (db == null)
            {
                throw new ArgumentNullException(nameof(db));
            }

            this.db = db;
        }

        public IEnumerable<FeatureFlagModel> Search(EmptyQueryParameters<IEnumerable<FeatureFlagModel>> parameters)
        {
            return this.db.Connection.Query<FeatureFlagModel>(
                @"SELECT [FeatureFlagId]
                      ,[FlagName]
                      ,[Enabled]
                      ,[Description]
                      ,[CreatedDate]
                      ,[LastModifiedDate]
                      ,[LastModifiedBy]
                  FROM [icon].[dbo].[FeatureFlag]");
        }
    }
}
