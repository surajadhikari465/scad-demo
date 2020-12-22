using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WebSupport.DataAccess;
using WebSupport.Models;
using System.Collections.Generic;
using WebSupport.Helpers;

namespace WebSupport.ViewModels
{
	/// <summary>
	/// Re-Queue Event View-Model
	/// </summary>
	public class EventReQueueViewModel
	{
		/// <summary>
		/// Initializes an instance of ReQueueEventViewModel
		/// </summary>
		public EventReQueueViewModel()
		{
			var initialOption = new string[] { "- Select a region first -" };
			OptionsForRegion = SelectListHelper.ArrayToSelectList(StaticData.WholeFoodsRegions.ToArray(), 0);

			SelectListItem initialAllItem = new SelectListItem { Text = " - All - ", Value = "0", Selected = true };

			Queues = Enum.GetNames(typeof(QueueName))
						 .Cast<string>()
						 .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = ((int)Enum.Parse(typeof(QueueName), x)).ToString(), Selected = (i == 0) })
						 .ToArray();

			List<SelectListItem> messageTypeList = Enum.GetNames(typeof(MessageTypeName))
						 .Cast<string>()
						 .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = (((int)Enum.Parse(typeof(MessageTypeName), x) + 1)).ToString() })
						 .ToList();
			messageTypeList.Insert(0, initialAllItem);
			MessageTypes = messageTypeList.ToArray();

			List<SelectListItem> eventTypeList = QueueEventTypes.Events
				.OrderBy(x => x.Value)
				.Select((x, i) => new SelectListItem { Text = x.Value, Value = x.Key.ToString() })
				.OrderBy(x => x.Text).ToList();
			eventTypeList.Insert(0, initialAllItem);

			EventTypes = eventTypeList.ToArray();

			List<SelectListItem> statusList = Enum.GetNames(typeof(StatusName))
						 .Cast<string>()
						 .OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = (((int)Enum.Parse(typeof(StatusName), x) + 1)).ToString() })
						 .ToList();
			statusList.Insert(0, initialAllItem);
			Status = statusList.ToArray();

		}

		/// <summary>
		/// Queue Name enum
		/// </summary>
		public enum QueueName { InventoryEvents, PurchaseOrderEvents, ReceiptEvents, TransferOrderEvents }
		
		/// <summary>
		/// Message Type Enum
		/// </summary>
		public enum MessageTypeName { Inventory, PurchaseOrder, Receipt, TransferOrder }
		
		/// <summary>
		/// Status Name Enum
		/// </summary>
		public enum StatusName { Failed, Processed }

		/// <summary>
		/// Gets or sets the Global Request Error
		/// </summary>
		public string Error { get; set; }
		
		/// <summary>
		/// Gets if the request was a success
		/// </summary>
		public bool IsSuccess { get { return String.IsNullOrEmpty(Error); } }
		
		/// <summary>
		/// Gets or sets the Query results.
		/// </summary>
		public DataTable ResultTable { get; set; }

		/// <summary>
		/// Gets or sets the selected options for region.
		/// </summary>
		public IEnumerable<SelectListItem> OptionsForRegion { get; set; }

		/// <summary>
		/// Gets or sets the selected Event Queues.
		/// </summary>
		public SelectListItem[] Queues { get; set; }

		/// <summary>
		/// Gest or sets the selected Message types.
		/// </summary>
		public SelectListItem[] MessageTypes { get; set; }

		/// <summary>
		/// Gets or sets the selected event types.
		/// </summary>
		public SelectListItem[] EventTypes { get; set; }

		/// <summary>
		/// Gets or sets the selected message status.
		/// </summary>
		public SelectListItem[] Status { get; set; }

		/// <summary>
		/// Gets or sets the Key Id. (PO, Invoice id, etc).
		/// </summary>
		[Display(Name = "Key ID")]
		[RegularExpression(ValidationConstants.RegExNumeric, ErrorMessage = ValidationConstants.InvalidNumericInput)]
		public string KeyID { get; set; }

		/// <summary>
		/// Gets or sets the secondary Key ID
		/// </summary>
		[Display(Name = "Secondary Key ID")]
		[RegularExpression(ValidationConstants.RegExNumeric, ErrorMessage = ValidationConstants.InvalidNumericInput)]
		public string SecondaryKeyID { get; set; }

		/// <summary>
		/// Gets or sets the selected Queue Index.
		/// </summary>
		[Required]
		[Display(Name = "Message / Event Queue")]
		public int QueueIndex { get; set; }

		/// <summary>
		/// Gets or sets the selected Region index
		/// </summary>
		[Required]
		[Display(Name = "Region")]
		public int RegionIndex { get; set; }

		/// <summary>
		/// Gets or sets the selected status index.
		/// </summary>
		[Display(Name = "Status")]
		public int StatusIndex { get; set; }

		/// <summary>
		/// Gets or sets the selected message type index.
		/// </summary>
		[Display(Name = "Message Type")]
		public int MessageTypeIndex { get; set; }

		/// <summary>
		/// Gets or sets the event type.
		/// </summary>
		[Display(Name = "Event Type")]
		public string EventType { get; set; }

		/// <summary>
		/// Gets or sets the start date time.
		/// </summary>
		[DataType(DataType.Date)]
		[Display(Name = "Start Datetime")]
		public DateTime? StartDatetime { get; set; }

		/// <summary>
		/// Gets or sets the end date time.
		/// </summary>
		[DataType(DataType.Date)]
		[Display(Name = "End Datetime")]
		public DateTime? EndDatetime { get; set; }

		/// <summary>
		/// Gets or sets if the request was sucessfull.
		/// </summary>
		public bool? RequeueSuccess { get; set; }
	}
}