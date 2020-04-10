using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface ISystemListBuilder
    {
        List<string> BuildNonRetailReceivingSystemsList();

        List<string> BuildRetailNonReceivingSystemsList();
    }
}