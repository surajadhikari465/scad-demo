using Icon.Common;

namespace Icon.Web.Common
{
    public class AppSettings
    {
        public string[] HierarchyClassAddEventConfiguredRegions => AppSettingsAccessor.GetStringSetting("HierarchyClassAddEventConfiguredRegions").Split(',');
        public string[] HierarchyClassUpdateEventConfiguredRegions => AppSettingsAccessor.GetStringSetting("HierarchyClassUpdateEventConfiguredRegions").Split(',');
        public string[] HierarchyClassDeleteEventConfiguredRegions => AppSettingsAccessor.GetStringSetting("HierarchyClassDeleteEventConfiguredRegions").Split(',');
    }
}
