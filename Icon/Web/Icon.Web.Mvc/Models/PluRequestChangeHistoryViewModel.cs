using Icon.Framework;

namespace Icon.Web.Mvc.Models
{
    public class PluRequestChangeHistoryViewModel
    {
        public int PluRequestChangeHistoryID { get; set; }
        public int PluRequestID { get; set; }
        public string PluRequestChange { get; set; }
        public string PluRequestChangeType { get; set; }
        public string InsertedDate { get; set; }
        public string InsertedUser { get; set; }
        
        public PluRequestChangeHistoryViewModel() {}

        public PluRequestChangeHistoryViewModel(PLURequestChangeHistory pluRequest)
        {
            this.PluRequestID = pluRequest.pluRequestID;
            this.PluRequestChangeHistoryID = pluRequest.pluRequestChangeHistoryID;
            this.PluRequestChange = pluRequest.pluRequestChange;
            this.PluRequestChangeType = pluRequest.pluRequestChangeTypeID == PLURequestChangeTypes.StatusChange ? PLURequestChangeTypeNames.StatusChange : PLURequestChangeTypeNames.Notes;
            this.InsertedDate = pluRequest.insertedDate.ToString("MM/dd/yy H:mm:ss");
            this.InsertedUser = pluRequest.insertedUser;
        }
    }
}