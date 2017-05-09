using TlogController.DataAccess.Models;
namespace TlogController.Controller.ProcessorModules
{
    public interface IIrmaTlogProcessorModule
    {
        void PushSalesSumByitemDataInBulkToIrma(IrmaTlog irmaTlog);
        void PushSalesSumByitemDataTransactionByTransactionToIrma(IrmaTlog irmaTlog);
    }
}
