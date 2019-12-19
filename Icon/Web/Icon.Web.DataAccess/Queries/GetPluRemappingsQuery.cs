using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRemappingsQuery : IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>>
    {
        private readonly IconContext context;

        public GetPluRemappingsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<BulkImportPluRemapModel> Search(GetPluRemappingsParameters parameters)
        {
            var importedItems = parameters.ImportedItems;

            List<BulkImportPluRemapModel> remappings = new List<BulkImportPluRemapModel>();

            // Set up types for reflection.
            var pluModelType = typeof(BulkImportPluModel);
            var pluModelProperties = pluModelType.GetProperties();
            int pluModelPropertiesCount = pluModelProperties.Count();

            var pluMapType = typeof(PLUMap);
            var pluMapProperties = pluMapType.GetProperties();
            int pluMapPropertiesCount = pluMapProperties.Count();

            List<PLUMap> pluMap = new List<PLUMap>();
            pluMap = context.PLUMap.ToList();

            // Start the loop through each property of the BulkImportPluModel (conceptually this is looping through each regional PLU column).  Start the index at three to
            // skip over the NationalPlu and informational columns.
            int regionalPluIndex = 3;
            while (regionalPluIndex < pluModelPropertiesCount)
            {
                string pluRegion = pluModelType.GetProperty(pluModelProperties[regionalPluIndex].Name).Name;

                // Build a list of all PLUs for this region from PLUMap.
                var pluByRegionFromPluMap = pluMap.Select(plu => new BulkImportPluRemapModel
                {
                    CurrentNationalPluId = plu.itemID,
                    RegionalPlu = pluMapType.GetProperty(pluRegion).GetValue(plu, null) == null ? null : pluMapType.GetProperty(pluRegion).GetValue(plu, null).ToString(),                    
                    Region = pluMapType.GetProperty(pluRegion).Name
                }).ToList();

                // Build a list of all PLUs for this region from the spreadsheet.
                var pluByRegionFromSpreadsheet = importedItems.Select(plu => pluModelType.GetProperty(pluModelProperties[regionalPluIndex].Name).GetValue(plu, null)).ToList();

                // For each regional PLU in the spreadsheet, check for a duplicate in the PLUMap table results.
                var duplicatePluMappings = (from dbPlu in pluByRegionFromPluMap
                                           join spreadsheetPlu in pluByRegionFromSpreadsheet on dbPlu.RegionalPlu equals spreadsheetPlu
                                           select dbPlu).ToList();

                // For each remapping that was found, the analysis begins by getting the new national PLU from the spreadsheet.
                foreach (var remapping in duplicatePluMappings)
                {
                    var remapRow = importedItems.Where(pluModel => pluModelType.GetProperty(pluRegion).GetValue(pluModel, null).ToString() == remapping.RegionalPlu.ToString()).Single();

                    string newNationalPlu = remapRow.NationalPlu;
                    remapping.NewNationalPlu = newNationalPlu;

                    int newNationalPluId = context.Item.Where(item => item.ScanCode.Any(scanCode => scanCode.scanCode == newNationalPlu)).Select(item => item.ItemId).Single();
                    string currentNationalPlu = context.ScanCode.Where(scanCode => scanCode.itemID == remapping.CurrentNationalPluId).Select(scanCode => scanCode.scanCode).FirstOrDefault();

                    remapping.NewNationalPluId = newNationalPluId;
                    remapping.CurrentNationalPlu = currentNationalPlu;

                    // If the previously mmapped national PLU and the new national PLU are the same, then this isn't really a remapping; it's just re-applying the current mapping.
                    // In that case, we'll skip this section and not add it to the list of remaps.
                    if (remapping.CurrentNationalPlu != remapping.NewNationalPlu)
                    {
                        string currentNationalPluDescription = context.ItemTrait
                            .Single(itemTrait => itemTrait.Trait.traitCode == TraitCodes.ProductDescription && itemTrait.itemID == remapping.CurrentNationalPluId).traitValue;
                        remapping.CurrentNationalPluDescription = currentNationalPluDescription;

                        string newPluDescription = context.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.ProductDescription && itemTrait.itemID == remapping.NewNationalPluId).Single().traitValue;
                        remapping.NewNationalPluDescription = newPluDescription;

                        remappings.Add(remapping);
                    }
                }

                regionalPluIndex++;
            }

            return remappings;
        }
    }
}
