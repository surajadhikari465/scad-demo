using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OOSCommon
{
    public class FtpDirectoryContent
    {
        //-rw-r--r--    1 8466     8466            8 Jan 13 11:46 NexusDevTest.txt
        public bool isDirectory { get; set; }       // field 0 [0]
        public string access { get; set; }          // field 0 [1..9] rwx for user, group, and other
        public long position { get; set; }          // field 1 position of the file/dir and Name of the Root (2,9)
        public string[] accounts { get; set; }      // field 2 and 3 not sure
        public long filesize { get; set; }          // field 4
        public DateTime createDate { get; set; }    // field 5 create date MMM dd {HH:mm or YYYY}
        public string fileName { get; set; }        // field 6 file name

        protected enum EFtpFields : int
        { ePermissions = 0, ePosition = 1, eAccounts = 2, eFileSize = 3, eDate = 4, eFileName = 5, eDone = 6 }

        public FtpDirectoryContent(string directoryLine)
        {
            List<string> fields = new List<string>();
            EFtpFields ftpField = EFtpFields.ePermissions;
            int ixString = 0;
            string word = string.Empty; // temp / working
            long lVal = 0;  // temp / working
            while (ftpField < EFtpFields.eDone && ixString < directoryLine.Length)
            {
                switch (ftpField)
                {
                    case EFtpFields.ePermissions: // directory and permissions
                        ixString = Utility.GetWord(directoryLine, ixString, out word);
                        this.isDirectory = (word.Length > 0 && char.ToLower(word[0]) == 'd');
                        this.access = word.Substring(1);
                        ++ftpField;
                        break;
                    case EFtpFields.ePosition: // field 1 position of the file/dir 
                        ixString = Utility.GetWord(directoryLine, ixString, out word);
                        if (long.TryParse(word, out lVal))
                            this.position = lVal;
                        ++ftpField;
                        break;
                    case EFtpFields.eAccounts: // field 2 and 3 not sure
                        // accounts
                        this.accounts = new string[] { "", "" };
                        ixString = Utility.GetWord(directoryLine, ixString, out word);
                        this.accounts[0] = word;
                        ixString = Utility.GetWord(directoryLine, ixString, out word);
                        this.accounts[1] = word;
                        ++ftpField;
                        break;
                    case EFtpFields.eFileSize: // field 4 file size
                        ixString = Utility.GetWord(directoryLine, ixString, out word);
                        if (long.TryParse(word, out lVal))
                            this.filesize = lVal;
                        ++ftpField;
                        break;
                    case EFtpFields.eDate: // field 5 create date MMM dd {HH:mm or YYYY}
                        {
                            DateTime dtVal = DateTime.Now;
                            string month;
                            ixString = Utility.GetWord(directoryLine, ixString, out month);
                            if (month.Length != 3)
                                month = dtVal.ToString("MMM");
                            string day;
                            ixString = Utility.GetWord(directoryLine, ixString, out day);
                            if (day.Length != 3)
                                day = dtVal.ToString("dd");
                            string year = dtVal.ToString("yyyy");
                            string time;
                            ixString = Utility.GetWord(directoryLine, ixString, out time);
                            if (!time.Contains(":"))
                            {
                                if (time.Length == 4)
                                    year = time;
                                time = dtVal.ToString("HH:mm");
                            }
                            if (DateTime.TryParse(month + " " + day + " " + year + " " + time, out dtVal))
                                this.createDate = dtVal;
                        }
                        ++ftpField;
                        break;
                    case EFtpFields.eFileName: // field 6 file name
                        while (ixString < directoryLine.Length && char.IsWhiteSpace(directoryLine[ixString]))
                            ++ixString;
                        this.fileName = (ixString >= directoryLine.Length ? string.Empty :
                            directoryLine.Substring(ixString).Trim());
                        ftpField = EFtpFields.eDone;
                        break;
                }
            }
        }

        public static List<FtpDirectoryContent> ParseLines(string directoryLines, bool dropDirectories)
        {
            List<FtpDirectoryContent> result = new List<FtpDirectoryContent>();
            string[] directoryContentLines = Regex.Split(directoryLines, "\r\n");
            foreach (string directoryContentLine in directoryContentLines)
            {
                if (!string.IsNullOrWhiteSpace(directoryContentLine))
                {
                    FtpDirectoryContent item = new FtpDirectoryContent(directoryContentLine);
                    if (!item.isDirectory || !dropDirectories)
                        result.Add(item);
                }
            }
            return result;
        }

    }
}
