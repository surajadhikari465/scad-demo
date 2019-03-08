using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using MoreLinq;
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace GlobalEventController.DataAccess.BulkCommands
{
	public class BulkDeleteNutriFactsCommandHandler : ICommandHandler<BulkDeleteNutriFactsCommand>
	{
		private IDbContextFactory<IrmaContext> contextFactory;

		public BulkDeleteNutriFactsCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public void Handle(BulkDeleteNutriFactsCommand command)
		{
			using(var context = contextFactory.CreateContext())
			{
				var Identifiers = new SqlParameter("Identifiers", SqlDbType.Structured)
				{
					TypeName = "dbo.IdentifiersType",
					Value = command.ScanCodes.Distinct(StringComparer.InvariantCultureIgnoreCase).Select(x => new{Identifier = x}).ToDataTable()
				};

				context.Database.ExecuteSqlCommand("EXEC dbo.DeleteNutriFactsFromIcon @Identifiers", Identifiers);
			}
		}
	}
}