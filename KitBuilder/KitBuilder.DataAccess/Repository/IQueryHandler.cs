using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Repository
{
	public interface IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult>
	{
		TResult Search(TParameters parameters);
	}
}
