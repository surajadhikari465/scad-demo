namespace Icon.Web.Mvc.Models
{
    public class ContactExportViewModel
    {
        public int ContactId { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string WebsiteURL { get; set; }
        public string HierarchyName { get; set; }
        public string ContactTypeName { get; set; }
    }
}