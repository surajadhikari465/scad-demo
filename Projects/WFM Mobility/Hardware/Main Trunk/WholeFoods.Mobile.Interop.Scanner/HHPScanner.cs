using System;

using HHP.DataCollection.Decoding;
using HHP.DataCollection.Common;


namespace HandheldHardware
{
    public class HHPScanner : Scanner
    {
        private ScanForm myForm;
        private HHP.DataCollection.Decoding.DecodeControl scanner;
        private Result result = Result.RESULT_SUCCESS;

        public HHPScanner(ref ScanForm form)
        {
            this.myForm = form;
            InitializeDecodeComponent();
            this.scanner.Connect();
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


        private void OnScanCompleteEvent(object sender, DecodeEventArgs e)
        {
            myForm.UpdateUPCText(e.DecodeResults.pchMessage);
            result = scanner.EnableLights(false);
            myForm.UpdateControlsOnScanCompleteEvent();
        }

        private void OnScanFailedEvent(object sender, DecodeEventArgs e)
        {
            myForm.UpdateControlsScanFailedEvent();
        }

        private void OnScanTriggerEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == (System.Windows.Forms.Keys)scanner.TriggerKey)
            {
                result = scanner.EnableLights(true);
                result = scanner.ScanBarcode();
                result = scanner.EnableLights(false);
            }
            myForm.UpdateControlsOnScanTriggerEvent();
        }

        //
        // This method reloads user set scanner settings
        //
        public void restore()
        {
            scanner.Disconnect();
        }

        private void InitializeDecodeComponent()
        {
            // 
            // scanner
            //added for HHP purposes only 
            //no need to setp scanner to not attach check digit as it is a default not to...
            this.scanner = new HHP.DataCollection.Decoding.DecodeControl();
            this.scanner.AcceptsTab = true;
            this.scanner.AcceptsReturn = true;
            this.scanner.AimerTimeout = 0;
            this.scanner.AimIDDisplay = false;
            this.scanner.AutoConnect = false;
            this.scanner.AutoLEDs = true;
            this.scanner.AutoScan = false;
            this.scanner.AutoSounds = true;
            this.scanner.CodeIDDisplay = false;
            this.scanner.ContinuousScan = false;
            this.scanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular);
            this.scanner.Location = new System.Drawing.Point(48, 16);
            this.scanner.ModifierDisplay = false;
            this.scanner.Multiline = true;
            this.scanner.ScanTimeout = 5000;
            this.scanner.Size = new System.Drawing.Size(120, 22);
            this.scanner.Text = "";
            this.scanner.TraceMode = false;
            this.scanner.TriggerKey = HHP.DataCollection.Common.TriggerKeyEnum.TK_ONSCAN;
            this.scanner.VirtualKeyMode = false;
            this.scanner.VirtualKeyTerm = HHP.DataCollection.Common.VirtualKeyTermEnum.VK_NONE;

            myForm.Controls.Add(this.scanner);
          
            //add events
            try
            {
                scanner.DecodeEvent += new DecodeEventHandler(OnScanCompleteEvent);
                myForm.KeyDown += new System.Windows.Forms.KeyEventHandler(OnScanTriggerEvent);
                scanner.KeyDown += new System.Windows.Forms.KeyEventHandler(OnScanTriggerEvent);
              
            }
            catch
            {
           
            }

           result = this.scanner.EnableSymbology(Symbology.symCODE39);
           result = this.scanner.EnableSymbology(Symbology.symCODABAR);
           result = this.scanner.EnableSymbology(Symbology.symALL);
        }


    }
}
