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
    public class GenerateHierarchyClassMessagesCommandHandler : ICommandHandler<GenerateHierarchyClassMessagesCommand>
    {
        private IRenewableContext<IconContext> context;

        public GenerateHierarchyClassMessagesCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(GenerateHierarchyClassMessagesCommand data)
        {
            if (data.HierarchyClasses.Any())
            {
                var dataAccessModels = data.HierarchyClasses.ToDataAccessModels();

                SqlParameter hierarchyClasses = dataAccessModels
                    .Select(hc => new
                    {
                        hc.HierarchyClassId,
                        hc.HierarchyClassName,
                        hc.HierarchyId,
                        hc.HierarchyLevelName,
                        hc.ParentHierarchyClassId,
                        hc.ActionId
                    })
                    .ToTvp("@hierarchyClasses", "infor.HierarchyClassType");

                string sqlCommand = "EXEC infor.HierarchyClassGenerateMessages @hierarchyClasses";
                try
                {
                    context.Context.Database.ExecuteSqlCommand(
                        sqlCommand,
                        hierarchyClasses);
                }
                catch (Exception ex)
                {
                    string errorDetails = ApplicationErrors.Descriptions
                        .AddOrUpdateHierarchyClassError + " Exception: " + ex.ToString();
                    foreach (var hierarchyClass in data.HierarchyClasses)
                    {
                        hierarchyClass.ErrorCode = ApplicationErrors.Codes.GenerateHierarchyClassMessagesError;
                        hierarchyClass.ErrorDetails = errorDetails;
                    }
                }
            }
        }
    }
}
