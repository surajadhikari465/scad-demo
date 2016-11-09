using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MessageGenerationAdmin
{
    public interface ISerializer<T>
    {
        string Serialize(T miniBulk, TextWriter writer);
    }
}
