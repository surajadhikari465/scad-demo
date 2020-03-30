using System;
using System.Data.SqlClient;

namespace BulkItemUploadProcessor.Common.Builder
{
    public class ErrorMessageBuilder : IErrorMessageBuilder
    {
        private const string UpdateError = "Error in updating data.";
        private const string InvalidHierarchyError = "The hierarchy value selected does not exist in Icon.";
        private const string DuplicateScanCodeError = "Item with this scan code already exists in Icon.";

        public string BuildErrorMessage(Exception error)
        {
            switch (error.GetType().ToString())

            {
                case "System.Data.SqlClient.SqlException":
                    return HandleSqlException((SqlException)error);
                default:
                    return error.Message;
            }
        }

        private string HandleSqlException(SqlException error)
        {
            switch (error.Number)
            {
                case 547: // Foreign Key violation
                    if (error.Message.Contains("HierarchyClass"))
                        return InvalidHierarchyError;
                    else
                        return UpdateError;
                case 2627: // duplicate error
                    if (error.Message.Contains("ScanCode"))
                        return DuplicateScanCodeError;
                    else
                        return UpdateError;
                default:
                    return UpdateError;
            }
        }
    }
}
