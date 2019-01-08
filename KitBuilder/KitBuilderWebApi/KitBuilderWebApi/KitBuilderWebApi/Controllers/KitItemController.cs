using System;
using System.Linq;
using System.Collections.Generic;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KitBuilderWebApi.QueryParameters;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;

namespace KitBuilderWebApi.Controllers
{
  [Route("api/KitItems")]
  public class KitItemController : Controller
  {
    IRepository<Items> Items;
    ILogger<KitItemController> logger;
    IHelper<ItemsDto, KitItemParameters> Helper;
  
    public KitItemController(IRepository<Items> itemsRepository, ILogger<KitItemController> log, IHelper<ItemsDto, KitItemParameters> itemHelper)
    {
      Items = itemsRepository;
      logger = log;
      Helper = itemHelper;
    }

    [HttpGet(Name = "GetKitItems")]
    public IActionResult GetKitItems(KitItemParameters Parameters)
    {
      try
      {
        var hsID = new HashSet<int>(Parameters.ItemIDs ?? new int[0]);
        var hsScan = new HashSet<string>(Parameters.ScanCodes == null ? new string[0] : Parameters.ScanCodes.Where(x => !String.IsNullOrWhiteSpace(x)).Select(x => x.Trim()), StringComparer.InvariantCultureIgnoreCase);

        foreach(var names in new string[]{ Parameters.Fields, Parameters.OrderBy }.Where(x => !String.IsNullOrEmpty(x)))
        {
          KitItemHelper.GetFieldList<ItemsDto>(names); //Exception will be thrown if there's invalid fields in Parameter.Fields
        }

        var itemList = (hsID.Count == 0 && hsScan.Count == 0 ? Items.GetAll()
                                                             : Items.GetAll().Where(x => hsID.Contains(x.ItemId) || hsScan.Contains(x.ScanCode)))
                       .Select(x => new ItemsDto(){ ItemId = x.ItemId,
                                                    ScanCode = x.ScanCode,
                                                    BrandName = x.BrandName,
                                                    CustomerFriendlyDesc = x.CustomerFriendlyDesc,
                                                    ProductDesc = x.ProductDesc,
                                                    KitchenDesc = x.KitchenDesc });
        
        itemList = Helper.SetOrderBy(itemList, Parameters);
        

        if(String.IsNullOrEmpty(Parameters.Fields)) //All fields
        {
          return Ok(Parameters.PageNumber > 0 ? PagedList<ItemsDto>.Create(itemList, Parameters.PageNumber, Parameters.PageSize).ToArray()
                                            : itemList.ToArray());
        }
        else  //Fields specified by Parameters.Fields
        {
          return Ok(Parameters.PageNumber > 0 ? PagedList<ItemsDto>.Create(itemList, Parameters.PageNumber, Parameters.PageSize).ShapeData(Parameters.Fields).ToArray()
                                            : itemList.ShapeData(Parameters.Fields).ToArray());
        }
      }
      catch(Exception ex)
      {
        logger.LogError(ex.Message);
        return BadRequest();
      }
    }
  }
}