namespace Icon.Web.DataAccess.Commands
{
    public class UpdateSkuCommand
    {
        public int SkuId { get; set; }
        public string SkuDescription { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDateTimeUtc { get; set; }
    }
}