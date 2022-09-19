
namespace InventoryProducer.Producer.Helpers
{
    public interface IArchiveInventoryEvents
    {
        void Archive(string messageBody, string eventType, int businessUnitId, int keyID, int secondaryKeyID, char status, string errorDescription, string messageNumber, string LastReprocessID);
    }
}
