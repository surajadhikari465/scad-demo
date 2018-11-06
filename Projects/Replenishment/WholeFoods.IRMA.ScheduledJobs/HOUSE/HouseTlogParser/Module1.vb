Imports log4net
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Threading
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.TLog
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic

Module Module1
    ' ---------------------------------------------------------------------------------------------------------------
    ' Update History
    ' ---------------------------------------------------------------------------------------------------------------
    ' TFS 12091 (v3.6)
    ' Tom Lux
    ' 3/11/2010
    ' 1) Added Log4NetRuntime.ConfigureLogging() call at beginning of main sub.  This dynamically configures Log4Net.
    ' 2) Added DB-logging purge-history call at end of main sub.  This removes old logging entries in the database.
    ' 3) Removed _logs list of log entries, as log4net can queue log entries.
    ' 4) Change writeToConsole() to logger calls, as a log4net console appender can be setup for any job.
    ' ---------------------------------------------------------------------------------------------------------------

    Private CmdArgs() As String = System.Environment.GetCommandLineArgs()
    Private _StoreNo As Integer
    Private _ParseDate As DateTime
    Private _DaysOffset As Integer
    Private _Factory As DataAccess.DataFactory
    Private _StoreInformation As List(Of TlogStoreInfo) = New List(Of TlogStoreInfo)
    Private _BasePath As String
    Private _RequiredFiles As String
    Private _Region As String
    Private _LogFileAge As String
    Private _TlogDBUsername As String
    Private _TlogDBpassword As String
    Private _TlogProcessUsername As String
    Private _TlogProcesspassword As String
    Private _TlogDBServer As String
    Private _TlogDatabase As String
    Private _DataDirectory As String
    Private _LogDirectory As String
    Private _UseModifiedSubTeamNo As String 'True' or 'False'
    Private _TlogLoginInfo As TlogLoginInfo = New TlogLoginInfo
    Private _jobExecutionMessage As String
    Private _errorNotificationFrom As String
    Private _primaryErrorNotification As String
    Private _secondaryErrorNotification As String
    Private _QueueReceivingonSalesLoad As String
    Private _bProcessMissingDataOnly As Boolean
    Private Const CLASSNAME As String = "TLOGSalesLoadJob"

    ' Define the log4net logger for this class.
    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Sub Main()

        Try
            ' Configure logging specified in app-config file.
            log4net.Config.XmlConfigurator.Configure()

            ' Download app settings doc.
            Configuration.CreateAppSettings()

            ' Apply logging settings from app settings.  We don't need to check cmd-line args for connection strings, this object will take care of everything for us.
            Log4NetRuntime.ConfigureLogging()

            ' setup the private variables after downloaing the document
            SetConfiguration()

            CheckLoginInformation()
            'Dim QueueReceivingonSalesLoad As String = ConfigurationManager.AppSettings("QueueReceivingonSalesLoad")
            ValidateArguments()

            CreateDatabaseConnection()
            GetStoreInfo()

            'Check if region wants to use queueing during sales load 
            ' Dim QueueReceivingonSalesLoad As String = ConfigurationManager.AppSettings("QueueReceivingonSalesLoad")

            If (_QueueReceivingonSalesLoad = 1) Then
                'Set up Sales Job as running
                JobStatusDAO.UpdateJobStatus("TLOGSalesLoadJob", DBJobStatus.Queueing)
            End If
            'Run Sales load job
            If _StoreNo = -1 Then
                parseall()
            Else
                ParseStore()
            End If

            If (_QueueReceivingonSalesLoad = 1) Then
                Dim oParser As FLParser = New FLParser(_TlogLoginInfo)
                'Call procedure Move data from Queue
                oParser.MovedatafromQueue()

                'Set up Sales Job as completed
                logger.Info("T-Log processing finished.")
                JobStatusDAO.UpdateJobStatus("TLOGSalesLoadJob", DBJobStatus.Complete)

                ' wait for 30 seconds
                Threading.Thread.Sleep(30000)
                'Call procedure Move data from Queue
                oParser.MovedatafromQueue()
            End If
        Catch ex As Exception
            'Set up Sales Job as failed
            JobStatusDAO.UpdateJobStatus(CLASSNAME, DBJobStatus.Failed)
            JobStatusDAO.InsertJobError(CLASSNAME, CLASSNAME & " failed at step: " & ex.Message)
            logger.Error("Error in main processing.", ex)
            Thread.Sleep(5000)

        Finally

            Try
                ' check to see if Logs direcotry exists. If not, create it.
                If Not Directory.Exists(_LogDirectory) Then Directory.CreateDirectory(_LogDirectory)

                logger.InfoFormat("Cleaning up stored data older than {0} day(s)...", _LogFileAge)
                logger.InfoFormat("Data Directory: {0}", _DataDirectory)
                Dim dirs As String() = Directory.GetFileSystemEntries(_DataDirectory)

                Dim di As DirectoryInfo
                For Each dir As String In dirs
                    di = New DirectoryInfo(dir)
                    If di.CreationTime < DateTime.Now.AddDays(Math.Abs(Integer.Parse(_LogFileAge)) * -1) Then
                        di.Delete(True)
                    End If
                Next

            Catch ex As Exception

                logger.Error("Error during final cleanup.", ex)

            End Try

        End Try

        ' Purge history from DB-based application log.
        Try
            AppDBLogBO.purgeHistory()
        Catch ex As Exception
            logger.Error("App DB Log purge failed.", ex)
        End Try

    End Sub

    Private Sub CheckLoginInformation()

        Dim _EncryptedConnectionStrings As String = ConfigurationManager.AppSettings("encryptedConnectionStrings")
        If _EncryptedConnectionStrings.ToLower().Equals("true") Then
            ' decrypt values.
            Dim dec As WholeFoods.Utility.Encryption.Encryptor = New Encryption.Encryptor
            _TlogDBUsername = dec.Decrypt(_TlogDBUsername)
            _TlogDBpassword = dec.Decrypt(_TlogDBpassword)
            _TlogProcessUsername = dec.Decrypt(_TlogProcessUsername)
            _TlogProcesspassword = dec.Decrypt(_TlogProcesspassword)
        End If

        _TlogLoginInfo.DBPass = _TlogDBpassword
        _TlogLoginInfo.DBUser = _TlogDBUsername
        _TlogLoginInfo.ProcessPass = _TlogProcesspassword
        _TlogLoginInfo.ProcessUser = _TlogProcessUsername
        _TlogLoginInfo.DBServer = _TlogDBServer
        _TlogLoginInfo.Database = _TlogDatabase

    End Sub

    Private Function FixDate(ByVal d As DateTime) As String
        Dim retval As String
        If d.Day < 10 Then
            retval = "0" & d.Day.ToString
        Else
            retval = d.Day.ToString
        End If

        If d.Month < 10 Then
            retval += "0" & d.Month.ToString
        Else
            retval += d.Month.ToString
        End If
        retval += d.Year.ToString
        Return retval
    End Function

    Private Sub parseall()

        Dim parsedList As List(Of TlogStoreInfo) = New List(Of TlogStoreInfo)


        For Each store As TlogStoreInfo In _StoreInformation
            Try
                If store.StoreRegion.Equals("") Then Throw New Exception("Could not find region code for Store_No: " & _StoreNo.ToString)
                If Not store.StoreAbbr.Equals("") Then
                    Dim FullPath As String = _BasePath & Date2String(_ParseDate) & "\" & store.StoreRegion & store.StoreAbbr & "\"
                    If File.Exists(FullPath) Then Throw New Exception(FullPath & " appears to be a file. A directory is expected.")
                    If Not Directory.Exists(FullPath) Then Throw New Exception("Cannot find " & FullPath)
                    If Not ValidateRequiredFiles(FullPath) Then Throw New Exception("Not all required files can be found. " & _RequiredFiles)
                    ParseRequiredFiles(New DirectoryInfo(FullPath), store, _UseModifiedSubTeamNo)
                    parsedList.Add(store)
                Else
                    logger.Warn("Skipped " & Trim(store.StoreName) & ". No Store Abbreviation found.")
                End If
            Catch ex As Exception
                logger.Error("Could not parse T-Logs for: " & Trim(store.StoreName), ex)
                Dim args(0) As String
                args(0) = "Could not parse T-Logs for: " & Trim(store.StoreName)
                ErrorHandler.ProcessError(ErrorType.HouseTlogParser_CouldNotParseTlog, args, SeverityLevel.Warning, ex)
                ' Add additional error message information, if it exists, to the error message
                Dim message As New StringBuilder
                message.Append(args(0))
                message.Append(Environment.NewLine)
                _jobExecutionMessage = message.ToString

            Finally
                logger.Info("Finished Processing.")
            End Try
        Next

        Dim oParser As FLParser = New FLParser(_TlogLoginInfo)
        For Each store As TlogStoreInfo In parsedList
            Try
                logger.InfoFormat("Begin Generating ItemHistory from Sales for {0}", store.StoreName)
                oParser.CreateSalesFromItemHistory(_ParseDate, store.StoreNo)
            Catch ex As Exception
                logger.Error("Failed Generating ItemHistory from Sales for " & store.StoreName, ex)
            End Try

        Next


    End Sub

    Private Sub ParseStore()

        Dim CurrentStore As TlogStoreInfo = Nothing
        Dim StartTime As DateTime = Nothing
        Dim EndTime As DateTime = Nothing

        Try
            For Each store As TlogStoreInfo In _StoreInformation
                If store.StoreNo = _StoreNo Then
                    CurrentStore = store
                    Exit For
                End If
            Next
            If CurrentStore Is Nothing Then Throw New Exception("The store Tlog has been loaded or could not find the store information for Store_No: " & _StoreNo.ToString)
            If CurrentStore.StoreRegion.Equals("") Then Throw New Exception("Could not find region code for Store_No: " & _StoreNo.ToString)
            If Not CurrentStore.StoreAbbr.Equals("") Then
                Dim FullPath As String = _BasePath & Date2String(_ParseDate) & "\" & CurrentStore.StoreRegion & CurrentStore.StoreAbbr.Trim & "\"
                If File.Exists(FullPath) Then Throw New Exception(FullPath & " appears to be a file. A directory is expected.")
                If Not Directory.Exists(FullPath) Then Throw New Exception("Cannot find " & FullPath)
                If Not ValidateRequiredFiles(FullPath) Then Throw New Exception("Not all required files can be found. " & _RequiredFiles)
                StartTime = DateTime.Now
                ParseRequiredFiles(New DirectoryInfo(FullPath), CurrentStore, _UseModifiedSubTeamNo)
                EndTime = DateTime.Now
                logger.Info("Time: " & DateDiff(DateInterval.Second, StartTime, EndTime).ToString())

                logger.InfoFormat("Begin Generating ItemHistory from Sales for {0}", CurrentStore.StoreName)
                Dim oParser As FLParser = New FLParser(_TlogLoginInfo)
                oParser.CreateSalesFromItemHistory(_ParseDate, CurrentStore.StoreNo)



            Else
                logger.Warn("Skipped " & Trim(CurrentStore.StoreName) & ". No Store Abbreviation found.")
            End If
        Catch ex As Exception
            Dim InnerMsg As String = ""
            If Not ex.InnerException Is Nothing Then
                InnerMsg = ex.InnerException.Message
            End If
            logger.Error("Could Not Parse Tlogs for: " & Trim(CurrentStore.StoreName), ex)
            Dim args(0) As String
            args(0) = "Could not parse T-Logs for: " & Trim(CurrentStore.StoreName)
            ErrorHandler.ProcessError(ErrorType.HouseTlogParser_CouldNotParseTlog, args, SeverityLevel.Warning, ex)
            ' Add additional error message information, if it exists, to the error message
            Dim message As New StringBuilder
            message.Append(args(0))
            message.Append(Environment.NewLine)
            _jobExecutionMessage = message.ToString
        Finally
            logger.Info("Finished Processing.")
        End Try
    End Sub

    Private Sub ParseRequiredFiles(ByRef ParsingDirectory As DirectoryInfo, ByRef store As TlogStoreInfo, ByRef UseModifiedSubTeamNo As String)
        Dim oParser As FLParser = New FLParser(_TlogLoginInfo)
        oParser.ClearLoadTables()
        For Each rFile As String In _RequiredFiles.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            For Each aFile As FileInfo In ParsingDirectory.GetFiles(rFile)
                logger.Info("Parsing " & aFile.Name & " for " & Trim(store.StoreName))
                If rFile.Contains("DWTENDER") Then

                    If aFile.FullName.EndsWith(".001") Then
                        oParser.ImportDWFile(aFile.FullName, FLParser.DWFileType.DWTENDER)
                    End If

                End If

                If rFile.Contains("DWITEM") Then
                    If aFile.FullName.EndsWith(".001") Then
                        oParser.ImportDWFile(aFile.FullName, FLParser.DWFileType.DWITEM)
                    End If

                End If

            Next
        Next
        logger.InfoFormat("Updating Aggregates for {0}.  UseModifiedSubTeamNo: {1}", Trim(store.StoreName), UseModifiedSubTeamNo)
        oParser.UpdateAggregates(store.StoreRegion, UseModifiedSubTeamNo)
        logger.Info("Archiving Data Files for " & Trim(store.StoreName))
        ArchiveDataFiles(store)

    End Sub

    Private Sub ArchiveDataFiles(ByRef CurrentStore As TlogStoreInfo)
        Dim Path As String

        ' Create directory
        ' check to see if data direcotry exists. If not, create it.
        If Not Directory.Exists(_DataDirectory) Then Directory.CreateDirectory(_DataDirectory)
        If Not Directory.Exists(_DataDirectory & "\" & Date2String(_ParseDate)) Then Directory.CreateDirectory(_DataDirectory & "\" & Date2String(_ParseDate))
        If Not Directory.Exists(_DataDirectory & "\" & Date2String(_ParseDate) & "\" & CurrentStore.StoreRegion & CurrentStore.StoreAbbr.Trim) Then Directory.CreateDirectory(_DataDirectory & "\" & Date2String(_ParseDate) & "\" & CurrentStore.StoreRegion & CurrentStore.StoreAbbr.Trim)
        Path = _DataDirectory & "\" & Date2String(_ParseDate) & "\" & CurrentStore.StoreRegion & CurrentStore.StoreAbbr.Trim & "\"

        'copy file
        If File.Exists(Path & "DWITEM.001.orig") Then File.Delete(Path & "DWITEM.001.orig")
        If File.Exists(Path & "DWITEM.001") Then File.Move(Path & "DWITEM.001", Path & "DWITEM.001.orig")

        If File.Exists(Path & "DWTENDER.001.orig") Then File.Delete(Path & "DWTENDER.001.orig")
        If File.Exists(Path & "DWTENDER.001") Then File.Move(Path & "DWTENDER.001", Path & "DWTENDER.001.orig")

        File.Move("./DWITEM.001", Path & "DWITEM.001")
        File.Move("./DWTENDER.001", Path & "DWTENDER.001")

    End Sub

    Private Function ValidateRequiredFiles(ByRef FullPath As String) As Boolean

        Dim IsValid As Boolean = True
        Dim ParsingDirectory As DirectoryInfo = New DirectoryInfo(FullPath)

        For Each rFile As String In _RequiredFiles.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            If ParsingDirectory.GetFiles(rFile).Length = 0 Then IsValid = False
        Next
        Return IsValid
    End Function

    Private Function Date2String(ByVal d As DateTime) As String
        'return a string representation of the date as yymmdd
        Dim retval As String
        retval = d.Year.ToString.Substring(2, 2)
        If d.Month < 10 Then
            retval += "0" & d.Month.ToString
        Else
            retval += d.Month.ToString
        End If
        If d.Day < 10 Then
            retval += "0" & d.Day.ToString
        Else
            retval += d.Day.ToString
        End If
        Return retval
    End Function

    Private Sub ValidateArguments()
        ' Example cmd-line args for 3.5: HouseTlogParser.exe all yesterday 0

        If CmdArgs.Length < 6 Then
            Throw New Exception("Too Few Arguments." & ShowUsage())
        End If
        If CmdArgs.Length > 6 Then
            Throw New Exception("Too Many Arguments." & ShowUsage())
        End If
        Try
            If CmdArgs(3).ToLower.Equals("all") Then
                _StoreNo = -1
            Else
                _StoreNo = CInt(CmdArgs(3))
            End If

        Catch ex As Exception
            Throw New Exception("Argument 1 does not appear to be a valid store number." & ShowUsage())
        End Try

        Try
            If CmdArgs(4).ToLower.Equals("today") Then
                _ParseDate = DateTime.Now

            ElseIf CmdArgs(4).ToLower.Equals("yesterday") Then
                _ParseDate = DateTime.Now.AddDays(-1)
            Else
                _ParseDate = CDate(CmdArgs(4))
            End If

        Catch ex As Exception
            Throw New Exception("Argument 2 does not appear to be a valid date." & ShowUsage())
        End Try

        Try
            _DaysOffset = CInt(CmdArgs(5))
            _ParseDate = _ParseDate.AddDays(_DaysOffset)
        Catch ex As Exception
            Throw New Exception("Argument 3 does not appear to be a valid integer." & ShowUsage())
        End Try


        logger.Info("Arguments Given: " & CmdArgs.Length.ToString)
        logger.Info("StoreNo: " & CmdArgs(3))
        logger.Info("Date: " & CmdArgs(4))
        logger.Info("DaysOffset: " & CmdArgs(5))
    End Sub

    Private Function ShowUsage() As String
        Return vbCrLf & vbCrLf & "Usage: HouseTlogParser.exe <Store_No> <Date To Parse> <Days Offset>" & vbCrLf & vbCrLf & _
                "Store_No:  A Valid Store_No " & vbCrLf & "Date To Parse: This can be 'today', 'yesterday', or a valid date. " & vbCrLf & "Days Offset: This number of days will be added or subtraced from the Date To Parse" & vbCrLf & _
                "Example: if Date To Parse = 8/24/2006 and Days Offset = -1 then 8/23/2006 will be the effective date"
    End Function

    Private Sub CreateDatabaseConnection()
        _Factory = New DataAccess.DataFactory(DataFactory.ItemCatalog)
    End Sub

    Private Sub GetStoreInfo()
        Dim ds As DataSet = _Factory.GetStoredProcedureDataSet("GetStores")

        _StoreInformation.Clear()
        For Each dr As DataRow In ds.Tables(0).Rows
            If (Not _bProcessMissingDataOnly) Or (_bProcessMissingDataOnly And Not CheckTlogExists(CInt(dr("Store_No")), _ParseDate)) Then
                Dim store As TlogStoreInfo = New TlogStoreInfo
                store.StoreName = dr("Store_Name").ToString
                store.StoreNo = CInt(dr("Store_No"))
                store.StoreAbbr = dr("Storeabbr").ToString.Trim
                store.StoreRegion = dr("Region_Code").ToString.Trim
                _StoreInformation.Add(store)
            End If
        Next

    End Sub
    Private Function CheckTlogExists(ByVal storeNo As Integer, ByVal tlogDate As Date) As Boolean

        Dim tlogExists As Boolean = False
        Dim results As SqlDataReader = Nothing

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As ArrayList = New ArrayList
        Dim CurrentParam As DBParam

        CurrentParam = New DBParam
        CurrentParam.Name = "storeNo"
        CurrentParam.Type = DBParamType.Int
        CurrentParam.Value = storeNo
        paramList.Add(CurrentParam)

        CurrentParam = New DBParam
        CurrentParam.Name = "tlogDate"
        CurrentParam.Type = DBParamType.DateTime
        CurrentParam.Value = tlogDate
        paramList.Add(CurrentParam)

        Try
            results = factory.GetStoredProcedureDataReader("Replenishment_Tlog_House_CheckTlogExists", paramList)

            While results.Read
                'set arbitrary threshhold of 100 rows to determine "fully imported"
                tlogExists = (CInt(results("Tlog_Count")) > 100)
            End While

        Catch ex As Exception
            If ex.InnerException IsNot Nothing Then
                logger.Info(ex.Message & " => " & ex.InnerException.Message)
            Else
                logger.Info(ex.Message & " => " & ex.InnerException.Message)
            End If

        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return tlogExists

    End Function
    Private Sub SetConfiguration()

        Try

            If Environment.GetCommandLineArgs.Length > 1 Then

                _DataDirectory = My.Computer.FileSystem.CurrentDirectory & "\" & Environment.GetCommandLineArgs.GetValue(1).ToString & "\Data"
                _LogDirectory = My.Computer.FileSystem.CurrentDirectory & "\" & Environment.GetCommandLineArgs.GetValue(1).ToString & "\Logs"

            Else

                _DataDirectory = ".\Data"
                _LogDirectory = ".\Logs"

            End If

            _QueueReceivingonSalesLoad = ConfigurationServices.AppSettings("QueueReceivingonSalesLoad")
            _BasePath = ConfigurationServices.AppSettings("TlogBasePath")
            _RequiredFiles = ConfigurationServices.AppSettings("RequiredFiles")
            _Region = ConfigurationServices.AppSettings("Region")
            _LogFileAge = ConfigurationServices.AppSettings("LogFileAge")
            _TlogDBUsername = ConfigurationServices.AppSettings("TlogDatabaseUsername")
            _TlogDBpassword = ConfigurationServices.AppSettings("TlogDatabasePassword")
            _TlogProcessUsername = ConfigurationServices.AppSettings("TlogProcessUsername")
            _TlogProcesspassword = ConfigurationServices.AppSettings("TlogProcessPassword")
            _TlogDBServer = ConfigurationServices.AppSettings("TlogDBServer")
            _TlogDatabase = ConfigurationServices.AppSettings("Tlogdatabase")

            _errorNotificationFrom = ConfigurationServices.AppSettings("errorNotificationFrom")
            _primaryErrorNotification = ConfigurationServices.AppSettings("primaryErrorNotification")
            _secondaryErrorNotification = ConfigurationServices.AppSettings("secondaryErrorNotification")
            _bProcessMissingDataOnly = CBool(ConfigurationServices.AppSettings("ProcessMissingDataOnly"))
            Try
                _UseModifiedSubTeamNo = ConfigurationServices.AppSettings("UseModifiedSubTeamNo")
            Catch ex As Exception
                _UseModifiedSubTeamNo = "False"
            End Try
            logger.InfoFormat("[Config Setting] UseModifiedSubTeamNo: {0}", _UseModifiedSubTeamNo)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

End Module
