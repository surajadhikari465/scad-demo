using System;

namespace BrandUploadProcessor.Common.Interfaces
{
    public interface IErrorMessageBuilder
    {
        string BuildErrorMessage(Exception error);
    }
}