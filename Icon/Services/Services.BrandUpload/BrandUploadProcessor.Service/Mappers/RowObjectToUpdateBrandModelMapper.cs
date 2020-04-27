using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Mappers.Interfaces;

namespace BrandUploadProcessor.Service.Mappers
{
    public class RowObjectToUpdateBrandModelMapper : IRowObjectToUpdateBrandModelMapper
    {
        public RowObjectToUpdateBrandModelMapper()
        {
        }

        public RowObjectToBrandMapperResponse<UpdateBrandModel> Map(
            List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders,
            List<BrandAttributeModel> attributeModels,
            string uploadedBy)
        {

            //Set up column indexes
            var brandIdIndex = columnHeaders.First(c => c.Name == Constants.BrandIdColumnHeader).ColumnIndex;
            var brandNameIndex = columnHeaders.First(c => c.Name == Constants.BrandNameColumnHeader).ColumnIndex;
            var brandAbbreviationIndex = columnHeaders.First(c => c.Name == Constants.BrandAbbreviationColumnHeader).ColumnIndex;
            var zipCodeIndex = columnHeaders.First(c => c.Name == Constants.ZipCodeColumnHeader).ColumnIndex;
            var designationIndex = columnHeaders.First(c => c.Name == Constants.DesignationColumnHeader).ColumnIndex;
            var localityIndex = columnHeaders.First(c => c.Name == Constants.LocalityColumnHeader).ColumnIndex;
            var parentCompanyIndex = columnHeaders.First(c => c.Name == Constants.ParentCompanyColumnHeader).ColumnIndex;


            //Convert RowObjects into addBrandModels
            var rowObjectDictionary = rowObjects.Select(r => new
            {
                Row = r,
                Cells = r.Cells.ToDictionary(
                    c => c.Column.ColumnIndex,
                    c => c.CellValue)
            });
            var updateBrandModels = rowObjectDictionary.Select(r =>
            {
                var brand = new UpdateBrandModel
                {
                    BrandId = int.Parse(r.Cells[brandIdIndex]),
                    BrandAbbreviation = Extensions.GetCellValue(brandAbbreviationIndex, r.Cells, false, Constants.RemoveExcelValue),
                    BrandName = Extensions.GetCellValue(brandNameIndex, r.Cells, true, Constants.RemoveExcelValue),
                    ZipCode = Extensions.GetCellValue(zipCodeIndex, r.Cells, true, Constants.RemoveExcelValue),
                    Designation = Extensions.GetCellValue(designationIndex, r.Cells, true, Constants.RemoveExcelValue),
                    Locality = Extensions.GetCellValue(localityIndex, r.Cells, true, Constants.RemoveExcelValue),
                    ParentCompany = Extensions.GetCellValue(parentCompanyIndex, r.Cells, true, Constants.RemoveExcelValue),
                };
                return new { Brand = brand, r.Row };
            }).ToDictionary(
                i => i.Brand,
                i => i.Row);

            return new RowObjectToBrandMapperResponse<UpdateBrandModel>
            {
                Brands = updateBrandModels.Keys.ToList(),
                BrandToRowDictionary = updateBrandModels
            };
        }
     
    }
}