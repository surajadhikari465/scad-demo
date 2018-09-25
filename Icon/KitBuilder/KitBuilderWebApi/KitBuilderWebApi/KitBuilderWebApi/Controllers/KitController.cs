using AutoMapper;
using KitBuilderWebApi.Common;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.Rest;
using KitInstructionList = KitBuilderWebApi.DatabaseModels.KitInstructionList;
using KitBuilderWebApi.DataAccess.Enums;

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
        private IRepository<KitLinkGroupLocale> kitlinkGroupLocaleRepository;
        private ILogger<KitController> logger;
        private IRepository<KitLinkGroupItem> kitLinkGroupItemRepository;
        private IRepository<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRepository;
        private IRepository<LocaleType> localeTypeRepository;
        private IHelper<KitDto, KitSearchParameters> kitHelper;

        public KitController(IRepository<LinkGroup> linkGroupRepository,                   
                             IRepository<Kit> kitRepository,
                             IRepository<Locale> localeRepository,
                             IRepository<KitLocale> kitLocaleRepository,
                             IRepository<LinkGroupItem> linkGroupItemRepository,
                             IRepository<Items> itemsRepository,
                             IRepository<KitLinkGroup> kitLinkGroupRepository,
                             IRepository<KitLinkGroupLocale> kitlinkGroupLocaleRepository,
                             IRepository<KitLinkGroupItem> kitLinkGroupItemRepository,
                             IRepository<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRepository,
                             IRepository<LocaleType> localeTypeRepository,
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
            this.kitlinkGroupLocaleRepository = kitlinkGroupLocaleRepository;
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
                                   select new KitDto()
                                   {
                                       Description = k.Description,
                                       InsertDate = k.InsertDate,
                                       InstructionListId = k.InstructionListId,
                                       ItemId = k.ItemId,
                                       KitId = k.KitId
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

		[HttpGet(Name = "GetKitLocale")]
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
                 [FromBody] List<KitLocaleDto> kitLocaleDtoList)
        {
            if (kitLocaleDtoList == null)
            {
                logger.LogWarning("The object passed is either null or does not contain any rows.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<KitLocale> kitLocaleDbList = kitLocaleRepository.GetAll().Where(kl => kl.KitId == kitLocaleDtoList.FirstOrDefault().KitId).ToList();

            List<KitLocale> kitLocaleListPassed = new List<KitLocale>();
            foreach (var kitLocaleDto in kitLocaleDtoList)
            {
                var kitLocale = Mapper.Map<KitLocale>(kitLocaleDto);
                kitLocaleListPassed.Add(kitLocale);
            }

            // delete records that exist in db but not in list. Cascade delete is enabled so child records will be deleted.
            var kitLocaleRecordsToDelete = kitLocaleDbList.Where(t => !kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));

            // records that are in list passed but not in database
            var kitLocaleRecordsToAdd = kitLocaleListPassed.Where(t => !kitLocaleDbList.Select(l => l.LocaleId).Contains(t.LocaleId));

            var kitLocaleRecordsToUpdate = kitLocaleDbList.Where(t => kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));

            kitLocaleRepository.UnitOfWork.Context.KitLocale.RemoveRange(kitLocaleRecordsToDelete);
            kitLocaleRepository.UnitOfWork.Context.KitLocale.AddRange(kitLocaleRecordsToAdd);

            foreach (KitLocale kitToUpdate in kitLocaleRecordsToUpdate)
            {
                KitLocale currentKit = kitLocaleDbList.Where(kl => kl.KitLocaleId == kitToUpdate.KitLocaleId).FirstOrDefault();
                currentKit.Exclude = kitToUpdate.Exclude;
                currentKit.LastUpdatedDate = DateTime.Today;
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

            var existingKitLinkGroupLocals = from klgl in kitlinkGroupLocaleRepository.GetAll()
                                             join klg in kitLinkGroupRepository.GetAll() on klgl.KitLinkGroupId equals klg.KitLinkGroupId
                                             where klg.KitId == kitPropertiesDto.KitId && klgl.KitLocaleId == kitPropertiesDto.KitLocaleId
                                             select klgl;

            var existingKitLinkGroupItemLocals = from klgl in kitLinkGroupItemLocaleRepository.GetAll()
                                                 join klg in kitLinkGroupItemRepository.GetAll() on klgl.KitLinkGroupItemId equals klg.KitLinkGroupItemId
                                                 where klg.KitId == kitPropertiesDto.KitId && klgl.KitLocaleId == kitPropertiesDto.KitLocaleId
                                                 select klgl;

            var kitLinkGroupLocaleRecordsToAdd = kitPropertiesDto.KitLinkGroupLocaleList.Where(t => !existingKitLinkGroupLocals.Select(l => l.KitLinkGroupId).Contains(t.LinkGroupId));

            var kitLinkGroupLocaleRecordsToUpdate = existingKitLinkGroupLocals.Where(t => kitPropertiesDto.KitLinkGroupLocaleList.Select(l => l.LinkGroupId).Contains(t.KitLinkGroupId));

            IEnumerable<KitLinkGroupLocale> KitLinkGroupLocales = ConvertPropertiesToLinkGroupLocale(kitLinkGroupLocaleRecordsToAdd, kitPropertiesDto.KitLocaleId);

            kitlinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupLocale.AddRange(KitLinkGroupLocales);

            UpdateKitLinkGroupLocale(kitLinkGroupLocaleRecordsToUpdate, kitPropertiesDto);

            var kitLinkGroupItemLocaleRecordsToAdd = kitPropertiesDto.KitLinkGroupItemLocaleList.Where(t => !existingKitLinkGroupItemLocals.Select(l => l.KitLinkGroupItemId).Contains(t.LinkGroupItemId));

            var kitLinkGroupItemLocaleRecordsToUpdate = existingKitLinkGroupItemLocals.Where(t => kitPropertiesDto.KitLinkGroupItemLocaleList.Select(l => l.LinkGroupItemId).Contains(t.KitLinkGroupItemId));

            IEnumerable<KitLinkGroupItemLocale> KitLinkGroupItemLocales = ConvertPropertiesToLinkGroupItemLocale(kitLinkGroupItemLocaleRecordsToAdd, kitPropertiesDto.KitLocaleId);

            kitlinkGroupLocaleRepository.UnitOfWork.Context.KitLinkGroupItemLocale.AddRange(KitLinkGroupItemLocales);

            UpdateKitLinkGroupItemLocale(kitLinkGroupItemLocaleRecordsToUpdate, kitPropertiesDto);

            try
            {
                kitlinkGroupLocaleRepository.UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost(Name = "SaveKit")]
        public IActionResult KitSaveDetails([FromBody]KitSaveParameters kitSaveParameters)
        {
            var errorMessage = string.Empty;

            try
            {
                ValidateKitSaveParameters(kitSaveParameters);

                var kit = kitRepository.GetAll().Where(k => k.KitId == kitSaveParameters.KitId).FirstOrDefault();
                var item = itemsRepository.GetAll().Where(i => i.ItemId == kitSaveParameters.KitItem).FirstOrDefault();

                var existingKitInstructionLists =
                    kitInstructionListRepository.GetAll().Where(kil => kil.KitId == kit.KitId);
                var newKitInstructionLists = from id in kitSaveParameters.InstructionListIds
                                             select new KitInstructionList()
                                             {
                                                 InstructionListId = id,
                                                 KitId = kitSaveParameters.KitId
                                             };

                var existingKitLinkGroups =
                    kitLinkGroupRepository.GetAll().Where(klg => klg.KitId == kitSaveParameters.KitId);
                var newKitLinkGroups = from id in kitSaveParameters.LinkGroupIds
                                       select new KitLinkGroup()
                                       {
                                           KitId = kitSaveParameters.KitId,
                                           KitLinkGroupId = id
                                       };

                var existingKitLinkGroupItems =
                    kitLinkGroupItemRepository.GetAll().Where(kli => kli.KitId == kitSaveParameters.KitId);
                var newKitLinkGroupItems =
                    from id in kitSaveParameters.LinkGroupItemIds
                    select new KitLinkGroupItem()
                    {
                        KitId = kitSaveParameters.KitId,
                        KitLinkGroupItemId = id
                    };

                if (kit == null)
                {
                    // error.
                    errorMessage = $"SaveKit: Unable to find Kit [id: {kitSaveParameters.KitId}]";
                    logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                if (item == null)
                {
                    // error
                    errorMessage = $"SaveKit: Unable to find Item [id: {kitSaveParameters.KitItem}]";
                    logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                kit.Description = kitSaveParameters.KitDescription;
                kit.ItemId = kitSaveParameters.KitItem;

                kitRepository.UnitOfWork.Context.KitInstructionList.RemoveRange(existingKitInstructionLists);
                kitRepository.UnitOfWork.Context.KitInstructionList.AddRange(newKitInstructionLists);

                kitRepository.UnitOfWork.Context.KitLinkGroupItem.RemoveRange(existingKitLinkGroupItems);
                kitRepository.UnitOfWork.Context.KitLinkGroupItem.AddRange(newKitLinkGroupItems);

                kitRepository.UnitOfWork.Context.KitLinkGroup.RemoveRange(existingKitLinkGroups);
                kitRepository.UnitOfWork.Context.KitLinkGroup.AddRange(newKitLinkGroups);

                kitRepository.UnitOfWork.Commit();

                return Ok();
            }
            catch (DbUpdateConcurrencyException DbConcurrencyEx)
            {
                logger.LogError($"SaveKit: Concurrency Error Saving Kit. [id: {kitSaveParameters.KitId}]");
                logger.LogError(DbConcurrencyEx.Message);
                return BadRequest();

            }
            catch (DbUpdateException DbUpdateException)
            {
                logger.LogError($"SaveKit: Database Update Error Saving Kit. [id: {kitSaveParameters.KitId}]");
                logger.LogError(DbUpdateException.Message);
                return BadRequest();
            }
            catch (Exception Ex)
            {
                logger.LogError($"SaveKit: Error Saving Kit. [id: {kitSaveParameters.KitId}]");
                logger.LogError(Ex.Message);
                return BadRequest(Ex.Message);
            }
        }

        internal void UpdateKitLinkGroupItemLocale(IQueryable<KitLinkGroupItemLocale> kitLinkGroupItemLocaleRecordsToUpdate, KitPropertiesDto kitPropertiesDto)
        {
            foreach (KitLinkGroupItemLocale kitLinkGroupItemLocale in kitLinkGroupItemLocaleRecordsToUpdate)
            {
                var kitLinkGroupItemLocalePropertiesDto = kitPropertiesDto.KitLinkGroupLocaleList.Where(s => s.LinkGroupItemId == kitLinkGroupItemLocale.KitLinkGroupItemId).FirstOrDefault();
                kitLinkGroupItemLocale.Properties = kitLinkGroupItemLocalePropertiesDto.Properties;
                kitLinkGroupItemLocale.Exclude = kitLinkGroupItemLocalePropertiesDto.Excluded;
                kitLinkGroupItemLocale.DisplaySequence = (int)kitLinkGroupItemLocalePropertiesDto.DisplaySequence;
                kitLinkGroupItemLocale.LastModifiedDate = DateTime.Now;
                kitLinkGroupItemLocale.LastModifiedBy = kitLinkGroupItemLocalePropertiesDto.LastModifiedBy;
            }
        }

        internal void UpdateKitLinkGroupLocale(IQueryable<KitLinkGroupLocale> kitLinkGroupLocaleRecordsToUpdate, KitPropertiesDto kitPropertiesDto)
        {
            foreach (KitLinkGroupLocale kitLinkGroupLocale in kitLinkGroupLocaleRecordsToUpdate)
            {
                var KitLinkGroupLocalePropertiesDto = kitPropertiesDto.KitLinkGroupLocaleList.Where(s => s.LinkGroupId == kitLinkGroupLocale.KitLinkGroupId).FirstOrDefault();
                kitLinkGroupLocale.Properties = KitLinkGroupLocalePropertiesDto.Properties;
                kitLinkGroupLocale.Exclude = KitLinkGroupLocalePropertiesDto.Excluded;
                kitLinkGroupLocale.LastModifiedDate = DateTime.Now;
                kitLinkGroupLocale.DisplaySequence = (int)KitLinkGroupLocalePropertiesDto.DisplaySequence;
                kitLinkGroupLocale.LastModifiedBy = KitLinkGroupLocalePropertiesDto.LastModifiedBy;
            }
        }

        internal List<KitLinkGroupItemLocale> ConvertPropertiesToLinkGroupItemLocale(IEnumerable<PropertiesDto> kitLinkGroupItemLocaleRecordsToAdd, int kitLocaleId)
        {
            List<KitLinkGroupItemLocale> KitLinkGroupItemLocaleList = new List<KitLinkGroupItemLocale>();

            foreach (PropertiesDto propertiesDto in kitLinkGroupItemLocaleRecordsToAdd)
            {
                KitLinkGroupItemLocale KitLinkGroupItemLocale = new KitLinkGroupItemLocale
                {
                    KitLinkGroupItemId = propertiesDto.LinkGroupId,
                    KitLocaleId = kitLocaleId,
                    Properties = propertiesDto.Properties,
                    DisplaySequence = (int)propertiesDto.DisplaySequence,
                    Exclude = propertiesDto.Excluded,
                    LastModifiedBy = propertiesDto.LastModifiedBy
                };
                KitLinkGroupItemLocaleList.Add(KitLinkGroupItemLocale);
            }

            return KitLinkGroupItemLocaleList;
        }

        internal List<KitLinkGroupLocale> ConvertPropertiesToLinkGroupLocale(IEnumerable<PropertiesDto> kitLocaleRecordsToAdd, int kitLocaleId)
        {
            List<KitLinkGroupLocale> KitLinkGroupLocaleList = new List<KitLinkGroupLocale>();

            foreach (PropertiesDto propertiesDto in kitLocaleRecordsToAdd)
            {
                KitLinkGroupLocale KitLinkGroupLocale = new KitLinkGroupLocale
                {
                    KitLinkGroupId = propertiesDto.LinkGroupId,
                    KitLocaleId = kitLocaleId,
                    Properties = propertiesDto.Properties,
                    DisplaySequence = (int)propertiesDto.DisplaySequence,
                    MinimumCalories = propertiesDto.MaximumCalories,
                    MaximumCalories = propertiesDto.MinimumCalories,
                    Exclude = propertiesDto.Excluded,
                };
                KitLinkGroupLocaleList.Add(KitLinkGroupLocale);
            }

            return KitLinkGroupLocaleList;
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
            if (kitLocaleRepository.GetAll().Where(kl => kl.LocaleId == localeId).Any())
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
                       .Include(k => k.KitLinkGroupItem).ThenInclude(d => d.LinkGroupItem)
                       .ThenInclude(lg => lg.Item)
                       .Include(k => k.KitLinkGroupItem).ThenInclude(kgi => kgi.KitLinkGroupItemLocale)
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

            List<PropertiesDto> KitLinkGroupLocaleList = (from klg in kitLinkGroupRepository.GetAll().Where(klgr => klgr.KitId == kitId)
                                                          join lg in linkGroupRepository.GetAll() on klg.LinkGroupId equals lg.LinkGroupId
                                                          join kl in kitLocaleRepository.GetAll() on kitId equals kl.KitId
                                                          join klgl in kitlinkGroupLocaleRepository.GetAll()
                                                          on new { kl.KitLocaleId, klg.KitLinkGroupId } equals new { klgl.KitLocaleId, klgl.KitLinkGroupId } into ps
                                                          from klgl in ps.DefaultIfEmpty()
                                                          where kl.LocaleId == localeIdWithKitLocaleRecord
                                                          select new PropertiesDto
                                                          {
                                                              LinkGroupId = klg.KitLinkGroupId,
                                                              LinkGroupItemId = 0,
                                                              Name = lg.GroupName,
                                                              Properties = klgl != null ? klgl.Properties : null,
                                                              Excluded = klgl != null ? klgl.Exclude : null,
                                                              MinimumCalories = klgl != null ? klgl.MinimumCalories : null,
                                                              MaximumCalories = klgl != null ? (int?)klgl.MaximumCalories: null,
                                                              DisplaySequence = klgl != null ? (int?)klgl.DisplaySequence : null,
                                                          }).ToList();


            List<PropertiesDto> KitLinkGroupItemLocaleList = (from klg in kitLinkGroupRepository.GetAll().Where(klg => klg.KitId == kitId)
                                                              join lgi in linkGroupItemRepository.GetAll() on klg.LinkGroupId equals lgi.LinkGroupId
                                                              join i in itemsRepository.GetAll() on lgi.ItemId equals i.ItemId
                                                              join klgi in kitLinkGroupItemRepository.GetAll() on lgi.LinkGroupItemId equals klgi.LinkGroupItemId
                                                              join klr in kitLocaleRepository.GetAll() on kitId equals klr.KitId
                                                              join klgil in kitLinkGroupItemLocaleRepository.GetAll()
                                                              on new { klgi.KitLinkGroupItemId, klr.KitLocaleId } equals new { klgil.KitLinkGroupItemId, klgil.KitLocaleId } into kli
                                                              from klgil in kli.DefaultIfEmpty()

                                                              where klr.LocaleId == localeIdWithKitLocaleRecord
                                                              select new PropertiesDto
                                                              {
                                                                  LinkGroupId = klg.KitLinkGroupId,
                                                                  LinkGroupItemId = klgi.KitLinkGroupItemId,
                                                                  Name = i.ProductDesc,
                                                                  Properties = klgil != null ? klgil.Properties : null,
                                                                  Excluded = klgil != null ? klgil.Exclude : null,
                                                                  DisplaySequence = klgil != null ? (int?)klgil.DisplaySequence: null,
                                                              }).ToList();

            KitPropertiesDto.KitLinkGroupLocaleList = KitLinkGroupLocaleList;
            KitPropertiesDto.KitLinkGroupItemLocaleList = KitLinkGroupItemLocaleList;

            return KitPropertiesDto;
        }

        internal void BuildQueryToFilterKitData(KitSearchParameters kitSearchParameters,
            ref IQueryable<KitDto> kitsBeforePaging)
        {

            if (!string.IsNullOrEmpty(kitSearchParameters.ItemScanCode))
            {
                var scancodeForWhereClause = kitSearchParameters.ItemScanCode.Trim().ToLower();
                kitsBeforePaging = from k in kitsBeforePaging
                                   join i in itemsRepository.GetAll() on k.ItemId equals i.ItemId
                                   where i.ScanCode == scancodeForWhereClause
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

        internal void ValidateKitSaveParameters(KitSaveParameters parameters)
        {
            var errorFound = false;
            var message = string.Empty;

            if (parameters.InstructionListIds == null)
            {
                errorFound = true;
                message = "InstructionListIds cannot be null";
            }

            if (parameters.LinkGroupIds == null)
            {
                errorFound = true;
                message = "LinkGroupIds cannot be null";
            }

            if (parameters.LinkGroupItemIds == null)
            {
                errorFound = true;
                message = "LinkGroupItemIds cannot be null";
            }

            if (errorFound) throw new ValidationException(message);
        }
    }
}