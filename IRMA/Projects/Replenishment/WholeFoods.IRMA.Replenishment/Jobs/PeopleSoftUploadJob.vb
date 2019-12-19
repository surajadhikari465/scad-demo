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
    Public Class PeopleSoftUploadJob
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

        Private iGLRowNumber As Integer = 0

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
            'Dim dr As DataRow
            Dim bhasErrors As Boolean = False
            ' Is there data for more than one region stored in this IRMA instance?  If so, generate a 
            ' separate output file for each region.
            logger.Info("Reading the list of regional identifiers from the database")

            Try

                regionListEnum = PeopleSoftUploadDAO.GetRegionList().GetEnumerator()

                While regionListEnum.MoveNext()
                    currentRegion = CStr(regionListEnum.Current)
                    logger.Info("Processing Region " + currentRegion)
                    Try
                        '=================
                        ' AP Upload         
                        '=================
                        ' Read the records that are ready to be sent from IRMA to PeopleSoft for this region
                        logger.Info("Reading the AP records for PeopleSoft upload from the database")
                        Try
                            results = PeopleSoftUploadDAO.GetAPUploads(currentRegion)
                        Catch ex As Exception
                            bhasErrors = True
                            Throw New Exception("Step: PeopleSoftUploadDAO.GetAPUploads(currentRegion)" & Environment.NewLine & ex.InnerException.Message)
                        End Try

                        ' Add the result set values to the output file for the region
                        logger.Info("Adding the AP records to the output file")
                        Try
                            AddResultsToOutputFile(results, currentRegion)
                        Catch ex As Exception
                            bhasErrors = True
                            Throw New Exception("Step: AP AddResultsToOutputFile(results, currentRegion) " & Environment.NewLine & ex.InnerException.Message)
                        End Try

                        '=================
                        ' GL Upload
                        '=================
                        '20101208 - DaveStacey - TFS 13790 - Conditionally skip GLUpload piece via app config
                        Dim boolSkipGLUpload As Boolean = ConfigurationServices.AppSettings("SkipGLUpload")

                        'TFS 11384, 03/30/2013, Faisal Ahmed - Disable the creating of GL data file from this AP upload job
                        boolSkipGLUpload = True

                        If boolSkipGLUpload = False Then
                            ' Read the records that are ready to be sent from IRMA to PeopleSoft for this region
                            logger.Info("Reading the GL records for PeopleSoft upload from the database")
                            Try
                                dt = GLDAO.GetGLTransactions(GetExportParameters)
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

                            'Try
                            '    GLDAO.CommitGLTransactions(GetExportParameters)
                            'Catch ex As Exception
                            '    bhasErrors = True
                            '    Throw New Exception("Step: GLDAO.CommitGLTransactions(GetExportParameters)" & Environment.NewLine & ex.InnerException.Message)
                            'End Try
                        End If
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


                OpsGenieUtility.SendMail("IRMA PeopleSoft Upload Job Failure", ex.ToString())
                Throw New Exception(Classname & " failed at " & IIf(ex.Message.Contains("Step"), ex.Message, "Step: PeopleSoftUploadDAO.GetRegionList() " & Environment.NewLine & ex.InnerException.Message))
            End Try

            jobSuccess = Not bhasErrors

            logger.Debug("RunProcess exit: jobSuccess=" + jobSuccess.ToString)
            Return jobSuccess
        End Function

        ''' <summary>
        ''' Processes the results set for a single region, adding each of the records
        ''' to the output file.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Private Sub AddResultsToOutputFile(ByRef results As SqlDataReader, ByVal region As String)
            logger.Debug("AddResultsToOutputFile entry for region=" + region)
            Dim currentPSRecord As PeopleSoftUploadBO
            Dim softCurrencyPSRec As PeopleSoftUploadBO = Nothing
            Dim bUseSoftRec As Boolean = False
            Try
                ' Open an EDI output file
                If OpenOutputFile(region, "AP") Then
                    ' Define a variable to keep track of the last order header id processed
                    Dim previousOrderHeaderID As Integer = -1
                    Dim previousFreightOrderType As Boolean = False
                    Dim skipOutput As Boolean

                    ' Add each record in the result set to the output file
                    logger.Info("Begin processing the result set, adding each record to the PeopleSoft output file")
                    Dim recordCount As Integer = 0
                    Dim processedOrderCount As Integer = 0
                    While results.Read()
                        ' Populate a new upload BO with the next result set
                        currentPSRecord = New PeopleSoftUploadBO(results)
                        recordCount += 1

                        ' Have we switched order headers, making sure to take into account the type of order?
                        If previousOrderHeaderID <> currentPSRecord.OrderHeaderID Or (previousOrderHeaderID = currentPSRecord.OrderHeaderID AndAlso previousFreightOrderType <> currentPSRecord.ThirdPartyFreightInvoice) Then

                            ' TFS 11556 BR 006 Line should only occur once per order and should be the last line
                            If Not skipOutput And bUseSoftRec Then
                                ' Add additional line to the output file:  a single order will contain a single currency row
                                logger.Info("Adding the currency row for OrderHeaderID=" + softCurrencyPSRec.OrderHeaderID.ToString() + ", VendorCurr=" + softCurrencyPSRec.CURR_VENDOR_CODE.ToString + ", BUCurr=" + softCurrencyPSRec.CURR_BU_CODE.ToString)
                                _outFileText.WriteLine(BuildPeopleSoftCurrencyRow(softCurrencyPSRec))
                                bUseSoftRec = False
                            End If

                            logger.Info("Now beginning processing OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString())
                            ' Include the current record in the output file if the gross amount is not zero
                            If currentPSRecord.GROSS_AMT = 0 Then
                                logger.Info("Skipping the output rows because GROSS_AMT=0 for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", Freight3Party=" + currentPSRecord.ThirdPartyFreightInvoice.ToString)
                                skipOutput = True
                            Else
                                skipOutput = False
                            End If

                            If Not skipOutput Then
                                ' Add two lines to the output file: each order contains one header row and one voucher row
                                logger.Info("Adding the header row for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", Freight3Party=" + currentPSRecord.ThirdPartyFreightInvoice.ToString)
                                _outFileText.WriteLine(BuildPeopleSoftHeaderRow(currentPSRecord))
                                logger.Info("Adding the line row for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", Freight3Party=" + currentPSRecord.ThirdPartyFreightInvoice.ToString)
                                _outFileText.WriteLine(BuildPeopleSoftVoucherRow(currentPSRecord))
                            End If
                        End If

                        If Not skipOutput Then
                            ' Add one more line to the output file:  a single order can contain multiple distribution rows
                            logger.Info("Adding the distribution row for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", Freight3Party=" + currentPSRecord.ThirdPartyFreightInvoice.ToString)
                            _outFileText.WriteLine(BuildPeopleSoftDistributionRow(currentPSRecord))
                        End If

                        If Not skipOutput And currentPSRecord.VCHR_CURR_ROW_ID = "006" AndAlso bUseSoftRec = False Then
                            '  TFS 11556 BR commented out the 2 lines below, this line should only be written at the end of the order
                            ' Add additional line to the output file:  a single order will contain a single currency row
                            'logger.Info("Adding the currency row for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", VendorCurr=" + currentPSRecord.CURR_VENDOR_CODE.ToString + ", BUCurr=" + currentPSRecord.CURR_BU_CODE.ToString)
                            '_outFileText.WriteLine(BuildPeopleSoftCurrencyRow(currentPSRecord))
                            softCurrencyPSRec = currentPSRecord
                            bUseSoftRec = True
                        End If

                        ' Have we switched order headers?
                        If previousOrderHeaderID <> currentPSRecord.OrderHeaderID Or (previousOrderHeaderID = currentPSRecord.OrderHeaderID AndAlso previousFreightOrderType <> currentPSRecord.ThirdPartyFreightInvoice) Then
                            previousOrderHeaderID = currentPSRecord.OrderHeaderID
                            previousFreightOrderType = currentPSRecord.ThirdPartyFreightInvoice
                            ' Mark the processed PO as uploaded in IRMA
                            logger.Info("Completed processing for OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", Freight3Party=" + currentPSRecord.ThirdPartyFreightInvoice.ToString + ".  Setting the order to uploaded in IRMA.")
                            PeopleSoftUploadDAO.SetOrderAsUploaded(currentPSRecord)
                            processedOrderCount += 1
                        End If
                    End While

                    ' TFS 11556 BR 006 Line should only occur once per order and should be the last line
                    If Not skipOutput And bUseSoftRec Then
                        ' Add additional line to the output file:  a single order will contain a single currency row
                        logger.Info("Adding the currency row for OrderHeaderID=" + softCurrencyPSRec.OrderHeaderID.ToString() + ", VendorCurr=" + softCurrencyPSRec.CURR_VENDOR_CODE.ToString + ", BUCurr=" + softCurrencyPSRec.CURR_BU_CODE.ToString)
                        _outFileText.WriteLine(BuildPeopleSoftCurrencyRow(softCurrencyPSRec))
                        bUseSoftRec = False
                    End If

                    Dim currentMsg As String = "Completed processing the result set: Region=" + region + ", Record Count=" + recordCount.ToString + ", Completed Order Count=" + processedOrderCount.ToString
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

        ''' <summary>
        ''' Sets the temporary filename for the local EDI file that is generated during processing.
        ''' </summary>
        ''' <remarks></remarks>
        Private Function GenerateTempFilename(ByVal region As String, ByVal sType As String) As String
            logger.Debug("GenerateTempFilename entry: region=" + region)
            ' create sub directory in temp location specified in app.config
            Dim filePath As String = Path.Combine(ConfigurationServices.AppSettings("PSFileDir"), region)

            Try  ' check for existence of directory before adding the new file
                If Not Directory.Exists(filePath) Then
                    Directory.CreateDirectory(filePath)
                End If
            Catch ex As Exception
                Throw New Exception($"PeopleSoftUpload.GenerateTempFilename(): Failed to create directory {filePath}", ex.InnerException)
            End Try

            Dim prefix$ = If(sType = "GL", "PSGLFilePrefix", "PSFilePrefix")
            Dim ext$ = If(sType = "GL", "txt", "EDI")

            logger.Debug("GenerateTempFilename exit")
            'File name: Prefix_Region_Date.Ext
            Return Path.Combine(filePath, String.Format("{0}_{1}_{2}.{3}",
                    ConfigurationServices.AppSettings(prefix),
                    region,
                    Format(Today, "yyyyMMdd"),
                    ext))
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
                logger.Info("Opening the EDI file for processing: " + filename)
                If Not ((_outFileText IsNot Nothing) AndAlso (_outFileText.BaseStream.CanWrite)) Then
                    _outFileText = New StreamWriter(filename, True, _fileEncoding)
                End If
            Catch ex As Exception
                success = False
                ' Log the exception and send out an email error notification.
                logger.Error("PeopleSoftUploadJob failed when trying to open the output file for processing.", ex)
                Dim args(1) As String
                args(0) = filename
                ErrorHandler.ProcessError(ErrorType.PeopleSoftUpload_FileError, args, SeverityLevel.Fatal, ex)

                ' Set the job status flag in case this job is being executed from the UI to notify the user of the
                ' error that was encoutered, but handled.
                jobSuccess = False
                Dim msg As New StringBuilder
                msg.Append("PeopleSoftUploadJob failed when trying to open the output file for processing.")
                msg.Append(Environment.NewLine)
                msg.Append(ex.Message)
                msg.Append(Environment.NewLine)
                msg.Append(ex.StackTrace)
                _jobExecutionMessage = msg.ToString()
            End Try
            logger.Debug("OpenOutputFile exit: success=" + success.ToString())
            Return success
        End Function

        ''' <summary>
        ''' Build the first line that is included in the PeopleSoft output file for a record.
        ''' </summary>
        ''' <param name="currentPSRecord"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildPeopleSoftHeaderRow(ByRef currentPSRecord As PeopleSoftUploadBO) As String
            logger.Debug("BuildPeopleSoftHeaderRow entry: currentPSRecord.OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", currentPSRecord.VCHR_HDR_ROW_ID=" + currentPSRecord.VCHR_HDR_ROW_ID)
            Dim currentLine As New StringBuilder()
            currentLine.Append(currentPSRecord.VCHR_HDR_ROW_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VOUCHER_ID)
            currentLine.Append("|")
            currentLine.Append("""")
            currentLine.Append(currentPSRecord.INVOICE_ID)     ' required
            currentLine.Append("""")
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.INVOICE_DT, "yyyy/MM/dd"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VENDOR_SETID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VENDOR_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VNDR_LOC)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ADDRESS_SEQ_NUM)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.GRP_AP_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ORIGIN)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.OPRID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VCHR_TTL_LINES)     ' required
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.ACCOUNTING_DT, "yyyy/MM/dd"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.POST_VOUCHER)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DST_CNTRL_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VOUCHER_ID_RELATED)
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.GROSS_AMT, "######0.00"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DSCNT_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.USETAX_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SALETX_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SALETX_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.FREIGHT_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DUE_DT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DSCNT_DUE_DT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PYMNT_TERMS_CD)
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.ENTERED_DT, "yyyy/MM/dd"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TXN_CURRENCY_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RT_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RATE_MULT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RATE_DIV)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_ENTRD_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.MATCH_ACTION)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.MATCH_STATUS_VCHR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BCM_TRAN_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CNTRCT_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.REMIT_ADDR_SEQ_NUM)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CUR_RT_SOURCE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DSCNT_AMT_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DUE_DT_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VCHR_APPRVL_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSPROCNAME)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.APPR_RULE_SET)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_DCLRTN_POINT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_CALC_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_ENTITY)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_TXN_TYPE_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TAX_CD_VAT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RCRD_INPT_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RCRD_OUTPT_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RECOVERY_PCT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_CALC_GROSS_NET)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RECALC_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_CALC_FRGHT_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RGSTRN_SELLER)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.COUNTRY_SHIP_FROM)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.COUNTRY_SHIP_TO)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.COUNTRY_VAT_BILLFR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.COUNTRY_VAT_BILLTO)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_TREATMENT_PUR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_EXCPTN_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_EXCPTN_CERTIF)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_USE_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DSCNT_PRORATE_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.USETAX_PRORATE_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SALETX_PRORATE_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.FRGHT_PRORATE_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.IST_TXN_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DOC_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DOC_SEQ_DATE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DOC_SEQ_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_CF_ANLSYS_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DESCR254_MIXED)
            currentLine.Append("|")

            logger.Debug("BuildPeopleSoftHeaderRow exit")
            Return currentLine.ToString()
        End Function

        ''' <summary>
        ''' Build the first second that is included in the PeopleSoft output file for a record.
        ''' </summary>
        ''' <param name="currentPSRecord"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildPeopleSoftVoucherRow(ByRef currentPSRecord As PeopleSoftUploadBO) As String
            logger.Debug("BuildPeopleSoftLineRow entry: currentPSRecord.OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", currentPSRecord.VCHR_HDR_ROW_ID=" + currentPSRecord.VCHR_HDR_ROW_ID)
            Dim currentLine As New StringBuilder()
            currentLine.Append(currentPSRecord.VCHR_LINE_ROW_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.LINE_BUSINESS_UNIT_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.LINE_VOUCHER_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VOUCHER_LINE_NUM)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TOTAL_DISTRIBS)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_PO)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PO_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.LINE_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SCHED_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DESCR)
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.MERCHANDISE_AMT, "######0.00"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ITEM_SETID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.INV_ITEM_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.QTY_VCHR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.STATISTIC_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.UNIT_OF_MEASURE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.UNIT_PRICE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SALETX_APPL_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.USETAX_APPL_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.FRGHT_PRORATE_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DSCNT_APPL_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.WTHD_SW)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TAX_CD_VAT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_RECOVERY_PCT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_RECV)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECEIVER_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECV_LN_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECV_SHIP_SEQ_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.MATCH_LINE_OPT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_MTHD_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TXN_CURRENCY_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BASE_CURRENCY)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CURRENCY_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SHIPTO_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SUT_BASE_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.TAX_CD_SUT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SUT_EXCPTN_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SUT_EXCPTN_CERTIF)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SUT_APPLICABILITY)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.WTHD_SETID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.WTHD_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_APPL_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_APPLICABILITY)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_TXN_TYPE_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.NATURE_OF_TXN1)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.NATURE_OF_TXN2)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_USE_ID)
            currentLine.Append("|")

            logger.Debug("BuildPeopleSoftLineRow exit")
            Return currentLine.ToString()
        End Function


        ''' <summary>
        ''' Build the first third that is included in the PeopleSoft output file for a record.
        ''' </summary>
        ''' <param name="currentPSRecord"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildPeopleSoftDistributionRow(ByRef currentPSRecord As PeopleSoftUploadBO) As String
            logger.Debug("BuildPeopleSoftDistributionRow entry: currentPSRecord.OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", currentPSRecord.VCHR_HDR_ROW_ID=" + currentPSRecord.VCHR_HDR_ROW_ID)
            Dim currentLine As New StringBuilder()
            currentLine.Append(currentPSRecord.VCHR_DIST_ROW_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DIST_BUSINESS_UNIT_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DIST_VOUCHER_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DIST_VOUCHER_LINE_NUM)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_LINE_NUM)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_GL)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ACCOUNT)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.STATISTIC_CODE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.STATISTIC_AMOUNT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.QTY_VCHR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.JRNL_LN_REF)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.OPEN_ITEM_STATUS)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_DESCR)
            currentLine.Append("|")
            currentLine.Append(Format(currentPSRecord.DIST_MERCHANDISE_AMT, "######0.00"))     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_PO)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_PO_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_LINE_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DISTRIB_SCHED_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PO_DIST_LINE_NUM)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_PC)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ACTIVITY_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ANALYSIS_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RESOURCE_TYPE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RESOURCE_CATEGORY)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RESOURCE_SUB_CAT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ASSET_FLG)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_AM)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.ASSET_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PROFILE_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.FREIGHT_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.SALETX_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.USETAX_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_TXN_TYPE_CD)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_INV_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_NONINV_AMT)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.BUSINESS_UNIT_RECV)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECEIVER_ID)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECV_LN_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECV_SHIP_SEQ_NBR)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.RECV_DIST_LINE_NUM)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.DEPTID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PRODUCT)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.PROEJCT_ID)     ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.AFFILIATE)
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.VAT_APORT_CNTRL)
            currentLine.Append("|")

            logger.Debug("BuildPeopleSoftDistributionRow exit")
            Return currentLine.ToString()
        End Function


        ''' <summary>
        ''' Build the first fourth that is included in the PeopleSoft output file for a record.
        ''' </summary>
        ''' <param name="currentPSRecord"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BuildPeopleSoftCurrencyRow(ByRef currentPSRecord As PeopleSoftUploadBO) As String
            logger.Debug("BuildPeopleSoftCurrencyRow entry: currentPSRecord.OrderHeaderID=" + currentPSRecord.OrderHeaderID.ToString() + ", currentPSRecord.VCHR_HDR_ROW_ID=" + currentPSRecord.VCHR_HDR_ROW_ID)
            Dim currentLine As New StringBuilder()
            currentLine.Append(currentPSRecord.VCHR_CURR_ROW_ID)            ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CURR_BUSINESS_UNIT_ID)       ' required
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CURR_VENDOR_CODE)            ' required
            currentLine.Append("|")
            currentLine.Append(currentPSRecord.CURR_BU_CODE())              ' required
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")
            currentLine.Append("|")

            logger.Debug("BuildPeopleSoftDistributionRow exit")
            Return currentLine.ToString()
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
                    Dim previousFreightOrderType As Boolean = False

                    ' Add each record in the result set to the output file
                    logger.Info("Begin processing the result set, adding each record to the PeopleSoft GL output file")
                    Dim recordCount As Integer = 0

                    For Each dataRow In results.Rows
                        ' Populate a new upload BO with the next result set
                        currentGLRecord = New GLBO(dataRow)

                        If previousBusinessUnitID <> currentGLRecord.BusinessUnitId Then
                            previousBusinessUnitID = currentGLRecord.BusinessUnitId
                            iGLRowNumber = 0
                            blnWriteHeader = True
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

                        logger.Info("Adding the line row for BusinessUnitID=" + currentGLRecord.BusinessUnitId.ToString() + " and PO# " + currentGLRecord.OrderHeaderID.ToString)

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
            logger.Debug("BuildPeopleSoftHeaderRow entry: currentGLRecord.OrderHeaderID=" + currentGLRecord.OrderHeaderID.ToString())
            Dim currentLine As New StringBuilder()
            currentLine.Append("H")                                                                         'Record Type
            currentLine.Append(currentGLRecord.BusinessUnitId.ToString.PadRight(5, " "))                    'Business Unit
            currentLine.Append("NEXT".PadRight(10, " "))                                                    'Journal ID
            currentLine.Append(CDate(currentGLRecord.JournalDate).ToString("MMddyyyy").PadRight(8, " "))    'Journal Date
            currentLine.Append("GLBOOK".PadRight(10, " "))                                                  'Ledger Group
            currentLine.Append("N")                                                                         'Reversal Code
            currentLine.Append(" ".PadRight(8, " "))                                                        'Reversal Date
            currentLine.Append("XLS")                                                                       'Source
            currentLine.Append(" ".PadRight(8, " "))                                                        'Transaction Reference Number
            currentLine.Append(CStr("IRMA POs for BU " & currentGLRecord.BusinessUnitId).PadRight(30, " ")) 'Journal Description
            currentLine.Append("USD")                                                                       'Currency Type
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

        Private Function BuildPeopleSoftGLDetailRow(ByRef currentGLRecord As GLBO) As String
            logger.Debug("BuildPeopleSoftHeaderRow entry: currentGLRecord.OrderHeaderID=" + currentGLRecord.OrderHeaderID.ToString())
            Dim currentLine As New StringBuilder()
            currentLine.Append("L")                                                                             'Record Type
            'task BR 11422 store to store transfer functionality
            If currentGLRecord.BusinessUnitId.ToString <> currentGLRecord.TransferBusinessUnitID.ToString _
                And currentGLRecord.TransferBusinessUnitID.ToString & "" <> "" Then
                currentLine.Append(currentGLRecord.TransferBusinessUnitID.ToString.PadRight(5, " "))            'Transfer Business Unit
            Else
                currentLine.Append(currentGLRecord.BusinessUnitId.ToString.PadRight(5, " "))                    'Business Unit
            End If
            currentLine.Append(iGLRowNumber.ToString.PadRight(9, " "))                                          'Journal Line
            currentLine.Append("ACTUAL".PadRight(10, " "))                                                     'Ledger
            currentLine.Append(currentGLRecord.AccountNumber.ToString.PadRight(6, " "))                         'Account
            'currentLine.Append(currentGLRecord.DepartmentId.ToString.PadRight(10, " "))                        'Department ID
            currentLine.Append(IIf(currentGLRecord.DepartmentId = 0, " ", currentGLRecord.DepartmentId).ToString.PadRight(10, " ")) 'Department ID
            currentLine.Append(currentGLRecord.ProductId.ToString.PadRight(6, " "))                             'Product
            currentLine.Append(" ".PadRight(15, " "))                                                           'Project ID
            currentLine.Append(" ".PadRight(5, " "))                                                            'Affiliate
            currentLine.Append(" ".PadRight(3, " "))                                                            'Statistics Code
            currentLine.Append("USD")                                                                           'Currency Type
            currentLine.Append(currentGLRecord.Amount.ToString.PadRight(28, " "))                               'Monetary Amount
            currentLine.Append(" ".PadRight(4, " "))                                                            'Blank Field
            currentLine.Append(" ".PadRight(10, " "))                                                           'Journal Line Reference
            currentLine.Append(currentGLRecord.OrderHeaderID.PadRight(30, " "))                                 'Line Description
            currentLine.Append("CRRNT")                                                                         'Rate Type
            currentLine.Append("1".PadRight(16, " "))                                                           'Rate Mult
            currentLine.Append(currentGLRecord.Amount.ToString.PadRight(16, " "))                               'Foreign Amount
            currentLine.Append("N")                                                                             'Movement Flag
            currentLine.Append(" ".PadRight(30, " "))                                                           'Open Item Key

            logger.Debug("BuildPeopleSoftHeaderRow exit")
            Return currentLine.ToString()
        End Function
    End Class
End Namespace

