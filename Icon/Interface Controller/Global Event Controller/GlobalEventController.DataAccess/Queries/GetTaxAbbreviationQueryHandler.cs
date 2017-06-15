using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetTaxAbbreviationQueryHandler : IQueryHandler<GetTaxAbbreviationQuery, string>
    {
        private readonly IconContext context;

        public GetTaxAbbreviationQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public string Handle(GetTaxAbbreviationQuery parameters)
        {
            //verify search parameters
            if (parameters.HierarchyClassId < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(parameters.HierarchyClassId),
                    "The value of HierarchyClassId must be greater than 0.");
            }

            // this query joins the HierarchyClassTrait & Trait tables to find the tax
            //  abbreviation trait value for the HierarchyClass indicated by the id
            var taxAbbrTrait = (
                    from hct in context.HierarchyClassTrait
                    join t in context.Trait on hct.traitID equals t.traitID
                    where hct.hierarchyClassID == parameters.HierarchyClassId
                        && t.traitCode == parameters.TaxTraitCode
                    select new { hct.hierarchyClassID, t.traitCode, hct.traitValue }
                ).SingleOrDefault();

            //// equivalent query written with lambda syntax:
            //var taxAbbrTrait = context.HierarchyClassTrait
            //    .Join(context.Trait,
            //        hct => hct.traitID,
            //        trait => trait.traitID,
            //        (hct, trait) => new { hct.hierarchyClassID, trait.traitCode, hct.traitValue })
            //    .SingleOrDefault(anonObj => 
            //        anonObj.hierarchyClassID == parameters.HierarchyClassId 
            //            && anonObj.traitCode == parameters.TaxTraitCode);

            return taxAbbrTrait?.traitValue;
        }
    }
}
