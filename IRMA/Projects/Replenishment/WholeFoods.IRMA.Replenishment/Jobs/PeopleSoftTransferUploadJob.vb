Option Strict Off

Imports log4net
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.PeopleSoftUpload.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.PeopleSoftUpload.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Ordering.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that sends receiving data from IRMA to PeopleSoft to allow for the payment of external vendors.
    ''' </summary>
    Public Class PeopleSoftTransferUploadJob
        ' -----------------------------------------------------------------
        ' Update History
        ' -----------------------------------------------------------------
        ' TFS 12198 (v3.6)
        ' Tom Lux
        ' 04/12/2010
        ' Removed using system time in this class for the 'Uploaded Date' value in the order.
        ' This was moved to the DB proc, as the DB-system time is preferable to app-server time (matches design for other jobs).


        Inherits ScheduledJob
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' People soft output file - text format
        Private _outFileText As StreamWriter = Nothing

        ' Encoding to use on the output file
        Protected _fileEncoding As Encoding = Encoding.GetEncoding(Encoding.Default.CodePage)

        ' Flag to track the status of this job.  Any errors that are encountered, but handled by the job
        ' should set this flag to FALSE and provide an error description to the user in _jobExecutionMessage.
        Dim jobSuccess As Boolean = True

        Private _period As Integer
        Private _year As Integer
        Private _week As Integer
        Private _dt As DataTable = Nothing
        Private _dr As DataRow = Nothing
        Private _strDate1 As String = String.Empty
        Private _strDate2 As String = String.Empty
        Private iGLRowNumber As Integer = 0

        'App config keys
        Private _regionalCurrency As String = "USD"
        Private _accountExclusionList As String = ""

        Private _regionalBusinessUnit As String = ""
        Private _365regionalBusinessUnit As String = ""

        Private ExcludedAccountList As String()

        ''' <summary>
        ''' This is the method that performs the work for the job.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Function RunProcess() As Boolean
            logger.Debug("RunProcess entry")
            Dim results As SqlDataReader = Nothing
            Dim regionListEnum As IEnumerator = Nothing
            Dim currentRegion As String
            Dim dt As DataTable = Nothing
            Dim bhasErrors As Boolean = False

            logger.Info("Reading the list of regional identifiers from the database")

            Try
                _dt = GLDAO.GetFiscalCalendarInfo(DateTime.Today)
                If _dt.Rows.Count > 0 Then
                    _dr = _dt.Rows(0)
                    _period = _dr("FiscalPeriod")
                    _year = _dr("FiscalYear")
                    _week = _dr("FiscalWeek")
                End If

                _strDate1 = "FP" + _period.ToString.PadLeft(2, "0") + _year.ToString.Substring(2, 2) + "W" + _week.ToString
                _strDate2 = "WK" + _week.ToString + "FP" + _period.ToString.PadLeft(2, "0") + _year.ToString.Substring(2, 2)
                regionListEnum = PeopleSoftUploadDAO.GetRegionList().GetEnumerator()

                'Get the regional currency from app config
                _regionalCurrency = ConfigurationServices.AppSettings("Currency")

                'Get the list of accounts for which no team-subteams will be on the GL lines from app config
                _accountExclusionList = ConfigurationServices.AppSettings("GLTransferAccountExclusionList")

                'Get the regional business unit
                '_regionalBusinessUnit = ConfigurationServices.AppSettings("RegionalBU")
                '_365regionalBusinessUnit = ConfigurationServices.AppSettings("365RegionalBU")

                If _accountExclusionList <> String.Empty Then
                    Dim delimeterstr As String = ","
                    ExcludedAccountList = _accountExclusionList.Split(delimeterstr)

                    For i As Integer = 0 To ExcludedAccountList.Length - 1
                        ExcludedAccountList(i) = Trim(ExcludedAccountList(i))
                    Next i
                End If

                While regionListEnum.MoveNext()
                    currentRegion = CStr(regionListEnum.Current)
                    logger.Info("Processing Region " + currentRegion)

                    'Get the regional business unit
                    If currentRegion = "TS" Then
                        _regionalBusinessUnit = ConfigurationServices.AppSettings("365RegionalBU")
                    Else
                        _regionalBusinessUnit = ConfigurationServices.AppSettings("RegionalBU")
                    End If

                    Try
                        '=================
                        ' GL Upload
                        '=================
                        ' Read the records that are ready to be sent from IRMA to PeopleSoft for this region
                        logger.Info("Reading the GL records for PeopleSoft upload from the database")
                        Try
                            dt = GLDAO.GetGLTransferTransactions(GetExportParameters, currentRegion)
                        Catch ex As Exception
                            Throw New Exception("Step: GLDAO.GetGLTransactions(GetExportParameters)" & Environment.NewLine & ex.InnerException.Message)
                        End Try
                        ' Add the result set values to the output file for the region
                        logger.Info("Adding the GL records to the output file")
                        Try
                            AddGLResultsToOutputFile(dt, currentRegion)
                        Catch ex As Exception
                            bhasErrors = True
                            Throw New Exception("Step: GL AddGLResultsToOutputFile(dt, currentRegion)" & Environment.NewLine & ex.InnerException.Message)
                        End Try

                        Try
                            GLDAO.CommitGLTransferTransactions(currentRegion)
                        Catch ex As Exception
                            bhasErrors = True
                            Throw New Exception("Step: GLDAO.CommitGLTransferTransactions()" & Environment.NewLine & ex.InnerException.Message)
                        End Try

                    Catch ex As Exception
                        bhasErrors = True
                        Throw New Exception(ex.Message)
                    Finally
                        ' Close the results set 
                        Try
                            results.Close()
                        Catch ignore As Exception
                            ' this can be ignored
                        End Try
                    End Try

                End While
            Catch ex As Exception
                bhasErrors = True
                OpsGenieUtility.SendMail("IRMA PeopleSoft Transfer Upload Job Failure", ex.ToString())
                Throw New Exception(Classname & " failed at " & IIf(ex.Message.Contains("Step"), ex.Message, "Step: PeopleSoftUploadDAO.GetRegionList() " & Environment.NewLine & ex.InnerException.Message))
            End Try

            jobSuccess = Not bhasErrors

            logger.Debug("RunProcess exit: jobSuccess=" + jobSuccess.ToString)
            Return jobSuccess
        End Function


        ''' <summary>
        ''' Sets the temporary filename for the local EDI file that is generated during processing.
        ''' </summary>
        ''' <remarks></remarks>
        Private Function GenerateTempFilename(ByVal region As String, ByVal sType As String) As String
            logger.Debug("GenerateTempFilename entry: region=" + region)
            Dim filepath As String
            Dim tempFilename As New StringBuilder

            'create sub directory in temp location specified in app.config
            filepath = Path.Combine(ConfigurationServices.AppSettings("PSFileDir"), region)

            'filepath = "D:\Work\PsFiles\"

            'check for existence of directory before adding the new file
            If Not Directory.Exists(filepath) Then
                Directory.CreateDirectory(filepath)
            End If

            If sType = "GL" Then
                ' set the filename as root dir\PSGL_[region]_YYYYMMDD.txt
                tempFilename.Append(ConfigurationServices.AppSettings("PSGLFilePrefix"))
                tempFilename.Append("_")
                tempFilename.Append(region)
                tempFilename.Append("_")
                tempFilename.Append(Format(Today, "yyyyMMdd"))
                tempFilename.Append(".txt")
            End If

            logger.Debug("GenerateTempFilename exit")
            Return Path.Combine(filepath, tempFilename.ToString)
        End Function

        ''' <summary>
        ''' Open the EDI output file for processing.
        ''' </summary>
        ''' <remarks></remarks>
        Private Function OpenOutputFile(ByVal region As String, ByVal sType As String) As Boolean
            logger.Debug("OpenOutputFile enter: region=" + region)
            Dim success As Boolean = True

            ' Open the file for processing if it is not already open.
            ' If file exits then data will be appended.
            Dim filename As String = Nothing
            Try
                filename = GenerateTempFilename(region, sType)
                logger.Info("Opening the file for processing: " + filename)
                If Not ((_outFileText IsNot Nothing) AndAlso (_outFileText.BaseStream.CanWrite)) Then
                    _outFileText = New StreamWriter(filename, True, _fileEncoding)
                End If
            Catch ex As Exception
                success = False
                ' Log the exception and send out an email error notification.
                logger.Error("PeopleSoftTransferUploadJob failed when trying to open the output file for processing.", ex)
                Dim args(1) As String
                args(0) = filename
                ErrorHandler.ProcessError(ErrorType.PeopleSoftUpload_FileError, args, SeverityLevel.Fatal, ex)

                ' Set the job status flag in case this job is being executed from the UI to notify the user of the
                'error that was encoutered, but handled.
                jobSuccess = False
                Dim msg As New StringBuilder
                msg.Append("PeopleSoftTransferUploadJob failed when trying to open the output file for processing.")
                msg.Append(Environment.NewLine)
                msg.Append(ex.Message)
                msg.Append(Environment.NewLine)
                msg.Append(ex.StackTrace)
                _jobExecutionMessage = msg.ToString()
            End Try
            logger.Debug("OpenOutputFile exit: success=" + success.ToString())
            Return success
        End Function

        Private Function GetExportParameters() As GLBO

            Dim _glBO As New GLBO

            _glBO.TransactionType = 5
            _glBO.StoreNo = -1
            _glBO.StartDate = Now.Date.ToShortDateString
            _glBO.EndDate = Now.Date.ToShortDateString
            _glBO.CurrentDate = String.Empty

            Return _glBO
        End Function

        Private Sub AddGLResultsToOutputFile(ByVal results As DataTable, ByVal region As String)
            logger.Debug("AddResultsToOutputFile entry for region=" + region)
            Dim currentGLRecord As GLBO
            Dim dataRow As DataRow
            Dim blnWriteHeader As Boolean = False

            Try
                ' Open a GL txt output file
                If OpenOutputFile(region, "GL") Then
                    ' Define a variable to keep track of the last order header id processed
                    Dim previousBusinessUnitID As Integer = -1
                    Dim sameStore As Integer = -1
                    Dim betweenStoresGroup As Boolean = False

                    ' Add each record in the result set to the output file
                    logger.Info("Begin processing the result set, adding each record to the PeopleSoft GL output file")
                    Dim recordCount As Integer = 0

                    For Each dataRow In results.Rows
                        ' Populate a new upload BO with the next result set
                        currentGLRecord = New GLBO(dataRow)

                        If currentGLRecord.SameStore = 1 Then
                            If previousBusinessUnitID <> currentGLRecord.BusinessUnitId Then
                                previousBusinessUnitID = currentGLRecord.BusinessUnitId
                                iGLRowNumber = 0
                                blnWriteHeader = True                               
                            End If
                        Else
                            If betweenStoresGroup = False Then
                                iGLRowNumber = 0
                                blnWriteHeader = True
                                betweenStoresGroup = True
                            End If
                        End If

                        recordCount += 1
                        iGLRowNumber = iGLRowNumber + 1

                        ' Set the upload date to today
                        currentGLRecord.JournalDate = Now.Date.ToShortDateString

                        If blnWriteHeader Then
                            logger.Info("Adding the header row for BusinessUnitID=" + currentGLRecord.BusinessUnitId.ToString())
                            _outFileText.WriteLine(BuildPeopleSoftGLHeaderRow(currentGLRecord))
                            blnWriteHeader = False
                        End If

                        logger.Info("Adding the line row for BusinessUnitID=" + currentGLRecord.BusinessUnitId.ToString())

                        Dim _uploadRow As String = BuildPeopleSoftGLDetailRow(currentGLRecord)

                        If currentGLRecord.AccountNumber.Equals(String.Empty) Then
                            ' Log the exception and send out an email error notification.
                            Dim _errormsg As String = "PROCESSING ERROR BU" & currentGLRecord.BusinessUnitId.ToString & " - MISSING GL ACCOUNT INFORMATION - Record Omitted from GL Upload: " & vbNewLine & _uploadRow
                            logger.Error(_errormsg)
                            Dim args(1) As String
                            args(0) = _errormsg
                            ErrorHandler.ProcessError(ErrorType.PeopleSoftUpload_FileError, args, SeverityLevel.Warning, Nothing)
                        Else
                            _outFileText.WriteLine(_uploadRow)
                        End If
                    Next

                    Dim currentMsg As String = "Completed processing the result set: Region=" + region + ", Record Count=" + recordCount.ToString
                    logger.Info(currentMsg)
                    _jobExecutionMessage += Environment.NewLine + currentMsg
                End If
            Finally
                ' Close the output file
                Try
                    _outFileText.Flush()
                    _outFileText.Close()
                    _outFileText = Nothing
                Catch ignore As Exception
                    ' this can be ignored
                End Try
            End Try
            logger.Debug("AddResultsToOutputFile exit")
        End Sub

        Private Function BuildPeopleSoftGLHeaderRow(ByRef currentGLRecord As GLBO) As String
            logger.Debug("BuildPeopleSoftHeaderRow entry: currentGLRecord.BusinessUnitID=" + currentGLRecord.BusinessUnitId.ToString())

            Dim yesterday As Date = Now.AddDays(-1)
            Dim currentLine As New StringBuilder()

            currentLine.Append("H")                                                                         'Record Type
            currentLine.Append(_regionalBusinessUnit.ToString.PadRight(5, " "))                             'Regional Business Unit
            currentLine.Append("NEXT".PadRight(10, " "))                                                    'Journal ID

            'PBI 25066  02/02/2018 Make yesterday as journal date
            currentLine.Append(yesterday.ToString("MMddyyyy").PadRight(8, " "))                           'Journal Date

            currentLine.Append("GLBOOK".PadRight(10, " "))                                                  'Ledger Group
            currentLine.Append("N")                                                                         'Reversal Code
            currentLine.Append(" ".PadRight(8, " "))                                                        'Reversal Date
            currentLine.Append("XLS")                                                                       'Source
            currentLine.Append(" ".PadRight(8, " "))                                                        'Transaction Reference Number

            If currentGLRecord.SameStore = 1 Then
                currentLine.Append(CStr("IRMA" & _strDate1 & "TransSS" & currentGLRecord.BusinessUnitId).PadRight(30, " ")) 'Journal Description
            Else
                currentLine.Append(CStr("IRMA" & _strDate1 & "TransBS " & _regionalBusinessUnit).PadRight(30, " ")) 'Journal Description
            End If

            currentLine.Append(_regionalCurrency)                                                           'Currency Type

            currentLine.Append("CRRNT")                                                                     'Rate Type
            currentLine.Append(" ".PadRight(8, " "))                                                        'Current Effective Date
            currentLine.Append("1".PadRight(16, " "))                                                       'Rate Mult
            currentLine.Append(" ".PadRight(8, " "))                                                        'Document Type
            currentLine.Append(" ".PadRight(12, " "))                                                       'Document Sequence Number
            currentLine.Append(" ".PadRight(8, " "))                                                        'Blank Field
            currentLine.Append("EXT")                                                                       'System Source

            logger.Debug("BuildPeopleSoftHeaderRow exit")
            Return currentLine.ToString()
        End Function

        Private Function IsExlcudedAccount(ByVal accountNumber As String) As Boolean

            Dim found As Boolean = False

            If ExcludedAccountList Is Nothing Then Return False

            Dim total As Integer = ExcludedAccountList.Length
            If total <= 0 Then Return False


            For i As Integer = 0 To total - 1
                If accountNumber = ExcludedAccountList(i) Then
                    found = True
                    Exit For
                End If
            Next i

            Return found
        End Function

        Private Function BuildPeopleSoftGLDetailRow(ByRef currentGLRecord As GLBO) As String
            logger.Debug("BuildPeopleSoftGLDetailRow entry: currentGLRecord.BusinessUnitID=" + currentGLRecord.BusinessUnitId.ToString())

            Dim currentLine As New StringBuilder()
            currentLine.Append("L")                                                                             'Record Type
            currentLine.Append(currentGLRecord.BusinessUnitId.ToString.PadRight(5, " "))                        'Business Unit
            currentLine.Append(iGLRowNumber.ToString.PadLeft(9, " "))                                          'Journal Line
            currentLine.Append("ACTUAL".PadRight(10, " "))                                                     'Ledger
            currentLine.Append(currentGLRecord.AccountNumber.ToString.PadRight(6, " "))                         'Account

            If IsExlcudedAccount(currentGLRecord.AccountNumber) Then
                currentLine.Append(" ".PadRight(10, " "))                                                       'Department ID
                currentLine.Append(" ".PadRight(6, " "))                                                        'Product
            Else
                currentLine.Append(IIf(currentGLRecord.DepartmentId = 0, " ", currentGLRecord.DepartmentId).ToString.PadRight(10, " "))'Department ID
                If currentGLRecord.ProductId = 0 Then
                    currentLine.Append(" ".PadRight(6, " "))
                Else
                    currentLine.Append(currentGLRecord.ProductId.ToString.PadRight(6, " "))                                                 'Product
                End If
            End If

            currentLine.Append(" ".PadRight(15, " "))                                                           'Project ID
            currentLine.Append(" ".PadRight(5, " "))                                                            'Affiliate
            currentLine.Append(" ".PadRight(3, " "))                                                            'Statistics Code

            currentLine.Append(currentGLRecord.Currency)                                                        'Currency Type

            currentLine.Append(currentGLRecord.Amount.ToString("F").PadRight(28, " "))                          'Monetary Amount
            currentLine.Append(" ".PadRight(4, " "))                                                            'Blank Field
            currentLine.Append(" ".PadRight(10, " "))                                                           'Journal Line Reference

            If currentGLRecord.SameStore = 1 Then
                currentLine.Append(CStr("OPSTRANS-Same Store " & _strDate2).PadRight(30, " ")) 'Line Description
            Else
                currentLine.Append(CStr("OPSTRANS-Btwn Stores " & _strDate2).PadRight(30, " ")) 'Line Description
            End If

            currentLine.Append("CRRNT")                                                                         'Rate Type
            currentLine.Append("1".PadRight(16, " "))                                                           'Rate Mult
            currentLine.Append(currentGLRecord.Amount.ToString("F").PadRight(16, " "))                          'Foreign Amount
            currentLine.Append("N")                                                                             'Movement Flag
            currentLine.Append(" ".PadRight(30, " "))                                                           'Open Item Key

            logger.Debug("BuildPeopleSoftGLDetailRow exit")
            Return currentLine.ToString()
        End Function
    End Class
End Namespace

