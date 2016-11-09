
namespace PushController.Common
{
    public static class StartupOptions
    {
        public static int Instance { get; set; }
        public static string[] RegionsToProcess { get; set; }
        public static int MaxRecordsToProcess { get; set; }
    }
}
