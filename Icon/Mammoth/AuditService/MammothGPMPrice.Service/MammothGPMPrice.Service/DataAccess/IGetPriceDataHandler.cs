using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothGpmService.DataAccess
{
	public interface IGetPriceDataHandler
	{
		bool GetAuditFileByRegion(string region);
		void DeleteFile(string region);
	}
}
