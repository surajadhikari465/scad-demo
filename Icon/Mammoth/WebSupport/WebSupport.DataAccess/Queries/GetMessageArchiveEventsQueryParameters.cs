using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
	/// <summary>
	/// GetMessageArchiveEventsQuery Parameters
	/// </summary>
	public class GetMessageArchiveEventsQueryParameters : IQuery<IList<ArchivedMessage>>
    {
		/// <summary>
		/// Gets or sets the region.
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// Gets or sets the Max Records.
		/// </summary>
		public int MaxRecords { get; set; }

		/// <summary>
		/// Gets or sets the Key ID (Invoice ID, Purchase Order ID).
		/// </summary>
		public string KeyID { get; set; }

		/// <summary>
		/// Gets or sets the Secondary Key ID.
		/// </summary>
		public string SecondaryKeyID { get; set; }

		/// <summary>
		/// Gets or sets the Start Datetime.
		/// </summary>
		public DateTime? StartDatetime { get; set; }

		/// <summary>
		/// Gets or sets the End Datetime.
		/// </summary>
		public DateTime? EndDatetime { get; set; }

		/// <summary>
		/// Gets or sets the Event Type.
		/// </summary>
		public string EventType { get; set; }

		/// <summary>
		/// Gets or sets the Queue.
		/// </summary>
		public string Queue { get; set; }

		/// <summary>
		/// Gets or sets the Status.
		/// </summary>
		public string Status { get; set; }
	}
}
