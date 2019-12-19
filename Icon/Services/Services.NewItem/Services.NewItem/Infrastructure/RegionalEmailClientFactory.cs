using Icon.Common;
using Icon.Common.Email;
using System;

namespace Services.NewItem.Infrastructure
{
    public class RegionalEmailClientFactory : IRegionalEmailClientFactory
    {
        private const string RegionalNotificationRecipientKeyPrefix = "RegionalNotificationRecipient_";

        public IEmailClient CreateEmailClient(string regionCode)
        {
            var emailRecipient = AppSettingsAccessor.GetStringSetting(RegionalNotificationRecipientKeyPrefix + regionCode, false);

            if (!string.IsNullOrWhiteSpace(emailRecipient))
            {
                EmailClientSettings settings = EmailClientSettings.CreateFromConfig();
                settings.Recipients = new[] { emailRecipient };

                return new EmailClient(settings);
            }
            else
            {
                throw new ArgumentException($"No regional email recipient was configured for region '{regionCode}'.", nameof(regionCode));
            }
        }
    }
}
