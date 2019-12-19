using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;

namespace OffshelfUploadBoundedContext
{
    public class OffshelfUploadService
    {
        private readonly IServiceBus _bus;

        public OffshelfUploadService(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            _bus.Dispose();
        }

    }
}
