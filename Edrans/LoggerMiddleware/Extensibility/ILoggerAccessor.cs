using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerMiddleware.Extensibility
{
    public interface ILoggerAccessor
    {
        void AddToLog(Dictionary<string, string> pairs);
        Dictionary<string, string> GetValues();
    }

    public class LoggerAccessor : ILoggerAccessor
    {
        Dictionary<string, string> Values { get; set; }

        public LoggerAccessor()
        {
            Values = new Dictionary<string, string>();
        }

        public void AddToLog(Dictionary<string, string> pairs)
        {
            foreach (var item in pairs)
            {
                Values.Add(item.Key, item.Value);
            }
        }

        public Dictionary<string, string> GetValues() => Values;
    }
}
