using System;
using System .Data;

namespace MammothWebApi.DataAccess.Commands
{
	public class AddOrUpdateItemLocaleCommand
	{
		public string Region { get; set; }
		public DateTime Timestamp { get; set; }
		public Guid TransactionId { get; set; }
	}
}
