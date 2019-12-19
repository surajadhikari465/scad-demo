using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Configuration;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class BrandCommandHandler : ICommandHandler<BrandCommand>
    {
        private IconContext context;

        public BrandCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(BrandCommand data)
        {
            var brandToUpdate = data.Brand.hierarchyClassID > 0
                ? context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Brand.hierarchyClassID)
                : data.Brand;

            if(brandToUpdate == null) return;
            
            string currentBrandName = brandToUpdate.hierarchyClassName.Trim();
            
            if(brandToUpdate.hierarchyClassID <= 0 || ((String.IsNullOrWhiteSpace(currentBrandName) ? string.Empty : currentBrandName) != (String.IsNullOrWhiteSpace(data.Brand.hierarchyClassName) ? String.Empty : data.Brand.hierarchyClassName.Trim())))
            {
                AddOrUpdateBrandName(brandToUpdate, data.Brand.hierarchyClassName);
                GenerateBrandUpdateEvents(data.Brand.hierarchyClassID, data.Brand.hierarchyClassName);
                this.context.UpdateIrmaItemBrand(currentBrandName, data.Brand.hierarchyClassName);
            }
        }


        private void GenerateBrandUpdateEvents(int brandClassId, string brandName)
        {
            string[] configRegions = ConfigurationManager.AppSettings["BrandNameUpdateEventConfiguredRegions"].Split(',').Select(x => x.Trim()).ToArray();
            if(configRegions.Length == 0) return;

            int brandNameUpdateEventId = context.EventType.Single(et => et.EventName == "Brand Name Update").EventId;

            context.EventQueue.AddRange(configRegions.Select(x => new EventQueue()
                {
                    EventId = brandNameUpdateEventId,
                    EventMessage = brandName,
                    EventReferenceId = brandClassId,
                    RegionCode = x,
                    InsertDate = DateTime.Now,
                }).ToArray());

            context.SaveChanges();
        }

        void AddOrUpdateBrandName(HierarchyClass brand, string brandName)
        {
            brand.hierarchyClassName = brandName;

            if(brand.hierarchyClassID <= 0) //New Brand
            {
                brand.HierarchyClassTrait.Add(new HierarchyClassTrait{  traitID = Traits.SentToEsb, traitValue = null });
                context.HierarchyClass.Add(brand);
            }

            context.SaveChanges();
        }
    }
}