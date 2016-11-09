namespace TlogController.Controller.Processors
{
    public interface IIrmaTlogProcessor
    {
        void PopulateTlogReprocessRequests();
        void UpdateSalesSumByitem();
    }
}
