using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemsByScanCodeQueryHandler : IQueryHandler<GetItemsByScanCodeQuery, List<IrmaItemModel>>
    {
        private readonly IrmaContext context;

        public GetItemsByScanCodeQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<IrmaItemModel> Handle(GetItemsByScanCodeQuery parameters)
        {
            var items = context.ItemIdentifier
                .Where(ii => parameters.ScanCodes.Contains(ii.Identifier)
                    && ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0)
                .Select(ii => new IrmaItemModel
                {
                    Item_Key = ii.Item_Key,
                    Identifier = ii.Identifier,
                    Description = ii.Item.Item_Description,
                    SubTeamName = ii.Item.SubTeam.SubTeam_Name,
                    SubTeamNo = ii.Item.SubTeam_No,
                    RetailPack = ii.Item.Package_Desc1,
                    RetailSize = ii.Item.Package_Desc2,
                    RetailUomAbbreviation = ii.Item.ItemUnit3.Unit_Abbreviation,
                    RetailUnitAbbreviation = ii.Item.ItemUnit4.Unit_Abbreviation,
                    RetailUomIsWeightedUnit = ii.Item.ItemUnit3.Weight_Unit,
                    IsDefaultIdentifier =  ii.Default_Identifier == 1
                })
                .ToList();

            return items;
        }
    }
}
