using System.Collections.Generic;

namespace Services.Extract
{
    public static class Constants
    {
        public const string ReadyJobStatus = "ready";
        public const string RunningJobStatus = "running";
        public static List<string> SevenZipArchiveTypes => new List<string> { "*", "#", "7z", "xz", "split", "zip", "gzip", "bzip2", "tar" };
    }
}
