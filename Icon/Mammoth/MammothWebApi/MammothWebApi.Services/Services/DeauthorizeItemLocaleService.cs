using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.Service.Services;

public class DeauthorizeItemLocaleService : IUpdateService<DeauthorizeItemLocale>
{
    private ICommandHandler<DeleteItemLocalePriceCommand> deleteItemLocalePriceCommandHandler;
    private ILogger logger;

    public DeauthorizeItemLocaleService(
        ICommandHandler<DeleteItemLocalePriceCommand> deleteItemLocalePriceCommandHandler,
        ILogger logger)
    {
        this.deleteItemLocalePriceCommandHandler = deleteItemLocalePriceCommandHandler;
        this.logger = logger;
    }

    public void Handle(DeauthorizeItemLocale data)
    {
        foreach (var itemLocale in data.ItemLocaleServiceModelList)
        {
            deleteItemLocalePriceCommandHandler.Execute(new DeleteItemLocalePriceCommand
            {
                Region = itemLocale.Region,
                BusinessUnitId = itemLocale.BusinessUnitId,
                ScanCode = itemLocale.ScanCode
            });

            logger.Info("Price Records Deleted for Region:" + itemLocale.Region + ", ScanCode:" + itemLocale.ScanCode + ", BusinessUnitId:" + itemLocale.BusinessUnitId.ToString());
        }
    }
}