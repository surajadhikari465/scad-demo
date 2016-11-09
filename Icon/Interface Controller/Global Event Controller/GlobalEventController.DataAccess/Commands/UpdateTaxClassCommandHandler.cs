using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateTaxClassCommandHandler : ICommandHandler<UpdateTaxClassCommand>
    {
        private readonly IrmaContext context;

        public UpdateTaxClassCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(UpdateTaxClassCommand command)
        {
            TaxClass taxClassToUpdate = context.TaxClass
                .SingleOrDefault(tc => tc.ExternalTaxGroupCode != null && (tc.ExternalTaxGroupCode == command.TaxCode || tc.TaxClassDesc.Contains(command.TaxCode)));

            if (taxClassToUpdate == null)
            {
                command.TaxClassId = 0;
                return;
            }

            command.TaxClassId = taxClassToUpdate.TaxClassID;

            if (taxClassToUpdate.TaxClassDesc != command.TaxClassDescription || taxClassToUpdate.ExternalTaxGroupCode != command.TaxCode)
            {
                taxClassToUpdate.TaxClassDesc = command.TaxClassDescription;
                taxClassToUpdate.ExternalTaxGroupCode = command.TaxCode;
            }
        }
    }
}
