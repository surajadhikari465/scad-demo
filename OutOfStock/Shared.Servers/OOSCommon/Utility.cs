using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using StructureMap;

namespace OOSCommon
{
    public class Utility
    {

        /// <summary>
        /// Get the name of the calling method
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMethodName()
        {
            string result = string.Empty;
            StackTrace stackTrace = new StackTrace(true);
            if (stackTrace != null)
            {
                StackFrame stackFrame = stackTrace.GetFrame(1);
                if (stackFrame != null)
                {
                    // The filename and number are not valued
                    string fileName = stackFrame.GetFileName();
                    int fileLineNumber = stackFrame.GetFileLineNumber();
                    MethodBase methodBase = stackFrame.GetMethod();
                    if (methodBase != null)
                        result = methodBase.ReflectedType.FullName + "." + methodBase.Name + "()";
                }
            }
            return result;
        }

        /// <summary>
        /// Determine the validity of the UPC (checkUPC) and return it VIM equivalent value
        /// </summary>
        /// <param name="checkUPC"></param>
        /// <param name="vimUPC"></param>
        /// <returns></returns>
        public enum eUPCCheck : int { Good, Empty, NotNumeric, TooLong, TooShort, IsPLU }
        public static eUPCCheck UPCCheck(string checkUPC, out string vimUPC)
        {
            eUPCCheck result = eUPCCheck.Good;
            vimUPC = string.Empty;
            string checkUPCWork =
                (string.IsNullOrWhiteSpace(checkUPC) ? string.Empty : checkUPC.Trim());
            if (checkUPCWork.ToCharArray().Where(c => !char.IsDigit(c)).Any())
                result = eUPCCheck.NotNumeric;
            else if (checkUPCWork.Length < 1)
                result = eUPCCheck.Empty;
            else if (checkUPCWork.Length < 6)
            {
                // http://en.wikipedia.org/wiki/Price_Look-Up_code
                result = eUPCCheck.IsPLU;
            }
            else if (checkUPCWork.Length > 13)
                result = eUPCCheck.TooLong;
                // A VIM UPC
            else if (checkUPCWork.Length == 13)
                vimUPC = checkUPCWork;
                // A full UPC with a check digit
            else if (checkUPCWork.Length == 12)
            {
                if (HasChecksum(checkUPCWork))
                    vimUPC = "00" + checkUPCWork.Substring(0, 11);
                else
                    vimUPC = "0" + checkUPCWork;
            }

        // Cashier trained entry (??)
            else
                vimUPC = "000000000000".Substring(0, 13 - checkUPCWork.Length) +
                    checkUPCWork;
            return result;
        }

        public static bool HasChecksum(string upc)
        {
            return (LastDigit(upc) == CalculateChecksum(upc));
        }

        private static int LastDigit(IEnumerable<char> code)
        {
            return Convert.ToInt32(code.LastOrDefault().ToString());
        }

        public static int CalculateChecksum(string upc)
        {
            int sum = 0;
            var digitPositions = new int[upc.Length];
            for (int i = 0; i < upc.Length; i++)
            {
                digitPositions[i] = i + 1;
            }
            for (int i = 1; i <= upc.Length - 1; i++)
            {
                var weight = (digitPositions[i]%2 == 0) ? 1 : 3;
                
                int digit = Convert.ToInt32(upc.Substring(i - 1, 1));
                sum += digit * weight;
            }
            return (10 - (sum % 10)) % 10;
        }

        /// <summary>
        /// Get the next white-space delimited word from and the position 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="ixStart"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static int GetWord(string line, int ixStart, out string word)
        {
            int ixNext = ixStart;
            while (ixNext < line.Length && char.IsWhiteSpace(line[ixNext]))
                ++ixNext;
            int ixWord = ixNext;
            while (ixWord < line.Length && !char.IsWhiteSpace(line[ixWord]))
                ++ixWord;
            word = (ixNext >= line.Length ? string.Empty : line.Substring(ixNext, ixWord - ixNext));
            return ixWord;
        }

        /// <summary>
        /// Find and interpret the first date or date-time in line.
        /// Fields are transitions to/from numeric
        /// Recognized patterns for date:
        ///     MMddyy      Yeilding year 20yy
        ///     MMddyyyy
        ///     yyyyMMdd
        /// Recognized patterns for time:
        ///     <date>HHmm
        ///     <date>HHmmss<extra digits>
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static DateTime? FindDateInString(string line)
        {
            DateTime? result = null;
            // Get numeric phrases
            List<string> fields = new List<string>();
            for (int ixStart = 0; ixStart < line.Length; )
            {
                while (ixStart < line.Length && !char.IsNumber(line[ixStart]))
                    ++ixStart;
                if (ixStart < line.Length)
                {
                    int ixSpan = ixStart + 1;
                    for (; ixSpan < line.Length && char.IsNumber(line[ixSpan]); ++ixSpan)
                        ;
                    fields.Add(line.Substring(ixStart, ixSpan - ixStart));
                    ixStart = ixSpan;
                }
            }
            // Check numeric fields for plausible dates
            foreach (string field in fields)
            {
                if (field.Length == 6 || field.Length == 8 || field.Length == 12 || field.Length >= 14)
                {
                    int year = 0;
                    int month = 0;
                    int day = 0;
                    int hour = 0;
                    int minute = 0;
                    int second = 0;

                    // Month first?
                    int.TryParse(field.Substring(0, 2), out month);
                    if (month >= 1 && month <= 12)
                    {
                        int.TryParse(field.Substring(2, 2), out day);
                        if (field.Length == 6)
                        {
                            int.TryParse(field.Substring(4, 2), out year);
                            year += 2000;
                        }
                        else
                            int.TryParse(field.Substring(4, 4), out year);
                    }
                    // Year first
                    else
                    {
                        int.TryParse(field.Substring(0, 4), out year);
                        int.TryParse(field.Substring(4, 2), out month);
                        int.TryParse(field.Substring(6, 2), out day);
                    }
                    bool isCandidate = ((month >= 1 && month <= 12) &&
                        (day >= 1 && day <= 31) &&
                        (year >= 1300 && year < 4999));
                    // Check time
                    if (isCandidate && field.Length > 8)
                    {
                        // hour
                        int.TryParse(field.Substring(8, 2), out hour);
                        int.TryParse(field.Substring(10, 2), out minute);
                        if (field.Length >= 14)
                            int.TryParse(field.Substring(12, 2), out second);
                        isCandidate = ((hour >= 0 && hour <= 23) &&
                            (minute >= 0 && minute <= 60) &&
                            (second >= 0 && second < 60));
                    }
                    if (isCandidate)
                    {
                        result = new DateTime(year, month, day, hour, minute, second);
                        break;
                    }
                }
            }
            return result;
        }

    }
}
