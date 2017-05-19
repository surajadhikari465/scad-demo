using System;
using System.Collections.Generic;
using System.Text;

namespace HandheldHardware
{
    public class SymbolScanner : Scanner
    {
        private ScanForm myForm;
        private Symbol.Barcode.Reader scanner = null;
        private Symbol.Barcode.ReaderData MyReaderData = null;
        private System.EventHandler MyReadNotifyHandler = null;
        private object[] MyReadParamsList = null;
        private int CurrentReadParamIndex = 0;

        public SymbolScanner(ref ScanForm form)
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

        private void ReaderForm_Activated(object sender, EventArgs e)
        {
            this.StartRead();
        }

        private void ReaderForm_Deactivate(object sender, EventArgs e)
        {
            this.StopRead();
        }

        private void OnScanCompleteEvent(object sender, EventArgs e)
        {
            Symbol.Barcode.ReaderData TheReaderData = scanner.GetNextReaderData();

            if (TheReaderData != null)
            {
                // If it is a successful read (as opposed to a failed one)
                if (TheReaderData.Result == Symbol.Results.SUCCESS)
                {
                    // Handle the data from this read
                    //this.HandleData(TheReaderData);

                    myForm.UpdateUPCText(TheReaderData.Text.Substring(0, TheReaderData.Text.Length - 1));

                    // Start the next read
                    this.StartRead();
                }
                else
                {
                    OnScanFailedEvent(sender, e);
                }
            }

            myForm.UpdateControlsOnScanCompleteEvent();
        }

        private void OnScanFailedEvent(object sender, EventArgs e)
        {
            myForm.UpdateControlsScanFailedEvent();
        }

        private void OnScanTriggerEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            myForm.UpdateControlsOnScanTriggerEvent();
        }

        //
        // This method reloads user set scanner settings
        //
        public void restore()
        {
            // Terminate reader
            this.TermReader();
        }

        private void InitializeDecodeComponent()
        {
          
            // Create new reader, first available reader will be used.
            this.scanner = new Symbol.Barcode.Reader();
            // Create reader data
            this.MyReaderData = new Symbol.Barcode.ReaderData(
                                Symbol.Barcode.ReaderDataTypes.Text,
                                Symbol.Barcode.ReaderDataLengths.MaximumLabel);
            // Enable reader
            this.scanner.Actions.Enable();
             // Initialize Reader parameters
            this.InitReadParams();
            //  Attach ReaderData event handler
            this.MyReadNotifyHandler = new EventHandler(OnScanCompleteEvent);
            // Start a read on the reader
            if (scanner.Equals(null))
            {
                this.StartRead();
            }
            myForm.Activated += new EventHandler(ReaderForm_Activated);
            myForm.Deactivate += new EventHandler(ReaderForm_Deactivate);
            myForm.Closing += new System.ComponentModel.CancelEventHandler(this.frm_Closing);
            
        }

        /// <summary>
        /// Initialize the Reader parameters
        /// </summary>
        private void InitReadParams()
        {
            this.MyReadParamsList = new object[2];

            this.MyReadParamsList[0] = this.scanner.Changes.Save();

            this.scanner.Decoders.UPCE0.Enabled = true;
            this.scanner.Decoders.UPCE0.ConvertToUPCA = true;
            this.scanner.Decoders.UPCE0.Preamble = Symbol.Barcode.UPCE0.Preambles.CountryAndSystem;

            this.scanner.Decoders.UPCE1.Enabled = true;
            this.scanner.Decoders.UPCE1.ConvertToUPCA = true;
            this.scanner.Decoders.UPCE1.Preamble = Symbol.Barcode.UPCE1.Preambles.CountryAndSystem;

            this.MyReadParamsList[1] = this.scanner.Changes.Save();

            this.CurrentReadParamIndex = 0;
        }

        /// <summary>
        /// Stop reading and disable/close reader
        /// </summary>
        private void TermReader()
        {
            // If we have a reader
            if (this.scanner != null)
            {
                // Disable the reader
                this.scanner.Actions.Disable();

                // Free it up
                this.scanner.Dispose();

                // Indicate we no longer have one
                this.scanner = null;
            }

            // If we have a reader data
            if (this.MyReaderData != null)
            {
                // Free it up
                this.MyReaderData.Dispose();

                // Indicate we no longer have one
                this.MyReaderData = null;
            }
        }

        /// <summary>
        /// Start a read on the reader
        /// </summary>
        private void StartRead()
        {
            // If we have both a reader and a reader data
            if ((this.scanner != null) &&
                (this.MyReaderData != null))
            {
                // Submit a read
                this.scanner.ReadNotify += this.MyReadNotifyHandler;
                this.scanner.Actions.Read(this.MyReaderData);
            }
        }

        /// <summary>
        /// Stop all reads on the reader
        /// </summary>
        private void StopRead()
        {
            // If we have a reader
            if (this.scanner != null)
            {
                // Flush (Cancel all pending reads)
                this.scanner.Actions.Flush();
                this.scanner.ReadNotify -= this.MyReadNotifyHandler;
            }
        }

        private void frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Terminate reader
            this.TermReader();
        }

    }
}
