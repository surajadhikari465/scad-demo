namespace BrandUploadProcessor.Common.Models
{
    public class BrandAttributeModel
    {
        public int TraitId { get; set; }
        public string TraitCode { get; set; }
        public string TraitPattern { get; set; }
        public string TraitDesc { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }
    }
}

