using System;

namespace BulkItemUploadProcessor.Common.Builder
{
    public interface IErrorMessageBuilder
    {
        string BuildErrorMessage(Exception error);
    }
}