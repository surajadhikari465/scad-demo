using System;
using System.Linq;
using System.Collections.Generic;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KitBuilderWebApi.QueryParameters;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;

namespace KitBuilderWebApi.Controllers
{
  public class VenueInfo
  {
    public int? VenueID               { get; set; }
    public int? StoreBU               { get; set; }
    public string VenueDisplayName    { get; set; }
    public int MainItemID             { get; set; }
    public string KitStatus           { get; set; }
    public string MainItemScanCode    { get; set; }
    public string MainItemDescription { get; set; }
  }

  [Produces("application/json")]
  [Route("api/Venues")]
  public class VenueController : Controller
  {
    readonly IRepository<Kit> Items;
    readonly ILogger<VenueController> logger;
    readonly IHelper<VenueInfo, VenueParameters> Helper;
    enum LocaleType { Uknown = 0, Chain = 1, Region = 2, Metro = 3, Store = 4, Venue = 5 }
  
    public VenueController(IRepository<Kit> itemsRepository, ILogger<VenueController> log, IHelper<VenueInfo, VenueParameters> itemHelper)
    {
      Items = itemsRepository;
      logger = log;
      Helper = itemHelper;
    }

    [HttpGet(Name = "GetVenue")]
    public IActionResult GetDMKits(VenueParameters Parameters)
    {
      try
      {
        HashSet<int> hsExcluded = null;
        var hsID = new HashSet<int>(Parameters.ItemIDs ?? new int[0]);

        foreach(var names in new string[]{ Parameters.Fields, Parameters.OrderBy }.Where(x => !String.IsNullOrEmpty(x)))
        {
          KitItemHelper.GetFieldList<VenueInfo>(names); //Exception will be thrown if there's invalid fields in Parameter.Fields
        }
       
        var itemList = new List<VenueInfo>();
        var dbContext = Items.UnitOfWork.Context;
        var validTypeIDs = Enum.GetValues(typeof(LocaleType)).Cast<int>().Where(x => x > 0).ToArray();
        
        var kits = (from A in dbContext.Kit
                    join B in dbContext.Items on A.ItemId equals B.ItemId
                    join C in dbContext.KitLocale on A.KitId equals C.KitId
                    join D in dbContext.Status on C.StatusId equals D.StatusId
                    join E in dbContext.Locale on C.LocaleId equals E.LocaleId
                    where validTypeIDs.Contains(E.LocaleTypeId) && (hsID.Count() == 0 || hsID.Contains(A.ItemId))
                    select new { A.KitId,
                                 B.ItemId,
                                 B.ScanCode,
                                 B.ProductDesc,
                                 C.LocaleId,
                                 E.LocaleTypeId,
                                 C.Exclude,
                                 Status = D.StatusDescription,
                                 C.KitLocaleId }
                   ).GroupBy(x => x.LocaleTypeId).ToDictionary(x => (LocaleType)x.Key, x => x.ToArray());

        if(kits.Count() == 0) return Ok(); //Nothing to process

        var locales = dbContext.Locale.Select(x => new { x.LocaleId, x.LocaleName, x.LocaleTypeId, x.StoreId, x.MetroId, x.RegionId, x.ChainId, x.BusinessUnitId })
                                      .GroupBy(x => x.LocaleTypeId).ToDictionary(x => (LocaleType)x.Key, x => x.ToList());

        if(!locales.ContainsKey(LocaleType.Venue) || !locales.ContainsKey(LocaleType.Store)) return Ok(); //Nothing to process

        var exclKitStore = new Dictionary<int, HashSet<int>>(); 
        foreach(var kit in kits.SelectMany(x => x.Value.Where(y => (y.Exclude ?? false))).OrderBy(x => x.KitId))
        {
          var lType = (LocaleType)kit.LocaleTypeId;
          if(!exclKitStore.ContainsKey(kit.KitId)) exclKitStore.Add(kit.KitId, new HashSet<int>());
          
          switch(lType)
          {
            case LocaleType.Venue:
              exclKitStore[kit.KitId].UnionWith(new HashSet<int>(locales[LocaleType.Venue].Where(x => x.LocaleId == kit.LocaleId).Select(x => x.StoreId ?? 0)));
              break;
            case LocaleType.Store:
            case LocaleType.Metro:
            case LocaleType.Region:
            case LocaleType.Chain:
              exclKitStore[kit.KitId].UnionWith(new HashSet<int>(locales[LocaleType.Store]
                                     .Where(x => kit.LocaleId  == (lType == LocaleType.Metro  ? (x.MetroId ?? 0) :
                                                                   lType == LocaleType.Region ? (x.RegionId ?? 0) :
                                                                   lType == LocaleType.Chain  ? (x.ChainId ?? 0) : x.LocaleId))
                                     .Select(x => x.LocaleId)));
              break;
            default: break;
          }
        }

        foreach(var lType in Enum.GetValues(typeof(LocaleType)).Cast<LocaleType>().Where(x => kits.ContainsKey(x)).OrderByDescending(x => x))
        {
          foreach(var venue in locales[LocaleType.Venue].ToArray())
          {
            foreach(var kit in kits[lType].Where(x => !(x.Exclude ?? false) &&
                                                      x.LocaleId == (lType == LocaleType.Store  ? venue.StoreId  :
                                                                     lType == LocaleType.Metro  ? venue.MetroId  :
                                                                     lType == LocaleType.Region ? venue.RegionId :
                                                                     lType == LocaleType.Chain  ? venue.ChainId  : venue.LocaleId)))
            {
              hsExcluded = !exclKitStore.ContainsKey(kit.KitId) ? new HashSet<int>() : exclKitStore[kit.KitId];

              switch(lType)
              {
                case LocaleType.Venue:    //Direct venue assignment
                  itemList.AddRange(locales[LocaleType.Venue].Where(x => x.LocaleId == kit.LocaleId && !hsExcluded.Contains(x.LocaleId))
                                                             .Select(x => new VenueInfo { VenueID = venue.LocaleId,
                                                                                          StoreBU = x.BusinessUnitId,
                                                                                          VenueDisplayName = venue.LocaleName,
                                                                                          MainItemID = kit.ItemId,
                                                                                          MainItemScanCode = kit.ScanCode,
                                                                                          MainItemDescription = kit.ProductDesc,
                                                                                          KitStatus = kit.Status }));
                  break;
                case LocaleType.Store:    //Direct store assignment
                  itemList.AddRange(locales[LocaleType.Store].Where(x => x.LocaleId == kit.LocaleId && !hsExcluded.Contains(x.LocaleId))
                                                               .Select(x => new VenueInfo { VenueID = venue.LocaleId,
                                                                                            StoreBU = x.BusinessUnitId,
                                                                                            VenueDisplayName = venue.LocaleName,
                                                                                            MainItemID = kit.ItemId,
                                                                                            MainItemScanCode = kit.ScanCode,
                                                                                            MainItemDescription = kit.ProductDesc,
                                                                                            KitStatus = kit.Status }));
                  break;
                case LocaleType.Metro:
                case LocaleType.Region:
                case LocaleType.Chain:
                  itemList.AddRange(locales[LocaleType.Store].Where(x => !hsExcluded.Contains(x.StoreId ?? 0) &&
                                                                         kit.LocaleId == (lType == LocaleType.Metro ? (x.MetroId ?? 0) :
                                                                                          lType == LocaleType.Region ? (x.RegionId ?? 0) : (x.ChainId ?? 0)))
                                                             .Select(x => new VenueInfo { VenueID = venue.LocaleId,
                                                                                          StoreBU = x.BusinessUnitId,
                                                                                          VenueDisplayName = venue.LocaleName,
                                                                                          MainItemID = kit.ItemId,
                                                                                          MainItemScanCode = kit.ScanCode,
                                                                                          MainItemDescription = kit.ProductDesc,
                                                                                          KitStatus = kit.Status }));
                  break;
                default: break;
              }
            }
          }
        }

        itemList = itemList.GroupBy(x => new { x.MainItemID, x.VenueID, BU = (x.StoreBU ?? 0) }).Select(x => x.First()).ToList();

        if(!(string.IsNullOrEmpty(Parameters.OrderBy)))
        {
          itemList = KitItemHelper.SetOrderBy<VenueInfo>(itemList.AsQueryable(), Parameters.OrderBy).ToList();
        }

        if(String.IsNullOrEmpty(Parameters.Fields)) //All fields
        {
          return Ok(Parameters.PageNumber > 0 ? PagedList<VenueInfo>.Create(itemList.AsQueryable(), Parameters.PageNumber, Parameters.PageSize).ToArray()
                                            : itemList.ToArray());
        }
        else
        {
          return Ok(Parameters.PageNumber > 0 ? PagedList<VenueInfo>.Create(itemList.AsQueryable(), Parameters.PageNumber, Parameters.PageSize).ShapeData(Parameters.Fields).ToArray()
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