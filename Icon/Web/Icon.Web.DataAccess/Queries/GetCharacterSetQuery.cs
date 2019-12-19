using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCharacterSetQuery : IQueryHandler<GetCharacterSetParameters, List<CharacterSetModel>>
    {
        private IDbConnection connection;

        public GetCharacterSetQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<CharacterSetModel> Search(GetCharacterSetParameters parameters)
        {
            return connection.Query<CharacterSetModel>(@"
                           SELECT  [CharactersetId]
                                  ,[Name]   
                                  ,[RegEx]
                                 FROM [dbo].[CharacterSets]").ToList();
        }
    }
}
