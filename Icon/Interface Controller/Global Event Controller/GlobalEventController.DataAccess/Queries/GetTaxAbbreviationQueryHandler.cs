using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using System;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetTaxAbbreviationQueryHandler : IQueryHandler<GetTaxAbbreviationQuery, string>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public GetTaxAbbreviationQueryHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
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

            using (var context = contextFactory.CreateContext())
            {
                // this query joins the HierarchyClassTrait & Trait tables to find the tax
                //  abbreviation trait value for the HierarchyClass indicated by the id
                var taxAbbrTrait = (
                        from hct in context.HierarchyClassTrait
                        join t in context.Trait on hct.traitID equals t.traitID
                        where hct.hierarchyClassID == parameters.HierarchyClassId
                            && t.traitCode == parameters.TaxTraitCode
                        select new { hct.hierarchyClassID, t.traitCode, hct.traitValue }
                    ).SingleOrDefault();

                return taxAbbrTrait?.traitValue;
            }
        }
    }
}
