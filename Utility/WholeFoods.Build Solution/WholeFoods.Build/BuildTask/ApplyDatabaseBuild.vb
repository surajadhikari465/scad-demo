Imports Microsoft.Build.Utilities
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Imports System.Text
Imports System.Xml
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.Build.BuildTask
    Public Class ApplyDatabaseBuild
        Inherits Microsoft.Build.Utilities.Task
#Region "Property definitons"
        Private _sourceFiles As String
        Private _errorFile As String
        Private _targetDB As String
        Private _continueWhenDBErrors As Boolean
        Private _emailOnError As String
        Private _manualTimeout As Integer
#End Region

        Private Const EMAIL_HOST As String = "smtp.wholefoods.com"
        Private Const EMAIL_FROM As String = "irma.build@wholefoods.com"
        Private Const EMAIL_SUBJECT As String = "IRMA Build Error"

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method applies the changes listed in the database scripts to the target database.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering ApplyDatabaseBuild.Execute method: _sourceFiles=" + _sourceFiles + Environment.NewLine + _
                             ", _errorFile=" + _errorFile + Environment.NewLine + _
                             ", _targetDB=" + _targetDB + Environment.NewLine + _
                             ", _continueOnError=" + _continueWhenDBErrors.ToString + Environment.NewLine )
            Dim dbApplySuccess As Boolean = True
            Dim currentErrorSize As Long = 0
            Dim errorWriter As StreamWriter = Nothing

            ' Does an error file exist?  If so, delete it since we are starting a new execution.
            If File.Exists(_errorFile) Then
                Log().LogMessage("Deleting existing error file before processing begins")
                File.Delete(_errorFile)
            End If

            Dim factory As New DataFactory(_targetDB, _manualTimeout, True, False)
            ' Create a new transaction so that all of the db updates can be rolled back if an error is encountered
            Log().LogMessage("Creating the database transaction")
            'Dim transaction As SqlTransaction = Nothing

            'Try
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadUncommitted)
            'Catch ex As Exception
            '    Log().LogMessage("Transaction Error:")
            '    Log().LogMessage(ex.Message)
            '    If Not ex.InnerException Is Nothing Then
            '        Log().LogMessage(ex.InnerException.Message)
            'End If
            'Throw New Exception("Transaction failed")
            'End Try


            ' Split the different filenames into an array
            Dim delimiter() As Char = ",".ToCharArray
            Dim fileList() As String = _sourceFiles.Split(delimiter)
            Dim currentFile As String = Nothing
            For i As Integer = 0 To UBound(fileList)
                ' Execute the current file if it exists and has contents.
                currentFile = fileList(i)
                If File.Exists(currentFile) Then
                    Dim currentFileInfo As New FileInfo(currentFile)
                    If (currentFileInfo.Length >= 1) Then
                        Log().LogMessage("Executing file: " + currentFile)
                        factory.ExecuteFile(currentFile, _errorFile, transaction, _continueWhenDBErrors)

                        ' Did the file execute without logging an error?  
                        If File.Exists(_errorFile) Then
                            Dim errorFileInfo As New FileInfo(_errorFile)
                            If (errorFileInfo.Length >= 1) AndAlso (currentErrorSize <> errorFileInfo.Length) Then
                                ' New error messages were added during the processing of the last file
                                Log().LogMessage("Error processing file: " + currentFile)
                                dbApplySuccess = False

                                ' Add the name of the current file to the bottom of the error messages for that file
                                errorWriter = New StreamWriter(_errorFile, True)
                                errorWriter.WriteLine("=====================================================================")
                                errorWriter.WriteLine("Completed Processing File (errors listed above): " + currentFile)
                                errorWriter.WriteLine("=====================================================================")
                                errorWriter.Flush()
                                errorWriter.Close()

                                ' Store the new size of the error file
                                errorFileInfo.Refresh()
                                currentErrorSize = errorFileInfo.Length

                                ' If we do not continue processing once an error has been reached, 
                                ' do not keep going with the next file.
                                If Not _continueWhenDBErrors Then
                                    Exit For
                                End If
                            Else
                                Log().LogMessage("Success processing file: " + currentFile)
                            End If
                        Else
                            Log().LogMessage("Success processing file: " + currentFile)
                        End If
                    Else
                        ' The file exists, but it has a length of zero
                        Log().LogMessage("Skipping file with no content: " + currentFile)
                    End If
                Else
                    ' The file does not exist
                    Log().LogMessage("Skipping file that does not exist: " + currentFile)
                End If
            Next

            ' all file if any errors were encountered.
            If dbApplySuccess Then
                ' Commit the database transaction since all files executed successfully
                Log().LogMessage("Committing the database transactions")
                transaction.Commit()

                ' Delete the error log since there were no errors to report
                Log().LogMessage("Deleting the empty error file: " + _errorFile)
                File.Delete(_errorFile)
            Else
                ' Rollback all of the statements since there was at least one error
                Log().LogMessage("Rolling back the database transactions")
                transaction.Rollback()

                ' Send an email message with a link to the error file
                Log().LogMessage("Calling SendEmailErrorMessage")
                SendEmailErrorMessage()
            End If

            Log().LogMessage("Exiting ApplyDatabaseBuild.Execute method: " + dbApplySuccess.ToString())
            Return dbApplySuccess
        End Function

        Private Sub SendEmailErrorMessage()
            Log().LogMessage("Entering SendEmailErrorMessage")

            If Not (_emailOnError Is Nothing Or _emailOnError = "") Then
                'send error message using MailMessage
                Dim message As New MailMessage
                Dim mailClient As New SmtpClient
                Dim messageText As New StringBuilder

                message.From = New MailAddress(EMAIL_FROM)
                message.Subject = EMAIL_SUBJECT
                mailClient.Host = EMAIL_HOST

                Dim msgTo As String
                Dim msgToList As String() = _emailOnError.Split(";"c)
                For Each msgTo In msgToList
                    message.To.Add(New MailAddress(msgTo))
                Next

                'build the email message
                messageText.Append("Build error(s) encountered when applying the database updates.  All database scripts were rolled back.")
                messageText.Append(Environment.NewLine)
                messageText.Append("Database Scripts:")
                messageText.Append(_sourceFiles)
                messageText.Append(Environment.NewLine)
                messageText.Append("Target DB:")
                messageText.Append(_targetDB)
                messageText.Append(Environment.NewLine)
                messageText.Append(Environment.NewLine)
                messageText.Append("The error file is located here:")
                messageText.Append(_errorFile)

                'append user information to the message
                messageText.Append(Environment.NewLine)
                messageText.Append(Environment.NewLine)
                messageText.Append(String.Format("< Logged by user '{0}' on machine '{1}' ({2}) >", Environment.UserName, Environment.MachineName, Environment.OSVersion))

                message.Body = messageText.ToString()

                'deliver the message
                Log().LogMessage("Email message is being sent to the following recipients: " + _emailOnError)
                mailClient.Send(message)
            Else
                ' Email not sent because recipients are not defined in the build configuration
                Log().LogMessage("Email message is not sent because recipients are not defined")
            End If
            Log().LogMessage("Exiting SendEmailErrorMessage")
        End Sub
#End Region

#Region "Property access methods"
        ''' <summary>
        ''' A comma-separated list of source files to be run against the database.  
        ''' All of the files will be run in a single transaction and
        ''' rolled back on error.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceFiles() As String
            Get
                Return _sourceFiles
            End Get
            Set(ByVal value As String)
                _sourceFiles = value
            End Set
        End Property

        Public Property ManualTimeout() As Integer
            Get
                Return _manualTimeout
            End Get
            Set(ByVal value As Integer)
                _manualTimeout = value
            End Set
        End Property

        ''' <summary>
        ''' If errors are encountered during processing of the SourceFile on the TargetDB, the errors will be
        ''' written to this file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorFile() As String
            Get
                Return _errorFile
            End Get
            Set(ByVal value As String)
                _errorFile = value
            End Set
        End Property

        ''' <summary>
        ''' The connection string for the database where the source file is being applied.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetDB() As String
            Get
                Return _targetDB
            End Get
            Set(ByVal value As String)
                _targetDB = value
            End Set
        End Property

        ''' <summary>
        ''' TRUE if the application continues executing the current file and all other files in the list 
        ''' once an error is reached.  This allows for more errors to be identified with a single run of the
        ''' build.  All statements are wrapped in a single transaction, so nothing is committed to the database
        ''' if any errors are encountered.
        ''' 
        ''' FALSE if the application stops the execution of the current file and skips the execution of all other
        ''' files in the list when an error is reached.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContinueWhenDBErrors() As Boolean
            Get
                Return _continueWhenDBErrors
            End Get
            Set(ByVal value As Boolean)
                _continueWhenDBErrors = value
            End Set
        End Property

        ''' <summary>
        ''' Specify the recepient(s) for delivery of the email error notification if there is a database build failure.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmailOnError() As String
            Get
                Return _emailOnError
            End Get
            Set(ByVal value As String)
                _emailOnError = value
            End Set
        End Property
#End Region

    End Class
End Namespace
