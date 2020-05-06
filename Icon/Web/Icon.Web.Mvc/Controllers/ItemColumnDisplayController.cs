using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Controllers
{
    public class ItemColumnDisplayController : Controller
    {

        private readonly IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQuery;
        private readonly ICommandHandler<UpdateItemColumnOrderCommand> updateItemColumnOrderCommandHandler;
        private readonly ICommandHandler<SetDefaultItemColumnOrderCommand> setDefaultItemColumnOrderCommandHandler;

        private readonly ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>
            addMissingColumnsToItemColumnOrderTableCommandHandler;

        public ItemColumnDisplayController(IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQuery,
            ICommandHandler<UpdateItemColumnOrderCommand> updateItemColumnOrderCommandHandler, 
            ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand> addMissingColumnsToItemColumnOrderTableCommandHandler, 
            ICommandHandler<SetDefaultItemColumnOrderCommand> setDefaultItemColumnOrderCommandHandler)
        {
            this.getItemColumnOrderQuery = getItemColumnOrderQuery;
            this.updateItemColumnOrderCommandHandler = updateItemColumnOrderCommandHandler;
            this.addMissingColumnsToItemColumnOrderTableCommandHandler = addMissingColumnsToItemColumnOrderTableCommandHandler;
            this.setDefaultItemColumnOrderCommandHandler = setDefaultItemColumnOrderCommandHandler;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetDisplayOrder( List<ItemColumnOrderModel> items)
        {
            var paramters = new UpdateItemColumnOrderCommand { DisplayData = items};
            try
            {
                updateItemColumnOrderCommandHandler.Execute(paramters);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetDisplayOrder()
        {
            var hierarchies = getItemColumnOrderQuery.Search(new EmptyQueryParameters<List<ItemColumnOrderModel>>());
            return Json(hierarchies, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDefaultDisplayOrder()
        {
            try
            {
                setDefaultItemColumnOrderCommandHandler.Execute(new SetDefaultItemColumnOrderCommand());
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddMissingColumnsToOrderTable()
        {
            try
            {
                addMissingColumnsToItemColumnOrderTableCommandHandler.Execute(new AddMissingColumnsToItemColumnDisplayTableCommand());
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }

    
}