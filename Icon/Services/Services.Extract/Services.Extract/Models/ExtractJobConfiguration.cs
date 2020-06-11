namespace Services.Extract.Models
{
    public class ExtractJobConfiguration
    {
        public bool ZipOutput { get; set; }
        public string CompressionType { get; set; }
        public bool ConcatenateOutputFiles { get; set; }
        public bool IncludeHeaders { get; set; }
        public string Source { get; set; }
        public string[] Regions { get; set; }
        public string StagingQuery { get; set; }
        public ExtractJobParameter[] StagingParameters { get; set; }
        public string DynamicParameterQuery { get; set; }
        public string Query { get; set; }
        public ExtractJobParameter[] Parameters { get; set; }
        public string OutputFileName { get; set; }
        public string Delimiter { get; set; }
        public Destination Destination { get; set; }

    }
}