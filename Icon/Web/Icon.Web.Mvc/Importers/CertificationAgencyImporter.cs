using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Importers
{
    public class CertificationAgencyImporter : ISpreadsheetImporter<BulkImportCertificationAgencyModel>
    {
        private const int MaxNumberOfRowsToImport = 1000;
        private string[] validHeaders;
        private IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQueryHandler;
        private IQueryHandler<GetCertificationAgencyIdsAssociatedToItemsParameters, List<int>> getCertificationAgencyIdsAssociatedToItemsQueryHandler;

        public Workbook Workbook { get; set; }
        public List<BulkImportCertificationAgencyModel> ErrorRows { get; set; }
        public List<BulkImportCertificationAgencyModel> ParsedRows { get; set; }
        public List<BulkImportCertificationAgencyModel> ValidRows { get; set; }
        public IObjectValidator<string> Validator { get; set; }

        public CertificationAgencyImporter(IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQueryHandler,
            IQueryHandler<GetCertificationAgencyIdsAssociatedToItemsParameters, List<int>> getCertificationAgencyIdsAssociatedToItemsQueryHandler)
        {
            ErrorRows = new List<BulkImportCertificationAgencyModel>();
            ParsedRows = new List<BulkImportCertificationAgencyModel>();
            ValidRows = new List<BulkImportCertificationAgencyModel>();

            this.getCertificationAgenciesQueryHandler = getCertificationAgenciesQueryHandler;
            this.getCertificationAgencyIdsAssociatedToItemsQueryHandler = getCertificationAgencyIdsAssociatedToItemsQueryHandler;
            this.validHeaders = new[] { "Agency", "Gluten Free", "Kosher", "Non-GMO", "Organic", "Vegan" };
        }

        public void ConvertSpreadsheetToModel()
        {
            var rows = Workbook.Worksheets[0].Rows
                .Skip(1)
                .Where(r => r.Cells.Any(c => c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString())))
                .Take(MaxNumberOfRowsToImport);

            foreach (var row in rows)
            {
                string agencyNameAndId = ExcelHelper.GetCellStringValue(row.Cells[0].Value);
                string glutenFree = ExcelHelper.GetCellStringValue(row.Cells[1].Value);
                string kosher = ExcelHelper.GetCellStringValue(row.Cells[2].Value);
                string nonGmo = ExcelHelper.GetCellStringValue(row.Cells[3].Value);
                string organic = ExcelHelper.GetCellStringValue(row.Cells[4].Value);
                string vegan = ExcelHelper.GetCellStringValue(row.Cells[5].Value);

                BulkImportCertificationAgencyModel certificationAgency = new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyNameAndId = agencyNameAndId,
                    CertificationAgencyId = ExcelHelper.GetIdFromCellText(agencyNameAndId),
                    CertificationAgencyName = agencyNameAndId.Split('|').First().Trim(),
                    GlutenFree = glutenFree.GetBoolStringFromCellText(),
                    Kosher = kosher.GetBoolStringFromCellText(),
                    NonGmo = nonGmo.GetBoolStringFromCellText(),
                    Organic = organic.GetBoolStringFromCellText(),
                    Vegan = vegan.GetBoolStringFromCellText()
                };

                ParsedRows.Add(certificationAgency);
            }
        }

        public bool IsValidSpreadsheetType()
        {
            var headerRow = Workbook.Worksheets[0].Rows[0].Cells.Select(c => c.GetText()).Take(6);
            
            return headerRow.SequenceEqual(validHeaders);
        }

        public void ValidateSpreadsheetData()
        {
            ValidRows.AddRange(ParsedRows);
            var existingAgencies = getCertificationAgenciesQueryHandler.Search(new GetCertificationAgenciesParameters());

            CheckForInvalidId();
            CheckForInvalidName();
            CheckForDuplicateIds();
            CheckForDuplicateNames();
            CheckForNonExistingIds(existingAgencies);
            CheckForAlreadyExistingCertificationAgencies(existingAgencies);
            CheckForInvalidGlutenFree();
            CheckForInvalidKosher();
            CheckForInvalidNonGmo();
            CheckForInvalidOrganic();
            CheckForInvalidVegan();
            CheckThatAgencyTraitCanBeRemoved();
        }

        private void CheckForInvalidId()
        {
            var validRowsCopy = ValidRows.ToList();
            foreach (var row in validRowsCopy)
            {
                int id = 0;
                if(int.TryParse(row.CertificationAgencyId, out id))
                {
                    if(id < 0)
                    {
                        AddErrorRow(row, string.Format("{0} is not a valid ID. IDs must be integers greater than negative 1.", id));
                    }
                }
                else
                {
                    AddErrorRow(row, string.Format("{0} is not a valid ID. IDs must be integers greater than negative 1.", row.CertificationAgencyId));
                }
            }
        }

        private void CheckForInvalidName()
        {
            foreach (var row in ValidRows.Where(r => string.IsNullOrWhiteSpace(r.CertificationAgencyName)).ToList())
            {
                AddErrorRow(row, "Certification Agency Name cannot be empty.");
            }
        }

        private void CheckForDuplicateNames()
        {
            var duplicateRows = ValidRows.GroupBy(r => r.CertificationAgencyName)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            foreach (var row in duplicateRows)
            {
                AddErrorRow(row, string.Format("The name {0} appears multiple times on the spreadsheet", row.CertificationAgencyName));
            }
        }

        private void CheckForDuplicateIds()
        {
            var duplicateRows = ValidRows.Where(r => r.CertificationAgencyId != "0")
                .GroupBy(r => r.CertificationAgencyId)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            foreach (var row in duplicateRows)
            {
                AddErrorRow(row, string.Format("The ID {0} appears multiple times on the spreadsheet", row.CertificationAgencyId));
            }
        }

        private void CheckForNonExistingIds(List<CertificationAgencyModel> existingAgencies)
        {
            var invalidRows = ValidRows
                .Where(r => r.CertificationAgencyId != "0" && !existingAgencies.Any(a => a.HierarchyClassId.ToString() == r.CertificationAgencyId))
                .ToList();

            foreach (var row in invalidRows)
            {
                AddErrorRow(row, string.Format("The ID {0} does not exist or is not associated to a Certification Agency.", row.CertificationAgencyId));
            }
        }

        private void CheckForAlreadyExistingCertificationAgencies(List<CertificationAgencyModel> existingAgencies)
        {
            var newAgencies = ValidRows.Where(r => r.CertificationAgencyId == "0").ToList();

            if(newAgencies.Any())
            {
                foreach (var duplicateAgency in newAgencies.Where(na => existingAgencies.Any(a => a.HierarchyClassName == na.CertificationAgencyName)).ToList())
                {
                    AddErrorRow(duplicateAgency, string.Format("{0} already exists.", duplicateAgency.CertificationAgencyName));
                }
            }
        }

        private void CheckForInvalidGlutenFree()
        {
            foreach (var row in ValidRows.Where(r => !IsValidBoolean(r.GlutenFree)).ToList())
            {
                AddErrorRow(row, string.Format("{0} is not a valid value for Gluten Free. Values must be Y or N.", row.GlutenFree));
            }
        }

        private void CheckForInvalidKosher()
        {
            foreach (var row in ValidRows.Where(r => !IsValidBoolean(r.Kosher)).ToList())
            {
                AddErrorRow(row, string.Format("{0} is not a valid value for Kosher. Values must be Y or N.", row.Kosher));
            }
        }

        private void CheckForInvalidNonGmo()
        {
            foreach (var row in ValidRows.Where(r => !IsValidBoolean(r.NonGmo)).ToList())
            {
                AddErrorRow(row, string.Format("{0} is not a valid value for Non-GMO. Values must be Y or N.", row.NonGmo));
            }
        }

        private void CheckForInvalidOrganic()
        {
            foreach (var row in ValidRows.Where(r => !IsValidBoolean(r.Organic)).ToList())
            {
                AddErrorRow(row, string.Format("{0} is not a valid value for Organic. Values must be Y or N.", row.Organic));
            }
        }

        private void CheckForInvalidVegan()
        {
            foreach (var row in ValidRows.Where(r => !IsValidBoolean(r.Vegan)).ToList())
            {
                AddErrorRow(row, string.Format("{0} is not a valid value for Vegan. Values must be Y or N.", row.Vegan));
            }
        }

        private void CheckThatAgencyTraitCanBeRemoved()
        {
            if (ValidRows.Any())
            {
                var glutenFreeAgencies = ValidRows.Where(r => r.GlutenFree == "0" && r.CertificationAgencyId != "0");
                var kosherAgencies = ValidRows.Where(r => r.Kosher == "0" && r.CertificationAgencyId != "0");
                var nonGmoAgencies = ValidRows.Where(r => r.NonGmo == "0" && r.CertificationAgencyId != "0");
                var organicAgencies = ValidRows.Where(r => r.Organic == "0" && r.CertificationAgencyId != "0");
                var veganAgencies = ValidRows.Where(r => r.Vegan == "0" && r.CertificationAgencyId != "0");

                if(glutenFreeAgencies.Any())
                {
                    var invalidGluteFreeIds = getCertificationAgencyIdsAssociatedToItemsQueryHandler.Search(new GetCertificationAgencyIdsAssociatedToItemsParameters
                        {
                            HierarchyClassIds = glutenFreeAgencies.Select(r => int.Parse(r.CertificationAgencyId)).ToList(),
                            TraitId = Traits.GlutenFree
                        });

                    if (invalidGluteFreeIds.Any())
                    {
                        var invalidAgencies = ValidRows.Where(r => invalidGluteFreeIds.Contains(int.Parse(r.CertificationAgencyId))).ToList();
                        foreach (var invalidAgency in invalidAgencies)
                        {
                            AddErrorRow(invalidAgency, String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", TraitDescriptions.GlutenFree, invalidAgency.CertificationAgencyName));
                        }
                    }
                }
                if(kosherAgencies.Any())
                {
                    var invalidKosherIds = getCertificationAgencyIdsAssociatedToItemsQueryHandler.Search(new GetCertificationAgencyIdsAssociatedToItemsParameters
                    {
                        HierarchyClassIds = kosherAgencies.Select(r => int.Parse(r.CertificationAgencyId)).ToList(),
                        TraitId = Traits.Kosher
                    });

                    if (invalidKosherIds.Any())
                    {
                        var invalidAgencies = ValidRows.Where(r => invalidKosherIds.Contains(int.Parse(r.CertificationAgencyId))).ToList();
                        foreach (var invalidAgency in invalidAgencies)
                        {
                            AddErrorRow(invalidAgency, String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", TraitDescriptions.Kosher, invalidAgency.CertificationAgencyName));
                        }
                    }
                }
                if(nonGmoAgencies.Any())
                {
                    var invalidNonGmoIds = getCertificationAgencyIdsAssociatedToItemsQueryHandler.Search(new GetCertificationAgencyIdsAssociatedToItemsParameters
                    {
                        HierarchyClassIds = nonGmoAgencies.Select(r => int.Parse(r.CertificationAgencyId)).ToList(),
                        TraitId = Traits.NonGmo
                    });

                    if (invalidNonGmoIds.Any())
                    {
                        var invalidAgencies = ValidRows.Where(r => invalidNonGmoIds.Contains(int.Parse(r.CertificationAgencyId))).ToList();
                        foreach (var invalidAgency in invalidAgencies)
                        {
                            AddErrorRow(invalidAgency, String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", TraitDescriptions.NonGmo, invalidAgency.CertificationAgencyName));
                        }
                    }
                }
                if(organicAgencies.Any())
                {
                    var invalidOrganicIds = getCertificationAgencyIdsAssociatedToItemsQueryHandler.Search(new GetCertificationAgencyIdsAssociatedToItemsParameters
                    {
                        HierarchyClassIds = organicAgencies.Select(r => int.Parse(r.CertificationAgencyId)).ToList(),
                        TraitId = Traits.Organic
                    });

                    if (invalidOrganicIds.Any())
                    {
                        var invalidAgencies = ValidRows.Where(r => invalidOrganicIds.Contains(int.Parse(r.CertificationAgencyId))).ToList();
                        foreach (var invalidAgency in invalidAgencies)
                        {
                            AddErrorRow(invalidAgency, String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", TraitDescriptions.Organic, invalidAgency.CertificationAgencyName));
                        }
                    }
                }
                if(veganAgencies.Any())
                {
                    var invalidVeganIds = getCertificationAgencyIdsAssociatedToItemsQueryHandler.Search(new GetCertificationAgencyIdsAssociatedToItemsParameters
                    {
                        HierarchyClassIds = veganAgencies.Select(r => int.Parse(r.CertificationAgencyId)).ToList(),
                        TraitId = Traits.Vegan
                    });

                    if(invalidVeganIds.Any())
                    {
                        var invalidAgencies = ValidRows.Where(r => invalidVeganIds.Contains(int.Parse(r.CertificationAgencyId))).ToList();
                        foreach (var invalidAgency in invalidAgencies)
                        {
                            AddErrorRow(invalidAgency, String.Format("The {0} trait from {1} cannot be removed since it exists as at least one item's {0} certification agency. The update is failed.", TraitDescriptions.Vegan, invalidAgency.CertificationAgencyName));
                        }
                    }
                }
            }
        }

        private bool IsValidBoolean(string cellValue)
        {
            return cellValue == "1" || cellValue == "0" || string.IsNullOrWhiteSpace(cellValue);
        }

        private void AddErrorRow(BulkImportCertificationAgencyModel model, string error)
        {
            model.Error = error;
            ErrorRows.Add(model);
            ValidRows.Remove(model);
        }
    }
}