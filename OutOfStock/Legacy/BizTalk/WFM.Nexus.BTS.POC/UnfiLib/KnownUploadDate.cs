using System;
using System.Collections.Generic;

namespace UnfiLib
{
    [Serializable]
    public class KnownUploadDate
    {
        public DateTime FindDateInString(string line)
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
            return !result.HasValue ? DateTime.Now : result.Value;
        }
    }
}
