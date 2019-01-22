using AutoMapper;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Enums;
using KitBuilder.DataAccess.Repository;
using KitBuilderWebApi.Helper;
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
using KitInstructionList = KitBuilder.DataAccess.DatabaseModels.KitInstructionList;
using  LocaltypeModel = KitBuilder.DataAccess.DatabaseModels.LocaleType;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Kits")]
    public class KitController : Controller
    {
        private IRepository<KitInstructionList> kitInstructionListRepository;
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<Kit> kitRepository;
        private IRepository<Locale> localeRepository;
        private IRepository<KitLocale> kitLocaleRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<KitLinkGroup> kitLinkGroupRepository;
        private IRepository<KitLinkGroupLocale> kitLinkGroupLocaleRepository;
        private ILogger<KitController> logger;
        private IRepository<KitLinkGroupItem> kitLinkGroupItemRepository;
        private IRepository<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRepository;
        private IRepository<LocaltypeModel> localeTypeRepository;
        private IHelper<KitDto, KitSearchParameters> kitHelper;
        private const string deleteKitSpName = "DeleteKitByKitId";

        public KitController(IRepository<LinkGroup> linkGroupRepository,                   
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
                             IHelper<KitDto, KitSearchParameters> kitHelper
                            )
        {
            this.linkGroupRepository = linkGroupRepository;
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
        }


        // GET api/kits/1/ViewKit/1 GetKitByLocaleId
        [HttpGet("{kitId}/ViewKit/{localeId}", Name = "ViewKit")]
        public IActionResult GetKitByLocaleId(int kitId, int localeId, bool loadChildObjects)
        {
            int? localeIdWithKitLocaleRecord = getlocaleIdAtWhichkitRecordExits(kitId, localeId);

            if (localeIdWithKitLocaleRecord == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
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

        [HttpGet("{kitId}",Name = "GetKitByKitId")]
        public IActionResult GetKitbyKitId(int kitId)
        {
           var kit =  kitRepository.GetAll().Where(k => k.KitId == kitId).FirstOrDefault();
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
                                   select new KitDto()
                                   {
                                       Description = k.Description,
                                       InsertDateUtc = k.InsertDateUtc,
									   LastUpdatedDateUtc = k.LastUpdatedDateUtc,
                                       ItemId = k.ItemId,
                                       KitId = k.KitId,
                                       Item = new ItemsDto
                                       {
                                           ScanCode = i.ScanCode,
                                           ProductDesc = i.ProductDesc,
                                           CustomerFriendlyDesc = i.CustomerFriendlyDesc
                                       }
                                   };

            BuildQueryToFilterKitData(kitSearchParameters, ref kitsBeforePaging);

            var kitsAfterPaging = PagedList<KitDto>.Create(kitsBeforePaging,
                kitSearchParameters.PageNumber,
                kitSearchParameters.PageSize
            );

            var paginationMetadata = kitHelper.GetPaginationData(kitsAfterPaging, kitSearchParameters);

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(kitsAfterPaging.ShapeData(kitSearchParameters.Fields));
        }

		[HttpGet("{kitId}/GetKitLocale", Name="GetKitLocale")]
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

			var passedInKitLinkGroupItemLocales = kitPropertiesDto.KitLinkGroupLocaleList.SelectMany(x => x.KitLinkGroupItemLocaleList);

			var existingKitLinkGroupLocals = from klgl in kitLinkGroupLocaleRepository.GetAll()
											 join klg in kitPropertiesDto.KitLinkGroupLocaleList on klgl.KitLocaleId equals klg.KitLocaleId
											 select klgl;

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

        [HttpPost(Name = "SaveKit")]
        public IActionResult KitSaveDetails([FromBody]KitDto kitToSave)
        {
            var errorMessage = string.Empty;

            try
            {
                ValidateKitSaveParameters(kitToSave);

                var item = itemsRepository.GetAll().Where(i => i.ItemId == kitToSave.ItemId).FirstOrDefault();

				if (item == null)
				{
					// error
					errorMessage = $"SaveKit: Unable to find Item [id: {kitToSave.ItemId}]";
					logger.LogError(errorMessage);
					throw new Exception(errorMessage);
				}

				if (kitToSave.KitId <= 0) //brand new kit
				{
					var newKit = Mapper.Map<Kit>(kitToSave);
                    newKit.LastUpdatedDateUtc = DateTime.UtcNow;
                    newKit.InsertDateUtc=DateTime.UtcNow;

					kitLocaleRepository.UnitOfWork.Context.Kit.Add(newKit);
				}
				else
				{
					var kit = kitRepository.GetAll().Where(k => k.KitId == kitToSave.KitId).FirstOrDefault();

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
												 .Where(i => !existingKitInstructionLists.Select(l => l.KitInstructionListId).Contains(i.KitInstructionListId))
												 select new KitInstructionList()
												 {
													 InstructionListId = id.InstructionListId,
													 KitId = kitToSave.KitId
												 };

					var kitInstructionListsToRemove = existingKitInstructionLists.Where(i => !kitToSave.KitInstructionList.Select(l => l.KitInstructionListId).Contains(i.KitInstructionListId));

					var existingKitLinkGroups =
						kitLinkGroupRepository.GetAll().Where(klg => klg.KitId == kitToSave.KitId);
					var newKitLinkGroups = from lg in kitToSave.KitLinkGroup
										   .Where(lg => lg.KitLinkGroupId <= 0)
										   select new KitLinkGroup()
										   {
											   KitId = kitToSave.KitId,
											   LinkGroupId = lg.LinkGroupId,
											   KitLinkGroupItem = ConvertToLinkGroupItemList(kitToSave.KitLinkGroup.Where(l => l.LinkGroupId == lg.LinkGroupId).SelectMany(i => i.KitLinkGroupItem)),
                                               LastUpdatedDateUtc = DateTime.UtcNow,
                                               InsertDateUtc = DateTime.UtcNow
										   };
					var KitLinkGroupsToRemove = existingKitLinkGroups.Where(i => !kitToSave.KitLinkGroup.Select(k => k.LinkGroupId).Contains(i.LinkGroupId));

					var existingKitLinkGroupItems = from klgi in kitLinkGroupItemRepository.GetAll()
													join klg in kitLinkGroupRepository.GetAll() on klgi.KitLinkGroupId equals klg.KitLinkGroupId
													where klg.KitId == kitToSave.KitId
													select klgi;


					var newKitLinkGroupItemDtos = kitToSave.KitLinkGroup.SelectMany(li => li.KitLinkGroupItem)
						.Where(li => li.KitLinkGroupItemId <= 0 && li.KitLinkGroupId > 0);

					List<KitLinkGroupItem> newKitLinkGroupItems = new List<KitLinkGroupItem>();
					foreach(KitLinkGroupItemDto newLinkGroupItemDto in newKitLinkGroupItemDtos)
					{
					    var linkGroupItem = Mapper.Map<KitLinkGroupItem>(newLinkGroupItemDto);
                        linkGroupItem.LastUpdatedDateUtc = DateTime.UtcNow;
					    linkGroupItem.InsertDateUtc = DateTime.UtcNow;

						newKitLinkGroupItems.Add(Mapper.Map<KitLinkGroupItem>(newLinkGroupItemDto));
					}

					var KitLinkGroupItemsToRemove = from e in existingKitLinkGroupItems
													join i in kitToSave.KitLinkGroup.SelectMany(li => li.KitLinkGroupItem)
														 on e.KitLinkGroupItemId equals i.KitLinkGroupItemId into ps
													from sub in ps.DefaultIfEmpty()
													where sub == null
													select new KitLinkGroupItem
													{
														KitLinkGroupItemId = e.KitLinkGroupItemId
													};
													  

					kit.Description = kitToSave.Description;
					kit.ItemId = kitToSave.ItemId;
					kit.LastUpdatedDateUtc = DateTime.UtcNow;

					kitRepository.UnitOfWork.Context.KitInstructionList.RemoveRange(kitInstructionListsToRemove);
					kitRepository.UnitOfWork.Context.KitInstructionList.AddRange(newKitInstructionLists);

					kitRepository.UnitOfWork.Context.KitLinkGroupItem.RemoveRange(KitLinkGroupItemsToRemove);
					kitRepository.UnitOfWork.Context.KitLinkGroupItem.AddRange(newKitLinkGroupItems);

					kitRepository.UnitOfWork.Context.KitLinkGroup.RemoveRange(KitLinkGroupsToRemove);
					kitRepository.UnitOfWork.Context.KitLinkGroup.AddRange(newKitLinkGroups);			
				}
                kitRepository.UnitOfWork.Commit();

                return Ok();
            }
            catch (DbUpdateConcurrencyException DbConcurrencyEx)
            {
                logger.LogError($"SaveKit: Concurrency Error Saving Kit. [id: {kitToSave.KitId}]");
                logger.LogError(DbConcurrencyEx.Message);
                return BadRequest();

            }
            catch (DbUpdateException DbUpdateException)
            {
                logger.LogError($"SaveKit: Database Update Error Saving Kit. [id: {kitToSave.KitId}]");
                logger.LogError(DbUpdateException.Message);
                return BadRequest();
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

            if(locale == null)
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
                                        KitLocaleId = klr.KitLocaleId
                                    }).FirstOrDefault();

            if (KitPropertiesDto == null)
            {
                return null;
            }

            List<KitLinkGroupPropertiesDto> KitLinkGroupLocaleList = (from klg in kitLinkGroupRepository.GetAll().Where(klgr => klgr.KitId == kitId)
                                                          join lg in linkGroupRepository.GetAll() on klg.LinkGroupId equals lg.LinkGroupId
                                                          join kl in kitLocaleRepository.GetAll() on kitId equals kl.KitId
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
                                                              MaximumCalories = klgl != null ? (int?)klgl.MaximumCalories: null,
                                                              DisplaySequence = klgl != null ? (int?)klgl.DisplaySequence : null,
															  KitLinkGroupItemLocaleList = new HashSet<PropertiesDto>(),
														  }).ToList();


			List<PropertiesDto> KitLinkGroupItemLocaleList = (from klgl in KitLinkGroupLocaleList
															  join klg in kitLinkGroupRepository.GetAll() on klgl.KitLinkGroupId equals klg.KitLinkGroupId
															  join klgi in kitLinkGroupItemRepository.GetAll() on klgl.KitLinkGroupId equals klgi.KitLinkGroupId
															  join lgi in linkGroupItemRepository.GetAll()
															  on new { klg.LinkGroupId, klgi.LinkGroupItemId } equals new { lgi.LinkGroupId, lgi.LinkGroupItemId }
															  join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
															  join klgil in kitLinkGroupItemLocaleRepository.GetAll()
															  on new { klgi.KitLinkGroupItemId, klgl.KitLinkGroupLocaleId } equals new { klgil.KitLinkGroupItemId, klgil.KitLinkGroupLocaleId } into kli
															  from klgil in kli.DefaultIfEmpty()
															  select new PropertiesDto
															  {
																  KitLinkGroupLocaleId = klgil != null ? klgil.KitLinkGroupLocaleId : 0,
																  KitLinkGroupItemLocaleId = klgil != null ? klgil.KitLinkGroupItemLocaleId : 0,
																  KitLinkGroupId = klg.KitLinkGroupId,
																  KitLinkGroupItemId = klgi.KitLinkGroupItemId,
																  Name = i.ProductDesc,
																  Properties = klgil != null ? klgil.Properties : null,
																  Excluded = klgil != null ? klgil.Exclude : null,
																  DisplaySequence = klgil != null ? (int?)klgil.DisplaySequence : null,
															  }).ToList();

			foreach (KitLinkGroupPropertiesDto kitLinkGroupLocale in KitLinkGroupLocaleList)
			{
				List<PropertiesDto> kitLinkGroupItemLocales = KitLinkGroupItemLocaleList.Where(i => i.KitLinkGroupLocaleId == kitLinkGroupLocale.KitLinkGroupLocaleId).ToList();
				kitLinkGroupLocale.KitLinkGroupItemLocaleList = kitLinkGroupItemLocales;
			};
			KitPropertiesDto.KitLinkGroupLocaleList = KitLinkGroupLocaleList;
            //KitPropertiesDto.KitLinkGroupItemLocaleList = KitLinkGroupItemLocaleList;




            return KitPropertiesDto;
        }

        internal void BuildQueryToFilterKitData(KitSearchParameters kitSearchParameters,
            ref IQueryable<KitDto> kitsBeforePaging)
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
                    linkGroupRepository.ExecWithStoreProcedure(deleteKitSpName + " @kitId", param1);

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
    }
}