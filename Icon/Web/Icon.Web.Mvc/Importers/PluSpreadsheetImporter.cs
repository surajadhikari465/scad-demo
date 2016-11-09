using Icon.Common.DataAccess;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Importers
{
    public class PluSpreadsheetImporter : IPluSpreadsheetImporter
    {
        private IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodesQuery;
        private IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>> remappingQuery;
        private string[] nonRegionalPluPropertyNames = new string[3] { "BrandName", "ProductDescription", "NationalPlu" };

        public Workbook Workbook { get; set; }
        public List<BulkImportPluModel> ParsedRows { get; set; }
        public List<BulkImportPluModel> ErrorRows { get; set; }
        public List<BulkImportPluModel> ValidRows { get; set; }
        public List<BulkImportPluRemapModel> Remappings { get; set; }
        public IObjectValidator<string> Validator { get; set; }

        public PluSpreadsheetImporter(IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodesQuery, 
            IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>> remappingQuery)
        {
            this.getNewScanCodesQuery = getNewScanCodesQuery;
            this.remappingQuery = remappingQuery;

            ParsedRows = new List<BulkImportPluModel>();
            ValidRows = new List<BulkImportPluModel>();
            ErrorRows = new List<BulkImportPluModel>();
            Remappings = new List<BulkImportPluRemapModel>();
        }

        public bool ValidSpreadsheetType()
        {
            string[] validHeaders = 
            {
                "Brand",
                "Product Description",
                "National PLU",
                "FL PLU",
                "MA PLU",
                "MW PLU",
                "NA PLU",
                "NC PLU",
                "NE PLU",
                "PN PLU",
                "RM PLU",
                "SO PLU",
                "SP PLU",
                "SW PLU",
                "UK PLU"
            };

            int headersCount = validHeaders.Count();

            string[] spreadsheetHeaders = new string[headersCount];

            for (int i = 0; i < headersCount; i++)
            {
                var cell = Workbook.Worksheets[0].Rows[0].Cells[i];
                var header = cell.Value == null ? String.Empty : cell.Value.ToString();

                spreadsheetHeaders[i] = header;
            }

            return validHeaders.SequenceEqual(spreadsheetHeaders);
        }

        public void ConvertSpreadsheetToModel()
        {
            // Begin at 1 to skip the header row.
            int rowIndex = 1;
            while (Workbook.Worksheets[0].Rows[rowIndex].Cells.Count() > 0)
            {
                var parsedRow = new BulkImportPluModel
                {
                    BrandName = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(0),
                    ProductDescription = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(1),
                    NationalPlu = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(2).TrimStart('0'),
                    
                    // Regional PLUs can contain a single "0", so be careful with trimming.
                    flPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(3) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(3).TrimStart('0'),
                    maPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(4) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(4).TrimStart('0'),
                    mwPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(5) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(5).TrimStart('0'),
                    naPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(6) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(6).TrimStart('0'),
                    ncPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(7) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(7).TrimStart('0'),
                    nePLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(8) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(8).TrimStart('0'),
                    pnPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(9) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(9).TrimStart('0'),
                    rmPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(10) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(10).TrimStart('0'),
                    soPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(11) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(11).TrimStart('0'),
                    spPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(12) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(12).TrimStart('0'),
                    swPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(13) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(13).TrimStart('0'),
                    ukPLU = Workbook.Worksheets[0].Rows[rowIndex].GetCellText(14) == "0" ?
                                "0" : Workbook.Worksheets[0].Rows[rowIndex].GetCellText(14).TrimStart('0')
                };

                // If somehow the importer parses a completely blank row, don't add it to the ParsedRows collection.
                var populatedCells = parsedRow.GetType().GetProperties().Where(cell => cell.GetValue(parsedRow, null).ToString() != String.Empty).ToList();

                if (populatedCells.Count > 0)
                {
                    ParsedRows.Add(parsedRow);
                }

                rowIndex++;
            }
        }

        public void ConvertRemappingsToModel()
        {
            foreach (var remapping in Remappings)
            {
                // Build up a new BulkImportPluModel class using the information from the BulkImportPluRemapModel classes.
                var pluImportModel = new BulkImportPluModel
                {
                    NationalPlu = remapping.NewNationalPlu
                };

                // Extract the remapped regional PLU with reflection, and assign it to the new BulkImportPluModel.
                string regionalPlu = remapping.RegionalPlu;
                string region = remapping.Region;

                var pluModelType = typeof(BulkImportPluModel);
                var pluModelProperties = pluModelType.GetProperties();
                string regionProperty = pluModelType.GetProperty(region).Name;

                pluModelType.GetProperty(regionProperty).SetValue(pluImportModel, regionalPlu, null);

                // Set all other properties to emtpy string instead of null.
                foreach (var property in pluModelProperties)
                {
                    if (property.GetValue(pluImportModel, null) == null)
                    {
                        pluModelType.GetProperty(property.Name).SetValue(pluImportModel, String.Empty, null);
                    }
                }

                ValidRows.Add(pluImportModel);
            }
        }

        public void ValidateSpreadsheetData()
        {
            ValidRows.AddRange(ParsedRows);

            CheckForMissingNationalPlu();

            if (ValidRows.Count > 0)
            {
                CheckForNationalPluWithNoMappings();
            }

            if (ValidRows.Count > 0)
            {
                ValidatePluFormat();
            }

            if (ValidRows.Count > 0)
            {
                CheckForNewNationalPluInserts();
            }

            if (ValidRows.Count > 0)
            {
                CheckForDuplicateRegionalPlu();
            }

            if (ValidRows.Count > 0)
            {
                ValidateRowByNationalPluLength();
            }

            if (ValidRows.Count > 0)
            {
                GetPluRemappings();
            }
        }

        private void CheckForMissingNationalPlu()
        {
            var missingNationalPlus = ValidRows.Where(row => String.IsNullOrEmpty(row.NationalPlu)).ToList();

            if (missingNationalPlus.Count > 0)
            {
                ValidRows = ValidRows.Where(row => !missingNationalPlus.Contains(row)).ToList();
                ErrorRows.AddRange(missingNationalPlus);
            }
        }

        private void CheckForNationalPluWithNoMappings()
        {
            Type pluModelType = typeof(BulkImportPluModel);
            var pluModelProperties = pluModelType.GetProperties();

            List<BulkImportPluModel> invalidRows = new List<BulkImportPluModel>();

            foreach (var row in ValidRows)
            {
                var regionalPluCells = pluModelProperties.Where(pluProperty => !nonRegionalPluPropertyNames.Contains(pluProperty.Name));

                var populatedCells = regionalPluCells.Where(cell => cell.GetValue(row, null).ToString() != String.Empty).ToList();

                if (populatedCells.Count == 0)
                {
                    invalidRows.Add(row);
                }
            }

            ValidRows = ValidRows.Where(row => !invalidRows.Contains(row)).ToList();
            ErrorRows.AddRange(invalidRows);
        }

        private void ValidatePluFormat()
        {
            Validator = new PluValidator();

            Type pluModelType = typeof(BulkImportPluModel);
            var pluModelProperties = pluModelType.GetProperties();
            int pluModelPropertiesCount = pluModelProperties.Count();

            // The properties of BulkImportPluModel that we need to validate begin at index 2 (conceptually, this is every PLU column, including National PLU).
            int pluPropertyIndex = 2;
            while (ValidRows.Count > 0 && pluPropertyIndex < pluModelPropertiesCount)
            {
                string region = pluModelProperties[pluPropertyIndex].Name;

                var invalidRows = ValidRows.Where(plu => !Validator.Validate(pluModelType.GetProperty(region).GetValue(plu, null).ToString())).ToList();

                if (invalidRows.Count > 0)
                {
                    ValidRows = ValidRows.Where(plu => !invalidRows.Contains(plu)).ToList();
                    ErrorRows.AddRange(invalidRows);
                }

                pluPropertyIndex++;
            }
        }

        private void CheckForNewNationalPluInserts()
        {
            var parameters = new GetNewScanCodeUploadsParameters
            {
                ScanCodes = ValidRows.Select(item => new ScanCodeModel { ScanCode = item.NationalPlu }).ToList()
            };

            var newScanCodes = getNewScanCodesQuery.Search(parameters);

            if (newScanCodes.Count > 0)
            {
                var errorRows = ValidRows.Where(plu => newScanCodes.Any(scanCode => scanCode.ScanCode == plu.NationalPlu)).ToList();
                ValidRows = ValidRows.Where(plu => !newScanCodes.Any(scanCode => scanCode.ScanCode == plu.NationalPlu)).ToList();
                ErrorRows.AddRange(errorRows);
            }
        }

        private void CheckForDuplicateRegionalPlu()
        {
            Type pluModelType = typeof(BulkImportPluModel);
            var pluModelProperties = pluModelType.GetProperties();
            int pluModelPropertiesCount = pluModelProperties.Count();

            // Regional PLU properties begin at index 3.
            int pluPropertyIndex = 3;
            while (ValidRows.Count > 0 && pluPropertyIndex < pluModelPropertiesCount)
            {
                string region = pluModelProperties[pluPropertyIndex].Name;

                // Empty strings and zeros shouldn't be counted as duplicates since they have specific business meaning.
                // Empty string = ignore cell, Zero = insert NULL to db.
                var allPluInColumn = ValidRows
                    .Where(plu => pluModelType.GetProperty(region).GetValue(plu, null).ToString() != String.Empty &&
                        pluModelType.GetProperty(region).GetValue(plu, null).ToString() != "0")
                    .Select(plu => pluModelType.GetProperty(region).GetValue(plu, null)).GroupBy(plu => plu).ToList();

                var distinctPluDuplicates = allPluInColumn.Where(plu => plu.Count() > 1).Distinct().Select(distinctPlu => distinctPlu.Key).ToList();

                var invalidRows = ValidRows.Where(plu => distinctPluDuplicates.Contains(pluModelType.GetProperty(region).GetValue(plu, null).ToString())).ToList();

                if (invalidRows.Count > 0)
                {
                    ValidRows = ValidRows.Where(plu => !invalidRows.Contains(plu)).ToList();
                    ErrorRows.AddRange(invalidRows);
                }

                pluPropertyIndex++;
            }
        }

        private void ValidateRowByNationalPluLength()
        {
            List<BulkImportPluModel> invalidRows = new List<BulkImportPluModel>();

            foreach (var plu in ValidRows)
            {
                int nationalPluLength = plu.NationalPlu.Length;

                Type pluModelType = plu.GetType();
                var pluModelProperties = pluModelType.GetProperties();
                int pluModelPropertiesCount = pluModelProperties.Count();

                // Regional PLU properties begin at index 3.
                int regionalPluIndex = 3;
                bool validRow = true;
                while (validRow && regionalPluIndex < pluModelPropertiesCount)
                {
                    string regionalPluColumn = pluModelProperties[regionalPluIndex].Name;
                    string regionalPluValue = pluModelType.GetProperty(regionalPluColumn).GetValue(plu, null).ToString();

                    // It is allowable for the cell to be empty, or to contain only 0.  Otherwise, the length matching should occur.
                    if (nationalPluLength <= 6)
                    {
                        if (!String.IsNullOrEmpty(regionalPluValue) && regionalPluValue != "0" && regionalPluValue.Length > 6)
                        {
                            invalidRows.Add(plu);
                            validRow = false;
                        }
                    }
                    else if (nationalPluLength == 11)
                    {
                        if (!String.IsNullOrEmpty(regionalPluValue) && regionalPluValue != "0" && regionalPluValue.Length != 11)
                        {
                            invalidRows.Add(plu);
                            validRow = false;
                        }
                    }

                    regionalPluIndex++;
                }
            }

            if (invalidRows.Count > 0)
            {
                ValidRows = ValidRows.Where(plu => !invalidRows.Contains(plu)).ToList();
                ErrorRows.AddRange(invalidRows);
            }
        }

        private void GetPluRemappings()
        {
            var parameters = new GetPluRemappingsParameters
            {
                ImportedItems = ValidRows
            };

            Remappings = remappingQuery.Search(parameters);
            int remappingsCount = Remappings.Count;

            Type pluModelType = typeof(BulkImportPluModel);
            var pluModelProperties = pluModelType.GetProperties();
            int pluModelPropertiesCount = pluModelType.GetProperties().Count();

            Type remapModelType = typeof(BulkImportPluRemapModel);
            var remapModelProperties = remapModelType.GetProperties();

            // Regional PLU properties begin at index 3.
            int regionalPluIndex = 3;
            while (remappingsCount > 0 && regionalPluIndex < pluModelPropertiesCount)
            {
                string spreadsheetRegion = pluModelType.GetProperty(pluModelProperties[regionalPluIndex].Name).Name;
                string remapRegionPropertyName = remapModelProperties[4].Name;
                string remapRegionalPluPropertyName = remapModelProperties[5].Name;

                // Get all the PLUs for the given region from both the spreadsheet and the remappings.
                var regionalPluColumn = ValidRows.Select(plu => pluModelType.GetProperty(spreadsheetRegion).GetValue(plu, null)).ToList();
                var duplicateMappings = Remappings
                    .Where(plu => remapModelType.GetProperty(remapRegionPropertyName).GetValue(plu, null).ToString() == spreadsheetRegion)
                    .Select(plu => remapModelType.GetProperty(remapRegionalPluPropertyName).GetValue(plu, null).ToString()).ToList();

                if (duplicateMappings.Count > 0)
                {
                    // When remappings are found, the remapped value should be cleared (empty string) any time that value occurs in the
                    // given regional column of the spreadsheet.  If, due to this process, a row becomes empty, it is no longer considered valid.

                    var invalidRows = new List<BulkImportPluModel>();
                    
                    foreach (var row in ValidRows)
                    {
                        var regionalPluCell = pluModelType.GetProperty(spreadsheetRegion).GetValue(row, null).ToString();

                        if (duplicateMappings.Contains(regionalPluCell))
                        {
                            pluModelType.GetProperty(spreadsheetRegion).SetValue(row, String.Empty, null);
                            remappingsCount--;
                        }

                        var regionalPluCells = pluModelProperties.Where(pluProperty => !nonRegionalPluPropertyNames.Contains(pluProperty.Name));
                        var populatedCells = regionalPluCells.Where(plu => plu.GetValue(row, null).ToString() != String.Empty).ToList();

                        // If by clearing a remapped PLU the row becomes empty, it's no longer considered valid.
                        if (populatedCells.Count == 0)
                        {
                            invalidRows.Add(row);
                        }
                    }

                    if (invalidRows.Count > 0)
                    {
                        ValidRows = ValidRows.Where(plu => !invalidRows.Contains(plu)).ToList();
                    }
                }

                regionalPluIndex++;
            }
        }
    }
}
