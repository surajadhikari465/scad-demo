using System;
using MassTransit;
using OOS.Model;
using OutOfStock.Messages;

namespace OffshelfUploadBoundedContext
{
    public class ProductStatusDeletedEventConsumer : Consumes<ProductStatusRemovedEvent>.All
    {
        private ProjectProductStatusService service;

        public ProductStatusDeletedEventConsumer(ProjectProductStatusService service)
        {
            this.service = service;
        }
        
        public void Consume(ProductStatusRemovedEvent message)
        {
            Console.WriteLine("Product Status Created : '{0}'", message.ProductStatus);

            //var productStatus = ProductStatusMapper.ToProductStatus(message);
            //service.DeleteProductStatus(productStatus);
        }
    }
}
