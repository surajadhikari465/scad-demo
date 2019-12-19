using System;
using System.Linq;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateManufacturerCommandHandler : ICommandHandler<UpdateManufacturerCommand>
    {
        private IconContext context;

        public UpdateManufacturerCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateManufacturerCommand data)
        {
            Validate(data);

            var existingManufacturer = this.context.HierarchyClass.FirstOrDefault(x => x.hierarchyClassID == data.Manufacturer.hierarchyClassID);

            AddTrait(existingManufacturer, Traits.ArCustomerId, data.ArCustomerId);
            AddTrait(existingManufacturer, Traits.ZipCode, data.ZipCode);

            existingManufacturer.hierarchyID = Hierarchies.Manufacturer;
            existingManufacturer.hierarchyClassName = data.Manufacturer.hierarchyClassName;

            context.SaveChanges();
        }

        private void Validate(UpdateManufacturerCommand data)
        {
            if (context.HierarchyClass.Any(hc => hc.hierarchyID == Hierarchies.Manufacturer && hc.hierarchyClassName.ToLower() == data.Manufacturer.hierarchyClassName.ToLower()
            && hc.hierarchyClassID != data.Manufacturer.hierarchyClassID))
            {
                throw new DuplicateValueException(String.Format("The manufacturer {0} already exists.", data.Manufacturer.hierarchyClassName));
            }
        }

        private void AddTrait(HierarchyClass manufacturer, int id, string traitValue)
        {
            if (!string.IsNullOrWhiteSpace(traitValue))
            {
                var existingTrait = manufacturer.HierarchyClassTrait.FirstOrDefault(x => x.traitID == id);
                if(existingTrait == null)
                {
                    manufacturer.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = id, traitValue = traitValue.Trim() });
                }
                else
                {
                    existingTrait.traitValue = traitValue.Trim();
                }
            }
        }
    }
}
