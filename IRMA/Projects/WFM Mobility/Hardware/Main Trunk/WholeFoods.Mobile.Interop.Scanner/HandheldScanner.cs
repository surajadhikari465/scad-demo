using System;

namespace HandheldHardware
{
    public class HandheldScanner
    {
        private HandheldHardware.TeklogixScanner4 teklogixScanner4;
        private HandheldHardware.TeklogixScanner teklogixScanner;
        private HandheldHardware.HHPScanner hhpScanner;
        private HandheldHardware.SymbolScanner symbolScanner;
        private HandheldHardware.IntermecScanner intermecScanner;
        public string HHType;        
        
        public HandheldScanner(ref ScanForm form)
        {
            string hhptype = getHHType();
            
            if (hhptype == Scanner.TEKLOGIX4)
            {
                teklogixScanner4 = new HandheldHardware.TeklogixScanner4(form);
            }
            else if (hhptype == Scanner.TEKLOGIX)
            {
                teklogixScanner = new HandheldHardware.TeklogixScanner(form);
            }
            else if (hhptype == Scanner.HANDHELD)
            {
                hhpScanner = new HandheldHardware.HHPScanner(ref form);
            }
            else if (hhptype == Scanner.SYMBOL)
            {
                symbolScanner = new HandheldHardware.SymbolScanner(ref form);
            }
            else if (hhptype == Scanner.INTERMEC)
            {
                intermecScanner = new HandheldHardware.IntermecScanner(ref form);
            }

            HHType = hhptype;
        }

        private string getHHType()
        {
            string hhtype = null;

            // Get the the registry value.
            try
            {
                Microsoft.Win32.RegistryKey platformKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Platform");
                hhtype = (string)platformKey.GetValue("Name", Scanner.UNKNOWN);

                if (hhtype.ToLower().StartsWith("workabout"))
                {
                    string osType = null;
                    try
                    {
                        Microsoft.Win32.RegistryKey osTypeKey = Microsoft.Win32.Registry.LocalMachine
                        .OpenSubKey("Software")
                        .OpenSubKey("PsionTeklogix")
                        .OpenSubKey("SystemProperties")
                        .OpenSubKey("Software")
                        .OpenSubKey("OS")
                        .OpenSubKey("OS Type");

                        osType = (string)osTypeKey.GetValue("Value", String.Empty);

                        if (osType.ToLower().Contains("windows mobile"))
                        {
                            hhtype = Scanner.TEKLOGIX4;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        hhtype = Scanner.TEKLOGIX; 
                    }
                }
                else if (hhtype.ToLower().StartsWith("symbol"))
                {
                    hhtype = Scanner.SYMBOL;
                }
                else if (hhtype.ToLower().StartsWith("intermec"))
                {
                    hhtype = Scanner.INTERMEC;
                }
                else if (hhtype.ToLower().StartsWith("microsoft"))
                {
                    hhtype = (string)platformKey.GetValue("Manufacturer", Scanner.UNKNOWN);

                    if (hhtype.ToLower().StartsWith("hand"))
                    {
                        hhtype = Scanner.HANDHELD;
                    }
                    else if (hhtype.Equals(Scanner.UNKNOWN))
                    {
                        Microsoft.Win32.RegistryKey identKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Ident");
                        hhtype = (string)identKey.GetValue("Name", Scanner.UNKNOWN);

                        if (hhtype.ToLower().StartsWith("symbol"))
                        {
                            hhtype = Scanner.SYMBOL;
                        }
                    }
                }
                else
                {
                    hhtype = Scanner.UNKNOWN;
                }
            }
            catch (System.Exception) {}

            return hhtype; 
        }

        public void restoreScannerSettings()
        {
            HHType = getHHType();
            if (HHType == Scanner.TEKLOGIX)
            {
                teklogixScanner.restore();
            }
            if (HHType == Scanner.TEKLOGIX4)
            {
                teklogixScanner4.restore();
            }
            else if (HHType == Scanner.HANDHELD)
            {
                hhpScanner.restore();
            }
            else if (HHType == Scanner.SYMBOL)
            {
                symbolScanner.restore();
            }
            else if (HHType == Scanner.INTERMEC)
            {
                intermecScanner.restore();
            }
        }         
    }
}
