using System;
using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.Service.Validation
{
    public class RowObjectsValidator : IRowObjectsValidator
    {

        private readonly IBrandsCache brandsCache;
        private readonly IRegexTextValidator regexTextValidator;
        

        public RowObjectsValidator(
            IRegexTextValidator regexTextValidator, IBrandsCache brandsCache)
        {
            this.regexTextValidator = regexTextValidator;
            this.brandsCache = brandsCache;
        }

        public RowObjectValidatorResponse Validate(Enums.FileModeTypeEnum fileModeType, List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<BrandAttributeModel> brandAttributeModels)
        {
            switch (fileModeType)
            {
                case Enums.FileModeTypeEnum.CreateNew:
                    return ValidateCreateNew(rowObjects, columnHeaders, brandAttributeModels);
                case Enums.FileModeTypeEnum.UpdateExisting:
                    return ValidateUpdateExisting(rowObjects, columnHeaders, brandAttributeModels);
                default:
                    throw new ArgumentException($"No validator is set for fileModeType {fileModeType}", nameof(fileModeType));
            }
        }

        private static void ValidateParentCompany(RowObjectDictionary rowObjectDictionary, string parentCompany, List<BrandModel> brandsFromDatabase, List<InvalidRowError> errors)
        {

            var matchingBrand = brandsFromDatabase.FirstOrDefault(b =>  string.Equals(b.BrandName, parentCompany, StringComparison.CurrentCultureIgnoreCase));

            if (matchingBrand == null)
            {
                errors.Add(new InvalidRowError
                {
                    RowId = rowObjectDictionary.Row, 
                    Error = $"'{Constants.ParentCompanyColumnHeader}' has invalid value. '{parentCompany}' does not exist in Icon."
                });
            }
        }

        private static void ValidateDuplicateBrandAbbreviations(RowObjectDictionary rowObjectDictionary,BrandModel existingBrand, List<BrandModel> brandsFromDatabase,
            string brandAbbreviation, HashSet<string> brandAbbreviatoinsThatExistMoreThanOnceInWorksheet, List<InvalidRowError> errors)
        {
            if (!string.IsNullOrWhiteSpace(brandAbbreviation))
            {
                IEnumerable<BrandModel> dupesInDatabase;
                if (existingBrand != null)
                {
                    dupesInDatabase = brandsFromDatabase.Where(b => b.BrandAbbreviation == brandAbbreviation && b.BrandId != existingBrand.BrandId);
                }
                else
                {
                    dupesInDatabase = brandsFromDatabase.Where(b => b.BrandAbbreviation == brandAbbreviation);
                }

                if (dupesInDatabase.Any())
                {
                    errors.Add(new InvalidRowError
                    {
                        RowId = rowObjectDictionary.Row,
                        Error =
                            $"'{Constants.BrandAbbreviationColumnHeader}' has invalid value. '{brandAbbreviation}' already exists in the database and must be unique."
                    });
                }

                if (brandAbbreviatoinsThatExistMoreThanOnceInWorksheet.Contains(brandAbbreviation))
                {
                    errors.Add(new InvalidRowError
                    {
                        RowId = rowObjectDictionary.Row,
                        Error =
                            $"'{Constants.BrandAbbreviationColumnHeader}' has invalid value. '{brandAbbreviation}' exists more than once in the worksheet and must be unique."
                    });
                }
            }
        }

        private static void ValidateDuplicateBrandNames(RowObjectDictionary rowObjectDictionary, BrandModel existingBrand, List<BrandModel> brandsFromDatabase, string brandName, 
             HashSet<string> brandNamesThatExistMoreThanOnceInWorksheet, HashSet<string> trimmedBrandNamesThatExistMoreThanOnceInWorksheet, List<InvalidRowError> errors)
        {
             if (string.IsNullOrWhiteSpace(brandName)) return;

            IEnumerable<BrandModel> dupesInDatabase;
            IEnumerable<BrandModel> dupesInDatabaseByTrimmedName = null;
            string trimmedBrandName = string.Empty;
            

            if (existingBrand != null)
            {
                dupesInDatabase = brandsFromDatabase.Where(b => b.BrandName == brandName && b.BrandId != existingBrand.BrandId);
            }
            else
            {
                dupesInDatabase = brandsFromDatabase.Where(b => b.BrandName == brandName);
            }

            if (brandName.Length > Constants.IrmaBrandNameMaxLength)
            {
                trimmedBrandName = brandName.Substring(0, Constants.IrmaBrandNameMaxLength);
                if (existingBrand != null)
                {
                    dupesInDatabaseByTrimmedName = brandsFromDatabase.Where(b => b.BrandName == trimmedBrandName && b.BrandId != existingBrand.BrandId);
                }
                else
                {
                    dupesInDatabaseByTrimmedName = brandsFromDatabase.Where(b => b.BrandName == trimmedBrandName);
                }
            }

            if (dupesInDatabase.Any())
            {
                errors.Add(new InvalidRowError
                {
                    RowId = rowObjectDictionary.Row,
                    Error =
                        $"'{Constants.BrandNameColumnHeader}' has invalid value. '{brandName}' already exists in the database and must be unique."
                });
            }

            if (brandNamesThatExistMoreThanOnceInWorksheet.Contains(brandName))
                errors.Add(new InvalidRowError
                {
                    RowId = rowObjectDictionary.Row,
                    Error =
                        $"'{Constants.BrandNameColumnHeader}' has invalid value. '{brandName}' exists more than once in the worksheet and must be unique."
                });


            if (trimmedBrandNamesThatExistMoreThanOnceInWorksheet.Contains(trimmedBrandName))
                errors.Add(new InvalidRowError
                {
                    RowId = rowObjectDictionary.Row,
                    Error =
                        $"Brand name '{brandName}' trimmed to {Constants.IrmaBrandNameMaxLength} characters already exists. Change the brand name so that the first {Constants.IrmaBrandNameMaxLength} characters are unique."
                });

            if (dupesInDatabaseByTrimmedName != null && dupesInDatabaseByTrimmedName.Any())
            {
                errors.Add(new InvalidRowError
                {
                    RowId = rowObjectDictionary.Row,
                    Error =
                        $"Brand name '{brandName}' trimmed to {Constants.IrmaBrandNameMaxLength} characters already exists in the spreadsheet. Change the brand name so that the first {Constants.IrmaBrandNameMaxLength} characters are unique."
                });

            }
        }

        public RowObjectValidatorResponse ValidateCreateNew(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<BrandAttributeModel> brandAttributeModels)
        {
            var response = new RowObjectValidatorResponse();
            var brandIdIndex = columnHeaders.First(c => c.Name == Constants.BrandIdColumnHeader).ColumnIndex;
            var brandNameIndex = columnHeaders.First(c => c.Name == Constants.BrandNameColumnHeader).ColumnIndex;
            var brandAbbreviationIndex = columnHeaders.First(c => c.Name == Constants.BrandAbbreviationColumnHeader).ColumnIndex;
            var parentComapnyIndex = columnHeaders.First(c => c.Name == Constants.ParentCompanyColumnHeader).ColumnIndex;

            var attributeColumns = columnHeaders
                .Join(brandAttributeModels.Where(a => !a.IsReadOnly),
                    c => c.Name,
                    a => a.TraitDesc,
                    (c, a) => new AttributeColumn
                    {
                        ColumnHeader = c,
                        IsRequired = a.IsRequired,
                        RegexPattern = a.TraitPattern
                    })
                .ToList();

            var rowObjectDictionaries = rowObjects
                .Select(r => new RowObjectDictionary()
                {
                    Row = r.Row,
                    Cells = r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue),
                    RowObject = r
                }).ToList();

            

            // Brand Names that appear in uploaded worksheet more than once.
            var brandNamesThatExistMoreThanOnceInWorksheet = DuplicateColumnValuesFromWorksheet(rowObjectDictionaries, brandNameIndex, brandName => brandName);
            // brand names trimmed to meet IRMA length requirements in uploaded worksheet more than once.
            var trimmedBrandNamesThatExistMoreThanOnceInWorksheet = DuplicateColumnValuesFromWorksheet(rowObjectDictionaries, brandNameIndex, brandName => brandName.Length >= Constants.IrmaBrandNameMaxLength ? brandName.Substring(0, Constants.IrmaBrandNameMaxLength) : brandName);
            // Brand Abbreviations that appear in uploaded worksheet more than once.
            var brandAbbreviationsThatExistMoreThanOnceInWorksheet = DuplicateColumnValuesFromWorksheet(rowObjectDictionaries, brandAbbreviationIndex, brandAbbrev => brandAbbrev);


            foreach (var rowObjectDictionary in rowObjectDictionaries)
            {
                List<InvalidRowError> errors = new List<InvalidRowError>();

                try
                {
                    // make sure brandname and brandabbreviation are unique.
                    string brandName = rowObjectDictionary.Cells.ContainsKey(brandNameIndex) ? rowObjectDictionary.Cells[brandNameIndex] : null;
                    string brandAbbreviation = rowObjectDictionary.Cells.ContainsKey(brandAbbreviationIndex) ? rowObjectDictionary.Cells[brandAbbreviationIndex] : null;
                    string brandId = rowObjectDictionary.Cells.ContainsKey(brandIdIndex) ? rowObjectDictionary.Cells[brandIdIndex] : null;
                    string parentCompany = rowObjectDictionary.Cells.ContainsKey(parentComapnyIndex) ? rowObjectDictionary.Cells[parentComapnyIndex] : null;

                    if ( string.IsNullOrWhiteSpace(brandName))
                    {
                        errors.Add(new InvalidRowError {RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.RequiredBrandName});
                    }

                    if (string.IsNullOrWhiteSpace(brandAbbreviation))
                    {
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.RequiredBrandAbbreviation });
                    }

                    if (brandName != null && string.Equals(brandName, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        errors.Add(new InvalidRowError {RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.InvalidRemoveBrandName});
                    }
                    if (brandAbbreviation != null &&  string.Equals(brandAbbreviation, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.InvalidRemoveBrandAbbreviation });
                    }

                    if (brandId != null) errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.CreateNewBrandIdNotAllowed });

                    ValidateDuplicateBrandNames(rowObjectDictionary, null, brandsCache.Brands, brandName, brandNamesThatExistMoreThanOnceInWorksheet, trimmedBrandNamesThatExistMoreThanOnceInWorksheet, errors);
                    ValidateDuplicateBrandAbbreviations(rowObjectDictionary, null, brandsCache.Brands, brandAbbreviation, brandAbbreviationsThatExistMoreThanOnceInWorksheet, errors);

                    if (errors.Any())
                    {
                        response.InvalidRows.AddRange(errors);
                        continue;
                    }

                    foreach (var attributeColumn in attributeColumns)
                    {
                        // skip parent company when validating regex. It uses different rules.
                        if (attributeColumn.ColumnHeader.Name == "Parent Company") continue;

                        var rowObjectContainsAttribute = rowObjectDictionary.Cells.ContainsKey(attributeColumn.ColumnHeader.ColumnIndex);
                        if (attributeColumn.IsRequired && !rowObjectContainsAttribute)
                        {
                            errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{attributeColumn.ColumnHeader.Name}' is required." });
                        }
                        else if (rowObjectContainsAttribute)
                        {
                            var value = rowObjectDictionary.Cells[attributeColumn.ColumnHeader.ColumnIndex];
                            if (attributeColumn.IsRequired || value != string.Empty)
                            {
                                if (string.Equals(value, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{attributeColumn.ColumnHeader.Name}' has invalid value. '{Constants.RemoveExcelValue}' cannot be used when creating new brands." });
                                    continue;
                                }

                                var result = regexTextValidator.Validate(attributeColumn, value);
                                if (!result.IsValid) errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = result.Error });
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(parentCompany) )
                    {
                        if (string.Equals(parentCompany, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{Constants.ParentCompanyColumnHeader}' has invalid value. '{Constants.RemoveExcelValue}' cannot be used when creating new brands." });
                        }
                        else
                        {
                            ValidateParentCompany(rowObjectDictionary, parentCompany, brandsCache.Brands, errors);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"Unexpected error occurred while validating row. Error: {ex.Message}" });
                }

                if (errors.Count > 0)
                    response.InvalidRows.AddRange(errors);
                else
                    response.ValidRows.Add(rowObjectDictionary.RowObject);
            }
            return response;
        }

        private RowObjectValidatorResponse ValidateUpdateExisting(List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders, List<BrandAttributeModel> brandAttributeModels)
        {
            var response = new RowObjectValidatorResponse();
            var brandIdIndex = columnHeaders.First(c => c.Name == Constants.BrandIdColumnHeader).ColumnIndex;
            var brandNameIndex = columnHeaders.First(c => c.Name == Constants.BrandNameColumnHeader).ColumnIndex;
            var brandAbbreviationIndex =
                columnHeaders.First(c => c.Name == Constants.BrandAbbreviationColumnHeader).ColumnIndex;
            var parentComapnyIndex =
                columnHeaders.First(c => c.Name == Constants.ParentCompanyColumnHeader).ColumnIndex;

            var attributeColumns = columnHeaders
                .Join(brandAttributeModels.Where(a => !a.IsReadOnly),
                    c => c.Name,
                    a => a.TraitDesc,
                    (c, a) => new AttributeColumn
                    {
                        ColumnHeader = c,
                        IsRequired = a.IsRequired,
                        RegexPattern = a.TraitPattern
                    })
                .ToList();

            var rowObjectDictionaries = rowObjects
                .Select(r => new RowObjectDictionary()
                {
                    Row = r.Row,
                    Cells = r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue),
                    RowObject = r
                }).ToList();

            // Brand Names that appear in uploaded worksheet more than once.
            var brandNamesThatExistMoreThanOnceInWorksheet =
                DuplicateColumnValuesFromWorksheet(rowObjectDictionaries, brandNameIndex, brandName => brandName);
            // brand names trimmed to meet IRMA length requirements in uploaded worksheet more than once.
            var trimmedBrandNamesThatExistMoreThanOnceInWorksheet = DuplicateColumnValuesFromWorksheet(
                rowObjectDictionaries, brandNameIndex,
                brandName => brandName.Length >= Constants.IrmaBrandNameMaxLength
                    ? brandName.Substring(0, Constants.IrmaBrandNameMaxLength)
                    : brandName);
            // Brand Abbreviations that appear in uploaded worksheet more than once.
            var brandAbbreviationsThatExistMoreThanOnceInWorksheet =
                DuplicateColumnValuesFromWorksheet(rowObjectDictionaries, brandAbbreviationIndex,
                    brandAbbrev => brandAbbrev);

            foreach (var rowObjectDictionary in rowObjectDictionaries)
            {
                List<InvalidRowError> errors = new List<InvalidRowError>();
                int brandIdInt;
                // make sure brandname and brandabbreviation are unique.
                string brandName = rowObjectDictionary.Cells.ContainsKey(brandNameIndex)
                    ? rowObjectDictionary.Cells[brandNameIndex]
                    : null;
                string brandAbbreviation = rowObjectDictionary.Cells.ContainsKey(brandAbbreviationIndex)
                    ? rowObjectDictionary.Cells[brandAbbreviationIndex]
                    : null;
                string brandId = rowObjectDictionary.Cells.ContainsKey(brandIdIndex)
                    ? rowObjectDictionary.Cells[brandIdIndex]
                    : null;
                string parentCompany = rowObjectDictionary.Cells.ContainsKey(parentComapnyIndex)
                    ? rowObjectDictionary.Cells[parentComapnyIndex]
                    : null;
                
                BrandModel existingBrand = null;

                if (string.IsNullOrWhiteSpace(brandId))
                {
                    errors.Add(new InvalidRowError
                    {
                        RowId = rowObjectDictionary.Row,
                        Error = Constants.ErrorMessages.RequiredBrandId
                    });
                }
                else
                {
                    if (!int.TryParse(brandId, out brandIdInt))
                    {
                        errors.Add(new InvalidRowError
                            {RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.InvalidBrandIdDataType});
                    }
                    else
                    {
                        existingBrand = brandsCache.Brands.FirstOrDefault(b => b.BrandId == brandIdInt);

                        if (existingBrand == null)
                            errors.Add(new InvalidRowError
                                { RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.InvalidbrandId });
                    }
                }

                // only validate name and abbreviation if values are included.
                if (!string.IsNullOrWhiteSpace(brandName))
                {
                    // validate brand.
                    if (string.Equals(brandName, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                    {
                        errors.Add(new InvalidRowError
                            {RowId = rowObjectDictionary.Row, Error = Constants.ErrorMessages.InvalidRemoveBrandName});
                    }

                    ValidateDuplicateBrandNames(rowObjectDictionary, existingBrand, brandsCache.Brands, brandName,
                        brandNamesThatExistMoreThanOnceInWorksheet, trimmedBrandNamesThatExistMoreThanOnceInWorksheet,
                        errors);
                }

                if (!string.IsNullOrWhiteSpace(brandAbbreviation))
                {
                    //validate brand abbreviation
                    if (brandAbbreviation != null && string.Equals(brandAbbreviation, Constants.RemoveExcelValue,
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        errors.Add(new InvalidRowError
                        {
                            RowId = rowObjectDictionary.Row,
                            Error = Constants.ErrorMessages.InvalidRemoveBrandAbbreviation
                        });
                    }

                    ValidateDuplicateBrandAbbreviations(rowObjectDictionary, existingBrand, brandsCache.Brands,
                        brandAbbreviation, brandAbbreviationsThatExistMoreThanOnceInWorksheet, errors);
                }


                if (existingBrand != null)
                {
                    if (!string.IsNullOrEmpty(parentCompany) && parentCompany.ToLower() != "remove")
                    {
                        ValidateParentCompany(rowObjectDictionary, parentCompany, brandsCache.Brands, errors);
                    }

                    if (errors.Any())
                    {
                        response.InvalidRows.AddRange(errors);
                        continue;
                    }

                    foreach (var attributeColumn in attributeColumns)
                    {
                        // skip parent company when validating regex. It uses different rules.
                        if (attributeColumn.ColumnHeader.Name == "Parent Company") continue;
                        if (!rowObjectDictionary.Cells.ContainsKey(attributeColumn.ColumnHeader.ColumnIndex)) continue;

                        var value = rowObjectDictionary.Cells[attributeColumn.ColumnHeader.ColumnIndex];

                        if (value == string.Empty) continue;

                        if (string.Equals(value, Constants.RemoveExcelValue, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (attributeColumn.IsRequired)
                            {
                                errors.Add(new InvalidRowError
                                {
                                    RowId = rowObjectDictionary.Row,
                                    Error = $"'{attributeColumn.ColumnHeader.Name}' is required and can't be removed."
                                });
                            }
                        }
                        else
                        {
                            var result = regexTextValidator.Validate(attributeColumn, value);
                            if (result.IsValid) continue;
                            errors.Add(new InvalidRowError
                            {
                                RowId = rowObjectDictionary.Row,
                                Error = $"'{attributeColumn.ColumnHeader.Name}' has invalid value. {result.Error}"
                            });
                        }
                    }
                }

                if (errors.Count > 0)
                {
                    response.InvalidRows.AddRange(errors);
                }
                else
                {
                    response.ValidRows.Add(rowObjectDictionary.RowObject);
                }
            }
            return response;
        }

        private static HashSet<string> DuplicateColumnValuesFromWorksheet(List<RowObjectDictionary> rowObjectDictionaries, int columnIndex, Func<string, string> transformKey)
        {
            var valuesThatExistMoreThanOnceInWorksheet = rowObjectDictionaries
                .Where(r => r.Cells.ContainsKey(columnIndex)
                            && !string.IsNullOrEmpty(r.Cells[columnIndex]))
                .GroupBy(g => transformKey(g.Cells[columnIndex]))
                .Where(g => g.Count() > 1)
                .Select(s => s.Key)
                .ToHashSet();
            return valuesThatExistMoreThanOnceInWorksheet;
        }
    }
}