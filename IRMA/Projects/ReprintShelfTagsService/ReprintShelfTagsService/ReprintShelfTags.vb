Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility.FTP
Imports wholefoods.IRMA.Replenishment.Common.BusinessLogic
Imports wholefoods.IRMA.Replenishment.Common.DataAccess
Imports wholefoods.IRMA.Replenishment.Common.Writers
Imports System.Text
Imports System.Net
Imports System.IO

Public Class ReprintShelfTags

    Friend Event UpdateProgressBar(ByVal iteration As Integer, ByVal total As Integer)
    Friend Event UpdateOutputText(ByVal textToAppend As String, ByVal outputTextType As enumOutputTextType)

    Private _runAsWindowsApp As Boolean = False
    Private _tagFileName As String
    Private _ftpInfo As Hashtable = New Hashtable
    Private _ftpClient As FTP.FTPclient = Nothing
    Private _sftpClient As wodSFTP.SFTPClient = Nothing
    Private _timerInterval As Double = 0
    Private _deleteSourceFile As Boolean = False
    Private _useEmailOutput As Boolean = True
    Private _verboseOutput As Boolean = False
    Private _appVersion As String
    Private _dbConnectionString As String = String.Empty
    Private _delimiter As String = String.Empty
    Private _errFileNotFound As New ArrayList()
    Private _itemDataSet As DataSet = Nothing
    Private _StoreDataTable As DataTable = Nothing
    Private _errorsOccurred As Boolean = False
    Private _lineSeparator As New String("_"c, 50)
    Private _stopProcessRequested As Boolean = False
    Private _storeCacheDate As Date
    Private BULLET As Char = Chr(149)

    Friend Enum enumInterfaceType
        WindowsService = 0      'no user interface
        WindowsApplication = 1
    End Enum

    Friend Enum enumProcessType
        CheckFTP = 0
        Reprint = 1
    End Enum

    Friend Enum enumOutputTextType
        [Default] = 0
        Information = 1
        Warning = 2
        [Error] = 4
    End Enum

    Private Enum enumFilePathType
        ImportFilePath = 0
        ArchivePath = 1
        ErrorPath = 2
    End Enum

#Region "Constructors and Initialization"

    Public Sub New()

        With System.Reflection.Assembly.GetExecutingAssembly.GetName
            _appVersion = String.Format("{0} v{1}", .Name, .Version)
        End With

        If _appVersion.EndsWith(".0") Then
            'remove the build number if zero
            _appVersion = _appVersion.Remove(_appVersion.Length - 2)
        End If

        Call ReadConfigFile()

    End Sub

    Public Sub New(ByVal RunAsWindowsApplication As Boolean)

        Me.New()

        _runAsWindowsApp = RunAsWindowsApplication

    End Sub
#End Region

#Region "Properties"
    Friend ReadOnly Property TimerInterval() As Double
        Get
            Select Case _timerInterval
                Case 0

            End Select
            If _timerInterval = 0 Then
                'default to 1 minute if not specified
                _timerInterval = 1
            ElseIf _timerInterval > 10000 Then
                'assume value was specified in milliseconds, not minutes
                _timerInterval = _timerInterval / 60000
            End If

            Return (_timerInterval * 60000)
        End Get
    End Property

    Friend Property UseEmailOutput() As Boolean
        Get
            Return _useEmailOutput
        End Get
        Set(ByVal value As Boolean)
            _useEmailOutput = value
        End Set
    End Property

    Friend ReadOnly Property ErrorsOccurred() As Boolean
        Get
            Return _errorsOccurred
        End Get
    End Property

    Friend ReadOnly Property AppVersion() As String
        Get
            Return _appVersion
        End Get
    End Property

    Friend ReadOnly Property DBServer() As String
        Get
            Dim serverName As String = String.Empty

            If _dbConnectionString.Length <> 0 Then
                'connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ItemCatalog;Data Source=idd-na;"
                For Each element As String In _dbConnectionString.Split((";").ToCharArray)
                    If element.StartsWith("Data Source") Then
                        serverName = element.Split(("=").ToCharArray).GetValue(1)
                        Exit For
                    End If
                Next
            Else
                serverName = "<db connection string not set>"
            End If

            Return serverName
        End Get
    End Property

    Friend ReadOnly Property StoreDataTable() As DataTable
        Get
            'refresh the cached store info daily in the event new stores are added
            If DateDiff(DateInterval.Day, _storeCacheDate, Today) <> 0 Then
                _storeCacheDate = Today
                _StoreDataTable = Nothing
            End If

            If _StoreDataTable Is Nothing Then
                _StoreDataTable = GetStoreDataTable()
            End If

            Return _StoreDataTable
        End Get
    End Property

    Private ReadOnly Property LocalFilePath(ByVal pathType As enumFilePathType) As String
        Get
            Dim basePath As String = ConfigurationManager.AppSettings("LocalBasePath")

            Return String.Concat(basePath, ConfigurationManager.AppSettings(pathType.ToString))
        End Get
    End Property

    Private ReadOnly Property RemoteFilePath(ByVal pathType As enumFilePathType) As String
        Get
            Dim basePath As String = ConfigurationManager.AppSettings("RemoteBasePath")

            If basePath.StartsWith("\\ia", StringComparison.OrdinalIgnoreCase) AndAlso Environment.MachineName.StartsWith("CEW", StringComparison.OrdinalIgnoreCase) Then
                Dim serverName As String = basePath.Substring(0, basePath.IndexOf("\"c, 2))
                basePath = basePath.Replace(serverName, "\\localhost")
            End If

            Return String.Concat(basePath, ConfigurationManager.AppSettings(pathType.ToString))
        End Get
    End Property

    Private ReadOnly Property ServerFilePath(ByVal pathType As enumFilePathType) As String
        Get
            If _runAsWindowsApp Then
                Return RemoteFilePath(pathType)
            Else
                Return LocalFilePath(pathType)
            End If
        End Get
    End Property
#End Region

#Region "Friend Methods"
    Friend Function CheckRemoteServers() As String

        Return Run(enumProcessType.CheckFTP, Nothing)

    End Function

    Friend Function CheckRemoteServers(ByVal selectedStoreList As SortedList) As String

        Return Run(enumProcessType.CheckFTP, selectedStoreList)

    End Function

    Friend Function ProcessReprintRequests() As String

        Return Run(enumProcessType.Reprint, Nothing)

    End Function

    Friend Function ProcessReprintRequests(ByVal selectedStoreList As SortedList) As String

        Return Run(enumProcessType.Reprint, selectedStoreList)

    End Function

    Friend Sub HandleError(ByVal ex As System.Exception)

        Call SendEmail(ex.ToString, True)

    End Sub

    Friend Sub StopProcessing()

        _stopProcessRequested = True

    End Sub
#End Region

#Region "Private Methods"
    Private Function Run(ByVal processType As enumProcessType, ByVal selectedStoreList As SortedList) As String

        Dim storeFtpConfigDAO As New StoreFTPConfigDAO
        Dim storeFTP As StoreFTPConfigBO
        Dim statusMessage As StringBuilder = New StringBuilder()
        Dim outputMessage As StringBuilder = New StringBuilder()
        Dim currentStoreNo As Integer = 0
        Dim storeData As DataRow = Nothing
        Dim isFileNotFound As Boolean = False
        Dim isFileFound As Boolean = False
        Dim storeHasErrors As Boolean = False
        Dim fileCount As Short = 0
        Dim sStatusText As String = String.Empty
        Dim textStartIndex As Integer = 0
        Dim iteration As Integer = 0
        Dim storeCount As Integer = 0

        _errorsOccurred = False
        _stopProcessRequested = False

        Try
            ' Get ftp info for all configured stores
            _ftpInfo = storeFtpConfigDAO.GetFTPConfigDataForWriterType(Constants.FileWriterType_REPRINTTAGS)

            'get list of store names; useful unless StoreFTPConfigBO adds a StoreName property
            If selectedStoreList Is Nothing Then
                'process all stores; get complete list
                selectedStoreList = GetStoreDetails()
            End If

            If _runAsWindowsApp Then
                'Debug.WriteLine(RemoteFilePath(enumFilePathType.ImportFilePath))
                With outputMessage
                    .AppendFormat("Application: {0}", _appVersion)
                    .AppendLine()
                    .AppendFormat("Server: {0}", Environment.MachineName)
                    .AppendLine()
                    .AppendFormat("User: {0}", Environment.UserName)
                    .AppendLine()
                    .AppendFormat("Start Time: {0}", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .AppendLine()
                    .AppendFormat("End Time: {0}", String.Empty)    'value will be inserted below... 
                    .AppendLine()
                    .AppendLine()
                    If processType = enumProcessType.CheckFTP Then
                        .AppendFormat("Checking connections to {0} remote server(s)...", selectedStoreList.Count)
                    Else
                        .AppendFormat("Checking reprint requests on {0} remote server(s)...", selectedStoreList.Count)
                    End If
                    .AppendLine()
                End With

                sStatusText = outputMessage.ToString(textStartIndex, outputMessage.Length - textStartIndex - 1)
                textStartIndex = outputMessage.Length
                RaiseEvent UpdateProgressBar(iteration, selectedStoreList.Count)
                RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Default)
            End If

            'loop through list of selected stores
            For Each currentStoreNo In selectedStoreList.Keys

                Try
                    If _runAsWindowsApp Then
                        Windows.Forms.Application.DoEvents()
                        If _stopProcessRequested Then
                            Exit For
                        End If
                    End If

                    iteration += 1

                    'reset loop variables
                    storeHasErrors = False
                    statusMessage.Length = 0

                    RaiseEvent UpdateProgressBar(iteration, selectedStoreList.Count)

                    storeFTP = _ftpInfo.Item(currentStoreNo)

                    'get the current store information from the store no. key
                    storeData = CType(selectedStoreList(currentStoreNo), DataRow)

                    'add horizontal line delimiter between stores
                    statusMessage.AppendLine(_lineSeparator)
                    statusMessage.AppendFormat("Store: {0} ({1}); IP: ", storeData.Item("Store_Name").ToString.Trim, currentStoreNo)

                    If storeFTP Is Nothing Then
                        statusMessage.AppendLine("< NONE >")
                    Else
                        statusMessage.AppendLine(storeFTP.IPAddress)
                    End If

                    isFileFound = False

                    If processType = enumProcessType.CheckFTP Then
                        'check the FTP connection to the store's remote server; no files are processed
                        sStatusText = GetFTPConnectionStatus(storeFTP)
                        If sStatusText.StartsWith("Failed") Then
                            storeHasErrors = True
                            _errorsOccurred = True
                        ElseIf sStatusText.Contains("exists") Then
                            isFileFound = True
                        End If

                        'statusMessage.AppendFormat("{0} {1}", BULLET, sStatusText) - mep
                        'statusMessage.AppendLine() - mep
                        outputMessage.AppendFormat("{0} {1}", BULLET, sStatusText)
                        outputMessage.AppendLine()
                    Else
                        ' Pull the tag file from the controller over to the directory on this box
                        If DownloadFile(storeFTP) Then
                            'increment the counter for files found
                            fileCount += 1
                            sStatusText = "File downloaded"
                            isFileFound = True
                        Else
                            sStatusText = "No file found"
                            'If _runAsWindowsApp Then
                            'statusMessage.AppendFormat("{0} {1}", BULLET, sStatusText)
                            'statusMessage.AppendLine()
                            'Else
                            'statusMessage.Length = 0 - mep
                            'End If
                        End If

                        outputMessage.AppendFormat("{0} {1}", BULLET, sStatusText) ' - mep
                        outputMessage.AppendLine() ' - mep

                        If Not isFileFound Then
                            'loop to the next store
                            Continue For
                        End If

                        'statusMessage.AppendFormat("{0} {1}", BULLET, sStatusText) - mep
                        'statusMessage.AppendLine() - mep

                        ' Import File
                        sStatusText = ImportFile(currentStoreNo)

                        statusMessage.Append(sStatusText)

                        'archive the local file; returns filename if successful
                        sStatusText = ArchiveFile(storeData)

                        'statusMessage.AppendFormat("{0} File archived: {1}", BULLET, sStatusText) - mep
                        'statusMessage.AppendLine() - mep
                        outputMessage.AppendFormat("{0} File archived: {1}", BULLET, sStatusText)
                        outputMessage.AppendLine()

                        'delete the file from the controller
                        If _deleteSourceFile Then
                            If DeleteFile(storeFTP) Then
                                sStatusText = "File deleted from the controller"
                            Else
                                sStatusText = "Error deleting file from the controller"
                            End If

                            'statusMessage.AppendFormat("{0} {1}", BULLET, sStatusText) - mep
                            'statusMessage.AppendLine() - mep
                            outputMessage.AppendFormat("{0} {1}", BULLET, sStatusText)
                            outputMessage.AppendLine()
                        End If
                    End If

                Catch ex As Exception
                    'write to event log 
                    storeHasErrors = True
                    _errorsOccurred = True
                    statusMessage.AppendFormat("{0} {1}", BULLET, "Unexpected error!  Processing will continue...")
                    statusMessage.AppendLine()
                    If _verboseOutput Then
                        statusMessage.AppendLine(ex.ToString)
                    Else
                        statusMessage.AppendLine(ex.Message)
                    End If
                    'statusMessage.AppendLine()
                    WriteToEventLog(statusMessage.ToString, EventLogEntryType.Error)
                    WriteToDatabaseErrorLog(String.Format("{0}: Unexpected error occurred processing Store No. {1}", _appVersion, currentStoreNo), ex)
                    ArchiveFile(storeData, _errorsOccurred)

                Finally
                    outputMessage.Append(statusMessage.ToString)
                    If _runAsWindowsApp AndAlso Not _stopProcessRequested Then
                        If outputMessage.Length > textStartIndex Then
                            sStatusText = outputMessage.ToString(textStartIndex, outputMessage.Length - textStartIndex - 1)
                        End If
                        textStartIndex = outputMessage.Length
                        If storeHasErrors Then
                            RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Error)
                        ElseIf isFileFound Then
                            RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Information)
                        Else
                            RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Default)
                        End If
                    End If

                End Try

            Next

        Catch ex As Exception
            'write to event log
            _errorsOccurred = True
            outputMessage.AppendFormat("{0} {1}", BULLET, "*** FATAL error! ***  Processing will be aborted...")
            outputMessage.AppendLine()
            outputMessage.AppendLine(ex.ToString)
            WriteToEventLog(outputMessage.ToString, EventLogEntryType.Error)
            WriteToDatabaseErrorLog(String.Format("{0}: FATAL error occurred processing Store No. {1}", _appVersion, currentStoreNo), ex)

        Finally
            outputMessage.AppendLine(_lineSeparator)

            If _runAsWindowsApp Then
                'update the time in the output message
                sStatusText = Now.ToString("yyyy-MM-dd HH:mm:ss")
                outputMessage.Replace(String.Format("End Time: {0}", String.Empty), String.Format("End Time: {0}", sStatusText))
                textStartIndex += sStatusText.Length

                sStatusText = outputMessage.ToString(textStartIndex, outputMessage.Length - textStartIndex - 1)
                RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Default)

                If _stopProcessRequested Then
                    sStatusText = "      *** < User cancelled processing > ***"
                    RaiseEvent UpdateOutputText(sStatusText, enumOutputTextType.Warning)
                End If
            End If

            If _useEmailOutput AndAlso (fileCount <> 0 OrElse _errorsOccurred) Then
                SendEmail(outputMessage.ToString(), _errorsOccurred)
            End If

            'clear the reusable dataset after each processing loop to ensure data stays current
            If _itemDataSet IsNot Nothing Then
                _itemDataSet.Clear()
                _itemDataSet.Dispose()
                _itemDataSet = Nothing
            End If

            selectedStoreList = Nothing
            storeFtpConfigDAO = Nothing
            _ftpInfo.Clear()

        End Try

        Return outputMessage.ToString

    End Function

    Private Function GetStoreDetails() As System.Collections.SortedList

        Dim row As DataRow = Nothing
        Dim storeList As System.Collections.SortedList = New SortedList()

        Try
            For Each row In StoreDataTable().Rows
                storeList.Add(row.Item("Store_No"), row)
            Next

        Catch ex As Exception
            WriteToDatabaseErrorLog("[ReprintShelfTags].[GetStoreDetails]: ", ex)
            HandleError(ex)

        Finally
            row = Nothing

        End Try

        Return storeList

    End Function

    Private Function GetStoreDataTable() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim storeTable As DataTable = Nothing

        Try
            _dbConnectionString = factory.ConnectString()

            storeTable = factory.GetStoredProcedureDataSet("GetStores").Tables.Item(0)

        Catch ex As Exception
            WriteToDatabaseErrorLog("[ReprintShelfTags].[GetStoreDataTable]: GetStores", ex)
            HandleError(ex)

        Finally
            factory = Nothing

        End Try

        Return storeTable

    End Function

    Private Sub ReadConfigFile()

        ' Get the IBM directory\filenames from the config file
        _tagFileName = ConfigurationManager.AppSettings("ReprintTagsFileName")

        _timerInterval = ConfigurationManager.AppSettings("TimerInterval")

        _deleteSourceFile = CType(ConfigurationManager.AppSettings("DeleteSourceFile"), Boolean)

        _delimiter = ConfigurationManager.AppSettings("FileDelimiter")

        _useEmailOutput = (Not _runAsWindowsApp) OrElse ConfigurationManager.AppSettings("EmailWindowsAppOutput")

        _verboseOutput = ConfigurationManager.AppSettings("VerboseOutput")

        With _errFileNotFound
            .Add(ConfigurationManager.AppSettings("FileNotFoundOne"))
            .Add(ConfigurationManager.AppSettings("FileNotFoundTwo"))
            .Add(ConfigurationManager.AppSettings("FileNotFoundThree"))
            .Add(ConfigurationManager.AppSettings("FileNotFoundFour"))
        End With

    End Sub

    ''' <summary>
    ''' Checks for the existence of a remote file using FTP
    ''' </summary>
    ''' <param name="store">StoreFTPConfigBO</param>
    ''' <returns>True if the file exists; otherwise False</returns>
    ''' <remarks></remarks>
    Private Function GetFTPConnectionStatus(ByVal store As StoreFTPConfigBO) As String

        Dim status As String = String.Empty
        Dim fileDirectory As String = String.Empty
        Dim remoteFileName As String = String.Empty
        Dim fileExists As Boolean = False

        Try
            If store Is Nothing Then
                Throw New FTP.FTPException(String.Format("Store FTP info not configured for '{0}' writer type!", Constants.FileWriterType_REPRINTTAGS))
            End If

            fileDirectory = store.ChangeDirectory

            If fileDirectory.EndsWith("\") Then
                fileDirectory.Remove(fileDirectory.LastIndexOf("\"))
            End If

            remoteFileName = String.Format("{0}\{1}", fileDirectory, _tagFileName)

            'create an FTP client for this store with its FTP config settings
            With store
                If .IsSecureTransfer Then
                    _sftpClient = New wodSFTP.SFTPClient(.IPAddress, .FTPUser, .FTPPassword)

                    fileExists = _sftpClient.FileExists(remoteFileName)
                Else
                    _ftpClient = New FTP.FTPclient(.IPAddress, .FTPUser, .FTPPassword, .IsSecureTransfer)

                    fileExists = _ftpClient.FtpFileExists(remoteFileName)
                End If
            End With

            'Check the file for each store as a way to detect connection issues
            If fileExists Then
                status = "Successful connection; tag file exists"
            Else
                status = "Successful connection"
            End If

        Catch ex As Exception
            status = String.Format("Failed connection: {0}", ex.Message)

        Finally
            _ftpClient = Nothing
            _sftpClient = Nothing

        End Try

        Return status

    End Function

    ''' <summary>
    ''' Downloads a remote tag file using FTP
    ''' </summary>
    ''' <param name="store">StoreFTPConfigBO</param>
    ''' <returns>True if the file was successfully downloaded; otherwise False</returns>
    ''' <remarks></remarks>
    Private Function DownloadFile(ByVal store As StoreFTPConfigBO) As Boolean

        Dim status As Boolean = False
        Dim fileDirectory As String = String.Empty
        Dim remoteFileName As String = String.Empty
        Dim localFileName As String = String.Empty

        Try
            If store Is Nothing Then
                Throw New FTP.FTPException(String.Format("Store FTP info not configured for '{0}' writer type!", Constants.FileWriterType_REPRINTTAGS))
            End If

            fileDirectory = store.ChangeDirectory

            If fileDirectory.EndsWith("/") Then
                fileDirectory.Remove(fileDirectory.LastIndexOf("/"))
            End If

            If fileDirectory.Length = 0 Then
                remoteFileName = _tagFileName
            Else
                remoteFileName = String.Format("{0}/{1}", fileDirectory, _tagFileName)
            End If

            localFileName = String.Format("{0}{1}", LocalFilePath(enumFilePathType.ImportFilePath), _tagFileName)

            'create an FTP client for this store with its FTP config settings
            With store
                If .IsSecureTransfer Then
                    _sftpClient = New wodSFTP.SFTPClient(.IPAddress, .FTPUser, .FTPPassword)

                    'Download the file from each store 

                    If _sftpClient.Download(remoteFileName, localFileName, True) Then
                        status = True
                    Else
                        status = False
                    End If
                Else
                    _ftpClient = New FTP.FTPclient(.IPAddress, .FTPUser, .FTPPassword, .IsSecureTransfer)

                    'Download the file from each store 
                    If _ftpClient.Download(remoteFileName, localFileName, True) Then
                        status = True
                    Else
                        status = False
                    End If
                End If
            End With

        Catch ex1 As FileNotFoundException
            Throw ex1

        Catch ex As Exception
            Dim errEnumerator As IEnumerator = _errFileNotFound.GetEnumerator
            Dim currentError As String
            Dim expectedError As Boolean = False

            While errEnumerator.MoveNext
                currentError = CType(errEnumerator.Current, String)

                If ex.Message.Contains(currentError) Then
                    'swallow expected error
                    expectedError = True
                    Exit While
                End If
            End While

            If Not expectedError Then
                Throw ex
            End If

        Finally
            _ftpClient = Nothing
            _sftpClient = Nothing

        End Try

        Return status

    End Function

    ''' <summary>
    ''' Deletes a remote tag file using FTP
    ''' </summary>
    ''' <param name="store">StoreFTPConfigBO</param>
    ''' <returns>True if the file was successfully deleted; otherwise False</returns>
    ''' <remarks></remarks>
    Private Function DeleteFile(ByVal store As StoreFTPConfigBO) As Boolean

        Dim status As Boolean = False
        Dim fileDirectory As String = String.Empty
        Dim remoteFileName As String = String.Empty

        ' Remove the file from the controller after it's downloaded
        Try
            fileDirectory = store.ChangeDirectory

            If fileDirectory.EndsWith("/") Then
                fileDirectory.Remove(fileDirectory.LastIndexOf("/"))
            End If

            If fileDirectory.Length = 0 Then
                remoteFileName = _tagFileName
            Else
                remoteFileName = String.Format("{0}/{1}", fileDirectory, _tagFileName)
            End If

            'create an FTP client for this store with its FTP config settings
            With store
                If .IsSecureTransfer Then
                    _sftpClient = New wodSFTP.SFTPClient(.IPAddress, .FTPUser, .FTPPassword)

                    status = _sftpClient.DeleteFile(remoteFileName)
                Else
                    _ftpClient = New FTP.FTPclient(.IPAddress, .FTPUser, .FTPPassword, .IsSecureTransfer)

                    status = _ftpClient.FtpDelete(remoteFileName)
                End If
            End With

        Catch ex As Exception
            status = False

        Finally
            _ftpClient = Nothing
            _sftpClient = Nothing

        End Try

        Return status

    End Function

    Private Function ImportFile(ByVal store_no As Integer) As String

        Dim fileReader As System.IO.StreamReader = Nothing
        Dim lineText As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim ItemDataset As DataSet = Nothing
        Dim itemKeyList As StringBuilder = New StringBuilder()
        Dim identifiersNotFound As StringBuilder = New StringBuilder()
        Dim identifiersFound As StringBuilder = New StringBuilder()
        Dim statusMessage As StringBuilder = New StringBuilder()
        Dim identifierCount As Integer = 0
        Dim identifierNotFoundCount As Integer = 0
        Dim fileDirectory As String = String.Empty
        Dim localFileName As String = String.Empty
        Dim index1 As Integer = 0
        Dim index2 As Integer = 0
        Dim identifier As String = String.Empty
        Dim quantity As Integer = 0
        Dim fieldText As String()

        Try
            ItemDataset = GetItemDataset()

            fileDirectory = LocalFilePath(enumFilePathType.ImportFilePath)

            If fileDirectory.EndsWith("\") Then
                fileDirectory.Remove(fileDirectory.LastIndexOf("\"))
            End If

            localFileName = String.Format("{0}\{1}", fileDirectory, _tagFileName)

            Dim foundRows As DataRow() = Nothing
            fileReader = My.Computer.FileSystem.OpenTextFileReader(localFileName)
            While Not fileReader.EndOfStream
                lineText = fileReader.ReadLine()

                If _delimiter.Length = 0 Then
                    'no delimiter; use entire line as the identifier
                    identifier = lineText
                    quantity = 1
                Else
                    ' split string using delimiter
                    fieldText = lineText.Split(_delimiter)
                    Select Case fieldText(1).ToUpper
                        Case "HR"
                            'we could grab the date off the Header Row
                            ' this will require some stored procedure logic changes that could mess up the reprint screen
                            identifier = String.Empty
                            quantity = 0
                        Case "R"
                            ' this style of file with the delimiter is padding to 13 by adding a zero on the right, 
                            ' everything else pads to 12 on the left, so dropping right-most zero
                            identifier = fieldText(3).Remove(fieldText(3).Length - 1, 1).Trim
                            quantity = fieldText(5)
                        Case "T"
                            'Do we need anything off this row?
                            identifier = String.Empty
                            quantity = 0
                    End Select
                End If

                If identifier.Length <> 0 Then
                    sSQL = String.Format("PaddedIdentifier = '{0}'", identifier)

                    foundRows = ItemDataset.Tables(0).Select(sSQL)
                    If foundRows IsNot Nothing AndAlso foundRows.Length <> 0 Then
                        For index1 = 0 To foundRows.GetUpperBound(0)
                            For index2 = 1 To quantity
                                itemKeyList.AppendFormat("| {0} ", CType(foundRows(index1).Item("Item_Key"), Integer))
                                identifiersFound.AppendFormat("| {0} ", identifier)
                            Next index2
                        Next index1
                    Else
                        identifiersNotFound.AppendFormat("| {0} ", identifier)
                        identifierNotFoundCount += 1
                    End If
                    identifierCount += 1
                End If

            End While

            If itemKeyList.Length <> 0 Then
                'remove the leading delimiter
                itemKeyList.Remove(0, 1)
            End If

            If identifiersFound.Length <> 0 Then
                'remove the leading delimiter
                identifiersFound.Remove(0, 1)
            End If

            If identifiersNotFound.Length <> 0 Then
                'remove the leading delimiter
                identifiersNotFound.Remove(0, 1)
            End If

            ' update the IRMA database for all the item identifiers
            UpdateSignQueue(itemKeyList.ToString.Trim, store_no)

            With statusMessage
                If identifierCount = 0 Then
                    .AppendFormat("{0} Unable to locate any items in the file to process!{1}", BULLET, Environment.NewLine)
                Else
                    .AppendFormat("{0} Queued {1} of {2} items for reprinting:{3}", BULLET, identifierCount - identifierNotFoundCount, identifierCount, Environment.NewLine)
                    If identifierCount <> 0 Then
                        .AppendLine(identifiersFound.ToString.Trim)
                    End If
                    If identifierNotFoundCount <> 0 Then
                        .AppendFormat("{0} Unable to locate {1} of {2} items:{3}", BULLET, identifierNotFoundCount, identifierCount, Environment.NewLine)
                        .AppendLine(identifiersNotFound.ToString.Trim)
                    End If
                End If
            End With

        Catch ex As Exception
            'bubble up to the calling procedure
            Throw ex

        Finally
            If fileReader IsNot Nothing Then
                fileReader.Close()
                fileReader.Dispose()
                fileReader = Nothing
            End If

        End Try

        Return statusMessage.ToString

    End Function

    Private Function ArchiveFile(ByVal storeInfo As DataRow, Optional ByVal isError As Boolean = False) As String

        Dim backupFolder As String = String.Empty
        Dim sourceFile As String = LocalFilePath(enumFilePathType.ImportFilePath) & _tagFileName
        Dim destinationFile As String = String.Empty
        Dim storeFolder As String = String.Empty

        Try
            If isError AndAlso Not File.Exists(sourceFile) Then
                'already an error; no new error if source file doesn't exist
                Exit Try
            End If

            With storeInfo
                If .Item("StoreAbbr") Is DBNull.Value OrElse .Item("StoreAbbr").ToString.Trim.Length = 0 Then
                    storeFolder = storeInfo.Item("Store_Name").ToString.Trim
                Else
                    storeFolder = String.Format("{0}{1}", storeInfo.Item("Region_Code"), storeInfo.Item("StoreAbbr")).ToString.Trim.ToUpper
                End If
            End With

            If Not isError Then
                backupFolder = String.Format("{0}{1}\{2}", ServerFilePath(enumFilePathType.ArchivePath), Now.ToString("yyyyMMdd"), storeFolder)
            Else
                backupFolder = String.Format("{0}{1}\{2}", ServerFilePath(enumFilePathType.ErrorPath), Now.ToString("yyyyMMdd"), storeFolder)
            End If

            If Not Directory.Exists(backupFolder) Then
                System.IO.Directory.CreateDirectory(backupFolder)
            End If

            destinationFile = String.Format("{0}\{1}_{2}.bak", backupFolder, _tagFileName.Replace(".dat", ""), Now.ToString("HHmmss"))

            'move and rename the local tag file
            File.Move(sourceFile, destinationFile)

        Catch ex As Exception
            'bubble up to the calling procedure
            destinationFile = String.Empty
            Throw ex

        End Try

        Return destinationFile

    End Function

    Friend Sub SendEmail(ByVal processingMessage As String, Optional ByVal isError As Boolean = False)

        Dim SendMailClient As New Mail.SmtpClient(ConfigurationManager.AppSettings("EmailSMTPServer"))
        Dim mmsg As Mail.MailMessage
        Dim emailDisplayName As String
        Dim emailFrom As String
        Dim emailTo As String
        Dim emailMessage As New Text.StringBuilder()
        Dim emailSubject As String

        Try
            emailFrom = ConfigurationManager.AppSettings("EmailFromAddress")
            'check for tokens in the "from" e-mail address
            If emailFrom.ToUpper.Contains("%DBSERVER%") Then
                emailFrom = emailFrom.Replace("%DBSERVER%", Me.DBServer)
            End If

            If Not isError Then
                emailTo = ConfigurationManager.AppSettings("EmailToAddress")
                emailSubject = String.Format("{0} Confirmation", _appVersion)
            Else
                emailTo = ConfigurationManager.AppSettings("EmailErrorToAddress")
                emailSubject = String.Format("{0} ERROR", _appVersion)
            End If

            With emailMessage
                .AppendLine(processingMessage)

                If Not _runAsWindowsApp Then
                    .AppendLine()
                    .AppendLine("This is an automated email notification.")
                    .AppendLine()
                    .AppendFormat("Service: {0}", _appVersion)
                    .AppendLine()
                    .AppendFormat("Server: {0}", Environment.MachineName)
                    .AppendLine()
                    .AppendFormat("User: {0}", Environment.UserName)
                    .AppendLine()
                    .AppendFormat("Date: {0}", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    .AppendLine()
                End If
            End With

            mmsg = New Mail.MailMessage()
            With mmsg
                If emailFrom.Contains("[") AndAlso emailFrom.Contains("]") Then
                    'address is in the format "Display Name [address@email.com]"
                    emailDisplayName = emailFrom.Substring(0, emailFrom.IndexOf("[")).Trim
                    emailFrom = emailFrom.Substring(emailFrom.IndexOf("[") + 1).Replace("]", "").Trim

                    .From = New Mail.MailAddress(emailFrom, emailDisplayName)
                Else
                    .From = New Mail.MailAddress(emailFrom)
                End If

                For Each address As String In emailTo.Split(";"c)
                    .To.Add(New Mail.MailAddress(address))
                Next

                If ConfigurationManager.AppSettings("EmailReplyTo").Length <> 0 Then
                    .ReplyTo = New Mail.MailAddress(ConfigurationManager.AppSettings("EmailReplyTo"))
                End If

                .Subject = emailSubject
                .Body = emailMessage.ToString

                .Priority = IIf(isError, Mail.MailPriority.High, Mail.MailPriority.Normal)
                '.IsBodyHtml = True
            End With

            SendMailClient.Send(mmsg)

        Catch ex As Exception
            WriteToDatabaseErrorLog(emailMessage.ToString, ex)

        Finally
            If mmsg IsNot Nothing Then
                mmsg.Dispose()
            End If
            SendMailClient = Nothing

        End Try

    End Sub

    Private Sub WriteToEventLog(ByVal message As String, ByVal type As System.Diagnostics.EventLogEntryType)

        'Try
        '    Diagnostics.EventLog.WriteEntry("ReprintShelfTagsService", message, type)

        'Catch ex As Exception
        '    message = String.Format("Unable to write to Event Log:{1}{0}{1}{1}{2}", ex.ToString, Environment.NewLine, message)
        '    SendEmail(message, True)

        'End Try

    End Sub

    Private Sub WriteToDatabaseErrorLog(ByVal odbcCall As String, ByVal ex As Exception)

        Dim odbcError As New ODBCErrorLog()
        Dim errorMessage As String = String.Empty

        Try
            'log exception to ODBCErrorLog table

            If ex.InnerException Is Nothing Then
                errorMessage = String.Format("{0}", ex.Message)
            Else
                errorMessage = String.Format("{0}; {1}", ex.Message, ex.InnerException.Message)
            End If

            With odbcError
                .ODBCStart = Now
                .ODBCEnd = Now
                .ErrorNumber = 0

                .ErrorDescription = errorMessage
                .ODBCCall = odbcCall

                .InsertODBCErrorLog()
            End With

        Catch ex1 As Exception
            errorMessage = String.Format("Unable to write to ODBCErrorLog:{1}{0}{1}{1}{2}", ex1.ToString, Environment.NewLine, errorMessage)
            SendEmail(errorMessage, True)

        Finally
            odbcError = Nothing

        End Try

    End Sub

    Private Sub UpdateSignQueue(ByVal itemKeyList As String, ByVal store_no As Integer)

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New DBParamList()

        Try
            ' setup parameters for stored proc
            With paramList
                .Add(New DBParam("ItemList", DBParamType.String, itemKeyList))
                .Add(New DBParam("ItemListSeparator", DBParamType.Char, "|"))
                .Add(New DBParam("Store_No", DBParamType.Int, store_no))
                .Add(New DBParam("Printed", DBParamType.Bit, 0))
                .Add(New DBParam("User_ID", DBParamType.Int, 0))
                .Add(New DBParam("Type", DBParamType.SmallInt, 2))
            End With

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("UpdateSignQueuePrinted", paramList.CopyToArrayList)

        Catch ex As Exception

        Finally
            paramList = Nothing
            factory = Nothing

        End Try

    End Sub

    Private Function GetItemDataset() As DataSet

        Dim factory As DataFactory = Nothing
        Dim ds As DataSet = Nothing

        Try
            If _itemDataSet IsNot Nothing Then
                'reuse the existing data set
                ds = _itemDataSet
            Else
                _itemDataSet = New DataSet

                factory = New DataFactory(DataFactory.ItemCatalog)

                _itemDataSet = factory.GetStoredProcedureDataSet("Replenishment_POSPull_GetIdentifier")

                ds = _itemDataSet
            End If

        Finally
            factory = Nothing

        End Try

        Return ds

    End Function

    'Private Function GetSubTeamDetails() As System.Collections.SortedList

    '    Dim row As DataRow = Nothing
    '    Dim subteamList As System.Collections.SortedList = New SortedList()
    '    Dim subteamKey As Integer
    '    Dim subteamNo_3digit As String

    '    Try
    '        For Each row In GetSubTeamDataTable().Rows

    '            subteamKey = row.Item("SubTeam_No")

    '            subteamNo_3digit = subteamKey.ToString

    '            If subteamNo_3digit.Length > 3 Then
    '                subteamNo_3digit = subteamNo_3digit.Substring(0, 3)
    '            End If

    '            subteamList.Add(subteamKey, subteamNo_3digit)

    '        Next

    '    Catch ex As Exception
    '        HandleError(ex)

    '    Finally
    '        row = Nothing

    '    End Try

    '    Return subteamList

    'End Function

    'Private Function GetSubTeamDataTable() As DataTable

    '    Dim factory As New DataFactory(DataFactory.ItemCatalog)
    '    Dim subteamTable As DataTable = Nothing

    '    Try
    '        subteamTable = factory.GetStoredProcedureDataSet("dbo.GetSubTeams").Tables.Item(0)

    '    Catch ex As Exception
    '        HandleError(ex)

    '    Finally
    '        factory = Nothing

    '    End Try

    '    Return subteamTable

    'End Function
#End Region

End Class
