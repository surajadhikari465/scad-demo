using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconWebApi.Service.Services
{
	public interface IQueryService<TRequest, TResult>
	{
		TResult Get(TRequest request);
	}
}
