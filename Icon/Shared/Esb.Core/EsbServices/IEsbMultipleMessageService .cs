using Icon.Esb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esb.Core.EsbServices
{
    public interface IEsbMultipleMessageService<TRequest>
    {
        /// <summary>
        /// The settings of the connection to the ESB that this service is configured with.
        /// </summary>
        EsbConnectionSettings Settings { get; set; }

        /// <summary>
        /// Delivers the request to the ESB using the current EsbConnectionSettings.
        /// </summary>
        /// <param name="request">The request to send to the ESB.</param>
        /// <returns>The responses of the send to the ESB containing a status of whether the message was sent and the text of the message that was sent.</returns>
        List<EsbServiceResponse> Send(TRequest request);
    }
}
