using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Icon.Web.Mvc.Excel
{
    public class ExcelReader : IDataReader
    {
        SheetData wsData;
        SpreadsheetDocument doc;
        String[] stringValues;
        IEnumerator<Row> rowEnumerator;
        ParallelOptions parallelOpt = new ParallelOptions() { MaxDegreeOfParallelism = 4 };

        public int Depth => 0;
        public int FieldCount => Fields == null ? -1 : Fields.Length;
        public int RowIndex => rowEnumerator == null || rowEnumerator.Current == null ? -1 : (int)rowEnumerator.Current.RowIndex.Value;
        public bool IsClosed => doc == null;
        public bool IsEmpty { get; private set; }
        public int RecordsAffected{ get; private set; }
        public object this[int i] => GetValue(i);
        public object this[string name] => GetValue(GetOrdinal(name));
        public Field[] Fields { get; private set; }
        public string SourceTable { get; set; }
        public string FileName { get; private set; }

        public ExcelReader(Stream inputStream)
        {
            this.doc = SpreadsheetDocument.Open(inputStream, true);
        }

        public ExcelReader(Stream inputStream, string sourceWorksheet, Field[] fields)
        {
            if (String.IsNullOrWhiteSpace(sourceWorksheet))
            {
                throw new Exception("Source worksheet is not specified");
            }

            if (fields == null || fields.Length == 0)
            {
                throw new Exception($"Fields are not specified");
            }

            if (fields.Select(x => x.Name.ToLower()).GroupBy(x => x).Any(x => x.Count() > 1))
            {
                throw new Exception("Duplicate fields name found.");
            }

            IsEmpty = false;
            this.Fields = fields;
            this.SourceTable = sourceWorksheet;
            this.doc = SpreadsheetDocument.Open(inputStream, true);

            var sheet = doc.WorkbookPart.Workbook.Sheets.Cast<Sheet>().Where(x => x.Name == this.SourceTable).FirstOrDefault();
            if (sheet == null)
            {
                throw new Exception($"Excel file does not have a required worksheet {sourceWorksheet}");
            }

            var workSheet = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
            this.wsData = workSheet.Worksheet.Elements<SheetData>().FirstOrDefault();
            this.rowEnumerator = wsData.Elements<Row>().GetEnumerator();
            IsEmpty = wsData.Elements<Row>().Count() < 2;

            if(!IsEmpty)
            {
                int id;
                this.rowEnumerator.MoveNext();
                var headerRow = this.rowEnumerator.Current;
                this.stringValues = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.Select(x => x.InnerText).ToArray();

                for (int i = 0; i < headerRow.Elements<Cell>().Count(); i++)
                {
                    var c = (Cell)headerRow.ElementAt(i);
                    if (c.DataType != null && c.DataType == CellValues.SharedString)
                    {
                        if (int.TryParse(c.InnerText, out id))
                        {
                            var name = this.stringValues[id];
                            var key = name.Trim().Replace(" ", String.Empty);

                            var f = fields.Where(x => String.Compare(key, x.Name, StringComparison.InvariantCultureIgnoreCase) == 0).FirstOrDefault();
                            if (f != null)
                            {
                                if (f.Index > -1)
                                {
                                    throw new Exception($"Duplicate column in header: {name}");
                                }
                                else
                                {
                                    f.Index = i;
                                    f.CellRefPattern = new Regex($"^{ToExcelColumn(i + 1)}[0-9]+$", RegexOptions.Compiled); //^[A-Z]+[0-9]+$, ^AB[0-9]+$
                                }
                            }
                        }

                        if (!fields.Any(x => x.Index < 0)) break;
                    }
                }

                if(fields.Any(x => x.IsRequired && x.Index < 0))
                {
                    Close();
                    throw new MissingFieldException($"Missing fields: { String.Join(", ", fields.Where(x => x.IsRequired && x.Index < 0).Select(x => x.Name)) }");
                }
            }
        }

        public bool GetBoolean(int i) { return (bool)GetValue(i); }
        public byte GetByte(int i) { return (byte)GetValue(i); }
        public char GetChar(int i) { return (char)GetValue(i); }
        public DateTime GetDateTime(int i) { return (DateTime)GetValue(i); }
        public decimal GetDecimal(int i) { return (decimal)GetValue(i); }
        public double GetDouble(int i) { return (double)GetValue(i); }
        public Type GetFieldType(int i) { return Fields[i].fldType.GetType(); }
        public float GetFloat(int i) { return (float)GetValue(i); }
        public Guid GetGuid(int i) { return (Guid)GetValue(i); }
        public short GetInt16(int i) { return (short)GetValue(i); }
        public int GetInt32(int i) { return (int)GetValue(i); }
        public long GetInt64(int i) { return (long)GetValue(i); }
        public string GetName(int i) { return Fields[i].Name; }

        public IDataReader GetData(int i) { throw new NotImplementedException(); }
        public string GetDataTypeName(int i) { throw new NotImplementedException(); }
        public long GetBytes(int i, long fldOffset, byte[] buf, int bufoffset, int length) { throw new NotImplementedException(); }
        public long GetChars(int i, long fldoffset, char[] buf, int bufoffset, int length) { throw new NotImplementedException(); }

        public int GetOrdinal(string name)
        {
            var f = Fields.Where(x => String.Compare(x.Name, name, true) == 0).FirstOrDefault();
            return f == null ? -1 : Array.IndexOf(Fields, f);
        }

        public string GetString(int i) { return GetValue(i).ToString(); }
        public object GetValue(int i) { return Fields[i].Value; }
        public object GetValue(string fldName)
        {
            var f = Fields.Where(x => String.Compare(x.Name, fldName, true) == 0).FirstOrDefault();
            if (f == null) throw new MissingFieldException(String.Format("Inavalid field name({0})", fldName));
            return f.Value;
        }

        public int GetValues(object[] values)
        {
            Array.Copy(Fields.Select(x => x.Value).ToArray(), values, FieldCount);
            return FieldCount;
        }

        public bool IsDBNull(int i) { return GetValue(i) == null || GetValue(i) == DBNull.Value; }
        public DataTable GetSchemaTable() { throw new NotImplementedException(); }
        public bool NextResult() { return IsClosed; }

        public void Close()
        {
            this.stringValues = null;
            if(this.rowEnumerator != null) this.rowEnumerator.Dispose();
            this.wsData = null;
            doc = null;
        }

        public void Dispose() { Close(); }

        bool IsValidRecord(Cell[] cells )
        {
            try
            {
                Parallel.ForEach(Fields, parallelOpt, (fld) => { fld.TryParse(ref cells, ref this.stringValues); });
                return Fields.Any(x => !x.IsDefault); //Skip record if all fields have default values 
            }
            catch { return false; }
        }

        public bool Read()
        {
            if (IsEmpty) return false;

            while (this.rowEnumerator.MoveNext())
            {
                this.RecordsAffected++;
                if (IsValidRecord(rowEnumerator.Current.Elements<Cell>().ToArray())) return true;
            }

            return false;
        }

        private Row GetHeader(params string[] values)
        {
            Row row = new Row();
            foreach(var val in values)
            {
                row.AppendChild(new Cell(){ CellValue = new CellValue(val), DataType = CellValues.String });
            }
            return row;
        }

        public void SetErrorLinks(Hyperlinks links, string validationTableName)
        {
            WorksheetPart wsPart = null;
            var sheet = doc.WorkbookPart.Workbook.Sheets.Cast<Sheet>()
                .Where(x => String.Compare(x.Name, validationTableName, true) == 0)
                .FirstOrDefault();

            if(sheet != null)
            {
                wsPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                sheet.Remove();
                doc.WorkbookPart.DeletePart(wsPart);
                wsPart = null;
            }

            var ws = new Worksheet();
            var sheetData = new SheetData();
            wsPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();

            var columns = new DocumentFormat.OpenXml.Spreadsheet.Columns();
            columns.Append(new Column(){ Min = 1, Max = 1, Width = 20, CustomWidth = true});
            columns.Append(new Column(){ Min = 2, Max = 2, Width = 100, CustomWidth = true});
            ws.Append(columns);

            sheetData.AppendChild(GetHeader("Ref Link", "Validation Message"));
            foreach (Hyperlink hl in links)
            {
                var run = new Run()
                {
                    RunProperties = new RunProperties(new Color() { Rgb = new HexBinaryValue() { Value = "0000FF" } }, new Underline() { Val = UnderlineValues.Single }),
                    Text = new Text(hl.Display)
                };
                
                var r = new Row(new Cell(new InlineString(run)) { DataType = CellValues.InlineString },
                                new Cell() { CellValue = new CellValue(hl.Tooltip), DataType = CellValues.String });                
                
                sheetData.AppendChild(r);
            }

            ws.Append(sheetData, links);
            wsPart.Worksheet = ws;

            var sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = doc.WorkbookPart.GetIdOfPart(wsPart);

            //Get a unique ID for the new worksheet.
            uint sheetId = sheets.Elements<Sheet>().Count() > 0
                ? sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1
                : 1;

            //Append the new worksheet and associate it with the workbook.
            var wsValidation = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = validationTableName };
            sheets.Append(wsValidation);
            doc.Save();
        }
        public void SetErrorLinks(Hyperlinks links, string validationTableName, List<int> rowIds)
        {
            SetErrorLinks(links, "ItemsValidation");

            var sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            IEnumerable<Sheet> sheets1 = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == "Items");
            if (sheets1.Any())
            {
                string relationshipId1 = sheets1?.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(relationshipId1);
                SheetData sheetData1 = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                var allRows = sheetData1.Elements<Row>().Where(a => a.RowIndex > 1).OrderBy(a => a.RowIndex.Value).ToArray();
                var errorRows = sheetData1.Elements<Row>().Where(r => rowIds.Contains((int)r.RowIndex.Value)).OrderBy(a => a.RowIndex.Value).ToArray();

                //Delete all records
                for (int x = 0; x < allRows.Count(); x++)
                {
                    ((Row)allRows[x]).Remove();
                }
                worksheetPart.Worksheet.Save();

                //Insert bad records only
                uint rowid = 1;
                foreach (var r in errorRows)
                {
                    r.RowIndex.Value = ++rowid;
                    int inx = 0;

                    foreach (Cell c in r.Elements<Cell>().ToArray())
                    {
                        c.CellReference.Value = String.Format("{0}{1}", ToExcelColumn(++inx), rowid);
                    }
                    sheetData1.Append(r);
                }
                worksheetPart.Worksheet.Save();
            }
            doc.Save();
        }
        private string ToExcelColumn(int inx)
        {
            int mod;
            var name = String.Empty;

            while (inx > 0)
            {
                mod = (inx - 1) % 26;
                name = String.Format("{0}{1}", Convert.ToChar('A' + mod).ToString(), name);
                inx = (int)((inx - mod) / 26);
            }

            return name;
        }
    }
}