using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlogController.Common
{
    public static class StartupOptions
    {
        public static int Instance { get; set; }
        public static int MaxTransactionsToProcess { get; set; }

        public static int MaxTlogTransactionsWhenSplit { get; set; }
    }
}
