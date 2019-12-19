using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Data;
using System.Data.SqlClient;
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
                    if (command.TaxClassDescription.Length > 50)
                    {
                        command.TaxClassDescription = command.TaxClassDescription.Substring(0, 50);
                    }
                    SqlParameter TaxClassDesc = new SqlParameter("TaxClassDesc", SqlDbType.VarChar);
                    SqlParameter ExternalTaxGroupCode = new SqlParameter("ExternalTaxGroupCode", SqlDbType.VarChar);

                    TaxClassDesc.Value = command.TaxClassDescription;
                    ExternalTaxGroupCode.Value = command.TaxCode;
                    context.Database.ExecuteSqlCommand("EXEC dbo.PopulateDefaultTaxFlags @TaxClassDesc, @ExternalTaxGroupCode", TaxClassDesc, ExternalTaxGroupCode);
                }
            }
        }
    }
}