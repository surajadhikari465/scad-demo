using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Models
{
	/// <summary>
	/// Archived Message.
	/// </summary>
    public class ArchivedMessage
    {
		/// <summary>
		/// Gets or sets the Archived Message Id.
		/// </summary>
		public int ArchiveID { get; set; }

		/// <summary>
		/// Gets or sets the Store Id.
		/// </summary>
		public int StoreBU { get; set; }

		/// <summary>
		/// Gets or sets the Event.
		/// </summary>
		public string Event { get; set; }

		/// <summary>
		/// Gets or sets the Key ID (PO Id, Invoice Id, etc.)
		/// </summary>
		public int KeyID { get; set; }

		/// <summary>
		/// Gets or sets the secondary Id.
		/// </summary>
		public int SecondaryKeyID { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the Insert Date.
		/// </summary>
		public string InsertDate { get; set; }

		/// <summary>
		/// Gets or sets the Reset by.
		/// </summary>
		public string ResetBy { get; set; }

	}
}
