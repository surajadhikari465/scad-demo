using AutoMapper;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Enums;
using KitBuilder.DataAccess.Repository;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.Services;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using KitInstructionList = KitBuilder.DataAccess.DatabaseModels.KitInstructionList;
using LocaltypeModel = KitBuilder.DataAccess.DatabaseModels.LocaleType;
using Newtonsoft.Json.Linq;
using System.Text;
using KitBuilderWebApi.Models;
using LocaleStatus = KitBuilder.DataAccess.Enums.Status;
using Status = KitBuilder.DataAccess.DatabaseModels.Status;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Kits")]
    public class KitController : Controller
    {
        private IRepository<KitInstructionList> kitInstructionListRepository;
        private IRepository<InstructionList> instructionListRepository;
        private IRepository<KitQueue> kitQueueRepository;
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<Kit> kitRepository;
        private IRepository<Locale> localeRepository;
        private IRepository<KitLocale> kitLocaleRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<KitLinkGroup> kitLinkGroupRepository;
        private IRepository<KitBuilder.DataAccess.DatabaseModels.Status> statusRepository;
        private IRepository<KitLinkGroupLocale> kitLinkGroupLocaleRepository;
        private ILogger<KitController> logger;
        private IRepository<KitLinkGroupItem> kitLinkGroupItemRepository;
        private IRepository<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRepository;
        private IRepository<LocaltypeModel> localeTypeRepository;
        private IHelper<KitDtoWithStatus, KitSearchParameters> kitHelper;
        private IServiceProvider services;
        private const string DELETE_KIT_SP_NAME = "DeleteKitByKitId";
        private const string PUBLISH_KIT_EVENTS = "PublishKitEvents";
        private const string ADD_OR_UPDATE_ACTION = "AddOrUpdate";
        private const string DELETE_ACTION = "Delete";
        private const string MAIN_ITEM_NO_CALORIE = "Main item has no Nutrition record in Mammoth.";
        private const string MAIN_ITEM_NOT_AUTHORIZED = "Main item either is not authorized or has no price record for the selected store in Mammoth.";
        private const string MODIFIER_NO_CALORIE = "One or more modifiers have no Nutrition record in Mammoth.";
        private const string MODIFIER_NOT_AUTHORIZED = "One or more modifiers either are not authorized or have no price records for the selected store in Mammoth.";


        private IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>> calorieCalculator;

        public KitController(IRepository<LinkGroup> linkGroupRepository,
                             IRepository<InstructionList> instructionListRepository,
                             IRepository<Kit> kitRepository,
                             IRepository<Locale> localeRepository,
                             IRepository<KitLocale> kitLocaleRepository,
                             IRepository<LinkGroupItem> linkGroupItemRepository,
                             IRepository<Items> itemsRepository,
                             IRepository<KitLinkGroup> kitLinkGroupRepository,
                             IRepository<KitLinkGroupLocale> kitLinkGroupLocaleRepository,
                             IRepository<KitLinkGroupItem> kitLinkGroupItemRepository,
                             IRepository<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRepository,
                             IRepository<LocaltypeModel> localeTypeRepository,
                             IRepository<KitInstructionList> kitInstructionListRepository,
                             ILogger<KitController> logger,
                            IHelper<KitDtoWithStatus, KitSearchParameters> kitHelper,
                             IServiceProvider services,
                             IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>> calorieCalculator,
                             IRepository<KitQueue> kitQueueRepository,
                             IRepository<Status> statusRepository

                            )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.instructionListRepository = instructionListRepository;
            this.kitRepository = kitRepository;
            this.localeRepository = localeRepository;
            this.kitLocaleRepository = kitLocaleRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.kitLinkGroupRepository = kitLinkGroupRepository;
            this.kitLinkGroupLocaleRepository = kitLinkGroupLocaleRepository;
            this.kitLinkGroupItemRepository = kitLinkGroupItemRepository;
            this.kitLinkGroupItemLocaleRepository = kitLinkGroupItemLocaleRepository;
            this.localeTypeRepository = localeTypeRepository;
            this.kitInstructionListRepository = kitInstructionListRepository;
            this.logger = logger;
            this.kitHelper = kitHelper;
            this.services = services;
            this.calorieCalculator = calorieCalculator;
            this.kitQueueRepository = kitQueueRepository;
            this.statusRepository = statusRepository;
        }

        [HttpGet("{kitLocaleId}/GetKitCalories/{storeLocaleId}", Name = "ViewKitByStore")]
        public IActionResult GetKitCalories(int kitLocaleId, int storeLocaleId)
        {
            GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters
            {
                KitLocaleId = kitLocaleId,
                StoreLocaleId = storeLocaleId
            };

            CaloricCalculator calculator = (CaloricCalculator)services.GetService(typeof(IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>>));
            Task<KitLocaleDto> kitLocaleDto = calculator.Run(parameters);

            if (kitLocaleDto.Result.KitLocaleId == 0)
            {
                logger.LogWarning("No KitLocale can be found by KitLocaleId: " + kitLocaleId.ToString());
                return NotFound();
            }

            return Ok(kitLocaleDto.Result);
        }

        // GET api/kits/1/ViewKit/1 GetKitByLocaleId
        [HttpGet("{kitId}/ViewKit/{localeId}", Name = "ViewKit")]
        public IActionResult GetKitByLocaleId(int kitId, int localeId, bool loadChildObjects)
        {
            int? localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, localeId);

            if (localeIdWithKitLocaleRecord == null)
            {
                logger.LogWarning("Kit does not exist for selected store.");
                return NotFound();
            }

            var kitQuery = BuildKitByLocaleIdQuery(kitId, (int)localeIdWithKitLocaleRecord, loadChildObjects);
            var kit = kitQuery.FirstOrDefault();

            if (kit == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            KitDto kitDtoList = Mapper.Map<KitDto>(kit);

            return Ok(kitDtoList);
        }

        [HttpGet("{kitId}/{storeId}/GetKitView", Name = "GetKitView")]
        public IActionResult GetKitView(int kitId, int storeId)
        {
            int? localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, storeId);
            var kit = kitRepository.GetAll().Where(k => k.KitId == kitId).FirstOrDefault();
            KitView kitView = new KitView();

            if (localeIdWithKitLocaleRecord == null || kit == null)
            {
                kitView.ErrorMessage = "Error: Kit does not exist for selected store.";
                return Ok(kitView);
            }

            var kitProperties = BuildKitPropertiesByLocaleId(kitId, (int)localeIdWithKitLocaleRecord, storeId);

            //A kit has to be assigned to a locale, and for a customizable kit, a locale property has to be set before it can be viewed by a store
            if (kitProperties == null || kit.KitType == KitType.Customizable && kitProperties.KitLinkGroupLocaleList.Where(klg => klg.KitLinkGroupLocaleId > 0).Count() == 0)
            {
                kitView.ErrorMessage = "Error: Please make sure Kit is set up correctly for this store.";
                return Ok(kitView);
            }

            kitView = BuildKitView(kitProperties, storeId, kit);

            if (kitView == null)
            {
                kitView.ErrorMessage = "Error: Please make sure Kit is set up correctly for this store.";
                return Ok(kitView);
            }
            else
            {
                return Ok(kitView);

            }

        }

        internal KitView BuildKitView(KitPropertiesDto kitProperties, int storeId, Kit kit)
        {
            KitView kitView = new KitView();
            Task<KitLocaleDto> kitLocaleDtoTask;

            try
            {
                kitLocaleDtoTask = calorieCalculator.Run(new GetKitLocaleByStoreParameters { KitLocaleId = kitProperties.KitLocaleId, StoreLocaleId = storeId });
            }
            catch (Exception ex)
            {
                logger.LogError(message: "BuildKitView Error");
                logger.LogError(message: $"{ex.Message}");
                if (ex.InnerException != null) logger.LogError(message: $"{ex.InnerException.Message}");
                kitView.ErrorMessage = "Error in getting data from mammoth.";
                return kitView;
            }

            var maximumCalories = kitLocaleRepository.GetAll().Where(k => k.KitLocaleId == kitProperties.KitLocaleId).Select(s => s.MaximumCalories).FirstOrDefault();
            kitView.LinkGroups = new List<LinkGroupView>();
            kitView.KitId = kitProperties.KitId;
            kitView.Description = kitProperties.Description;
            kitView.StoreId = storeId;
            kitView.KitLocaleId = kitProperties.KitLocaleId;
            kitView.ErrorMessage = "";
            kitLocaleDtoTask.Wait();

            var kitLocaleDto = kitLocaleDtoTask.Result;


            if ((kitLocaleDto.MaximumCalories != null || kit.KitType == KitType.Simple) && kitLocaleDto.MinimumCalories != null && kitLocaleDto.RegularPrice != null
                && kitLocaleDto.AuthorizedByStore != null && kitLocaleDto.AuthorizedByStore != false
                && kitLocaleDto.Exclude != null)
            {
                if (kitLocaleDto.Exclude == true)
                {
                    kitView.ErrorMessage = "Error: Kit is excluded for selected store.";
                    return kitView;
                }

                if (kit.KitType == KitType.Simple && kitLocaleDto.MaximumCalories == null)
                {
                    kitView.MaximumCalories = (int)kitLocaleDto.MinimumCalories;
                }
                else if (maximumCalories == null || maximumCalories == 0)
                {
                    kitView.MaximumCalories = (int)kitLocaleDto.MaximumCalories;
                }
                else
                {
                    kitView.MaximumCalories = (int)maximumCalories;
                }

                kitView.MinimumCalories = (int)kitLocaleDto.MinimumCalories;
                kitView.KitPrice = (decimal)kitLocaleDto.RegularPrice;
                kitView.AuthorizedByStore = (bool)kitLocaleDto.AuthorizedByStore;
                kitView.Excluded = (bool)kitLocaleDto.Exclude;

                foreach (KitLinkGroupPropertiesDto KitLinkGroupPropertiesDto in kitProperties.KitLinkGroupLocaleList.Where(l => l.Excluded == false))
                {
                    LinkGroupView linkGroupView = new LinkGroupView();
                    linkGroupView.Modifiers = new List<ModifierView>();
                    linkGroupView.LinkGroupId = KitLinkGroupPropertiesDto.KitLinkGroupId;
                    linkGroupView.GroupName = KitLinkGroupPropertiesDto.Name;
                    linkGroupView.GroupDescription = KitLinkGroupPropertiesDto.Name;
                    linkGroupView.LinkGroupProperties = KitLinkGroupPropertiesDto.Properties;
                    linkGroupView.FormattedLinkGroupProperties = formatLinkGroupProperties(KitLinkGroupPropertiesDto.Properties).ToString();

                    var propertiesDtoList = KitLinkGroupPropertiesDto.KitLinkGroupItemLocaleList.OrderBy(s => s.Excluded).ToList();
                    var listCount = propertiesDtoList.Count;
                    int currentCount = 0;

                    foreach (PropertiesDto propertiesDto in propertiesDtoList)//.Where(l => l.Excluded == false))
                    {
                        currentCount = currentCount + 1;
                        KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto = (kitLocaleDto.KitLinkGroupLocale.Where(kl => kl.KitLinkGroupId == propertiesDto.KitLinkGroupId).FirstOrDefault()).KitLinkGroupItemLocales
                                                                              .Where(k => k.KitLinkGroupItemLocaleId == propertiesDto.KitLinkGroupItemLocaleId).FirstOrDefault();

                        if (kitLinkGroupItemLocaleDto != null)
                        {
                            ModifierView modifierView = new ModifierView();
                            modifierView.AuthorizedByStore = (bool)kitLinkGroupItemLocaleDto.AuthorizedByStore;
                            modifierView.Excluded = (bool)kitLinkGroupItemLocaleDto.Exclude;
                            modifierView.ItemID = (int)kitLinkGroupItemLocaleDto.ItemId;
                            modifierView.ModifierName = propertiesDto.Name;
                            modifierView.ModifierProperties = kitLinkGroupItemLocaleDto.Properties;
                            modifierView.Price = kitLinkGroupItemLocaleDto.RegularPrice.ToString();
                            modifierView.Calories = kitLinkGroupItemLocaleDto.Calories.ToString(); ;
                            modifierView.FormattedModifierProperties = formatModifierProperties(kitLinkGroupItemLocaleDto.Properties, kitLinkGroupItemLocaleDto).ToString();
                            if (currentCount == listCount)
                            {
                                linkGroupView.FormattedAllModifiersProperties = AppendToAllModifiersProperties(modifierView, linkGroupView.FormattedAllModifiersProperties, true);
                            }
                            else
                            {
                                linkGroupView.FormattedAllModifiersProperties = AppendToAllModifiersProperties(modifierView, linkGroupView.FormattedAllModifiersProperties, false);
                            }

                            linkGroupView.Modifiers.Add(modifierView);
                        }
                        else
                        {
                            kitView.ErrorMessage = "Error in fetching info from mammoth. kitLinkGroupItemLocaleDto is null.";
                        }
                    }
                    linkGroupView.Modifiers = linkGroupView.Modifiers.OrderBy(s => s.AuthorizedByStore).ThenBy(a => a.Excluded).ToList();
                    kitView.LinkGroups.Add(linkGroupView);
                }

                kitView.ErrorMessage = BuildErrorMessage(kitLocaleDto, kit.KitType);

                return kitView;
            }
            else
            {

                kitView.ErrorMessage = BuildErrorMessage(kitLocaleDto, kit.KitType);
                return kitView;
            }
        }

        // GET api/kits/1/ViewKit/1 GetKitByLocationID
        [HttpGet("{kitId}/GetKitProperties/{localeId}", Name = "GetKitProperties")]
        public IActionResult GetKitPropertiesByLocaleId(int kitId, int localeId)
        {
            int? localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, localeId);

            if (localeIdWithKitLocaleRecord == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            var kitProperties = BuildKitPropertiesByLocaleId(kitId, (int)localeIdWithKitLocaleRecord, localeId);

            if (kitProperties == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            return Ok(kitProperties);
        }

		//// GET api/kits/1/ViewKit/1 GetKitByLocationID
		[HttpPost("{kitId}/CopyKitProperties/{fromLocaleId}", Name = "CopyKitProperties")]
		public IActionResult CopyKitProperties(int kitId, int fromLocaleId, [FromBody] List<int> toLocaleIds)
		{
			try
			{
				var paramKitId = new SqlParameter("kitId", SqlDbType.BigInt) { Value = kitId };
				var paramFromLocaleId = new SqlParameter("fromLocaleId", SqlDbType.Int) { Value = fromLocaleId };

				var table = new DataTable();
				table.Columns.Add("LocaleId");
				toLocaleIds.ForEach(i => table.Rows.Add(i));

				var paramToLocaleIds = new SqlParameter
				{
					ParameterName = "toLocaleIds",
					Value = table,
					TypeName = "dbo.CopyToLocalesType"
				};

				var sql = "exec CopyKitLocaleProperties @kitId, @fromLocaleId, @toLocaleIds";

				kitLocaleRepository.UnitOfWork.Context.Database.ExecuteSqlCommand(sql, paramKitId, paramFromLocaleId, paramToLocaleIds);

				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				return StatusCode(500, "A problem happened while KitLocale properties.");
			}

		}

		private IEnumerable<Object> GetKits(int kitId, bool loadChildObjects)
        {
            if (loadChildObjects)
            {
                var kitLinkGroupItems = kitRepository.UnitOfWork.Context.KitLinkGroupItem.FromSql<KitLinkGroupItem>(@"select klgi.*
                                                           from kit 
                                                            inner join kitlinkgroup klg on klg.KitId = kit.KitId
                                                            inner join KitLinkGroupItem klgi on klgi.KitLinkGroupId= klg.KitLinkGroupId
                                                             where kit.kitid = {0};", kitId).ToList();

                return
                    from k in kitRepository.UnitOfWork.Context.Kit
                    where k.KitId == kitId
                    select new
                    {
                        k.KitId,
                        k.Description,
                        k.KitType,
                        k.isDisplayMandatory,
                        k.showRecipe,
                        k.Item,
                        KitLinkGroup = k.KitLinkGroup.Select(x => new
                        {
                            x.KitLinkGroupId,
                            x.LinkGroupId,
                            x.KitId,
                            KitLinkGroupItems = from klgi2 in kitLinkGroupItems
                                                where klgi2.KitLinkGroupId == x.KitLinkGroupId
                                                join lgi2 in linkGroupItemRepository.GetAll() on klgi2.LinkGroupItemId equals lgi2.LinkGroupItemId
                                                select new
                                                {
                                                    klgi2.KitLinkGroupId,
                                                    x.LinkGroupId,
                                                    klgi2.LinkGroupItemId,
                                                }
                        }),
                        KitInstructionList = k.KitInstructionList.Select(kil => new
                        {
                            kil.KitId,
                            kil.InstructionListId,
                            kil.InstructionList,
                        })
                    };
            }

            else
            {
                return kitRepository.GetAll().Where(kit => kit.KitId == kitId);
            }
        }
        [HttpGet("{kitId}/{loadChildObjects?}", Name = "GetKitByKitId")]
        public IActionResult GetKitbyKitId(int kitId, bool loadChildObjects = false)
        {

            var kit = GetKits(kitId, loadChildObjects);
            if (kit == null)
            {
                logger.LogWarning("Kit does not exist.");
                return BadRequest();
            }

            return Ok(kit);
        }


        [HttpGet(Name = "GetKits")]
        public IActionResult GetKits(KitSearchParameters kitSearchParameters)
        {
            if (kitSearchParameters == null)
            {
                logger.LogWarning("The kitSearchParameters object passed is either null or does not contain any rows.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kitsBeforePaging = from k in kitRepository.GetAll()
                                   join i in itemsRepository.GetAll() on k.ItemId equals i.ItemId
                                   select new KitDtoWithStatus
                                   {
                                       Description = k.Description,
                                       InsertDateUtc = k.InsertDateUtc,
                                       LastUpdatedDateUtc = k.LastUpdatedDateUtc,
                                       ItemId = k.ItemId,
                                       KitId = k.KitId,
                                       KitStatus = GetKitStatusFromLocales(k.KitLocale.ToArray()),
                                       KitType = k.KitType,
                                       Item = new ItemsDto
                                       {
                                           ScanCode = i.ScanCode,
                                           ProductDesc = i.ProductDesc,
										   PosDesc = i.PosDesc,
                                           CustomerFriendlyDesc = i.CustomerFriendlyDesc
                                       }
                                   };

            BuildQueryToFilterKitData(kitSearchParameters, ref kitsBeforePaging);

            var kitsAfterPaging = PagedList<KitDtoWithStatus>.Create(kitsBeforePaging,
                kitSearchParameters.PageNumber,
                kitSearchParameters.PageSize
            );

            var paginationMetadata = kitHelper.GetPaginationData(kitsAfterPaging, kitSearchParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(kitsAfterPaging.ShapeData(kitSearchParameters.Fields));
        }

        [HttpGet("{kitId}/GetKitLocale", Name = "GetKitLocale")]
        public IActionResult GetKitLocale(int KitId)
        {
            var queryBuilder = BuildKitLocaleQuery(KitId);
            try
            {
                var matchingElements = queryBuilder.ToList();
                if (matchingElements == null)
                {
                    logger.LogWarning("The object passed is either null or does not contain any rows.");
                    return NotFound();
                }
                return Ok(matchingElements);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{storeId}/GetAllKitsForStore", Name = "GetAllKitsForStore")]
        public IActionResult GetAllKitsForStore(int storeId)
        {
            var store = localeRepository.GetAll().Where(k => k.LocaleId == storeId && k.LocaleTypeId == LocaleTypes.Store).FirstOrDefault();
            var venueCount = localeRepository.GetAll().Where(k => k.StoreId == storeId && k.LocaleTypeId == LocaleTypes.Venue && k.Hospitality==true).Count();

            try
            {
                if(venueCount == 0)
                {
                    return BadRequest("No venue exists for selected store.");
                }

                if (store == null)
                {
                    logger.LogWarning("The store does not exist");
                    return BadRequest();
                }

                var kits = (from kl in kitLocaleRepository.GetAll()
                           join lr in localeRepository.GetAll() on new { s = storeId, lt = LocaleTypes.Store } equals new { s = lr.LocaleId, lt = lr.LocaleTypeId }
                           join s in kitRepository.GetAll() on kl.KitId equals s.KitId
                           join i in itemsRepository.GetAll() on s.ItemId equals i.ItemId
                           join l in localeRepository.GetAll() on kl.LocaleId equals l.LocaleId
                           join st in statusRepository.GetAll() on kl.StatusId equals st.StatusId
                            where l.LocaleId == lr.MetroId || l.LocaleId == lr.RegionId || l.LocaleId == lr.ChainId || l.StoreId == lr.LocaleId || l.LocaleId == lr.LocaleId
                            select new
                           {
                               kitId = s.KitId,
                               kitDescription = s.Description,
                               scanCode = i.ScanCode,
                               productDescription  = i.ProductDesc,
                               excluded = kl.Exclude,
                               status = st.StatusDescription,
                               localeTypeId = l.LocaleTypeId 
                           }).ToList();

                if (kits == null)
                {
                    return NotFound();
                }

                var excludedkitIds = kits.Where(k => k.excluded == true && k.localeTypeId != LocaleTypes.Venue).Select(s => s.kitId).ToList();
                var kitsWithNoVenueIncluded = (kits.Where(k => k.excluded == true && k.localeTypeId == LocaleTypes.Venue).GroupBy(k => k.kitId).Where(grp => grp.Count() == venueCount)).Select(s=>s.Key);

                var kitsAssignedToStore = (kits
                         .Where(x => !excludedkitIds.Contains(x.kitId) && !kitsWithNoVenueIncluded.Contains(x.kitId))
                         .ToList()).GroupBy(s => s.kitId).Select(s => s.First());

                return Ok(kitsAssignedToStore);
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        internal IQueryable<KitLocaleResponse> BuildKitLocaleQuery(int kitId)
        {
            return
                from l in localeRepository.GetAll()
                join kl in kitLocaleRepository.GetAll().Where(x => x.KitId == kitId) on l.LocaleId equals kl.LocaleId
                into temp
                from de in temp.DefaultIfEmpty()
                select new KitLocaleResponse()
                {
                    KitId = de == null ? null : (int?)de.KitId,
                    LocaleId = de == null ? null : (int?)de.LocaleId,
                    KitLocaleId = de == null ? null : (int?)de.KitLocaleId,
                    LocaleName = l.LocaleName,
                    LocaleTypeId = l.LocaleTypeId,
                    StoreId = l.StoreId,
                    MetroId = l.MetroId,
                    RegionId = l.RegionId,
                    ChainId = l.ChainId,
                    StoreAbbreviation = l.StoreAbbreviation,
                    RegionCode = l.RegionCode,
                    BusinessUnitId = l.BusinessUnitId,
                    Exclude = de == null ? null : (bool?)de.Exclude,
                    StatusId = de == null ? null : (int?)de.StatusId
                };
        }
        [HttpPost("{kitLocaleId}/UpdateMaximumCalories", Name = "UpdateMaximumCalories")]
        public IActionResult UpdateMaximumCalories(int kitLocaleId, [FromBody]int MaximumCalories)
        {
            var kitLocale = kitLocaleRepository.GetAll().Where(x => x.KitLocaleId == kitLocaleId).FirstOrDefault();
            if (kitLocale == null)
            {

                return BadRequest("Kit Locale Record does not exist for passed Id");
            }
            kitLocale.MaximumCalories = MaximumCalories;
            try
            {
                kitLocaleRepository.UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPost("{kitId}/AssignUnassignLocations", Name = "AssignUnassignLocations")]
        public IActionResult AssignUnassignLocations(
                   [FromBody] List<AssignKitToLocaleDto> assignKitToLocaleDtoList, int kitId)
        {
            if (assignKitToLocaleDtoList == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kit = kitRepository.GetAll().Where(k => k.KitId == kitId).FirstOrDefault();

            if (kit == null)
            {
                logger.LogWarning("The kit does not exist");
                return BadRequest();
            }

            List<KitLocale> kitLocaleDbList = kitLocaleRepository.GetAll().Where(kl => kl.KitId == kitId).ToList();

            List<KitLocale> kitLocaleListPassed = new List<KitLocale>();
            foreach (var assignKitToLocaleDto in assignKitToLocaleDtoList)
            {
                KitLocale kitLocale = new KitLocale
                {
                    KitId = kitId,
                    LocaleId = assignKitToLocaleDto.LocaleId,
                    Exclude = assignKitToLocaleDto.IsExcluded,
                    StatusId = (int)StatusType.IP,
                    MaximumCalories = null,
                    MinimumCalories = null
                };

                kitLocale.InsertDateUtc = DateTime.UtcNow;
                kitLocale.LastUpdatedDateUtc = DateTime.UtcNow;
                kitLocaleListPassed.Add(kitLocale);
            }

            // delete records that exist in db but not in list. Cascade delete is enabled so child records will be deleted.
            var kitLocaleRecordsToDelete = kitLocaleDbList.Where(t => !kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));

            // records that are in list passed but not in database
            var kitLocaleRecordsToAdd = kitLocaleListPassed.Where(t => !kitLocaleDbList.Select(l => l.LocaleId).Contains(t.LocaleId)).ToList();
            foreach (var kitLocale in kitLocaleRecordsToAdd)
            {
                kitLocale.InsertDateUtc = DateTime.UtcNow;
                kitLocale.LastUpdatedDateUtc = DateTime.Now;

                if (kit.KitType == KitType.Simple)
                {
                    kitLocale.StatusId = (int)LocaleStatus.ReadytoPublish;
                }
                else
                {
                    kitLocale.StatusId = (int)LocaleStatus.Building;
                }
            }

            var kitLocaleRecordsToUpdate = kitLocaleDbList.Where(t => kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));

            kitLocaleRepository.UnitOfWork.Context.KitLocale.RemoveRange(kitLocaleRecordsToDelete);
            kitLocaleRepository.UnitOfWork.Context.KitLocale.AddRange(kitLocaleRecordsToAdd);

            foreach (KitLocale kitToUpdate in kitLocaleRecordsToUpdate)
            {
                KitLocale currentKit = kitLocaleDbList.Where(kl => kl.KitLocaleId == kitToUpdate.KitLocaleId).FirstOrDefault();
                KitLocale toBeUpdatedKit = kitLocaleListPassed.Where(kl => kl.KitId == kitId && kl.LocaleId == kitToUpdate.LocaleId).FirstOrDefault();
                currentKit.MaximumCalories = toBeUpdatedKit.MaximumCalories;
                currentKit.MinimumCalories = toBeUpdatedKit.MinimumCalories;
                currentKit.Exclude = toBeUpdatedKit.Exclude;
                currentKit.LastUpdatedDateUtc = DateTime.UtcNow;

                //if (kitToUpdate.Exclude != true && kit.KitType == KitType.Simple)
                //{
                //    kitToUpdate.StatusId = (int)LocaleStatus.ReadytoPublish;
                //}
            }

            try
            {
                kitLocaleRepository.UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPut(Name = "PublishKit")]
        public IActionResult Publish([FromBody]int kitId)
        {
            var kit = kitRepository.Find(k => k.KitId == kitId);
            List<string> storesWithNoProperties = new List<string>();

            if (kit == null) return NotFound();
            if (kit.KitType != KitType.Simple)
            {
                var kitlocaleList = (from kl in kitLocaleRepository.GetAll().Where(k => k.KitId == kitId && k.Exclude == false).ToList()
                                     join kli in kitLinkGroupLocaleRepository.GetAll() on kl.KitLocaleId equals kli.KitLocaleId into ps
                                     from sub in ps.DefaultIfEmpty()
                                     select new
                                     {
                                         isPropertiesExist = sub != null ? true : false
                                     }).ToList();

                if (kitlocaleList == null)
                {
                    return NotFound();
                }

                if (kitlocaleList.Where(k => k.isPropertiesExist == false).Any())
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }
            }
            try
            {
                var paramKitId = new SqlParameter("kitId", SqlDbType.BigInt) { Value = kitId };
                var paramAction = new SqlParameter("action", SqlDbType.NVarChar) { Value = ADD_OR_UPDATE_ACTION };
                var paramlocaleListWithNoVenues = new SqlParameter("localeListWithNoVenues", SqlDbType.NVarChar, -1);
                paramlocaleListWithNoVenues.Direction = ParameterDirection.Output;

                var sql = "exec publishKitEvents @kitId, @Action, @localeListWithNoVenues OUT";

                linkGroupRepository.UnitOfWork.Context.Database.ExecuteSqlCommand(sql, paramKitId, paramAction, paramlocaleListWithNoVenues);

                if (!string.IsNullOrEmpty(paramlocaleListWithNoVenues.Value.ToString()))
                {
                    string error = "Locales: " + paramlocaleListWithNoVenues.Value.ToString() + " do not have associated hospitality venues. Please remove kit assignment from these locales.";
                    return StatusCode(409, error);
                }
                return NoContent();
            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPost("{kitId}", Name = "SaveKitProperties")]
        public IActionResult SaveKitProperties(
          [FromBody] KitPropertiesDto kitPropertiesDto)
        {
            if (kitPropertiesDto == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var kitLocaleID = kitPropertiesDto.KitLinkGroupLocaleList.FirstOrDefault().KitLocaleId;
            var kitLocale = kitLocaleRepository.Get(kitLocaleID);

            var passedInKitLinkGroupItemLocales = kitPropertiesDto.KitLinkGroupLocaleList.SelectMany(x => x.KitLinkGroupItemLocaleList);

            var existingKitLinkGroupLocals = kitLinkGroupLocaleRepository.GetAll().Where(klgl => klgl.KitLocaleId == kitLocaleID);


            var existingKitLinkGroupItemLocals = from klgl in kitLinkGroupItemLocaleRepository.GetAll()
                                                 join klg in passedInKitLinkGroupItemLocales on klgl.KitLinkGroupLocaleId equals klg.KitLinkGroupLocaleId
                                                 select klgl;

            var kitLinkGroupLocaleRecordsToAdd = kitPropertiesDto.KitLinkGroupLocaleList.Where(t => !existingKitLinkGroupLocals.Select(l => l.KitLinkGroupLocaleId).Contains(t.KitLinkGroupLocaleId));

            var kitLinkGroupLocalesToUpdate = existingKitLinkGroupLocals.Where(t => kitPropertiesDto.KitLinkGroupLocaleList.Select(l => l.KitLinkGroupLocaleId).Contains(t.KitLinkGroupLocaleId));
            var kitLinkGroupLocalesToRemove = existingKitLinkGroupLocals.Where(t => !kitPropertiesDto.KitLinkGroupLocaleList.Select(l => l.KitLinkGroupLocaleId).Contains(t.KitLinkGroupLocaleId));

            IEnumerable<KitLinkGroupLocale> kitLinkGroupLocalesToAdd = ConvertPropertiesToLinkGroupLocale(kitLinkGroupLocaleRecordsToAdd, kitPropertiesDto.KitLocaleId);
            foreach (var kitLinkGroupLocale in kitLinkGroupLocalesToAdd)
            {
                kitLinkGroupLocale.InsertDateUtc = DateTime.UtcNow;
                kitLinkGroupLocale.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            //Add brand new KitLinkGroupLocale records and their corresponding kids (KitLinkGroupItemLocale) records
            kitLinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupLocale.AddRange(kitLinkGroupLocalesToAdd);
            kitLinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupLocale.RemoveRange(kitLinkGroupLocalesToRemove);

            UpdateKitLinkGroupLocale(kitLinkGroupLocalesToUpdate, kitPropertiesDto);

            var kitLinkGroupItemLocalesToAdd = passedInKitLinkGroupItemLocales.Where(pi => pi.KitLinkGroupLocaleId > 0 && pi.KitLinkGroupItemLocaleId == 0);
            var kitLinkGroupItemLocalesToUpdate = existingKitLinkGroupItemLocals.Where(t => kitPropertiesDto.KitLinkGroupLocaleList.SelectMany(i => i.KitLinkGroupItemLocaleList).Select(l => l.KitLinkGroupItemLocaleId).Contains(t.KitLinkGroupItemLocaleId));
            var kitLinkGroupItemLocalesToRemove = existingKitLinkGroupItemLocals.Where(t => !kitPropertiesDto.KitLinkGroupLocaleList.SelectMany(i => i.KitLinkGroupItemLocaleList).Select(l => l.KitLinkGroupItemLocaleId).Contains(t.KitLinkGroupItemLocaleId));

            IEnumerable<KitLinkGroupItemLocale> kitLinkGroupItemLocales = ConvertPropertiesToLinkGroupItemLocale(kitLinkGroupItemLocalesToAdd).ToList();
            foreach (var kitLinkGroupItemLocale in kitLinkGroupItemLocales)
            {
                kitLinkGroupItemLocale.InsertDateUtc = DateTime.UtcNow;
                kitLinkGroupItemLocale.LastUpdatedDateUtc = DateTime.UtcNow;
            }

            kitLinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupItemLocale.AddRange(kitLinkGroupItemLocales);
            kitLinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupItemLocale.RemoveRange(kitLinkGroupItemLocalesToRemove);

            UpdateKitLinkGroupItemLocale(kitLinkGroupItemLocalesToUpdate, kitPropertiesDto);

            kitLocale.StatusId = (int)LocaleStatus.ReadytoPublish;
            try
            {
                kitLinkGroupLocaleRepository.UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        private void CreateNewKit(KitDto newKit)
        {
            var kit = new Kit()
            {
                Description = newKit.Description,
                ItemId = newKit.ItemId,
                InsertDateUtc = DateTime.UtcNow,
                LastUpdatedDateUtc = DateTime.UtcNow,
                isDisplayMandatory = newKit.isDisplayMandatory,
                showRecipe = newKit.showRecipe,
                KitType = newKit.KitType
            };

            //add a kit to the database first, so we can get a kitId
            var addedKit = kitRepository.Add(kit);

            if (newKit.KitType != KitType.Simple)
            {
                var addedKitLinkGroups = AddNewLinkedGroups(newKit.KitLinkGroup, addedKit);

                var linkedGroupItems = newKit.KitLinkGroup.SelectMany(lg => lg.KitLinkGroupItem);
                AddLinkedGroupItems(addedKitLinkGroups, linkedGroupItems.ToList());
            }

            AddNewInstrunctionLists(newKit.KitInstructionList, addedKit);
        }

        private void AddLinkedGroupItems(ICollection<KitLinkGroup> addedKitLinkGroups, ICollection<KitLinkGroupItemDto> linkedGroupItems)
        {
            //now last we need to add the items, but associate them with the kitlinkedgroup's
            var linkedGroupItemIds = linkedGroupItems.Select(lgi => lgi.LinkGroupItemId);
            var linkedGroupItemsToAdd = linkGroupItemRepository.GetAll()
                .Where(lgi => linkedGroupItemIds.Contains(lgi.LinkGroupItemId) && addedKitLinkGroups.Select(x => x.LinkGroupId).Contains(lgi.LinkGroupId))
                .Select(lgi => new KitLinkGroupItem()
                {
                    KitLinkGroupId = addedKitLinkGroups.Where(klg => klg.LinkGroupId == lgi.LinkGroupId).Select(klg => klg.KitLinkGroupId).First(),
                    LinkGroupItemId = lgi.LinkGroupItemId
                });


            //now add the LinkedGroupItems
            kitLinkGroupItemRepository.UnitOfWork.Context.KitLinkGroupItem.AddRange(linkedGroupItemsToAdd);
            kitLinkGroupItemRepository.UnitOfWork.Commit();
        }
        private ICollection<KitLinkGroup> AddNewLinkedGroups(ICollection<KitLinkGroupDto> linkGroups, Kit addedKit)
        {
            var linkedGroupIds = linkGroups.Select(lg => lg.LinkGroupId);
            //get the linked groups to be added from the database
            var linkedGroupsToAdd = linkGroupRepository.GetAll()
                .Where(lg => linkedGroupIds.Contains(lg.LinkGroupId))
                .Select(lg => new KitLinkGroup()
                {
                    KitId = addedKit.KitId,
                    LinkGroupId = lg.LinkGroupId
                });
            kitLinkGroupRepository.UnitOfWork.Context.KitLinkGroup.AddRange(linkedGroupsToAdd);
            kitLinkGroupRepository.UnitOfWork.Commit();
            return kitLinkGroupRepository.GetAll().Where(klg => klg.KitId == addedKit.KitId).ToList();
        }

        private void AddNewInstrunctionLists(ICollection<KitInstructionListDto> instructions, Kit addedKit)
        {
            //get just the id's of the instructions to be added
            var instructionIds = instructions.Select(klg => klg.InstructionListId);

            //now get the instruction objects from the database
            var instructionsToAdd = instructionListRepository.GetAll()
                .Where(instruction => instructionIds.Contains(instruction.InstructionListId))
                .Select(ins => new KitInstructionList()
                {
                    KitId = addedKit.KitId,
                    InstructionListId = ins.InstructionListId,

                });

            kitInstructionListRepository.UnitOfWork.Context.KitInstructionList.AddRange(instructionsToAdd);
            kitInstructionListRepository.UnitOfWork.Commit();
        }

        [HttpPost(Name = "SaveKit")]
        public IActionResult KitSaveDetails([FromBody]KitDto kitToSave)
        {
            var errorMessage = string.Empty;

            try
            {
                ValidateKitSaveParameters(kitToSave);

                var item = itemsRepository.GetAll().FirstOrDefault(i => i.ItemId == kitToSave.ItemId);

                if (item == null)
                {
                    errorMessage = $"SaveKit: Unable to find Item [id: {kitToSave.ItemId}]";
                    logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                if (kitToSave.KitId <= 0) //brand new kit
                {
                    var isItemAlreadyUsedForAKit =
                    kitRepository.GetAll()
                    .Where(kit => kit.ItemId == kitToSave.ItemId)
                    .ToArray().Length > 0;

                    if (isItemAlreadyUsedForAKit)
                    {
                        errorMessage = $"SaveKit: ItemId {kitToSave.ItemId} is already associated with a kit.";
                        logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    }
                    CreateNewKit(kitToSave);
                }
                else
                {
                    var kit = kitRepository.GetAll().Where(k => k.KitId == kitToSave.KitId).FirstOrDefault();
                    var kitLocales = kitLocaleRepository.GetAll().Where(k => k.KitId == kitToSave.KitId);

                    if (kitToSave.KitType != KitType.Simple && (kitLocales.Where(k => k.StatusId == (int)LocaleStatus.Published
                            || k.StatusId == (int)LocaleStatus.PublishQueued || k.StatusId == (int)LocaleStatus.Modifying
                            || k.StatusId == (int)LocaleStatus.PublishReQueued).Any()))
                    {
                        foreach (KitLocale kitlocale in kitLocales)
                        {
                            kitlocale.StatusId = (int)LocaleStatus.Modifying;
                        }
                    }

                    else if (kitToSave.KitType == KitType.Simple)
                    {
                        foreach (KitLocale kitlocale in kitLocales)
                        {
                            kitlocale.StatusId = (int)LocaleStatus.ReadytoPublish;
                        }
                    }
                    else
                    {
                        foreach (KitLocale kitlocale in kitLocales)
                        {
                            kitlocale.StatusId = (int)LocaleStatus.Building;
                        }
                    }
                    if (kit == null)
                    {
                        // error
                        errorMessage = $"SaveKit: Unable to find Kit [id: {kitToSave.KitId}]";
                        logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    }

                    var existingKitInstructionLists =
                        kitInstructionListRepository.GetAll().Where(kil => kil.KitId == kit.KitId);

                    var newKitInstructionLists = from id in kitToSave.KitInstructionList
                                                 .Where(i => !existingKitInstructionLists.Select(l => l.InstructionListId).Contains(i.InstructionListId))
                                                 select new KitInstructionList()
                                                 {
                                                     InstructionListId = id.InstructionListId,
                                                     KitId = kitToSave.KitId
                                                 };

                    var kitInstructionListsToRemove = existingKitInstructionLists.Where(i => !kitToSave.KitInstructionList.Select(l => l.InstructionListId).Contains(i.InstructionListId));

                    var existingKitLinkGroups =
                        kitLinkGroupRepository.GetAll().Where(klg => klg.KitId == kitToSave.KitId);

                    var newKitLinkGroupsAndTheirNewItems = kitToSave.KitLinkGroup.Where(klg => !existingKitLinkGroups.Select(el => el.LinkGroupId).Contains(klg.LinkGroupId))
                                           .Select(lg => new KitLinkGroup()
                                           {
                                               KitId = kitToSave.KitId,
                                               LinkGroupId = lg.LinkGroupId,
                                               KitLinkGroupItem = ConvertToLinkGroupItemList(kitToSave.KitLinkGroup.Where(l => l.LinkGroupId == lg.LinkGroupId).SelectMany(i => i.KitLinkGroupItem)),
                                               LastUpdatedDateUtc = DateTime.UtcNow,
                                               InsertDateUtc = DateTime.UtcNow
                                           });

                    var KitLinkGroupsToRemove = existingKitLinkGroups.Where(i => !kitToSave.KitLinkGroup.Select(k => k.LinkGroupId).Contains(i.LinkGroupId));

                    if (KitLinkGroupsToRemove.Count() > 0)
                    {
                        var localeRecordExists = kitLinkGroupLocaleRepository.GetAll().Where(klgl => klgl.Exclude == false && KitLinkGroupsToRemove.Select(k => k.KitLinkGroupId).Contains(klgl.KitLinkGroupId)).Any();

                        if (localeRecordExists)
                        {
                            return StatusCode(StatusCodes.Status409Conflict);
                        }
                    }
                    var kitLinkGroupItemsThatHaveExistingKitLinkGroupParent = kitToSave.KitLinkGroup.SelectMany(klg => klg.KitLinkGroupItem.Select(klgi => new KitLinkGroupItem()
                    {
                        InsertDateUtc = DateTime.UtcNow,
                        KitLinkGroup = existingKitLinkGroups.Where(eklg => eklg.LinkGroupId == klg.LinkGroupId).FirstOrDefault(),
                        LinkGroupItemId = klgi.LinkGroupItemId,
                    })).Where(klgi => klgi.KitLinkGroup != null)
                    .Select(klgi =>
                    {
                        klgi.KitLinkGroupId = klgi.KitLinkGroup.KitLinkGroupId;
                        return klgi;
                    });

                    var existingKitLinkGroupItems = kitLinkGroupItemRepository.GetAll().Include(klgi => klgi.KitLinkGroup).Where(klgi => klgi.KitLinkGroup.KitId == kitToSave.KitId);

                    var newKitLinkGroupItemsThatHaveExistingParentKitLinkGroup = kitLinkGroupItemsThatHaveExistingKitLinkGroupParent
                        .Where(klgi => !existingKitLinkGroupItems.Select(eklgi => eklgi.LinkGroupItemId).Contains(klgi.LinkGroupItemId));

                    var KitLinkGroupItemsToRemove = from e in existingKitLinkGroupItems
                                                    join i in kitToSave.KitLinkGroup.SelectMany(li => li.KitLinkGroupItem)
                                                         on e.LinkGroupItemId equals i.LinkGroupItemId into ps
                                                    from sub in ps.DefaultIfEmpty()
                                                    where sub == null
                                                    select e;

                    kit.KitType = kitToSave.KitType;
                    kit.Description = kitToSave.Description;
                    kit.isDisplayMandatory = kitToSave.isDisplayMandatory;
                    kit.showRecipe = kitToSave.showRecipe;
                    kit.ItemId = kitToSave.ItemId;
                    kit.LastUpdatedDateUtc = DateTime.UtcNow;

                    kitRepository.UnitOfWork.Context.KitInstructionList.RemoveRange(kitInstructionListsToRemove);
                    kitRepository.UnitOfWork.Context.KitInstructionList.AddRange(newKitInstructionLists);

                    kitRepository.UnitOfWork.Context.KitLinkGroupItem.RemoveRange(KitLinkGroupItemsToRemove);
                    kitRepository.UnitOfWork.Context.KitLinkGroupItem.AddRange(newKitLinkGroupItemsThatHaveExistingParentKitLinkGroup);

                    kitRepository.UnitOfWork.Context.KitLinkGroup.RemoveRange(KitLinkGroupsToRemove);
                    kitRepository.UnitOfWork.Context.KitLinkGroup.AddRange(newKitLinkGroupsAndTheirNewItems);
                }
                kitRepository.UnitOfWork.Commit();
                var kitSaved = kitRepository.GetAll().Where(k => k.ItemId == kitToSave.ItemId).FirstOrDefault();
                return Ok(kitSaved);
            }
            catch (DbUpdateConcurrencyException DbConcurrencyEx)
            {
                logger.LogError($"SaveKit: Concurrency Error Saving Kit. [id: {kitToSave.KitId}]");
                logger.LogError(DbConcurrencyEx.Message);
                return BadRequest($"SaveKit: Concurrency Error Saving Kit. [id: {kitToSave.KitId}]");

            }
            catch (DbUpdateException DbUpdateException)
            {
                logger.LogError($"SaveKit: Database Update Error Saving Kit. [id: {kitToSave.KitId}]");
                logger.LogError(DbUpdateException.Message);
                return BadRequest("Error in Saving Kit. Make sure you are not removing modifier in use.");
            }
            catch (Exception Ex)
            {
                logger.LogError($"SaveKit: Error Saving Kit. [id: {kitToSave.KitId}]");
                logger.LogError(Ex.Message);
                return BadRequest(Ex.Message);
            }
        }

        internal void UpdateKitLinkGroupItemLocale(IQueryable<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRecordsToUpdate, KitPropertiesDto kitPropertiesDto)
        {
            foreach (KitLinkGroupItemLocale kitLinkGroupItemLocale in kitLinkGroupItemLocaleRecordsToUpdate)
            {
                var kitLinkGroupItemLocalePropertiesDto = kitPropertiesDto.KitLinkGroupLocaleList.SelectMany(i => i.KitLinkGroupItemLocaleList).Where(s => s.KitLinkGroupItemId == kitLinkGroupItemLocale.KitLinkGroupItemId).FirstOrDefault();
                kitLinkGroupItemLocale.Properties = kitLinkGroupItemLocalePropertiesDto.Properties;
                kitLinkGroupItemLocale.Exclude = kitLinkGroupItemLocalePropertiesDto.Excluded;
                kitLinkGroupItemLocale.DisplaySequence = (int)kitLinkGroupItemLocalePropertiesDto.DisplaySequence;
                kitLinkGroupItemLocale.LastUpdatedDateUtc = DateTime.UtcNow;
                kitLinkGroupItemLocale.LastModifiedBy = kitLinkGroupItemLocalePropertiesDto.LastModifiedBy;
            }
        }

        internal void UpdateKitLinkGroupLocale(IQueryable<KitLinkGroupLocale> kitLinkGroupLocaleRecordsToUpdate, KitPropertiesDto kitPropertiesDto)
        {
            foreach (KitLinkGroupLocale kitLinkGroupLocale in kitLinkGroupLocaleRecordsToUpdate)
            {
                var KitLinkGroupLocalePropertiesDto = kitPropertiesDto.KitLinkGroupLocaleList.Where(s => s.KitLinkGroupLocaleId == kitLinkGroupLocale.KitLinkGroupLocaleId).FirstOrDefault();
                kitLinkGroupLocale.Properties = KitLinkGroupLocalePropertiesDto.Properties;
                kitLinkGroupLocale.Exclude = KitLinkGroupLocalePropertiesDto.Excluded;
                kitLinkGroupLocale.LastUpdatedDateUtc = DateTime.UtcNow;
                kitLinkGroupLocale.DisplaySequence = (int)KitLinkGroupLocalePropertiesDto.DisplaySequence;
                kitLinkGroupLocale.LastModifiedBy = KitLinkGroupLocalePropertiesDto.LastModifiedBy;
            }
        }

        internal List<KitLinkGroupItemLocale> ConvertPropertiesToLinkGroupItemLocale(IEnumerable<PropertiesDto> kitLinkGroupItemLocaleRecordsToAdd)
        {
            List<KitLinkGroupItemLocale> KitLinkGroupItemLocaleList = new List<KitLinkGroupItemLocale>();

            foreach (PropertiesDto propertiesDto in kitLinkGroupItemLocaleRecordsToAdd)
            {
                KitLinkGroupItemLocale KitLinkGroupItemLocale = new KitLinkGroupItemLocale
                {
                    KitLinkGroupItemId = propertiesDto.KitLinkGroupItemId,
                    KitLinkGroupLocaleId = propertiesDto.KitLinkGroupLocaleId,
                    Properties = propertiesDto.Properties,
                    DisplaySequence = (int)propertiesDto.DisplaySequence,
                    Exclude = propertiesDto.Excluded,
                    LastModifiedBy = propertiesDto.LastModifiedBy
                };
                KitLinkGroupItemLocaleList.Add(KitLinkGroupItemLocale);
            }

            return KitLinkGroupItemLocaleList;
        }

        internal List<KitLinkGroupLocale> ConvertPropertiesToLinkGroupLocale(IEnumerable<KitLinkGroupPropertiesDto> kitLocaleRecordsToAdd, int kitLocaleId)
        {
            List<KitLinkGroupLocale> KitLinkGroupLocaleList = new List<KitLinkGroupLocale>();

            foreach (KitLinkGroupPropertiesDto kitLinkGroupPropertiesDto in kitLocaleRecordsToAdd)
            {
                KitLinkGroupLocale KitLinkGroupLocale = new KitLinkGroupLocale
                {
                    KitLinkGroupId = kitLinkGroupPropertiesDto.KitLinkGroupId,
                    KitLocaleId = kitLocaleId,
                    Properties = kitLinkGroupPropertiesDto.Properties,
                    DisplaySequence = (int)kitLinkGroupPropertiesDto.DisplaySequence,
                    MinimumCalories = kitLinkGroupPropertiesDto.MaximumCalories,
                    MaximumCalories = kitLinkGroupPropertiesDto.MinimumCalories,
                    Exclude = kitLinkGroupPropertiesDto.Excluded,
                    KitLinkGroupItemLocale = ConvertPropertiesToLinkGroupItemLocale(kitLinkGroupPropertiesDto.KitLinkGroupItemLocaleList),
                };
                KitLinkGroupLocaleList.Add(KitLinkGroupLocale);
            }

            return KitLinkGroupLocaleList;
        }
        internal List<KitLinkGroupItem> ConvertToLinkGroupItemList(IEnumerable<KitLinkGroupItemDto> kitLinkGroupItems)
        {
            List<KitLinkGroupItem> kitLinkGroupItemList = new List<KitLinkGroupItem>();

            foreach (KitLinkGroupItemDto kitLinkGroupItemDto in kitLinkGroupItems)
            {

                KitLinkGroupItem kitLinkGroupItem = new KitLinkGroupItem
                {
                    KitLinkGroupId = kitLinkGroupItemDto.KitLinkGroupId,
                    LinkGroupItemId = kitLinkGroupItemDto.LinkGroupItemId,
                };
                kitLinkGroupItemList.Add(kitLinkGroupItem);
            }

            return kitLinkGroupItemList;
        }
        internal int? getlocaleIdAtWhichkitRecordExits(int kitId, int localeId)
        {
            int? localeIdWithKitLocaleRecord = null;
            Locale locale = (from l in localeRepository.GetAll()
                             join lt in localeTypeRepository.GetAll() on l.LocaleTypeId equals lt.LocaleTypeId
                             where l.LocaleId == localeId
                             select l).FirstOrDefault();

            if (locale == null)
            {
                return null;
            }
            if (kitLocaleRepository.GetAll().Where(kl => kl.LocaleId == localeId && kl.KitId == kitId).Any())
            {
                return localeId;
            }
            else
            {
                switch (locale.LocaleTypeId)
                {
                    case LocaleTypes.Chain:
                        localeIdWithKitLocaleRecord = null;
                        break;
                    case LocaleTypes.Region:
                        localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, (int)locale.ChainId);
                        break;
                    case LocaleTypes.Metro:
                        localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, (int)locale.RegionId);
                        break;
                    case LocaleTypes.Store:
                        localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, (int)locale.MetroId);
                        break;
                    case LocaleTypes.Venue:
                        localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, (int)locale.StoreId);
                        break;
                }

                return localeIdWithKitLocaleRecord;
            }
        }

        internal IQueryable<Kit> BuildKitByLocaleIdQuery(int kitId, int localeId, bool loadChildObjects)
        {
            if (loadChildObjects)
            {
                return kitRepository.UnitOfWork.Context.Kit.Where(k => k.KitId == kitId)
                       .Include(k => k.KitLinkGroup).ThenInclude(s => s.LinkGroup)
                       .Include(k => k.KitLinkGroup).ThenInclude(k => k.KitLinkGroupItem).ThenInclude(d => d.LinkGroupItem)
                       .ThenInclude(lg => lg.Item)
                       .Include(k => k.KitLinkGroup).ThenInclude(k => k.KitLinkGroupItem).ThenInclude(kgi => kgi.KitLinkGroupItemLocale)
                       .Include(k => k.KitLocale).ThenInclude(f => f.KitLinkGroupLocale)
                       .Include(k => k.KitLocale)
                       .Where(k => k.KitLocale.Any(l => l.LocaleId == localeId));

            }
            else
            {
                return kitRepository.UnitOfWork.Context.Kit.Where(k => k.KitId == kitId)
                      .Include(k => k.KitLocale)
                      .Where(k => k.KitLocale.Any(l => l.LocaleId == localeId));
            }
        }

        internal KitPropertiesDto BuildKitPropertiesByLocaleId(int kitId, int localeIdWithKitLocaleRecord, int localeId)
        {
            var KitPropertiesDto = (from kr in kitRepository.GetAll().Where(ki => ki.KitId == kitId)
                                    join i in itemsRepository.GetAll() on kr.ItemId equals i.ItemId
                                    join l in localeRepository.GetAll() on localeId equals l.LocaleId
                                    join klr in kitLocaleRepository.GetAll()
                                    on new { kr.KitId } equals new { klr.KitId }
                                    where klr.LocaleId == localeIdWithKitLocaleRecord
                                    select new KitPropertiesDto
                                    {
                                        KitId = kr.KitId,
                                        ItemId = i.ItemId,
                                        Description = kr.Description,
                                        LocaleId = localeId,
                                        LocaleIdAtWhichKitExists = localeIdWithKitLocaleRecord,
                                        KitLocaleId = klr.KitLocaleId,
                                        ImageUrl = i.ImageUrl,
                                        LocaleName = l.LocaleName
                                    }).FirstOrDefault();

            if (KitPropertiesDto == null)
            {
                return null;
            }

            List<KitLinkGroupPropertiesDto> KitLinkGroupLocaleList = (from klg in kitLinkGroupRepository.GetAll().Where(klgr => klgr.KitId == kitId)
                                                                      join lg in linkGroupRepository.GetAll() on klg.LinkGroupId equals lg.LinkGroupId
                                                                      join kl in kitLocaleRepository.GetAll() on kitId equals kl.KitId into s
                                                                      from kl in s.DefaultIfEmpty()
                                                                      join klgl in kitLinkGroupLocaleRepository.GetAll()
                                                                      on new { kl.KitLocaleId, klg.KitLinkGroupId } equals new { klgl.KitLocaleId, klgl.KitLinkGroupId } into ps
                                                                      from klgl in ps.DefaultIfEmpty()
                                                                      where kl.LocaleId == localeIdWithKitLocaleRecord
                                                                      select new KitLinkGroupPropertiesDto
                                                                      {
                                                                          KitLocaleId = kl.KitLocaleId,
                                                                          KitLinkGroupLocaleId = klgl != null ? klgl.KitLinkGroupLocaleId : 0,
                                                                          KitLinkGroupItemLocaleId = 0,
                                                                          KitLinkGroupId = klg.KitLinkGroupId,
                                                                          KitLinkGroupItemId = 0,
                                                                          Name = lg.GroupName,
                                                                          Properties = klgl != null ? klgl.Properties : null,
                                                                          Excluded = klgl != null ? klgl.Exclude : null,
                                                                          MinimumCalories = klgl != null ? klgl.MinimumCalories : null,
                                                                          MaximumCalories = klgl != null ? (int?)klgl.MaximumCalories : null,
                                                                          DisplaySequence = klgl != null ? (int?)klgl.DisplaySequence : null,
                                                                          KitLinkGroupItemLocaleList = new HashSet<PropertiesDto>(),
                                                                      }).OrderByDescending(s => s.DisplaySequence.HasValue)
                                                                        .ThenBy(p => p.DisplaySequence).ThenBy(p => p.Name).ToList();


            List<PropertiesDto> KitLinkGroupItemLocaleList = (from klgl in KitLinkGroupLocaleList
                                                              join klg in kitLinkGroupRepository.GetAll() on klgl.KitLinkGroupId equals klg.KitLinkGroupId
                                                              join klgi in kitLinkGroupItemRepository.GetAll() on klgl.KitLinkGroupId equals klgi.KitLinkGroupId
                                                              join lgi in linkGroupItemRepository.GetAll()
                                                              on new { klgi.LinkGroupItemId } equals new { lgi.LinkGroupItemId }
                                                              // join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                                                              join klgil in kitLinkGroupItemLocaleRepository.GetAll()
                                                              on new { klgi.KitLinkGroupItemId, klgl.KitLinkGroupLocaleId } equals new { klgil.KitLinkGroupItemId, klgil.KitLinkGroupLocaleId } into kli
                                                              from klgil in kli.DefaultIfEmpty()
                                                              select new PropertiesDto
                                                              {
                                                                  KitLinkGroupLocaleId = klgl != null ? klgl.KitLinkGroupLocaleId : 0,
                                                                  KitLinkGroupItemLocaleId = klgil != null ? klgil.KitLinkGroupItemLocaleId : 0,
                                                                  KitLinkGroupId = klg.KitLinkGroupId,
                                                                  KitLinkGroupItemId = klgi.KitLinkGroupItemId,
                                                                  //  Name = i.ProductDesc,
                                                                  Properties = klgil != null ? klgil.Properties : null,
                                                                  Excluded = klgil != null ? klgil.Exclude : null,
                                                                  DisplaySequence = klgil != null ? (int?)klgil.DisplaySequence : null,
                                                              }).ToList();

            List<int> KitLinkGroupItemIds = KitLinkGroupItemLocaleList.Select(s => s.KitLinkGroupItemId).ToList();

            var itemsData = (from kl in kitLinkGroupItemRepository.GetAll().Where(s => KitLinkGroupItemIds.Contains(s.KitLinkGroupItemId))
                             join lgi in linkGroupItemRepository.GetAll() on kl.LinkGroupItemId equals lgi.LinkGroupItemId
                             join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                             select new
                             {
                                 i.ItemId,
                                 i.ProductDesc,
                                 i.ImageUrl,
                                 kl.KitLinkGroupItemId
                             }).ToList();

            foreach (PropertiesDto kitLinkGroupItemLocale in KitLinkGroupItemLocaleList)
            {
                kitLinkGroupItemLocale.Name = itemsData.Where(i => i.KitLinkGroupItemId == kitLinkGroupItemLocale.KitLinkGroupItemId).FirstOrDefault().ProductDesc;
            }

            foreach (KitLinkGroupPropertiesDto kitLinkGroupLocale in KitLinkGroupLocaleList)
            {
                List<PropertiesDto> kitLinkGroupItemLocales = KitLinkGroupItemLocaleList.Where(i => i.KitLinkGroupId == kitLinkGroupLocale.KitLinkGroupId).ToList();
                kitLinkGroupLocale.KitLinkGroupItemLocaleList = kitLinkGroupItemLocales.OrderByDescending(s => s.DisplaySequence.HasValue)
                                                                        .ThenBy(p => p.DisplaySequence).ThenBy(p => p.Name).ToList();
            };
            KitPropertiesDto.KitLinkGroupLocaleList = KitLinkGroupLocaleList;
            //KitPropertiesDto.KitLinkGroupItemLocaleList = KitLinkGroupItemLocaleList;




            return KitPropertiesDto;
        }

        internal void BuildQueryToFilterKitData(KitSearchParameters kitSearchParameters,
            ref IQueryable<KitDtoWithStatus> kitsBeforePaging)
        {
            if (!string.IsNullOrEmpty(kitSearchParameters.KitDescription))
            {
                var kitDescriptionForWhereClause = kitSearchParameters.KitDescription.Trim().ToLower();
                kitsBeforePaging = kitsBeforePaging.Where(k => k.Description.Contains(kitDescriptionForWhereClause));
            }
            if (!string.IsNullOrEmpty(kitSearchParameters.ItemScanCode))
            {
                var scancodeForWhereClause = kitSearchParameters.ItemScanCode.Trim().ToLower();
                kitsBeforePaging = from k in kitsBeforePaging
                                   join i in itemsRepository.GetAll() on k.ItemId equals i.ItemId
                                   where i.ScanCode.Contains(scancodeForWhereClause)
                                   select k;
            }

            if (!string.IsNullOrEmpty(kitSearchParameters.ItemDescription))
            {
                var itemDescriptionForWhereClause = kitSearchParameters.ItemDescription.Trim().ToLower();

                kitsBeforePaging = from k in kitsBeforePaging
                                   join i in itemsRepository.GetAll() on k.ItemId equals i.ItemId
                                   where i.ProductDesc.Contains(itemDescriptionForWhereClause)
                                   select k;
            }

            if (!string.IsNullOrEmpty(kitSearchParameters.LinkGroupName))
            {
                var linkGroupNameForWhereClause = kitSearchParameters.LinkGroupName.Trim().ToLower();
                kitsBeforePaging = from k in kitsBeforePaging
                                   join klg in kitLinkGroupRepository.GetAll() on k.KitId equals klg.KitId
                                   join lg in linkGroupRepository.GetAll() on klg.LinkGroupId equals lg.LinkGroupId
                                   where lg.GroupName.ToLower().Contains(linkGroupNameForWhereClause)
                                   select k;
            }
        }

        internal void ValidateKitSaveParameters(KitDto parameters)
        {
            var errorFound = false;
            var message = string.Empty;

            if (parameters.KitInstructionList == null)
            {
                errorFound = true;
                message = "InstructionListIds cannot be null";
            }

            if (parameters.KitLinkGroup == null)
            {
                errorFound = true;
                message = "LinkGroups cannot be null";
            }

            if (parameters.KitLinkGroup != null)
                if (parameters.KitLinkGroup.Select(i => i.KitLinkGroupItem) == null)
                {
                    errorFound = true;
                    message = "LinkGroupItems cannot be null";
                }

            if (errorFound) throw new ValidationException(message);
        }

        [HttpDelete("{id}", Name = "DeleteKit")]
        public IActionResult DeleteKit(int id)
        {
            var kitToDelete = BuildKitByKitIdQuery(id).FirstOrDefault();

            if (kitToDelete == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return NotFound();
            }

            if (!IsKitInUse(id))
            {
                try
                {
                    var param1 = new SqlParameter("kitId", SqlDbType.BigInt) { Value = id };
                    linkGroupRepository.ExecWithStoreProcedure(DELETE_KIT_SP_NAME + " @kitId", param1);

                    return NoContent();
                }

                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
        }



        private string AppendToAllModifiersProperties(ModifierView modifierView, string formattedAllModifiersProperties, bool isLast)
        {
            StringBuilder linkgroupModifierProperties = new StringBuilder();

            linkgroupModifierProperties.Append(formattedAllModifiersProperties);
            if (modifierView.Excluded == true)
                linkgroupModifierProperties.Append("^");



            linkgroupModifierProperties.Append(modifierView.ModifierName);

            linkgroupModifierProperties.Append("[");
            linkgroupModifierProperties.Append(modifierView.FormattedModifierProperties);
            linkgroupModifierProperties.Append("]");
            if (modifierView.AuthorizedByStore == false)
                linkgroupModifierProperties.Append("Unauthorized");

            if (!isLast)
            {
                linkgroupModifierProperties.Append(",");
                linkgroupModifierProperties.Append("\r");
            }


            return linkgroupModifierProperties.ToString();
        }

        private StringBuilder formatModifierProperties(string properties, KitLinkGroupItemLocaleDto KitLinkGroupItemLocaleDto)
        {
            dynamic data = JObject.Parse(properties);
            StringBuilder modifierProperties = new StringBuilder();

            modifierProperties.Append(KitLinkGroupItemLocaleDto.Calories);
            modifierProperties.Append(" Calories,");

            modifierProperties.Append(" $");
            modifierProperties.Append(KitLinkGroupItemLocaleDto.RegularPrice);
            modifierProperties.Append(",");
            modifierProperties.Append("\n");
            modifierProperties.Append(" Min=");
            modifierProperties.Append(data.Minimum);
            modifierProperties.Append(",");

            modifierProperties.Append(" Max=");
            modifierProperties.Append(data.Maximum);
            // modifierProperties.Append(",");
            // modifierProperties.Append("\n");
            //modifierProperties.Append(" NumOfFreeToppings = ");
            //modifierProperties.Append(data.NumOfFreePortions);
            //modifierProperties.Append(",");

            //modifierProperties.Append(" Default Portions=");
            //modifierProperties.Append(data.DefaultPortions);
            //modifierProperties.Append(",");

            //modifierProperties.Append(" MandatoryItem = ");
            //modifierProperties.Append(data.DefaultPortions);

            return modifierProperties;
        }

        private StringBuilder formatLinkGroupProperties(string properties)
        {
            dynamic data = JObject.Parse(properties);
            StringBuilder linkgroupProperties = new StringBuilder();

            linkgroupProperties.Append("Min=");
            linkgroupProperties.Append(data.Minimum);
            linkgroupProperties.Append(",");

            linkgroupProperties.Append("Max=");
            linkgroupProperties.Append(data.Maximum);
            linkgroupProperties.Append(",");
            linkgroupProperties.Append("\n");

            linkgroupProperties.Append("Free Toppings = ");
            linkgroupProperties.Append(String.IsNullOrEmpty(data.NumOfFreeToppings.ToString()) ? "Undefined" : data.NumOfFreeToppings);

            return linkgroupProperties;

        }

        internal IQueryable<Kit> BuildKitByKitIdQuery(int id)
        {
            return kitRepository.GetAll().Where(l => l.KitId == id);

        }

        internal bool IsKitInUse(int kitId)
        {
            var checkKitLocale = from kl in kitLocaleRepository.GetAll().Where(kl => kl.KitId == kitId)
                                 select kl;
            var checkLinkGroupLocale = from klg in kitLinkGroupItemLocaleRepository.GetAll()
                                       join kl in kitLinkGroupItemRepository.GetAll() on klg.KitLinkGroupItemId equals kl.KitLinkGroupItemId
                                       join k in kitLinkGroupRepository.GetAll() on kl.KitLinkGroupId equals k.KitLinkGroupId
                                       where k.KitId == kitId
                                       select k;


            return checkKitLocale.Any() || checkLinkGroupLocale.Any();

        }

        private string BuildErrorMessage(KitLocaleDto kitLocaleDto, KitType kitType)
        {
            StringBuilder errorMessages = new StringBuilder();

            if (kitLocaleDto.AuthorizedByStore == false)
            {
                errorMessages.Append(MAIN_ITEM_NOT_AUTHORIZED);
                errorMessages.Append("\n");
            }
            //If the main item is not authorized, the CaloricCalculator will not check the nutrition info for main item and modifiers.
            else
            {
                if (kitLocaleDto.MinimumCalories == null)
                {
                    errorMessages.Append(MAIN_ITEM_NO_CALORIE);
                    errorMessages.Append("\n");
                }

                if (kitType != KitType.Simple)
                {
                    int unauthorizedCounter = (from kitLinkGroupLocale in kitLocaleDto.KitLinkGroupLocale
                                               where !(bool)kitLinkGroupLocale.Exclude
                                               from kitLinkGroupItemLocales in kitLinkGroupLocale.KitLinkGroupItemLocales
                                               where !(bool)kitLinkGroupItemLocales.Exclude && !(bool)kitLinkGroupItemLocales.AuthorizedByStore
                                               select kitLinkGroupItemLocales).Count();

                    if (unauthorizedCounter > 0)
                    {
                        errorMessages.Append(MODIFIER_NOT_AUTHORIZED);
                        errorMessages.Append("\n");
                    }

                    int noCaloriesCounter = (from kitLinkGroupLocale in kitLocaleDto.KitLinkGroupLocale
                                             where !(bool)kitLinkGroupLocale.Exclude
                                             from kitLinkGroupItemLocales in kitLinkGroupLocale.KitLinkGroupItemLocales
                                             where !(bool)kitLinkGroupItemLocales.Exclude && kitLinkGroupItemLocales.Calories == null
                                             select kitLinkGroupItemLocales).Count();
                    if (noCaloriesCounter > 0)
                    {
                        errorMessages.Append(MODIFIER_NO_CALORIE);
                        errorMessages.Append("\n");
                    }
                }
            }
            return errorMessages.ToString().TrimEnd('\n');
        }

        private LocaleStatus GetKitStatusFromLocales(KitLocale[] kitLocales)
        {
            var statusArray = kitLocales.Where(s => s.Exclude == false).Select(kl => kl.StatusId);

            if (statusArray.Count() == 0 || statusArray.Any(status => status == (int)LocaleStatus.Building))
            {
                return LocaleStatus.Building;
            }
            else if (statusArray.Any(status => status == (int)LocaleStatus.Modifying))
            {
                return LocaleStatus.Modifying;
            }
            else if (statusArray.Any(status => status == (int)LocaleStatus.ReadytoPublish))
            {
                return LocaleStatus.ReadytoPublish;
            }
            else if (statusArray.All(status => status == (int)LocaleStatus.PublishQueued))
            {
                return LocaleStatus.PublishQueued;
            }
            else if (statusArray.All(status => status == (int)LocaleStatus.PublishFailed))
            {
                return LocaleStatus.PublishFailed;
            }
            else if (statusArray.Any(status => status == (int)LocaleStatus.PartiallyPublished))
            {
                return LocaleStatus.PartiallyPublished;
            }

            else if (statusArray.All(status => status == (int)LocaleStatus.Disabled))
            {
                return LocaleStatus.Disabled;
            }
            else if (statusArray.All(status => status == (int)LocaleStatus.Published))
            {
                return LocaleStatus.Published;
            }
            else
            {
                return 0;
            }
        }
    }
}