using CommandLine;

namespace AmazonLoad.IconHierarchy
{
    partial class Program
    {
        public class CommandLineOptions
        {
            [Option('h', "HierarchyName", Required = false, Default = "", HelpText = "Choose a hierarchy name: Financial, National, Merchandise, Brands")]
            public string HierarchyName { get; set; }
            [Option('m', "MaxRows", Required = false, Default = -1, HelpText = "Max number of rows to process.")]
            public int MaxNumberOfRows { get; set; }
            [Option('s', "SaveMessagesToDisk", Required = false, Default = "notset", HelpText = "Save messages to disk [true or false]")]
            public string SaveMessages { get; set; }
            [Option('e', "SendMessagesToEsb", Required = false, Default = "notset", HelpText = "Send messages to ESB [true or false]")]
            public string SendMessagesToEsb { get; set; }


        }
    }
}