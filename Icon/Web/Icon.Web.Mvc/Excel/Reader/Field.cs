using System;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Icon.Web.Mvc.Excel
{
    public class Field
    {
        const string sngl_space = " ";
        object internalVal;
        bool isRemoveSpaces;
        bool isConversionRequired;
        Regex validationRgx = null;
        Regex rgxSpace = new Regex(@"\s+", RegexOptions.Compiled);
        Regex rgxNRT = new Regex(@"[\n\r\t]", RegexOptions.Compiled); //New Line & Tab

        internal object Value => internalVal ?? Default;
        internal bool IsDefault { get; private set; }
        public string Name { get; private set; }    //Field Name
        public Type fldType { get; private set; }   //Field Type
        public object Default { get; private set; } //Default value
        public bool IsRequired { get; private set; }
        public int Index { get; internal set; }
        public Regex CellRefPattern { get; internal set; }

        public Field(string fldName, Type fldType, object dflt, bool isRequired = false, Regex validationRegex = null, bool removeSpaces = false)
        {
            Index = -1;
            Name = fldName;
            this.fldType = fldType;
            Default = dflt;
            IsRequired = isRequired;
            isRemoveSpaces = removeSpaces;
            validationRgx = validationRegex;
            isConversionRequired = fldType != typeof(string);
        }

        internal void TryParse(ref Cell[] cells, ref string[] stringValues)
        {
            internalVal = null;

            try
            { 
                var refCell = cells?.FirstOrDefault(x => this.CellRefPattern.IsMatch(x.CellReference.Value));
                IsDefault = (refCell == null || refCell.CellValue == null);

                if(IsDefault)
                {
                    return;
                }

                string val = null;
                if(refCell.DataType == null)
                {
                    val = refCell.CellValue.InnerText?.Trim();
                }
                else if(refCell.DataType == CellValues.SharedString)
                {
                    int id;
                    val = int.TryParse(refCell.InnerText, out id)
                        ? rgxSpace.Replace(rgxNRT.Replace(stringValues[id], sngl_space), sngl_space).Trim()
                        : null;
                }

                IsDefault = String.IsNullOrWhiteSpace(val);
                if(!IsDefault)
                {
                    if(isRemoveSpaces)
                    {
                        val = val.Replace(sngl_space, String.Empty);
                    }

                    if(validationRgx == null || validationRgx.IsMatch(val))
                    {
                        internalVal = isConversionRequired ? Convert.ChangeType(val, fldType) : val;
                        IsDefault = (internalVal == null);
                    }
                }
            }
            catch
            {
               internalVal = null;
               IsDefault = true;
            }
        }
    }
}