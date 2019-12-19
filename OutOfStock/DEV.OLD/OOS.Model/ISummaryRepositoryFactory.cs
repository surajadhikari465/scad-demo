using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface ISummaryRepositoryFactory
    {
        ISummaryRepository New(string regionAbbrev);
    }
}
