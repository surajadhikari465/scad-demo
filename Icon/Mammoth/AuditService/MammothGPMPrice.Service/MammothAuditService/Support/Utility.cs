using System.Collections;
using System.Configuration;
using System.Reflection;

namespace Audit
{
	public class Utility
	{
		const string IS_ENCRYPTED = "isEncrypted";
		public const string VARIABLES = "Variables";
		public const string SQL_CONNECTIONS = "connectionStrings";

		static bool IsConfigEncrypted()
		{
			try
			{
				bool isEncrypt = false;
				return bool.TryParse(ConfigurationManager.AppSettings[IS_ENCRYPTED], out isEncrypt);
			}
			catch { return false; }
		}

		public static void EncryptAppSettings(params string[] sections)
		{
			if(IsConfigEncrypted())
			{
				foreach(var section in sections)
				{
					var mods = Assembly.GetExecutingAssembly().GetModules();
					var cnfg = ConfigurationManager.OpenExeConfiguration(mods[0].FullyQualifiedName);
					var settings = cnfg.GetSection(section);

					if(settings != null && !settings.SectionInformation.IsProtected)
					{
						settings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
						settings.SectionInformation.ForceSave = true;
						cnfg.Save(ConfigurationSaveMode.Modified);
					}
				}
			}
		}

		public static Hashtable GetVariables()
		{
			return ConfigurationManager.GetSection(Utility.VARIABLES) as Hashtable;
		}
	}
}