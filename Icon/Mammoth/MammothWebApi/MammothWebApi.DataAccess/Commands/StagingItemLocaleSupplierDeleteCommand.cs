using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleSupplierDeleteCommand
    {
        public List<StagingItemLocaleSupplierDeleteModel> ItemLocaleSupplierDeletes { get; set; }
    }
}
