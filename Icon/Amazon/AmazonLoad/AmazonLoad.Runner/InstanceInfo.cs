using System.Diagnostics;

namespace AmazonLoad.Runner
{
    partial class Program
    {
        public class InstanceInfo
        {
            public Process Instance { get; set; }
            public bool IsRunning { get
                {
                    return !this.Instance.HasExited;
                }
            }
            public int StartGroup { get; set; }
            public int EndGroup { get; set; }
        }
    }
}
