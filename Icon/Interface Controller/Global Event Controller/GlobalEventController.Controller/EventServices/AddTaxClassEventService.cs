using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventServices
{
    public class AddTaxClassEventService : TaxClassEventServiceBase
    {
        private readonly IrmaContext irmaContext;
        private ICommandHandler<AddTaxClassCommand> addTaxClassHandler;

        public AddTaxClassEventService(IrmaContext irmaContext,
            ICommandHandler<AddTaxClassCommand> addTaxClassHandler,
            IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassHandler) 
                : base(getHierarchyClassHandler)
        {
            this.irmaContext = irmaContext;
            this.addTaxClassHandler = addTaxClassHandler;
        }

        public override void Run()
        {
            base.VerifyEventInformation();
            string taxAbbreviation = base.GetTaxAbbreviation(ReferenceId.Value);

            AddTaxClassCommand addTaxClass = new AddTaxClassCommand { TaxClassDescription = taxAbbreviation, TaxCode = Message };
            addTaxClassHandler.Handle(addTaxClass);

            this.irmaContext.SaveChanges();
        }
    }
}
