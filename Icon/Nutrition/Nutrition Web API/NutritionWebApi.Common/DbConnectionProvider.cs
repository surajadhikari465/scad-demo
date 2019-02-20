using System.Data;

namespace NutritionWebApi.Common
{
	public class DbConnectionProvider : IDbConnectionProvider
    {
        public IDbConnection Connection { get; set; }
    }
}
