using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb
{
    public class MockEsbProducer : IEsbProducer
    {
        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {

        }

        public Icon.Esb.EsbConnectionSettings Settings
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsConnected
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<TIBCO.EMS.EMSException> ExceptionHandlers;

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}