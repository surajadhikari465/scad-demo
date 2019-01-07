using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Queries;
using System.Collections.Generic;
using System.Web.Http;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Models.DataMonster;

namespace MammothWebApi.Controllers
{
    public class ItemController : ApiController
    {

        private IQueryHandler<GetItemsQuery, ItemComposite> getItemsQueryHandler;
        private IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>> getItemsBySearchCriteriaQueryHandler;

        public ItemController(IQueryHandler<GetItemsQuery, ItemComposite> itemsQueryHandler, IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>> itemsBySearchCriteriaQueryHandler)
        {
            getItemsQueryHandler = itemsQueryHandler;
            getItemsBySearchCriteriaQueryHandler = itemsBySearchCriteriaQueryHandler;
        }

        [HttpPost]
        [Route("api/item/getItems")]
        public IHttpActionResult GetItemsByScanCodes([FromBody] List<string> scanCodes)
        {
            if (scanCodes == null) return BadRequest("ScanCodes are requried");
            var results = getItemsQueryHandler.Search(new GetItemsQuery{ ScanCodes = scanCodes });
            return Ok(results);
        }

        [HttpPost]
        [Route("api/item/GetItemsBySearchCriteria")]
        public IHttpActionResult GetItemsBySearchCriteria([FromBody]SearchItemModel searchItemModel)
        {
            if (string.IsNullOrEmpty(searchItemModel.BrandName) && string.IsNullOrEmpty(searchItemModel.Supplier) && string.IsNullOrEmpty(searchItemModel.ItemDescription))
                return BadRequest("Search Criteria is required.");

            if (string.IsNullOrEmpty(searchItemModel.Region))
                return BadRequest("Region must be passed.");

            var results = getItemsBySearchCriteriaQueryHandler.Search(new GetItemsBySearchCriteriaQuery { BrandName = searchItemModel.BrandName,
                                                                                                          Subteam = searchItemModel.Subteam,
                                                                                                          Supplier = searchItemModel.Supplier,
                                                                                                          ItemDescription = searchItemModel.ItemDescription,
                                                                                                          Region = searchItemModel.Region,
                                                                                                          IncludeLocales = searchItemModel.IncludeLocales,                                                                            
                                                                                                          IncludedStores = searchItemModel.IncludedStores
            });
            return Ok(results);
        }
    }
}
