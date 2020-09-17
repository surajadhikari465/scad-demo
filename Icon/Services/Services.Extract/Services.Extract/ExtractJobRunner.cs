using Amazon.S3;
using Amazon.S3.Model;
using Dapper;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Producer;
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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TIBCO.EMS;

[assembly: InternalsVisibleTo("Services.Extract.Tests")]

namespace Services.Extract
{
    public class ExtractJobRunner : IExtractJobRunner
    {
        const string WorkspacePath = @".\Workspace";
        internal List<FileInfo> OutputFiles = new List<FileInfo>();
        internal readonly string[] Regions;
        internal IEnumerable<ExtractSourcesAndDestinationFile> ActiveSourcesAndDestinationFiles;
        internal ExtractJobConfiguration Configuration;
        internal Dictionary<string, SqlConnection> Connections;
        internal ILogger<ExtractJobRunner> Logger;
        internal IEnumerable<ExtractJobParameter> DynamicParam;
        internal IEnumerable<ExtractDataAndFileInformation> OutputData;
        internal IOpsgenieAlert OpsGenie;
        internal string OpsGenieApiKey;
        internal string OpsGenieUrl;
        private ICredentialsCacheManager CredentialsCacheManager;
        private IFileDestinationCache FileDestinationCache;

        public string JobName { get; set; }

        public ExtractJobRunner(string jobName, ILogger<ExtractJobRunner> logger, IOpsgenieAlert opsGenieAlert, ICredentialsCacheManager credentialsCacheManager, IFileDestinationCache fileDestinationCache)
        {
            OpsGenieApiKey = AppSettingsAccessor.GetStringSetting("OpsGenieApiKey", true);
            OpsGenieUrl = AppSettingsAccessor.GetStringSetting("OpsGenieUri", true);
            CredentialsCacheManager = credentialsCacheManager;
            FileDestinationCache = fileDestinationCache;
            Regions = AppSettingsAccessor.GetStringSetting("Regions", "FL,MA,MW,NA,NC,NE,PN,RM,SO,SP,SW,UK").Split(',');
            JobName = jobName;
            Logger = logger;
            OpsGenie = opsGenieAlert;
            LoadConnectionConfiguration();
        }

        public void Run(ExtractJobConfiguration configuration)
        {
            try
            {
                Logger.Info($"{JobName}: Refreshing CredentialsCache");
                Configuration = configuration;
                CredentialsCacheManager.S3CredentialsCache.Refresh();
                CredentialsCacheManager.SFtpCredentialsCache.Refresh();
                CredentialsCacheManager.EsbCredentialsCache.Refresh();
                FileDestinationCache.Refresh();

                Logger.Info($"{JobName}: Creating Workspace");
                CreateWorkspace(WorkspacePath);
                CleanWorkspace(WorkspacePath);
                Logger.Info($"{JobName}: Transforming Filenames");
                TransformFilenames();
                GetActiveSourceConnectionsAndDestinationFiles(out ActiveSourcesAndDestinationFiles);
                if (Configuration.StagingQuery != null && Configuration.StagingQuery.Trim().Length > 0)
                {
                    Logger.Info($"{JobName}: Executing Staging Query");
                    ExecuteStagingQuery(ActiveSourcesAndDestinationFiles.ToArray(), Configuration.StagingQuery,
                    Configuration.StagingParameters);
                }

                if (Configuration.DynamicParameterQuery != null && Configuration.DynamicParameterQuery.Trim().Length > 0)
                {
                    Logger.Info($"{JobName}: Executing Dynamic Parameters Query");
                    if (ActiveSourcesAndDestinationFiles.Count() > 1) throw new Exception($"Only one database connection is allowed if DynamicParameterQuery is specified.");

                    LoadDynamicParameters(ActiveSourcesAndDestinationFiles.ToArray(), Configuration.DynamicParameterQuery,
                    out DynamicParam);
                }

                Logger.Info($"{JobName}: Executing Extract Query");
                ExecuteExtractQuery(ActiveSourcesAndDestinationFiles.ToArray(), Configuration.Query,
                        Configuration.Parameters, DynamicParam, out OutputData);

                Logger.Info($"{JobName}: Getting Headers");
                GetHeaders(OutputData);

                Logger.Info($"{JobName}: Writing the output files to workspace");
                OutputFiles.AddRange(WriteDataToFiles(OutputData, ActiveSourcesAndDestinationFiles.Count()));
                ProcessHeaders(OutputFiles);

                OutputFiles = ConcatenateFiles(Configuration.OutputFileName, OutputFiles);
                OutputFiles = CompressFiles(OutputFiles,
                    WorkspacePath + @"\" + configuration.OutputFileName.Replace("{source}", Configuration.Source));

                Logger.Info($"{JobName}: Sending to destination");
                ProcessDestination();
            }
            catch (Exception ex)
            {
                Logger.Error($"Job Failed: {JobName}: {ex}");
                var data = new Dictionary<string, string> { { "Job", JsonConvert.SerializeObject(configuration) } };

                if (AppSettingsAccessor.GetBoolSetting("SendOpsGenieAlerts"))
                    OpsGenie.CreateOpsgenieAlert(OpsGenieApiKey, OpsGenieUrl, ex.Message, "SCAD Extract Service Alert", data);
            }
            finally
            {
                Cleanup();
            }
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
                case "7-zip":
                    OutputFiles = SevenZipFiles(inputFiles);
                    break;
                default:
                    Logger.Warn($"Unknown Compression Type [{Configuration.CompressionType}]");
                    break;
            }
            return OutputFiles;
        }

        private List<FileInfo> SevenZipFiles(List<FileInfo> inputFiles)
        {
            if (string.IsNullOrWhiteSpace(Configuration.SevenZipArchiveType))
            {
                throw new ArgumentException("7-Zip archive type is not set and compression type is set to 7-Zip.");
            }
            if (!Constants.SevenZipArchiveTypes.Contains(Configuration.SevenZipArchiveType))
            {
                throw new ArgumentException($"7-Zip archive type {Configuration.SevenZipArchiveType} is an unsupported archive type for 7-Zip compression.");
            }
            var compressedFiles = new List<FileInfo>();
            foreach (var inputFile in inputFiles)
            {
                string sourceName = inputFile.FullName;
                string targetName = Path.ChangeExtension(inputFile.FullName, ".gz");

                ProcessStartInfo p = new ProcessStartInfo();
                p.FileName = ".\\7Zip\\7za.exe";

                p.Arguments = $"a -t{Configuration.SevenZipArchiveType} \"{targetName}\" \"{sourceName}\" -mx=9";
                p.WindowStyle = ProcessWindowStyle.Hidden;

                Process x = Process.Start(p);
                x.WaitForExit();
                compressedFiles.Add(new FileInfo(targetName));
            }
            return compressedFiles;
        }

        internal List<FileInfo> GzipFiles(List<FileInfo> inputFiles)
        {
            var compressedFiles = new List<FileInfo>();
            foreach (var file in inputFiles)
            {
                var compressedFile = GZipFile(file.FullName);
                compressedFiles.Add(new FileInfo(compressedFile));
            }

            return compressedFiles;
        }

        internal string GZipFile(string inputFileName)
        {
            var gzipFile = Path.ChangeExtension(inputFileName, "gz");
            // changing from System.IO.File.ReadAllBytes() to System.IO.FileStream.CopyTo() because ReadAllBytes doesnt support files over 2gigs.
            using (FileStream target = new FileStream(gzipFile, FileMode.Create, FileAccess.Write))
            using (GZipStream zipStream = new GZipStream(target, CompressionMode.Compress, leaveOpen: false))
            {
                using (var fileToRead = File.Open(inputFileName, FileMode.Open))
                {
                    fileToRead.CopyTo(zipStream);
                }
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

            return new List<FileInfo> { new FileInfo(destinationFile) };
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

            return new List<FileInfo> { destinationFileInfo };
        }

        internal void CreateWorkspace(string path)
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
            var jObject = (JObject)JToken.FromObject(data);
            var output = string.Join(delimiter,
                jObject.ToObject<Dictionary<string, string>>().Select(x1 => x1.Key.RemoveChar(delimiter)));

            if (includeLineFeed) output += Environment.NewLine;
            return output;
        }

        internal string DynamicToConcatenatedValuesString(dynamic data, string delimiter, bool includeLineFeed, bool boolToInt)
        {
            var jObject = (JObject)JToken.FromObject(data);
            var output = string.Join(delimiter, jObject.ToObject<Dictionary<string, string>>().Select(x1 => x1.Value.RemoveChar(delimiter).BoolToInt(boolToInt)));
            if (includeLineFeed) output += Environment.NewLine;
            return output;
        }

        internal void LoadDynamicParameters(ExtractSourcesAndDestinationFile[] connections, string sql, out IEnumerable<ExtractJobParameter> outPutParam)
        {
            var outputList = new List<dynamic>();
            var param = new List<ExtractJobParameter>();

            foreach (var connection in connections)
            {
                outputList.Add(connection.Source.Query(sql, buffered: false, commandTimeout: 7200));
            }

            Parallel.ForEach(outputList, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, output =>
            {
                foreach (var row in output)
                {
                    param.Add(new ExtractJobParameter
                    {
                        Key = row.Key,
                        Value = row.Value
                    });
                }
            });

            outPutParam = param;
        }

        internal void ExecuteStagingQuery(ExtractSourcesAndDestinationFile[] connections, string sql, ExtractJobParameter[] parameters)
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
                connection.Source.Query(sql, dbArgs, buffered: false, commandTimeout: 7200).ToList();
            }
        }

        internal void ExecuteExtractQuery(ExtractSourcesAndDestinationFile[] connections, string sql, ExtractJobParameter[] parameters, IEnumerable<ExtractJobParameter> dynamicParameters, out IEnumerable<ExtractDataAndFileInformation> extractDataAndFileInformation)
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
                if (dynamicParameters != null && dynamicParameters.Count() > 0)
                {
                    foreach (var dynamicParameter in dynamicParameters)
                    {
                        var destinationFile = connection.DestinationFile.Contains(dynamicParameter.Key.ToString()) ? connection.DestinationFile.Replace(('{' + dynamicParameter.Key.ToString() + '}'), dynamicParameter.Value.ToString()) : connection.DestinationFile;

                        var combinedArgs = new DynamicParameters();
                        if (parameters != null && parameters.Count() > 0)
                        {
                            combinedArgs = dbArgs;
                            combinedArgs.Add(dynamicParameter.Key, dynamicParameter.Value);
                        }
                        else
                        {
                            combinedArgs.Add(dynamicParameter.Key, dynamicParameter.Value);
                        }

                        Logger.Info($"{JobName}: Executing Extract Query with dynamic arguments");
                        outputDataAndFileInformation.Add(new ExtractDataAndFileInformation()
                        {
                            Data = connection.Source.Query(sql, combinedArgs, buffered: false, commandTimeout: 7200),
                            FileInformation = new FileInfo(destinationFile)
                        });
                    }
                }
                else
                {
                    Logger.Info($"{JobName}: Executing Extract Query without dynamic arguments");
                    outputDataAndFileInformation.Add(new ExtractDataAndFileInformation()
                    {
                        Data = connection.Source.Query(sql, dbArgs, buffered: false, commandTimeout: 7200),
                        FileInformation = new FileInfo(connection.DestinationFile)
                    });
                }
            }

            extractDataAndFileInformation = outputDataAndFileInformation;
        }

        internal void GetActiveSourceConnectionsAndDestinationFiles(out IEnumerable<ExtractSourcesAndDestinationFile> extractSourcesAndDestinationFiles)
        {
            var activeConnections = new List<ExtractSourcesAndDestinationFile>();

            if (Configuration.Regions != null)
            {
                var irmaSourcesAndFiles = from c in Connections
                                          let nameWithoutExtension = Path.GetFileNameWithoutExtension(Configuration.OutputFileName)
                                          let extension = Path.GetExtension(Configuration.OutputFileName)
                                          let region = c.Key
                                          let regionalFilename = nameWithoutExtension.Contains("{region}")
                                              ? nameWithoutExtension.Replace("{region}", $"{region}") + extension
                                              : $"{nameWithoutExtension}_{region}{extension}"
                                          where Configuration.Regions.Contains(region)
                                                && Configuration.Source.ToLower() == "irma"
                                          select new ExtractSourcesAndDestinationFile
                                          {
                                              Source = c.Value,
                                              DestinationFile = WorkspacePath + @"\" + regionalFilename
                                          };

                activeConnections.AddRange(irmaSourcesAndFiles);
            }

            var iconSourcesAndFiles =
                from c in Connections
                where c.Key.ToLower() == "icon" && Configuration.Source.ToLower() == "icon"
                select new ExtractSourcesAndDestinationFile
                {
                    Source = c.Value,
                    DestinationFile = WorkspacePath + @"\" + Configuration.OutputFileName
                };
            activeConnections.AddRange(iconSourcesAndFiles);

            var mammothSourcesAndFiles =
                from c in Connections
                where c.Key.ToLower() == "mammoth" && Configuration.Source.ToLower() == "mammoth"
                select new ExtractSourcesAndDestinationFile
                {
                    Source = c.Value,
                    DestinationFile = WorkspacePath + @"\" + Configuration.OutputFileName
                };
            activeConnections.AddRange(mammothSourcesAndFiles);

            extractSourcesAndDestinationFiles = activeConnections;
        }

        internal FileInfo GetHeaders(IEnumerable<ExtractDataAndFileInformation> Data)
        {
            if (string.IsNullOrWhiteSpace(Configuration.Delimiter))
            {
                Configuration.Delimiter = "|";
            }
            dynamic firstRow = null;

            foreach (var singleDateSet in Data)
            {
                if (singleDateSet != null)
                {
                    firstRow = singleDateSet.Data.FirstOrDefault();
                    if (firstRow != null) break;
                }
            }

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
            Connections.Add("Mammoth", new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
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
                                headerStream.Position = 0;
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

        internal void SetConfiguration(ExtractJobConfiguration config)
        {
            Configuration = config;
        }

        internal void TransformFilenames()
        {
            Configuration.OutputFileName = Configuration.OutputFileName.TransformSource(Configuration.Source);
            Configuration.OutputFileName = Configuration.OutputFileName.TransformDateTimeStamp();
        }

        internal IEnumerable<FileInfo> WriteDataToFiles(IEnumerable<ExtractDataAndFileInformation> dataAndFileInfo, int connectionCounter)
        {
            if (string.IsNullOrWhiteSpace(Configuration.Delimiter))
            {
                Configuration.Delimiter = "|";
            }
            var files = dataAndFileInfo.Select(d => d.FileInformation);
            var outputWriters = new Dictionary<string, StreamWriter>();

            foreach (var file in dataAndFileInfo)
            {
                outputWriters.Add(file.FileInformation.Name, new StreamWriter(file.FileInformation.FullName));
            }

            if (connectionCounter > 1)
            {
                Parallel.ForEach(dataAndFileInfo, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, currentFile =>
                {
                    using (var sw = outputWriters[currentFile.FileInformation.Name])
                    {
                        foreach (var row in currentFile.Data)
                        {
                            var x = DynamicToConcatenatedValuesString(row, Configuration.Delimiter, false, true);
                            sw.WriteLine(x);
                        }
                    }
                });
            }
            else
            {
                foreach (var currentFile in dataAndFileInfo)
                    using (var sw = outputWriters[currentFile.FileInformation.Name])
                    {
                        foreach (var row in currentFile.Data)
                        {
                            var x = DynamicToConcatenatedValuesString(row, Configuration.Delimiter, false, true);
                            sw.WriteLine(x);
                        }
                    }
            }

            return files;
        }

        internal void CopyFilesToSFtp(string credentialKey, ISFtpCredentialsCache credentialsCache, string destinationDir, List<FileInfo> files)
        {
            if (!credentialsCache.Credentials.ContainsKey(credentialKey)) throw new Exception($"Unable to find SFtp Credentials for Key: {credentialKey}");
            var credentials = credentialsCache.Credentials[credentialKey];
            if (credentials == null) throw new Exception($"Expected to find SFtp Credentials for Key: {credentialKey} but they were null");

            var connectionInfo = new ConnectionInfo(credentials.Host, credentials.Username, new PasswordAuthenticationMethod(credentials.Username, credentials.Password));
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

        internal void CopyFilesToEsb(string credentialKey, IEsbCredentialsCache credentialsCache, List<FileInfo> files)
        {
            if (!credentialsCache.Credentials.ContainsKey(credentialKey)) throw new Exception($"Unable to find ESB Credentials for Key: {credentialKey}");
            var credentials = credentialsCache.Credentials[credentialKey];
            if (credentials == null) throw new Exception($"Expected to find ESB Credentials for Key: {credentialKey} but they were null");

            using (var producer = new EsbProducer(new EsbConnectionSettings()
            {
                ConnectionFactoryName = credentials.ConnectionFactoryName,
                CertificateStoreName = credentials.CertificateStoreName.ConvertToEnum<StoreName>(),
                CertificateName = credentials.CertificateName,
                TargetHostName = credentials.TargetHostName,
                SessionMode = credentials.SessionMode.ConvertToEnum<SessionMode>(),
                QueueName = credentials.QueueName,
                DestinationType = credentials.DestinationType,
                JmsUsername = credentials.JmsUsername,
                JmsPassword = credentials.JmsPassword,
                SslPassword = credentials.SslPassword,
                JndiUsername = credentials.JndiUsername,
                JndiPassword = credentials.JndiPassword,
                ServerUrl = credentials.ServerUrl,
                CertificateStoreLocation = credentials.CertificateStoreLocation.ConvertToEnum<StoreLocation>(),
                ReconnectDelay = String.IsNullOrEmpty(credentials.ReconnectDelay) ? 0 : int.Parse(credentials.ReconnectDelay)
            }))
            {
                var computedClientId = $"IconExtractService-{credentialKey}.{Environment.MachineName}.{Guid.NewGuid().ToString()}";
                var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

                producer.OpenConnection(clientId);

                foreach (var file in files)
                {
                    string dynamicParamKey = "{" + DynamicParam.FirstOrDefault().Key + "}";
                    string transactionId = credentials.TransactionId;

                    if (credentials.TransactionId.Contains(dynamicParamKey))
                    {
                        foreach (var value in DynamicParam.Select(v => v.Value))
                        {
                            if (file.Name.Contains(value.ToString()))
                            {
                                transactionId = credentials.TransactionId.Replace(dynamicParamKey, value.ToString());
                                break;
                            }
                        }
                    }
                    byte[] bytes = File.ReadAllBytes(file.FullName);
                    var messageProperties = new Dictionary<string, string>
                    {
                        { "TransactionType", credentials.TransactionType },
                        { "TransactionID", transactionId.TransformDateTimeStamp() },
                        { "FileName", file.Name },
                        { "Source", Configuration.Source.ToUpper() },
                        { "MessageType", credentials.MessageType }
                    };

                    producer.Send(bytes, clientId, messageProperties);
                }
            }
        }

        internal void CopyFilesToS3(string credentialKey, IS3CredentialsCache credentialsCache, string destinationDir, List<FileInfo> files)
        {
            if (!credentialsCache.Credentials.ContainsKey(credentialKey)) throw new Exception($"Unable to find S3 Credentials for Key: {credentialKey}");
            var credentials = credentialsCache.Credentials[credentialKey];
            if (credentials == null) throw new Exception($"Expected to find S3 Credentials for Key: {credentialKey} but they were null");

            using (var client = new AmazonS3Client(credentials.AccessKey, credentials.SecretKey, new AmazonS3Config() { ServiceURL = credentials.BucketRegion }))
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

        internal void CopyFilesToStoredDestination(string destinationPathKey, IFileDestinationCache fileDestinationCache, List<FileInfo> outputFiles)
        {
            if (!fileDestinationCache.FileDestinations.ContainsKey(destinationPathKey)) throw new Exception($"Unable to find destination path for Key: {destinationPathKey}");
            var destinationPath = fileDestinationCache.FileDestinations[destinationPathKey].Path;

            OutputFiles.ForEach(of => CopyFile(sourceFileName: of.FullName, destFileName: destinationPath + of.Name, overwrite: true));
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
                    OutputFiles.ForEach(of => CopyFile(sourceFileName: of.FullName, destFileName: Configuration.Destination.Path + of.Name, overwrite: true));
                    break;
                case "pathkey":
                    CopyFilesToStoredDestination(Configuration.Destination.PathKey, FileDestinationCache, OutputFiles);
                    break;
                case "sftp":
                    CopyFilesToSFtp(Configuration.Destination.CredentialsKey, CredentialsCacheManager.SFtpCredentialsCache, Configuration.Destination.Path, OutputFiles);
                    break;
                case "esb":
                    CopyFilesToEsb(Configuration.Destination.CredentialsKey, CredentialsCacheManager.EsbCredentialsCache, OutputFiles);
                    break;
                default:
                    Logger.Warn($"Unknown Destination Type [{Configuration.Destination.Type}]");
                    break;
            }
        }
    }
}
