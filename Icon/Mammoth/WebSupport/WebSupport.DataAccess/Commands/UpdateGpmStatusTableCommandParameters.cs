using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Commands
{
    public class UpdateGpmStatusTableCommandParameters
    {
        public UpdateGpmStatusTableCommandParameters() { }

        public UpdateGpmStatusTableCommandParameters(IEnumerable<RegionGpmStatus> regions)
            : this(regions?.ToList()) { }

        public UpdateGpmStatusTableCommandParameters(IList<RegionGpmStatus> regions) : this()
        {
            Regions = regions;
        }

        public IList<RegionGpmStatus> Regions { get; set; }
    }
}
