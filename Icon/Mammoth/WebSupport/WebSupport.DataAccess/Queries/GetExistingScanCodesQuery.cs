namespace WebSupport.DataAccess.Queries
{
	using Icon.Common;
	using Icon.Common.DataAccess;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;
	public class GetExistingScanCodesQuery : IQueryHandler<GetExistingScanCodesParameters, List<string>>
	{
		private IIrmaContextFactory contextFactory;

		public GetExistingScanCodesQuery(IIrmaContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public List<string> Search(GetExistingScanCodesParameters parameters)
		{
			var identifiers = new SqlParameter
			{
				ParameterName = "IdentifiersType",
				TypeName = "dbo.IdentifiersType",
				Value = parameters.ScanCodes
					.Select(sc => new { Identifier = sc })
					.ToDataTable()
			};

			return contextFactory.CreateContext(parameters.Region)
				.Database
				.SqlQuery<string>("EXEC mammoth.ValidateScanCodesExist @IdentifiersType", identifiers)
				.ToList();
		}
	}
}
