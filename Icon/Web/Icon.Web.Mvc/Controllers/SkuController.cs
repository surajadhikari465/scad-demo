using Icon.Common.DataAccess;
using Icon.FeatureFlags;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    /// <summary>
    /// Sku Controller.
    /// </summary>
    public class SkuController : Controller
    {
        private IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>> getItemGroupPageQuery;
        private IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int> getFilteredResultsCountQuery;
        private IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int> getUnfilteredResultsCountQuery;
        private IFeatureFlagService featureFlagService;

        /// <summary>
        /// Initializes an instance of SkuController.
        /// </summary>
        /// <param name="getItemGroupPageQuery">Sku/Priceline Query.</param>
        /// <param name="getFilteredResultsCountQuery">Sku/Priceline filtered Count Query.</param>
        /// <param name="getUnfilteredResultsCountQuery">Sku/Priceline unfiltered Count Query.</param>
        /// <param name="featureFlagService">Feature Flag Service</param>
        public SkuController(
            IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>> getItemGroupPageQuery,
            IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int> getFilteredResultsCountQuery,
            IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int> getUnfilteredResultsCountQuery,
            IFeatureFlagService featureFlagService)
        {
            if (getItemGroupPageQuery == null)
            {
                throw new ArgumentNullException(nameof(getItemGroupPageQuery));
            }
            if (getFilteredResultsCountQuery == null)
            {
                throw new ArgumentNullException(nameof(getFilteredResultsCountQuery));
            }
            if (getUnfilteredResultsCountQuery == null)
            {
                throw new ArgumentNullException(nameof(getUnfilteredResultsCountQuery));
            }
            if (featureFlagService == null)
            {
                throw new ArgumentNullException(nameof(featureFlagService));
            }

            this.getItemGroupPageQuery = getItemGroupPageQuery;
            this.getFilteredResultsCountQuery = getFilteredResultsCountQuery;
            this.getUnfilteredResultsCountQuery = getUnfilteredResultsCountQuery;
            this.featureFlagService = featureFlagService;
        }

        /// <summary>
        /// GET: /Sku
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Get the feature flag.
            bool skuManagementFeatureFlag;
            try
            {
                skuManagementFeatureFlag = this.featureFlagService.IsEnabled("SkuManagement");
            }
            catch
            {
                skuManagementFeatureFlag = false;
            }

            // If the feature is not enable, redirect.
            if (skuManagementFeatureFlag == false)
            {
                return Redirect("/");
            }
            return View();
        }

        /// <summary>
        /// GET: /Sku/AllSku
        /// List of all Skus.
        /// </summary>
        /// <param name="draw"
        /// <returns>Json with a list of Sku.</returns>
        public JsonResult AllSku(DataTableAjaxPostModel dataTableAjaxPostModel)
        {
            if (dataTableAjaxPostModel == null)
            {
                throw new ArgumentNullException(nameof(dataTableAjaxPostModel));
            }

            // Validate argument
            EnsureArgumentCondition(dataTableAjaxPostModel.start >= 0, "start cannot be negative");
            EnsureArgumentCondition(dataTableAjaxPostModel.length >= 0, "length cannot be negative");
            EnsureArgumentCondition((dataTableAjaxPostModel.columns?.Count ?? -1) >= 0, "columns is invalid");
            EnsureArgumentCondition((dataTableAjaxPostModel.order?.Count ?? -1) == 1, "order is invalid");
            EnsureArgumentCondition((dataTableAjaxPostModel.order[0]?.column ?? -1) >= 0, "order column is invalid");
            EnsureArgumentCondition((dataTableAjaxPostModel.order[0]?.column ?? int.MaxValue) < dataTableAjaxPostModel.columns.Count, "order column is invalid");
            EnsureArgumentCondition(string.IsNullOrWhiteSpace(dataTableAjaxPostModel.order[0]?.dir) == false, "order direction is invalid");

            var sortOrderColumnIndex = dataTableAjaxPostModel.order[0].column;
            string sortOrderColumnName = dataTableAjaxPostModel.columns[sortOrderColumnIndex].data;
            EnsureArgumentCondition(string.IsNullOrWhiteSpace(sortOrderColumnName) == false, "order column is invalid");

            // Convert Data from input to internal values

            // Escape search field;
            string search = dataTableAjaxPostModel?.search?.value;

            // Translate Datatable columns to Query Column
            ItemGroupColumns sortColumn = ItemGroupColumns.ItemGroupId;
            if (sortOrderColumnName == "PrimaryItemUpc")
            {
                sortColumn = ItemGroupColumns.ScanCode;
            }
            else if (sortOrderColumnName == "SkuDescription")
            {
                sortColumn = ItemGroupColumns.SKUDescription;
            }
            else if (sortOrderColumnName == "CountOfItems")
            {
                sortColumn = ItemGroupColumns.ItemCount;
            }

            // Translate Sort order from input to internal enum.
            SortOrder sortOrder = (dataTableAjaxPostModel.order[0].dir == "asc") ? SortOrder.Ascending : SortOrder.Descending;
            
            // Result variables
            List<ItemGroupModel> itemGroups = null;
            int unfilteredCount = 0;
            int filteredCount = 0;

            // Run queries in paralled depending on if they contain search or not
            if (string.IsNullOrWhiteSpace(search))
            {
                Parallel.Invoke(
                    () =>
                    {
                        itemGroups = getItemGroupPageQuery.Search(
                            new GetItemGroupParameters
                            {
                                ItemGroupTypeId = ItemGroupTypeId.Sku,
                                PageSize = dataTableAjaxPostModel.length,
                                RowsOffset = dataTableAjaxPostModel.start,
                                SearchTerm = null,
                                SortColumn = sortColumn,
                                SortOrder = sortOrder
                            });
                    },
                    () =>
                    {
                        filteredCount = unfilteredCount = getUnfilteredResultsCountQuery.Search(
                            new GetItemGroupUnfilteredResultsCountQueryParameters
                            {
                                ItemGroupTypeId = ItemGroupTypeId.Sku
                            });
                    });
            }
            else
            {
                Parallel.Invoke(
                    () =>
                    {
                        itemGroups = getItemGroupPageQuery.Search(
                            new GetItemGroupParameters
                            {
                                ItemGroupTypeId = ItemGroupTypeId.Sku,
                                PageSize = dataTableAjaxPostModel.length,
                                RowsOffset = dataTableAjaxPostModel.start,
                                SearchTerm = search,
                                SortColumn = sortColumn,
                                SortOrder = sortOrder
                            });
                    },
                    () =>
                    {
                        unfilteredCount = getUnfilteredResultsCountQuery.Search(
                            new GetItemGroupUnfilteredResultsCountQueryParameters
                            {
                                ItemGroupTypeId = ItemGroupTypeId.Sku
                            });
                    },
                    () =>
                    {
                        filteredCount =  getFilteredResultsCountQuery.Search(
                            new GetItemGroupFilteredResultsCountQueryParameters
                            {
                                ItemGroupTypeId = ItemGroupTypeId.Sku,
                                SearchTerm = search
                            });
                    });
            }

            // return datatable page data
            return Json(new DataTableResponse<SkuViewModel>() {
                draw = dataTableAjaxPostModel.draw,
                data = itemGroups.Select(ig => new SkuViewModel {
                    CountOfItems = ig.ItemCount,
                    PrimaryItemUpc = ig.ScanCode,
                    SkuDescription = ig.SKUDescription,
                    SkuId = ig.ItemGroupId
                }).ToList(),
                recordsFiltered = filteredCount,
                recordsTotal = unfilteredCount
            },
            behavior: JsonRequestBehavior.AllowGet);
        }

        private void EnsureArgumentCondition(bool condition, string message)
        {
            if (condition== false)
            {
                throw new ArgumentException(message);
            }
        }

    }
}