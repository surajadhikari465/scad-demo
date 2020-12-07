using System.Diagnostics;

namespace LoggerInspector
{
    public class CorrelationState
    {
        public string CorrelationID { get; }
        public Stopwatch Watch { get; }

        public CorrelationState(string correlationID)
        {
            CorrelationID = correlationID;
            Watch = new Stopwatch();
        }

        public CorrelationState StartWatch()
        {
            Watch.Start();
            return this;
        }
    }
}
