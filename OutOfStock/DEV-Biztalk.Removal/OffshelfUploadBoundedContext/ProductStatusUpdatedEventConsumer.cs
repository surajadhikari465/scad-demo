using System;
using MassTransit;
using OOS.Model;
using OutOfStock.Messages;

namespace OffshelfUploadBoundedContext
{
    public class ProductStatusUpdatedEventConsumer : Consumes<ProductStatusModifiedEvent>.All
    {
        private ProjectProductStatusService service;

        public ProductStatusUpdatedEventConsumer(ProjectProductStatusService service)
        {
            this.service = service;
        }

        public void Consume(ProductStatusModifiedEvent message)
        {
            Console.WriteLine("Product Status Created : '{0}'", message.ProductStatus);

            //var productStatus = ProductStatusMapper.ToProductStatus(message);
            //service.UpdateProductStatus(productStatus);

        }
    }
}
