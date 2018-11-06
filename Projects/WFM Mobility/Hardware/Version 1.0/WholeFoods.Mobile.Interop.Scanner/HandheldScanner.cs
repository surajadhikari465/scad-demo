using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using OpenNETCF;


namespace HandheldHardware
{

    public class HandheldScanner
    {
        private HandheldHardware.TeklogixScanner teklogixScanner;
        private HandheldHardware.HHPScanner hhpScanner;
        private HandheldHardware.SymbolScanner symbolScanner;
        private HandheldHardware.IntermecScanner intermecScanner;
        public String HHType;
        
        
        public HandheldScanner(ref ScanForm form)
        {
            string hhptype = getHHType();
            if (hhptype == Scanner.TEKLOGIX)
            {
                //implement it
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
        private String getHHType()
        {
            String hhtype = null;

            // Specify values for setting the registry.
            string subkey = "Platform";
            
            // Get the the registry value.
            try
            {
                Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkey);
                hhtype = rkey.Name;
                hhtype = (String)rkey.GetValue("Name", Scanner.UNKNOWN);
                if (hhtype.ToLower().StartsWith("workabout"))
                    hhtype = Scanner.TEKLOGIX;
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
                    hhtype = (String)rkey.GetValue("Manufacturer", Scanner.UNKNOWN);
                    if (hhtype.ToLower().StartsWith("hand"))
                        hhtype = Scanner.HANDHELD;
                    else if (hhtype.Equals(Scanner.UNKNOWN))
                    {
                        subkey = "Ident";
                        rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkey);
                        hhtype = (String)rkey.GetValue("Name", Scanner.UNKNOWN);
                        if (hhtype.ToLower().StartsWith("symbol"))
                            hhtype = Scanner.SYMBOL;
                    }
                }
                else
                    hhtype = Scanner.UNKNOWN;
            }
            catch (System.Exception ex)
            {
                
            }


            return hhtype; 
        }

        public void restoreScannerSettings()
        {
            HHType = getHHType();
            if (HHType == Scanner.TEKLOGIX)
            {
                //implement it
                teklogixScanner.restore();
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
