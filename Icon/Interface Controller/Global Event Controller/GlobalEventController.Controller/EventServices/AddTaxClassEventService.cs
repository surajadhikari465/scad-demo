﻿using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalEventController.Controller.EventServices
{
    public class AddTaxClassEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<AddTaxClassCommand> addTaxClassHandler;
        private IQueryHandler<GetTaxAbbreviationQuery, string> getTaxAbbreviationQueryHandler;

        public AddTaxClassEventService(IrmaContext irmaContext,
            ICommandHandler<AddTaxClassCommand> addTaxClassHandler,
            IQueryHandler<GetTaxAbbreviationQuery, string> getTaxAbbreviationQueryHandler)
        : base(irmaContext)
        {
            this.addTaxClassHandler = addTaxClassHandler;
            this.getTaxAbbreviationQueryHandler = getTaxAbbreviationQueryHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(AddTaxClassEventService), ReferenceId, Message, Region);

            var taxAbbreviationQuery = new GetTaxAbbreviationQuery()
            {
                HierarchyClassId = ReferenceId.Value
            };
            string taxAbbreviationTraitValue = getTaxAbbreviationQueryHandler.Handle(taxAbbreviationQuery);
            if (String.IsNullOrWhiteSpace(taxAbbreviationTraitValue))
            {
                throw new InvalidOperationException($"The tax class doesn't have an abbreviation. (HierarchyClassId {ReferenceId.Value})");
            }

            var addTaxClassCommand = new AddTaxClassCommand
            {
                TaxClassDescription = taxAbbreviationTraitValue,
                TaxCode = Message
            };
            addTaxClassHandler.Handle(addTaxClassCommand);

            this.irmaContext.SaveChanges();
        }
    }
}
