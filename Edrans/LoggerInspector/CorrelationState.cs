using log4net;
using System.Diagnostics;

namespace LoggerInspector
{
    public class CorrelationState
    {
        public string CorrelationID { get; }
        public Stopwatch Watch { get; }

        public ILog Logger { get; }

        public string methodName { get; }

        public CorrelationState(string correlationID, string methodName, ILog logger)
        {
            CorrelationID = correlationID;
            Watch = new Stopwatch();
            Logger = logger;
            this.methodName = methodName;
        }

        public void StartWatch()
        {
            Watch.Start();
        }
    }
}
