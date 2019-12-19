using System.Data.SqlClient;

namespace Services.Extract.Models
{
    public class ExtractSourcesAndDestinationFile
    {
        public SqlConnection Source { get; set; }
        public string DestinationFile { get; set; }

        public void CleanUp()
        {
            Source?.Dispose();
        }
    }
}