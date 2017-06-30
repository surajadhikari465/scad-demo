using System;
using Irma.Framework;
using InterfaceController.Common;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public class RegionalIrmaDbContextFactory : IRegionalIrmaDbContextFactory
    {
        private string connectionString;

        private string region;
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                connectionString = ConnectionBuilder.GetConnection(region);
            }
        }
        public int CommandTimeout { get; set; }

        public IrmaContext CreateContext()
        {
            var context = new IrmaContext(connectionString);
            if (CommandTimeout != 0)
            {
                context.Database.CommandTimeout = CommandTimeout;
            }
            return context;
        }

        public IrmaContext CreateContext(string connectionStringName)
        {
            var context = new IrmaContext(connectionStringName);
            if (CommandTimeout != 0)
            {
                context.Database.CommandTimeout = CommandTimeout;
            }
            return context;
        }

        public IrmaContext CreateContext(object settings)
        {
            throw new NotImplementedException();
        }
    }
}
