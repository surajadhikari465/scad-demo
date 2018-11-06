Imports System.Configuration
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.Jobs
    Public Class PlumHostImportJob

        ''' <summary>
        ''' Flag to control processing behavior.  If true (default), then the store extract and ftp job will
        ''' run after the import job successfully completes.
        ''' </summary>
        ''' <remarks></remarks>
        Private _includeStoreExtract As Boolean = True

        ''' <summary>
        ''' PLUM Application Directory.  Used for the UI version of the application to indicate the install dir
        ''' on the user's machine.  This value is read from app.config for the server version of the application.
        ''' </summary>
        ''' <remarks></remarks>
        Private _plumAppDirectory As String = Nothing

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
        ''' The console application writes log data to a file.  This is a collection of messages to
        ''' include in this log.
        ''' </summary>
        ''' <remarks></remarks>
        Private _logMessages As List(Of String) = New List(Of String)

#Region "Property Access Methods"

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

        Public Property IncludeStoreExtract() As Boolean
            Get
                Return _includeStoreExtract
            End Get
            Set(ByVal value As Boolean)
                _includeStoreExtract = value
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
        ''' Process an error during processing.  Move the file to the error directory and send out an email
        ''' notification.  The ErrorHandler also logs the error to the system event log.
        ''' </summary>
        ''' <param name="errorNum"></param>
        ''' <param name="fi"></param>
        ''' <param name="importDir"></param>
        ''' <param name="ex"></param>
        ''' <remarks></remarks>
        Private Sub ProcessPLUMError(ByVal errorNum As ErrorType, ByRef fi As FileSystemInfo, ByRef importDir As String, Optional ByRef ex As Exception = Nothing)
            ' Process the error
            Dim args(2) As String
            args(0) = fi.Name
            args(1) = importDir & "\Error\"
            If ex Is Nothing Then
                ErrorHandler.ProcessError(errorNum, args, SeverityLevel.Fatal)
            Else
                ErrorHandler.ProcessError(errorNum, args, SeverityLevel.Fatal, ex)
            End If

            ' Move the file to the Error directory
            Dim errorDir As String = importDir & "\Error\"
            If Not Directory.Exists(errorDir) Then
                Directory.CreateDirectory(errorDir)
            End If
            FileSystem.FileCopy(importDir & "\" & fi.Name, errorDir & fi.Name)

            ' Update the error text in case this job was started with the UI
            Dim errorTxt As String = ""
            Select Case errorNum
                Case ErrorType.PLUMImport_Timeout
                    errorTxt = "Shell of PlumHost.exe timed out for file: " & fi.FullName
                Case ErrorType.PLUMImport_MissingCompleteFile
                    errorTxt = "PLUM Import Complete file not found for file: " & fi.FullName
                Case ErrorType.PLUMImport_Failed
                    errorTxt = "Exception during processing of PLUM Import file: " & fi.FullName
            End Select
            _errorMessage = _errorMessage & Environment.NewLine & errorTxt
            If ex Is Nothing Then
                Logger.LogError(errorTxt, Me.GetType())
            Else
                Logger.LogError(errorTxt, Me.GetType(), ex)
                _errorException = ex
            End If
        End Sub

        ''' <summary>
        ''' Kicks off the PLUM Host Import job, processing any of the PLUM extract files that are
        ''' waiting on the application server to be imported into PLUM host.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main() As Boolean
            Logger.LogDebug("Main entry", Me.GetType())
            Dim plumSuccess As Boolean = True    ' Flag set to true if the entire process completes successfully - for UI
            Dim importSuccess As Boolean = False ' Flag set to true if at least one file was imported - for export job logic

            Try
                'Get the directories
                Dim importDir As String = ConfigurationServices.AppSettings("PlumImportDir")
                Dim plumTimeout As Integer = CInt(ConfigurationServices.AppSettings("plumHostImportTimeout"))

                ' if the application dir wasn't set on the job startup, read it from the property file
                If _plumAppDirectory Is Nothing Or _plumAppDirectory = "" Then
                    _plumAppDirectory = ConfigurationServices.AppSettings("PlumHostAppDir")
                End If

                'Sort the file list in name order (name is PLUMYYYYMMDDHHMMSS.DAT)
                ScheduledLog("Starting PLUM Import job to process files in directory: " & importDir)
                Dim dir As New DirectoryInfo(importDir)
                Dim fsi As FileSystemInfo() = dir.GetFileSystemInfos
                Dim fileComparer As New FileNameComparerBO
                Array.Sort(fsi, fileComparer)

                'Process the files
                Dim fi As FileSystemInfo
                Dim iShellResult1 As Long
                For Each fi In fsi
                    'If Not Directory.Exists(fi.FullName) Then 'Skip directories
                    If TypeOf fi Is System.IO.FileInfo Then 'Skip directories
                        Try
                            ' Run the plumhost import command for the file
                            ScheduledLog("Starting auto import command for file: " & fi.FullName)
                            iShellResult1 = Shell(_plumAppDirectory & "\Bin\Plumhost.exe -a -i -fi " & fi.FullName & " -n ODBCHost", , True, plumTimeout)
                            If iShellResult1 <> 0 Then
                                ' Timeout Error - process the error and move the file to the Error directory
                                ScheduledLog("Shell timeout.  The PLUM Import will continue, but the IRMA job will not complete any more processing.")
                                ProcessPLUMError(ErrorType.PLUMImport_Timeout, fi, importDir)
                                plumSuccess = False
                            Else
                                If FileSystem.Dir(_plumAppDirectory & "\Log\ODBC*_IC").Length = 0 Then
                                    ' Processing Error - process the error and move the file to the Error directory
                                    ScheduledLog("Shell command executed successfully, but there was a PLUM processing error.  Moving the file to the Error directory.")
                                    ProcessPLUMError(ErrorType.PLUMImport_MissingCompleteFile, fi, importDir)
                                    plumSuccess = False
                                Else
                                    ' Success - move the file to the Archive directory
                                    ScheduledLog("Import was a success.  Archiving the IRMA batch file")
                                    Dim archiveDir As String = importDir & "\Archive\"
                                    If Not Directory.Exists(archiveDir) Then
                                        Directory.CreateDirectory(archiveDir)
                                    End If
                                    FileSystem.FileCopy(importDir & "\" & fi.Name, archiveDir & fi.Name)
                                    importSuccess = True

                                    ' Archive the log file so we can check to see if new warnings appear during the next run
                                    ScheduledLog("Import was a success.  Archiving the AutoPlum log file.")
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
                                            ScheduledLog("Import was a success.  AutoPlum log file contains warning(s).  Sending email to notify the region.")
                                            Dim args(1) As String
                                            args(0) = fi.FullName
                                            ErrorHandler.ProcessError(ErrorType.PLUMImport_AutoPlumLog, args, SeverityLevel.Warning)
                                            Exit For
                                        End If
                                    Next

                                End If
                            End If
                        Catch ex As Exception
                            ' Exception during processing - process the error and move the file to the Error directory
                            ScheduledLog("Exception during processing.  The Import failed.")
                            ProcessPLUMError(ErrorType.PLUMImport_Failed, fi, importDir, ex)
                            plumSuccess = False
                        End Try
                        Try
                            ' Delete the file that has been processed - a copy exists in the archive or error dir
                            FileSystem.Kill(fi.FullName)
                        Catch ex As Exception
                            ' Exception during processing - handle this if an error has not already been reported
                            If plumSuccess Then
                                Dim args(1) As String
                                args(0) = fi.FullName
                                ErrorHandler.ProcessError(ErrorType.PLUMImport_DeleteFailed, args, SeverityLevel.Warning, ex)
                                _errorMessage = ex.Message()
                                _errorException = ex
                            End If
                        End Try
                    End If
                Next
            Catch ex As Exception
                ' Exception during processing - handle this if an error has not already been reported
                If plumSuccess Then
                    ScheduledLog("Exception during processing.  The Import failed.")
                    ErrorHandler.ProcessError(ErrorType.PLUMImport_Failed, SeverityLevel.Fatal, ex)
                    Logger.LogError("Exception during processing of PLUM Import files.", Me.GetType(), ex)
                    _errorMessage = ex.Message()
                    _errorException = ex
                    plumSuccess = False
                End If
            End Try

            'Kick off the export to plum store
            If importSuccess AndAlso _includeStoreExtract Then
                Dim storeExtract As New PlumStoreExtractJob
                storeExtract.PlumAppDirectory = _plumAppDirectory
                Dim storeSuccess As Boolean = storeExtract.Main()
                ' Add the log messages to the queue
                _logMessages.AddRange(storeExtract.LogMessages)
                If Not storeSuccess Then
                    ' The store exports failed
                    plumSuccess = False
                    ' Update the error and exception text for the UI
                    If storeExtract.ErrorMessage IsNot Nothing Then
                        _errorMessage = storeExtract.ErrorMessage
                    End If
                    If storeExtract.ErrorException IsNot Nothing Then
                        _errorException = storeExtract.ErrorException
                    End If
                End If
            End If

            Logger.LogDebug("Main exit: " & plumSuccess.ToString(), Me.GetType())
            Return plumSuccess
        End Function

    End Class
End Namespace
