using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Irma.Framework;

namespace GlobalEventController.Controller.EventServices
{
    public class UpdateTaxClassEventService : TaxClassEventServiceBase
    {
        private readonly IrmaContext irmaContext;
        private ICommandHandler<UpdateTaxClassCommand> updateTaxClassHandler;

        public UpdateTaxClassEventService(IrmaContext irmaContext,
            ICommandHandler<UpdateTaxClassCommand> updateTaxClassHandler,
            IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassHandler) 
            : base(getHierarchyClassHandler)
        {
            this.irmaContext = irmaContext;
            this.updateTaxClassHandler = updateTaxClassHandler;
        }

        public override void Run()
        {
            base.VerifyEventInformation();
            string taxAbbreviation = base.GetTaxAbbreviation(ReferenceId.Value);
            UpdateTaxClassCommand updateTaxClass = new UpdateTaxClassCommand { TaxClassDescription = taxAbbreviation, TaxCode = Message };
            updateTaxClassHandler.Handle(updateTaxClass);

            this.irmaContext.SaveChanges();
        }
    }
}
