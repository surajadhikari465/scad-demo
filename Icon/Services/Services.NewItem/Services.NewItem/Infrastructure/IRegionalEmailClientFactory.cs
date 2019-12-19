using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Infrastructure
{
    public interface IRegionalEmailClientFactory
    {
        IEmailClient CreateEmailClient(string regionCode);
    }
}
