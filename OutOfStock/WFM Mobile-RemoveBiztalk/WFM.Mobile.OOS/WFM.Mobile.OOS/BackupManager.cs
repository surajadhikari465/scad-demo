using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WFM.Mobile.OOS
{
    public class BackupManager 
    {
        private string filePath;
        private StreamWriter backupWriter;
        private StreamReader backupReader;
        private SavedItemList ItemsList = new SavedItemList();

        public BackupManager(string filepath)
        {
            filePath = filepath;
        }

        

        public bool BackupExists()
        {
            return File.Exists(filePath);
        }

        public void CreateNewBackupFile()
        {
            backupWriter = new StreamWriter(filePath, false);
            backupWriter.WriteLine(DateTime.Now);
        }

        public SavedItemList LoadFromBackup()
        {
            CloseBackup();
            ItemsList.Items.Clear();


            backupReader = new StreamReader(filePath);

            var ts = backupReader.ReadLine();
            ItemsList.TimeStamp = DateTime.Parse(ts);

            string upc;
            while ((upc= backupReader.ReadLine()) != null)
            {
                ItemsList.Items.Add(new SavedItem(upc));
            }
            backupReader.Close();
            backupReader.Dispose();
            return ItemsList;
        }

        public void SaveToBackup(string item)
        {
            if (backupWriter == null)
                CreateNewBackupFile();

            backupWriter.WriteLine(item);
        }

        public  void CloseBackup()
        {
            if (backupWriter != null)
            {
                backupWriter.Close();
                backupWriter.Dispose();
            }
        }

        public void RemoveBackup()
        {
            File.Delete(filePath);
        }
    }
}
