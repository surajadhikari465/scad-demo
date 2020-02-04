using Icon.Common.DataAccess;
using Icon.Logging;
using IconWebApi.DataAccess.Models;
using IconWebApi.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace IconWebApi.Controllers
{
    [Route("api/Hierarchy")]
    public class HierarchyController : ApiController
    {
        private IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>> getContactsByHierarchyClassIdsQueryHandler;
        private ILogger logger;
        private const string NOCONTACTSFOUNDERRORMESSAGE = "No contacts found.";
        private const string INVALIDHIERARCHYID = "No Brands found matching this hierarchy class ID.";

        public HierarchyController(IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>> getContactsByHierarchyClassIdsQueryHandler,
            ILogger logger)
        {
            this.getContactsByHierarchyClassIdsQueryHandler = getContactsByHierarchyClassIdsQueryHandler;
            this.logger = logger;
        }

        [HttpPost]
        [Route("getContactsForBrands")]
        public IHttpActionResult Contacts([FromBody] List<int> hierarchyClassIds)
        {
            if (hierarchyClassIds == null) return BadRequest("HierarchyClassIds are requried.");

            try
            {
                var contacts = getContactsByHierarchyClassIdsQueryHandler.Search(new GetContactsByHierarchyClassIdsQuery()
                {
                    HierarchyClassIds = hierarchyClassIds
                }).ToList();

                contacts.Where(c => c.ContactName == null).ToList().ForEach(c => c.ErrorMessage = NOCONTACTSFOUNDERRORMESSAGE);

                var invalidHierarchyIds = hierarchyClassIds.Where(p => !contacts.Any(p2 => p2.HierarchyClassId == p));

                foreach (int hierarchyId in invalidHierarchyIds)
                {
                    contacts.Add(new AssociatedContact
                    {
                        HierarchyClassId = hierarchyId,
                        ErrorMessage = INVALIDHIERARCHYID
                    });
                }
                return Json(contacts);
            }

            catch (Exception e)
            {
                this.logger.Error("Error performing contacts http Get request. " + e.Message);
                return InternalServerError(new Exception(
                    "There was an error retrieving contacts from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
            }
        }
    }
}