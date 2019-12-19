using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface IRegionRepository
    {
        Region ForAbbrev(string abbrev);
        IEnumerable<string> RegionAbbreviations();
        IEnumerable<string> RegionNames();
        Region ForName(string name);
    }
}
