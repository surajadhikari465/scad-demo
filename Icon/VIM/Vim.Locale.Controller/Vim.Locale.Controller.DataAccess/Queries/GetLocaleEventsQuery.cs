using System.Collections.Generic;
using Vim.Common.DataAccess;
using Vim.Locale.Controller.DataAccess.Models;

namespace Vim.Locale.Controller.DataAccess.Queries
{
    public class GetLocaleEventsQuery : IQuery<List<LocaleEventModel>>
    {
        public int Instance { get; set; }
        public int FirstAttemptWaitTimeInMinute { get; set; }
        public int SecondAttemptWaitTimeInMinute { get; set; }
        public int ThirdAttemptWaitTimeInMinute { get; set; }
    }
}
