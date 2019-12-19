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
            CreateNew = 0,
            UpdateExisting
        }

        public enum FileStatusEnum
        {
            New = 0,
            Processing,
            Complete,
            Error
        }
    }
}
