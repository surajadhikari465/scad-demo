Imports System.ComponentModel
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports IRMA_WebServices.Response
Imports System.IO
Imports System.Diagnostics
Imports ICSharpCode.SharpZipLib
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Net.Mime
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

<WebService(Name:="IRMA Services", Namespace:="http://wholefoodsmarket.com/IRMA/", Description:="Available IRMA Web Service Methods")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class IRMAServices
    Inherits System.Web.Services.WebService

#Region " Private Members"

    Private _errCount As Integer = 0

    ''' <summary>
    ''' Contains the SQL configuration settings for this web service.
    ''' </summary>
    ''' <remarks>All methods must store their SQL text in this file. Both Stored Procedures and SQL Text are supported.</remarks>
    Private _sqlConfigName As String = "SQL.config"

#End Region

#Region " Public Enumerations"

    ''' <summary>
    ''' The currently supported Web Service Methods.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SupportedMethod
        COST_FILE = 0
        ITEM_FILE = 1
        PLANO_FILE = 2
        INVENTORY_SERVICE_FILE = 3
        AUTO_PUSH_LOGS = 4
        WEEKLY_ROLLUP = 5
        TLOG_ERROR_PULL = 6
        VIM_SUPPLEMENT_EXTRACT = 7
    End Enum

#End Region

#Region " Public Web Methods"

    <WebMethod(Description:="Creates VIM Supplemental Item and Cost Data Extract files and FTPs them to the location specified in the FTPBO object.")> _
  Public Function GetVIMExtracts(ByVal FTPParams As FTPBO) As ServiceResponse

        Dim response As ServiceResponse
        Dim _error As Integer = 0

        Try

            response = GetVIMCostExtract(FTPParams)

        Catch ex As Exception

            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            _error = _error + 1

        End Try

        Try

            response = GetVIMItemExtract(FTPParams)

        Catch ex As Exception

            WriteToEventLog(ex.Message, EventLogEntryType.Error)

            _error = _error + 1

        End Try

        If _error = 0 Then

            Return ServiceResponse.Success

        Else

            Return ServiceResponse.UnknownError

        End If

    End Function

    ''' <summary>
    ''' Creates a WIMP Data Extract file for the specified WIMP method type and FTPs it to the location specified in the FTPBO object.
    ''' </summary>
    ''' <param name="FTPParams">FTOBO Object</param>
    ''' <param name="WIMPType">Supported WIMPMethod</param>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks></remarks>
    <WebMethod(Description:="Generates a WIMP Data Extract File and sends the file via FTP to the location specified.")> _
  Public Function GetWIMPExtract(ByVal FTPParams As FTPBO, ByVal WIMPType As SupportedMethod) As ServiceResponse

        Dim tempFile As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + Path.GetRandomFileName
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim FTPSuccess As Boolean = False
        Dim FTPUtil As FTP.FTPclient
        Dim dReader As SqlDataReader = Nothing
        Dim response As ServiceResponse = Nothing
        Dim lineStr As String = String.Empty
        Dim _type As String = String.Empty
        Dim _fName As String = String.Empty

        ' determine the SQL to use and the file name of the output file
        Select Case WIMPType
            Case SupportedMethod.COST_FILE
                _type = GetCommonAppSetting("WIMP_COST", Me._sqlConfigName)
                _fName = ConfigurationManager.AppSettings.Get("COSTFilePrefix")
            Case SupportedMethod.ITEM_FILE
                _type = GetCommonAppSetting("WIMP_ITEM", Me._sqlConfigName)
                _fName = ConfigurationManager.AppSettings.Get("ITEMFilePrefix")
            Case SupportedMethod.PLANO_FILE
                _type = GetCommonAppSetting("WIMP_PLANO", Me._sqlConfigName)
                _fName = ConfigurationManager.AppSettings.Get("PLANOFilePrefix")
        End Select

        _fName = _fName + ConfigurationManager.AppSettings.Get("FileExt")

        ' first, get the data
        Try
            factory.CommandTimeout = 3600
            dReader = factory.GetDataReader(_type)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.ADOError
            GoTo EndOfMethod
        End Try

        ' now create the file using the specified delimeter
        Try
            If File.Exists(tempFile) Then
                File.Delete(tempFile)
            End If

            Using sw As StreamWriter = New StreamWriter(tempFile, True)

                Dim fCount As Integer = dReader.FieldCount - 1
                Dim i As Integer

                If FTPParams.FTP_DELIM = String.Empty Then FTPParams.FTP_DELIM = ConfigurationManager.AppSettings.Get("ExportDelimiter")

                Do While dReader.Read
                    For i = 0 To fCount
                        ' always convert True & False to numeric representation
                        Select Case dReader.Item(i).ToString
                            Case "True"
                                lineStr &= "Y"
                            Case "False"
                                lineStr &= "N"
                            Case Else
                                lineStr &= dReader.Item(i).ToString
                        End Select
                        If i < fCount Then
                            lineStr &= FTPParams.FTP_DELIM
                        End If
                    Next
                    sw.WriteLine(lineStr)
                    lineStr = String.Empty
                Loop

                sw.Flush()
                sw.Close()
                sw.Dispose()

            End Using

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.FileBuilderError
            GoTo EndOfMethod
        End Try

        ' now FTP the file
        Try
            FTPUtil = New FTP.FTPclient(FTPParams.FTP_IP, FTPParams.FTP_UID, FTPParams.FTP_PWD, FTPParams.FTP_SECURE, FTPParams.FTP_PORT)

            If Not FTPParams.FTP_DIR = String.Empty Then
                If Not Mid(FTPParams.FTP_DIR, 1, 1) = "/" Then
                    FTPParams.FTP_DIR &= "/" & FTPParams.FTP_DIR
                End If
                _fName = FTPParams.FTP_DIR + "/" + _fName
            End If

            FTPUtil.Upload(tempFile, _fName, False)
            FTPSuccess = True

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            FTPSuccess = False
        Finally
            FTPUtil = Nothing
        End Try

        If Not FTPSuccess Then
            response = ServiceResponse.FTPError
        Else
            response = ServiceResponse.Success
        End If

EndOfMethod:

        ' now delete the temp file
        If File.Exists(tempFile) Then
            File.Delete(tempFile)
        End If

        If Not dReader Is Nothing Then
            dReader.Close()
        End If

        Return response

    End Function

    ''' <summary>
    ''' Generates a full inventory service file extract and sends the file via FTP to the location specified in the FTPParams Object.
    ''' </summary>
    ''' <param name="FTPParams">FTPBO Object</param>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks>Customize the SQL for this method in SQL.config to output your inventory files in the preffered format.</remarks>
    <WebMethod(Description:="Generates a full inventory service file extract and sends the file via FTP to the location specified.")> _
    Public Function GetInventoryFileExtract(ByVal FTPParams As FTPBO) As ServiceResponse

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim fName As String
        Dim tempFile As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + Path.GetRandomFileName
        Dim FTPSuccess As Boolean = False
        Dim FTPUtil As FTP.FTPclient
        Dim dReader As SqlDataReader = Nothing
        Dim response As ServiceResponse = Nothing
        Dim cmd As SqlCommand
        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim _date As String = String.Empty
        Dim _day As String = Today.Day.ToString
        Dim _month As String = Today.Month.ToString
        Dim _year As String = Today.Year.ToString

        Dim lineStr As String = String.Empty

        Try        ' first, get the stores
            factory.CommandTimeout = 3600
            dt = New DataTable
            dt = factory.GetStoredProcedureDataTable(GetCommonAppSetting("VIM_STORESET", Me._sqlConfigName))
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.ADOError
            GoTo EndOfMethod
        End Try

        For Each dr In dt.Rows

            Try  ' now get the inventory data for each store
                factory.CommandTimeout = 3600
                cmd = New SqlCommand(GetCustomAppSetting("SQL_INV", Me._sqlConfigName))
                dReader = factory.GetDataReader(cmd.CommandText.Replace("@StoreID", dr.Item("REG_STORE_NUM")))
            Catch ex As Exception
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                ' return the error response to the caller
                response = ServiceResponse.ADOError
                GoTo EndOfMethod
            End Try


            ' now create the store file
            Try
                If File.Exists(tempFile) Then
                    File.Delete(tempFile)
                End If

                Using sw As StreamWriter = New StreamWriter(tempFile, True)

                    Dim fCount As Integer = dReader.FieldCount - 1
                    Dim i As Integer

                    Do While dReader.Read
                        For i = 0 To fCount
                            lineStr &= dReader.Item(i).ToString
                            If i < fCount Then
                                lineStr &= vbTab
                            End If
                        Next
                        sw.WriteLine(lineStr)
                        lineStr = String.Empty
                    Loop

                    sw.Flush()
                    sw.Close()
                    sw.Dispose()

                End Using

                If Not dReader Is Nothing Then
                    dReader.Close()
                End If

            Catch ex As Exception
                WriteToEventLog(ex.Message, EventLogEntryType.Error)

                If Not dReader Is Nothing Then
                    dReader.Close()
                End If

                ' return the error response to the caller
                response = ServiceResponse.FileBuilderError
                GoTo EndOfMethod
            End Try

            Try            ' now FTP the file

                ' needs a zero in front of any single-digits
                If Len(_day) = 1 Then _day = "0" + _day
                If Len(_month) = 1 Then _month = "0" + _month

                _year = Mid(Today.Year.ToString, 3, 2)

                _date = (_month + _day + _year).ToString

                fName = ConfigurationManager.AppSettings.Get("InventoryFilePrefix")

                FTPUtil = New FTP.FTPclient(FTPParams.FTP_IP, FTPParams.FTP_UID, FTPParams.FTP_PWD, FTPParams.FTP_SECURE, FTPParams.FTP_PORT)

                If Not FTPParams.FTP_DIR = String.Empty Then
                    If Not Mid(FTPParams.FTP_DIR, 1, 1) = "/" Then
                        FTPParams.FTP_DIR &= "/" & FTPParams.FTP_DIR
                    End If
                    fName = FTPParams.FTP_DIR.ToString + "/" + fName.ToString + dr.Item("PS_BU").ToString + "_" + _date + ConfigurationManager.AppSettings.Get("InvFileExt").ToString
                End If

                FTPUtil.Upload(tempFile, fName, False)
                FTPSuccess = True

            Catch ex As Exception
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                FTPSuccess = False
            Finally
                FTPUtil = Nothing
            End Try

            If Not FTPSuccess Then
                response = ServiceResponse.FTPError
            Else
                response = ServiceResponse.Success
            End If

            '' now delete the temp file
            If File.Exists(tempFile) Then
                File.Delete(tempFile)
            End If

        Next

EndOfMethod:

        Return response

    End Function

    ''' <summary>
    ''' This mehod will go to your IRMA app server and grab/zip the automated POS Push log files and the archived batch files
    ''' and email them to the specified recipient. Two file shares on the IRMA application server are required for this method. 
    ''' See Settings.config for an example.</summary>
    ''' <param name="Recipient">Mail.MailAddress object</param>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks>Requires a reference to ICSharpCode.SharpZipLib (http://www.icsharpcode.net/OpenSource/SharpZipLib/)</remarks>
    <WebMethod(Description:="Gets the Automated POS Push logs and emails them to the specified recipient.")> _
    Public Function GetPOSPushLogs(ByVal Recipient As String) As ServiceResponse

        Dim _pushLogs As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + "pushLogs.zip"
        Dim _pushArchive As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + "batchArchive.zip"

        Dim _attachment As Attachment
        Dim _mm As MailMessage

        Try

            ' email cannot be empty string and must be a valid address
            If Recipient = String.Empty Or Not ValidateEmail(Recipient) Then
                Throw New Exception("A valid email address is required to call this method.")
            End If

            ' clear the temp directory first then create the zip files

            If File.Exists(_pushLogs) Then
                File.Delete(_pushLogs)
            End If

            Try
                CreateZip(_pushLogs, ConfigurationManager.AppSettings.Get("POSPushLogPath"), True, ConfigurationManager.AppSettings.Get("POSPushLogFilter"))
            Catch ex As Exception
                Me._errCount = Me._errCount + 1
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                CreateMessage(Recipient, "An error ocurred creating the zip file: " + _pushLogs + Chr(10) + ex.Message, MailPriority.High)
            End Try

            If File.Exists(_pushArchive) Then
                File.Delete(_pushArchive)
            End If

            Try
                CreateZip(_pushArchive, ConfigurationManager.AppSettings.Get("POSPushArchivePath"), True, ConfigurationManager.AppSettings.Get("POSPushArchiveFilter"))
            Catch ex As Exception
                Me._errCount = Me._errCount + 1
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                CreateMessage(Recipient, "An error ocurred creating the zip file: " + _pushArchive + Chr(10) + ex.Message, MailPriority.High)
            End Try

            ' now attach the zip files to a mail message
            _mm = New MailMessage

            ' POSPush Logs
            Try
                _attachment = New Attachment(_pushLogs, MediaTypeNames.Application.Zip)
                _mm.Attachments.Add(_attachment)
            Catch ex As Exception
                Me._errCount = Me._errCount + 1
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                CreateMessage(Recipient, "An error ocurred attaching the zip file: " + _pushArchive + Chr(10) + ex.Message, MailPriority.High)
            End Try

            ' POSPush Archive Directory
            Try
                _attachment = New Attachment(_pushArchive, MediaTypeNames.Application.Zip)
                _mm.Attachments.Add(_attachment)
            Catch ex As Exception
                Me._errCount = Me._errCount + 1
                WriteToEventLog(ex.Message, EventLogEntryType.Error)
                CreateMessage(Recipient, "An error ocurred attaching the zip file: " + _pushArchive + Chr(10) + ex.Message, MailPriority.High)
            End Try

            ' send the email if there are no errors
            If Me._errCount > 0 Then
                Throw New Exception("Errors ocurred attempting to run the GetPOSPushLogs.")
            Else
                Try
                    CreateMessage(Recipient, _mm.Attachments)
                    Return ServiceResponse.Success
                Catch ex As Exception
                    Me._errCount = Me._errCount + 1
                    WriteToEventLog(ex.Message, EventLogEntryType.Error)
                    CreateMessage(Recipient, "An error ocurred sending the POS Push Logs message: " + ex.Message, MailPriority.High)
                End Try
                Return ServiceResponse.Success
            End If

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            Return ServiceResponse.UnknownError
        End Try

    End Function

    ''' <summary>
    ''' This method allows the Weekly Sales Rollup process built into the IRMA Admin client to be a scheduled process
    ''' rather than run manually.
    ''' </summary>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks>Setup your scheduled task to run only once per week. Set the TLOGRunAllowedOn configuration setting
    ''' with the correct DayOfWeek value that corresponds to the day you want to run it (ex. Monday = 1). The method will then determine
    ''' the appropriate StartDate and EndDate values to use in the stored procedure.</remarks>
    <WebMethod(Description:="Updates the Sales_SumByItemWkly rollup table in IRMA from the Sales_SumByItem table.")> _
    Public Function UpdateSalesSubByWeekly() As ServiceResponse
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try

            ' ONLY allow to run on day of week specified
            If Today.DayOfWeek = CInt(ConfigurationManager.AppSettings.Get("TLOGRunAllowedOn")) Then

                factory.CommandTimeout = 3600

                ' setup parameters for stored proc

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                ' always subtract 8 days from today (inclusive) to get the start date
                currentParam.Value = Date.Today.AddDays(-CInt("8"))
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                ' always subtract 1 day from today to get the end date
                ' the "EndDate" is always the table's Date_Key rollup date value for the data inserted
                currentParam.Value = Date.Today.AddDays(-CInt("1"))
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure(GetCommonAppSetting("SALES_WEEKLY_ROLLUP", Me._sqlConfigName), paramList)

                Return ServiceResponse.Success

            Else

                WriteToEventLog(My.Resources.WeeklyRollupDateMismatchError, EventLogEntryType.Warning)
                CreateMessage(ConfigurationManager.AppSettings.Get("EmailFrom"), My.Resources.WeeklyRollupDateMismatchError, MailPriority.High)

            End If

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            CreateMessage(ConfigurationManager.AppSettings.Get("EmailFrom"), Date.Now + " : The Weekly Sales Rollup failed with error: " + ex.Message, MailPriority.High)
            ' return the error response to the caller
            Return ServiceResponse.ADOError
        End Try

    End Function

    ''' <summary>
    ''' This method can be called to verify that data being submitted to IRMA does not contain any invalid characters.
    ''' </summary>
    ''' <param name="strToValidate"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>A True result indicates that an invalid character was found in the string.</remarks>
    <WebMethod(Description:="A test to validate a string is free of special characters. A True result indicates that an invalid character was found in the string.")> _
    Public Function ValidateStringText(ByVal strToValidate As String) As Boolean

        Return ValidateString(strToValidate)

    End Function

    ''' <summary>
    ''' This method can be called to pull any errors and the error message from the tlog parser logs and email them to
    ''' the specified Recipient.
    ''' </summary>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks></remarks>
    <WebMethod(Description:="This method can be called to pull any errors and the error message from the tlog parser logs. Format date string as ddmmyyyy.")> _
    Public Function GetTlogErrors(ByVal strDate As String, ByVal Recipient As String) As ServiceResponse

        Dim _tfr As IO.StreamReader = Nothing
        Dim _line As String
        Dim _fName As String = ConfigurationManager.AppSettings.Get("TLOGParserLogPath") + String.Format(ConfigurationManager.AppSettings.Get("TLOGParserLogFileName"), strDate)
        Dim _SmtpSend As Mail.SmtpClient
        Dim _mailMsg As Mail.MailMessage
        Dim _sender As MailAddress
        Dim _recipient As MailAddress
        Dim _errorsExist As Boolean = False

        Try
            ' validate the input
            If Recipient = String.Empty Or Recipient = Nothing Or Not ValidateEmail(Recipient) Then
                Return ServiceResponse.RecipientError
                Exit Function
            End If

            ' create the mail message
            _sender = New MailAddress(ConfigurationManager.AppSettings.Get("EmailFrom"))
            _recipient = New MailAddress(Recipient)

            _mailMsg = New MailMessage

            _mailMsg.Subject = ConfigurationManager.AppSettings.Get("TLOGEmailSubject")
            _mailMsg.To.Add(_recipient)
            _mailMsg.From = _sender
            _mailMsg.IsBodyHtml = True
            _mailMsg.Body = "<body>"

            ' grab the log info
            If File.Exists(_fName) Then

                _tfr = New StreamReader(File.OpenRead(_fName))

                Do
                    'loop through the log and pull out just the errors and add them to the email body
                    _line = _tfr.ReadLine()

                    If Not _line = Nothing Then

                        If _line.Contains("Could Not Parse Tlogs for: ") Then

                            _errorsExist = True ' found an error

                            _mailMsg.Body = _mailMsg.Body + "<br/>" + _line ' add the error line

                            _line = _tfr.ReadLine() ' read the next line to get the error message line

                            _mailMsg.Body = _mailMsg.Body + "<br/>" + _line + "<br/>" ' add the error message

                        End If

                    End If

                Loop Until _line Is Nothing

            Else
                ' the log file was not found
                _errorsExist = True
                _mailMsg.Body = "Log file for " + strDate + " not found<br/>"

            End If

            If _errorsExist = False Then
                ' there weren't any errors to report
                _mailMsg.Body = "No errors were found for " + strDate

            End If

            _mailMsg.Body = _mailMsg.Body + "</body>"

            ' send the emailo
            _SmtpSend = New Mail.SmtpClient
            _SmtpSend.Host = ConfigurationManager.AppSettings.Get("smtpServer").ToString
            _SmtpSend.Send(_mailMsg)

        Catch ex As Exception

            ' log the error
            WriteToEventLog(ex.Message, EventLogEntryType.Error)

            ' return the unhappy response
            Return ServiceResponse.UnknownError

        Finally

            If Not _tfr Is Nothing Then
                _tfr.Close()
            End If

        End Try

    End Function

#End Region

#Region " Private Methods"

    '<WebMethod(Description:="Creates a VIM Supplemental Cost Data Extract file and FTPs it to the location specified in the FTPBO object.")> _
    ''' <summary>
    ''' Creates a VIM Supplemental Data Extract file and FTPs it to the location specified in the FTPBO object.
    ''' </summary>
    ''' <param name="FTPParams">FTOBO Object</param>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks></remarks>
    Private Function GetVIMCostExtract(ByVal FTPParams As FTPBO) As ServiceResponse

        Dim tempFile As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + Path.GetRandomFileName
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim FTPSuccess As Boolean = False
        Dim FTPUtil As FTP.FTPclient
        Dim dReader As SqlDataReader = Nothing
        Dim response As ServiceResponse = Nothing
        Dim lineStr As String = String.Empty
        Dim _type As String = String.Empty
        Dim _fName As String = String.Empty

        ' set the SQL or procedure to run
        _type = GetCommonAppSetting("VIM_SUPPLEMENT_COST", Me._sqlConfigName)

        ' how should we name the file?
        _fName = ConfigurationManager.AppSettings.Get("Region") + "_COST_" + ConfigurationManager.AppSettings.Get("VIMExtractPrefix") + ConfigurationManager.AppSettings.Get("VIMFileExt")

        ' first, get the data
        Try
            factory.CommandTimeout = 36000
            dReader = factory.GetDataReader(_type)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.ADOError
            GoTo EndOfMethod
        End Try

        ' now create the file using the specified delimeter
        Try
            If File.Exists(tempFile) Then
                File.Delete(tempFile)
            End If

            Using sw As StreamWriter = New StreamWriter(tempFile, True)

                Dim fCount As Integer = dReader.FieldCount - 1
                Dim i As Integer

                If FTPParams.FTP_DELIM = String.Empty Then FTPParams.FTP_DELIM = ConfigurationManager.AppSettings.Get("ExportDelimiter")

                Do While dReader.Read
                    For i = 0 To fCount

                        If dReader.Item(i) Is DBNull.Value Then
                            ' handle nulls
                            lineStr &= String.Empty
                        Else
                            ' always convert True & False
                            Select Case dReader.Item(i).ToString
                                Case "True"
                                    lineStr &= "Y"
                                Case "False"
                                    lineStr &= "N"
                                Case Else
                                    lineStr &= dReader.Item(i).ToString
                            End Select
                        End If

                        If i < fCount Then
                            lineStr &= FTPParams.FTP_DELIM
                        End If
                    Next
                    sw.WriteLine(lineStr & FTPParams.FTP_DELIM)
                    lineStr = String.Empty
                Loop

                sw.Flush()
                sw.Close()
                sw.Dispose()

            End Using

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.FileBuilderError
            GoTo EndOfMethod
        End Try

        ' now FTP the file
        Try
            FTPUtil = New FTP.FTPclient(FTPParams.FTP_IP, FTPParams.FTP_UID, FTPParams.FTP_PWD, FTPParams.FTP_SECURE, FTPParams.FTP_PORT)

            If Not FTPParams.FTP_DIR = String.Empty Then
                If Not Mid(FTPParams.FTP_DIR, 1, 1) = "/" Then
                    FTPParams.FTP_DIR &= "/" & FTPParams.FTP_DIR
                End If
                _fName = FTPParams.FTP_DIR + "/" + _fName
            End If

            FTPUtil.Upload(tempFile, _fName, False)
            FTPSuccess = True

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            FTPSuccess = False
        Finally
            FTPUtil = Nothing
        End Try

        If Not FTPSuccess Then
            response = ServiceResponse.FTPError
        Else
            response = ServiceResponse.Success
        End If

EndOfMethod:

        ' now delete the temp file
        If File.Exists(tempFile) Then
            File.Delete(tempFile)
        End If

        If Not dReader Is Nothing Then
            dReader.Close()
        End If

        Return response

    End Function

    '<WebMethod(Description:="Creates a VIM Supplemental Item Data Extract file and FTPs it to the location specified in the FTPBO object.")> _
    ''' <summary>
    ''' Creates a VIM Supplemental Data Extract file and FTPs it to the location specified in the FTPBO object.
    ''' </summary>
    ''' <param name="FTPParams">FTOBO Object</param>
    ''' <returns>ServiceResponse</returns>
    ''' <remarks></remarks>
    Private Function GetVIMItemExtract(ByVal FTPParams As FTPBO) As ServiceResponse

        Dim tempFile As String = ConfigurationManager.AppSettings.Get("TempDIRPath") + Path.GetRandomFileName
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim FTPSuccess As Boolean = False
        Dim FTPUtil As FTP.FTPclient
        Dim dReader As SqlDataReader = Nothing
        Dim response As ServiceResponse = Nothing
        Dim lineStr As String = String.Empty
        Dim _type As String = String.Empty
        Dim _fName As String = String.Empty

        ' set the SQL or procedure to run
        _type = GetCommonAppSetting("VIM_SUPPLEMENT_ITEM", Me._sqlConfigName)

        ' how should we name the file?
        _fName = ConfigurationManager.AppSettings.Get("Region") + "_ITEM_" + ConfigurationManager.AppSettings.Get("VIMExtractPrefix") + ConfigurationManager.AppSettings.Get("VIMFileExt")

        ' first, get the data
        Try
            factory.CommandTimeout = 36000
            dReader = factory.GetDataReader(_type)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.ADOError
            GoTo EndOfMethod
        End Try

        ' now create the file using the specified delimeter
        Try
            If File.Exists(tempFile) Then
                File.Delete(tempFile)
            End If

            Using sw As StreamWriter = New StreamWriter(tempFile, True)

                Dim fCount As Integer = dReader.FieldCount - 1
                Dim i As Integer

                If FTPParams.FTP_DELIM = String.Empty Then FTPParams.FTP_DELIM = ConfigurationManager.AppSettings.Get("ExportDelimiter")

                Do While dReader.Read
                    For i = 0 To fCount

                        If dReader.Item(i) Is DBNull.Value Then
                            ' handle nulls
                            lineStr &= String.Empty
                        Else
                            ' always convert True & False
                            Select Case dReader.Item(i).ToString
                                Case "True"
                                    lineStr &= "Y"
                                Case "False"
                                    lineStr &= "N"
                                Case Else
                                    lineStr &= dReader.Item(i).ToString
                            End Select
                        End If

                        If i < fCount Then
                            lineStr &= FTPParams.FTP_DELIM
                        End If

                    Next
                    sw.WriteLine(lineStr & FTPParams.FTP_DELIM)
                    lineStr = String.Empty
                Loop

                sw.Flush()
                sw.Close()
                sw.Dispose()

            End Using

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            ' return the error response to the caller
            response = ServiceResponse.FileBuilderError
            GoTo EndOfMethod
        End Try

        ' now FTP the file
        Try
            FTPUtil = New FTP.FTPclient(FTPParams.FTP_IP, FTPParams.FTP_UID, FTPParams.FTP_PWD, FTPParams.FTP_SECURE, FTPParams.FTP_PORT)

            If Not FTPParams.FTP_DIR = String.Empty Then
                If Not Mid(FTPParams.FTP_DIR, 1, 1) = "/" Then
                    FTPParams.FTP_DIR &= "/" & FTPParams.FTP_DIR
                End If
                _fName = FTPParams.FTP_DIR + "/" + _fName
            End If

            FTPUtil.Upload(tempFile, _fName, False)
            FTPSuccess = True

        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            FTPSuccess = False
        Finally
            FTPUtil = Nothing
        End Try

        If Not FTPSuccess Then
            response = ServiceResponse.FTPError
        Else
            response = ServiceResponse.Success
        End If

EndOfMethod:

        ' now delete the temp file
        If File.Exists(tempFile) Then
            File.Delete(tempFile)
        End If

        If Not dReader Is Nothing Then
            dReader.Close()
        End If

        Return response

    End Function

    ''' <summary>
    ''' Creates a zip file.
    ''' </summary>
    ''' <param name="zipFileName">The name of the file to create.</param>
    ''' <param name="sourceDirectory">The location of the files to include in the zip file.</param>
    ''' <param name="recurse">Zip files in sub-directories.</param>
    ''' <param name="fileFilter">A regular expression filter to limit file search.</param>
    ''' <remarks></remarks>
    Private Sub CreateZip(ByVal zipFileName As String, ByVal sourceDirectory As String, ByVal recurse As Boolean, ByVal fileFilter As String)

        Dim _zip As Zip.FastZip
        Try
            _zip = New Zip.FastZip
            _zip.CreateZip(zipFileName, sourceDirectory, recurse, fileFilter)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            Throw New Exception(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Validates whether the specified string contains special characters not allowed in IRMA.
    ''' </summary>
    ''' <param name="str">The string to validate.</param>
    ''' <returns>Boolean</returns>
    ''' <remarks>A result of True indicates the string includes a special character not allowed in IRMA.</remarks>
    Private Function ValidateString(ByVal str As String) As Boolean

        Dim objRegExp
        objRegExp = New System.Text.RegularExpressions.Regex("[#""@;',&%!;+]")
        Dim strMatch As System.Text.RegularExpressions.Match = objRegExp.Match(str)

        Return strMatch.Success

    End Function

    ''' <summary>
    ''' Gets the specified key value from a custom configuration file.
    ''' </summary>
    ''' <param name="Key">Key value to retrieve</param>
    ''' <param name="ConfigFile">Configuration File Name</param>
    ''' <returns>Value(String)</returns>
    ''' <remarks></remarks>
    Private Function GetCustomAppSetting(ByVal Key As String, ByVal ConfigFile As String) As String

        Try
            Dim fPath As String = ConfigurationManager.AppSettings.Get("CustomConfigurationFilesPath") + ConfigFile
            Return ConfigurationFile.GetAppSetting(Key, fPath)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            Throw New Exception(ex.Message)
        End Try

    End Function

    ''' <summary>
    ''' Gets the specified key value from the Common configuration file.
    ''' </summary>
    ''' <param name="Key">Key value to retrieve</param>
    ''' <param name="ConfigFile">Configuration File Name</param>
    ''' <returns>Value(String)</returns>
    ''' <remarks></remarks>
    Private Function GetCommonAppSetting(ByVal Key As String, ByVal ConfigFile As String) As String

        Try
            Dim fPath As String = ConfigurationManager.AppSettings.Get("CommonConfigurationFilesPath") + ConfigFile
            Return ConfigurationFile.GetAppSetting(Key, fPath)
        Catch ex As Exception
            WriteToEventLog(ex.Message, EventLogEntryType.Error)
            Throw New Exception(ex.Message)
        End Try

    End Function

    ''' <summary>
    ''' Writes to the Event Log.
    ''' </summary>
    ''' <param name="strLogEntry">The message to log.</param>
    ''' <param name="logType">EventLogEntryType</param>
    ''' <remarks>Additional server configuration is necessary to use this method.
    ''' SEE http://support.microsoft.com/default.aspx?scid=kb;en-us;329291 - Implement the FIRST approach, 
    ''' replacing TEST with IRMAWebService where specified.</remarks>
    Private Sub WriteToEventLog(ByVal strLogEntry As String, ByVal logType As EventLogEntryType)
        Try
            EventLog.WriteEntry("IRMAWebService", strLogEntry, logType)
        Catch ex As Exception
            Throw New Exception("An error ocurred trying to write to the event log. See http://support.microsoft.com/default.aspx?scid=kb;en-us;329291 for more information. Implement this FIRST approach, replacing TEST with IRMAWebService where specified.")
        End Try

    End Sub

    ''' <summary>
    ''' Sends the zip files created by the GetPOSPushLogs web method to the specified recipient.
    ''' </summary>
    ''' <param name="Recipient">The recipient of the zip files.</param>
    ''' <param name="Attachments">POSPush Archive and Log zip file attachments</param>
    ''' <remarks></remarks>
    Private Overloads Sub CreateMessage(ByVal Recipient As String, ByVal Attachments As AttachmentCollection)

        Dim _sender As New MailAddress(ConfigurationManager.AppSettings.Get("EmailFrom"))
        Dim _receiver As New MailAddress(Recipient)
        Dim message As New MailMessage(_sender, _receiver)

        Try

            message.Body = "POSPush Archive & Log files from IRMA Web Services"
            message.Subject = "Daily Automated POS Push Log/Archive Retrieval"

            Dim _attachment As Attachment
            For Each _attachment In Attachments
                message.Attachments.Add(_attachment)
            Next

            Dim client As New SmtpClient(ConfigurationManager.AppSettings.Get("smtpServer").ToString)

            client.Credentials = CredentialCache.DefaultNetworkCredentials

            client.Send(message)

        Catch ex As Exception
            Throw New Exception(ServiceResponse.MailError.ToString)
        Finally
            message.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' Can be used to send a generic mail message
    ''' </summary>
    ''' <param name="Recipient">Email recipient</param>
    ''' <param name="strMessage">Message to send</param>
    ''' <remarks></remarks>
    Private Overloads Sub CreateMessage(ByVal Recipient As String, ByVal strMessage As String, ByVal mPriority As Mail.MailPriority)

        Dim _sender As New MailAddress(ConfigurationManager.AppSettings.Get("EmailFrom"))
        Dim _receiver As New MailAddress(Recipient)
        Dim message As New MailMessage(_sender, _receiver)

        Try

            message.Priority = mPriority
            message.Body = strMessage
            message.Subject = "IRMA Web Services Notification"

            Dim client As New SmtpClient(ConfigurationManager.AppSettings.Get("smtpServer").ToString)

            client.Credentials = CredentialCache.DefaultNetworkCredentials
            client.Send(message)

        Catch ex As Exception
            Throw New Exception(ServiceResponse.MailError.ToString)
        Finally
            message.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' Validates that an email is in the correct format
    ''' </summary>
    ''' <param name="email">Email address to validate</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function ValidateEmail(ByVal email As String) As Boolean

        Dim emailRegex As New System.Text.RegularExpressions.Regex("^(?<user>[^@]+)@(?<host>.+)$")
        Dim emailMatch As System.Text.RegularExpressions.Match = emailRegex.Match(email)

        Return emailMatch.Success

    End Function

#End Region

End Class