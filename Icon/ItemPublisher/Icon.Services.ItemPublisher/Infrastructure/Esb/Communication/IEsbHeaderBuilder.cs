using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface IEsbHeaderBuilder
    {
        Dictionary<string, string> BuildMessageHeader(List<string> nonReceivingSystemsProduct, string messageId);
    }
}