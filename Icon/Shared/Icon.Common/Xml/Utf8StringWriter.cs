﻿using System.IO;
using System.Text;

namespace Icon.Common.Xml
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
