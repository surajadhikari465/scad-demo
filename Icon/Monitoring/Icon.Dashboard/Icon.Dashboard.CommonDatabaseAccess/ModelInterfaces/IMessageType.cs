using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageType
    {
        int MessageTypeId { get; set; }
        string MessageTypeName { get; set; }
    }
}
