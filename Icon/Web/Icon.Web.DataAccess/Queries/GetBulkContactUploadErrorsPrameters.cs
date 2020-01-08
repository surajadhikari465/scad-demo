using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadErrorsPrameters : IQuery<List<BulkUploadErrorModel>>
	{
		public int BulkContactUploadId { get; set; }
	}
}
