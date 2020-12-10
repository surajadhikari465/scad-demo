//using MassTransit;
using OOSCommon;
using OutOfStock.Messages;

namespace OOS.Model.Handlers
{
    //public class UploadKnownUploadCommandHandler : Handles<KnownUploadCommand>
    //{
    //    private IBuildKnownUpload builder;
    //    private IServiceBus bus;

    //    public UploadKnownUploadCommandHandler(IBuildKnownUpload builder, IServiceBus bus)
    //    {
    //        this.builder = builder;
    //        this.bus = bus;
    //    }

    //    public void Handle(KnownUploadCommand message)
    //    {
    //        var uploadDate = message.UploadDate;
    //        builder.Build(uploadDate, ItemDataMapper.ToOOSKnownItemDatas(message.Items), VendorRegionMapper.ToOOSKnownVendorRegionMaps(message.VendorRegionMaps));
    //        var upload = builder.ToKnownUpload();
    //        var events = upload.GetChanges();
    //        foreach (var change in events)
    //        {
    //            bus.Publish(change);
    //        }
    //    }
    //}
}
