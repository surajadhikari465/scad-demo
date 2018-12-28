using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using IconWebApi.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace IconWebApi.DataAccess.Queries
{
	public class GetLocalesQueryHandler : IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>>
	{
		private IDbProvider db;

		public GetLocalesQueryHandler(IDbProvider db)
		{
			this.db = db;
		}
		public IEnumerable<GenericLocale> Search(GetLocalesQuery parameters)
		{
			var includeAddress = parameters.includeAddress;

			IEnumerable<GenericLocale> locales = this.db.Connection.Query<GenericLocale>(
			   "[app].[GetLocationHierarchy]",
			   new
			   {
				   IncludeAddress = includeAddress
			   },
			   commandType: CommandType.StoredProcedure,
			   transaction: this.db.Transaction);

			return locales;
		}
	}
}
