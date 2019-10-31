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
	public class MessageExportViewModel
	{
		public enum QueueName { ArchivedMessage, InventoryEvents, PurchaseOrderEvents, ReceiptEvents, TransferOrderEvents }
		public enum MessageTypeName { Inventory, PurchaseOrder, Receipt, TransferOrder }
		public enum StatusName { Failed, Processed, Unprocessed }

		public string Error { get; set; }
		public bool IsSuccess { get { return String.IsNullOrEmpty(Error); } }
		public DataTable ResultTable { get; set; }
		public IEnumerable<SelectListItem> OptionsForRegion { get; set; }
		public IEnumerable<SelectListItem> OptionsForStores { get; set; }
		public SelectListItem[] Queues { get; set; }
		public SelectListItem[] MessageTypes { get; set; }
		public SelectListItem[] EventTypes { get; set; }
		public SelectListItem[] Status { get; set; }

		[Display(Name = "Key ID")]
		[RegularExpression(ValidationConstants.RegExNumeric, ErrorMessage = ValidationConstants.InvalidNumericInput)]
		public string KeyID { get; set; }

		[Display(Name = "Secondary Key ID")]
		[RegularExpression(ValidationConstants.RegExNumeric, ErrorMessage = ValidationConstants.InvalidNumericInput)]
		public string SecondaryKeyID { get; set; }

		[Required]
		[Display(Name = "Message / Event Queue")]
		public int QueueIndex { get; set; }

		[Required]
		[Display(Name = "Region")]
		public int RegionIndex { get; set; }

		[Display(Name = "Store (ArchivedMessage Only)")]
		public int StoreIndex { get; set; }

		[Display(Name = "Status (ArchivedMessage Only)")]
		public int StatusIndex { get; set; }

		[Display(Name = "Message Type")]
		public int MessageTypeIndex { get; set; }

		[Display(Name = "Event Type")]
		public string EventType { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Start Datetime")]
		public DateTime? StartDatetime { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "End Datetime")]
		public DateTime? EndDatetime { get; set; }
		public bool? RequeueSuccess { get; set; }

		public MessageExportViewModel()
		{
			var initialOption = new string[] { "- Select a region first -" };
			OptionsForStores = SelectListHelper.ArrayToSelectList(initialOption);
			OptionsForRegion = SelectListHelper.ArrayToSelectList(StaticData.WholeFoodsRegions.ToArray(), 0);

			SelectListItem initialAllItem = new SelectListItem { Text = " - All - ", Value = "0", Selected = true };

			Queues = Enum.GetNames(typeof(QueueName)).Cast<string>().OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = ((int)Enum.Parse(typeof(QueueName), x)).ToString(), Selected = (i == 0) })
						 .ToArray();

			List<SelectListItem> messageTypeList = Enum.GetNames(typeof(MessageTypeName)).Cast<string>().OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = (((int)Enum.Parse(typeof(MessageTypeName), x) + 1)).ToString() })
						 .ToList();
			messageTypeList.Insert(0, initialAllItem);
			MessageTypes = messageTypeList.ToArray();

			List<SelectListItem> eventTypeList = QueueEventTypes.Events.OrderBy(x => x.Value)
				.Select((x, i) => new SelectListItem { Text = x.Value, Value = x.Key.ToString() })
				.OrderBy(x => x.Text).ToList();
			eventTypeList.Insert(0, initialAllItem);
			EventTypes = eventTypeList.ToArray();

			List<SelectListItem> statusList = Enum.GetNames(typeof(StatusName)).Cast<string>().OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
						 .Select((x, i) => new SelectListItem { Text = x, Value = (((int)Enum.Parse(typeof(StatusName), x) + 1)).ToString() })
						 .ToList();
			statusList.Insert(0, initialAllItem);
			Status = statusList.ToArray();

		}

		public void SetStoreOptions(IEnumerable<StoreViewModel> storeSelections)
		{
			OptionsForStores = SelectListHelper.StoreArrayToSelectList(storeSelections.ToArray());
			
		}
	}
}