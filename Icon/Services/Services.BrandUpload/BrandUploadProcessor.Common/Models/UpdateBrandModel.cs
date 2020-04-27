namespace BrandUploadProcessor.Common.Models
{
    public class UpdateBrandModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Locality { get; set; }
        public string ZipCode { get; set; }
        public string ParentCompany { get; set; }
        public string Designation { get; set; }

    }
}