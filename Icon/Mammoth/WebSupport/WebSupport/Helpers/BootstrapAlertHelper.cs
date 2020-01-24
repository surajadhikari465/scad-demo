namespace WebSupport.Helpers
{
    public static class BootstrapAlertHelper
    {
        public static string AlertClass(bool? isError)
        {
            string alertType = isError.HasValue && isError.Value ? "danger" : "success";
            return string.Format("alert alert-{0} alert-dismissable", alertType);
        }
    }
}