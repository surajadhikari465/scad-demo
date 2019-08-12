using System.Data;
using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace WebSupport.DataAccess.Queries
{
	public class GetItemIdsToScanCodesQuery : IQueryHandler<GetItemIdsToScanCodesParameters, List<string>>
	{
		private IIrmaContextFactory context;

        public GetItemIdsToScanCodesQuery(IIrmaContextFactory contextFactory)
        {
            context = contextFactory;
        }

		public List<string> Search(GetItemIdsToScanCodesParameters parameters)
        {
			if(parameters.ItemIds == null)
			{
				return new List<string>();
			}
			else
			{
				int id = 0;
				var hsID = new HashSet<int>(parameters.ItemIds.Where(x => int.TryParse(x, out id)).Select(x => id));
				return context.CreateContext(parameters.Region)
							  .ValidatedScanCode
							  .Where(x => hsID.Contains(x.InforItemId))
							  .Select(x => x.ScanCode).ToList();
			}
        }
	}
}
