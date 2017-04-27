Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.SendOrders.DataAccess
Imports WholeFoods.IRMA.FaxLog
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.SMTP
Imports WholeFoods.Utility
Imports System.Configuration
Imports System.IO
Imports System.Xml
Imports System.Text.RegularExpressions
Imports log4net

Namespace WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
    Public Class SendOrdersBO
        Private Shared gsSupportEmail As String
        Private Shared gsSMTPHost As String
        Private Shared gbProduction As Boolean
        Private Shared gsSystemFromEmailAddress As String
        Private Shared bOverrideEnvironmentCheck As Boolean
        Dim gbEXE_FTP As Boolean
        Dim gsEXE_FTPRemoteHost As String
        Dim gsEXE_FTPRemotePath As String
        Dim gsEXE_FTPRemoteUser As String
        Dim gsEXE_FTPRemotePassword As String
        Dim data_object As New Windows.Forms.DataObject
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO")
        Public Shared EDI_FILE_TYPE As String = "EDI"
        Public Shared ORD_FILE_TYPE As String = "ORD"
        Public Shared XML_FILE_TYPE As String = "XML"
        Public Shared HTML_FILE_TYPE As String = "HTML"
        Private Shared sRegion As String

        Enum XMLTagType
            BeginOnly = 1
            Data = 2
            EndOnly = 3
            EmptyTag = 4
        End Enum

#Region "Constructors"
        ''' <summary>
        ''' Blank constructor
        ''' </summary>
        ''' <remarks>Hostname, message body, mail recipients and subject must be set manually</remarks>
        Sub New()
            gsSupportEmail = CType(ConfigurationServices.AppSettings("SendOrdersSupportEmail"), String)
            gbProduction = CType(ConfigurationServices.AppSettings("isSendOrdersProduction"), Boolean)
            gsSystemFromEmailAddress = CType(ConfigurationServices.AppSettings("SendOrdersSystemEmail"), String)
            gsSMTPHost = CType(ConfigurationServices.AppSettings("SMTPHost"), String)
            FaxLog.Log.FaxLogLocation = ConfigurationServices.AppSettings("FaxLogLocation")
            FaxLog.Log.ExpirationThreshold = CInt(ConfigurationServices.AppSettings("FaxLogExpirationThreshold"))
            FaxLog.Log.RetentionPolicy = CInt(ConfigurationServices.AppSettings("FaxLogRetentionPolicy"))
            bOverrideEnvironmentCheck = CType(ConfigurationServices.AppSettings("OverrideEnvironmentCheck"), Boolean)

            If Environment.GetCommandLineArgs.Length >= 2 Then
                ' running from SendOrders.exe or other scheduled app. use command line arguments.
                sRegion = Environment.GetCommandLineArgs.GetValue(1).ToString()
            Else
                ' running from client. we wouldnt need region variable which was used for project jeannie. If that changes, edit here.
                sRegion = ""
            End If


        End Sub

#End Region

        Private log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private Shared Function removeDecimalTrailingZeros(ByVal value As String, Optional ByVal preservedDecimalLength As Integer = 0) As String
            ' This function assumes a string number is passed in, but does not check this condition.
            If value Is Nothing Then Return String.Empty
            value = value.TrimEnd
            If Not (value.EndsWith("0")) Then Return value
            ' Find decimal index.
            Dim lPntIndex As Integer
            lPntIndex = value.IndexOf(".")
            If lPntIndex = -1 Then Return value
            While value.EndsWith("0") And value.Length > (lPntIndex + 1 + preservedDecimalLength)
                value = value.Remove(value.Length - 1)
            End While
            ' If we've removed all decimal trailing zeros up to the whole number, we need to remove the decimal point.
            If value.EndsWith(".") Then value = value.Remove(value.Length - 1)
            Return value
        End Function

        Public Function SendAllOrders() As ArrayList

            Dim dbReport As ADODB.Connection
            Dim sCoverPage As String
            Dim sPhoneNum As String = ""
            Dim bFaxOrder As Boolean
            Dim Expected_Date As Date
            Dim sPrinter As String
            'Dim prnX As Printer
            Dim sExpectedDate As String
            Dim bElectronicTransfer As Boolean
            Dim sSenderAddress As String
            Dim sSQL As String
            Dim sSendString As String
            Dim lTransfer_SubTeam As Long
            Dim sErrMsg As String
            Dim bTimer As Boolean
            Dim sFTP_Addr As String
            Dim sFTP_Path As String
            Dim sFTP_User As String
            Dim sFTP_Password As String
            Dim sPS_Vendor_ID As String
            Dim sFTP_FileName As String
            Dim lPrevOrderHeaderID As Long
            Dim lRetryCount As Long
            Dim sNotes As String
            Dim bReturn_Order As Boolean
            Dim glOrderHeaderID As Integer
            Dim poheader As POHeaderBO
            Dim listOfItems As ArrayList
            Dim orderItem As ItemCatalog.OrderItem
            Dim myEnum As IEnumerator
            Dim sFilename As String
            Dim wfmfaxFromAlias As String
            Dim ftpStatus As Boolean
            Dim logText As ArrayList = New ArrayList
            Dim faxSubjectLine As String

            On Error GoTo me_err

            glOrderHeaderID = 0

            '-- Find out what orders need to be faxed
            gbProduction = CType(ConfigurationServices.AppSettings("isSendOrdersProduction"), Boolean)
            log.InfoFormat("Region: {0}", sRegion)
            log.Info("Scanning for orders to send")
            While SendOrdersDAO.GetNextOrder() > 0

                glOrderHeaderID = SendOrdersDAO.NextID
                log.Info("Processing Order #" & glOrderHeaderID)

                If glOrderHeaderID <> lPrevOrderHeaderID Then
                    lRetryCount = 0
                    lPrevOrderHeaderID = glOrderHeaderID
                Else
                    lRetryCount = lRetryCount + 1
                    If lRetryCount > 5 Then
                        SendOrdersDAO.UpdateOrderCancelSend(glOrderHeaderID)
                        SendMail(gsSupportEmail, "Order #" + glOrderHeaderID.ToString() + " Not Sent", "Order NOT sent - retry limit exceeded.  Order must be re-sent from the Order Information window ('Send Now' button) to retry automatic send again.")
                        log.Info("Order #" + glOrderHeaderID.ToString() + " Not Sent. Order NOT sent - retry limit exceeded.  Order must be re-sent from the Order Information window ('Send Now' button) to retry automatic send again.")
                        'GoTo EndOfLoop
                        Continue While
                    End If
                End If

                sSendString = ""

                log.Info("PO #" & glOrderHeaderID & " - Get PO Info")
                'logText.Add("PO #" & glOrderHeaderID & " - Get PO Info")

                ' OpenQuery(grsRecordset, "GetPOHeader " & glOrderHeaderID, gdbInventory)
                poheader = SendOrdersDAO.GetPOHeader(glOrderHeaderID)

                If (poheader.Transfer_SubTeam = 0) Then
                    lTransfer_SubTeam = -1
                Else
                    lTransfer_SubTeam = poheader.Transfer_SubTeam
                End If

                If poheader.OverrideTransmissionMethod And poheader.Fax_Order Then
                    poheader.Vendor_Fax = poheader.OverrideTransmissionTarget
                End If

                '-- Simply update SentDate if not faxed
                If (Not poheader.Fax_Order) Then
                    SendOrdersDAO.UpdateOrderSentDate(glOrderHeaderID)
                    'go to next po
                    Continue While

                ElseIf (Not poheader.Electronic_Transfer) Then

                    'TODO:  Modify this validation to match the validation regular expression
                    'on the OrderSend.vb form.
                    '-- See if its got a good phone number
                    sPhoneNum = ReturnNumbers(poheader.Vendor_Fax & "")
                    If Len(sPhoneNum) < 10 Then

                        SendOrdersDAO.UpdateOrderCancelSend(glOrderHeaderID)
                        SendMail(gsSupportEmail, "Order #" & glOrderHeaderID.ToString() & " Not Sent", "Fax not sent (Invalid Phone Number)")
                        log.Info("Order #" & glOrderHeaderID.ToString() & " Not Sent. Fax not sent (Invalid Phone Number)")
                        Continue While
                    End If

                    '-- Make sure they have a coversheet
                    If Trim(poheader.Cover_Page & "") = "" Then

                        SendOrdersDAO.UpdateOrderCancelSend(glOrderHeaderID)
                        SendMail(gsSupportEmail, "Order #" + glOrderHeaderID.ToString() + " Not Sent", " Fax not sent (No cover sheet)")

                        'GoTo EndOfLoop
                        Continue While
                    End If

                    'If Mid(sPhoneNum, 1, 3) <> "678" And Mid(sPhoneNum, 1, 3) <> "404" And Mid(sPhoneNum, 1, 3) <> "770" Then sPhoneNum = "1" & sPhoneNum '& ",,,," & FAX_LONG_DIS_CODE
                    sSendString = "^[D" & sPhoneNum & "^[NPO" & glOrderHeaderID & " " & poheader.vendor.CompanyName & "^[S" & poheader.Full_Name & "^]"
                    sCoverPage = poheader.Cover_Page & ""

                    If (poheader.Expected_Date = Nothing) Then
                        sExpectedDate = "ASAP"
                    Else
                        sExpectedDate = "ON " & Format(poheader.Expected_Date, "MM/dd/yyyy")
                    End If

                End If

                'set default FTP and fax/email instances if NOT in production
                If (Not gbProduction) Then
                    sPhoneNum = CType(ConfigurationServices.AppSettings("DEFAULT_FAX"), String)
                    poheader.FTP_Addr = CType(ConfigurationServices.AppSettings("DEFAULT_FTPRemoteHost"), String)
                    poheader.FTP_User = CType(ConfigurationServices.AppSettings("DEFAULT_FTPRemoteUser"), String)
                    poheader.FTP_Password = CType(ConfigurationServices.AppSettings("DEFAULT_FTPRemotePassword"), String)
                End If

                If (poheader.Fax_Order) Then

                    If poheader.Electronic_Transfer Then

                        If (poheader.FileType.ToUpper.Equals(ORD_FILE_TYPE)) Then
                            sFilename = CreateORDOrderFile(poheader)
                        Else
                            sFilename = CreateXMLOrderFile(poheader)
                        End If


                        If (sFilename <> Nothing) Then
                            '-- Push the order file

                            On Error Resume Next

                            ftpStatus = FTP_Order(sFilename, poheader.FTP_Addr, poheader.FTP_User, poheader.FTP_Password)

                            If (Not ftpStatus) Then
                                SendMessage(False, glOrderHeaderID, "Unable to FTP order.  Will re-try shortly.  Contact the Help Desk if the problem continues.")
                                SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, "Unable to FTP order.  Will re-try shortly.  Contact the Help Desk if the problem continues.")
                                SendOrdersDAO.UpdateOrderNotSent(glOrderHeaderID)
                                log.Info("Unable to FTP order.  Will re-try shortly.  Contact the Help Desk if the problem continues.")
                            Else
                                SendOrdersDAO.UpdateOrderSentDate(glOrderHeaderID)
                                log.Info("FTP order success.")
                            End If
                            'now delete file
                            Kill(My.Application.Info.DirectoryPath & "\" & sFilename)
                            On Error GoTo me_err
                        End If

                    Else
                        log.Info("PO #" & glOrderHeaderID & " - Send to Fax Server")
                        'logText.Add("PO #" & glOrderHeaderID & " - Send to Fax Server")
                        wfmfaxFromAlias = CType(ConfigurationServices.AppSettings("WFMFAXEMAIL"), String)
                        sFilename = createHTMLFromXSL(poheader)

                        'strip area code if 5124827278 -- note that central fax server cannot handle the area code
                        If (sPhoneNum.StartsWith("512")) Then
                            sPhoneNum = sPhoneNum.Substring(3, 7)
                        End If

                        If (gbProduction) Then
                            faxSubjectLine = "Attention: " & poheader.vendor.CompanyName & " *** Whole Foods Market Order *** Store: " & poheader.UserTitle & " *** #" & glOrderHeaderID
                        Else
                            faxSubjectLine = "TEST ORDER Attention: " & poheader.vendor.CompanyName & " *** Whole Foods Market Order *** Store: " & poheader.UserTitle & " *** #" & glOrderHeaderID
                        End If

                        log.Info("PO #" & glOrderHeaderID & " - Order HTML file created.")
                        'logText.Add("PO #" & glOrderHeaderID & " - Order HTML file created.")

                        SendMailWithAttachment("fax=/NUM=" & sPhoneNum & "@" & "fax.wholefoods.com", "Attention: " & poheader.vendor.CompanyName & " *** Whole Foods Market Order *** Store: " & poheader.UserTitle & " *** #" & glOrderHeaderID, "", sFilename, wfmfaxFromAlias, logText)

                        SendOrdersDAO.UpdateOrderSentToFaxDate(glOrderHeaderID)

                        FaxLog.Log.CreateEntry(glOrderHeaderID, sPhoneNum)

                        log.Info("PO #" & glOrderHeaderID & " - Order faxed successfully")
                        'logText.Add("PO #" & glOrderHeaderID & " - Order faxed successfully")

                        'now delete file
                        Kill(sFilename)

                    End If

                End If
            End While

            log.Info("Purging FaxLog Transmission History")
            FaxLog.Log.PurgeTransmissionHistory()

            Dim dt As New DataTable
            Dim dr As DataRow

            log.Info("Checking for Unconfirmed Fax Transmissions")
            dt = FaxLog.Log.GetUnconfirmedTransmissions()

            If dt.Rows.Count > 0 Then

                Dim sb As New Text.StringBuilder

                For Each dr In dt.Rows

                    sb.AppendLine("PO: " & dr.Item("PO").ToString & vbTab & _
                                  "Destination: " & dr.Item("Destination").ToString & vbTab & _
                                  "Timestamp: " & dr.Item("Timestamp").ToString & vbTab & _
                                  "Attempts: " & dr.Item("Attempts").ToString & vbTab & _
                                  "Status: " & dr.Item("Status").ToString)
                Next


                SendMail(gsSupportEmail, _
                        ConfigurationServices.AppSettings("errorSubject") & " - UNCONFIRMED FAX TRANSMISSIONS", _
                        "No response has been received from the fax server for the following transmissions: " & _
                        vbNewLine & vbNewLine & _
                        sb.ToString, _
                        bOverrideEnvironmentCheck)

            End If

me_exit:
            'Kill(My.Application.Info.DirectoryPath & "\*.XML")
            log.Info("Exiting...All queued orders have been processed...")
            Return logText
me_err:

            sErrMsg = "Order failed to transmit due to system error: " & CStr(Err.Number) & " - " & Err.Description & ".  This error message was also sent to IRS support."

            log.Error(sErrMsg)
            SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg)

            If glOrderHeaderID > 0 Then
                SendMessage(False, glOrderHeaderID, sErrMsg)
                SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg)
            End If

            GoTo me_exit

me_f_err:

            sErrMsg = "Order failed to transmit due to system error: " & CStr(Err.Number) & " - " & Err.Description & ".  This error message was also sent to IRS support."

            log.Error(sErrMsg)
            SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg)

            If glOrderHeaderID > 0 Then
                SendMessage(False, glOrderHeaderID, sErrMsg & "  Will re-try shortly.")
                SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg & "  Will re-try shortly.")

                'ExecuteQuery(gdbInventory, "UpdateOrderNotSent " & glOrderHeaderID)
                SendOrdersDAO.UpdateOrderNotSent(glOrderHeaderID)

                If Err.Number <> 0 Then
                    sErrMsg = "Error in UpdateOrderNotSent: " & CStr(Err.Number) & " - " & Err.Description
                    log.Error(sErrMsg)
                    Err.Clear()
                End If
            End If


me_db_err:

            sErrMsg = "Order failed to transmit due to system error: " & CStr(Err.Number) & " - " & Err.Description & ".  This error message was also sent to IRS support."

            log.Error(sErrMsg)
            SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg)

            If glOrderHeaderID > 0 Then
                SendMessage(False, glOrderHeaderID, sErrMsg & "  Will re-try shortly.")
                SendOwnerMessage(gsSupportEmail, "Order #" & glOrderHeaderID, sErrMsg & "  Will re-try shortly.")

                'ExecuteQuery(gdbInventory, "UpdateOrderNotSent " & glOrderHeaderID)
                SendOrdersDAO.UpdateOrderNotSent(glOrderHeaderID)

                If Err.Number <> 0 Then
                    sErrMsg = "Error in UpdateOrderNotSent: " & CStr(Err.Number) & " - " & Err.Description
                    log.Info(sErrMsg)
                    Err.Clear()
                End If
            End If

            GoTo me_exit

        End Function

        Private Function FTP_Order(ByRef sFileName As String, ByVal sFtpServer As String, ByVal sFtpUser As String, ByVal sFtpPassword As String) As Boolean 'add error check

            Dim ftp As FTPclient
            Dim fileInfo As FileInfo

            ' To fix the bug 5792. Deleting the file after FTP operation.
            Dim bFTPError As Boolean
            bFTPError = False


            If FileLen(sFileName) > 0 Then
                'FTP the file
                Try
                    fileInfo = New FileInfo(sFileName)
                    ftp = New FTPclient(sFtpServer, sFtpUser, sFtpPassword)
                    ftp.Upload(fileInfo)
                Catch ex As Exception
                    log.Error("Unable to FTP orders: " & ex.ToString, ex)
                    SendMail(gsSupportEmail, "FTP_Order", "Unable to FTP orders: " & ex.ToString)
                    bFTPError = True
                    Return False
                Finally
                End Try

                ' To fix the bug 5792. Deleting the file after FTP operation.
                If Not bFTPError Then
                    Kill(sFileName)
                End If
            Else
                Try
                    Kill(sFileName)
                Catch ex As Exception
                    'Swallow
                End Try
            End If
            Return True
        End Function



        'Public Sub LogError(ByRef sErrDesc As String)

        '    On Error Resume Next

        '    FileOpen(99, My.Application.Info.DirectoryPath & "\replenishment_errs.txt", OpenMode.Append, OpenAccess.Write, OpenShare.Shared)
        '    If Err.Number <> 0 Then
        '        SendMail(gsSupportEmail, "Send Orders" & ": LogError Failed", "Error opening " & My.Application.Info.DirectoryPath & "\replenishment_errs.txt - " & Err.Description & vbCrLf & "The error that could not be logged was " & sErrDesc, bOverrideProdCheck:=True)
        '    Else
        '        WriteLine(99, CStr(Now) & vbCrLf & sErrDesc)
        '        If Err.Number <> 0 Then
        '            SendMail(gsSupportEmail, "Send Orders" & ": LogError Failed", "Error writing to " & My.Application.Info.DirectoryPath & "\replenishment_errs.txt - " & Err.Description & vbCrLf & "The error that could not be logged was " & sErrDesc, bOverrideProdCheck:=True)
        '        End If
        '    End If

        '    FileClose(99)

        'End Sub

        Public Shared Sub SendMail(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String, Optional ByVal bOverrideProdCheck As Boolean = False)

            Dim smtp As New SMTP(gsSMTPHost) 'use Central's SMPT host
            'Do not send emails if this is not the production environment
            If (Not gbProduction) And Not bOverrideProdCheck Then Exit Sub

            Try
                'Dim mmsg As New System.Net.Mail.MailMessage(gsSystemFromEmailAddress, sRecipient, sSubject, sMessage)
                'SendMailClient.SendAsync(mmsg, "SendMail " & sRecipient & " " & sSubject & " " & sMessage)
                smtp.send(sMessage, sRecipient, Nothing, gsSystemFromEmailAddress, sSubject)

            Catch ex As Exception
                Logger.LogError(ex.ToString, CLASSTYPE)
            End Try

        End Sub

        Public Shared Sub SendMailWithAttachment(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String, ByVal sFileName As String, ByVal wfmFaxEmail As String, ByRef logText As ArrayList, Optional ByVal bOverrideProdCheck As Boolean = False)

            If gsSMTPHost = Nothing Then
                Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                Dim mailSettings As System.Net.Configuration.MailSettingsSectionGroup = System.Net.Configuration.NetSectionGroup.GetSectionGroup(config).MailSettings
                gsSMTPHost = mailSettings.Smtp.Network.Host
            End If

            Dim smtp As New SMTP(gsSMTPHost) 'use Central's SMPT host
            logText.Add("SMTPHost = " & gsSMTPHost)
            Try
                'Dim mmsg As New System.Net.Mail.MailMessage(gsSystemFromEmailAddress, sRecipient, sSubject, sMessage)
                'SendMailClient.SendAsync(mmsg, "SendMail " & sRecipient & " " & sSubject & " " & sMessage)
                Logger.LogInfo("Recipient: " & sRecipient & " From: " & wfmFaxEmail, CLASSTYPE)
                smtp.sendWithAttachment(sMessage, sRecipient, Nothing, wfmFaxEmail, sSubject, sFileName)

            Catch ex As Exception
                Logger.LogError(ex.ToString, CLASSTYPE)
                logText.Add(ex.ToString)
            End Try

        End Sub
        Shared Sub SendOwnerMessage(ByVal sRecipient As String, ByVal sSubject As String, ByVal sMessage As String)

            If Trim(sRecipient) = "" Then Exit Sub

            SendMail(sRecipient, sSubject, sMessage)

        End Sub
        Shared Function SendMessage(ByVal bSuccessful As Boolean, ByVal lPurchaseOrder As Long, ByVal sMessage As String) As Boolean

            Dim emailMap As Hashtable
            Dim sRecipients As String
            Dim sCompanyName As String
            Dim sStatus As String

            On Error GoTo me_err

            'OpenQuery(rs, "GetOrderEmail " & lPurchaseOrder, gdbInventory)
            emailMap = SendOrdersDAO.GetOrderEmail(lPurchaseOrder)

            If Not (emailMap.Count = 0) Then

                sCompanyName = Replace(CStr(emailMap.Item(SendOrdersDAO.COMPANY)), ",", " ")
                sRecipients = Trim("" & CStr(emailMap.Item(SendOrdersDAO.BUYER_EMAIL)))
                Logger.LogInfo("Buyer Email: " & CStr(emailMap.Item(SendOrdersDAO.BUYER_EMAIL)), CLASSTYPE)
                If Trim("" & CStr(emailMap.Item(SendOrdersDAO.TL_EMAIL))) <> "" Then
                    Logger.LogInfo("TL Email: " & CStr(emailMap.Item(SendOrdersDAO.TL_EMAIL)), CLASSTYPE)
                    If sRecipients <> "" Then
                        sRecipients = sRecipients & ";" & CStr(emailMap.Item(SendOrdersDAO.TL_EMAIL))
                    Else
                        sRecipients = CStr(emailMap.Item(SendOrdersDAO.TL_EMAIL))
                    End If
                End If

                If (bSuccessful) Then
                    sStatus = "Successful"
                Else
                    sStatus = "Failed"
                End If

                If sRecipients <> "" Then
                    SendMail(sRecipients, "PO #" & lPurchaseOrder & " " & sCompanyName & " (" & sStatus & ")", Replace(sMessage, ",", " "), True)

                    SendMessage = True
                Else
                    SendMessage = False
                End If
            Else
                SendMessage = False
                Logger.LogInfo("No valid email address found associated with this order. [ " & lPurchaseOrder.ToString() & " ]", CLASSTYPE)
            End If

me_exit:

            Exit Function

me_err:
            Logger.LogError("SendMessage failed: " & Err.Number & " - " & Err.Description & " for PO=" & lPurchaseOrder & " Message=" & sMessage, CLASSTYPE)
            Resume me_exit

        End Function
        Function ReturnNumbers(ByVal sString As String) As String

            Dim iLoop As Integer
            Dim returnNumber As String = ""
            For iLoop = 1 To Len(sString)
                If Mid(sString, iLoop, 1) >= "0" And Mid(sString, iLoop, 1) <= "9" Then
                    returnNumber = returnNumber + Mid(sString, iLoop, 1)
                End If
            Next iLoop
            Return returnNumber
        End Function
        Private Sub CreateFTPOrderFile(ByVal bFaxOrder As Boolean, ByVal poheader As POHeaderBO)

            Dim lErrNum As Long
            Dim sErrDesc As String = String.Empty
            Dim iLoop As Integer
            Dim sItemErrors As String = String.Empty
            Dim sAttr() As String = Nothing
            Dim sFileExt As String
            Dim sFileName As String
            Dim bORD As Boolean
            Dim bXML As Boolean
            Dim bANS As Boolean
            Dim dDate As Date
            Dim dTime As Date
            Dim oFileStream As System.IO.FileStream
            Dim sStreamWrtr As System.IO.StreamWriter
            Dim tempStr As String = ""
            Dim poDetails As ArrayList = Nothing
            Dim orderItem As ItemCatalog.OrderItem = Nothing
            Dim ienum As IEnumerator

            'frmReplenishment.StatusBar.SimpleText = "PO #" & glOrderHeaderID & " - " & sPS_Vendor_ID & " File"
            'frmReplenishment.StatusBar.Refresh()

            Select Case poheader.PeopleSoftNumber
                Case "00043937", "0000073378"
                    sFileExt = ".ORD"
                    bORD = True
                Case "0000080131"
                    sFileExt = ".XML"
                    bXML = True
                    bANS = True
                Case Else
                    sFileExt = ".XML"
                    bXML = True
            End Select

            If poheader.PeopleSoftNumber = "0000080131" Then
                sFileName = "po_" & poheader.OrderHeader_ID & sFileExt
            Else
                sFileName = poheader.OrderHeader_ID & sFileExt
            End If

            On Error Resume Next
            'Kill(App.Path & "\" & sFileName)
            Kill(My.Application.Info.DirectoryPath & "\" & sFileName)
            On Error GoTo me_err

            'FileOpen(1, sPathFileName, OpenMode.Output)
            'Open App.Path & "\" & sFileName For Binary As #1

            oFileStream = New System.IO.FileStream(sFileName, System.IO.FileMode.Create)
            'instantiate streamwriter
            sStreamWrtr = New StreamWriter(oFileStream)

            On Error GoTo me_f_err

            If bORD Then
                'OpenQuery(grsRecordset, "GetUNFIOrder " & glOrderHeaderID, gdbInventory)
                poheader = SendOrdersDAO.GetUNFIOrder(poheader)
            Else
                'OpenQuery(grsRecordset, "GetANSOrderHeader " & glOrderHeaderID, gdbInventory)
                poheader = SendOrdersDAO.GetANSOrderHeader(CInt(poheader.OrderHeader_ID))
            End If

            iLoop = 0

            If (Not poheader Is Nothing) Then
                If bORD Then

                    'Put #1, LOF(1) + 1, "#" & Right$("00000" & Trim(Str(glOrderHeaderID)), 5) & String$(29, " ") & vbLf
                    tempStr = "#" & Right$("00000" & Trim(Str(poheader.OrderHeader_ID)), 5) & New String(CChar(" "), 29) & vbLf
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    If poheader.UNFIStore <> Nothing Then 'IsNull(grsRecordset!UNFI_Store) Then
                        bFaxOrder = False
                        SendMessage(False, poheader.OrderHeader_ID, "Order failed to transmit due to receiving location not having a UNFI store number assosciated with it.  Please Contact MIS.")
                        SendOwnerMessage(gsSupportEmail, poheader.PeopleSoftNumber & " Order #" & poheader.OrderHeader_ID, "Order failed to transmit due to receiving location not having a UNFI store number assosciated with it.  Please Contact MIS.")
                    Else
                        'Put #1, LOF(1) + 1, "C" & Left$(grsRecordset!UNFI_Store & String$(12, " "), 12) & String$(6, " ") & "0101CH" & String$(10, " ") & vbLf
                        tempStr = "C" & Left$(poheader.UNFIStore & New String(CChar(" "), 12), 12) & New String(CChar(" "), 6) & "0101CH" & New String(CChar(" "), 10) & vbLf
                        'oFileStream.Write(tempStr, 0, tempStr.Length)
                        sStreamWrtr.WriteLine(tempStr)
                        'Put #1, LOF(1) + 1, "J" & Left$(Trim(Str(glOrderHeaderID)) & String$(10, " "), 10) & String$(8, " ") & "0101" & String$(12, " ") & vbLf
                        tempStr = "J" & Left$(Trim(Str(poheader.OrderHeader_ID)) & New String(CChar(" "), 10), 10) & New String(CChar(" "), 8) & "0101" & New String(CChar(" "), 12) & vbLf
                        'oFileStream.Write(tempStr, 0, tempStr.Length)
                        sStreamWrtr.WriteLine(tempStr)
                    End If
                Else
                    'Put the XML version info
                    'Put #1, LOF(1) + 1, "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & vbCrLf
                    tempStr = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & vbCrLf
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    'Put the order header info
                    ReDim sAttr(1)
                    sAttr(0) = "orderDate=""" & Format(poheader.OrderDate, "YYYY-MM-DD") & """"
                    sAttr(1) = "id=""" & poheader.OrderHeader_ID & """"
                    'Put #1, LOF(1) + 1, XMLElementString(0, "purchaseOrder", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(0, "purchaseOrder", XMLTagType.BeginOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "referenceNumber", Data, sAttr(), grsRecordset!referenceNumber)
                    tempStr = XMLElementString(1, "referenceNumber", XMLTagType.Data, sAttr, CStr(poheader.OrderHeader_ID))
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    Call SendOrdersDAO.SystemDateTime(dDate, dTime)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "shipdate", Data, sAttr(), Format(dDate, "YYYY-MM-DD"))
                    tempStr = XMLElementString(1, "shipdate", XMLTagType.Data, sAttr, Format(dDate, "YYYY-MM-DD"))
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    If bANS Then
                        'Put #1, LOF(1) + 1, XMLElementString(1, "shipdatequalifier", Data, sAttr(), 0)
                        tempStr = XMLElementString(1, "shipdatequalifier", XMLTagType.Data, sAttr, CStr(0))
                        'oFileStream.Write(tempStr, 0, tempStr.Length)
                        sStreamWrtr.WriteLine(tempStr)
                    End If

                    'Vendor
                    sAttr(0) = "country=""" & poheader.vendor.Country & """"
                    'Put #1, LOF(1) + 1, XMLElementString(1, "vendor", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "vendor", XMLTagType.BeginOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "name", Data, sAttr(), grsRecordset!VendName)
                    tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, poheader.vendor.CompanyName)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "address", Data, sAttr(), grsRecordset!VendAddress)
                    tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, poheader.vendor.AddressLine1)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "city", Data, sAttr(), grsRecordset!VendCity)
                    tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, poheader.vendor.City)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "state", Data, sAttr(), grsRecordset!VendState)
                    tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, poheader.vendor.State)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "zip", Data, sAttr(), grsRecordset!VendZip)
                    tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, poheader.vendor.ZipCode)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "vendor", EndOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "vendor", XMLTagType.EndOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    'billTo
                    sAttr(0) = "country=""" & poheader.vendor.Country & """"
                    'Put #1, LOF(1) + 1, XMLElementString(1, "billto", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "billto", XMLTagType.BeginOnly, sAttr, "")
                    ' oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "name", Data, sAttr(), grsRecordset!billToName)
                    tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, poheader.purchase_vendor.CompanyName)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "address", Data, sAttr(), grsRecordset!billToAddress)
                    tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, poheader.purchase_vendor.AddressLine1)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "city", Data, sAttr(), grsRecordset!billToCity)
                    tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, poheader.purchase_vendor.City)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "state", Data, sAttr(), grsRecordset!billToState)
                    tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, poheader.purchase_vendor.State)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "zip", Data, sAttr(), grsRecordset!billToZip)
                    tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, poheader.purchase_vendor.ZipCode)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "billto", EndOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "billto", XMLTagType.EndOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    'shipTo
                    sAttr(0) = "country=""" & poheader.vendor.Country & """"
                    'Put #1, LOF(1) + 1, XMLElementString(1, "shipto", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "shipto", XMLTagType.BeginOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "name", Data, sAttr(), grsRecordset!shipToName)
                    tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, poheader.receiving_vendor.CompanyName)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "address", Data, sAttr(), grsRecordset!shipToAddress)
                    tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, poheader.receiving_vendor.AddressLine1)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "city", Data, sAttr(), grsRecordset!shipToCity)
                    tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, poheader.receiving_vendor.City)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "state", Data, sAttr(), grsRecordset!shipToState)
                    tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, poheader.receiving_vendor.State)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "zip", Data, sAttr(), grsRecordset!shipToZip)
                    tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, poheader.receiving_vendor.ZipCode)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "shipto", EndOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "shipto", XMLTagType.EndOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    'buyer
                    sAttr(0) = "country=""" & poheader.vendor.Country & """"
                    'Put #1, LOF(1) + 1, XMLElementString(1, "buyer", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "buyer", XMLTagType.BeginOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "name", Data, sAttr(), grsRecordset!billToName)
                    tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, poheader.purchase_vendor.CompanyName)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "address", Data, sAttr(), grsRecordset!billToAddress)
                    tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, poheader.purchase_vendor.AddressLine1)
                    ' oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "city", Data, sAttr(), grsRecordset!billToCity)
                    tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, poheader.purchase_vendor.City)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "state", Data, sAttr(), grsRecordset!billToState)
                    tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, poheader.purchase_vendor.State)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(2, "zip", Data, sAttr(), grsRecordset!billToZip)
                    tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, poheader.purchase_vendor.ZipCode)
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "buyer", EndOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "buyer", XMLTagType.EndOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "comment", Data, sAttr(), grsRecordset!comment & "")
                    tempStr = XMLElementString(1, "comment", XMLTagType.Data, sAttr, poheader.PODescription & "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(1, "customer", Data, sAttr(), grsRecordset!customer)
                    tempStr = XMLElementString(1, "customer", XMLTagType.Data, sAttr, CStr(poheader.BusinessUnit))
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                End If
                If Not bANS Then
                    'Put #1, LOF(1) + 1, XMLElementString(1, "credit", Data, sAttr(), IIf(grsRecordset!credit, "true", "false"))
                    If (poheader.Return_Order) Then
                        tempStr = XMLElementString(1, "credit", XMLTagType.Data, sAttr, "true")
                    Else
                        tempStr = XMLElementString(1, "credit", XMLTagType.Data, sAttr, "false")
                    End If
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)

                    'Items
                    'Put #1, LOF(1) + 1, XMLElementString(1, "items", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(1, "items", XMLTagType.BeginOnly, sAttr, "")
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                End If
            Else
                bFaxOrder = False
                SendMessage(False, poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
                SendOwnerMessage(gsSupportEmail, poheader.PeopleSoftNumber & " Order #" & poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
            End If

            If bXML Then
                'OpenQuery(grsRecordset, "GetANSOrderItems " & glOrderHeaderID, gdbInventory)
                poDetails = SendOrdersDAO.GetANSOrderItems(CInt(poheader.OrderHeader_ID))
                If poDetails.Count = 0 Then
                    bFaxOrder = False
                    SendMessage(False, poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
                    SendOwnerMessage(gsSupportEmail, poheader.PeopleSoftNumber & " Order #" & poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
                End If
            End If
            If (bORD) Then
                poDetails = poheader.PODetailsList
            End If
            ienum = poDetails.GetEnumerator()
            While (ienum.MoveNext) And bFaxOrder
                orderItem = CType(ienum.Current, ItemCatalog.OrderItem)
                iLoop = iLoop + 1

                If bORD Then
                    If orderItem.QuantityOrdered > 9999 Or orderItem.QuantityOrdered <> Math.Round(orderItem.QuantityOrdered, 0) Then
                        'Report it later, skip and send the order anyway
                        sItemErrors = sItemErrors & "Line # " & iLoop & ": Ordered quantity for an item cannot be greater than 9999 or a fraction." & vbCrLf
                        Continue While
                    End If

                    Select Case orderItem.QuantityUnit
                        Case 120, 112 'Bulk - case, box, shipper
                            If orderItem.Identifier & "" = "" Then
                                'Report it later, skip and send the order anyway
                                sItemErrors = sItemErrors & "Line # " & iLoop & ": has no non-scale UPC." & vbCrLf
                                Continue While
                            Else
                                'Put #1, LOF(1) + 1, "P" & _
                                '                   Right$(String(12, "0") & grsRecordset!Identifier, 12) & _
                                '                  Format$(grsRecordset!QuantityOrdered, "0000") & _
                                '                 "120101" & String$(12, " ") & vbLf
                                tempStr = "P" & _
                                            Right$(New String(CChar("0"), 12) & orderItem.Identifier, 12) & _
                                            Format$(orderItem.QuantityOrdered, "0000") & _
                                            "120101" & New String(CChar(" "), 12) & vbLf
                                sStreamWrtr.WriteLine(tempStr)
                            End If
                        Case Else 'Single
                            If Len(orderItem.OrderItem_ID) < 5 Then
                                'Report it later, skip and send the order anyway
                                sItemErrors = sItemErrors & "Line # " & iLoop & ": Vendor ID required to order in units." & vbCrLf
                                Continue While
                            Else
                                'Put #1, LOF(1) + 1, "P" & _
                                '                   Left$(grsRecordset!Item_ID & String(12, " "), 12) & _
                                '                  Format$(grsRecordset!QuantityOrdered, "0000") & _
                                '                 "050101" & String$(12, " ") & vbLf
                                tempStr = "P" & _
                                            Left$(orderItem.OrderItem_ID & New String(CChar(" "), 12), 12) & _
                                            Format$(orderItem.QuantityOrdered, "0000") & _
                                            "050101" & New String(CChar(" "), 12) & vbLf
                                sStreamWrtr.WriteLine(tempStr)
                            End If
                    End Select
                Else
                    ReDim sAttr(0)
                    sAttr(0) = "vendorPartNum=""" & orderItem.OrderItem_ID & """"
                    'Put #1, LOF(1) + 1, XMLElementString(2, "item", BeginOnly, sAttr(), "")
                    tempStr = XMLElementString(2, "item", XMLTagType.Data, sAttr, "")
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "WFMSKU", Data, sAttr(), grsRecordset!WFMSKU)
                    tempStr = XMLElementString(3, "WFMSKU", XMLTagType.Data, sAttr, orderItem.Identifier)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "posDept", Data, sAttr(), grsRecordset!posDept)
                    tempStr = XMLElementString(3, "posDept", XMLTagType.Data, sAttr, CStr(orderItem.SubTeam_No))
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "productName", Data, sAttr(), grsRecordset!ProductName)
                    tempStr = XMLElementString(3, "productName", XMLTagType.Data, sAttr, orderItem.Item_Description)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "casePack", Data, sAttr(), grsRecordset!casePack)
                    tempStr = XMLElementString(3, "casePack", XMLTagType.Data, sAttr, CStr(orderItem.Package_Desc1))
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "packSize", Data, sAttr(), grsRecordset!packSize)
                    tempStr = XMLElementString(3, "UOM", XMLTagType.Data, sAttr, CStr(orderItem.Package_Desc2))
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "UOM", Data, sAttr(), grsRecordset!UOM)
                    tempStr = XMLElementString(3, "UOM", XMLTagType.Data, sAttr, orderItem.Package_Unit_Abbr)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "USPrice", Data, sAttr(), grsRecordset!USPrice)
                    tempStr = XMLElementString(3, "USPrice", XMLTagType.Data, sAttr, CStr(orderItem.Cost))
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, XMLElementString(3, "quantity", Data, sAttr(), grsRecordset!quantity)
                    tempStr = XMLElementString(3, "quantity", XMLTagType.Data, sAttr, CStr(orderItem.QuantityOrdered))
                    sStreamWrtr.WriteLine(tempStr)
                    If Not bANS Then
                        'Put #1, LOF(1) + 1, XMLElementString(3, "comment", Data, sAttr(), grsRecordset!comment & "")
                        tempStr = XMLElementString(3, "comment", XMLTagType.Data, sAttr, orderItem.Comments & "")
                        sStreamWrtr.WriteLine(tempStr)
                        'Put #1, LOF(1) + 1, XMLElementString(2, "item", EndOnly, sAttr(), "")
                        tempStr = XMLElementString(2, "item", XMLTagType.EndOnly, sAttr, "")
                        sStreamWrtr.WriteLine(tempStr)
                    End If
                    'next_item:
                    'grsRecordset.MoveNext()
                End If
            End While

            'grsRecordset.Close()

            If bXML And bFaxOrder Then
                'Add end tags
                'Put #1, LOF(1) + 1, XMLElementString(1, "items", EndOnly, sAttr(), "")
                tempStr = XMLElementString(1, "items", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)
                'Put #1, LOF(1) + 1, XMLElementString(0, "purchaseOrder", EndOnly, sAttr(), "")
                tempStr = XMLElementString(0, "purchaseOrder", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)
            End If

            If Len(sItemErrors) > 0 Then
                SendMessage(True, poheader.OrderHeader_ID, "The following items were skipped due to errors:" & vbCrLf & sItemErrors)
                sItemErrors = ""
            End If

            If bORD And bFaxOrder Then
                'Put #1, LOF(1) + 1, "T001" & Format$(iLoop, "00000") & String$(26, " ") & vbLf
                tempStr = "T001" & Format$(iLoop, "00000") & New String(CChar(" "), 26) & vbLf
                sStreamWrtr.WriteLine(tempStr)
            End If

            On Error GoTo me_err

me_exit:
            Exit Sub

me_err:
            lErrNum = Err.Number
            sErrDesc = Err.Description

            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)

me_f_err:

            lErrNum = Err.Number
            sErrDesc = Err.Description


            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)

        End Sub
        Private Function CreateORDOrderFile(ByVal poheader As POHeaderBO) As String

            Dim lErrNum As Long
            Dim sErrDesc As String = String.Empty
            Dim iLoop As Integer
            Dim sItemErrors As String = String.Empty
            Dim sAttr() As String = Nothing
            Dim sFileName As String = String.Empty
            Dim sPath As String = String.Format(My.Application.Info.DirectoryPath & "\{0}\", sRegion)
            Dim bORD As Boolean
            Dim bXML As Boolean
            Dim bANS As Boolean
            Dim dDate As Date
            Dim dTime As Date
            Dim oFileStream As System.IO.FileStream
            Dim sStreamWrtr As System.IO.StreamWriter
            Dim tempStr As String = String.Empty
            Dim poDetails As ArrayList = Nothing
            Dim orderItem As ItemCatalog.OrderItem = Nothing
            Dim ienum As IEnumerator = Nothing

            sFileName = "po_" & poheader.OrderHeader_ID & ".ORD"


            On Error Resume Next
            'Kill(App.Path & "\" & sFileName)
            Kill(String.Format(sPath & sFileName, sRegion))
            On Error GoTo me_err

            'FileOpen(1, sPathFileName, OpenMode.Output)
            'Open App.Path & "\" & sFileName For Binary As #1

            oFileStream = New System.IO.FileStream(sPath & sFileName, System.IO.FileMode.Create)
            'instantiate streamwriter
            sStreamWrtr = New StreamWriter(oFileStream)

            On Error GoTo me_f_err

            poheader = SendOrdersDAO.GetUNFIOrder(poheader)

            iLoop = 0

            If (Not poheader Is Nothing) Then

                'Put #1, LOF(1) + 1, "#" & Right$("00000" & Trim(Str(glOrderHeaderID)), 5) & String$(29, " ") & vbLf
                tempStr = "#" & Right$("00000" & Trim(Str(poheader.OrderHeader_ID)), 5) & New String(CChar(" "), 29) & vbLf
                'oFileStream.Write(tempStr, 0, tempStr.Length)
                sStreamWrtr.WriteLine(tempStr)

                If poheader.UNFIStore <> Nothing Then 'IsNull(grsRecordset!UNFI_Store) Then
                    SendMessage(False, poheader.OrderHeader_ID, "Order failed to transmit due to receiving location not having a UNFI store number assosciated with it.  Please Contact MIS.")
                    SendOwnerMessage(gsSupportEmail, poheader.PeopleSoftNumber & " Order #" & poheader.OrderHeader_ID, "Order failed to transmit due to receiving location not having a UNFI store number assosciated with it.  Please Contact MIS.")
                    Kill(sPath & sFileName)
                    sFileName = Nothing
                    Return sFileName
                Else
                    'Put #1, LOF(1) + 1, "C" & Left$(grsRecordset!UNFI_Store & String$(12, " "), 12) & String$(6, " ") & "0101CH" & String$(10, " ") & vbLf
                    tempStr = "C" & Left$(poheader.UNFIStore & New String(CChar(" "), 12), 12) & New String(CChar(" "), 6) & "0101CH" & New String(CChar(" "), 10) & vbLf
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                    'Put #1, LOF(1) + 1, "J" & Left$(Trim(Str(glOrderHeaderID)) & String$(10, " "), 10) & String$(8, " ") & "0101" & String$(12, " ") & vbLf
                    tempStr = "J" & Left$(Trim(Str(poheader.OrderHeader_ID)) & New String(CChar(" "), 10), 10) & New String(CChar(" "), 8) & "0101" & New String(CChar(" "), 12) & vbLf
                    'oFileStream.Write(tempStr, 0, tempStr.Length)
                    sStreamWrtr.WriteLine(tempStr)
                End If


                poDetails = poheader.PODetailsList
                ienum = poDetails.GetEnumerator()

                While (ienum.MoveNext)
                    orderItem = CType(ienum.Current, ItemCatalog.OrderItem)
                    iLoop = iLoop + 1

                    If orderItem.QuantityOrdered > 9999 Or orderItem.QuantityOrdered <> Math.Round(orderItem.QuantityOrdered, 0) Then
                        'Report it later, skip and send the order anyway
                        sItemErrors = sItemErrors & "Line # " & iLoop & ": Ordered quantity for an item cannot be greater than 9999 or a fraction." & vbCrLf
                        Continue While
                    End If

                    Select Case orderItem.QuantityUnit
                        Case 120, 112 'Bulk - case, box, shipper
                            If orderItem.Identifier & "" = "" Then
                                'Report it later, skip and send the order anyway
                                sItemErrors = sItemErrors & "Line # " & iLoop & ": has no non-scale UPC." & vbCrLf
                                Continue While
                            Else
                                'Put #1, LOF(1) + 1, "P" & _
                                '                   Right$(String(12, "0") & grsRecordset!Identifier, 12) & _
                                '                  Format$(grsRecordset!QuantityOrdered, "0000") & _
                                '                 "120101" & String$(12, " ") & vbLf
                                tempStr = "P" & _
                                            Right$(New String(CChar("0"), 12) & orderItem.Identifier, 12) & _
                                            Format$(orderItem.QuantityOrdered, "0000") & _
                                            "120101" & New String(CChar(" "), 12) & vbLf
                                sStreamWrtr.WriteLine(tempStr)
                            End If
                        Case Else 'Single
                            If Len(orderItem.OrderItem_ID) < 5 Then
                                'Report it later, skip and send the order anyway
                                sItemErrors = sItemErrors & "Line # " & iLoop & ": Vendor ID required to order in units." & vbCrLf
                                Continue While
                            Else
                                'Put #1, LOF(1) + 1, "P" & _
                                '                   Left$(grsRecordset!Item_ID & String(12, " "), 12) & _
                                '                  Format$(grsRecordset!QuantityOrdered, "0000") & _
                                '                 "050101" & String$(12, " ") & vbLf
                                tempStr = "P" & _
                                            Left$(orderItem.OrderItem_ID & New String(CChar(" "), 12), 12) & _
                                            Format$(orderItem.QuantityOrdered, "0000") & _
                                            "050101" & New String(CChar(" "), 12) & vbLf
                                sStreamWrtr.WriteLine(tempStr)
                            End If
                    End Select
                End While

                If Len(sItemErrors) > 0 Then
                    SendMessage(True, poheader.OrderHeader_ID, "The following items were skipped due to errors:" & vbCrLf & sItemErrors)
                    sItemErrors = ""
                End If


                'Put #1, LOF(1) + 1, "T001" & Format$(iLoop, "00000") & String$(26, " ") & vbLf
                tempStr = "T001" & Format$(iLoop, "00000") & New String(CChar(" "), 26) & vbLf
                sStreamWrtr.WriteLine(tempStr)
            End If

            sStreamWrtr.Close()

            On Error GoTo me_err

me_exit:
            Return sPath & sFileName

me_err:
            lErrNum = Err.Number
            sErrDesc = Err.Description

            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)
            Return sPath & sFileName
me_f_err:
            lErrNum = Err.Number
            sErrDesc = Err.Description


            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)
            Return sPath & sFileName

        End Function

        Private Shared Function CreateXMLOrderFile(ByVal poheader As POHeaderBO) As String

            Dim lErrNum As Long
            Dim sErrDesc As String
            Dim iLoop As Integer
            Dim sItemErrors As String = Nothing
            Dim sAttr() As String
            Dim sFileExt As String
            Dim sFileName As String
            Dim sPath As String = String.Format(My.Application.Info.DirectoryPath & "\{0}\", sRegion)
            Dim bORD As Boolean
            Dim bXML As Boolean
            Dim bANS As Boolean
            Dim dDate As Date
            Dim dTime As Date
            Dim oFileStream As System.IO.FileStream = Nothing
            Dim sStreamWrtr As System.IO.StreamWriter = Nothing
            Dim tempStr As String = ""
            Dim poDetails As ArrayList = Nothing
            Dim orderItem As ItemCatalog.OrderItem = Nothing
            Dim ienum As IEnumerator = Nothing
            Dim sFooterMsg As String
            Dim sTransferToSubteamName As String
            Dim sCreatedByName As String
            Dim sExpectedDate As Date
            Dim sVendPhone As String
            Dim sVendFax As String
            Dim sPLPhone As String
            Dim sPLFax As String
            Dim sRLPhone As String
            Dim sRLFax As String
            Dim region As String

            sFooterMsg = ConfigurationServices.AppSettings("SendOrdersFooterMsg")
            ' Restrict to 315 chars.
            If sFooterMsg.Length > 315 Then sFooterMsg = sFooterMsg.Substring(0, 315) & " [*Message Truncated*]"

            region = ConfigurationServices.AppSettings("Region")

            sFileName = sPath & poheader.OrderHeader_ID & ".XML"

            On Error Resume Next
            Kill(sFileName)
            On Error GoTo me_err

            oFileStream = New System.IO.FileStream(sFileName, System.IO.FileMode.Create)
            'instantiate streamwriter
            sStreamWrtr = New StreamWriter(oFileStream)

            On Error GoTo me_f_err

            ' Be(careful) : the() 'poheader' object was originally populated in 'SendAllOrders' function by the
            ''GetPOHeader' stored proc and it will be reset below to the results of the 'GetANSOrderHeader' stored proc,
            'which get less information.

            'Capture/preserve phone & fax (for vendor/purchaser/shipto), expected date, dest subteam, creator name.
            sExpectedDate = poheader.Expected_Date
            sTransferToSubteamName = poheader.TransferToSubTeamName
            sCreatedByName = poheader.Full_Name
            sVendPhone = poheader.vendor.Phone
            sVendFax = poheader.Vendor_Fax
            sPLPhone = poheader.purchase_vendor.Phone
            sPLFax = poheader.Purchase_Vendor_Fax
            sRLPhone = poheader.receiving_vendor.Phone
            sRLFax = poheader.Receiving_Vendor_Fax

            poheader = SendOrdersDAO.GetANSOrderHeader(CInt(poheader.OrderHeader_ID))

            iLoop = 0

            If (Not poheader Is Nothing) Then

                ' Note about dates:  The two mixed format regions (MW, PN) have been emailing all orders with US date format for years now, so
                ' changing the date format to be based on the user's selected culture may produce unintended side-effects and confusion for vendors.  
                ' Instead, a check will be made to determine if the region is UK (EU), and only then will the date format be changed.

                'Put the XML version info
                tempStr = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & vbCrLf
                sStreamWrtr.WriteLine(tempStr)

                'Put the order header info
                ReDim sAttr(1)

                If region = "EU" Then
                    sAttr(0) = "orderDate=""" & poheader.OrderDate.ToString("dd/MM/yyyy") & """"
                Else
                    sAttr(0) = "orderDate=""" & poheader.OrderDate.ToString("MM/dd/yyyy") & """"
                End If

                sAttr(1) = "id=""" & poheader.OrderHeader_ID & """"
                tempStr = XMLElementString(0, "purchaseOrder", XMLTagType.BeginOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                ReDim sAttr(0)
                tempStr = XMLElementString(1, "referenceNumber", XMLTagType.Data, sAttr, CStr(poheader.OrderHeader_ID))
                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "customer_id", XMLTagType.Data, sAttr, CStr(poheader.BusinessUnit))
                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "subteamName", XMLTagType.Data, sAttr, CStr(poheader.SubteamAbbreviation))
                sStreamWrtr.WriteLine(tempStr)

                Call SendOrdersDAO.SystemDateTime(dDate, dTime)

                If region = "EU" Then
                    tempStr = XMLElementString(1, "shipdate", XMLTagType.Data, sAttr, dDate.ToString("dd/MM/yyyy"))
                Else
                    tempStr = XMLElementString(1, "shipdate", XMLTagType.Data, sAttr, dDate.ToString("MM/dd/yyyy"))
                End If

                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "shipdatequalifier", XMLTagType.Data, sAttr, CStr(0))
                sStreamWrtr.WriteLine(tempStr)

                If region = "EU" Then
                    tempStr = XMLElementString(1, "expectedDate", XMLTagType.Data, sAttr, sExpectedDate.ToString("dd/MM/yyyy"))
                Else
                    tempStr = XMLElementString(1, "expectedDate", XMLTagType.Data, sAttr, sExpectedDate.ToString("MM/dd/yyyy"))
                End If
                
                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "transferToSubteamName", XMLTagType.Data, sAttr, Trim(sTransferToSubteamName))
                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "createdByFullName", XMLTagType.Data, sAttr, Trim(sCreatedByName))
                sStreamWrtr.WriteLine(tempStr)

                'Vendor
                sAttr(0) = "country=""" & poheader.vendor.Country & """"
                tempStr = XMLElementString(1, "vendor", XMLTagType.BeginOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                ReDim sAttr(0)
                tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, Trim(poheader.vendor.CompanyName))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, Trim(poheader.vendor.AddressLine1))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address2", XMLTagType.Data, sAttr, Trim(poheader.vendor.AddressLine2))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, Trim(poheader.vendor.City))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, Trim(poheader.vendor.State))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, Trim(poheader.vendor.ZipCode))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "phone", XMLTagType.Data, sAttr, Trim(sVendPhone))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "fax", XMLTagType.Data, sAttr, Trim(sVendFax))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(1, "vendor", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                ' Removed 'billto' because buyer is assumed to be the purchasing location.

                'shipTo
                sAttr(0) = "country=""" & poheader.vendor.Country & """"
                tempStr = XMLElementString(1, "shipto", XMLTagType.BeginOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)
                ReDim sAttr(0)
                tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.CompanyName))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.AddressLine1))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address2", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.AddressLine2))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.City))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.State))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, Trim(poheader.receiving_vendor.ZipCode))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "phone", XMLTagType.Data, sAttr, Trim(sRLPhone))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "fax", XMLTagType.Data, sAttr, Trim(sRLFax))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(1, "shipto", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                'buyer
                sAttr(0) = "country=""" & poheader.vendor.Country & """"
                tempStr = XMLElementString(1, "buyer", XMLTagType.BeginOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)
                ReDim sAttr(0)
                tempStr = XMLElementString(2, "name", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.CompanyName))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.AddressLine1))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "address2", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.AddressLine2))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "city", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.City))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "state", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.State))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "zip", XMLTagType.Data, sAttr, Trim(poheader.purchase_vendor.ZipCode))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "phone", XMLTagType.Data, sAttr, Trim(sPLPhone))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(2, "fax", XMLTagType.Data, sAttr, Trim(sPLFax))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(1, "buyer", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                tempStr = XMLElementString(1, "comment", XMLTagType.Data, sAttr, Trim(poheader.PODescription) & "")
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(1, "customer", XMLTagType.Data, sAttr, CStr(poheader.BusinessUnit))
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(1, "acknowledge_note", XMLTagType.Data, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                If (poheader.Return_Order) Then
                    tempStr = XMLElementString(1, "credit", XMLTagType.Data, sAttr, "true")
                Else
                    tempStr = XMLElementString(1, "credit", XMLTagType.Data, sAttr, "false")
                End If
                sStreamWrtr.WriteLine(tempStr)

                'Footer
                tempStr = XMLElementString(1, "footerMsg", XMLTagType.Data, sAttr, Trim(sFooterMsg))
                sStreamWrtr.WriteLine(tempStr)


                'Items
                tempStr = XMLElementString(1, "items", XMLTagType.BeginOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                poDetails = SendOrdersDAO.GetANSOrderItems(CInt(poheader.OrderHeader_ID))
                If poDetails.Count = 0 Then
                    SendMessage(False, poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
                    SendOwnerMessage(gsSupportEmail, poheader.PeopleSoftNumber & " Order #" & poheader.OrderHeader_ID, "Order failed to transmit because it does not have any items.")
                End If

                ienum = poDetails.GetEnumerator()
                While (ienum.MoveNext)
                    orderItem = CType(ienum.Current, ItemCatalog.OrderItem)
                    iLoop = iLoop + 1

                    ReDim sAttr(0)
                    sAttr(0) = "vendorPartNum=""" & orderItem.VendorItem_ID & """"
                    tempStr = XMLElementString(2, "item", XMLTagType.BeginOnly, sAttr, "")
                    sStreamWrtr.WriteLine(tempStr)

                    ReDim sAttr(0)
                    tempStr = XMLElementString(3, "WFMSKU", XMLTagType.Data, sAttr, Trim(orderItem.Identifier))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "posDept", XMLTagType.Data, sAttr, CStr(orderItem.SubTeam_No))
                    sStreamWrtr.WriteLine(tempStr)

                    ' Set variables for the 'eaches' and 'netUnitCost' before populating xml values for readability
                    ' These values differ based on the VendorOrderUnitName
                    Dim eaches As Decimal
                    Dim netUnitCost As Decimal
                    If Trim(orderItem.VendorOrderUnitName) <> "CASE" And Trim(orderItem.VendorOrderUnitName) <> "BOX" Then
                        eaches = orderItem.QuantityOrdered
                        netUnitCost = orderItem.LineItemCost / orderItem.QuantityOrdered
                    Else
                        eaches = orderItem.QuantityOrdered * orderItem.Package_Desc1
                        netUnitCost = orderItem.LineItemCost / orderItem.QuantityOrdered / orderItem.Package_Desc1
                    End If

                    Dim sOriginCOPInfo As String = String.Empty
                    If (Not orderItem.Origin Is Nothing And Trim(orderItem.Origin) <> String.Empty) Then sOriginCOPInfo = "Origin: " & Trim(orderItem.Origin)
                    If (Not orderItem.CountryOfProcessing Is Nothing And Trim(orderItem.CountryOfProcessing) <> String.Empty) Then
                        If (Not orderItem.Origin Is Nothing) Then sOriginCOPInfo += ", "
                        sOriginCOPInfo += "COP: " & Trim(orderItem.CountryOfProcessing)
                    End If
                    sAttr(0) = "originCOPInfo=""" & sOriginCOPInfo & """"
                    tempStr = XMLElementString(3, "productName", XMLTagType.Data, sAttr, Trim(orderItem.Item_Description))
                    sStreamWrtr.WriteLine(tempStr)
                    ReDim sAttr(0)
                    tempStr = XMLElementString(3, "casePack", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.Package_Desc1)))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "packSize", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.Package_Desc2)))
                    sStreamWrtr.WriteLine(tempStr)
                    'tempStr = XMLElementString(3, "UOM", XMLTagType.Data, sAttr, CStr(orderItem.Package_Desc2))
                    'sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "UOM", XMLTagType.Data, sAttr, Trim(orderItem.Package_Unit_Abbr))
                    sStreamWrtr.WriteLine(tempStr)
                    ' 1/4/09 Tom Lux: Changed this to select 'UnitCost' rather than 'Cost'.
                    tempStr = XMLElementString(3, "USPrice", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.UnitCost), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "quantity", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.QuantityOrdered)))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "eaches", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(eaches)))
                    sStreamWrtr.WriteLine(tempStr)
                    ' Tom Lux, TFS 8261: Adding the XML elements below.
                    tempStr = XMLElementString(3, "brandName", XMLTagType.Data, sAttr, Trim(orderItem.BrandName))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "itemUOMName", XMLTagType.Data, sAttr, Trim(orderItem.ItemUnit))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "itemCasePack", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.ItemCasePack)))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "vendorOrderUOMName", XMLTagType.Data, sAttr, Trim(orderItem.VendorOrderUnitName))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "vendorOrderUOMCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.Cost), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "allowancesAndDiscounts", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.ItemAllowanceDiscountAmount), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "adjustedCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.AdjustedCost), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "quantityDiscount", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.QuantityDiscount)))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "discountLabel", XMLTagType.Data, sAttr, orderItem.DiscountLabel)
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "totalDiscount", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(Math.Round(orderItem.Cost - (orderItem.LineItemCost / orderItem.QuantityOrdered), 3)), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "netOrderUOMCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(Math.Round(orderItem.LineItemCost / orderItem.QuantityOrdered, 3)), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "unitCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(Math.Round(orderItem.Cost / orderItem.Package_Desc1, 3))))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "netUnitCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(Math.Round(netUnitCost, 3)), 2))
                    sStreamWrtr.WriteLine(tempStr)
                    tempStr = XMLElementString(3, "netLineItemCost", XMLTagType.Data, sAttr, removeDecimalTrailingZeros(CStr(orderItem.LineItemCost), 2))
                    sStreamWrtr.WriteLine(tempStr)


                    ' End of item element.
                    tempStr = XMLElementString(2, "item", XMLTagType.EndOnly, sAttr, "")
                    sStreamWrtr.WriteLine(tempStr)
                End While

                'Add end tags
                tempStr = XMLElementString(1, "items", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)
                tempStr = XMLElementString(0, "purchaseOrder", XMLTagType.EndOnly, sAttr, "")
                sStreamWrtr.WriteLine(tempStr)

                sStreamWrtr.Close()

                If Len(sItemErrors) > 0 Then
                    SendMessage(True, poheader.OrderHeader_ID, "The following items were skipped due to errors:" & vbCrLf & sItemErrors)
                    sItemErrors = ""
                End If

            End If
            On Error GoTo me_err

me_exit:
            Return sFileName

me_err:
            lErrNum = Err.Number
            sErrDesc = Err.Description

            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)

me_f_err:

            lErrNum = Err.Number
            sErrDesc = Err.Description


            On Error GoTo 0
            Err.Raise(CInt(lErrNum), , sErrDesc)

        End Function

        Public Shared Function XMLElementString(ByVal iIndent_Level As Integer, ByVal sTagName As String, ByVal TagType As XMLTagType, _
        ByVal sAttr() As String, ByVal sData As String) As String

            Dim sOut As String = String.Empty
            Dim i As Long

            'Errors get raised to the caller

            If iIndent_Level > 0 Then sOut = Space(iIndent_Level * 2)

            If (TagType = XMLTagType.BeginOnly) Or (TagType = XMLTagType.Data) Then
                sOut = sOut & "<" & sTagName
                For i = 0 To UBound(sAttr)
                    If sAttr(CInt(i)) <> "" Then sOut = sOut & " " & sAttr(CInt(i))
                Next
                sOut = sOut & ">"
                If TagType = XMLTagType.BeginOnly Then sOut = sOut & vbCrLf
            End If

            If TagType = XMLTagType.Data Then sOut = sOut & XMLConvertIllegalChar(sData)

            If (TagType = XMLTagType.Data) Or (TagType = XMLTagType.EndOnly) Then sOut = sOut & "</" & sTagName & ">" & vbCrLf

            If TagType = XMLTagType.EmptyTag Then sOut = sOut & "<" & sTagName & " />" & vbCrLf

            XMLElementString = sOut

        End Function

        Public Shared Function XMLConvertIllegalChar(ByVal sIn As String) As String

            Dim i As Integer
            Dim c As String
            Dim sOut As String = ""

            'Convert characters illegal in XML to the escape sequences
            For i = 1 To Len(sIn)
                c = Mid(sIn, i, 1)
                Select Case c
                    Case "&" : sOut = sOut & "&amp;"
                    Case "<" : sOut = sOut & "&lt;"
                    Case """" : sOut = sOut & "&quot;"
                    Case "'" : sOut = sOut & "&apos;"
                    Case ">" : sOut = sOut & "&gt;"
                    Case Else : sOut = sOut & c
                End Select
            Next

            XMLConvertIllegalChar = sOut

        End Function

        Public Function CheckWFMFaxAndEmailStatus() As ArrayList

            Dim sLogRecords As Long
            'Dim sLogRec As typSLOGREC
            'Dim sLogDestRec As typSLOGDEST
            Dim iIndex As Long
            Dim iIndex2 As Long
            Dim sErrMsg As String
            Dim bTimer As Boolean
            Dim bHeadersOnly As Boolean
            'Dim bItem As Boolean
            Dim messageList As VBA.Collection
            Dim messageEntry As OSPOP3.MessageListEntry
            Dim message As OSPOP3.Message
            'Prepare a regular expression objects
            Dim myFaxFailureRegExp As Text.RegularExpressions.Regex
            Dim myOrderInfoRegExp As System.Text.RegularExpressions.Regex
            Dim myOrderInfoSubjectRegExp As Text.RegularExpressions.Regex
            Dim myOrderInfoExplanationRegExp As Text.RegularExpressions.Regex
            Dim myMatches As Text.RegularExpressions.MatchCollection
            Dim myMatch As Text.RegularExpressions.Match
            Dim msgBody As String
            Dim orderErrorMsg As String = ""
            Dim orderHeaderId As String
            Dim oSession As OSPOP3.Session
            Dim szWFMFaxFailurePattern As String
            Dim szWFMFaxOrderInfoPattern As String
            Dim szWFMFaxOrderInfoExplanationPattern As String
            Dim szWFMFaxServer As String
            Dim szWFMFaxUser As String
            Dim szWFMFaxPassword As String
            Dim soptions As System.Text.RegularExpressions.RegexOptions = (System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim logtext As ArrayList = New ArrayList


            'Set all regular expressions

            szWFMFaxFailurePattern = CType(ConfigurationServices.AppSettings("FAX_FAILURE_PATTERN"), String)
            szWFMFaxOrderInfoPattern = CType(ConfigurationServices.AppSettings("ORDER_INFO_PATTERN"), String)
            szWFMFaxOrderInfoExplanationPattern = CType(ConfigurationServices.AppSettings("ORDER_INFO_PATTERN_EXPLANATION"), String)
            szWFMFaxServer = CType(ConfigurationServices.AppSettings("WFMFAXSERVER"), String)
            szWFMFaxUser = CType(ConfigurationServices.AppSettings("WFMFAXLOGIN"), String)
            szWFMFaxPassword = CType(ConfigurationServices.AppSettings("WFMFAXPWD"), String)

            myFaxFailureRegExp = New Text.RegularExpressions.Regex(szWFMFaxFailurePattern, soptions)
            myOrderInfoRegExp = New System.Text.RegularExpressions.Regex(szWFMFaxOrderInfoPattern, soptions)
            myOrderInfoExplanationRegExp = New Text.RegularExpressions.Regex(szWFMFaxOrderInfoExplanationPattern, soptions)

            On Error GoTo me_err

            'connect to POP account
            oSession = New OSPOP3.Session

            oSession.OpenPOP3(szWFMFaxServer, 110, szWFMFaxUser, szWFMFaxPassword)

            If (oSession.State = OSPOP3.StateConstants.popAuthenticated) Then
                Logger.LogInfo("Connected to mailserver", CLASSTYPE)

                While (oSession.GetMessageCount() > 0)
                    Logger.LogInfo("Scanning " & oSession.GetMessageCount().ToString & " message(s)", CLASSTYPE)
                    oSession.GetMessageList()
                    For Each messageEntry In oSession.MessageList

                        message = oSession.GetMessage(messageEntry.ID)
                        'Logger.LogInfo("Parsing: " & message.Subject & " Received: " & message., CLASSTYPE)
                        'Get Header Id
                        orderHeaderId = vbNullString
                        myMatch = myOrderInfoRegExp.Match(message.Body)
                        If (myMatch.Groups.Count > 0) Then
                            orderHeaderId = myMatch.Groups(1).Value
                            Logger.LogInfo("Found Order Header Id: " & orderHeaderId.ToString(), CLASSTYPE)
                        End If

                        myMatch = myOrderInfoExplanationRegExp.Match(message.Body)
                        If (myMatch.Groups.Count > 0) Then
                            orderErrorMsg = myMatch.Groups(1).Value
                        End If

                        'check subject for a failure message
                        myMatch = myFaxFailureRegExp.Match(message.Subject)

                        Dim _po As New POHeaderBO
                        If (orderHeaderId <> vbNullString) Then
                            _po = SendOrdersDAO.GetPOHeader(CInt(orderHeaderId))
                        End If
                        Dim faxNum As String = CStr(IIf(_po.Purchase_Vendor_Fax = String.Empty, _po.OverrideTransmissionTarget, _po.Purchase_Vendor_Fax))

                        If (myMatch.Success) Then

                            'fax failure found
                            If (orderHeaderId <> vbNullString) Then
                                'send notification
                                SendOrdersDAO.UpdateOrderCancelSend(CInt(orderHeaderId))

                                sErrMsg = "Fax failed to transmit." & vbCrLf & "Error Reason : " & orderErrorMsg & vbCrLf
                                sErrMsg = sErrMsg & vbCrLf & vbCrLf & "The fax failed with the above error after 3 attempts to send." & vbCrLf & "Check with the vendor to determine if they are having problems receiving faxes.  If not, please contact the Help Desk." & vbCrLf & "You must Send the PO again to re-try automatic faxing."
                                SendMessage(False, CLng(orderHeaderId), sErrMsg)

                                FaxLog.Log.UpdateStatus(CInt(orderHeaderId), faxNum, sErrMsg, FaxLog.FaxStatus.TransmissionFailure)

                                Logger.LogInfo("Order: #" & orderHeaderId & "Fax failed to transmit." & vbCrLf & "Error Reason : " & orderErrorMsg, CLASSTYPE)
                            End If
                        Else
                            If (orderHeaderId <> vbNullString) Then
                                SendOrdersDAO.UpdateOrderSentDate(CInt(orderHeaderId))
                                SendMessage(True, CLng(orderHeaderId), "Fax transmitted successfully.")

                                FaxLog.Log.UpdateStatus(CInt(orderHeaderId), faxNum, "Fax transmitted successfully.", FaxLog.FaxStatus.TransmissionSuccess)

                                Logger.LogInfo("Order: #" & orderHeaderId & " Fax transmitted successfully.", CLASSTYPE)
                            End If
                        End If
                        'delete message
                        oSession.DeleteMessage(messageEntry.ID)
                    Next
                End While
                Logger.LogInfo("Closing connection", CLASSTYPE)
                oSession.ClosePOP3()
            Else
                Logger.LogInfo("Unable to Connect to mailserver: Session State [ " & oSession.State.ToString & " ]", CLASSTYPE)
            End If

me_exit:

            Return logtext

me_err:

            sErrMsg = "CheckFaxStatus failed due to system error: " & CStr(Err.Number) & " - " & Err.Description
            Logger.LogError(sErrMsg, CLASSTYPE)
            SendOwnerMessage(gsSupportEmail, "Replenishment: CheckFaxStatus Failed", sErrMsg)
            logtext.Add(sErrMsg)

            GoTo me_exit

        End Function

        Public Shared Function createHTMLFromXSL(ByVal poheader As POHeaderBO) As String
            Dim tr As System.IO.TextReader
            Dim writer As System.IO.TextWriter
            Dim xr As System.Xml.XmlReader
            'Dim trans As New System.Xml.Xsl.XslTransform (used obsolete object)
            Dim trans As New System.Xml.Xsl.XslCompiledTransform
            Dim xslArg As New System.Xml.Xsl.XsltArgumentList
            Dim str As New System.IO.MemoryStream
            Dim xp As System.Xml.XPath.XPathDocument
            Dim filename As String
            Dim xmlFilename As String

            xmlFilename = CreateXMLOrderFile(poheader)

            xp = New System.Xml.XPath.XPathDocument(xmlFilename)
            tr = New System.IO.StreamReader("fax.xsl")
            filename = String.Format(My.Application.Info.DirectoryPath & "\{0}\" & "po_" & poheader.OrderHeader_ID & ".HTML", sRegion)
            writer = New System.IO.StreamWriter(filename)
            xr = New System.Xml.XmlTextReader(tr)
            trans.Load(xr)

            trans.Transform(xp, xslArg, writer)
            str.Flush()
            str.Position = 0

            writer.Close()
            writer.Dispose()

            xr.Close()

            'delete XML file
            'now delete file
            Kill(xmlFilename)


            Return filename
        End Function

    End Class
End Namespace
