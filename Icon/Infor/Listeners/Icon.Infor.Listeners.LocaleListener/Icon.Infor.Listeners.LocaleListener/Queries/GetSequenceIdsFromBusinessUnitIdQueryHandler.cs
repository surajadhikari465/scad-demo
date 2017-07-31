using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.LocaleListener.Models;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace Icon.Infor.Listeners.LocaleListener.Queries
{
    public class GetSequenceIdFromBusinessUnitIdQueryHandler : IQueryHandler<GetSequenceIdFromBusinessUnitIdParameters, int>
    {
        private IDbProvider dbProvider;
        private string sql;
        public GetSequenceIdFromBusinessUnitIdQueryHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public int Search(GetSequenceIdFromBusinessUnitIdParameters parameters)
        {
            sql = @"DECLARE @businessUnitIdTraitId INT = (SELECT traitID FROM Trait WHERE traitCode = 'BU')
                    SELECT COALESCE((SELECT IsNull(SequenceId,0) 
                    FROM infor.localeSequence Where LocaleId = 
                   (SELECT TOP 1 l.localeID 
                    FROM dbo.Locale l
		            JOIN dbo.LocaleTrait lt ON l.localeID = lt.localeID
			        AND lt.traitID = @businessUnitIdTraitId 
			        AND lt.traitValue =" + parameters.businessUnitId + " )),0)";

            int result = Convert.ToInt32(dbProvider.Connection.ExecuteScalar(sql));
            return result;
        }
    }
}