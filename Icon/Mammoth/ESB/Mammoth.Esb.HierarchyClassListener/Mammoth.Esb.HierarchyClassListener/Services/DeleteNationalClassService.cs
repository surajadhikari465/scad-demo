using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class DeleteNationalClassService : IHierarchyClassService<DeleteNationalClassRequest>
    {
        private ICommandHandler<DeleteNationalClassParameter> deleteNationalClassesCommand;

        public DeleteNationalClassService(ICommandHandler<DeleteNationalClassParameter> deleteNationalHierarchiesCommand)
        {
            this.deleteNationalClassesCommand = deleteNationalHierarchiesCommand;
        }

        public void ProcessHierarchyClasses(DeleteNationalClassRequest request)
        {
            var nationalClasses = request.HierarchyClasses
                .Where(hc => hc.HierarchyId == Hierarchies.National && hc.Action == ActionEnum.Delete)
                .ToList();

            if (nationalClasses.Any())
            {
                var commandParameters = new DeleteNationalClassParameter { NationalClasses = nationalClasses };
                this.deleteNationalClassesCommand.Execute(commandParameters);
            }
        }
    }
}
