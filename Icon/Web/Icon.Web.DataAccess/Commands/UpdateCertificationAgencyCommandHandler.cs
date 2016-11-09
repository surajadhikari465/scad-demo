using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateCertificationAgencyCommandHandler : ICommandHandler<UpdateCertificationAgencyCommand>
    {
        private IconContext context;

        public UpdateCertificationAgencyCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateCertificationAgencyCommand data)
        {
            var agencyToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Agency.hierarchyClassID);
            var currentGlutenFreeTrait = agencyToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.GlutenFree);
            var currentKosherTrait = agencyToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Kosher);
            var currentNonGMOTrait = agencyToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.NonGmo);
            var currentOrganicTrait = agencyToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Organic);
            var currentVeganTrait = agencyToUpdate.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Vegan);

            //Check if Agency Name is updated 
            if (agencyToUpdate.hierarchyClassName != data.Agency.hierarchyClassName)
            {
                bool duplicateAgencyNameExists = context.HierarchyClass.ContainsDuplicateAgencyName(data.Agency.hierarchyClassName);

                if (duplicateAgencyNameExists)
                {
                    throw new DuplicateValueException(String.Format("The agency {0} already exists.", data.Agency.hierarchyClassName));
                }
            }

            if (AllowSignAttributeTraitUpdate(TraitCodes.GlutenFree, agencyToUpdate, currentGlutenFreeTrait, data.GlutenFree, TraitDescriptions.GlutenFree)
            && AllowSignAttributeTraitUpdate(TraitCodes.Kosher, agencyToUpdate, currentKosherTrait, data.Kosher, TraitDescriptions.Kosher)
            && AllowSignAttributeTraitUpdate(TraitCodes.NonGmo, agencyToUpdate, currentNonGMOTrait, data.NonGMO, TraitDescriptions.NonGmo)
            && AllowSignAttributeTraitUpdate(TraitCodes.Organic, agencyToUpdate, currentOrganicTrait, data.Organic, TraitDescriptions.Organic)
            && AllowSignAttributeTraitUpdate(TraitCodes.Vegan, agencyToUpdate, currentVeganTrait, data.Vegan, TraitDescriptions.Vegan))
            {
                SqlParameter agencies = new SqlParameter("agencies", SqlDbType.Structured);
                agencies.TypeName = "app.CertificationAgencyImportType";

                List<UpdateCertificationAgencyCommand> dataList = new List<UpdateCertificationAgencyCommand>();
                dataList.Add(data);
                agencies.Value = dataList.ConvertAll(a => new
                {
                    AgencyId = a.Agency.hierarchyClassID,
                    AgencyName = a.Agency.hierarchyClassName,
                    GlutenFree = a.GlutenFree,
                    Kosher = a.Kosher,
                    NonGMO = a.NonGMO,
                    Organic = a.Organic,
                    Vegan = a.Vegan
                }).ToDataTable();

                string sql = "EXEC app.AddOrUpdateCertificationAgencies @agencies";

                context.Database.ExecuteSqlCommand(sql, agencies);
            }
        }

        private bool AllowSignAttributeTraitUpdate(string traitCode, HierarchyClass agencyToUpdate, HierarchyClassTrait agencyTraitToUpdate, string updatedTraitValue, string traitName)
        {
            string currentTaitValue = "0";

            if (agencyTraitToUpdate != null)
            {
                currentTaitValue = agencyTraitToUpdate.traitValue;
            }

            if (currentTaitValue == "1" && updatedTraitValue == "0")
            {
                bool existsAsItemSignAttributeAgency = context.ItemSignAttribute.InItemSignAttribute(traitCode, agencyToUpdate.hierarchyClassID);

                if (existsAsItemSignAttributeAgency)
                {
                    throw new HierarchyClassTraitUpdateException(String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", traitName, agencyToUpdate.hierarchyClassName));
                }
                else
                    return true;
            }
            return true;
        }

    }
}