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

       
    }
}