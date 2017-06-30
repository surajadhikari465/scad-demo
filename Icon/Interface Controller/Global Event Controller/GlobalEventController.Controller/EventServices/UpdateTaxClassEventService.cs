using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using System;

namespace GlobalEventController.Controller.EventServices
{
    public class UpdateTaxClassEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<UpdateTaxClassCommand> updateTaxClassHandler;
        private IQueryHandler<GetTaxAbbreviationQuery, string> getTaxAbbreviationQueryHandler;

        public UpdateTaxClassEventService(
            ICommandHandler<UpdateTaxClassCommand> updateTaxClassHandler,
            IQueryHandler<GetTaxAbbreviationQuery, string> getTaxAbbreviationQueryHandler)
        {
            this.updateTaxClassHandler = updateTaxClassHandler;
            this.getTaxAbbreviationQueryHandler = getTaxAbbreviationQueryHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(UpdateTaxClassEventService), ReferenceId, Message, Region);

            var taxAbbreviationQuery = new GetTaxAbbreviationQuery()
            {
                HierarchyClassId = ReferenceId.Value
            };
            string taxAbbreviationTraitValue = getTaxAbbreviationQueryHandler.Handle(taxAbbreviationQuery);
            if (String.IsNullOrWhiteSpace(taxAbbreviationTraitValue))
            {
                throw new InvalidOperationException($"The tax class doesn't have an abbreviation. (HierarchyClassId {ReferenceId.Value})");
            }

            UpdateTaxClassCommand updateTaxClassCommand = new UpdateTaxClassCommand
            {
                TaxClassDescription = taxAbbreviationTraitValue,
                TaxCode = Message
            };
            updateTaxClassHandler.Handle(updateTaxClassCommand);
        }
    }
}
