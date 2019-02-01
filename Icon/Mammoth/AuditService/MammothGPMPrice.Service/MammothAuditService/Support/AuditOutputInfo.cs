namespace Audit
{
	public class AuditOutputInfo
	{
		public string Query { get; private set; }
		public string FileName { get; private set; }
		public Region Region { get; private set; }

		public AuditOutputInfo(string argQuery, string argFileName, Region argRegion = null)
		{
			this.Query = argQuery;
			this.Region = argRegion;
			this.FileName = argFileName;
		}
	}
}