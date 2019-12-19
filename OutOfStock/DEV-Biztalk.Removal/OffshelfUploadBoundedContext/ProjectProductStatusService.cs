using OOS.Model;

namespace OffshelfUploadBoundedContext
{
    public class ProjectProductStatusService
    {
        private IProductStatusRepository repository;

        public ProjectProductStatusService(IProductStatusRepository repository)
        {
            this.repository = repository;
        }

        public void CreateProductStatus(ProductStatus productStatus)
        {
            repository.Insert(productStatus);
        }

        public void UpdateProductStatus(ProductStatus productStatus)
        {
            repository.Modify(productStatus);
        }

        public void DeleteProductStatus(ProductStatus productStatus)
        {
            repository.Remove(productStatus);
        }
    }
}
