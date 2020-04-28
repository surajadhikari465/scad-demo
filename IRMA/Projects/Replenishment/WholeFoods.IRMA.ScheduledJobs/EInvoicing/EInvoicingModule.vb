Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility
Imports System.IO
Imports Renci.SshNet
Imports Renci.SshNet.Sftp
Imports System.Configuration
Imports WholeFoods.IRMA.Replenishment
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
Imports System.Net.Mail
Imports WholeFoods.Utility.SMTP

Imports log4net

Module EInvoicingModule
    '########################################################
    ' Update History
    '
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 11684
    ' Tom Lux
    ' 2010.01.19
    ' The processing design was changed to avoid dropping/missing
    ' e-inv files that are retrieved and removed from the remote FTP
    ' server, but, for whatever reason, never processed by this job.
    ' The main culprit of unprocessed files is when the FTP code in this
    ' class hangs after the e-inv file has been successfully deleted from
    ' the FTP server.  The job hangs until Windows Scheduler kills the process
    ' and the file that was pulled down is left in the base job folder and
    ' never processed.
    '
    ' Summary of changes:
    ' 1) Files are downloaded from FTP server to 'processing' folder.
    ' 2) Converted hard-coded values to new app-setting: eInvoiceFilePurgeFilenameFilter - The filename filter applied to the archive folder
    ' when files are checked to be purged/deleted.
    ' 3) Converted hard-coded values to new app-setting: eInvoiceFilePurgeThreshold - Number of days to keep invoice files in the archive folder.
    ' 4) Before the FTP server is checked for file, any files in the 'processing' folder are processed first.
    ' 5) Moved processing of downloaded files into a new 'ImportAndArchiveFiles' sub that is called after all files are downloaded from FTP server.
    ' Previously, each file was processed/imported after it was pulled down.
    ' 6) A new "SpecialFiles" folder is used to process requests where a file or FTP-file filter is provided on the command line.
    ' This keeps things separated from the normal processing folder.  During "special" runs (when someone has passed cmd-line args),
    ' no files are processed from the main processing folder (if there were any left from previous normal run).
    ' 7) When a file passed via cmd-line, the file is copied into the 'SpecialFiles' processing folder and then processed.
    ' 8) When a filename and "/ftp" option are passed via cmd-line, the files are pulled down into the 'SpecialFiles' processing folder.
    ' 9) Consolidated archive logic into the existing ArchiveCleanup sub and sub now has purge filter and threshold args.
    '
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091
    ' Tom Lux
    ' 3/8/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' ---------------------------------------------------------------------------------------------------------------
    '#########################################################

    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _workDirectory As String
    Private _archiveDirectory As String
    Private _logsDirectory As String
    Private _errorsDirectory As String
    ' This is the working/active variable that is set using one of the next two vars (main or special folders).
    Private processingFolder As String
    Private processingMainFolder As String
    Private processingSpecialFolder As String
    Private lastRuntimeFile As String

    Sub Main(ByVal sArgs() As String)
        ' Configure logging specified in app-config file.
        log4net.Config.XmlConfigurator.Configure()

        ' Download app settings doc.
        Configuration.CreateAppSettings()

        ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
        Log4NetRuntime.ConfigureLogging()

        ' To separate each pass/run in the log file.
        logger.InfoFormat("Region: [{0}]", ConfigurationServices.AppSettings("Region").ToString())

        ' Get archive filename filter app-setting.
        Dim eInvoiceFilePurgeFilenameFilter As String = Nothing
        Try
            eInvoiceFilePurgeFilenameFilter = ConfigurationServices.AppSettings("eInvoiceFilePurgeFilenameFilter").ToString()
        Catch ex As Exception
        End Try
        If eInvoiceFilePurgeFilenameFilter Is Nothing Then
            eInvoiceFilePurgeFilenameFilter = "*.xml"
            logger.InfoFormat("Using default value of {0} for app setting 'eInvoiceFilePurgeFilenameFilter'.", eInvoiceFilePurgeFilenameFilter)
        End If

        ' Get archive threshold app-setting.
        Dim eInvoiceFilePurgeThreshold As Integer
        Try
            eInvoiceFilePurgeThreshold = CInt(ConfigurationServices.AppSettings("eInvoiceFilePurgeThreshold"))
        Catch ex As Exception
            ' Purge files older than 60 days if app setting is not defined or is not integer.
            eInvoiceFilePurgeThreshold = 60
            logger.InfoFormat("Using default value of {0} days for app setting 'eInvoiceFilePurgeThreshold'.", eInvoiceFilePurgeThreshold)
        End Try

        Try
            ' ==================================================================
            ' If Archive/Errors/Logs/Processing directory does not exist then create it.
            ' ==================================================================

            Try
                _workDirectory = ConfigurationServices.AppSettings("eInvoiceWorkingDirectory").Trim()
            Catch ex As Exception
                Throw New Exception("The eInvoiceWorkingDirectory Configuration Value has not been set.")
            End Try

            ' if trailing slash is not there, add one.
            If Not _workDirectory.EndsWith("\") Then _workDirectory += "\"
            ' append region.
            _workDirectory += ConfigurationServices.AppSettings("region") & "\"

            _archiveDirectory = _workDirectory & "Archive\"
            _logsDirectory = _workDirectory & "Logs\"
            _errorsDirectory = _workDirectory & "Errors\"
            processingFolder = Nothing
            processingMainFolder = _workDirectory & "Processing\"
            processingSpecialFolder = processingMainFolder & "SpecialFiles\"

            'lastRuntimeFile = _workDirectory & "LastRunTime.dat"

            If Not Directory.Exists(_workDirectory) Then
                Directory.CreateDirectory(_workDirectory)
            End If

            If Not Directory.Exists(_archiveDirectory) Then
                Directory.CreateDirectory(_archiveDirectory)
            End If

            If Not Directory.Exists(_logsDirectory) Then
                Directory.CreateDirectory(_logsDirectory)
            End If

            If Not Directory.Exists(_errorsDirectory) Then
                Directory.CreateDirectory(_errorsDirectory)
            End If

            If Not Directory.Exists(processingMainFolder) Then
                ' Create the main folder the special subfolder.
                Directory.CreateDirectory(processingMainFolder)
                Directory.CreateDirectory(processingSpecialFolder)
            End If

            ' for Project Jeannie, two parameters are always passed in.
            ' the argument length testing was reworked to allow for this, put preserve the ability
            ' to run it for a specific file locally or retrieve a file from FTP and process it

            If sArgs.Length = 2 Then

                ' test the second argument for ftp
                If sArgs(1).ToLower.Equals("/ftp") Then

                    ' ==================================================================
                    ' Retreive specific file from ftp using command line input.
                    ' ==================================================================
                    ' Use a special processing folder here.
                    processingFolder = processingSpecialFolder
                    logger.InfoFormat("Retrieving specific files from FTP with names matching '{0}'...", sArgs(0))
                    ReadDataFromSSHSFtp(sArgs(0), False)
                    ImportAndArchiveFiles()
                Else

                    ' if the second argument is not ftp, run it as a scheduled job for all files

                    ' ==================================================================
                    ' Process all new files from FTP
                    ' This is the default/normal path for the process.
                    ' ==================================================================
                    ' 2010.01.19, Tom Lux
                    ' This job can hang during FTP processing, so we process any pending files first.
                    ' Use the default processing folder here.
                    processingFolder = processingMainFolder
                    ImportAndArchiveFiles()
                    ' This pulls all files down from FTP server to a local 'Processing' folder.
                    ReadDataFromSSHSFtp(String.Empty, True)
                    ' This processes files we just pulled down from FTP.
                    ImportAndArchiveFiles()

                End If

            ElseIf sArgs.Length = 1 Then

                ' only one argument was passed in, so we're trying to process a specific file

                ' ==================================================================
                ' Process file from local directory using command line input.
                ' ==================================================================
                logger.InfoFormat("Processing local file: {0}", sArgs(0))
                ' Use a special processing folder here.
                processingFolder = processingSpecialFolder
                ReadDataFromFile(sArgs(0))
            End If

            ' ==================================================================
            ' Remove all files from the archive that are older than 60 days old. 
            ' ==================================================================

            ' Updated to use common method (sub in this class), with configurable options.
            ' There used to be (effectively) 2 calls to the archive, one here and one in the
            ' ReadDataFromFTP() sub.
            ArchiveCleanup(eInvoiceFilePurgeFilenameFilter, eInvoiceFilePurgeThreshold)

        Catch ex As Exception

            My.Application.Log.WriteException(ex, TraceEventType.Error, String.Empty)

            Dim msg As String = String.Empty

            Dim dao As EInvoicingDAO = New EInvoicingDAO
            logger.Info("== [Fatal Exception] =========================================")
            msg = ex.Message & vbCrLf & vbCrLf
            If Not ex.InnerException Is Nothing Then
                msg = msg & ex.InnerException.Message & vbCrLf & vbCrLf
            End If
            msg = msg & ex.StackTrace
            dao.InsertErrorHistory()
            logger.Info(msg)
            logger.Info("^===========================================================^")

        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

    Private Sub ReadDataFromFile(ByVal _filename As String)
        ' project jeannnie needs this to have a work directory for the region
        _filename = _workDirectory & _filename

        Dim eInv As EInvoicingJob = New EInvoicingJob()
        Dim fileToProcess As FileInfo = Nothing
        Try
            If File.Exists(_filename) Then

                'Get the filename for debugging purposes. 
                EInvoicing_CurrentInvoice.Filename = _filename

                ' Copy file to special processing folder, if it's not already there.
                Dim invFile As FileInfo = New FileInfo(_filename)
                fileToProcess = New FileInfo(processingSpecialFolder & invFile.Name)
                logger.InfoFormat("Copying file to special processing location '{0}'...", fileToProcess)
                If Not fileToProcess.Exists Then
                    Try
                        File.Copy(_filename, fileToProcess.FullName)
                    Catch ex As Exception
                        logger.ErrorFormat("Could not copy '{0}' to '{1}' for processing.  Error: {2}", _filename, fileToProcess.FullName, ex.ToString)
                    End Try
                End If
                logger.InfoFormat("Processing file '{0}'...", fileToProcess)

                'Get a valid XML document.
                eInv.LoadEInvoicingDataFromString(File.ReadAllText(fileToProcess.FullName))

                'Scan the XML document for invoices.
                eInv.ParseInvoicesFromXML(eInv.XMLData, Nothing)

                ' Archive the file (make sure it doesn't exist first).
                If File.Exists(_archiveDirectory & invFile.Name) Then File.Delete(_archiveDirectory & invFile.Name)
                fileToProcess.MoveTo(_archiveDirectory & invFile.Name)
            Else
                logger.Info("File Does Not Exist: " & _filename)
            End If
        Catch ex As Exception
            logger.InfoFormat("Error processing file, original file: '{0}', copied file to be processed: '{1}'.", _filename, fileToProcess.FullName)
            logger.Info(ex.Message)
            logger.Info(ex.StackTrace)
            logInnerException(ex)
            ' If the file exists in the processing folder, it needs to be moved to the error folder.
            moveInvFileToErrors(fileToProcess.FullName)
            SendMail("Error occurred while processing e-Invoice file", EmailBodyMessage(_filename))
        End Try

    End Sub

    Private Sub ReadDataFromSSHSFtp(ByVal _filename As String, ByVal _setLastRunTime As Boolean)
        Dim fileList As List(Of String)
        Dim ftpServer As String
        Dim ftpUser As String
        Dim ftpPassword As String
        Dim ftpDir As String
        Dim ftpPort As String

        'ActionList is used to collect LOG messages and write them in 1 line instead of multiple. Logging cleanup effort.
        Dim ActionList As List(Of String) = New List(Of String)

        Dim regionCodes() As String

        If _filename Is Nothing Then _filename = String.Empty

        '  LastRunTime = getLastRunTime()
        'logger.InfoFormat("Last Run Time: {0}", LastRunTime.ToString)
        ftpServer = ConfigurationServices.AppSettings("EInvoicing_Server")
        ftpUser = ConfigurationServices.AppSettings("EInvoicing_Username")
        ftpPassword = ConfigurationServices.AppSettings("EInvoicing_Password")
        ftpDir = ConfigurationServices.AppSettings("EInvoicing_Directory")
        ftpPort = ConfigurationServices.AppSettings("EInvoicing_Port")

        regionCodes = ConfigurationServices.AppSettings("Region").ToString().Split(",")
        For Each region As String In regionCodes
            EInvoicing_CurrentInvoice.Regions += region & ","
        Next
        EInvoicing_CurrentInvoice.Regions = Left(EInvoicing_CurrentInvoice.Regions, Len(EInvoicing_CurrentInvoice.Regions) - 1)

        logger.InfoFormat("FTP Server: {0} Dir: {1}", ftpServer, ftpDir)

        Dim sftp = New SftpClient(ftpServer, ftpUser, ftpPassword)
        Dim files As IEnumerable(Of SftpFile)

        Try
            sftp.Connect()

            'Retrieves the directory list
            files = sftp.ListDirectory(ftpDir)

        Catch ex As Exception
            logger.Info(ex.Message)
            Throw ex
        End Try

        'Removes leading '/' from the directory
        If ftpDir.IndexOf("/") = 0 Then
            ftpDir = ftpDir.Remove(0, 1)
        End If

        fileList = New List(Of String)
        For Each file As SftpFile In files
            If file.Name = "" Then
                Continue For
            End If

            If (file.Name.Length > 0 And file.Name.Substring(0, 1) = ".") Then
                Continue For
            End If

            fileList.Add(file.Name)
        Next

        EInvoicing_CurrentInvoice.Filename = "test"

        If fileList.Count = 0 Then
            logger.Info("No FTP files found.")
        Else
            ' Log all pending/available files on FTP server.

            Dim fileListMsg As String = Nothing
            For Each name As String In fileList
                If Not fileListMsg Is Nothing Then
                    fileListMsg += ", " & name
                Else
                    fileListMsg = name
                End If
            Next
            logger.Info("Pending e-invoice files on FTP server: " & fileListMsg)
        End If

        Dim _FileOK As Boolean = False
        Dim filename As String = String.Empty

        ' If no specific remote FTP file was provided as cmd-line arg, we process all remote files.
        If _filename.Equals(String.Empty) Then
            For Each item As String In fileList
                Try
                    _FileOK = False
                    filename = String.Empty
                    ActionList.Clear()
                    ActionList.Add(String.Format("[ {0} ]:", item))
                    'logger.InfoFormat(" Retrieving: {0}", item)
                    filename = item.Replace(ftpDir, "")

                    If item.Contains("_invoice_") And item.Contains(".xml") Then

                        'this loop seems to be obsolete. should be verified and removed when scope allows.
                        For Each region As String In regionCodes
                            ' loop through the configured regions and make sure the current files matches at least one of them. 
                            If item.Contains(region & "_invoice") Then
                                _FileOK = True
                            End If
                        Next

                        ActionList.Add("Downloaded.")

                        Dim RemotePath As String = ftpDir + filename
                        Using fileStream As Stream = File.OpenWrite(processingFolder + filename)
                            sftp.DownloadFile(RemotePath, fileStream)
                        End Using

                        sftp.DeleteFile(RemotePath)

                        'logger.InfoFormat("Removing from FTP: {0}", filename)
                        ActionList.Add("Removed from FTP.")

                    Else
                        ActionList.Add("Skipped: doesnt match filename rules.")
                    End If
                Catch ex As Exception
                    ActionList.Add(String.Format("Error. {0}", ex.Message))
                Finally
                    logger.Info(String.Join(" ", ActionList.ToArray()))
                End Try
            Next
        Else

            ' If we are here, a specific FTP file was specified as cmd-line arg.
            For Each item As String In fileList
                Try
                    filename = String.Empty
                    ActionList.Clear()
                    ActionList.Add(String.Format("[ {0} ]:", item))
                    filename = item.Replace(ftpDir, "")
                    If item.Contains(_filename) Then
                        ActionList.Add("Downloaded.")

                        Dim FileStream As Stream = File.OpenWrite(processingFolder + filename)

                        Dim RemotePath As String = ftpDir + filename
                        Using fileStreamSpecial As Stream = File.OpenWrite(processingFolder + filename)
                            sftp.DownloadFile(RemotePath, fileStreamSpecial)
                        End Using

                        ActionList.Add("Removed from FTP.")
                        sftp.DeleteFile(RemotePath)
                    End If
                Catch ex As Exception
                    ActionList.Add(String.Format("Error. {0}", ex.Message))
                Finally
                    logger.Info(String.Join(" ", ActionList.ToArray()))
                End Try
            Next
        End If

        sftp.Disconnect()

    End Sub
    <Obsolete("This sub is deprecated. you should probably use ReadDataFromFTP() instead.", True)>
    Private Sub ReadDataFromJScapeFTP(ByVal _filename As String, ByVal _setLastRunTime As Boolean)
        ' Dim LastRunTime As DateTime
        Dim ftpClient As FTPclient = Nothing
        Dim fileList As List(Of String)
        Dim ftpServer As String
        Dim ftpUser As String
        Dim ftpPassword As String
        Dim ftpDir As String
        Dim ftpPort As String
        Dim regionCodes() As String

        If _filename Is Nothing Then _filename = String.Empty

        '  LastRunTime = getLastRunTime()
        'logger.InfoFormat("Last Run Time: {0}", LastRunTime.ToString)
        ftpServer = ConfigurationServices.AppSettings("EInvoicing_Server")
        ftpUser = ConfigurationServices.AppSettings("EInvoicing_Username")
        ftpPassword = ConfigurationServices.AppSettings("EInvoicing_Password")
        ftpDir = ConfigurationServices.AppSettings("EInvoicing_Directory")
        ftpPort = ConfigurationServices.AppSettings("EInvoicing_Port")
        regionCodes = ConfigurationServices.AppSettings("Region").ToString().Split(",")
        For Each region As String In regionCodes
            EInvoicing_CurrentInvoice.Regions = EInvoicing_CurrentInvoice.Regions & ","
        Next
        EInvoicing_CurrentInvoice.Regions = Right(EInvoicing_CurrentInvoice.Regions, Len(EInvoicing_CurrentInvoice.Regions) - 1)

        logger.InfoFormat("FTP Server: {0}", ftpServer)
        logger.InfoFormat("FTP Dir: {0}", ftpDir)

        Try
            ftpClient = New FTPclient(ftpServer, ftpUser, ftpPassword, True, ftpPort)
        Catch ex As Exception
            logger.Info(ex.Message)
            Throw
        End Try
        logger.Info("FTP Connection made.")
        EInvoicing_CurrentInvoice.Filename = "test"
        fileList = ftpClient.ListDirectory(ftpDir)
        If fileList.Count = 0 Then
            logger.Info("No FTP files found.")
        Else
            ' Log all pending/available files on FTP server.
            Dim fileListMsg As String = Nothing
            For Each name As String In fileList
                If Not fileListMsg Is Nothing Then
                    fileListMsg += ", " & name
                Else
                    fileListMsg = name
                End If
            Next
            logger.Info("Pending e-invoice files on FTP server: " & fileListMsg)
        End If

        ' The IF below had a NOT in it, which would execute the top conditional section when a specific FTP file was specified.
        ' However, it's the bottom/second conditional section that checks if the FTP file contains the desired file, so I removed the NOT.

        ' If no specific remote FTP file was provided as cmd-line arg, we process all remote files.
        If _filename.Equals(String.Empty) Then
            For Each item As String In fileList
                logger.InfoFormat("Scanning {0}", item)
                Dim _FileOK As Boolean = False
                Dim filename As String = item.Replace(ftpDir, "")

                If item.Contains("_invoice_") And item.Contains(".xml") Then
                    For Each region As String In regionCodes
                        ' loop through the configured regions and make sure the current files matches at least one of them. 
                        If item.Contains(region & "_invoice") Then
                            _FileOK = True
                        End If
                    Next

                    'If ParseDateFromFileName(item.Replace(ftpDir, "")) > LastRunTime Then
                    logger.InfoFormat("Downloading: {0}", filename)
                    ftpClient.Download(item, processingFolder & filename, True)
                    logger.InfoFormat("Removing from FTP: {0}", filename)
                    ftpClient.FtpDelete(item)
                End If
            Next
        Else
            ' If we are here, a specific FTP file was specified as cmd-line arg.
            For Each item As String In fileList
                logger.InfoFormat("Scanning {0}", item)
                Dim filename As String = item.Replace(ftpDir, "")
                If item.Contains(_filename) Then
                    logger.InfoFormat("Downloading: {0}", filename)
                    ftpClient.Download(item, processingFolder & filename, True)
                    logger.InfoFormat("Removing from FTP: {0}", filename)
                    ftpClient.FtpDelete(item)
                End If
            Next
        End If
    End Sub
    Private Sub ImportAndArchiveFiles()
        Dim eInv As EInvoicingJob = New EInvoicingJob()
        ' Get list of files in 'processing' folder.
        Dim fileList As String() = Directory.GetFiles(processingFolder)
        If fileList.Length = 0 Then
            logger.Info("'Processing' folder is empty.")
        Else
            ' Log all files in processing folder.
            Dim fileListMsg As String = Nothing
            For Each name As String In fileList
                If Not fileListMsg Is Nothing Then
                    fileListMsg += ", " & name
                Else
                    fileListMsg = name
                End If
            Next
            logger.InfoFormat("Processing {0} file(s): {1}", fileList.Length, fileListMsg)
        End If
        For Each filePath As String In fileList
            Dim invFile As FileInfo = New FileInfo(filePath)
            ' Load/import.
            logger.InfoFormat("Importing: {0}", filePath)
            Dim retry As Integer = 3
            While retry > 0
                Try
                    eInv.LoadEInvoicingData(filePath)
                    Exit While
                Catch ex As Exception
                    retry = retry - 1
                    If retry = 0 Then
                        logger.Error("Error loading e-inv :" & invFile.FullName & "; error :" & ex.ToString, ex)
                        logInnerException(ex)
                        moveInvFileToErrors(invFile.FullName)
                        SendMail("Error occurred while processing e-Invoice file", EmailBodyMessage(invFile.Name))
                        Continue For
                    End If
                End Try
            End While
            EInvoicing_CurrentInvoice.Filename = filePath
            Try
                eInv.ParseInvoicesFromXML(eInv.XMLData, Nothing)
            Catch ex As Exception
                logger.Error("Error parsing XML in e-inv :" & invFile.FullName & "; error :" & ex.ToString, ex)
                logInnerException(ex)
                moveInvFileToErrors(invFile.FullName)
                SendMail("Error occurred while processing e-Invoice file", EmailBodyMessage(invFile.Name))
                Continue For
            End Try
            ' Archive.
            logger.InfoFormat("Archiving: {0}", filePath)
            Dim archiveFile As FileInfo = New FileInfo(_archiveDirectory & invFile.Name)
            Try
                If archiveFile.Exists Then archiveFile.Delete()
                invFile.MoveTo(archiveFile.FullName)
            Catch ex As Exception
                logger.ErrorFormat("Error archiving e-inv '{0}', error: ", invFile.FullName, ex.ToString)
                logInnerException(ex)
                moveInvFileToErrors(invFile.FullName)
                SendMail("Error occurred while processing e-Invoice file", EmailBodyMessage(invFile.Name))
                Continue For
            End Try
        Next

    End Sub
    Private Sub ArchiveCleanup(ByVal filenameFilter As String, ByVal daysToKeep As Integer)
        '####################################################################################
        'ArchiveCleanup()
        '   Remove archived xml files after X days. 
        '####################################################################################
        Dim di As DirectoryInfo

        If Directory.Exists(_archiveDirectory) Then
            logger.InfoFormat("Purging '{0}' e-inv files older than {1} day(s).", _archiveDirectory & filenameFilter, daysToKeep)
            di = New DirectoryInfo(_archiveDirectory)
            ' The old, hard-coded filter here was "*invoice*.xml".
            Dim files As FileInfo() = di.GetFiles(filenameFilter)
            For Each file As FileInfo In files
                If file.CreationTime <= DateTime.Now.AddDays(daysToKeep * -1) Then
                    logger.InfoFormat("Archive Cleanup: {0}", file.Name)
                    file.Delete()
                End If
            Next
        End If
    End Sub

    Private Function ParseDateFromFileName(ByVal _filename As String) As DateTime
        Dim dt As DateTime
        'example filename: MA_invoice_20080818_151717.xml  
        'parts(0) = region code
        'parts(1) = "invoice"
        'parts(2) = date
        'parts(3) = time + ".xml"
        Dim parts As String()
        parts = _filename.Split("_")
        dt = New DateTime(parts(2).Substring(0, 4), parts(2).Substring(4, 2), parts(2).Substring(6, 2), parts(3).Substring(0, 2), parts(3).Substring(2, 2), parts(3).Substring(4, 2))
        Return dt
    End Function


    Private Sub logInnerException(ByVal ex As Exception)
        If Not ex.InnerException Is Nothing Then
            logger.Info("--- Inner Exception ---")
            logger.Info(ex.InnerException.Message)
            logger.Info(ex.InnerException.StackTrace)
        End If
    End Sub
    Private Sub moveInvFileToErrors(ByVal invFilePath As String)
        Dim invFile As FileInfo = New FileInfo(invFilePath)
        If invFile.Exists Then
            Dim destFile As String = _errorsDirectory & invFile.Name
            Try
                If File.Exists(destFile) Then File.Delete(destFile)
                invFile.MoveTo(destFile)
            Catch ex As Exception
                logger.ErrorFormat("Could not move error file '{0}' to '{1}'.  Error: {2}", invFile.FullName, destFile, ex.ToString)
            End Try
        End If
    End Sub
    Private Sub SendMail(ByVal sSubject As String, ByVal sMessage As String)
        Dim sSMTPHost As String = ConfigurationServices.AppSettings("SMTPHost")
        Dim sFromEmailAddress As String = ConfigurationServices.AppSettings("E-Invoicing_FromEmailAddress").ToString()
        Dim sEnvironMent As String = ConfigurationServices.AppSettings("environment").ToString()
        Dim sErrorToAddress As String = ConfigurationServices.AppSettings("E-Invoicing_ToEmailAddress").ToString()
        Dim sErrorCCAddress As String = ConfigurationServices.AppSettings("E-Invoicing_CcEmailAddress").ToString()
        Dim smtp As New SMTP(sSMTPHost)
        If sErrorCCAddress Is "" Then
            smtp.send(sMessage, sErrorToAddress, Nothing, sFromEmailAddress, sSubject)
        Else
            smtp.send(sMessage, sErrorToAddress, sErrorCCAddress, sFromEmailAddress, sSubject)
        End If
    End Sub
    Private Function EmailBodyMessage(ByVal fileName As String) As String
        Dim emailBody As String = "<HTML><table><tr><td>The e-Invoice job for " & ConfigurationServices.AppSettings("Region").ToString() & " failed to process the following file(s), these files have been moved to the "“Errors”" folder:</td></tr></table>" &
                                "<table><tr><td> " & fileName & " </td></tr></table><br /><br />" &
                                "<table><tr><td> Please find and re-process the file(s) from their respective directories. </td></tr></table>" &
                                "<table><tr><td> \\irmaprdfile\ScheduledJobs\EInvoicing\4.8.0\" & ConfigurationServices.AppSettings("Region").ToString() & "\Errors </td></tr></table><br /></HTML>"
        Return emailBody
    End Function
End Module