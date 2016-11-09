using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddUpdateLastChangeCommandHandler : ICommandHandler<AddUpdateLastChangeCommand>
    {
        private IrmaContext context;

        public AddUpdateLastChangeCommandHandler(IrmaContext context)
        {
            this.context = context;
        }

        public void Handle(AddUpdateLastChangeCommand command)
        {
            IconItemLastChange lastChange = this.context.IconItemLastChange.SingleOrDefault(lc => lc.Identifier == command.UpdatedItem.ScanCode);

            // Only Update or Add if something has changed.
            if (lastChange != null)
            {
                var entry = this.context.Entry(lastChange);

                lastChange.Item_Description = command.UpdatedItem.ProductDescription;
                lastChange.POS_Description = command.UpdatedItem.PosDescription.ToUpper();
                lastChange.Package_Desc1 = Convert.ToDecimal(command.UpdatedItem.PackageUnit);
                lastChange.Food_Stamps = Convert.ToBoolean(Convert.ToInt16(command.UpdatedItem.FoodStampEligible));
                lastChange.ScaleTare = Convert.ToDecimal(command.UpdatedItem.Tare);
                lastChange.Brand_ID = command.BrandId;
                lastChange.TaxClassID = command.TaxClassId;
                lastChange.InsertDate = DateTime.Now;
                lastChange.AreNutriFactsChanged = command.UpdatedItem.AreNutriFactsUpdated;
                lastChange.ClassID = command.ClassId;
                lastChange.Package_Unit_ID = command.PackageUnitId;
                lastChange.Package_Desc2 = Convert.ToDecimal(command.UpdatedItem.RetailSize);
                if (command.UpdatedItem.SubTeamNo != -1)
                {
                    lastChange.Subteam_No = command.UpdatedItem.SubTeamNo;
                }

                entry.State = EntityState.Modified;
            }
            else
            {
                // Add new version if it doesn't exist already.
                lastChange = new IconItemLastChange
                {
                    Identifier = command.UpdatedItem.ScanCode,
                    Brand_ID = command.BrandId,
                    Item_Description = command.UpdatedItem.ProductDescription,
                    POS_Description = command.UpdatedItem.PosDescription.ToUpper(),
                    Package_Desc1 = Convert.ToDecimal(command.UpdatedItem.PackageUnit),
                    Food_Stamps = Convert.ToBoolean(Convert.ToInt16(command.UpdatedItem.FoodStampEligible)),
                    ScaleTare = Convert.ToDecimal(command.UpdatedItem.Tare),
                    Subteam_No = (command.UpdatedItem.SubTeamNo != -1) ? command.UpdatedItem.SubTeamNo : (int?)null,
                    TaxClassID = command.TaxClassId,
                    InsertDate = DateTime.Now,
                    AreNutriFactsChanged = command.UpdatedItem.AreNutriFactsUpdated,
                    ClassID = command.ClassId,
                    Package_Unit_ID = command.PackageUnitId,
                    Package_Desc2 = Convert.ToDecimal(command.UpdatedItem.RetailSize),
                };

                this.context.IconItemLastChange.Add(lastChange);
            }
        }
    }
}
