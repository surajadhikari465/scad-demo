namespace Icon.Web.DataAccess.Models
{
    public class BulkImportCertificationAgencyModel
    {
        public string CertificationAgencyNameAndId { get; set; }
        public string CertificationAgencyId { get; set; }
        public string CertificationAgencyName { get; set; }
        public string GlutenFree { get; set; }
        public string Kosher { get; set; }
        public string NonGmo { get; set; }
        public string Organic { get; set; }
        public string DefaultOrganic { get; set; }
        public string Vegan { get; set; }
        public string Error { get; set; }
    }
}
