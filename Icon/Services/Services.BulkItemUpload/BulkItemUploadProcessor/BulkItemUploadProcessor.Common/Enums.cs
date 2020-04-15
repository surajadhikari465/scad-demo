using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkItemUploadProcessor.Common
{
    public static class Enums
    {
        public enum FileModeTypeEnum
        {
            CreateNew = 1,
            UpdateExisting = 2
        }

        public enum FileStatusEnum
        {
            New = 1,
            Processing = 2,
            Complete = 3, 
            Error = 4
        }
    }
}
