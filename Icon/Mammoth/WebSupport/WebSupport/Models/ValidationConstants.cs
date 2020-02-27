namespace WebSupport.Models
{
  public static class ValidationConstants
  {
    public const string RegExNumeric = @"[0-9]+";
    public const string InvalidNumericInput = "Numeric is expected.";

    public const string RegExForValidScanCode = @"^[1-9][0-9]{0,12}(\r?\n[1-9][0-9]{0,12})*(\r?\n)*$";
    public const string RegExForValidScanCodeWithWildCard = @"(^[*]{1}$)|(^[1-9][0-9]{0,12}(\r?\n[1-9][0-9]{0,12})*(\r?\n)*$)";

    public const string ErrorMsgForInvalidScanCode = "Invalid scan code format.";
    public const string ErrorMsgForInvalidScanCodeWithWildCard = "Invalid scan code format. Enter * for ALL items";


    public const string PromptToSelectRegion = "- Select a Region";


    public const string XmlDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";


    public const string JavascriptDateTimePickerFormat = "MM/DD/YYYY hh:mm:ss A";
  }
}