using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Linq;

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

				ResetConfigManager();
			}
		}

		public static Hashtable GetVariables()
		{
			return ConfigurationManager.GetSection(Utility.VARIABLES) as Hashtable;
		}

		static void ResetConfigManager()
		{
			var T = typeof(ConfigurationManager);

			T.GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
			T.GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
			T.Assembly.GetTypes()
				.Where(x => x.FullName == "System.Configuration.ClientConfigPaths")
				.First()
				.GetField("s_current", BindingFlags.NonPublic | BindingFlags.Static)
				.SetValue(null, null);
			
		}
	}
}