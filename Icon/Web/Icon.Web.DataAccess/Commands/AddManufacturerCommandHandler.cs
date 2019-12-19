using System;
using System.Linq;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Commands
{
    public class AddManufacturerCommandHandler : ICommandHandler<AddManufacturerCommand>
    {
        private IconContext context;

        public AddManufacturerCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddManufacturerCommand data)
        {
            Validate(data);
            AddTrait(data.Manufacturer, Traits.ArCustomerId, data.ArCustomerId);
            AddTrait(data.Manufacturer, Traits.ZipCode, data.ZipCode);

            data.Manufacturer.hierarchyID = Hierarchies.Manufacturer;
            data.Manufacturer.hierarchyParentClassID = null;
            data.Manufacturer.hierarchyLevel = HierarchyLevels.Manufacturer;

            context.HierarchyClass.Add(data.Manufacturer);
            context.SaveChanges();
        }

        private void Validate(AddManufacturerCommand data)
        {
            if (context.HierarchyClass.Any(hc => hc.hierarchyID == Hierarchies.Manufacturer && hc.hierarchyClassName.ToLower() == data.Manufacturer.hierarchyClassName.ToLower()))
            {
                throw new DuplicateValueException(String.Format("The manufacturer {0} already exists.", data.Manufacturer.hierarchyClassName));
            }
        }

        private void AddTrait(HierarchyClass manufacturer, int id, string traitValue)
        {
            if (!string.IsNullOrWhiteSpace(traitValue))
            {
                manufacturer.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = id, traitValue = traitValue.Trim() });
            }
        }
    }
}
