using System.IO;
using System.Text;

namespace Esb.Core.Serializer
{
    // ESB wants the xml in utf-8 format, but StringWriters produce utf-16 strings by default.  This override will be used to ensure that we create utf-8 strings
    // for sending to ESB.

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
