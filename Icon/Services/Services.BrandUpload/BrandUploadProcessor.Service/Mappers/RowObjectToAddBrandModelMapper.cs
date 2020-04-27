using System;
using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Mappers.Interfaces;

namespace BrandUploadProcessor.Service.Mappers
{
    public class RowObjectToAddBrandModelMapper  : IRowObjectToAddBrandModelMapper
    {
        public RowObjectToAddBrandModelMapper()
        {
        }

        public RowObjectToBrandMapperResponse<AddBrandModel> Map(
            List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders,
            List<BrandAttributeModel> attributeModels,
            string uploadedBy)
        {

            //Set up column indexes
            var brandNameIndex = columnHeaders.First(c => c.Name == Constants.BrandNameColumnHeader).ColumnIndex;
            var brandAbbreviationIndex = columnHeaders.First(c => c.Name == Constants.BrandAbbreviationColumnHeader).ColumnIndex;
            var zipCodeIndex  = columnHeaders.First(c => c.Name == Constants.ZipCodeColumnHeader).ColumnIndex;
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
            var addBrandModels = rowObjectDictionary.Select(r =>
            {
                
                var brand= new AddBrandModel()
                {
                    BrandId = null,
                    BrandAbbreviation= Extensions.GetCellValue(brandAbbreviationIndex, r.Cells, false, Constants.RemoveExcelValue),
                    BrandName = Extensions.GetCellValue(brandNameIndex, r.Cells, false, Constants.RemoveExcelValue), 
                    ZipCode = Extensions.GetCellValue(zipCodeIndex, r.Cells, false, Constants.RemoveExcelValue), 
                    Designation = Extensions.GetCellValue(designationIndex, r.Cells, false, Constants.RemoveExcelValue),  
                    Locality = Extensions.GetCellValue(localityIndex, r.Cells, false, Constants.RemoveExcelValue),
                    ParentCompany = Extensions.GetCellValue(parentCompanyIndex, r.Cells, false, Constants.RemoveExcelValue)
                };

                return new { Brand = brand, r.Row };
            }).ToDictionary(
                i => i.Brand,
                i => i.Row);

            return new RowObjectToBrandMapperResponse<AddBrandModel>
            {
                Brands = addBrandModels.Keys.ToList(),
                BrandToRowDictionary = addBrandModels
            };
        }

    }
}