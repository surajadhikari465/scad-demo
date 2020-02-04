namespace IconWebApi.DataAccess.Models
{
    public class AssociatedContact
    {
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string ContactType { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ErrorMessage { get; set; }
    }
}