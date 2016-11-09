using GlobalEventController.Common;
using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.DataAccess.DataServices
{
    public class EmailUomChangeService : IEmailUomChangeService
    {
        private IEmailClient emailClient;

        public EmailUomChangeService(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }

        public void NotifyUomChanges(List<IrmaItemModel> irmaItems, List<ValidatedItemModel> validatedItems, string regionAbbreviation, string emailSubjectEnvironment)
        {
            // We only want to send alerts for a Retail Unit change if the Retail Uom is changing from LB to something other than LB and vice versa
            var items = (from i in irmaItems
                         join vi in validatedItems on i.Identifier equals vi.ScanCode
                         where (((vi.RetailUom == "LB" && i.RetailUnitAbbreviation != "LB") // compare Icon's Retail Uom with IRMA's Retail Unit
                                    || (vi.RetailUom != "LB" && i.RetailUnitAbbreviation == "LB"))
                                 && i.IsDefaultIdentifier
                                 && ((vi.RetailUom == "LB" && i.RetailUomAbbreviation != "LB") // also compare Icon's Retail Uom with IRMA's Retail Uom
                                    || (vi.RetailUom != "LB" && i.RetailUomAbbreviation == "LB")))
                         select new IrmaItemModel
                         {
                             Item_Key = i.Item_Key,
                             Identifier = i.Identifier,
                             Description = i.Description,
                             SubTeamNo = i.SubTeamNo,
                             SubTeamName = i.SubTeamName,
                             RetailPack = i.RetailPack,
                             RetailSize = i.RetailSize,
                             RetailUomAbbreviation = i.RetailUomAbbreviation,
                             IconRetailUomAbbreviation = vi.RetailUom,
                             RetailUnitAbbreviation = i.RetailUnitAbbreviation,
                             RetailUomIsWeightedUnit = i.RetailUomIsWeightedUnit
                         })
                        .OrderBy(i => i.SubTeamName)
                        .ThenBy(i => i.Identifier)
                        .ThenBy(i => i.RetailUomAbbreviation)
                        .ToList();

            if (items.Any())
            {
                string body = EmailHelper.BuildUomChangeTable(items);
                emailClient.Send(String.Format("This is the list of Items in {0} with Retail UOM changes from Icon: {1}", regionAbbreviation, body),
                    String.Format("{0} - {1} Retail UOM Changes from Icon", emailSubjectEnvironment, regionAbbreviation));
            }
        }
    }
}
