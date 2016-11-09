using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb.MessageBuilders.Infrastructure
{
    public interface ISerializer<T>
    {
        string Serialize(T miniBulk, TextWriter writer);
    }
}