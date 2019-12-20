using Dapper;
using Icon.Common;
using Icon.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpsgenieAlert;
using Renci.SshNet;
using Services.Extract.Credentials;
using Services.Extract.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

[assembly: InternalsVisibleTo("Services.Extract.Tests")]

namespace Services.Extract
{
    public class ExtractJobRunner
    {
        const string WorkspacePath = @".\Workspace";
        internal List<FileInfo> OutputFiles = new List<FileInfo>();
        internal readonly string[] Regions;
        internal IEnumerable<ExtractSourcesAndDestinationFile> ActiveSourcesAndDestinationFiles;
        internal ExtractJobConfiguration Configuration;
        internal Dictionary<string, SqlConnection> Connections;
        internal ILogger<ExtractJobRunner> Logger;
        internal IEnumerable<ExtractDataAndFileInformation> OutputData;
        internal IOpsgenieAlert OpsGenie;
        internal string OpsGenieApiKey;
        internal string OpsGenieUrl;
        private ICredentialsCacheManager CredentialsCacheManager;

        

        public ExtractJobRunner(ILogger<ExtractJobRunner> logger, IOpsgenieAlert opsGenieAlert, ICredentialsCacheManager credentialsCacheManager)
        {
            OpsGenieApiKey = AppSettingsAccessor.GetStringSetting("OpsGenieApiKey", true);
            OpsGenieUrl = AppSettingsAccessor.GetStringSetting("OpsGenieUri", true);
            CredentialsCacheManager = credentialsCacheManager;
            Regions = AppSettingsAccessor.GetStringSetting("Regions", "FL,MA,MW,NA,NC,NE,PN,RM,SO,SP,SW,UK").Split(',');
            Logger = logger;
            OpsGenie = opsGenieAlert;
            LoadConnectionConfiguration();
        }


        internal void Cleanup()
        {
            if (Connections != null)
                foreach (var sqlConnection in Connections)
                {
                    sqlConnection.Value?.Dispose();
                }

            if (ActiveSourcesAndDestinationFiles != null)
                foreach (var extractSourcesAndDestinationFile in ActiveSourcesAndDestinationFiles)
                {
                    extractSourcesAndDestinationFile.CleanUp();
                }
        }
        
    
        internal List<FileInfo> CompressFiles(List<FileInfo> inputFiles, string destinationFile)
        {
            if (!Configuration.ZipOutput) return inputFiles;
            if (destinationFile == null) return inputFiles;

            List<FileInfo> OutputFiles = new List<FileInfo>();


            if (string.IsNullOrWhiteSpace(Configuration.CompressionType)) Configuration.CompressionType = "Zip";

            switch (Configuration.CompressionType.ToLower())
            {
                case "zip":
                    OutputFiles = ZipFiles(inputFiles, destinationFile);
                    break;
                case "gzip":
                    OutputFiles = GzipFiles(inputFiles);
                    break;
                default:
                    Logger.Warn($"Unknown Compression Type [{Configuration.CompressionType}]"); 
                    break;
            }
            return OutputFiles;
        }
        internal List<FileInfo> GzipFiles(List<FileInfo> inputFiles)
        {
            var compressedFiles = new List<FileInfo>();
            foreach (var file in inputFiles)
            {
               var compressedFile=  GZipFile(file.FullName);
               compressedFiles.Add(new FileInfo(compressedFile));
            }

            return compressedFiles;
        }
        internal string GZipFile(string inputFileName)
        {
            var gzipFile = Path.ChangeExtension(inputFileName, "gz");
            var bytes = File.ReadAllBytes(inputFileName);
            using (var fs = new FileStream(gzipFile, FileMode.CreateNew))
            using (var zipStream = new GZipStream(fs, CompressionMode.Compress, false))
            {
                zipStream.Write(bytes, 0, bytes.Length);
            }

            return gzipFile;
        }
        internal List<FileInfo> ZipFiles(List<FileInfo> inputFiles, string destinationFile)
        {
            destinationFile = Path.ChangeExtension(destinationFile, ".zip");
            if (File.Exists(destinationFile)) File.Delete(destinationFile);

            using (ZipArchive zip = ZipFile.Open(destinationFile, ZipArchiveMode.Create))
            {
                foreach (var input in inputFiles)
                {
                    zip.CreateEntryFromFile(input.FullName, input.Name);
                }
            }

            return new List<FileInfo> {new FileInfo(destinationFile)};
        }
        internal void CopyFile(string sourceFileName, string destFileName, bool overwrite)
        {
            var destDir = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            File.Copy(sourceFileName, destFileName, overwrite);
        }
        internal List<FileInfo> ConcatenateFiles(string destinationFile, List<FileInfo> sourceFiles)
        {
            if (!destinationFile.StartsWith(WorkspacePath)) destinationFile = WorkspacePath + @"\" + destinationFile;
            if (!Configuration.ConcatenateOutputFiles) return sourceFiles;
            if (sourceFiles.Count == 1) return sourceFiles;

            var destinationFileInfo = new FileInfo(destinationFile);
            using (var output = File.Create(destinationFileInfo.FullName))
            {
                foreach (var file in sourceFiles)
                {
                    using (var input = File.OpenRead(file.FullName))
                    {
                        Console.WriteLine($"adding {file.FullName} to {destinationFileInfo.FullName}");
                        input.CopyTo(output);
                    }
                }
            }

            foreach (var sourceFile in sourceFiles)
            {
                sourceFile.Delete();
            }

            return new List<FileInfo> {destinationFileInfo};
        }

        
        internal void CreateWorksapce(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        internal void CleanWorkspace(string path)
        {
            var workspaceDirectory = new DirectoryInfo(path);
            foreach (var file in workspaceDirectory.GetFiles())
            {
                file.Delete();
            }
        }

        
        internal string DynamicToConcatenatedHeaderString(dynamic data, string delimiter, bool includeLineFeed)
        {
            var jObject = (JObject) JToken.FromObject(data);
            var output = string.Join(delimiter,
                jObject.ToObject<Dictionary<string, string>>().Select(x1 => x1.Key.RemoveChar(delimiter)));

            if (includeLineFeed) output += Environment.NewLine;
            return output;
        }
        internal string DynamicToConcatenatedValuesString(dynamic data, string delimiter, bool includeLineFeed, bool boolToInt)
        {
            var jObject = (JObject) JToken.FromObject(data);
            var output = string.Join(delimiter,jObject.ToObject<Dictionary<string, string>>().Select(x1 => x1.Value.RemoveChar(delimiter).BoolToInt(boolToInt)));
            if (includeLineFeed) output += Environment.NewLine;
            return output;
        }


        
        internal void  ExecuteStoredProcedure(ExtractSourcesAndDestinationFile[] connections, string sql, ExtractJobParameter[] parameters, out IEnumerable<ExtractDataAndFileInformation> extractDataAndFileInformation)
        {
            var outputDataAndFileInformation = new List<ExtractDataAndFileInformation>();
            var dbArgs = new DynamicParameters();
            if (parameters != null)
                foreach (var extractJobParameter in parameters)
                {
                    dbArgs.Add(extractJobParameter.Key, extractJobParameter.Value);
                }

            foreach (var connection in connections)
            {
                outputDataAndFileInformation.Add(new ExtractDataAndFileInformation()
                {
                    Data = connection.Source.Query(sql, dbArgs, buffered: false, commandTimeout:3000),
                    FileInformation = new FileInfo(connection.DestinationFile)
                });
            }

            extractDataAndFileInformation =  outputDataAndFileInformation;
        }
        internal void  GetActiveSourceConnectionsAndDestinationFiles(out IEnumerable<ExtractSourcesAndDestinationFile> extractSourcesAndDestinationFiles)
        {
            
            var activeConnections = new List<ExtractSourcesAndDestinationFile>();

            var irmaSourcesAndFiles = from c in Connections
                let nameWithoutExtension = Path.GetFileNameWithoutExtension(Configuration.OutputFileName)
                let extension = Path.GetExtension(Configuration.OutputFileName)
                let region = c.Key
                let regionalFilename = $"{nameWithoutExtension}_{region}{extension}"
                    where Configuration.Regions.Contains(region)
                && Configuration.Source.ToLower() == "irma"
                    select new ExtractSourcesAndDestinationFile
                {
                    Source = c.Value,
                    DestinationFile = WorkspacePath + @"\" + regionalFilename
                };

            activeConnections.AddRange(irmaSourcesAndFiles);

            var iconSourcesAndFiles =
                from c in Connections
            where c.Key.ToLower() == "icon" && Configuration.Source.ToLower() == "icon"
            select new ExtractSourcesAndDestinationFile
            {
                Source = c.Value,
                DestinationFile = WorkspacePath + @"\" + Configuration.OutputFileName
            };

            activeConnections.AddRange(iconSourcesAndFiles);

            extractSourcesAndDestinationFiles= activeConnections;
        }
        internal FileInfo GetHeaders(IEnumerable<ExtractDataAndFileInformation> Data)
        {
            var firstDataSet = Data.FirstOrDefault();
            if (firstDataSet == null) return null;


            var firstRow = firstDataSet.Data.FirstOrDefault();
            if (firstRow == null) return null;

            var headerFile = new FileInfo(WorkspacePath + @"\headers.txt");
            if (!headerFile.Exists)
            {
                var headers = DynamicToConcatenatedHeaderString(firstRow, Configuration.Delimiter, true);
                File.AppendAllText(headerFile.FullName, string.Join(Configuration.Delimiter, headers));
            }

            return headerFile;
        }
        internal void LoadConnectionConfiguration()
        {
            Connections = (from c in ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().Where(w => w.Name.StartsWith("Irma_"))
                    let region = c.Name.Substring(c.Name.Length - 2, 2)
                    select new
                    {
                        Source = region,
                        Conn = new SqlConnection(c.ConnectionString)
                    })
                .ToDictionary(d => d.Source, d => d.Conn);

            Connections.Add("Icon", new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString));


        }
        internal void ProcessHeaders(List<FileInfo> sourceFiles)
        {
            if (!Configuration.IncludeHeaders) return;

            var headersFile = WorkspacePath + @"\headers.txt";
            string tempFilename;


            if (Configuration.ConcatenateOutputFiles)
            {
                FileStream headerStream;
                //add headers to first file
                var firstFile = sourceFiles.FirstOrDefault();
                if (firstFile != null)
                {
                    if (File.Exists(headersFile))
                    {
                        headerStream = File.OpenRead(headersFile);
                        tempFilename = Path.ChangeExtension(firstFile.FullName, ".tmp");
                        using (var tempOutput = File.OpenWrite(tempFilename))
                        {
                            using (var fileInput = File.OpenRead(firstFile.FullName))
                            {
                                headerStream.CopyTo(tempOutput);
                                fileInput.CopyTo(tempOutput);
                            }
                        }

                        File.Delete(firstFile.FullName);
                        File.Move(tempFilename, firstFile.FullName);
                        headerStream?.Close();
                        headerStream?.Dispose();
                    }
                }
            }
            else
            {
                //add headers to each file
                if (File.Exists(headersFile))
                {
                    FileStream headerStream = File.OpenRead(headersFile);

                    foreach (var outputFile in OutputFiles)
                    {
                        tempFilename = Path.ChangeExtension(outputFile.FullName, ".tmp");
                        using (var tempOutput = File.OpenWrite(tempFilename))
                        {
                            using (var fileInput = File.OpenRead(outputFile.FullName))
                            {
                                headerStream.CopyTo(tempOutput);
                                fileInput.CopyTo(tempOutput);
                            }
                        }

                        File.Delete(outputFile.FullName);
                        File.Move(tempFilename, outputFile.FullName);
                    }

                    headerStream?.Close();
                    headerStream?.Dispose();
                }
            }
        }
        public void Run(ExtractJobConfiguration configuration)
        {
            try
            {

                Configuration = configuration;
                CredentialsCacheManager.S3CredentialsCache.Refresh();
                CredentialsCacheManager.SFtpCredentialsCache.Refresh();

                CreateWorksapce(WorkspacePath);
                CleanWorkspace(WorkspacePath);
                TransformFilenames();
                GetActiveSourceConnectionsAndDestinationFiles(out ActiveSourcesAndDestinationFiles);
                ExecuteStoredProcedure(ActiveSourcesAndDestinationFiles.ToArray(), Configuration.Query,
                    Configuration.Parameters, out OutputData);
                GetHeaders(OutputData);
                OutputFiles.AddRange(WriteDataToFiles(OutputData));
                ProcessHeaders(OutputFiles);

                OutputFiles = ConcatenateFiles(Configuration.OutputFileName, OutputFiles);
                OutputFiles = CompressFiles(OutputFiles,
                    WorkspacePath + @"\" + configuration.OutputFileName.Replace("{source}", Configuration.Source));

                ProcessDestination();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                var data = new Dictionary<string, string> {{"Job", JsonConvert.SerializeObject(configuration)}};

                if (AppSettingsAccessor.GetBoolSetting("SendOpsGenieAlerts"))
                    OpsGenie.CreateOpsgenieAlert(OpsGenieApiKey, OpsGenieUrl, ex.Message, "SCAD Extract Service Alert", data);
            }
            finally
            {
                Cleanup();
            }
        }

        internal void SetConfiguration(ExtractJobConfiguration config)
        {
            Configuration = config;
        }
        internal void TransformFilenames()
        {
            Configuration.OutputFileName = Configuration.OutputFileName.TransformSource(Configuration.Source);
            Configuration.OutputFileName  =Configuration.OutputFileName.TransformDateTimeStamp();
        }

        internal IEnumerable<FileInfo> WriteDataToFiles(IEnumerable<ExtractDataAndFileInformation> dataAndFileInfo)
        {
            var files = dataAndFileInfo.Select(d => d.FileInformation);
            var outputWriters = new Dictionary<string, StreamWriter>();

            foreach (var file in dataAndFileInfo)
            {
                outputWriters.Add(file.FileInformation.Name, new StreamWriter(file.FileInformation.FullName));
            }

            Parallel.ForEach(dataAndFileInfo, new ParallelOptions() {MaxDegreeOfParallelism = 6}, currentFile =>
            {
                using (var sw = outputWriters[currentFile.FileInformation.Name])
                {
                    foreach (var row in currentFile.Data)
                    {
                        var x = DynamicToConcatenatedValuesString(row, "|", false, true);
                        sw.WriteLine(x);
                    }
                }
            });

            return files;
        }
        

        internal void CopyFilesToSFtp(string credentialKey, ISFtpCredentialsCache credentialsCache, string destinationDir, List<FileInfo> files)
        {
            if (!credentialsCache.Credentials.ContainsKey(credentialKey)) throw new Exception($"Unable to find SFtp Credentials for Key: {credentialKey}");
            var credentials = credentialsCache.Credentials[credentialKey];
            if (credentials == null) throw new Exception($"Expected to find SFtp Credentials for Key: {credentialKey} but they were null");
            
            var connectionInfo = new ConnectionInfo(credentials.Host,credentials.Username,new PasswordAuthenticationMethod(credentials.Username, credentials.Password));
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();

                client.ChangeDirectory(destinationDir);

                foreach (var file in files)
                {
                    using (var fStream = File.OpenRead(file.FullName))
                    {
                        client.UploadFile(fStream, file.Name);
                        fStream.Close();
                    } 
                }
            }
        }
        internal void CopyFilesToS3(string credentialKey, IS3CredentialsCache credentialsCache, string destinationDir, List<FileInfo> files)
        {

            if (!credentialsCache.Credentials.ContainsKey(credentialKey)) throw new Exception($"Unable to find S3 Credentials for Key: {credentialKey}");
            var credentials = credentialsCache.Credentials[credentialKey];
            if (credentials == null) throw new Exception($"Expected to find S3 Credentials for Key: {credentialKey} but they were null");

            using (var client = new AmazonS3Client(credentials.AccessKey,credentials.SecretKey,new AmazonS3Config() {ServiceURL = credentials.BucketRegion}))
            {
                foreach (var fileInfo in files)
                {
                    PutObjectRequest request = new PutObjectRequest
                    {
                        BucketName = credentials.BucketName,
                        Key = $"{destinationDir}/{Path.GetFileName(fileInfo.Name)}",
                        FilePath = fileInfo.FullName
                    };

                    var response = client.PutObject(request);
                }
            }
        }

        

        
        private void ProcessDestination()
        {
            if (Configuration.Destination == null)
            {
                Logger.Warn("No destination found in extract configuration. Skipping.");
                return;
            }

            switch (Configuration.Destination.Type.ToLower())
            {
                case "s3":
                    CopyFilesToS3(Configuration.Destination.CredentialsKey, CredentialsCacheManager.S3CredentialsCache, Configuration.Destination.Path, OutputFiles);
                    break;
                case "file":
                    OutputFiles.ForEach(of => CopyFile(sourceFileName: of.FullName, destFileName:Configuration.Destination.Path + of.Name, overwrite:true));
                    break;
                case "sftp":
                    CopyFilesToSFtp(Configuration.Destination.CredentialsKey, CredentialsCacheManager.SFtpCredentialsCache, Configuration.Destination.Path, OutputFiles);
                    break;
                default:
                    Logger.Warn($"Unknown Destination Type [{Configuration.Destination.Type}]");
                    break;
            }
        }
    }
}