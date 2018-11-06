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


    Public Enum ErrorType
        GeneralApplicationError
        DataFactoryException
        FTPException
        POSPush_StoreConfig
        POSPush_StoreNotFound
        POSPush_ApplyChangesInIRMA
        POSPush_AdminError
        PLUMImport_Timeout
        PLUMImport_MissingCompleteFile
        PLUMImport_Failed
        PLUMImport_DeleteFailed
        PLUMImport_AutoPlumLog
        PLUMExport_Timeout
        PLUMExport_MissingCompleteFile
        PLUMExport_Failed
        PLUMExport_StoreFTPError
    End Enum

    Public Class ErrorHandler
        Private Const DEFAULT_EMAIL_FROM As String = "irma.application@wholefoods.com"
        Private Const DEFAULT_EMAIL_TO As String = "amanda.black@wholefoods.com;sam.gordon@wholefoods.com"
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
                    messageText.Append(exception.StackTrace)
                End If

                ' create an event log for error handling
                appEventLog.Source = "IRMA"
                appEventLog.WriteEntry(messageText.ToString, EventLogEntryType.Error)

                'send email
                'build the message
                Dim msgFromStr As String = CStr(HttpContext.Current.Application.Get("errorNotificationFrom"))
                If msgFromStr Is Nothing Or msgFromStr = "" Then
                    msgFromStr = DEFAULT_EMAIL_FROM
                End If
                message.From = New MailAddress(msgFromStr)

                Dim msgTo As String
                Dim msgToStr As String = CStr(HttpContext.Current.Application.Get("primaryErrorNotification"))
                If msgToStr Is Nothing Or msgToStr = "" Then
                    msgToStr = DEFAULT_EMAIL_TO
                End If
                Dim msgToList As String() = msgToStr.Split(";"c)
                For Each msgTo In msgToList
                    message.To.Add(New MailAddress(msgTo))
                Next

                Dim msgCopy As String
                Dim msgCopyStr As String = CStr(HttpContext.Current.Application.Get("secondaryErrorNotification"))
                If Not msgToStr Is Nothing AndAlso msgToStr <> "" Then
                    Dim msgCopyList As String() = msgCopyStr.Split(";"c)
                    For Each msgCopy In msgCopyList
                        message.CC.Add(New MailAddress(msgCopy))
                    Next
                End If

                Dim msgSubStr As String = CStr(HttpContext.Current.Application.Get("errorSubject"))
                If msgSubStr Is Nothing Or msgSubStr = "" Then
                    msgSubStr = DEFAULT_EMAIL_SUBJECT
                End If
                message.Subject = msgSubStr

                message.Body = messageText.ToString()

                'deliver the message
                mailClient.Send(message)

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

            Select Case errorNo
                Case ErrorType.GeneralApplicationError
                    message.Append("One of the IRMA Applications encountered an unexpected error during processing.")
                    message.Append(Environment.NewLine)
                    message.Append("The execption stack trace is included in the email message to provide details about the error.")
                Case ErrorType.DataFactoryException
                    message.Append("One of the IRMA Applications encountered an unexpected error during processing when accessing the database.")
                    message.Append(Environment.NewLine)
                    message.Append("The execption stack trace is included in the email message to provide details about the error.")
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
                        message.Append(args(0))
                    End If
                Case ErrorType.POSPush_StoreNotFound
                    message.Append("Store: ")

                    If args IsNot Nothing Then
                        message.Append(args(0))
                    End If

                    message.Append(" is NOT configured in StorePOSConfig table.  The POS Push process will not successfully execute for this store.")
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
                    message.Append("The POS Push Admin UI encounterd an error when processing a request.")
                    message.Append(Environment.NewLine)
                    message.Append(args(0))
                Case ErrorType.PLUMImport_Timeout
                    message.Append("Shell of PlumHost.exe timed out when processing PLUM Import file: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the error directory on the server running the PLUM Import job: ")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
                    message.Append("The PLUM Import process will continue to run until completed, but the IRMA Scheduled job that calls this process has ended.  ")
                    message.Append("This means that the export from PLUM Host to PLUM Store has not been started for this file.  It also means that if there are other ")
                    message.Append("import batches pending, they will not be imported into PLUM Host during this execution of the scheduled job.")
                Case ErrorType.PLUMImport_MissingCompleteFile
                    message.Append("PLUM Import Complete file not found for file: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the fromirma\Error directory on the server running the PLUM Import job.  This is typically the region's IRMA application server.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_Failed
                    If (args(0) IsNot Nothing) Then
                        message.Append("Exception during processing of PLUM Import file: ")
                        message.Append(args(0))
                    Else
                        message.Append("Execption during processing of PLUM Import files.")
                    End If
                    message.Append(Environment.NewLine)
                    message.Append("The file has been moved to the fromirma\Error directory on the server running the PLUM Import job.  This is typically the region's IRMA application server.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_DeleteFailed
                    message.Append("Exception during processing of PLUM Import file.  File was not deleted: ")
                    message.Append(args(0))
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
                    message.Append("An execption occurred the during processing of PLUM Export files.  The job did not successfully complete.")
                    message.Append(Environment.NewLine)
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMImport_AutoPlumLog
                    message.Append("The Auto PLUM Import process completed successfully, but the AutoPlum.err log contains warnings for File: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("This log should be examined.")
                    message.Append("The Reports > Event Log from the PLUM Host client application can be used to review and diagnose issues.")
                Case ErrorType.PLUMExport_StoreFTPError
                    message.Append("PLUM Host was unable to successfully deliver a file to PLUM Store for ")
                    message.Append(Environment.NewLine)
                    message.Append("Filename: ")
                    message.Append(args(0))
                    message.Append(Environment.NewLine)
                    message.Append("Store No: ")
                    message.Append(args(1))
                    message.Append(Environment.NewLine)
            End Select

            Return message.ToString
        End Function

    End Class

End Namespace
