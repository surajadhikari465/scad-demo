Imports System.Configuration
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.Utility
Imports WholeFoods.Utility.FTP

Namespace WholeFoods.IRMA.Replenishment.Jobs
    Public Class PlumStoreExtractJob
        ''' <summary>
        ''' Flag to control processing behavior.  If true (default), then the store extract and ftp job will
        ''' run.  If it is set to false, only the FTP job will run.  This allows for extract files to be pushed
        ''' down to the stores.
        ''' </summary>
        ''' <remarks></remarks>
        Private _runExtractProcess As Boolean = True

        ''' <summary>
        ''' Contains a message describing the error condition if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Contains any exception caught during processing if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorException As Exception

        ''' <summary>
        ''' PLUM Application Directory.  Used for the UI version of the application to indicate the install dir
        ''' on the user's machine.  This value is read from app.config for the server version of the application.
        ''' </summary>
        ''' <remarks></remarks>
        Private _plumAppDirectory As String = Nothing

        ''' <summary>
        ''' The console application writes log data to a file.  This is a collection of messages to
        ''' include in this log.
        ''' </summary>
        ''' <remarks></remarks>
        Private _logMessages As List(Of String) = New List(Of String)

#Region "Property Access Methods"
        Public Property RunExtractProcess() As Boolean
            Get
                Return _runExtractProcess
            End Get
            Set(ByVal value As Boolean)
                _runExtractProcess = value
            End Set
        End Property

        Public Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal Value As String)
                _errorMessage = Value
            End Set
        End Property

        Public Property ErrorException() As Exception
            Get
                Return _errorException
            End Get
            Set(ByVal Value As Exception)
                _errorException = Value
            End Set
        End Property

        Public Property PlumAppDirectory() As String
            Get
                Return _plumAppDirectory
            End Get
            Set(ByVal value As String)
                _plumAppDirectory = value
            End Set
        End Property

        Public Property LogMessages() As List(Of String)
            Get
                Return _logMessages
            End Get
            Set(ByVal value As List(Of String))
                _logMessages = value
            End Set
        End Property

#End Region
        ''' <summary>
        ''' Add a status message to the scheduled job log file during application execution.
        ''' </summary>
        ''' <param name="msg">text of the message</param>
        ''' <remarks></remarks>
        Private Sub ScheduledLog(ByVal msg As String)
            _logMessages.Add(DateTime.Now.ToString() & " " & msg)
        End Sub

        ''' <summary>
        ''' Execute the PlumHost command to merge and export the PLUM Host data.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function PLUMMergeAndExport() As Boolean
            Dim exportSuccess As Boolean = True

            'Get the directories
            Dim exportDir As String = ConfigurationServices.AppSettings("PlumExportDir")
            Dim plumTimeout As Integer = CInt(ConfigurationServices.AppSettings("plumHostExportTimeout"))

            'Kick off the export to Plum Store
            Dim iShellResult1 As Long
            ScheduledLog("Starting auto merge and export command")
            iShellResult1 = Shell(_plumAppDirectory & "\Bin\Plumhost.exe -a -e -m -n ODBCHost", , True, plumTimeout)
            If iShellResult1 <> 0 Then
                ' Timeout Error - process the error 
                ScheduledLog("Shell timeout.  The PLUM Merge & Export will continue, but the IRMA job will not complete any more processing.")
                ErrorHandler.ProcessError(ErrorType.PLUMExport_Timeout, SeverityLevel.Fatal)
                exportSuccess = False
            Else
                If FileSystem.Dir(_plumAppDirectory & "\Log\ODBC*_MC").Length = 0 Then
                    ' Processing Error - process the error 
                    ScheduledLog("Shell command executed successfully, but there was a PLUM processing error.")
                    ErrorHandler.ProcessError(ErrorType.PLUMExport_MissingCompleteFile, SeverityLevel.Fatal)
                    exportSuccess = False
                Else
                    ' Success
                    ' Archive the log file so we can check to see if new warnings appear during the next run
                    ScheduledLog("Merge & export was a success.  Archiving the AutoPlum log file.")
                    ArchivePLUMLog()
                End If
            End If
            Return exportSuccess
        End Function

        ''' <summary>
        ''' Archive the AutoPlum log.  This allows the PLUM processes to scan the log file for new
        ''' warnings during processing.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ArchivePLUMLog()
            ' Archive the log file so we can check to see if new warnings appear during the next run
            Dim archiveLogName As New StringBuilder
            archiveLogName.Append(_plumAppDirectory)
            archiveLogName.Append("\Log\AutoPlum")
            archiveLogName.Append(Date.Now.Year.ToString("0000"))
            archiveLogName.Append(Date.Now.Month.ToString("00"))
            archiveLogName.Append(Date.Now.Day.ToString("00"))
            archiveLogName.Append(Date.Now.Hour.ToString("00"))
            archiveLogName.Append(Date.Now.Minute.ToString("00"))
            archiveLogName.Append(Date.Now.Second.ToString("00"))
            archiveLogName.Append(Date.Now.Millisecond.ToString("000"))
            archiveLogName.Append(".Err")
            File.Move(_plumAppDirectory & "\Log\AutoPlum.Err", archiveLogName.ToString)

            ' Does the log file include any WARNING messages
            Dim logFile As String() = File.ReadAllLines(archiveLogName.ToString)
            Dim logRec As String
            For Each logRec In logFile
                If logRec.Contains("WARNING") Then
                    ' Send out a warning email message to alert the region 
                    ScheduledLog("Merge & export was a success.  AutoPlum log file contains warning(s).  Sending email to notify the region.")
                    Dim args(1) As String
                    args(0) = "Generate Store Export File"
                    ErrorHandler.ProcessError(ErrorType.PLUMImport_AutoPlumLog, args, SeverityLevel.Warning)
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Move the PLUM Host export file to an Archive directory.
        ''' </summary>
        ''' <param name="exportDir"></param>
        ''' <param name="exportFileWithPath"></param>
        ''' <remarks></remarks>
        Private Sub ArchivePLUMHostFile(ByVal exportDir As String, ByVal exportFileWithPath As String)
            ' add the current timestamp to the file name
            Dim archiveFileName As New StringBuilder
            archiveFileName.Append(exportDir)
            archiveFileName.Append("\Archive\")
            archiveFileName.Append(Path.GetFileNameWithoutExtension(exportFileWithPath))
            archiveFileName.Append("_")
            archiveFileName.Append(Date.Now.Year.ToString("0000"))
            archiveFileName.Append(Date.Now.Month.ToString("00"))
            archiveFileName.Append(Date.Now.Day.ToString("00"))
            archiveFileName.Append(Date.Now.Hour.ToString("00"))
            archiveFileName.Append(Date.Now.Minute.ToString("00"))
            archiveFileName.Append(Date.Now.Second.ToString("00"))
            archiveFileName.Append(Date.Now.Millisecond.ToString("000"))
            archiveFileName.Append(Path.GetExtension(exportFileWithPath))
            ' move the file to the archive directory
            ScheduledLog("Archiving file: " & exportFileWithPath & ", to: " & archiveFileName.ToString)
            File.Move(exportFileWithPath, archiveFileName.ToString)
        End Sub

        ''' <summary>
        ''' Move a copy of the PLUM Host export file to an Error directory, by store.  Append the current timestamp to
        ''' the file so they can be reprocessed in the order they were created when FTP communication is restored for
        ''' the store.
        ''' </summary>
        ''' <param name="exportDir"></param>
        ''' <param name="exportFileWithPath"></param>
        ''' <param name="storeNo"></param>
        ''' <param name="storeIP"></param>
        ''' <remarks></remarks>
        Private Sub CreatePLUMHostStoreError(ByVal exportDir As String, ByVal exportFileWithPath As String, ByVal storeNo As String, ByVal storeIP As String)
            ' add the current timestamp to the file name
            Dim errorFileName As New StringBuilder
            errorFileName.Append(exportDir)
            errorFileName.Append("\Error\")
            errorFileName.Append(storeNo)
            errorFileName.Append("\")
            errorFileName.Append(Path.GetFileNameWithoutExtension(exportFileWithPath))
            errorFileName.Append("_")
            errorFileName.Append(Date.Now.Year.ToString("0000"))
            errorFileName.Append(Date.Now.Month.ToString("00"))
            errorFileName.Append(Date.Now.Day.ToString("00"))
            errorFileName.Append(Date.Now.Hour.ToString("00"))
            errorFileName.Append(Date.Now.Minute.ToString("00"))
            errorFileName.Append(Date.Now.Second.ToString("00"))
            errorFileName.Append(Date.Now.Millisecond.ToString("000"))
            errorFileName.Append(Path.GetExtension(Path.Combine(exportDir, exportFileWithPath)))
            ' move a copy of the file to the error directory for the store
            ScheduledLog("Creating a copy of file: " & exportFileWithPath & ", in: " & errorFileName.ToString)
            File.Copy(exportFileWithPath, errorFileName.ToString)

            ' Send out a warning email message to alert the region 
            Dim args(3) As String
            args(0) = errorFileName.ToString
            args(1) = storeNo
            args(2) = storeIP
            ErrorHandler.ProcessError(ErrorType.PLUMExport_StoreFTPError, args, SeverityLevel.Warning)

            ' Update the UI error message
            If _errorMessage Is Nothing Or _errorMessage <> "" Then
                _errorMessage = "PLUM Host Merge and Export process completed, but the export file was not successfully FTP'd to all of the stores with configuration data."
            End If
        End Sub

        ''' <summary>
        ''' Reprocess any files that were exported from PLUM Host but not successfully FTPed to PLUM Store.  The files
        ''' are reprocessed in the order they were created.  This allows for PLUM to self-correct if communication 
        ''' to a store goes down. 
        ''' </summary>
        ''' <param name="exportDir"></param>
        ''' <param name="currentStore"></param>
        ''' <param name="storeFilename"></param>
        ''' <param name="ftpClient"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ProcessPLUMHostStoreErrors(ByVal exportDir As String, ByRef currentStore As StoreFTPConfigBO, ByVal storeFilename As String, ByRef ftpClient As FTP.FTPclient) As Boolean
            Dim errorSuccess As Boolean = True

            ' create the error dir for the store if it does not already exist
            Dim errorDir As New StringBuilder
            errorDir.Append(exportDir)
            errorDir.Append("\Error\")
            errorDir.Append(currentStore.StoreNo.ToString)
            If Not Directory.Exists(errorDir.ToString) Then
                Directory.CreateDirectory(errorDir.ToString)
            End If

            'Sort the list of error files for the store in name order (name is HOSTCHNG_YYYYMMDDHHMMSS.DAT)
            Dim dir As New DirectoryInfo(errorDir.ToString)
            Dim fsi As FileSystemInfo() = dir.GetFileSystemInfos
            Dim fileComparer As New FileNameComparerBO
            Array.Sort(fsi, fileComparer)

            'Process the error files
            ScheduledLog("Reprocessing any error files in the directory: " & errorDir.ToString)
            Dim fi As FileSystemInfo
            For Each fi In fsi
                'If Not Directory.Exists(fi.FullName) Then 'Skip directories
                If TypeOf fi Is System.IO.FileInfo Then 'Skip directories
                    Try
                        ScheduledLog("Reprocessing file: " & fi.FullName)
                        ftpClient.Upload(fi.FullName, Path.Combine(currentStore.ChangeDirectory, storeFilename), True)
                    Catch ex As Exception
                        ' Exception during processing - no need to handle this because the file is already in the 
                        ' error directory
                        ScheduledLog("Error during reprocessing.  Error file(s) will be tried again during next run of the job.")
                        errorSuccess = False
                        Exit For
                    End Try
                    Try
                        ' Delete the file that has been reprocessed - a copy already exists in the archive or error dir
                        FileSystem.Kill(fi.FullName)
                    Catch ex As Exception
                        ' Exception during deletion of the error file that was successfully reprocessed
                        Dim args(1) As String
                        args(0) = fi.FullName
                        ErrorHandler.ProcessError(ErrorType.PLUMExport_DeleteErrorFileFailed, args, SeverityLevel.Warning, ex)
                        _errorMessage = ex.Message()
                        _errorException = ex
                    End Try
                End If
            Next
            Return errorSuccess
        End Function

        ''' <summary>
        ''' FTP a PLUM Host file to the PLUM store servers for the stores that are configured to receive the file.
        ''' Stores are configured to receive PLUM store files in the StoreFTPConfig table.  
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function DeliverExportFileToPLUMStores() As Boolean
            Dim deliverSuccess As Boolean = True
            Dim ftpClient As FTP.FTPclient = Nothing
            Dim storeDAO As New StoreFTPConfigDAO
            Dim storeConfigs As ICollection
            Dim currentStore As StoreFTPConfigBO

            ' Get the directories and filenames
            Dim exportDir As String = ConfigurationServices.AppSettings("PlumExportDir")
            Dim exportFilename As String = ConfigurationServices.AppSettings("PlumExportFilename")
            Dim storeFilename As String = ConfigurationServices.AppSettings("PlumStoreFilename")
            Dim exportFileWithPath As String

            ' Read the FTP Configuration for each of the stores
            storeConfigs = storeDAO.GetFTPConfigDataForWriterType(Constants.FileWriterType_PLUMSTORE).Values()

            ' Process each store - first reprocess any error files, and then process the current file
            For Each currentStore In storeConfigs
                ScheduledLog("Processing store " & currentStore.StoreNo & ": " & currentStore.IPAddress & ".")

                ' Build the filename - add the store number to the filename
                exportFileWithPath = Path.Combine(exportDir, exportFilename & ".S" & currentStore.StoreNo.ToString("###"))

                ' Check to see if a current file exists.  If not, we are only processing error files for the store.
                Dim currentFile As Boolean = File.Exists(exportFileWithPath)

                Try
                    ' create an FTP client for this store with its FTP config settings
                    ftpClient = New FTP.FTPclient(currentStore.IPAddress, currentStore.FTPUser, currentStore.FTPPassword, currentStore.IsSecureTransfer)

                    ' check to see if there are any error files for this store - they should be resent to the store before
                    ' sending the current file
                    If ProcessPLUMHostStoreErrors(exportDir, currentStore, storeFilename, ftpClient) Then
                        ' Verify the export file exists
                        If currentFile Then
                            ' ftp current the file
                            ScheduledLog("Export file exists: " & exportFileWithPath & ".  FTP the file to PLUM Store: " & currentStore.StoreNo.ToString & ".")
                            ftpClient.Upload(exportFileWithPath, Path.Combine(currentStore.ChangeDirectory, storeFilename), True)
                            ScheduledLog("FTP to the store succeeded.")
                        Else
                            Console.WriteLine("The export file does not exist, so the FTP process did not run:  " & exportFileWithPath)
                        End If
                    ElseIf currentFile Then
                        ' Error handling failed 
                        deliverSuccess = False
                        ScheduledLog("The error handling process failed for the store.  Sending an email alert and creating a copy of the current export file for reprocessing.")

                        ' Place a copy of the current file in the Export Error Directory for this store -
                        ' it will be picked up on the next run of the job
                        CreatePLUMHostStoreError(exportDir, exportFilename, currentStore.StoreNo.ToString, currentStore.IPAddress)
                    Else
                        ' Error handling failed but there is not a current file to handle
                        ScheduledLog("The error handling process failed for the store.  There is not a current export file to process.")
                    End If
                Catch ex As Exception
                    If currentFile Then
                        ' There was a new file that was not processed for the store.  Handle the error.
                        deliverSuccess = False
                        ScheduledLog("FTP to the store failed.  Sending an email alert and creating a copy of the export file for reprocessing.")

                        ' Place a copy of the file in the Export Error Directory for this store -
                        ' it will be picked up on the next run of the job
                        CreatePLUMHostStoreError(exportDir, exportFileWithPath, currentStore.StoreNo.ToString, currentStore.IPAddress)
                    End If
                End Try

                ' After the file has been processed for the store, move the file to the archive directory
                If currentFile Then
                    ScheduledLog("FTP completed.  Archiving the current PLUM Host export file for the store: " & exportFilename)
                    ArchivePLUMHostFile(exportDir, exportFileWithPath)
                End If
            Next

            Return deliverSuccess
        End Function

        ''' <summary>
        ''' Kicks off the PLUM Store Export job, exporting data from PLUM Host and sending it to
        ''' PLUM Store for processing.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main() As Boolean
            Logger.LogDebug("Main entry", Me.GetType())
            Dim plumSuccess As Boolean = True    ' Flag set to true if the entire process completes successfully - for UI

            Try
                ' if the application dir wasn't set on the job startup, read it from the property file
                If _plumAppDirectory Is Nothing Or _plumAppDirectory = "" Then
                    _plumAppDirectory = ConfigurationServices.AppSettings("PlumHostAppDir")
                End If

                ' The PLUM export process will overwrite any existing extract files.  To keep from losing data
                ' that should be sent to the stores, run the store FTP process before starting the merge and
                ' export.  This will reprocess any error files that exist for a store.
                Dim ftpSuccess As Boolean = DeliverExportFileToPLUMStores()

                ' Run the merge and extract process to generate the current file.
                Dim mergeSuccess As Boolean = True
                If _runExtractProcess Then
                    ' Merge and export the files - generating the file that is sent from PLUM Host to PLUM Stores
                    mergeSuccess = PLUMMergeAndExport()
                End If

                ' Set the PLUM success flag
                plumSuccess = ftpSuccess AndAlso mergeSuccess

                ' FTP the export file to each of the PLUM Store servers
                If mergeSuccess Then
                    If Not DeliverExportFileToPLUMStores() Then
                        plumSuccess = False
                    End If
                End If
            Catch ex As Exception
                ' Exception during processing
                ScheduledLog("Exception during processing.  The merge and export failed.")
                ErrorHandler.ProcessError(ErrorType.PLUMExport_Failed, SeverityLevel.Fatal, ex)
                Logger.LogError("Exception during processing of PLUM Export files.", Me.GetType(), ex)
                _errorMessage = ex.Message()
                _errorException = ex
                plumSuccess = False
            End Try

            Logger.LogDebug("Main exit: " & plumSuccess.ToString(), Me.GetType())
            Return plumSuccess
        End Function

    End Class
End Namespace

