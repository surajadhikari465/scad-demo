using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface ISystemListBuilder
    {
        List<string> BuildDepartmentSaleNonReceivingSystemsList();

        List<string> BuildNonRetailReceivingSystemsList();

        List<string> BuildRetailNonReceivingSystemsList();
    }
}