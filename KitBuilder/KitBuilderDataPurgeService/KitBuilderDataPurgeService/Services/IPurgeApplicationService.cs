using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitBuilder.DataPurge.Service.Services
{
	public interface IPurgeApplicationService
	{
		void Start();
		void Stop();
	}
}
