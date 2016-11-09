using System;

namespace Vim.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception exception);
        void Debug(string message);
    }
}
