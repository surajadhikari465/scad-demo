using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand>
    {
        private IconContext context;

        public UpdateBrandCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateBrandCommand data)
        {
            var brandToUpdate = context.HierarchyClass.Single(hc => hc.hierarchyClassID == data.Brand.hierarchyClassID);
            string currentBrandName = brandToUpdate.hierarchyClassName;

            UpdateBrandName(brandToUpdate, data.Brand.hierarchyClassName);

            GenerateBrandUpdateEvents(data.Brand.hierarchyClassID, data.Brand.hierarchyClassName);

            UpdateNewItemBrandNames(currentBrandName, data.Brand.hierarchyClassName);
        }

        private void UpdateNewItemBrandNames(string currentBrandName, string updatedBrandName)
        {
            if (currentBrandName != updatedBrandName)
            {
                this.context.UpdateIrmaItemBrand(currentBrandName, updatedBrandName);
            }
        }

        private void GenerateBrandUpdateEvents(int brandClassId, string brandName)
        {
            string[] brandUpdateConfiguredRegions = ConfigurationManager.AppSettings["BrandNameUpdateEventConfiguredRegions"].Split(',');
            var brandEvents = new List<EventQueue>();

            int brandNameUpdateEventId = context.EventType.Single(et => et.EventName == "Brand Name Update").EventId;

            foreach (string region in brandUpdateConfiguredRegions)
            {
                var brandEvent = new EventQueue();

                brandEvent.EventId = brandNameUpdateEventId;
                brandEvent.EventMessage = brandName;
                brandEvent.EventReferenceId = brandClassId;
                brandEvent.RegionCode = region.Trim();
                brandEvent.InsertDate = DateTime.Now;

                brandEvents.Add(brandEvent);
            }

            context.EventQueue.AddRange(brandEvents);
            context.SaveChanges();
        }

        private void UpdateBrandName(HierarchyClass brandToUpdate, string updatedBrandName)
        {
            if (brandToUpdate.hierarchyClassName != updatedBrandName)
            {
                bool duplicateBrandNameExists = context.HierarchyClass.ContainsDuplicateBrandName(updatedBrandName);

                if (duplicateBrandNameExists)
                {
                    throw new DuplicateValueException(String.Format("The brand {0} already exists.", updatedBrandName));
                }

                if (updatedBrandName.Length >= Constants.IrmaBrandNameMaxLength)
                {
                    string trimmedBrandName = updatedBrandName.Substring(0, Constants.IrmaBrandNameMaxLength);

                    bool duplicateTrimmedBrandNameExists = context.HierarchyClass.ContainsDuplicateTrimmedBrandName(trimmedBrandName);

                    if (duplicateTrimmedBrandNameExists)
                    {
                        throw new DuplicateValueException(String.Format("This brand trimmed to {0} characters {1} already exists. " +
                            "Change the brand name so that the first {0} characters are unique.", Constants.IrmaBrandNameMaxLength, updatedBrandName.Substring(0, Constants.IrmaBrandNameMaxLength)));
                    }
                }
            }

            brandToUpdate.hierarchyClassName = updatedBrandName;

            context.SaveChanges();
        }
    }
}
