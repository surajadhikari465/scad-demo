using TlogController.DataAccess.Models;
namespace TlogController.Controller.ProcessorModules
{
    public interface IIrmaTlogProcessorModule
    {
        void PushSalesSumByitemDataInBulkToIrma(IrmaTlog irmaTlog);
        void PushTlogReprocessRequestsInBulkToIrma(IrmaTlog irmaTlog);
        void PushSalesSumByitemDataTransactionByTransactionToIrma(IrmaTlog irmaTlog);
        void PushTlogReprocessRequestsOneByOneToIrma(IrmaTlog irmaTlog);
    }
}
