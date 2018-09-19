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


namespace KitBuilderWebApi.Controllers
{




    [Route("api/Kits")]
    public class KitController : Controller
    {
        private IRepository<LinkGroup> linkGroupRepository;
        private IRepository<Kit> kitRepository;
        private IRepository<KitLinkGroup> kitLinkGroupRepository;
        private IRepository<KitLocale> kitLocaleRepository;
        private IRepository<LinkGroupItem> linkGroupItemRepository;
        private IRepository<Items> itemsRepository;
        private IRepository<KitInstructionList> kitInstructionListRepository;
        private IRepository<KitLinkGroupItem> kitLinkGroupItemRepository;
        private IHelper<KitDto, KitSearchParameters> kitHelper;
        private readonly ILogger<LinkGroupController> logger;

        public KitController(IRepository<Kit> kitRepository,
                             IRepository<KitInstructionList> kitInstructionListRepository,
                             IRepository<KitLocale> kitLocaleRepository,
                             IRepository<LinkGroup> linkGroupRepository,
                             IRepository<LinkGroupItem> linkGroupItemRepository,
                             IRepository<Items> itemsRepository,
                             IRepository<KitLinkGroup> kitLinkGroupRepository,
                             IRepository<KitLinkGroupItem> kitLinkGroupItemRepository,
                             ILogger<LinkGroupController> logger,
                             IHelper<KitDto, KitSearchParameters> kitHelper
                            )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.kitInstructionListRepository = kitInstructionListRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.kitLinkGroupRepository = kitLinkGroupRepository;
            this.kitLocaleRepository = kitLocaleRepository;
            this.kitLinkGroupItemRepository = kitLinkGroupItemRepository;
            this.kitRepository = kitRepository;
            this.kitHelper = kitHelper;
            this.logger = logger;
        }


        [HttpGet(Name="GetKits")]
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

            //todo: which description(s) should be filtered on? CustomerFriendlyDesc or, ProductDesc, or KitchenDesc, or all?
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

            kitsBeforePaging.WriteDebugSql(" Kits ");
        }

        [HttpPost(Name="SaveKit")]
        public IActionResult KitSaveDetails([FromBody]KitSaveParameters kitSaveParameters)
        {
            var errorMessage = string.Empty;

            try
            {

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
                return BadRequest();
            }

        }

        [HttpPost("AssignLocations")]
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

            List<KitLocale> kitLocaleDbList = kitLocaleRepository.GetAll().Where(kl => kl.KitId == kitLocaleDtoList.FirstOrDefault().KitId).ToList() ;

            List<KitLocale> kitLocaleListPassed = new List<KitLocale>();
            foreach (var kitLocaleDto in kitLocaleDtoList)
            {
                var kitLocale = Mapper.Map<KitLocale>(kitLocaleDto);
                kitLocaleListPassed.Add(kitLocale);
            }

            // delete records that exist in db but not in list. Cascade delete is enabled so child records will be deleted.
            var kitLocaleRecordsToDelete = kitLocaleDbList.Where(t => !kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));
            // records that are in list passed but not in database
            var kitLocaleRecordsToAdd = kitLocaleListPassed.Where(t => !kitLocaleDtoList.Select(l => l.LocaleId).Contains(t.LocaleId));

            var kitLocaleRecordsToUpdate = kitLocaleDbList.Where(t => kitLocaleListPassed.Select(l => l.LocaleId).Contains(t.LocaleId));

            kitLocaleRepository.UnitOfWork.Context.KitLocale.RemoveRange(kitLocaleRecordsToDelete);
            kitLocaleRepository.UnitOfWork.Context.KitLocale.AddRange(kitLocaleRecordsToAdd);

            foreach(KitLocale kitToUpdate in kitLocaleRecordsToUpdate)
            {
                KitLocale currentKit = kitLocaleDbList.Where(kl => kl.KitLocaleId == kitToUpdate.KitLocaleId).FirstOrDefault();
                currentKit.Exclude = kitToUpdate.Exclude;
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
    }
}