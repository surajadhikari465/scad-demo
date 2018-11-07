Option Explicit On
Option Strict On

Imports System.Configuration
Imports System.Text
Imports System.Net.Mail

Namespace WholeFoods.Utility

    Public Enum SeverityLevel
        Warning
        Fatal
    End Enum

    ' error types it handles
    Public Enum ErrorType
        GeneralApplicationError
        DataFactoryException
        FTPException
        SSHException
        POSPush_StoreConfig
        POSPush_StoreNotFound
        POSPush_ApplyChangesInIRMA
        POSPush_AdminError
        POSPush_RunRemoteProcess
        POSPush_ProcessorRetrievalException
        POSPush_ApplyDeAuthChangesInIRMA
        POSAudit_ProcessorRetrievalException
        PLUMImport_Timeout
        PLUMImport_MissingCompleteFile
        PLUMImport_Failed
        PLUMImport_DeleteFailed
        PLUMImport_AutoPlumLog
        PLUMExport_Timeout
        PLUMExport_MissingCompleteFile
        PLUMExport_Failed
        PLUMExport_StoreFTPError
        PLUMExport_DeleteErrorFileFailed
        ScalePush_StoreNotFound
        ScalePush_ApplyAuthChangesInIRMA
        ScalePush_ApplyDeAuthChangesInIRMA
        PeopleSoftUpload_FileError
        ScheduledJob_ProcessingError
        ScheduledJob_DidNotRun
        Administration_UserStoreTeamTitle
        Administration_UserSubTeam
        Administration_UserDisableAccount
        ConfigurationManagerException
        HouseTlogParser_CouldNotParseTlog
    End Enum

    Public Class ErrorHandler
        Private Const DEFAULT_EMAIL_FROM As String = "irma.application@wholefoods.com"
        Private Const DEFAULT_EMAIL_SUBJECT As String = "IRMA Application Error"

        Public Shared Sub ProcessError(ByVal message As ErrorType, ByVal level As SeverityLevel)
            SendMessage(message, Nothing, level, Nothing)
        End Sub

        Public Shared Sub ProcessError(ByVal message As ErrorType, ByVal args() As String, ByVal level As SeverityLevel)
            SendMessage(message, args, level, Nothing)
        End Sub

        Public Shared Sub ProcessError(ByVal message As ErrorType, ByVal level As SeverityLevel, ByVal exception As System.Exception)
            SendMessage(message, Nothing, level, exception)
        End Sub

        Public Shared Sub ProcessError(ByVal message As ErrorType, ByVal args() As String, ByVal level As SeverityLevel, ByVal exception As System.Exception)
            SendMessage(message, args, level, exception)
        End Sub

        ''' <summary>
        ''' Sends a page and/or email to warn about system failure.  SeverityLevel used to 
        ''' determing if page should be sent.  Otherwise only email sent to configured email
        ''' addresses.
        ''' Also logs the error to the application event log.
        ''' </summary>
        ''' <param name="errorNo"></param>
        ''' <param name="args">Error message parameters</param>
        ''' <param name="level"></param>
        ''' <param name="exception"></param>
        ''' <remarks></remarks>
        Private Shared Sub SendMessage(ByVal errorNo As ErrorType, ByVal args() As String, ByVal level As SeverityLevel, ByVal exception As System.Exception)
            'send error message using MailMessage
            Dim message As New MailMessage
            Dim mailClient As New SmtpClient
            Dim messageText As New StringBuilder
            Dim appEventLog As New EventLog

            Try
                'get base message based on error type
                messageText.Append(GetMessage(errorNo, args))

                'append exception to mail message
                If exception IsNot Nothing Then
                    messageText.Append(Environment.NewLine)
                    messageText.Append(Environment.NewLine)
                    messageText.Append(exception.ToString())
                End If

                'append user information to the message
                messageText.Append(Environment.NewLine)
                messageText.Append(Environment.NewLine)
                messageText.Append(String.Format("< Logged by user '{0}' on machine '{1}' ({2}) >", Environment.UserName, Environment.MachineName, Environment.OSVersion))

                ' create an event log for error handling
                appEventLog.Source = "IRMA"
                appEventLog.WriteEntry(messageText.ToString, EventLogEntryType.Error)

                'send email
                'build the message
                Dim msgFromStr As String = ConfigurationServices.AppSettings("errorNotificationFrom")
                If msgFromStr Is Nothing Or msgFromStr = "" Then
                    msgFromStr = DEFAULT_EMAIL_FROM
                End If
                message.From = New MailAddress(msgFromStr)

                Dim sendEmail As Boolean = True
                Dim msgTo As String
                Dim msgToStr As String = ConfigurationServices.AppSettings("primaryErrorNotification")
                If msgToStr Is Nothing Or msgToStr = "" Then
                    ' "TO" was not defined, do not send the message
                    sendEmail = False
                End If
                Dim msgToList As String() = msgToStr.Split(";"c)
                For Each msgTo In msgToList
                    message.To.Add(New MailAddress(msgTo))
                Next

                Dim msgCopy As String
                Dim msgCopyStr As String = ConfigurationServices.AppSettings("secondaryErrorNotification")
                If Not msgCopyStr Is Nothing AndAlso msgCopyStr <> "" Then
                    Dim msgCopyList As String() = msgCopyStr.Split(";"c)
                    For Each msgCopy In msgCopyList
                        message.CC.Add(New MailAddress(msgCopy))
                    Next
                End If

                Dim msgSubStr As String = ConfigurationServices.AppSettings("errorSubject")
                If msgSubStr Is Nothing Or msgSubStr = "" Then
                    msgSubStr = DEFAULT_EMAIL_SUBJECT
                End If
                message.Subject = msgSubStr

                message.Body = messageText.ToString()

                'deliver the message
                If sendEmail Then
                    mailClient.Send(message)
                End If

                'send page based on SeverityLevel
                Select Case level
                    Case SeverityLevel.Fatal
                        'send page

                End Select
            Catch ex As Exception
                ' error processing failed
            End Try
        End Sub

        ''' <summary>
        ''' Gets specific error message based on ErrorType passed in
        ''' </summary>
        ''' <param name="errorNo"></param>
        ''' <param name="args"></param>
        ''' <returns>String message</returns>
        ''' <remarks></remarks>
        Private Shared Function GetMessage(ByVal errorNo As ErrorType, ByVal args() As String) As String
            Dim message As New StringBuilder
            Dim iArg As Integer

            Select Case errorNo
                Case ErrorType.GeneralApplicationError
                    message.Append("One of the IRMA Applications encountered an unexpected error during processing.")
                    message.Append(Environment.NewLine)
                    message.Append("The exception stack trace is included in the email message to provide details about the error.")
                    If args IsNot Nothing Then
                        For iArg = args.GetLowerBound(0) To args.GetUpperBound(0)
                            message.Append(Environment.NewLine)
                            message.Append(args(iArg))
                        Next
                    End If
                Case ErrorType.DataFactoryException
                    message.Append("One of the IRMA Applications encountered an unexpected error during processing when accessing the database.")
                    message.Append(Environment.NewLine)
                    message.Append("The exception stack trace is included in the email message to provide details about the error.")
                    If args IsNot Nothing Then
                        For iArg = args.GetLowerBound(0) To args.GetUpperBound(0)
                            message.Append(Environment.NewLine)
                            message.Append(args(iArg))
                        Next
                    End If
                Case ErrorType.SSHException
                    message.Append("SSH Exception has occurred processing Store: ")
                    If args IsNot Nothing Then
                        message.Append(args(0))
                        message.Append(Environment.NewLine)
                        message.Append("SSH Server: ")
                        message.Append(args(1))
                        message.Append(Environment.NewLine)
                        message.Append("SSH User: ")
                        message.Append(args(2))
                    End If
                Case ErrorType.FTPException
                    message.Append("FTP Exception has occurred processing Store: ")
                    If args IsNot Nothing Then
                        message.Append(args(0))
                        message.Append(Environment.NewLine)
                        message.Append("FTP Server: ")
                        message.Append(args(1))
                        message.Append(Environment.NewLine)
                        message.Append("FTP User: ")
                        message.Append(args(2))
                    End If
                Case ErrorType.POSPush_StoreConfig
                    message.Append("POSPush Error Processing Store(s).  Check the store(s) configuration in the following database tables: ")
                    message.Append(Environment.NewLine)
                    message.Append("StorePOSConfig")
                    message.Append(Environment.NewLine)
                    message.Append("POSWriter")
                    message.Append(Environment.NewLine)
                    message.Append("POSWriterFileConfig")
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    If args IsNot Nothing Then
                        For iArg = args.GetLowerBound(0) To args.GetUpperBound(0)
                            message.Append(Environment.NewLine)
                            message.Append(args(iArg))
                        Next
                    End If
                Case ErrorType.POSPush_StoreNotFound
                    message.Append("Store: ")

                    If args IsNot Nothing Then
                        message.Append(args(0))
                    End If

                    message.Append(" is NOT configured in StorePOSConfig table.  The POS Push process will not successfully execute for this store.")
                Case ErrorType.POSPush_ProcessorRetrievalException
                    message.Append("POS Push error when parsing the data and adding it to the IRMA output file.  This is typically caused by a data error.")
                    message.Append(Environment.NewLine)
                    message.Append("Change Type Being Processed: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("Current Store Being Processed:")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("# Successful Lines Written to Store File: ")
                    message.Append(args(2))
                    message.Append(Environment.NewLine)
                    message.Append("Current Row # Being Processed:")
                    message.Append(args(3))
                    message.Append(Environment.NewLine)
                    message.Append("Current Column # Being Processed:")
                    message.Append(args(4))
                    message.Append(Environment.NewLine)
                    message.Append("Current Field ID Being Processed:")
                    message.Append(args(5))
                    message.Append(Environment.NewLine)

                Case ErrorType.POSAudit_ProcessorRetrievalException
                    message.Append("Error when parsing the data and adding it to the IRMA output file during the Build Complete POS File process.  This is typically caused by a data error.")
                    message.Append(Environment.NewLine)
                    message.Append("Change Type Being Processed: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("Current Store Being Processed:")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("# Successful Lines Written to Store File: ")
                    message.Append(args(2))
                    message.Append(Environment.NewLine)
                    message.Append("Current Row # Being Processed:")
                    message.Append(args(3))
                    message.Append(Environment.NewLine)
                    message.Append("Current Column # Being Processed:")
                    message.Append(args(4))
                    message.Append(Environment.NewLine)
                    message.Append("Current Field ID Being Processed:")
                    message.Append(args(5))
                    message.Append(Environment.NewLine)

                Case ErrorType.POSPush_ApplyChangesInIRMA
                    'passing in change type string and error text
                    message.Append("POS Push Error when processing the Change Type: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    message.Append("Data was FTP'd to the Store POS systems, but IRMA data was not updated to reflect this as follows:")
                    message.Append(Environment.NewLine)
                    message.Append(args(1))
                Case ErrorType.POSPush_AdminError
                    ' passing in description of UI action where error occurred
                    message.Append("The POS Push Admin UI encountered an error when processing a request.")
                    message.Append(Environment.NewLine)
                    If args IsNot Nothing Then
                        For iArg = args.GetLowerBound(0) To args.GetUpperBound(0)
                            message.Append(Environment.NewLine)
                            message.Append(args(iArg))
                        Next
                    End If
                Case ErrorType.POSPush_ApplyDeAuthChangesInIRMA
                    'passing in change type string and error text
                    message.Append("POS Push Error when processing the Change Type: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    message.Append("Data was FTP'd to the Store POS systems, but IRMA data in the StoreItem table was not updated to reset the POSDeAuth flag.")
                    message.Append(Environment.NewLine)
                    message.Append(args(1))
                Case ErrorType.PLUMImport_Timeout
                    message.Append("Shell of PlumHost.exe timed out when processing PLUM Import file: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the error directory on the server running the PLUM Import job: ")
                    message.Append(ReplaceLocalhost(args(1)))
                    message.Append(Environment.NewLine)
                    message.Append("The PLUM Import process will continue to run until completed, but the IRMA Scheduled job that calls this process has ended.  ")
                    message.Append("This means that the export from PLUM Host to PLUM Store has not been started for this file.  It also means that if there are other ")
                    message.Append("import batches pending, they will not be imported into PLUM Host during this execution of the scheduled job.")
                Case ErrorType.PLUMImport_MissingCompleteFile
                    message.Append("PLUM Import Complete file not found for file: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the fromirma\Error directory on the server running the PLUM Import job.  This is typically the region's IRMA application server.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_Failed
                    If args IsNot Nothing Then
                        message.Append("Exception during processing of PLUM Import file: ")
                        For iArg = args.GetLowerBound(0) To args.GetUpperBound(0)
                            message.Append(Environment.NewLine)
                            message.Append(args(iArg))
                        Next
                    Else
                        message.Append("Exception during processing of PLUM Import files.")
                    End If
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the fromirma\Error directory on the server running the PLUM Import job.  This is typically the region's IRMA application server.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_DeleteFailed
                    message.Append("Exception during processing of PLUM Import file.  File was not deleted: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("This file may be picked up and processed by PLUM Host again the next time the PLUM Host Import/Export jobs run.")
                Case ErrorType.PLUMExport_Timeout
                    message.Append("Shell of PlumHost.exe for the export and merge timed out.  ")
                    message.Append(Environment.NewLine)
                    message.Append("The PLUM Host Merge & Export process will continue to run until completed, but the IRMA Scheduled job that calls this process has ended.  ")
                    message.Append("This means that the IRMA process will not FTP the export file from PLUM Host to the PLUM Stores during this execution of the scheduled job.")
                Case ErrorType.PLUMExport_MissingCompleteFile
                    message.Append("PLUM Merge Complete file not found during execution of the PLUM Host Merge & Export process.")
                    message.Append(Environment.NewLine)
                    message.Append("This means that the PLUM Host updates will not be sent down to PLUM Store for processing.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMExport_Failed
                    message.Append("An exception occurred the during processing of PLUM Export files.  The job did not successfully complete.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_AutoPlumLog
                    message.Append("The Auto PLUM Import process completed successfully, but the AutoPlum.err log contains warnings for File: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("This log should be examined.")
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMExport_StoreFTPError
                    ' If the application is running as a scheduled job on the app server, the importDir contains localhost in the path.
                    ' Format the string so the email message replaces localhost with the server name
                    message.Append("PLUM Host was unable to successfully deliver a file to PLUM Store for ")
                    message.Append(Environment.NewLine)
                    message.Append("Store No: ")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("IP Address: ")
                    message.Append(args(2))
                    message.Append(Environment.NewLine)
                    message.Append("Filename: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("The PLUM Host Import/Export Job will try to reprocess the error file the next time it runs.")
                    message.Append(Environment.NewLine)
                Case ErrorType.PLUMExport_DeleteErrorFileFailed
                    message.Append("Exception during the reprocessing of a PLUM Host Export file.  ")
                    message.Append("The file was successfully FTP'ed to PLUM Store, but the file was not deleted from the PLUM Host error directory: ")
                    message.Append(ReplaceLocalhost(args(0)))
                    message.Append(Environment.NewLine)
                    message.Append("This file may be picked up and processed by the PLUM Host error handling job again the next time the PLUM Store Extract job runs, ")
                    message.Append("resulting in outdated scale data being sent to the store.  ")
                    message.Append(Environment.NewLine)
                    message.Append("The file should be manually deleted.")
                Case ErrorType.POSPush_RunRemoteProcess
                    message.Append("POS Push error running remote process: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("STORE: ")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("ERROR DESCRIPTION: ")
                    message.Append(args(2))
                Case ErrorType.ScalePush_StoreNotFound
                    message.Append("Store: ")

                    If args IsNot Nothing Then
                        message.Append(args(0))
                    End If

                    message.Append(" is NOT configured in StoreScaleConfig table.  The Scale Push process will not successfully execute for this store.")
                Case ErrorType.ScalePush_ApplyAuthChangesInIRMA
                    'passing in change type string and error text
                    message.Append("Scale Push Error when processing the Change Type: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    message.Append("Data was FTP'd to the Store Scale systems, but IRMA data in the StoreItem table was not updated to reset the ScaleAuth flag.")
                    message.Append(Environment.NewLine)
                    message.Append(args(1))
                Case ErrorType.ScalePush_ApplyDeAuthChangesInIRMA
                    'passing in change type string and error text
                    message.Append("Scale Push Error when processing the Change Type: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    message.Append("Data was FTP'd to the Store Scale systems, but IRMA data in the StoreItem table was not updated to reset the ScaleDeAuth flag.")
                    message.Append(Environment.NewLine)
                    message.Append(args(1))
                Case ErrorType.PeopleSoftUpload_FileError
                    message.Append("People Soft upload failed when trying to generate the output file from IRMA.")
                    message.Append(Environment.NewLine)
                    message.Append("File name: ")
                    message.Append(args(0))
                Case ErrorType.ScheduledJob_ProcessingError
                    message.Append("A scheduled job failed to execute successfully.  The job status has been set to FAILED in the database.  ")
                    message.Append("The error log table should be examined, and the job status must be reset before it will be able to start again.")
                    message.Append(Environment.NewLine)
                    message.Append("Classname (job name): ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append(Environment.NewLine)
                    message.Append("** The error log and reset of the job status table can be managed using the Admin Client.  Select the Scheduled Jobs > Manage Scheduled Jobs menu option. **")
                Case ErrorType.ScheduledJob_DidNotRun
                    message.Append("The job ")
                    message.Append(args(0))
                    message.Append(" did NOT run at ")
                    message.Append(Now.ToShortTimeString)
                    message.Append(" with a JobStatus.Status value = ")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    If args(1).ToUpper.Equals("FAILED") Then
                        message.Append(Environment.NewLine)
                        message.Append("For FAILED jobs, the error log table should be examined, and the job status must be reset before it will be able to start again.")
                        message.Append(Environment.NewLine)
                        message.Append(Environment.NewLine)
                        message.Append("** The error log and reset of the job status table can be managed using the Admin Client.  Select the Scheduled Jobs > Manage Scheduled Jobs menu option. **")
                    End If
                Case ErrorType.HouseTlogParser_CouldNotParseTlog
                    message.Append("An error occurred while processing Tlogs.  ")
                    message.Append(Environment.NewLine)
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
            End Select

            Return message.ToString
        End Function

        ''' <summary>
        ''' The error messages for the scheduled jobs often reference a filename.  If this job is running on the application
        ''' server, the file path will include localhost.  Replace localhost with the name of the network share.
        ''' </summary>
        ''' <param name="inString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function ReplaceLocalhost(ByVal inString As String) As String
            Dim outString As String
            outString = inString.Replace("localhost", ConfigurationServices.AppSettings("PLUMJobServer"))
            outString = outString.Replace("127.0.0.1", ConfigurationServices.AppSettings("PLUMJobServer"))
            Return outString
        End Function

#Region "Format e-mail"

        Private Enum TextFormatType
            Plain
            RTF
            HTML
        End Enum

        Private Shared ReadOnly Property ApplicationInfo() As String
            Get
                With My.Application.Info
                    Return (String.Format("{0} v{1}.{2}.{3} {4}", .ProductName, .Version.Major, .Version.Minor, .Version.Build, ConfigurationServices.AppSettings("environment")))
                End With
            End Get
        End Property

        ''' <summary>
        ''' Replaces characters in formatted text that have special meaning to the format parser
        ''' </summary>
        ''' <param name="formattedText">Formatted text which may contain characters that have special meaning</param>
        ''' <param name="textFormat">HTML, Rich Text (RTF), Plain Text, etc.</param>
        ''' <returns>Formatted text in which these characters have been escaped with the syntax equivalent</returns>
        ''' <remarks></remarks>
        Private Shared Function ReplaceSpecialCharacters(ByVal formattedText As String, ByVal textFormat As TextFormatType) As String

            Dim scrubbedText As New System.Text.StringBuilder(formattedText)

            Select Case textFormat
                Case TextFormatType.HTML
                    '	less than		    '<'    =>	'&lt;'
                    '	greater than  	'>'    =>	'&gt;'
                    '	ampersand		'&'   =>	'&amp;'
                    '	double quote	'"'    =>	    '&quot;'
                    scrubbedText.Replace("<", "&lt;")
                    scrubbedText.Replace(">", "&gt;")
                    scrubbedText.Replace("&", "&amp;")
                    scrubbedText.Replace("""", "&quot;")

                Case TextFormatType.RTF
                    '	backslash		    '\'    =>	    '\\'
                    '	left brace  	        '{'    =>	    '\{'
                    '	right brace  	    '}'    =>	    '\}'
                    scrubbedText.Replace("\", "\\")
                    scrubbedText.Replace("{", "\{")
                    scrubbedText.Replace("}", "\}")

            End Select

            Return scrubbedText.ToString

        End Function

        Private Shared Function GetCustomTemplate(ByVal templateConfigKey As String) As String

            Dim templateFile As String = String.Empty
            Dim templateText As String = String.Empty

            Try
                templateFile = ConfigurationServices.AppSettings(templateConfigKey)

                If templateFile Is Nothing OrElse templateFile.Length = 0 Then
                    Exit Try
                End If

                'add the file path to the template file name
                templateFile = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, templateFile)

                'assume the file exists since error will be caught below and the template will be built
                templateText = My.Computer.FileSystem.ReadAllText(templateFile).Trim

            Catch ex As Exception
                'swallow error and continue without using template
                templateText = String.Empty

            End Try

            Return templateText

        End Function

        ''' <summary>
        ''' Replaces the following template variables:
        ''' [APPLICATION], [USER_NAME], [MACHINE_NAME], [DATE_TIME], 
        ''' [ERROR_1_MSG], [ERROR_1_SOURCE], [ERROR_1_TYPE], [ERROR_1_STACKTRACE],
        ''' [ERROR_2_MSG], [ERROR_2_SOURCE], [ERROR_2_TYPE], [ERROR_2_STACKTRACE] 
        ''' </summary>
        ''' <param name="templateText">Generic text that contains variables to be replaced</param>
        ''' <param name="exception">Error details to use when replacing the variables</param>
        ''' <param name="textFormat">HTML, Rich Text (RTF), Plain Text, etc.</param>
        ''' <returns>Text that matches the template, but with specific values in place of the variables</returns>
        ''' <remarks></remarks>
        Private Shared Function ReplaceTemplateVariables(ByVal templateText As String, ByVal exception As System.Exception, ByVal textFormat As TextFormatType) As String

            Dim outputText As New System.Text.StringBuilder(templateText)
            Dim customData As String = String.Empty
            Dim userName As String = Environment.UserName

            'For Each de As DictionaryEntry In exception.Data
            '    customData += String.Format("{0}    The key is '{1}' and the value is: {2}", Environment.NewLine, de.Key, de.Value)
            'Next de

            With outputText
                .Replace("[APPLICATION]", ApplicationInfo)
                .Replace("[USER_NAME]", userName)
                .Replace("[MACHINE_NAME]", Environment.MachineName)
                .Replace("[DATE_TIME]", Now.ToString("yyyy-MM-dd HH:mm:ss"))
            End With

            With exception
                outputText.Replace("[ERROR_1_MSG]", ReplaceSpecialCharacters(.Message, textFormat))
                outputText.Replace("[ERROR_1_SOURCE]", ReplaceSpecialCharacters(.Source, textFormat))
                outputText.Replace("[ERROR_1_TYPE]", ReplaceSpecialCharacters(.GetType.Name, textFormat))
                outputText.Replace("[ERROR_1_STACKTRACE]", ReplaceSpecialCharacters(.StackTrace, textFormat))

                If .InnerException IsNot Nothing Then
                    With .InnerException
                        outputText.Replace("[ERROR_2_MSG]", ReplaceSpecialCharacters(.Message, textFormat))
                        outputText.Replace("[ERROR_2_SOURCE]", ReplaceSpecialCharacters(.Source, textFormat))
                        outputText.Replace("[ERROR_2_TYPE]", ReplaceSpecialCharacters(.GetType.Name, textFormat))
                        outputText.Replace("[ERROR_2_STACKTRACE]", ReplaceSpecialCharacters(.StackTrace, textFormat))
                    End With
                End If
            End With

            Return outputText.ToString

        End Function

        Public Shared Function GenerateHTML(ByVal exception As System.Exception) As String

            Return GetTextFromTemplate(exception, TextFormatType.HTML)

        End Function

        Public Shared Function GenerateRTF(ByVal exception As System.Exception) As String

            Return GetTextFromTemplate(exception, TextFormatType.RTF)

        End Function

        Public Shared Function GeneratePlainText(ByVal exception As System.Exception) As String

            Return GetTextFromTemplate(exception, TextFormatType.Plain)

        End Function

        Private Shared Function GetTextFromTemplate(ByVal exception As System.Exception, ByVal textFormat As TextFormatType) As String

            Dim templateText As String = String.Empty
            Dim errorText As String = String.Empty
            Dim configKey As String = String.Empty
            Dim hasInnerException As Boolean = False

            ' 1.16.08 Robert S. - incorporating Tim's code, but only sending HTML and not reliant on a config key instead.

            Try
                ' check config key named 'HTMLTemplate_InnerException', 'RTFTemplate_ExceptionOnly', etc. (if it exists; otherwise use default hard-coded template)
                If exception.InnerException IsNot Nothing Then
                    hasInnerException = True
                    'configKey = String.Format("{0}Template_InnerException", textFormat.ToString)
                    'Else
                    'configKey = String.Format("{0}Template_ExceptionOnly", textFormat.ToString)
                End If

                'select the appropriate template: Exception only OR Exception and InnerException
                'templateText = GetCustomTemplate(configKey)

                'If templateText.Length = 0 Then
                'use the hard-coded default template
                templateText = GetDefaultTemplate(textFormat, hasInnerException)
                'End If

                'replace template variables: 
                errorText = ReplaceTemplateVariables(templateText, exception, textFormat)

            Catch ex As Exception
                'unable to send HTML e-mail from template; send plain text e-mail instead
                templateText = GetDefaultTemplate(TextFormatType.Plain, hasInnerException)
                errorText = ReplaceTemplateVariables(templateText, exception, TextFormatType.Plain)

            End Try

            Return errorText

        End Function

        Private Shared Function GetDefaultTemplate(ByVal textFormat As TextFormatType, ByVal hasInnerException As Boolean) As String

            Dim templateText As New System.Text.StringBuilder()

            'use the hard-coded default template for the specified text format
            Select Case textFormat
                Case TextFormatType.HTML         '--------------------------------------------------------------------------------------------------
                    With templateText
                        .AppendLine("<HTML>")
                        .AppendLine("<head>")
                        .AppendLine("<style type=""text/css"">")
                        .AppendLine("body {font-family: courier new,sans-serif;}")
                        .AppendLine("strong {color: red;}")
                        .AppendLine("</style>")
                        '.AppendLine("<title>[EMAIL_SUBJECT]</title>")
                        .AppendLine("</head>")
                        .AppendLine("<body>")
                        .AppendLine("<pre>Application: <b>[APPLICATION]</b>")
                        .AppendLine("User:        <b>[USER_NAME]</b>")
                        .AppendLine("Machine:     <b>[MACHINE_NAME]</b>")
                        .AppendLine("Date:        <b>[DATE_TIME]</b></pre>")
                        .AppendLine("<hr>")
                        .AppendLine("<pre><b>Exception:</b></pre>")
                        .AppendLine("<strong><code>[ERROR_1_MSG]</code></strong>")
                        .AppendLine("<br>")
                        .AppendLine("<br>")
                        If hasInnerException Then
                            .AppendLine("<pre><b>InnerException:</b></pre>")
                            .AppendLine("<strong><code>[ERROR_2_MSG]</code></strong>")
                            .AppendLine("<br>")
                            .AppendLine("<br>")
                        End If
                        .AppendLine("<hr>")
                        .AppendLine("<pre><b>Exception Detail:</b></pre>")
                        .AppendLine("<pre>Source: <b>[ERROR_1_SOURCE]</b>")
                        .AppendLine("Type:   <b>[ERROR_1_TYPE]</b>")
                        .AppendLine("StackTrace:")
                        .AppendLine("[ERROR_1_STACKTRACE]")
                        .AppendLine("</pre>")
                        If hasInnerException Then
                            .AppendLine("<hr>")
                            .AppendLine("<pre><b>InnerException Detail:</b></pre>")
                            .AppendLine("<pre>Source: <b>[ERROR_2_SOURCE]</b>")
                            .AppendLine("Type:   <b>[ERROR_2_TYPE]</b>")
                            .AppendLine("StackTrace:")
                            .AppendLine("[ERROR_2_STACKTRACE]")
                            .AppendLine("</pre>")
                        End If
                        .AppendLine("</body>")
                        .AppendLine("</html>")
                    End With

                Case TextFormatType.RTF   '--------------------------------------------------------------------------------------------------
                    With templateText
                        .AppendLine("{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fmodern\fprq1\fcharset0 Lucida Console;}}")
                        .AppendLine("{\colortbl ;\red255\green0\blue0;}")
                        .AppendLine("\viewkind4\uc1\pard\f0\fs16")
                        .AppendLine("Application: \b [APPLICATION]\b0\par")
                        .AppendLine("User:        \b [USER_NAME]\b0\par")
                        .AppendLine("Machine:     \b [MACHINE_NAME]\b0\par")
                        .AppendLine("Date:        \b [DATE_TIME]\b0\par")
                        .AppendLine("________________________________________\par")
                        .AppendLine("\par")
                        .AppendLine("\b Exception:\b0\par")
                        .AppendLine("\par")
                        .AppendLine("\cf1[ERROR_1_MSG]\cf0\par")
                        If hasInnerException Then
                            .AppendLine("\par")
                            .AppendLine("\b Inner Exception:\b0\par")
                            .AppendLine("\par")
                            .AppendLine("\cf1[ERROR_2_MSG]\cf0\par")
                        End If
                        .AppendLine("________________________________________\par")
                        .AppendLine("\par")
                        .AppendLine("\b Exception Detail:\b0\par")
                        .AppendLine("Source: \b [ERROR_1_SOURCE]\b0\par")
                        .AppendLine("Type:   \b [ERROR_1_TYPE]\b0\par")
                        .AppendLine("StackTrace:\par")
                        .AppendLine("[ERROR_1_STACKTRACE]\par")
                        If hasInnerException Then
                            .AppendLine("________________________________________\par")
                            .AppendLine("\par")
                            .AppendLine("\b Inner Exception Detail:\b0\par")
                            .AppendLine("Source: \b [ERROR_2_SOURCE]\b0\par")
                            .AppendLine("Type:   \b [ERROR_2_TYPE]\b0\par")
                            .AppendLine("StackTrace:\par")
                            .AppendLine("[ERROR_2_STACKTRACE]\par")
                        End If
                        .AppendLine("\par")
                        .AppendLine("}")
                    End With

                Case TextFormatType.Plain         '--------------------------------------------------------------------------------------------------
                    With templateText
                        .AppendLine("Application: [APPLICATION]")
                        .AppendLine("User:        [USER_NAME]")
                        .AppendLine("Machine:     [MACHINE_NAME]")
                        .AppendLine("Date:        [DATE_TIME]")
                        .Append("_"c, 40)
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("Exception:")
                        .AppendLine()
                        .AppendLine("[ERROR_1_MSG]")
                        If hasInnerException Then
                            .AppendLine()
                            .AppendLine("InnerException:")
                            .AppendLine()
                            .AppendLine("[ERROR_2_MSG]")
                        End If
                        .Append("_"c, 40)
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("Exception Detail:")
                        .AppendLine("Source: [ERROR_1_SOURCE]")
                        .AppendLine("Type:   [ERROR_1_TYPE]")
                        .AppendLine("StackTrace:")
                        .AppendLine("[ERROR_1_STACKTRACE]")
                        If hasInnerException Then
                            .Append("_"c, 40)
                            .AppendLine()
                            .AppendLine()
                            .AppendLine("InnerException Detail:")
                            .AppendLine("Source: [ERROR_2_SOURCE]")
                            .AppendLine("Type:   [ERROR_2_TYPE]")
                            .AppendLine("StackTrace:")
                            .AppendLine("[ERROR_2_STACKTRACE]")
                        End If
                    End With

            End Select

            Return templateText.ToString

        End Function

#End Region

    End Class

End Namespace
