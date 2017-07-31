using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.LocaleListener.Models;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace Icon.Infor.Listeners.LocaleListener.Queries
{
    public class GetSequenceIdFromLocaleIdQueryHandler: IQueryHandler<GetSequenceIdFromLocaleIdParameters, int>
    {
        private IDbProvider dbProvider;
        private string sql;
        public GetSequenceIdFromLocaleIdQueryHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public int Search(GetSequenceIdFromLocaleIdParameters parameters)
        {           
            sql = @"SELECT COALESCE((SELECT ISNULL(SequenceId,0) 
                    FROM infor.localeSequence 
                    WHERE LocaleId = " + parameters.localeId + " ),0)";        
            int result = Convert.ToInt32(dbProvider.Connection.ExecuteScalar(sql));
            return result;
        }
    }
}

