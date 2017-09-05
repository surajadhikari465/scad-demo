using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSupport.DataAccess;

namespace WebSupport.Models
{
    public static class StaticData
    {
        public static string[] WholeFoodsRegions
        {
            get
            {
                var regions = new List<string>(DataConstants.WholeFoodsRegions.Length + 1);
                regions.Add(ValidationConstants.PromptToSelectRegion);
                regions.AddRange(DataConstants.WholeFoodsRegions);
                return regions.ToArray();
            }
        }

		public static string[] DownstreamSystems => DataConstants.DownstreamSystems;
	}
}