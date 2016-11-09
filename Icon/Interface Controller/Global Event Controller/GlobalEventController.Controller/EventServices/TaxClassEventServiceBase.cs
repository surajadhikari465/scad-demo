using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventServices
{
    public abstract class TaxClassEventServiceBase : IEventService
    {
        private IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassHandler;

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }

        public TaxClassEventServiceBase(IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassHandler)
        {
            this.getHierarchyClassHandler = getHierarchyClassHandler;
        }

        public abstract void Run();

        /// <summary>
        /// Verifies that the EventQueue information is valid
        /// </summary>
        public void VerifyEventInformation()
        {
            if ((ReferenceId == null || ReferenceId < 1) || String.IsNullOrEmpty(Message) || String.IsNullOrEmpty(Region))
            {
                string message = String.Format("IconToIrmaTaxClassUpdateEventHandler was called with invalid arguments.  ReferenceId must be greater than 0. " +
                    "Region and Message must not be null or empty. ReferenceId = {0}, Message = {1}, Region = {2}", ReferenceId, Message, Region);

                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Gets the Tax Abbreviation from the HierarchyClassTrait of the Tax HierarchyClassId supplied in the EventQueue record
        /// </summary>
        /// <param name="hierarchyClassId">ID of the Tax Class</param>
        /// <returns>Tax Abbreviation String</returns>
        public string GetTaxAbbreviation(int hierarchyClassId)
        {
            GetHierarchyClassQuery getHierarchyClass = new GetHierarchyClassQuery();
            getHierarchyClass.HierarchyClassId = ReferenceId.Value;
            HierarchyClass taxClass = getHierarchyClassHandler.Handle(getHierarchyClass);

            HierarchyClassTrait taxAbbreviationTrait = taxClass.HierarchyClassTrait
                .SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation);
            string taxAbbreviation = null;

            if (taxAbbreviationTrait == null)
            {
                throw new InvalidOperationException("The tax class doesn't have an abbreviation.");
            }
            else
            {
                taxAbbreviation = taxAbbreviationTrait.traitValue;
            }

            return taxAbbreviation;
        }
    }
}
