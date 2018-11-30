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

        public ItemController(IQueryHandler<GetItemsQuery, ItemComposite> itemsQueryHandler)
        {
            getItemsQueryHandler = itemsQueryHandler;
        }

        [HttpPost]
        [Route("api/item/getItems")]
        public IHttpActionResult GetItemsByScanCodes([FromBody] List<string> scanCodes)
        {
            if (scanCodes == null) return BadRequest("ScanCodes are requried");
            var results = getItemsQueryHandler.Search(new GetItemsQuery{ ScanCodes = scanCodes });
            return Ok(results);
        }

    }
}
