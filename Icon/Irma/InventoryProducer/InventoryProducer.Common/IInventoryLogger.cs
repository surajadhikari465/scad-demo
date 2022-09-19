using System;
using System.Reflection;

namespace InventoryProducer.Common
{
    public interface IInventoryLogger<T> where T : class
    {
        void LogException(Exception ex, Type callingClass, MethodBase callingMethod);
        void LogException(string message, Exception ex, Type callingClass, MethodBase callingMethod);
        void LogInfo(string message);
        void LogError(string message, string exception);
    }
}
