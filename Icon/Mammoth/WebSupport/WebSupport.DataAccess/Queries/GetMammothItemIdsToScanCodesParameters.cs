using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace WebSupport.DataAccess.Queries
{
	public class GetMammothItemIdsToScanCodesParameters : IQuery<List<string>>
	{
		public List<string> ItemIds { get; set; }
	}
}
