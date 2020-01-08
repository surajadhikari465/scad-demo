namespace Icon.Web.DataAccess.Models
{
	public class BulkContactUploadModel
	{
		public string FileName { get; set; }
		public byte[] FileContent { get; set; }
		public string UploadedBy { get; set; }

	}
}