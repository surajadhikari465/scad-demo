using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingPriceCommand
    {
        public List<StagingPriceModel> Prices{ get; set; }
    }
}
