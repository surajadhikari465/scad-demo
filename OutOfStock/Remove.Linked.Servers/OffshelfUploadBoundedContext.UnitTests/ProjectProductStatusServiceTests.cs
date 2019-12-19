using NUnit.Framework;
using OOS.Model;
using OutOfStock.Messages;
using Rhino.Mocks;

namespace OffshelfUploadBoundedContext.UnitTests
{
    [TestFixture]
    public class ProjectProductStatusServiceTests
    {
        private ProjectProductStatusService sut;
        private IProductStatusRepository repository;

        [SetUp]
        public void Setup()
        {
            sut = CreateObjectUnderTest();
        }

        [Test]
        public void Create()
        {
            Assert.IsNotNull(sut);
        }

        private ProjectProductStatusService CreateObjectUnderTest()
        {
            repository = MockRepository.GenerateMock<IProductStatusRepository>();
            return new ProjectProductStatusService(repository);
        }

        [Test]
        public void Given_a_new_product_status_when_a_product_status_is_created_then_create_product_status()
        {
            //var productStatus = ProductStatusMapper.ToProductStatus(MakeProductStatusInsertedEvent());
            //repository.Expect(p => p.Insert(productStatus)).Repeat.Once();
            //sut.CreateProductStatus(productStatus);
            //repository.VerifyAllExpectations();
        }

        private ProductStatusInsertedEvent MakeProductStatusInsertedEvent()
        {
            var statusEvent = new ProductStatusInsertedEvent();
            return statusEvent;
        }

        [Test]
        public void Given_an_existing_product_status_when_the_product_status_is_updated_then_update_the_product_status()
        {
            //var productStatus = ProductStatusMapper.ToProductStatus(MakeProductStatusModifiedEvent());
            //repository.Expect(p => p.Modify(productStatus)).Repeat.Once();
            //sut.UpdateProductStatus(productStatus);
            //repository.VerifyAllExpectations();
        }

        private ProductStatusModifiedEvent MakeProductStatusModifiedEvent()
        {
            return new ProductStatusModifiedEvent();
        }

        [Test]
        public void Given_an_existing_product_status_when_the_product_status_is_deleted_then_delete_the_product_status()
        {
            //var productStatus = ProductStatusMapper.ToProductStatus(MakeProductStatusRemovedEvent());
            //repository.Expect(p => p.Remove(productStatus)).Repeat.Once();
            //sut.DeleteProductStatus(productStatus);
            //repository.VerifyAllExpectations();
        }

        private ProductStatusRemovedEvent MakeProductStatusRemovedEvent()
        {
            return new ProductStatusRemovedEvent();
        }
    }
}
