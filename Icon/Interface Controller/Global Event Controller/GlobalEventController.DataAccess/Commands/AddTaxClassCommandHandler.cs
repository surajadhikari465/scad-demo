using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddTaxClassCommandHandler : ICommandHandler<AddTaxClassCommand>
    {
        private readonly IrmaContext context;

        public AddTaxClassCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(AddTaxClassCommand command)
        {
            TaxClass taxClass = this.context.TaxClass
                .AsEnumerable()
                .SingleOrDefault(i => i.TaxClassDesc == command.TaxClassDescription || i.TaxClassDesc.Split(' ')[0] == command.TaxCode);

            if (taxClass == null)
            {
                taxClass = new TaxClass();
                taxClass.TaxClassDesc = command.TaxClassDescription;
                taxClass.ExternalTaxGroupCode = command.TaxCode;
                this.context.TaxClass.Add(taxClass);
            }

            command.TaxClassId = taxClass.TaxClassID;
        }
    }
}
