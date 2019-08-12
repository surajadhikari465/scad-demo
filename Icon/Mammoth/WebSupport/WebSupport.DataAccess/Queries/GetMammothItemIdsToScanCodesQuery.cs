using System.Data;
using System.Linq;
using Mammoth.Framework;
using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace WebSupport.DataAccess.Queries
{
	public class GetMammothItemIdsToScanCodesQuery : IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>>
	{
		private MammothContext context;

		public GetMammothItemIdsToScanCodesQuery(MammothContext context)
		{
			this.context = context;
		}

		public List<string> Search(GetMammothItemIdsToScanCodesParameters parameters)
        {
			if(parameters.ItemIds == null)
			{
				return new List<string>();
			}
			else
			{
				int id = 0;
				var hsID = new HashSet<int>(parameters.ItemIds.Where(x => int.TryParse(x, out id)).Select(x => id));
				return context.Items.Where(x => hsID.Contains(x.ItemID)).Select(x => x.ScanCode).ToList();
			}
        }
	}
}
