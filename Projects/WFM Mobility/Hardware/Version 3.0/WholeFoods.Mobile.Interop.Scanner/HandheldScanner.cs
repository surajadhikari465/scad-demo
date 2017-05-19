using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HandheldHardware
   {
   public class HandheldScanner
      {
      const uint SPI_GETPLATFORMTYPE          = 257;
      const uint SPI_GETOEMINFO               = 258;
      const uint SPI_GETPROJECTNAME           = 259;
      const uint SPI_GETPLATFORMNAME          = 260;
      const uint SPI_GETBOOTMENAME            = 261;
      const uint SPI_GETPLATFORMMANUFACTURER  = 262;
      const int  SPI_LENGTH                   = 256;
      const uint SPI_FALSE                    = 0;

      private HandheldHardware.TeklogixScanner4 teklogixScanner4;
      private HandheldHardware.TeklogixScanner  teklogixScanner;
      private HandheldHardware.HHPScanner       hhpScanner;
      private HandheldHardware.SymbolScanner    symbolScanner;
      private HandheldHardware.IntermecScanner  intermecScanner;
      public StringBuilder systemInfo = new StringBuilder(SPI_LENGTH);
      public String        HHType = "";
      public bool          systemResult;

      [DllImport("coredll.dll")]
      static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

      public HandheldScanner(ref ScanForm form)
         {
         //systemResult = SystemParametersInfo(SPI_GETPLATFORMTYPE,          SPI_LENGTH, systemInfo, SPI_FALSE);
         //systemResult = SystemParametersInfo(SPI_GETPROJECTNAME,           SPI_LENGTH, systemInfo, SPI_FALSE);
         //systemResult = SystemParametersInfo(SPI_GETPLATFORMNAME,          SPI_LENGTH, systemInfo, SPI_FALSE);
         //systemResult = SystemParametersInfo(SPI_GETBOOTMENAME,            SPI_LENGTH, systemInfo, SPI_FALSE);
         //systemResult = SystemParametersInfo(SPI_GETPLATFORMMANUFACTURER,  SPI_LENGTH, systemInfo, SPI_FALSE);
         systemResult = SystemParametersInfo(SPI_GETOEMINFO,               SPI_LENGTH, systemInfo, SPI_FALSE);
         HHType   = systemInfo.ToString().ToLower();

         //string hhptype = getHHType();

         if (HHType.Contains(Scanner.TEKLOGIX4))
            {
            HHType = Scanner.TEKLOGIX4;
            teklogixScanner4 = new HandheldHardware.TeklogixScanner4(form);
            }
         else if (HHType.Contains(Scanner.TEKLOGIX))
            {
            HHType = Scanner.TEKLOGIX;
            teklogixScanner = new HandheldHardware.TeklogixScanner(form);
            }
         else if (HHType.Contains(Scanner.HANDHELD))
            {
            HHType = Scanner.HANDHELD;
            hhpScanner = new HandheldHardware.HHPScanner(ref form);
            }
         else if (HHType.Contains(Scanner.SYMBOL))
            {
            HHType = Scanner.SYMBOL;
            symbolScanner = new HandheldHardware.SymbolScanner(ref form);
            }
         else if (HHType.Contains(Scanner.INTERMEC))
            {
            HHType = Scanner.INTERMEC;
            intermecScanner = new HandheldHardware.IntermecScanner(ref form);
            }
         }

      public void restoreScannerSettings()
         {
         if (HHType.Contains(Scanner.TEKLOGIX4))
            {
            teklogixScanner4.restore();
            }
         else if (HHType.Contains(Scanner.TEKLOGIX))
            {
            teklogixScanner.restore();
            }
         else if (HHType.Contains(Scanner.HANDHELD))
            {
            hhpScanner.restore();
            }
         else if (HHType.Contains(Scanner.SYMBOL))
            {
            symbolScanner.restore();
            }
         else if (HHType.Contains(Scanner.INTERMEC))
            {
            intermecScanner.restore();
            }
         }
      }
   }
