namespace Icon.Web.DataAccess.Models
{
    public class ItemColumnOrderModel
    {
        public string ColumnType { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public int DisplayOrder { get; set; }
        public string ReferenceNameWithoutSpecialCharacters { get; set; }     
    }
}