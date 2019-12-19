using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateTaxClassCommandHandler : ICommandHandler<UpdateTaxClassCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public UpdateTaxClassCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(UpdateTaxClassCommand command)
        {
            using (var context = contextFactory.CreateContext())
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
                    if (command.TaxClassDescription.Length > 50)
                    {
                        command.TaxClassDescription = command.TaxClassDescription.Substring(0, 50);
                    }
                    taxClassToUpdate.TaxClassDesc = command.TaxClassDescription;
                    taxClassToUpdate.ExternalTaxGroupCode = command.TaxCode;
                }
                context.SaveChanges();
            }
        }
    }
}
