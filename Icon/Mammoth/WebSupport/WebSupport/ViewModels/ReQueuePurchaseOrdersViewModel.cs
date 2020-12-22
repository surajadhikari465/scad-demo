using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebSupport.Helpers;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
    /// <summary>
    /// Re-Queue Purchase Orders View-Model
    /// </summary>
    public class ReQueuePurchaseOrdersViewModel
    {
        /// <summary>
        /// Initializes an instance of ReQueuePurchaseOrdersViewModel.
        /// </summary>
        public ReQueuePurchaseOrdersViewModel()
        {
            OptionsForRegion = SelectListHelper.ArrayToSelectList(StaticData.WholeFoodsRegions.ToArray(), 0);
        }

        /// <summary>
        /// PO number separators.
        /// </summary>
        private static readonly char[] separators = { ' ', '\t', '\n', '\r' };

        /// <summary>
        /// Gets or sets the selected region.
        /// </summary>
        [Required]
        [Display(Name = "Region")]
        public int RegionIndex { get; set; }

        /// <summary>
        /// Gets or sets the available region options.
        /// </summary>
        public IEnumerable<SelectListItem> OptionsForRegion { get; set; }

        /// <summary>
        /// Gets or sets the PO ids to requue
        /// </summary>
        [Required]
        [Display(Name = "Purchase Order ID(s) (one per line)")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[1-9][0-9]{0,9}(\r?\n[1-9][0-9]{0,9})*(\r?\n)*$", ErrorMessage = "Only numbers allowed")]
        public string PurchaseOrderRequeueText { get; set; }
                
        /// <summary>
        /// Gets  the list of Ids to re-queue.
        /// </summary>
        public int[] PurchaseOrderIdList
        { 
            get 
            { 
                if (string.IsNullOrWhiteSpace(this.PurchaseOrderRequeueText)) 
                { 
                    return Array.Empty<int>(); 
                } 
                else 
                {
                    return this.PurchaseOrderRequeueText
                        .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => { 
                            int id;
                            int.TryParse(s, out id);
                            return id;
                        })
                        .ToArray();
                }
            } 
        }

        /// <summary>
        /// Gets or sets the global request error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets if the request was a success.
        /// </summary>
        public bool IsSuccess { get { return String.IsNullOrEmpty(Error); } }

        /// <summary>
        /// Gets the results data table.
        /// </summary>
        public DataTable ResultTable { get; set; }

        /// <summary>
        /// Gets or sets if the re-queue was sucessfull.
        /// </summary>
        public bool? RequeueSuccess { get; set; }

        /// <summary>
        /// Gets or sets the number of records processed.
        /// </summary>
        public int RecordsProcessed { get; set; }
    }
}