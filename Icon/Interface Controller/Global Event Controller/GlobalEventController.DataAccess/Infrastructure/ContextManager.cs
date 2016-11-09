using Icon.Framework;
using InterfaceController.Common;
using Irma.Framework;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public class ContextManager : IContextManager
    {
        public IconContext IconContext { get; set; }
        public Dictionary<string, IrmaContext> IrmaContexts { get; set; }

        public ContextManager()
        {
            IconContext = new IconContext();
            IrmaContexts = new Dictionary<string, IrmaContext>();
            CreateIrmaContexts();
        }

        public void RefreshContexts()
        {
            if(IconContext != null)
            {
                IconContext.Dispose();
                IconContext = new IconContext();
            }

            foreach (var kvp in IrmaContexts)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.Dispose();
                }
            }
            CreateIrmaContexts();
        }

        private void CreateIrmaContexts()
        {
            IrmaContexts.Clear();
            IrmaContexts.Add("FL", new IrmaContext(ConnectionBuilder.GetConnection("FL")));
            IrmaContexts.Add("MA", new IrmaContext(ConnectionBuilder.GetConnection("MA")));
            IrmaContexts.Add("MW", new IrmaContext(ConnectionBuilder.GetConnection("MW")));
            IrmaContexts.Add("NA", new IrmaContext(ConnectionBuilder.GetConnection("NA")));
            IrmaContexts.Add("NC", new IrmaContext(ConnectionBuilder.GetConnection("NC")));
            IrmaContexts.Add("NE", new IrmaContext(ConnectionBuilder.GetConnection("NE")));
            IrmaContexts.Add("PN", new IrmaContext(ConnectionBuilder.GetConnection("PN")));
            IrmaContexts.Add("RM", new IrmaContext(ConnectionBuilder.GetConnection("RM")));
            IrmaContexts.Add("SO", new IrmaContext(ConnectionBuilder.GetConnection("SO")));
            IrmaContexts.Add("SP", new IrmaContext(ConnectionBuilder.GetConnection("SP")));
            IrmaContexts.Add("SW", new IrmaContext(ConnectionBuilder.GetConnection("SW")));
            IrmaContexts.Add("UK", new IrmaContext(ConnectionBuilder.GetConnection("UK")));
        }
    }
}
