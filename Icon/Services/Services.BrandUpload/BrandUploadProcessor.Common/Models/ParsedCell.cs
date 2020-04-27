namespace BrandUploadProcessor.Common.Models
{
    public class ParsedCell
    {
        public string Address { get; set; }
        public string CellValue { get; set; }
        public ColumnHeader Column { get; set; }
    }
}