
namespace InventoryProducer.Producer.Model.DBModel
{
    internal class ArchivedMessageModel
    {
        public int MessageArchiveID { get; set; }
        public string EventType { get; set; }
        public int BusinessUnitID { get; set; }
        public string MessageNumber { get; set; }
        public string Message { get; set; }
    }
}
