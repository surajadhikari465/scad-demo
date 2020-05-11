namespace BrandUploadProcessor.Common.Models
{
    public class AttributeColumn
    {
        public ColumnHeader ColumnHeader { get; set; }
        public bool IsRequired { get; set; }
        public string RegexPattern { get; set; }
    }
}