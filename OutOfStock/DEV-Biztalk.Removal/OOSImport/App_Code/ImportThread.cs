using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OOSImport.Test")]

namespace OOSImport
{
    public class ImportThread
    {
        public event EventHandler<ImportThreadProgressArgs> progressChangedEvent;
        public List<ListViewItem> itemsFromList { get; set; }
        public ImportThreadLogging logging { get; set; }
        public bool isValidationMode { get; set; }

        public ImportThread(bool isValidationMode, ListView.ListViewItemCollection itemsFromList)
        {
            this.isValidationMode = isValidationMode;
            this.itemsFromList = new List<ListViewItem>();
            foreach (ListViewItem item in itemsFromList)
                this.itemsFromList.Add(item);
            logging = new ImportThreadLogging(this);
        }

        public void OnProgressChanged(ImportThreadProgressArgs e)
        {
            if (progressChangedEvent != null)
                progressChangedEvent(this, e);
        }

        public void StartWork()
        {
            OnProgressChanged(new ImportThreadProgressArgs(
                ImportThreadProgressArgs.ProgressType.Starting, NLog.LogLevel.Info,
                "Importing batch of " + itemsFromList.Count + " items", null));
            foreach (ListViewItem item in itemsFromList)
            {
                string guidText = item.SubItems[0].Text;
                string formatText = item.SubItems[1].Text;
                string dateText = item.SubItems[2].Text;
                string storeAbbreviation = item.SubItems[3].Text;
                string fileName = item.SubItems[4].Text;
                OnProgressChanged(new ImportThreadProgressArgs(
                    ImportThreadProgressArgs.ProgressType.FileStart, NLog.LogLevel.Info,
                    "Importing with /f:" + formatText + " /d:" + dateText + " /s:" + storeAbbreviation + " " + fileName,
                    item));
                try
                {
                    ImportBatch importBatch = new ImportBatch(formatText, dateText, storeAbbreviation,
                        fileName, isValidationMode, logging);
                    while (importBatch.ImportNextFile())
                        ;
                }
                catch (Exception ex)
                {
                    OnProgressChanged(new ImportThreadProgressArgs(
                        ImportThreadProgressArgs.ProgressType.FileFailed, NLog.LogLevel.Info,
                        "Exception: Message='" + ex.Message + "', Stack='" + ex.StackTrace + "'" + (ex.InnerException == null ? string.Empty :
                        ", Inner=\"" + ex.InnerException.Message + "\""), item));
                }
                OnProgressChanged(new ImportThreadProgressArgs(
                    ImportThreadProgressArgs.ProgressType.FileComplete, NLog.LogLevel.Info,
                    "Imported " + fileName, item));
            }
            OnProgressChanged(new ImportThreadProgressArgs(
                ImportThreadProgressArgs.ProgressType.Stopping, NLog.LogLevel.Info,
                "Completed batch", null));
        }
    }
}
