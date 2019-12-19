using System;
using OOSCommon.DataContext;

namespace OutOfStock
{
    public interface IOutOfStockNotificationManager
    {
        void SetNotification(string message, DateTime expiration);
        void ClearNotification();
        void ClearNotification(IDisposableOOSEntities oos);
        string GetCurrentNotification();

    }
}