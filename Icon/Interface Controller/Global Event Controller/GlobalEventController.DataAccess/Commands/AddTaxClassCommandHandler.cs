using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddTaxClassCommandHandler : ICommandHandler<AddTaxClassCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public AddTaxClassCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(AddTaxClassCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                TaxClass taxClass = context.TaxClass
                    .AsEnumerable()
                    .SingleOrDefault(i => i.TaxClassDesc == command.TaxClassDescription || i.TaxClassDesc.Split(' ')[0] == command.TaxCode);

                if (taxClass == null)
                {
                    taxClass = new TaxClass();
                    taxClass.TaxClassDesc = command.TaxClassDescription;
                    taxClass.ExternalTaxGroupCode = command.TaxCode;
                    context.TaxClass.Add(taxClass);
                }

                context.SaveChanges();
                command.TaxClassId = taxClass.TaxClassID;
            }
        }
    }
}
