using System.Windows.Forms;

namespace HandheldHardware
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
            var scanForm = new ScanForm();
            HandheldScanner handheldScannerForm = new HandheldScanner(ref scanForm);            
        }
    }
}