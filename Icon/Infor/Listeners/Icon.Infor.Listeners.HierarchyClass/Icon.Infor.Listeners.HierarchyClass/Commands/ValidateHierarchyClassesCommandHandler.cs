using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ValidateHierarchyClassesCommandHandler : ICommandHandler<ValidateHierarchyClassesCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;
        private HierarchyClassListenerSettings settings;

        public ValidateHierarchyClassesCommandHandler(IDbContextFactory<IconContext> contextFactory, HierarchyClassListenerSettings settings)
        {
            this.contextFactory = contextFactory;
            this.settings = settings;
        }

        public void Execute(ValidateHierarchyClassesCommand data)
        {
            var hierarchyClasses = data.HierarchyClasses
                .Where(hc => hc.ErrorCode == null)
                .Select(hc => new ValidateHierarchyClassModel(hc))
                .ToTvp("hierarchyClasses", "infor.ValidateHierarchyClassType");
            var validateSequenceId = new SqlParameter("validateSequenceId", settings.ValidateSequenceId)
                {
                    DbType = DbType.Boolean
                };

            List<ValidateHierarchyClassesResultModel> validateResult = null;
            using (var context = contextFactory.CreateContext())
            {
                validateResult = context.Database.SqlQuery<ValidateHierarchyClassesResultModel>(
                    "exec infor.ValidateHierarchyClasses @hierarchyClasses, @validateSequenceId", 
                    hierarchyClasses,
                    validateSequenceId)
                    .ToList();
            }

            var errorHierarchyClasses = data.HierarchyClasses.Join(
                validateResult,
                hc => hc.HierarchyClassId,
                v => v.HierarchyClassId,
                (hc, v) => new
                {
                    HierarchyClass = hc,
                    ValidationErrorCode = v.ErrorCode,
                    ValidationErrorDetails = v.ErrorDetails
                })
                .Where(i => i.ValidationErrorCode != null);

            foreach (var hierarchyClass in errorHierarchyClasses)
            {
                hierarchyClass.HierarchyClass.ErrorCode = hierarchyClass.ValidationErrorCode;
                hierarchyClass.HierarchyClass.ErrorDetails = hierarchyClass.ValidationErrorDetails;
            }
        }
    }
}
