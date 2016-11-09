using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgencyIdsAssociatedToItemsQuery : IQueryHandler<GetCertificationAgencyIdsAssociatedToItemsParameters, List<int>>
    {
        private IconContext context;

        public GetCertificationAgencyIdsAssociatedToItemsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<int> Search(GetCertificationAgencyIdsAssociatedToItemsParameters parameters)
        {
            if(parameters.TraitId <= 0)
            {
                throw new ArgumentException("Invalid TraitId. TraitId cannot be less than 1.");
            }

            switch(parameters.TraitId)
            {
                case Traits.GlutenFree: return context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.CertificationAgencyManagement
                            && parameters.HierarchyClassIds.Contains(hc.hierarchyClassID)
                            && hc.ItemSignAttribute.Any())
                        .Select(hc => hc.hierarchyClassID)
                        .ToList();
                case Traits.Kosher: return context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.CertificationAgencyManagement
                            && parameters.HierarchyClassIds.Contains(hc.hierarchyClassID)
                            && hc.ItemSignAttribute1.Any())
                        .Select(hc => hc.hierarchyClassID)
                        .ToList();
                case Traits.NonGmo: return context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.CertificationAgencyManagement
                            && parameters.HierarchyClassIds.Contains(hc.hierarchyClassID)
                            && hc.ItemSignAttribute2.Any())
                        .Select(hc => hc.hierarchyClassID)
                        .ToList();
                case Traits.Organic: return context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.CertificationAgencyManagement
                            && parameters.HierarchyClassIds.Contains(hc.hierarchyClassID)
                            && hc.ItemSignAttribute3.Any())
                        .Select(hc => hc.hierarchyClassID)
                        .ToList();
                case Traits.Vegan: return context.HierarchyClass
                        .Where(hc => hc.hierarchyID == Hierarchies.CertificationAgencyManagement
                            && parameters.HierarchyClassIds.Contains(hc.hierarchyClassID)
                            && hc.ItemSignAttribute4.Any())
                        .Select(hc => hc.hierarchyClassID)
                        .ToList();
                default: throw new ArgumentException("Invalid TraitId. TraitId is not a certification agency trait ID.");
            }
        }
    }
}
