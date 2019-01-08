using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothGpmService.Models
{
	public class GpmPriceDataQuery : IQuery<List<DbPriceData>>
	{
		public int Instance { get; set; }
		public string Region { get; set; }
	}
}
