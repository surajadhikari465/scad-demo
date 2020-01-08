using System;

namespace Icon.Web.DataAccess.Models
{
	public class BulkContactUploadStatusModel
	{
		public int BulkContactUploadId { get; set; }
		public string FileName { get; set; }
		public DateTime FileUploadTime { get; set; }
		public string UploadedBy { get; set; }
		public string Status { get; set; }
		public string Message { get; set; }
	}
}