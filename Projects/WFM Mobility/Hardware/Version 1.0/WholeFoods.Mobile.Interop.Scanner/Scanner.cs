using System;

using System.Collections.Generic;
using System.Text;

namespace HandheldHardware
{
    public abstract class Scanner
    {
        //list of all HHs available
        public static string HANDHELD = "hhp";
        public static string SYMBOL = "symbol";
        public static string INTERMEC = "intermec";
        public static string TEKLOGIX = "teklogix";
        public static string UNKNOWN = "unknown";

        public abstract Object getScannerDriver();
        
        public abstract Object getScanner();

        public abstract void dispose();

        
    }
}
