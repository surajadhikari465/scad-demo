
namespace Icon.Web.DataAccess.Models
{
    public class BulkImportPluRemapModel
    {
        public int CurrentNationalPluId { get; set; }
        public string CurrentNationalPlu { get; set; }
        public int NewNationalPluId { get; set; }
        public string NewNationalPlu { get; set; }
        public string Region { get; set; }
        public string RegionalPlu { get; set; }
        public string CurrentNationalPluDescription { get; set; }
        public string NewNationalPluDescription { get; set; }
    }
}
