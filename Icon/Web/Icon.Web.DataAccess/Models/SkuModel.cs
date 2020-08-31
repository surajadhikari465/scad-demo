namespace Icon.Web.DataAccess.Models
{
    public class SkuModel
    {
        public int SkuId { get; set; }
        public string PrimaryItemUpc { get; set; }
        public int? CountOfItems { get; set; }
        public string SkuDescription { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string KeyWords { get; set; }
    }
}