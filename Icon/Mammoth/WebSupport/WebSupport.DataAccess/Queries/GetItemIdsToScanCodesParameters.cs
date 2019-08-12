using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace WebSupport.DataAccess.Queries
{
	public class GetItemIdsToScanCodesParameters : IQuery<List<string>>
	{
		public string Region { get; set; }
		public List<string> ItemIds { get; set; }
	}
}
