using System.Linq;
using AutoMapper;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using KitBuilderWebApi.QueryParameters;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using KitBuilderWebApi.DataAccess.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        private IRepository<KitLinkGroup> kitlinkGroupRepository;
        private ILogger<LinkGroupController> logger;

        public KitController(IRepository<LinkGroup> linkGroupRepository,
                             IRepository<LinkGroupItem> linkGroupItemRepository,
                             IRepository<Items> itemsRepository,
                             IRepository<KitLinkGroup> kitlinkGroupRepository,
                             ILogger<LinkGroupController> logger,
                             IHelper<LinkGroupDto, LinkGroupParameters> kitHelper
                            )
        {
            this.linkGroupRepository = linkGroupRepository;
            this.linkGroupItemRepository = linkGroupItemRepository;
            this.itemsRepository = itemsRepository;
            this.kitlinkGroupRepository = kitlinkGroupRepository;
            this.logger = logger;
        }


        [HttpPost()]
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

            List<KitLocale> kitLocalePassedList = new List<KitLocale>();
            foreach (var kitLocaleDto in kitLocaleDtoList)
            {
                var kitLocale = Mapper.Map<KitLocale>(kitLocaleDto);
                kitLocalePassedList.Add(kitLocale);
            }

            // delete records that exist in db but not in list. Cascade delete is enabled so child records will be deleted.
            var kitLocaleRecordsToDelete = kitLocaleDbList.Where(t => !kitLocaleDbList.Select(l => l.LocaleId).Contains(t.LocaleId));
            // records that are in list passed but not in database
            var kitLocaleRecordsToAdd = kitLocalePassedList.Where(t => !kitLocalePassedList.Select(l => l.LocaleId).Contains(t.LocaleId));

            var kitLocaleRecordsToUpdate = kitLocaleDbList.Where(t => kitLocaleDbList.Select(l => l.LocaleId).Contains(t.LocaleId));

            kitLocaleRepository.UnitOfWork.Context.KitLocale.RemoveRange(kitLocaleRecordsToDelete);
            kitLocaleRepository.UnitOfWork.Context.KitLocale.AddRange(kitLocaleRecordsToAdd);

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