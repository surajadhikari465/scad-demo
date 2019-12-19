using System;
using MassTransit;
using OOS.Model;
using OutOfStock.Messages;

namespace OffshelfUploadBoundedContext
{
    public class ProductStatusCreatedEventConsumer : Consumes<ProductStatusInsertedEvent>.All
    {
        private ProjectProductStatusService service;

        public ProductStatusCreatedEventConsumer(ProjectProductStatusService service)
        {
            this.service = service;
        }

        public void Consume(ProductStatusInsertedEvent message)
        {
            Console.WriteLine("Product Status Created : '{0}'", message.ProductStatus);
            
            //var productStatus = ProductStatusMapper.ToProductStatus(message);
            //service.CreateProductStatus(productStatus);
        }
    }
}
