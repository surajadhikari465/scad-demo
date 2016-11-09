using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace RegionalEventController.DataAccess.Queries
{
    public class GetTaxCodeToTaxIdMappingQueryHandler : IQueryHandler<GetTaxCodeToTaxIdMappingQuery, Dictionary<string, int>>
    {
        IconContext context;

        public GetTaxCodeToTaxIdMappingQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Execute(GetTaxCodeToTaxIdMappingQuery parameters)
        {
            var taxes = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation)
                         .ToList();

            var mapping = new Dictionary<string, int>();
            foreach (var tax in taxes)
            {
                mapping.Add(tax.traitValue.Split(' ')[0], tax.hierarchyClassID);
            }

            return mapping;
        }
    }
}
