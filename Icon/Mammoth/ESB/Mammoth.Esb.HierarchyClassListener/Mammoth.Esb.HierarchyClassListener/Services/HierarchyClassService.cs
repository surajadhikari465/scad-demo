using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class HierarchyClassService : IHierarchyClassService
    {
        private ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler;
        private ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand> addOrUpdateFinancialHierarchyClassesCommandHandler;
        private ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand> addOrUpdateMerchandiseHierarchyLineageCommandHandler;

        public HierarchyClassService(ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler,
            ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand> addOrUpdateMerchandiseHierarchyLineageCommandHandler,
            ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand> addOrUpdateFinancialHierarchyClassesCommandHandler)
        {
            this.addOrUpdateHierarchyClassesCommandHandler = addOrUpdateHierarchyClassesCommandHandler;
            this.addOrUpdateMerchandiseHierarchyLineageCommandHandler = addOrUpdateMerchandiseHierarchyLineageCommandHandler;
            this.addOrUpdateFinancialHierarchyClassesCommandHandler = addOrUpdateFinancialHierarchyClassesCommandHandler;
        }

        public void AddOrUpdateHierarchyClasses(AddOrUpdateHierarchyClassesCommand command)
        {
            var financialHierarchyClasses = command.HierarchyClasses.Where(hc => hc.HierarchyId == Hierarchies.Financial).ToList();
            var hierarchyClassesWithoutFinancial = command.HierarchyClasses.Except(financialHierarchyClasses).ToList();

            if (hierarchyClassesWithoutFinancial.Any())
            {
                addOrUpdateHierarchyClassesCommandHandler.Execute(command);

                var merchandiseHierarchyClasses = command.HierarchyClasses.Where(hc => hc.HierarchyId == Hierarchies.Merchandise);
                if (merchandiseHierarchyClasses.Any())
                {
                    addOrUpdateMerchandiseHierarchyLineageCommandHandler.Execute(new AddOrUpdateMerchandiseHierarchyLineageCommand
                    {
                        HierarchyClasses = merchandiseHierarchyClasses.ToList()
                    });
                }
            }

            if (financialHierarchyClasses.Any())
            {
                addOrUpdateFinancialHierarchyClassesCommandHandler.Execute(new AddOrUpdateFinancialHierarchyClassCommand { HierarchyClasses = financialHierarchyClasses });
            }
        }
    }
}
