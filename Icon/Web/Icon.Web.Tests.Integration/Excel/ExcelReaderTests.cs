using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Excel;

namespace Icon.Web.Tests.Unit.Excel
{
    [TestClass] //[Ignore]
    public class ExcelReaderTests
    {
        List<Field> fields;
        MemoryStream inputStream;
        const string  fileName = @"TestDocs\Contact.xlsx";
        Regex emailRegex = new Regex(@"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$", RegexOptions.Compiled);

        [TestInitialize]
        public void Initilize()
        {
            this.fields = new List<Field>(new Field[]
                {
                    new Field("ContactId", typeof(int), 0, true),
                    new Field("HierarchyClassName", typeof(string), null, true),
                    new Field("Email", typeof(string), null, true, emailRegex, true),
                    new Field("ContactName", typeof(string), null, false),
                    new Field("ContactType", typeof(string), null, false),
                    new Field("Title", typeof(string), null, false),
                    new Field("AddressLine1", typeof(string), null, false),
                    new Field("AddressLine2", typeof(string), null, false),
                    new Field("City", typeof(string), null, false),
                    new Field("State", typeof(string), null, false),
                    new Field("ZipCode", typeof(string), null, false),
                    new Field("Country", typeof(string), null, false),
                    new Field("PhoneNumber1", typeof(string), null, false),
                    new Field("PhoneNumber2", typeof(string), null, false),
                    new Field("WebsiteURL", typeof(string), null, false, null, true)
                });
        }
        
        void InitilizeInputStream()
        {
            var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fStream);
            this.inputStream = new MemoryStream();
            inputStream.Write(binaryReader.ReadBytes((int)fStream.Length), 0, (int)fStream.Length);
        }

        [TestMethod]
        public void OpenExcelFile_WorksheetSpecified_ShouldOpenFile()
        {
            //Given
            InitilizeInputStream();

            //When
            using(var rdr = new ExcelReader(this.inputStream, "Contact", this.fields.ToArray()));
            
            // Then.
            // Expected excecption.
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OpenExcelFile_WorksheetNotSpecified_ExcecptionShouldBeThrown()
        {
            //Given
            InitilizeInputStream();

            //When
            using(var rdr = new ExcelReader(this.inputStream, String.Empty, this.fields.ToArray()));
            
            // Then.
            // Expected excecption.
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void OpenExcelFile_MissingWorksheet_ExcecptionShouldBeThrown()
        {
            //Given
            InitilizeInputStream();

            //When
            using(var rdr = new ExcelReader(this.inputStream, "ContactX", this.fields.ToArray()));
            
            // Then.
            // Expected excecption.
        }

        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void OpenExcelFile_RequiredFieldIsMissing_ExcecptionShouldBeThrown()
        {
            //Given
            InitilizeInputStream();
            this.fields.Add(new Field("RequiredTest", typeof(string), null, true, null, true));

            //When
            using(var rdr = new ExcelReader(this.inputStream, "Contact", this.fields.ToArray()));
            
            // Then.
            // Expected excecption.
        }

        [TestMethod]
        public void OpenExcelFile_ReadData_ShouldRead5Records()
        {
            //Given
            int actualRecordCount = 0;
            int expectedRecordCount = 5;
            InitilizeInputStream();

            //When
            using(var rdr = new ExcelReader(this.inputStream, "Contact", this.fields.ToArray()))
            {
                while(rdr.Read()){}
               actualRecordCount = rdr.RecordsAffected;
            }
            
            // Then.
            Assert.AreEqual(actualRecordCount, expectedRecordCount);
        }

        [TestMethod]
        public void OpenExcelFile_ReadData_InvalidEmailShouldBeNull()
        {
            //Given
            int emptyRecordCount = 0;
            int validRecordCount = 0;
            InitilizeInputStream();

            //When
            using(var rdr = new ExcelReader(this.inputStream, "Contact", this.fields.ToArray()))
            {
                while(rdr.Read())
                {
                    if(rdr["Email"] == null)
                    {
                       emptyRecordCount++; 
                    }
                    else
                    {
                        validRecordCount++;
                    }
                }
            }
            
            // Then.
            Assert.AreEqual(emptyRecordCount, 2, "Expecting invalid record don't match");
            Assert.AreEqual(validRecordCount, 3, "Expecting valid record don't match");
        }
    }
}
