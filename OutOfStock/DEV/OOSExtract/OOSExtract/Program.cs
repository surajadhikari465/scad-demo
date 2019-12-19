using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using CommandLine;
using CsvHelper;
using Renci.SshNet;
using Serilog;
using Serilog.Events;

namespace OOSExtract
{
    internal class Options
    {
        [Option('d', "daysback", HelpText = "Number of days of data to extract. Starting from now.", Required = true)]
        public int DaysBack { get; set; }

        [Option('i', "includeMissingDetails", HelpText = "Include Scans Missing VIM Detalis")]
        public bool IncludeScansMissingDetails { get; set; }

        [Option('f', "useftp", Default = false, HelpText = "Send to Ted's Ftp")]
        public bool UseFtp { get; set; }

        [Option('s', "useS3", Default = false, HelpText = "Send to S3 Bucket")]
        public bool UseS3 { get; set; }
    }

    class Program
    {
        private const string BasePath = @".\Files\";
        private static string connectionStringTemplate;
        private static string serverName;
        private static  string connectionString;

        private const string AccessKey = "AKIAI4ZFUHNVCWA3UUKA";
        private const string SecretKey = "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";
        private const string KmsKey = "3a847b00-0d76-4f40-bec2-1f2f18b0381b";
        private const string BucketName = "oos-staging-dev";


        static void Main(string[] args)
        {

            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(ExecuteJob)
                .WithNotParsed(errs => { });
        }


        private static void ExecuteJob(Options options)
        {

            connectionStringTemplate = ConfigurationManager.ConnectionStrings["oosConnection"].ConnectionString;
            serverName = ConfigurationManager.AppSettings["DBServerName"].ToString();
            connectionString = string.Format(connectionStringTemplate, serverName);


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(@".\logs\log.txt", LogEventLevel.Verbose, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null, 5120000L, null, false, false, new TimeSpan?(), RollingInterval.Day, true)
                .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", null, null, new LogEventLevel?()).CreateLogger();

            Log.Logger.Information("{@options}", options);
            Log.Logger.Information("Database Server: {serverName}", serverName);

            var d = DateTime.Today.ToString("yyyMMdd");
            var infn = d + "_OOSExtract.csv";
            var outfn = d;
            if (options.DaysBack == 1)
                outfn += "_daily";
            outfn += "_OOSExtract.zip";
            try
            {
                var oosData = ExtractOosData(options.DaysBack, options.IncludeScansMissingDetails);

                
                var csv = WriteToCsv(infn, oosData);
                CompressData(infn, outfn, csv);
                if (options.UseFtp)
                    SendToFtp(outfn);

                if (options.UseS3)
                    SendToS3(outfn);


                switch (options.DaysBack)
                {
                    case 1:
                        MoveToDaily(outfn);
                        break;
                    case 8:
                        MoveToWeekly(outfn);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Failed...");
                Log.Logger.Error(ex.Message);
                if (ex.InnerException != null)
                    Log.Logger.Error(ex.InnerException.Message);
                Log.Logger.Error(ex.StackTrace);
            }


        }

        private static void SendToS3(string outfn)
        {

            Log.Logger.Information("s3 Upload...");

            var uploader = new S3Uploader(AccessKey, SecretKey, KmsKey);
            
            uploader.UploadFile(File.OpenRead(outfn), BucketName, outfn);
        }

        private static void MoveToDaily(string outfn)
        {
            var path = $@"{BasePath}Daily\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (File.Exists($"{path}{outfn}"))
                File.Delete($"{path}{outfn}");
            File.Move(outfn, $"{path}{outfn}");
        }

        private static void MoveToWeekly(string outfn)
        {
            var path = $@"{BasePath}Weekly\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (File.Exists($"{path}{outfn}"))
                File.Delete($"{path}{outfn}");
            File.Move(outfn, $"{path}{outfn}");
        }


        private static void SendToFtp(string outfn)
        {
            Log.Logger.Debug("Ftp ...");
            using (var sftp = new SftpClient(ConfigurationManager.AppSettings["sftphost"],
                                ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]))
            {
                sftp.Connect();
                using (var file = File.OpenRead(outfn))
                {
                    sftp.ChangeDirectory("/home");
                    sftp.UploadFile(file, outfn);
                }
                sftp.Disconnect();
            }
        }

        private static void CompressData(string infn, string outfn, string data)
        {
            Log.Logger.Debug("Compressing ...");
            using (var fileStream = new FileStream(outfn, FileMode.OpenOrCreate))
            {
                using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    using (var streamWriter = new StreamWriter(zipArchive.CreateEntry(infn).Open()))
                        streamWriter.Write(data);
                }
            }
            File.Delete(infn);
        }


        private static DataTable ExtractOosData(int daysback, bool includeScansMissingData)
        {
            if (daysback > 0) daysback = daysback *  -1;

            Log.Logger.Debug("Extracting ...");

            var query = "dbo.outofstockextract";

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var selectCommand = new SqlCommand())
                {
                    selectCommand.Connection = sqlConnection;
                    selectCommand.CommandTimeout = 600;
                    selectCommand.CommandText = query;
                    selectCommand.CommandType = CommandType.StoredProcedure;

                    selectCommand.Parameters.AddWithValue("DaysBack", daysback);
                    selectCommand.Parameters.AddWithValue("IncludeScansMissingDetails", includeScansMissingData);


                    using (var sqlDataAdapter = new SqlDataAdapter(selectCommand))
                        sqlDataAdapter.Fill(dataTable);
                    sqlConnection.Close();
                }
            }
            return dataTable;
        }

        private static string WriteToCsv(string infn, DataTable dt)
        {
            Log.Logger.Debug($"Writing {dt.Rows.Count} rows...");
            var cnt = 0;
            using (var text = File.CreateText(infn))
            {
                using (var csvWriter = new CsvWriter(text))
                {
                    foreach (DataColumn column in dt.Columns)
                        csvWriter.WriteField(column.ColumnName);
                    csvWriter.NextRecord();
                    foreach (DataRow row in dt.Rows)
                    {
                        cnt++;
                        for (var index = 0; index < dt.Columns.Count; ++index)
                            csvWriter.WriteField(row[index]);
                        csvWriter.NextRecord();
                    }
                }
                Log.Logger.Debug($"Wrote {cnt} rows...");
            }
            return File.ReadAllText(infn);
        }
    }
}
    

