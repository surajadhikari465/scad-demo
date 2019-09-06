namespace Audit
{
	public class AuditOutputInfo
	{
		public string Query { get; private set; }
		public string FileName { get; private set; }
		public string Region { get; private set; }
		public string ZipFile { get; set; }

		public AuditOutputInfo(string argQuery, string argFileName, string argRegion)
		{
			this.Query = argQuery;
			this.Region = argRegion;
			this.FileName = argFileName;
		}
	}
}