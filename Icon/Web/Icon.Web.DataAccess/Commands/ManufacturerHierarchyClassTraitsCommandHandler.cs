using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ManufacturerHierarchyClassTraitsCommandHandler : ICommandHandler<UpdateManufacturerHierarchyClassTraitsCommand>
    {
        private IconContext context;

        public ManufacturerHierarchyClassTraitsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateManufacturerHierarchyClassTraitsCommand data)
        {
            string traitValue;
            var manufacturer = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == data.Manufacturer.hierarchyClassID);
            if (manufacturer == null) return;

            HierarchyClassTrait existingTrait;
            var allTraits = manufacturer.HierarchyClassTrait.Where(x => x.traitID != Traits.SentToEsb).ToArray();

            foreach (var traitId in new int[] { Traits.ZipCode, Traits.ArCustomerId })
            {
                existingTrait = allTraits.SingleOrDefault(x => x.traitID == traitId);

                switch (traitId)
                {
                    case Traits.ZipCode:
                        traitValue = data.ZipCode;
                        break;
                    case Traits.ArCustomerId:
                        traitValue = data.ArCustomerId;
                        break;
                    default:
                        traitValue = null;
                        break;
                }

                if (existingTrait == null && traitValue == null) continue;

                if (existingTrait == null)
                {
                    manufacturer.AddHierarchyClassTrait(context, traitId, traitValue);
                }
                else
                {
                    existingTrait.UpdateHierarchyClassTrait(context, traitValue, removeIfNullOrEmpty: true, saveChanges: false);
                }
            }

            context.SaveChanges();
        }
    }
}