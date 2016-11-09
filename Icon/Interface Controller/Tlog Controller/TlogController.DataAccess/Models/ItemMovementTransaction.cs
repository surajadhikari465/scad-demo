namespace TlogController.DataAccess.Models
{
    public class ItemMovementTransaction
    {
        public string ESBMessageID { get; set; }
        public int FirstItemMovementToIrmaIndex { get; set; }
        public int LastItemMovementToIrmaIndex { get; set; }
        public bool? Processed { get; set; }
    }
}
