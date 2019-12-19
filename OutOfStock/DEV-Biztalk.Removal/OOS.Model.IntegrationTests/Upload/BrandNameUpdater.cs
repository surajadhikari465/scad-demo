using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model.IntegrationTests.Upload
{
    public class BrandNameUpdater
    {
        private IOOSEntitiesFactory entityFactory;
        private IOffShelfUploadRepository uploadRepository;
        private IProductRepository productRepository;

        public BrandNameUpdater(IOOSEntitiesFactory entityFactory, IOffShelfUploadRepository uploadRepository, IProductRepository productRepository)
        {
            this.entityFactory = entityFactory;
            this.uploadRepository = uploadRepository;
            this.productRepository = productRepository;
        }

        public void Update()
        {
            var uploads = uploadRepository.FindAll().ToList();
            var products = ProductsFor(UpcsFrom(uploads).Distinct()).ToDictionary(p => p.UPC.Code, q => q);
            UpdateBrandNameFrom(products);
        }

        private void UpdateBrandNameFrom(IDictionary<string, IProduct> products)
        {
            using (var dbContext = entityFactory.New())
            {
                var uploads = (from c in dbContext.REPORT_DETAIL select c).GroupBy(p => p.REPORT_HEADER_ID).ToList();
                foreach (var upload in uploads)
                {
                    foreach (var scan in upload)
                    {
                        var upc = scan.UPC;
                        if (products.ContainsKey(upc))
                        {
                            scan.BRAND_NAME = products[upc].BrandName;
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        internal IEnumerable<string> UpcsFrom(IEnumerable<OffShelfUpload> uploads)
        {
            var upcs = new List<string>();
            foreach (var upload in uploads)
            {
                var scans = upload.Scans.ToList();
                scans.ForEach(p => upcs.Add(p.Upc));
            }
            return upcs;
        }

        internal IEnumerable<IProduct> ProductsFor(IEnumerable<string> upcs)
        {
            return productRepository.For(upcs);
        }
    }
}
