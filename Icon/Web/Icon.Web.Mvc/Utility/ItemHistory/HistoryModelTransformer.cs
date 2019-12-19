using Icon.Common;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Utility.ItemHistory
{
    /// <summary>
    /// This class is responsible for transforming item history models
    /// </summary>
    public class HistoryModelTransformer : IHistoryModelTransformer
    {
        /// <summary>
        /// Converts ItemHistoryDbModel to a ItemHistoryModel
        /// </summary>
        /// <param name="itemHistory"></param>
        /// <returns></returns>
        public List<ItemHistoryModel> ToViewModels(IEnumerable<ItemHistoryDbModel> itemHistory)
        {
            List<ItemHistoryModel> viewModels = new List<ItemHistoryModel>();

            foreach (var history in itemHistory)
            {
                viewModels.Add(history.ToViewModel());
            }

            return viewModels.OrderBy(x => x.SysStartTimeUtc).ToList();
        }

        /// <summary>
        /// This method is responsible for taking an Infor item history json object and converting it into a ItemHistoryModel
        /// </summary>
        /// <param name="inforItemHistory"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public List<ItemHistoryModel> TransformInforHistory(IEnumerable<ItemInforHistoryDbModel> inforItemHistory, IEnumerable<AttributeDisplayModel> attributes)
        {
            var response = new List<ItemHistoryModel>();
            foreach (ItemInforHistoryDbModel history in inforItemHistory)
            {
                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(history.JsonObject);
                Dictionary<string, string> convertedAttributes = new Dictionary<string, string>();

                foreach (string key in values.Keys)
                {
                    if (attributes.Any(x => x.AttributeName == key))
                    {
                        convertedAttributes[key] = values[key].ToString();
                    }
                }
                convertedAttributes[Constants.Attributes.ModifiedBy] = values[InforConstants.UpdatedBy].ToString();

                if (values.ContainsKey(InforConstants.Brand))
                {
                    var brand = JsonConvert.DeserializeObject<Brand>(values[InforConstants.Brand].ToString());
                    convertedAttributes[InforConstants.Brand] = $"{brand.BrandName}";
                }

                if (values.ContainsKey(InforConstants.Merchandise))
                {
                    var merch = JsonConvert.DeserializeObject<Merchandise>(values[InforConstants.Merchandise].ToString());
                    convertedAttributes[InforConstants.Merchandise] = $"{merch.Segment.Name} | {merch.Family.Name} | {merch.Class.Name} | {merch.GS1Brick.Name} | {merch.SubBrick.Name}";
                }

                if (values.ContainsKey(InforConstants.National))
                {
                    var national = JsonConvert.DeserializeObject<National>(values[InforConstants.National].ToString());
                    convertedAttributes[InforConstants.National] = $"{national.Family.Name} | {national.Category.Name} | {national.SubCategory.Name} | {national.Class.Name}";
                }

                if (values.ContainsKey(InforConstants.Subteam))
                {
                    var financial = JsonConvert.DeserializeObject<Financial>(values[InforConstants.Subteam].ToString());
                    convertedAttributes[InforConstants.Financial] = $"{financial.SubteamName}";
                    convertedAttributes.Remove(InforConstants.Subteam);
                }

                if (values.ContainsKey(InforConstants.Tax))
                {
                    var tax = JsonConvert.DeserializeObject<Tax>(values[InforConstants.Tax].ToString());
                    convertedAttributes[InforConstants.Tax] = $"{tax.TaxRomance}";
                }

                string itemTypeCode = string.Empty;
                if (values.ContainsKey(InforConstants.ItemType))
                {
                    var itemType = JsonConvert.DeserializeObject<ItemType>(values[InforConstants.ItemType].ToString());
                    convertedAttributes.Remove(InforConstants.ItemType);
                    itemTypeCode = itemType.ItemTypeCode;

                }

                response.Add(new ItemHistoryModel()
                {
                    ItemId = history.ItemId,
                    ItemTypeCode = itemTypeCode,
                    ItemAttributes = convertedAttributes,
                    SysStartTimeUtc = DateTime.Parse(values[InforConstants.UpdatedOn].ToString())
                });
            }

            return response.OrderBy(x => x.SysStartTimeUtc).ToList();
        
        }
    }
}