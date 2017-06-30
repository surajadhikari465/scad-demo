using Icon.DbContextFactory;
using Irma.Framework;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public interface IRegionalIrmaDbContextFactory : IDbContextFactory<IrmaContext>
    {
        string Region { get; set; }
        int CommandTimeout { get; set; }
    }
}
