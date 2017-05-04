using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class AddOrUpdateHierarchyClassesCommandHandler : ICommandHandler<AddOrUpdateHierarchyClassesCommand>
    {
        private IRenewableContext<IconContext> context;
        private List<string> regions;

        public AddOrUpdateHierarchyClassesCommandHandler(
            IRenewableContext<IconContext> context,
            List<string> regions)
        {
            this.context = context;
            this.regions = regions;
        }

        public void Execute(AddOrUpdateHierarchyClassesCommand data)
        {
            var hierarchyName = data.HierarchyClasses.First().HierarchyName;

            var dataAccessModels = data.HierarchyClasses.ToDataAccessModels();

            SqlParameter hierarchyClasses = dataAccessModels.Select(hc => new
            {
                hc.HierarchyClassId,
                hc.HierarchyClassName,
                hc.HierarchyId,
                hc.HierarchyLevelName,
                hc.ParentHierarchyClassId,
                hc.ActionId
            }).ToTvp("@hierarchyClasses", "infor.HierarchyClassType");
            SqlParameter hierarchyClassTraits = dataAccessModels.ToTraitDataAccessModels().ToTvp("@hierarchyClassTraits", "infor.HierarchyClassTraitType");
            SqlParameter regionParameters = regions.Select(r => new { RegionCode = r }).ToTvp("@regions", "infor.RegionCodeList");

            try
            {
                context.Context.Database.ExecuteSqlCommand(
                    "EXEC infor.HierarchyClassAddOrUpdate @hierarchyClasses, @hierarchyClassTraits, @regions",
                    hierarchyClasses,
                    hierarchyClassTraits,
                    regionParameters);
            }
            catch(Exception ex)
            {
                string errorDetails = ApplicationErrors.Descriptions.AddOrUpdateHierarchyClassError + " Exception: " + ex.ToString();
                foreach (var hierarchyClass in data.HierarchyClasses)
                {
                    hierarchyClass.ErrorCode = ApplicationErrors.Codes.AddOrUpdateHierarchyClassError;
                    hierarchyClass.ErrorDetails = errorDetails;
                }
            }
        }
    }
}
