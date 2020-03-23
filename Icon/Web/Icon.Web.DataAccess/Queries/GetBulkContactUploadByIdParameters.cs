using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadByIdParameters : IQuery<BulkContactUploadStatusModel>
	{
		public int BulkContactUploadId { get; set; }
	}
}
