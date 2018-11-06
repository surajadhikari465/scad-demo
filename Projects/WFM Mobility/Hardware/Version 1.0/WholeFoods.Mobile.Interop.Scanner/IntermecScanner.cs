using System;
using System.Collections.Generic;
using System.Text;
using Intermec.DataCollection;

namespace HandheldHardware
{
    public class IntermecScanner : Scanner
    {
        private ScanForm myForm;
        private Intermec.DataCollection.BarcodeReader scanner;

        public IntermecScanner(ref ScanForm form)
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
            return null;
        }
        public override void dispose()
        {
            this.scanner.Dispose();

        }

        private void OnScanCompleteEvent(object sender, BarcodeReadEventArgs e)
        {
            myForm.UpdateUPCText(e.strDataBuffer);
            myForm.UpdateControlsOnScanCompleteEvent();
        }

        private void OnScanFailedEvent(object sender, BarcodeReadEventArgs e)
        {
            myForm.UpdateControlsScanFailedEvent();
        }

        private void OnScanTriggerEvent(object sender, BarcodeReadCancelEventArgs e)
        {
           
            myForm.UpdateControlsOnScanTriggerEvent();
        }

        //
        // This method reloads user set scanner settings
        //
        public void restore()
        {
            scanner.Dispose();
        }

        private void InitializeDecodeComponent()
        {
            scanner = new BarcodeReader();
            scanner.symbology.UPCEan.Ean13CheckDigit = false;
            scanner.symbology.UPCEan.Ean8CheckDigit = false;
            scanner.symbology.UPCEan.UPCACheckDigit = false;
            scanner.symbology.UPCEan.UPCECheckDigit = false;

            scanner.BarcodeRead += new BarcodeReadEventHandler(OnScanCompleteEvent);
            scanner.BarcodeReadCanceled += new BarcodeReadCancelEventHandler(OnScanTriggerEvent);
            scanner.ThreadedRead(true);

            myForm.Closing += new System.ComponentModel.CancelEventHandler(frm_Closing);
        }

        private void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scanner.Dispose();
        }


    }
}
