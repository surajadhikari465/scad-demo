using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ValidateHierarchyClassesCommandHandler : ICommandHandler<ValidateHierarchyClassesCommand>
    {
        private IRenewableContext<IconContext> globalContext;

        public ValidateHierarchyClassesCommandHandler(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(ValidateHierarchyClassesCommand data)
        {
            var hierarchyClasses = data.HierarchyClasses
                .Where(i => i.ErrorCode == null)
                .Select(i => new ValidateHierarchyClassModel(i))
                .ToTvp("hierarchyClasses", "infor.ValidateHierarchyClassType");

            var validateResult = globalContext.Context.Database.SqlQuery<ValidateHierarchyClassesResultModel>("exec infor.ValidateHierarchyClasses @hierarchyClasses", hierarchyClasses).ToList();

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
