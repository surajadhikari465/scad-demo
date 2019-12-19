using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public interface IEsbClient
    {
        Task<EsbSendResult> SendMessage(string message, List<string> nonReceivingSystems);
    }
}