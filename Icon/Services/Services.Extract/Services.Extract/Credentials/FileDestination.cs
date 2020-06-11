namespace Services.Extract.Credentials
{
    public class FileDestination
    {
        public FileDestination(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}