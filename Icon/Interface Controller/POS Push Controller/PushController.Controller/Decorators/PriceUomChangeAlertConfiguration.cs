using Icon.Common;

namespace PushController.Controller.Decorators
{
    public class PriceUomChangeAlertConfiguration : IPriceUomChangeConfiguration
    {
        public string PriceUomChangeSubject { get; set; }
        public string PriceUomChangeRecipients { get; set; }
        public bool SendEmails { get; set; }

        public PriceUomChangeAlertConfiguration()
        {
            PriceUomChangeSubject = AppSettingsAccessor.GetStringSetting("PriceUomChangeSubject", required: true);
            PriceUomChangeRecipients = AppSettingsAccessor.GetStringSetting("PriceUomChangeRecipients", required: true);
            SendEmails = AppSettingsAccessor.GetBoolSetting("SendUomChangeEmails", required: false);
        }
    }
}
