using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
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
        private IDbContextFactory<IconContext> contextFactory;

        public GenerateHierarchyClassMessagesCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
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
                    using (var context = contextFactory.CreateContext())
                    {
                        context.Database.ExecuteSqlCommand(
                            sqlCommand,
                            hierarchyClasses);
                    }
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
