using System.IO;

namespace Services.Extract.Tests
{
    public static class TestHelper
    {
        public static void SetupWorkspace(this ExtractJobRunner runner, string workspacePath)
        {
            runner.CreateWorksapce(workspacePath);
            runner.CleanWorkspace(workspacePath);
        }

        public static FileInfo CreateTestFile(string workspacePath)
        {
            var testfile = workspacePath + @"\test.txt";
            File.Create(testfile).Close();
            return new FileInfo(testfile);
        }
        public static FileInfo CreateLargeTestFile(string workspacePath)
        {
            var testfile = workspacePath + @"\test.txt";
            var fs = new FileStream(testfile, FileMode.CreateNew);
            fs.Seek(2048L * 1024 * 1024, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
            return new FileInfo(testfile);
        }

       
    }
}