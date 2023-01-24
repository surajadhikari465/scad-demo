namespace ErrorMessagesMonitor.Model
{
    internal class ErrorMessageModel
    {
        public string Application { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorSeverity { get; set; }
        public int? NumberOfErrors { get; set; }
    }
}
