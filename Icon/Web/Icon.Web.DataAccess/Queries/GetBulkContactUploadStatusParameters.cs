using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadStatusParameters : IQuery<List<BulkContactUploadStatusModel>>
	{
		public int RowCount { get; set; }
	}
}