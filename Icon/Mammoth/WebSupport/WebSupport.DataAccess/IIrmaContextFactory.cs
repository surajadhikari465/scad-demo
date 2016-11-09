namespace WebSupport.DataAccess
{
    using Irma.Framework;

    public interface IIrmaContextFactory
    {
        IrmaContext CreateContext(string regionAbbreviation);
    }
}
