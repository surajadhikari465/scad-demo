using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    /// <summary>
    /// Re-Queue-Purchase-Orders-Controller
    /// </summary>
    public class ReQueuePurchaseOrdersController : Controller
    {
        private readonly ILogger logger;
		private readonly IQueryHandler<GetPurchaseOrdersMessagesToResetQueryParameters, IList<PurchaseOrderArchivedMessage>> getPurchaseOrdersMessagesToResetQuery;
		private readonly ICommandHandler<ReQueueMessageArchiveCommandParameters> reQueueMessageArchiveCommandHandler;

		/// <summary>
		/// Initializes an instance of ReQueuePurchaseOrdersController.
		/// </summary>
		/// <param name="logger">ILogger.</param>
		/// <param name="getPurchaseOrdersMessagesToResetQuery">Get Purchase Orders Messages To Reset Query.</param>
		public ReQueuePurchaseOrdersController (
			ILogger logger,
			IQueryHandler<GetPurchaseOrdersMessagesToResetQueryParameters, IList<PurchaseOrderArchivedMessage>> getPurchaseOrdersMessagesToResetQuery,
			ICommandHandler<ReQueueMessageArchiveCommandParameters> reQueueMessageArchiveCommandHandler)
        {
            this.logger = logger;
			this.getPurchaseOrdersMessagesToResetQuery = getPurchaseOrdersMessagesToResetQuery;
			this.reQueueMessageArchiveCommandHandler = reQueueMessageArchiveCommandHandler;
		}

        /// <summary>
		/// Default View
		/// </summary>
		/// <returns>Default view</returns>
        public ActionResult Index()
        {
            return View(new ReQueuePurchaseOrdersViewModel());
        }

		/// <summary>
		/// Process a Reque request
		/// </summary>
		/// <param name="viewModel">Re-Queue Purchase Orders View-Model</param>
		/// <returns>View with the operations results</returns>
		[HttpPost]
		public ActionResult Index(ReQueuePurchaseOrdersViewModel viewModel)
		{
			try
			{
				viewModel.Error = null;

				if (viewModel.RegionIndex == 0)
				{
					viewModel.Error = "A region must be seleted first.";
				}
				if (viewModel.PurchaseOrderIdList.Length > 2000)
				{
					viewModel.Error = "No more than 2000 PO can be processed";
				}
				else
				{
					// Process PO to update
					string region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);

					var maxRecords = int.Parse(ConfigurationManager.AppSettings["MaxEntriesForInStockReq"]) + 1;

					// Get the messages to update.
					var messagesToUpdate = this.getPurchaseOrdersMessagesToResetQuery.Search(new GetPurchaseOrdersMessagesToResetQueryParameters {
													Region = region,
													MaxRecords = maxRecords,
													PurchaseOrderIdList = viewModel.PurchaseOrderIdList
												});

					var messageArchiveIDs = messagesToUpdate.Select(m => m.ArchiveID).ToArray();

					var reQueueMessageArchiveCommandParameters = new ReQueueMessageArchiveCommandParameters
																		{
																			Region = region,
																			MessageArchiveIDs = messageArchiveIDs,
																			UserName = User.Identity.Name
																		};
					reQueueMessageArchiveCommandHandler.Execute(reQueueMessageArchiveCommandParameters);

					var rowsAffected = reQueueMessageArchiveCommandParameters.RowsAffected;

					// Prepare results
					var processed = messagesToUpdate.ToDictionary(m => m.PurchaseOrderId);
					viewModel.ResultTable = new DataTable();
					viewModel.ResultTable.Columns.Add("Purchase_Order_ID", typeof(int));
					viewModel.ResultTable.Columns.Add("Archive_ID", typeof(int));
					viewModel.ResultTable.Columns.Add("Status", typeof(string));

					// Add Processed messages
					foreach (var purchaseOrderId in viewModel.PurchaseOrderIdList)
					{
						if (processed.ContainsKey(purchaseOrderId) == true)
						{
							var message = processed[purchaseOrderId];
							var row = viewModel.ResultTable.NewRow();
							row["Purchase_Order_ID"] = purchaseOrderId;
							row["Archive_ID"] = message.ArchiveID;
							row["Status"] = "Processed";
							viewModel.ResultTable.Rows.Add(row);
						}
					}

					// Add not found Purchase Orders
					foreach (var purchaseOrderId in viewModel.PurchaseOrderIdList)
					{
						if (processed.ContainsKey(purchaseOrderId) == false)
						{
							var row = viewModel.ResultTable.NewRow();
							row["Purchase_Order_ID"] = purchaseOrderId;
							row["Archive_ID"] = DBNull.Value;
							row["Status"] = "Not Found";
							viewModel.ResultTable.Rows.Add(row);
						}
					}

					// true if all the messages got updated.
					viewModel.RequeueSuccess = (rowsAffected == messageArchiveIDs.Length);
					viewModel.RecordsProcessed = rowsAffected;
				}
			}
			catch (Exception ex)
			{
				viewModel.Error = ex.Message;
				logger.Error(JsonConvert.SerializeObject(new
				{
					Message = "Unexpected error occurred",
					Controller = nameof(ReQueueMessageController),
					ViewModel = viewModel,
					Exception = ex
				}));
			}

			return View(viewModel);
		}
	}
}
