using Dapper;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using NutritionWebApi.DataAccess.Extensions;
using System.Data;
using System.Linq;

namespace NutritionWebApi.DataAccess.Commands
{
	public class DeleteNutritionCommandHandler : ICommandHandler<DeleteNutritionCommand>
	{
		 private IDbConnectionProvider DbConnectionProvider { get; set; }

    public DeleteNutritionCommandHandler(IDbConnectionProvider DbProvider)
    {
			this.DbConnectionProvider = DbProvider;
		}

		public string Execute(DeleteNutritionCommand command)
    {
			return this.DbConnectionProvider.Connection.Query<string>(
                sql: "nutrition.DeleteNutritionItems",
                param: new { @plu = command.Plus.Select(x => new {Plu = x}).ToArray().ToDataTable().AsTableValuedParameter("nutrition.Plu") },
                commandType: CommandType.StoredProcedure).Single();
		}
	}
}
