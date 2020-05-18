using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class AttributeExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private AttributeExporter exporter;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void AttributeExport_Attribute()
        {
            // Given
            List<PickListModel> pickListModels = new List<PickListModel>
            {
                new PickListModel{AttributeId=1, PickListValue="PickListValue1"},
                new PickListModel{AttributeId=2, PickListValue="PickListValue2" }
            };

            List<CharacterSetModel> characterSetModels = new List<CharacterSetModel>
            {
                new CharacterSetModel{Name="Char1", RegEx="Char1",CharacterSetId= 1},
                new CharacterSetModel{Name="Char2", RegEx="Char2",CharacterSetId = 2 }
            };
 

            var testAttribute1 = new AttributeViewModel
            { DisplayName = "Test1", AttributeName = "Test1", Description = "TEST1",PickListData= pickListModels, AvailableCharacterSets = characterSetModels, LastModifiedBy = "userTest",  LastModifiedDate = "2020/01/12 12:00:00:000" };
            var testAttribute2 = new AttributeViewModel
                {DisplayName = "Test2", AttributeName = "Test2", Description = "TEST2",PickListData = pickListModels, AvailableCharacterSets= characterSetModels, LastModifiedBy = "userTest" , LastModifiedDate = "2020/01/20 12:00:00:000" };
            var attributeExportData = new List<AttributeViewModel>();

            attributeExportData.Add(testAttribute1);
            attributeExportData.Add(testAttribute2);

            this.exporter = new AttributeExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = attributeExportData;

            // When
            this.exporter.Export();

            // Then
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(testAttribute1.DisplayName, worksheet.Rows[1].Cells[0].Value);
            Assert.AreEqual(testAttribute1.AttributeName, worksheet.Rows[1].Cells[1].Value);
            Assert.AreEqual(testAttribute1.LastModifiedBy, worksheet.Rows[1].Cells[25].Value);
            Assert.AreEqual(testAttribute1.LastModifiedDate, worksheet.Rows[1].Cells[26].Value);
            Assert.AreEqual(testAttribute2.DisplayName, worksheet.Rows[2].Cells[0].Value);
            Assert.AreEqual(testAttribute2.AttributeName, worksheet.Rows[2].Cells[1].Value);
            Assert.AreEqual(testAttribute2.LastModifiedBy, worksheet.Rows[2].Cells[25].Value);
            Assert.AreEqual(testAttribute2.LastModifiedDate, worksheet.Rows[2].Cells[26].Value);
        }


        [TestMethod]
        public void AttributeExport_AttributeWithItemCount()
        {
            // Given
            List<PickListModel> pickListModels = new List<PickListModel>
            {
                new PickListModel{AttributeId=1, PickListValue="PickListValue1"}               
            };

            List<CharacterSetModel> characterSetModels = new List<CharacterSetModel>
            {
                new CharacterSetModel{Name="Char1", RegEx="Char1",CharacterSetId= 1}               
            };

            var testAttribute = new AttributeViewModel
            { DisplayName = "Test1", AttributeName = "Test1", Description = "TEST1", PickListData = pickListModels, AvailableCharacterSets = characterSetModels,ItemCount = 2, LastModifiedBy = "userTest", LastModifiedDate = "2020/01/20 12:00:00:000" };           
            
            var attributeExportData = new List<AttributeViewModel>();

            attributeExportData.Add(testAttribute);           

            this.exporter = new AttributeExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = attributeExportData;

            // When
            this.exporter.Export();

            // Then
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(testAttribute.DisplayName, worksheet.Rows[1].Cells[0].Value);
            Assert.AreEqual(testAttribute.AttributeName, worksheet.Rows[1].Cells[1].Value);
            Assert.AreEqual(testAttribute.ItemCount, worksheet.Rows[1].Cells[23].Value);
            Assert.AreEqual(testAttribute.LastModifiedBy, worksheet.Rows[1].Cells[25].Value);
            Assert.AreEqual(testAttribute.LastModifiedDate, worksheet.Rows[1].Cells[26].Value);
        }
    }
}