using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace RegionalEventController.DataAccess.Queries
{
    public class GetNationalClassCodeToClassIdMappingQueryHandler : IQueryHandler<GetNationalClassCodeToClassIdMappingQuery, Dictionary<int, int>>
    {
        IconContext context;

        public GetNationalClassCodeToClassIdMappingQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<int, int> Execute(GetNationalClassCodeToClassIdMappingQuery parameters)
        {
            var nationalClasses = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.NationalClassCode)
                         .ToList();

            var nationalClassMapping = new Dictionary<int, int>();
            foreach (var nationalClass in nationalClasses)
            {
                int classCode = 0;
                int.TryParse(nationalClass.traitValue, out classCode);
                if (classCode > 0)
                {
                    nationalClassMapping.Add(classCode, nationalClass.hierarchyClassID);
                }
            }

            return nationalClassMapping;
        }
    }
}
