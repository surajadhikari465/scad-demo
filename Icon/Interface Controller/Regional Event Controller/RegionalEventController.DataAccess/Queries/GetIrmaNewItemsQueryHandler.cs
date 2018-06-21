using Icon.Framework;
using Irma.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetIrmaNewItemsQueryHandler : IQueryHandler<GetIrmaNewItemsQuery, List<IrmaNewItem>>
    {
        private IrmaContext context;
        public GetIrmaNewItemsQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<IrmaNewItem> Execute(GetIrmaNewItemsQuery parameters)
        {
            string regionCode;
            int defaultOrganicAgencyId = Cache.defaultCertificationAgencies[CertificationAgencyType.Organic];

            List<IrmaNewItem> newIrmaItems = new List<IrmaNewItem>();

            regionCode = (from r in context.Region
                          select r.RegionCode).Single().ToString();

            newIrmaItems.AddRange((from qe in context.IconItemChangeQueue
                                   join i in context.Item on qe.Item_Key equals i.Item_Key
                                   join ii in context.ItemIdentifier on qe.Identifier equals ii.Identifier
                                   join u in context.ItemUnit on i.Package_Unit_ID equals u.Unit_ID
                                   join s in context.SubTeam on i.SubTeam_No equals s.SubTeam_No
                                   from nc in context.NatItemClass
                                           .Where(nic => nic.ClassID == i.ClassID)
                                           .DefaultIfEmpty()
                                   where 
                                        (qe.InProcessBy == StartupOptions.Instance.ToString()
                                        && ii.Deleted_Identifier == 0 
                                        && ii.Remove_Identifier == 0 
                                        && !i.Deleted_Item 
                                        && i.Remove_Item == 0 
                                        && i.Item_Key == ii.Item_Key)
                                   select new IrmaNewItem
                                   {
                                       // General Properties
                                       RegionCode = regionCode,
                                       IrmaTaxClass = i.TaxClass.TaxClassDesc,
                                       IrmaNationalClass = nc != null ? nc.ClassID : 99999,

                                       // IconItemChangeQueue
                                       QueueId = qe.QID,
                                       IrmaItemKey = qe.Item_Key,
                                       Identifier = qe.Identifier,
                                       nonretailItem = i.Retail_Sale ? false : true,

                                       // IRMA
                                       IrmaItem = new IRMAItem
                                       {
                                           regioncode = regionCode,
                                           identifier = qe.Identifier,
                                           defaultIdentifier = (ii.Default_Identifier == 1),
                                           insertDate = qe.InsertDate,
                                           brandName = i.ItemBrand.Brand_Name.Trim(),
                                           itemDescription = i.Item_Description.ToUpper(),
                                           posDescription = i.POS_Description.ToUpper(),
                                           packageUnit = (int)i.Package_Desc1,
                                           retailSize = i.Package_Desc2,
                                           retailUom = u.Unit_Abbreviation,
                                           foodStamp = i.Food_Stamps,
                                           departmentSale = false,
                                           posScaleTare = 0.0m,
                                           giftCard = i.GiftCard,
                                           irmaSubTeamName = s.SubTeam_Name
                                       }
                                   }).ToList());

            return newIrmaItems;
        }
    }
}
