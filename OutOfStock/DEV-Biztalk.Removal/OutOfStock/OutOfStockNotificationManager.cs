using System;
using System.Linq;
using OOS.Model;
using OOSCommon.DataContext;
using OutOfStock.Models;

namespace OutOfStock
{


    public class OutOfStockNotificationManager : IOutOfStockNotificationManager
    {
        private readonly IOOSEntitiesFactory _oosFactory;

        public OutOfStockNotificationManager(IOOSEntitiesFactory oosFactory)
        {
            _oosFactory = oosFactory;
        }

        public void SetNotification(string message, DateTime expiration)
        {
            using (var oos = _oosFactory.New())
            {
                ClearNotification(oos);
                var notification = oos.OutOfStockNofitication.FirstOrDefault(o => o.Id == 1);
                if (notification == null)
                    oos.OutOfStockNofitication.AddObject(new OutOfStockNofitication()
                    {
                        Id = 1,
                        Message = message,
                        Expiration = expiration
                    });
                else
                {
                    notification.Expiration = expiration;
                    notification.Message = message;

                }

                oos.SaveChanges();
            }
        }

        public void ClearNotification()
        {
            using (var oos = _oosFactory.New())
            {
                var notification = oos.OutOfStockNofitication.FirstOrDefault(o => o.Id == 1);
                if (notification == null) return;
                oos.OutOfStockNofitication.DeleteObject(notification);
                oos.SaveChanges();
            }
        }



        public void ClearNotification(IDisposableOOSEntities oos)
        {
            var notification = oos.OutOfStockNofitication.FirstOrDefault(o => o.Id == 1);
            if (notification == null) return;
            oos.OutOfStockNofitication.DeleteObject(notification);
            oos.SaveChanges();


        }

        public string GetCurrentNotification()
        {
            string notification;
            using (var oos = _oosFactory.New())
            {
                var data = oos.OutOfStockNofitication.FirstOrDefault(o => o.Id == 1 && o.Expiration > DateTime.Now);
                if (data == null) return string.Empty;
                notification = data.Message;
            }

            return notification;

        }
    }
}
