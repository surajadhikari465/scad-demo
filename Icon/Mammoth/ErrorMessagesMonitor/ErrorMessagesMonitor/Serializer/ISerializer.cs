using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorMessagesMonitor.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
    }
}
