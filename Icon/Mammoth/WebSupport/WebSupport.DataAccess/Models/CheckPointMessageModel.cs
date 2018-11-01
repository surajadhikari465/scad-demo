namespace WebSupport.DataAccess.Models
{
    public class CheckPointMessageModel
    {
        public CheckPointMessageModel() { }

        public CheckPointMessageModel(int ItemId, string scanCode, string PatchFamilyId, int? SequenceId, int BusinessUnitID)
        {
            this.ItemId = ItemId;
            this.ScanCode = scanCode;
            this.PatchFamilyId = PatchFamilyId;
            this.SequenceId = SequenceId;
            this.BusinessUnitID = BusinessUnitID;
        }

        public int ItemId { get; set; }
        public int? SequenceId { get; set; }
        public string PatchFamilyId { get; set; }
        public int BusinessUnitID { get; set; }
        public string ScanCode { get; set; }
    }
}