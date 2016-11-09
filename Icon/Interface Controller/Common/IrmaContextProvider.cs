using Icon.Common.Context;
using Irma.Framework;

namespace InterfaceController.Common
{
    public interface IIrmaContextProvider
    {
        IrmaContext GetRegionalContext(string connectionString);
    }

    public class IrmaContextProvider : IIrmaContextProvider
    {
        public IrmaContext GetRegionalContext(string connectionString)
        {
            return new IrmaContext(connectionString);
        }
    }
}
