using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;

namespace KitBuilder.DataAccess.Queries
{
	public class GetKitByKitLocaleIdParameters : IQuery<KitLocale>
	{
		public int KitLocaleId { get; set; }
	}
}


