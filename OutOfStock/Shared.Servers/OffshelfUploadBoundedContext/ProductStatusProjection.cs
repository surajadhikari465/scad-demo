using System;
using System.Text;
using MassTransit;
using OOS.Model;
using OutOfStock.Messages;

namespace OffshelfUploadBoundedContext
{
    public class ProductStatusProjection : Consumes<ProductStatusInsertedEvent>.All, Consumes<ProductStatusModifiedEvent>.All, Consumes<ProductStatusRemovedEvent>.All
    {
        private IProductStatusRepository repo;
        private const string StatusInsertedTag = "Product Status Inserted : ";
        private const string StatusModifiedTag = "Product Status Modified : ";
        private const string StatusRemovedTag = "Product Status Removed : ";

        public ProductStatusProjection(IProductStatusRepository repo)
        {
            this.repo = repo;
        }

        public void Consume(ProductStatusInsertedEvent message)
        {
            //var status = ProductStatusMapper.ToProductStatus(message);
            //Console.WriteLine(new StringBuilder(StatusInsertedTag).Append(status.ToString()).ToString());
            //repo.Insert(status);
        }

        public void Consume(ProductStatusModifiedEvent message)
        {
            //var status = ProductStatusMapper.ToProductStatus(message);
            //Console.WriteLine(new StringBuilder(StatusModifiedTag).Append(status.ToString()).ToString());
            //repo.Modify(status);
        }

        public void Consume(ProductStatusRemovedEvent message)
        {
            //var status = ProductStatusMapper.ToProductStatus(message);
            //Console.WriteLine(new StringBuilder(StatusRemovedTag).Append(status.ToString()).ToString());
            //repo.Remove(status);
        }
    }
}
