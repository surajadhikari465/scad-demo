using Icon.Framework;

namespace RegionalEventController.DataAccess.Models
{
    public class IrmaNewItem 
    {
        //New Item Change Type in IRMA
        public byte ItemChangeType
        {
            get { return 1; }
        }
        public string RegionCode { get; set; }
        public bool ProcessedByController { get; set; }
        public string IrmaTaxClass { get; set; }
        public int IrmaNationalClass { get; set; }
        public bool nonretailItem { get; set; }
        public int IdentifierType
        {
            get
            {
                if (nonretailItem)
                {
                    return ScanCodeTypes.PosPlu; //Non-retail item will be treated as PLU in ReCon
                }
                if (Identifier.Length < 7)
                {
                    return ScanCodeTypes.PosPlu;
                }
                else if (Identifier.StartsWith("2") && Identifier.EndsWith("00000") && Identifier.Length == 11)
                {
                    return ScanCodeTypes.ScalePlu;
                }
                else
                {
                    return ScanCodeTypes.Upc;
                }
            }
        }
        public int QueueId { get; set; }
        public int IrmaItemKey { get; set; }
        public string Identifier { get; set; }
        public int IconItemId { get; set; }
        public IRMAItem IrmaItem { get; set; }
        public string FailureReason { get; set; }
        public bool IsInvalidQueueEntry { get; set; }
    }
}
