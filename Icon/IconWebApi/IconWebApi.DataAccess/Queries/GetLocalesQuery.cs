using Icon.Common.DataAccess;
using IconWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace IconWebApi.DataAccess.Queries
{
	public class GetLocalesQuery : IQuery<IEnumerable<GenericLocale>>
	{
		public bool includeAddress { get; set; }
	}
}
