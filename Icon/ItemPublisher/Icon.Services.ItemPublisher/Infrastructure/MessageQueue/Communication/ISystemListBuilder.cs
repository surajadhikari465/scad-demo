using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface ISystemListBuilder
    {
        List<string> BuildNonRetailReceivingSystemsList();

        List<string> BuildRetailNonReceivingSystemsList();
    }
}