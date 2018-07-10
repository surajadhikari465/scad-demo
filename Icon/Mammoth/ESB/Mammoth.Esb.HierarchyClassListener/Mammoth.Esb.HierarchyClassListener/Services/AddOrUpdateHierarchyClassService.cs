﻿using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Commands;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class AddOrUpdateHierarchyClassService : IHierarchyClassService<AddOrUpdateHierarchyClassRequest>
    {
        private ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler;
        private ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand> addOrUpdateFinancialHierarchyClassesCommandHandler;
        private ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand> addOrUpdateMerchandiseHierarchyLineageCommandHandler;
        private ICommandHandler<AddOrUpdateNationalHierarchyLineageCommand> addOrUpdateNationalHierarchyLineageCommandHandler;

        public AddOrUpdateHierarchyClassService(ICommandHandler<AddOrUpdateHierarchyClassesCommand> addOrUpdateHierarchyClassesCommandHandler,
            ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand> addOrUpdateMerchandiseHierarchyLineageCommandHandler,
            ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand> addOrUpdateFinancialHierarchyClassesCommandHandler,
            ICommandHandler<AddOrUpdateNationalHierarchyLineageCommand> addOrUpdateNationalHierarchyLineageCommandHandler)
        {
            this.addOrUpdateHierarchyClassesCommandHandler = addOrUpdateHierarchyClassesCommandHandler;
            this.addOrUpdateMerchandiseHierarchyLineageCommandHandler = addOrUpdateMerchandiseHierarchyLineageCommandHandler;
            this.addOrUpdateFinancialHierarchyClassesCommandHandler = addOrUpdateFinancialHierarchyClassesCommandHandler;
            this.addOrUpdateNationalHierarchyLineageCommandHandler = addOrUpdateNationalHierarchyLineageCommandHandler;
        }

        public void ProcessHierarchyClasses(AddOrUpdateHierarchyClassRequest request)
        {
            var financialHierarchyClasses = request.HierarchyClasses.Where(hc => hc.HierarchyId == Hierarchies.Financial).ToList();
            var hierarchyClassesWithoutFinancial = request.HierarchyClasses.Except(financialHierarchyClasses).ToList();

            if (hierarchyClassesWithoutFinancial.Any())
            {
                addOrUpdateHierarchyClassesCommandHandler.Execute(new AddOrUpdateHierarchyClassesCommand
                {
                    HierarchyClasses = hierarchyClassesWithoutFinancial
                });

                var merchandiseHierarchyClasses = request.HierarchyClasses.Where(hc => hc.HierarchyId == Hierarchies.Merchandise);
                if (merchandiseHierarchyClasses.Any())
                {
                    addOrUpdateMerchandiseHierarchyLineageCommandHandler.Execute(new AddOrUpdateMerchandiseHierarchyLineageCommand
                    {
                        HierarchyClasses = merchandiseHierarchyClasses.ToList()
                    });
                }

                var nationalHierarchyClasses = request.HierarchyClasses.Where(hc => hc.HierarchyId == Hierarchies.National);
                if (nationalHierarchyClasses.Any())
                {
                    addOrUpdateNationalHierarchyLineageCommandHandler.Execute(new AddOrUpdateNationalHierarchyLineageCommand
                    {
                        HierarchyClasses = nationalHierarchyClasses.ToList()
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
