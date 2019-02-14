using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.Services
{
	public interface IService<TParameters, TResult>
	{
		TResult Run(TParameters parameters);
	}
}
