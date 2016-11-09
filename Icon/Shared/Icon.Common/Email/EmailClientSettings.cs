using System;
using System.Configuration;
using System.Linq;

namespace Icon.Common.Email
{
    public class EmailClientSettings
    {
        public bool SendEmails { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string[] Recipients { get; set; }

        public void LoadFromConfig()
        {
            SendEmails = AppSettingsAccessor.GetBoolSetting("SendEmails");
            Host = AppSettingsAccessor.GetStringSetting("EmailHost");
            Port = AppSettingsAccessor.GetIntSetting("EmailPort");
            Username = AppSettingsAccessor.GetStringSetting("EmailUsername", false);
            Password = AppSettingsAccessor.GetStringSetting("EmailPassword", false);
            Sender = AppSettingsAccessor.GetStringSetting("Sender");
            Recipients = AppSettingsAccessor.GetStringSetting("Recipients").Split(',')
                .Select(s => s.Trim())
                .ToArray();
        }

        public static EmailClientSettings CreateFromConfig()
        {
            var settings = new EmailClientSettings();
            settings.LoadFromConfig();

            return settings;
        }

        public static EmailClientSettings CreateFromConfigForRegion(string regionAbbreviation)
        {
            var settings = new EmailClientSettings();
            settings.LoadFromConfig();
            settings.Recipients = AppSettingsAccessor.GetStringSetting(String.Format("{0}_Recipients", regionAbbreviation))
                .Split(',')
                .Select(s => s.Trim())
                .ToArray();
            return settings;
        }
    }
}
