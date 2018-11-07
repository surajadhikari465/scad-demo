using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HandheldHardware
{

    public class ScanForm : Form
    {
        public virtual void UpdateUPCText(string upc) { }

        public virtual void IsTriggerDown(Boolean isDown) { }

        public virtual void UpdateControlsOnScanCompleteEvent() { }

        public virtual void UpdateControlsScanFailedEvent() { }


        public virtual void UpdateControlsOnScanTriggerEvent() { }

        public virtual void UpdateControlsOnScanFailedEvent() { }
       


    }
}
