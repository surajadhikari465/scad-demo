namespace BulkItemUploadProcessor.Common.Models
{
    public class CellWithColumnInfo
    {
        public ColumnHeader Column { get; set; }
        public ParsedCell Cell { get; set; }
    }
}