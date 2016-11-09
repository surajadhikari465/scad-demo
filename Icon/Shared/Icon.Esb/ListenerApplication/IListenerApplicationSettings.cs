using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.ListenerApplication
{
    public interface IListenerApplicationSettings
    {
        string ListenerApplicationName { get; set; }
        string EmailSubjectPrefix { get; set; }
        string EmailSubjectConnectionSuccess { get; set; }
        string EmailSubjectError { get; set; }
        int NumberOfListenerThreads { get; set; }
    }
}
