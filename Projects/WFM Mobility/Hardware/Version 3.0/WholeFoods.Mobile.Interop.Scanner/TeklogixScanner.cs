using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HandheldHardware
{
    public class TeklogixScanner:Scanner
    {
        private PsionTeklogix.Barcode.Scanner scanner;
        private PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver scannerServicesDriver;
        private Boolean updateEnterKeys;
        private Boolean updateUPCE;
        public Boolean alreadyScanned = false;
        public Boolean updateUPCEStripTrailing;
        public Boolean updateUPCAStripTrailing;
        public Boolean updateEAN13StripTrailing;
        public Boolean updateEAN8StripTrailing;
        public Boolean updateUPCETransmitCheck;
        public Boolean updateUPCATransmitCheck;
        public Boolean updateEAN13TransmitCheck;
        public Boolean updateEAN8TransmitCheck;
        public Boolean updateGTCompliant;
        public Boolean updatePrefixKeys = false;

        public int prefixKey = 0;
        private ScanForm myForm;
        public PsionTeklogix.Barcode.ScanFailedEventArgs scanEvent;

        public TeklogixScanner(ScanForm form)
        {
            this.myForm = form;
            InitializeDecodeComponent();
        }

        public override Object getScanner()
        {
            return this.scanner;

        }
        public override Object getScannerDriver()
        {
            return this.scannerServicesDriver;
        }
        public override void dispose()
        {
            this.scanner.Dispose();
            this.scannerServicesDriver.Dispose();
        }
        
        public bool getUpdateEnterKeys()
        {
            return (this.updateEnterKeys);
        }
        
        public void setUpdateEnterKeys(bool yesOrNo)
        {
            this.updateEnterKeys = yesOrNo;
        }
        
        public bool getUpdateUPCE()
        {
            return (this.updateUPCE);
        }
        
        public void setUpdateUPCE(bool yesOrNo)
        {
            this.updateUPCE = yesOrNo;
        }

        public bool getUpdatePrefixKeys()
        {
            return (this.updatePrefixKeys);
        }
        public void setUpdatePrefixKeys(bool yesOrNo)
        {
            this.updatePrefixKeys = yesOrNo;
        }
        public bool getUpdateUPCEStripTrailing()
        {
            return (this.updateUPCEStripTrailing);
        }
        public void setUpdateUPCEStripTrailing(bool yesOrNo)
        {
            this.updateUPCEStripTrailing = yesOrNo;
        }

        public bool getUpdateUPCAStripTrailing()
        {
            return (this.updateUPCAStripTrailing);
        }
        public void setUpdateUPCAStripTrailing(bool yesOrNo)
        {
            this.updateUPCAStripTrailing = yesOrNo;
        }

        public bool getUpdateEAN13StripTrailing()
        {
            return (this.updateEAN13StripTrailing);
        }
        public void setUpdateEAN13StripTrailing(bool yesOrNo)
        {
            this.updateEAN13StripTrailing = yesOrNo;
        }

        public bool getUpdateEAN8StripTrailing()
        {
            return (this.updateEAN8StripTrailing);
        }
        public void setUpdateEAN8StripTrailing(bool yesOrNo)
        {
            this.updateEAN8StripTrailing = yesOrNo;
        }


        public bool getUpdateGTCompliant()
        {
            return (this.updateGTCompliant);
        }
        public void setUpdateGTCompliant(bool yesOrNo)
        {
            this.updateGTCompliant = yesOrNo;
        }
        public bool getUpdateUPCETransmitCheck()
        {
            return (this.updateUPCETransmitCheck);
        }
        public void setUpdateUPCETransmitCheck(bool yesOrNo)
        {
            this.updateUPCETransmitCheck = yesOrNo;
        }

        public bool getUpdateUPCATransmitCheck()
        {
            return (this.updateUPCATransmitCheck);
        }
        public void setUpdateUPCATransmitCheck(bool yesOrNo)
        {
            this.updateUPCATransmitCheck = yesOrNo;
        }

        public bool getUpdateEAN13TransmitCheck()
        {
            return (this.updateEAN13TransmitCheck);
        }
        public void setUpdateEAN13TransmitCheck(bool yesOrNo)
        {
            this.updateEAN13TransmitCheck = yesOrNo;
        }

        public bool getUpdateEAN8TransmitCheck()
        {
            return (this.updateEAN8TransmitCheck);
        }
        public void setUpdateEAN8TransmitCheck(bool yesOrNo)
        {
            this.updateEAN8TransmitCheck = yesOrNo;
        }
        public bool getAlreadyScanned()
        {
            return (this.alreadyScanned);
        }
        public void setAlreadyScanned(bool yesOrNo)
        {
            this.alreadyScanned = yesOrNo;
        }

        //instantiate Tecklogix scanner
        private void InitializeDecodeComponent() 
        {
            
            scanner = new PsionTeklogix.Barcode.Scanner();
            
            scannerServicesDriver = new PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver();
            
            // Make sure UPC A, UPC E, EAN 8, EAN 13 check digits are not transmitted

           
                if (PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver.InternalScannerType.IndexOf("Symbol") >= 0)
                {
                   
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\Decoded\UPCA Transmit Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\Decoded\UPCE Transmit Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\Scs\Strip Trailing") != 1 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Strip Trailing") != 1)
                        {
                            
                                scannerServicesDriver.SetProperty(@"Barcode\UPCA\Decoded\UPCA Transmit Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\UPCE\Decoded\UPCE Transmit Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN8\Scs\Strip Trailing", 1);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN13\Scs\Strip Trailing", 1);
                                scannerServicesDriver.ApplySettingChanges();
                            
                        }
                    
                   
                }
                else if (PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver.InternalScannerType.IndexOf("Intermec") >= 0)
                {

                    try
                    {
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\ICSP\Transmit Check Digit") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPCA\ICSP\Transmit Check Digit", 0);
                            setUpdateUPCATransmitCheck(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\ICSP\Transmit Check Digit") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPCE\ICSP\Transmit Check Digit", 0);
                            setUpdateUPCETransmitCheck(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\ICSP\Transmit Check Digit") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\EAN8\ICSP\Transmit Check Digit", 0);
                            setUpdateEAN8TransmitCheck(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\ICSP\Transmit Check Digit") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\EAN13\ICSP\Transmit Check Digit", 0);
                            setUpdateEAN13TransmitCheck(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\Scs\Strip Trailing") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPCA\Scs\Strip Trailing", 0);
                            setUpdateUPCAStripTrailing(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\Scs\Strip Trailing") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPCE\Scs\Strip Trailing", 0);
                            setUpdateUPCEStripTrailing(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\Scs\Strip Trailing") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\EAN8\Scs\Strip Trailing", 0);
                            setUpdateEAN8StripTrailing(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Strip Trailing") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\EAN13\Scs\Strip Trailing", 0);
                            setUpdateEAN13StripTrailing(true);
                        }
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPC_EAN\ICSP\GTIN Compliant") != 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPC_EAN\ICSP\GTIN Compliant", 0);
                            setUpdateGTCompliant(true);
                        }

                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Suffix Char") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\Scs\Suffix Char") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\C39\Scs\Suffix Char") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\C128\Scs\Suffix Char") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\Scs\Suffix Char") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\Scs\Suffix Char") != 0)
                        {
                            setUpdateEnterKeys(true);

                            scannerServicesDriver.SetProperty(@"Barcode\UPCA\Scs\Suffix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\UPCE\Scs\Suffix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\EAN8\Scs\Suffix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\EAN13\Scs\Suffix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\C39\Scs\Suffix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\C128\Scs\Suffix Char", 0);

                        }
                        else
                            setUpdateEnterKeys(false);

                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Prefix Char") != 0 ||
                           (int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\Scs\Prefix Char") != 0 ||
                           (int)scannerServicesDriver.GetProperty(@"Barcode\C39\Scs\Prefix Char") != 0 ||
                           (int)scannerServicesDriver.GetProperty(@"Barcode\C128\Scs\Prefix Char") != 0 ||
                           (int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\Scs\Prefix Char") != 0 ||
                           (int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\Scs\Prefix Char") != 0)
                        {
                            setUpdatePrefixKeys(true);
                            prefixKey = (int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Prefix Char");

                            scannerServicesDriver.SetProperty(@"Barcode\UPCA\Scs\Prefix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\UPCE\Scs\Prefix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\EAN8\Scs\Prefix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\EAN13\Scs\Prefix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\C39\Scs\Prefix Char", 0);
                            scannerServicesDriver.SetProperty(@"Barcode\C128\Scs\Prefix Char", 0);

                        }
                        else
                            setUpdatePrefixKeys(false);

                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\ICSP\Transmit as UPC-A") == 0)
                        {
                            scannerServicesDriver.SetProperty(@"Barcode\UPCE\ICSP\Transmit as UPC-A", 1);
                            setUpdateUPCE(true);
                        }
                        else
                            setUpdateUPCE(false);

                       // scannerServicesDriver.ApplySettingChanges();

                    }
                    catch (Exception ex)
                    {
                        //scannerServicesDriver.Dispose();
                        //MessageBox.Show("Setting Intermec scanner config. failed with\r\n" + ex.Message);
                        //return;
                    }
                    //moved to avoid scanner settings read issues
                    scannerServicesDriver.ApplySettingChanges();
                    
                }
                else if (PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver.InternalScannerType.IndexOf("HHP") >= 0)
                {
                  
                        if ((int)scannerServicesDriver.GetProperty(@"Barcode\UPCA\HHP\Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\UPCE\HHP\Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\HHP\Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\HHP\Check Digit") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN8\Scs\Strip Trailing") != 0 ||
                            (int)scannerServicesDriver.GetProperty(@"Barcode\EAN13\Scs\Strip Trailing") != 0)
                        {
                            
                                scannerServicesDriver.SetProperty(@"Barcode\UPCA\HHP\Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\UPCE\HHP\Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN8\HHP\Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN13\HHP\Check Digit", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN8\Scs\Strip Trailing", 0);
                                scannerServicesDriver.SetProperty(@"Barcode\EAN13\Scs\Strip Trailing", 0);
                                scannerServicesDriver.ApplySettingChanges();
                            
                        }
                   
                }
                else
                {
                    scannerServicesDriver.Dispose();
                    return;
                }
            
                
            //end of barcode settings
            
            scanner.Driver = scannerServicesDriver;

            //add events
            try
            {
                scanner.ScanCompleteEvent += new PsionTeklogix.Barcode.ScanCompleteEventHandler(OnScanCompleteEvent);
                scanner.ScanFailedEvent += new PsionTeklogix.Barcode.ScanFailedEventHandler(OnScanFailedEvent);
                scanner.ScanTriggerEvent += new PsionTeklogix.Barcode.ScanTriggerEventHandler(OnScanTriggerEvent);
            }
            catch
            {
                try
                {
                    scannerServicesDriver.Dispose();
                }
                catch
                {
                }
                //Session.exitApplication(this.session);
            }

            //this.qtyTextBox.KeyPress += new KeyPressEventHandler(barcode_TextChanged);
            
        }


        private void OnScanCompleteEvent(object sender, PsionTeklogix.Barcode.ScanCompleteEventArgs e)
        {
            //upcText = e.Text;
            myForm.UpdateUPCText(e.Text);
            myForm.UpdateControlsOnScanCompleteEvent();
        }

        private void OnScanFailedEvent(object sender, PsionTeklogix.Barcode.ScanFailedEventArgs e)
        {
            //myForm.scanEvent = e;
            myForm.UpdateControlsScanFailedEvent();
        }

        private void OnScanTriggerEvent(object sender, PsionTeklogix.Barcode.ScanTriggerEventArgs e)
        {
            if (e.ScanTriggerState == PsionTeklogix.Barcode.TriggerState.Down)
            {
                myForm.IsTriggerDown(true);
            }
            myForm.UpdateControlsOnScanTriggerEvent();
        }

        //
         // This method reloads user set scanner settings
         //
        //public void restore()
        //{
        //    if (updateEnterKeys || updateUPCE)
        //    {
        //        try
        //        {
        //            PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver myDriver = scannerServicesDriver;

        //            if (updateEnterKeys)
        //            {
        //                myDriver.SetProperty(@"Scs\Scan Result", 1); 
        //                myDriver.SetProperty(@"Barcode\UPCA\Scs\Strip Trailing", 1);
        //                myDriver.SetProperty(@"Barcode\UPCE\Scs\Strip Trailing", 1);
        //                myDriver.SetProperty(@"Barcode\EAN8\Scs\Strip Trailing", 1);
        //                myDriver.SetProperty(@"Barcode\EAN13\Scs\Strip Trailing", 1);
        //                myDriver.SetProperty(@"Barcode\UPC_EAN\ICSP\GTIN Compliant", 1);

        //                myDriver.SetProperty(@"Barcode\UPCA\Scs\Suffix Char", (int)0x0D);
        //                myDriver.SetProperty(@"Barcode\UPCE\Scs\Suffix Char", (int)0x0D);
        //                myDriver.SetProperty(@"Barcode\EAN8\Scs\Suffix Char", (int)0x0D);
        //                myDriver.SetProperty(@"Barcode\EAN13\Scs\Suffix Char", (int)0x0D);
        //                myDriver.SetProperty(@"Barcode\C39\Scs\Suffix Char", (int)0x0D);
        //                myDriver.SetProperty(@"Barcode\C128\Scs\Suffix Char", (int)0x0D);
        //            }
        //            if (updateUPCE)
        //                myDriver.SetProperty(@"Barcode\UPCE\ICSP\Transmit as UPC-A", 0);


        //            myDriver.ApplySettingChanges();
        //            scanner.Dispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Unable to reset Suffix Char on scanner settings\r\n" + ex.Message, "");
        //        }

        //    }
            
        //}

        public void restore()
        {
            PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver scannerServicesDriver1 = scannerServicesDriver;

            if (updateEnterKeys
                || updatePrefixKeys
                || updateUPCE
                || updateUPCEStripTrailing
                || updateUPCAStripTrailing
                || updateEAN13StripTrailing
                || updateEAN8StripTrailing
                || updateUPCATransmitCheck
                || updateUPCETransmitCheck
                || updateEAN8TransmitCheck
                || updateEAN13TransmitCheck
                || updateGTCompliant
                )
            {
                try
                {

                    if (updateEnterKeys)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCA\Scs\Suffix Char", (int)0x0D);
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCE\Scs\Suffix Char", (int)0x0D);
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN8\Scs\Suffix Char", (int)0x0D);
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN13\Scs\Suffix Char", (int)0x0D);
                        scannerServicesDriver1.SetProperty(@"Barcode\C39\Scs\Suffix Char", (int)0x0D);
                        scannerServicesDriver1.SetProperty(@"Barcode\C128\Scs\Suffix Char", (int)0x0D);
                    }
                    if (updatePrefixKeys)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCA\Scs\Prefix Char", prefixKey);
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCE\Scs\Prefix Char", prefixKey);
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN8\Scs\Prefix Char", prefixKey);
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN13\Scs\Prefix Char", prefixKey);
                        scannerServicesDriver1.SetProperty(@"Barcode\C39\Scs\Prefix Char", prefixKey);
                        scannerServicesDriver1.SetProperty(@"Barcode\C128\Scs\Prefix Char", prefixKey);
                    }
                    if (updateUPCE)
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCE\ICSP\Transmit as UPC-A", 0);
                    if (updateUPCAStripTrailing)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCA\Scs\Strip Trailing", 1);
                    }
                    if (updateUPCEStripTrailing)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCE\Scs\Strip Trailing", 1);
                    }
                    if (updateEAN8StripTrailing)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN8\Scs\Strip Trailing", 1);
                    }
                    if (updateEAN13StripTrailing)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN13\Scs\Strip Trailing", 1);
                    }
                    if (updateGTCompliant)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPC_EAN\ICSP\GTIN Compliant", 1);
                    }
                    if (updateUPCATransmitCheck)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCA\ICSP\Transmit Check Digit", 0);
                    }
                    if (updateUPCETransmitCheck)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\UPCE\ICSP\Transmit Check Digit", 0);
                    }
                    if (updateEAN8TransmitCheck)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN8\ICSP\Transmit Check Digit", 0);
                    }
                    if (updateEAN13TransmitCheck)
                    {
                        scannerServicesDriver1.SetProperty(@"Barcode\EAN13\ICSP\Transmit Check Digit", 0);
                    }

                    scannerServicesDriver1.ApplySettingChanges();
                    scanner.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to reset Suffix Char on scanner settings\r\n" + ex.Message, "");
                }

            }
        }

    }
}
