Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.InterfaceCommunication
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiModel
Imports WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.Utility

Friend Class frmPricingPrintSignInfo
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView
    Private mlBatchHeader As PriceBatchHeaderBO = Nothing
    Private mlStoreNo As Integer = 0
    Private lItem_Key() As Integer
    Private sItemList As String
    Private itemListSeparator As Char
    Private IsLoading As Boolean
    Private mIsPlanogramReg As Boolean
    Private mIsPlanogramNonReg As Boolean
    Private mdteStartDate As Date
    Private Shared isNetworkIssue As Boolean = False
    Private Shared NonNetworkIssueResponseCodes As List(Of Integer) = New List(Of Integer) From {422, 500}
    Private Shared _failedPrintRequests As List(Of PriceBatchFailedPrintRequestsBO) = New List(Of PriceBatchFailedPrintRequestsBO)
    Private Shared _slawApiErrorMessage As String

    Public ReadOnly Property FailedPrintRequests() As List(Of PriceBatchFailedPrintRequestsBO)
        Get
            Return _failedPrintRequests
        End Get
    End Property

    Public ReadOnly Property SlawApiErrorMessage As String
        Get
            Return _slawApiErrorMessage
        End Get
    End Property

    Private _notCancelled As Boolean
    Public ReadOnly Property NotCancelled As Boolean
        Get
            Return _notCancelled
        End Get
    End Property

    Public Sub Reset()
        isNetworkIssue = False
        _failedPrintRequests = New List(Of PriceBatchFailedPrintRequestsBO)
        _slawApiErrorMessage = String.Empty
    End Sub

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub SetPlanogramSignInfo(ByVal itemKeys() As Integer, ByVal separator As Char, ByVal storeNumber As Integer, ByVal isPlanogramReg As Boolean, ByVal startDate As Date)
        logger.Debug("SetPlanogramSignInfo Enter")

        mlStoreNo = storeNumber
        mIsPlanogramReg = isPlanogramReg
        mIsPlanogramNonReg = Not (isPlanogramReg)
        mdteStartDate = startDate
        mlBatchHeader = New PriceBatchHeaderBO()
        mlBatchHeader.StoreNumber = storeNumber
        SetItemInfo(itemKeys, separator)
        lblStartLabel.Visible = False
        StartLabelTextBox.Visible = False

        logger.Debug("SetPlanogramSignInfo Exit")
    End Sub

    Public Sub SetReprintSignInfo(ByVal itemKeys() As Integer, ByVal separator As Char, ByVal storeNumber As Integer)
        logger.Debug("SetReprintSignInfo Enter")

        mlStoreNo = storeNumber
        mlBatchHeader = New PriceBatchHeaderBO()
        mlBatchHeader.StoreNumber = storeNumber
        SetItemInfo(itemKeys, separator)

        logger.Debug("SetReprintSignInfo Exit")
    End Sub

    Public Sub SetBatchHeaderInfo(ByVal itemKeys() As Integer, ByVal separator As Char, ByRef batchHeader As PriceBatchHeaderBO)
        logger.Debug("SetBatchHeaderInfo Enter")

        mlBatchHeader = batchHeader
        SetItemInfo(itemKeys, separator)

        logger.Debug("SetBatchHeaderInfo Exit")
    End Sub

    Private Sub SetItemInfo(ByVal itemKeys() As Integer, ByVal separator As Char)
        logger.Debug("SetItemInfo Enter")

        Dim IsLoading As Boolean

        IsLoading = True
        lItem_Key = itemKeys
        itemListSeparator = separator

        Call LoadDataTable()

        IsLoading = False

        logger.Debug("SetItemInfo Exit")
    End Sub

    Private Sub LoadDataTable()
        logger.Debug("LoadDataTable Enter")

        Dim item As String
        Dim index As Integer

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        index = 0
        Do While index <= UBound(lItem_Key)
            sItemList = String.Empty
            item = CStr(lItem_Key(index))

            Do While index <= UBound(lItem_Key)
                If Len(sItemList) = 0 Then
                    sItemList = item
                Else
                    sItemList = sItemList & "|" & item
                End If

                index = index + 1

                If index <= UBound(lItem_Key) Then item = CStr(lItem_Key(index))
            Loop
        Loop

        logger.Info("PriceBatchHeaderLabelSummaryDAO.GetLabelSummaryData Start: sItemList=" + sItemList + ", PriceBatchHeaderID=" + mlBatchHeader.PriceBatchHeaderId.ToString)
        mdt = PriceBatchHeaderLabelSummaryDAO.GetLabelSummaryData(sItemList, itemListSeparator, mlBatchHeader.PriceBatchHeaderId)
        logger.Info("PriceBatchHeaderLabelSummaryDAO.GetLabelSummaryData End")

        mdt.AcceptChanges()

        mdv = New System.Data.DataView(mdt)
        mdv.Sort = "LabelTypeDesc"

        ugrdList.DataSource = mdv

        'This may or may not be required.
        If mdt.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdList.Rows(0).Selected = True
        End If

ExitSub:

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("LoadDataTable Exit")
    End Sub

    Private Sub frmPricingPrintSignInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.Debug("frmPricingPrintSignInfo_Load Enter")

        Dim sSortLabels As String

        CenterForm(Me)

        If IsRegionUsingPrintLab() Then
            chkSortLabels.Visible = True
            lblSortLabels.Visible = True
        Else
            chkSortLabels.Visible = False
            lblSortLabels.Visible = False
        End If

        Try
            sSortLabels = WholeFoods.Utility.ConfigurationServices.AppSettings("SortPrintLabShelfTags")
        Catch ex As Exception
            sSortLabels = String.Empty
        End Try

        If sSortLabels = String.Empty Then sSortLabels = "False"

        chkSortLabels.Checked = CBool(sSortLabels)

        logger.Debug("frmPricingPrintSignInfo_Load Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Enter")

        DialogResult = DialogResult.Cancel
        Me._notCancelled = False
        Me.Close()

        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        logger.Debug("cmdPrint_Click Enter")

        Dim index As Integer
        Dim filename As String
        Dim reportFlag As Boolean
        Dim reportUrl As New System.Text.StringBuilder
        Dim activateFileWriterFlag As Boolean = False
        Dim labelSizeId As Integer
        Dim labelType As Integer
        Dim labelSize As String = String.Empty
        Dim tagOverride As Boolean = False
        Dim specialTagPromo As Boolean = False
        Dim region As String

        filename = ConfigurationServices.AppSettings("region")
        region = ConfigurationServices.AppSettings("region")

        If InstanceDataDAO.IsFlagActive("ActivateFileTagWriter", mlBatchHeader.StoreNumber) Then
            activateFileWriterFlag = True
        End If

        reportFlag = False

        If (cmbTagOverride.SelectedIndex <> -1) Then
            tagOverride = True
        End If

        labelType = LabelTypeEnum.Regular

        If optPriceType_Regular.Checked Then
            labelType = LabelTypeEnum.Regular
        ElseIf optPriceType_Promo.Checked Then
            labelType = LabelTypeEnum.Promo
        ElseIf optPriceType_New.Checked Then
            labelType = LabelTypeEnum.[New]
        End If

        If ugrdList.Selected.Rows.Count < 1 Then Exit Sub

        labelSizeId = CType(ugrdList.Selected.Rows(0).Cells("LabelType_ID").Value, Integer)

        If tagOverride Then
            labelSizeId = CType(cmbTagOverride.SelectedValue, Integer)
            Select Case CType(labelSizeId, LabelSizeEnum)
                Case LabelSizeEnum.Small
                    labelSize = "Small"

                Case LabelSizeEnum.Medium
                    labelSize = "Medium"

                Case LabelSizeEnum.SmallGlutenFree
                    If optPriceType_New.Checked Then
                        labelSize = "Small"
                        specialTagPromo = True
                    Else
                        labelSize = "SmallGlutenFree"
                    End If

                Case LabelSizeEnum.SmallDairyFree
                    If optPriceType_New.Checked Then
                        labelSize = "Small"
                        specialTagPromo = True
                    Else
                        labelSize = "SmallDairyFree"
                    End If

                Case LabelSizeEnum.SmallPrivateLabel
                    If optPriceType_New.Checked Then
                        labelSize = "Small"
                        specialTagPromo = True
                    Else
                        labelSize = "SmallPrivateLabel"
                    End If

                Case LabelSizeEnum.MediumGlutenFree
                    If optPriceType_New.Checked Then
                        labelSize = "Medium"
                        specialTagPromo = True
                    Else
                        labelSize = "MediumGlutenFree"
                    End If

                Case LabelSizeEnum.MediumDairyFree
                    If optPriceType_New.Checked Then
                        labelSize = "Medium"
                    Else
                        labelSize = "MediumDairyFree"
                    End If

                Case LabelSizeEnum.MediumPrivateLabel
                    If optPriceType_New.Checked Then
                        labelSize = "Medium"
                    Else
                        labelSize = "MediumPrivateLabel"
                    End If
            End Select
        End If

        If (activateFileWriterFlag <> True) Then
            If ugrdList.Selected.Rows.Count <> 1 Then
                MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            Else
                If region = "EU" Then
                    If labelSizeId = LabelSizeEnum.Undefined Then
                        MsgBox("Unable to locate report for specified tag type.", MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If

                    filename = "UK_" + labelSize + CType(labelType, LabelTypeEnum).ToString

                    reportUrl.Append(filename)
                    reportUrl.Append("&rs:Format=PDF")
                    reportUrl.Append("&rs:Command=Render")
                    reportUrl.Append("&rc:Parameters=False")
                    reportUrl.Append("&ItemList=" & sItemList)

                    If mlBatchHeader.PriceBatchHeaderId = 0 Then
                        reportUrl.Append("&StoreNo=" & CType(mlStoreNo, Integer))
                        reportUrl.Append("&MarkPrinted=" & CType(False, Boolean))
                    Else
                        reportUrl.Append("&PriceBatchHeaderID=" & CType(mlBatchHeader.PriceBatchHeaderId, String))
                    End If

                    reportUrl.Append("&StartLabelPosition=" & CType(StartLabelTextBox.Text, Integer))

                    If tagOverride Then
                        'get original default size listed in the grid for overrides (since list of all items is passed)
                        reportUrl.AppendFormat("&LabelType={0}", CType(ugrdList.Selected.Rows(0).Cells("LabelType_ID").Value, Integer))
                    ElseIf specialTagPromo Then
                        reportUrl.AppendFormat("&LabelType={0}", labelSizeId)
                    End If

                    If Not itemListSeparator.Equals("|") Then
                        'parameter defaults to '|'; only need to specify when another delimiter is used
                        reportUrl.AppendFormat("&ListDelimiter={0}", itemListSeparator)
                    End If

                    Call ReportingServicesReport(reportUrl.ToString)
                Else
                    index = CType(ugrdList.Selected.Rows(0).Cells("LabelType_ID").Value, Integer)
                    Select Case index
                        Case 2
                            If mlBatchHeader.PriceBatchHeaderId = 0 Then
                                filename = filename + "_MediumSigns_Items"
                            Else
                                filename = filename + "_MediumSigns_Batch"
                            End If
                            reportFlag = True
                        Case 4
                            If mlBatchHeader.PriceBatchHeaderId = 0 Then
                                filename = filename + "_SmallSigns_Items"
                            Else
                                filename = filename + "_SmallSigns_Batch"
                            End If
                            reportFlag = True
                        Case Else
                            'oh crap, no reports available for these label types.
                    End Select

                    If reportFlag Then
                        reportUrl.Append(filename)
                        reportUrl.Append("&rs:Format=PDF")
                        reportUrl.Append("&rs:Command=Render")
                        reportUrl.Append("&rc:Parameters=False")
                        reportUrl.Append("&ItemList=" & sItemList)
                        reportUrl.Append("&ItemListSeparator=" & itemListSeparator)

                        If mlBatchHeader.PriceBatchHeaderId = 0 Then
                            reportUrl.Append("&StoreNo=" & CType(mlStoreNo, Integer))
                            reportUrl.Append("&MarkPrinted=" & CType(False, Boolean))
                        Else
                            reportUrl.Append("&PriceBatchHeaderID=" & CType(mlBatchHeader.PriceBatchHeaderId, String))
                        End If

                        reportUrl.Append("&StartLabelPosition=" & CType(StartLabelTextBox.Text, Integer))

                        Call ReportingServicesReport(reportUrl.ToString)
                    End If
                End If
            End If
        Else
            Me.Enabled = False
            Me.Refresh()
            Me.Cursor = Cursors.WaitCursor

            'call tag file writer logic
            PerformTagFileWriterPrinting()
            SendSlawPrintBatchMessage(isReprint:=True)

            If _failedPrintRequests.Count > 0 Then
                Dim failedPrintRequests As String = String.Join(Environment.NewLine, _failedPrintRequests.Select(Function(f) f.BatchName & ": Store " & f.StoreNumber & " - " & f.SubteamName))
                FailedRequestsDialog.HandleError("The following print requests failed to process.  This information has also been written to the local IRMA client log file.", failedPrintRequests, _slawApiErrorMessage)
            Else
                MessageBox.Show("The print request has been submitted successfully.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Me._notCancelled = True
            Me.Cursor = Cursors.Default
            Me.Enabled = True
            Me.Reset()
            Me.Close()
        End If

        logger.Debug("cmdPrint_Click Exit")
    End Sub

    ''' <summary>
    ''' logic that performs shelf tag printing using the tag file writer
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PerformTagFileWriterPrinting()
        logger.Debug("PerformTagFileWriterPrinting Enter: PriceBatchHeaderID=" + mlBatchHeader.PriceBatchHeaderId.ToString)

        Dim tagProc As TagPriceBatchProcessor = New TagPriceBatchProcessor(mlBatchHeader.PriceBatchHeaderId)

        If mlBatchHeader.PriceBatchHeaderId = 0 Then
            logger.Info("PerformTagFileWriterPrinting: PriceBatchHeaderID=0, mIsPlanogramReg=" + mIsPlanogramReg.ToString + ", mIsPlanogramNonReg=" + mIsPlanogramNonReg.ToString)

            If mIsPlanogramReg = False And mIsPlanogramNonReg = False Then
                logger.Info("PerformTagFileWriterPrinting - calling the TagProcessor to reprint tags.")
                tagProc.GetReprintTagDataFromIRMA(sItemList, itemListSeparator, CType(StartLabelTextBox.Text, Integer), mlBatchHeader.StoreNumber, chkSortLabels.Checked)
            Else
                logger.Info("PerformTagFileWriterPrinting - calling the TagProcessor for planogram printing.")
                tagProc.GetPlanogramTagDataFromIRMA(sItemList, itemListSeparator, mlBatchHeader.StoreNumber, mIsPlanogramReg, mdteStartDate)
            End If
        Else
            logger.Info("PerformTagFileWriterPrinting - calling the TagProcessor for batch tag printing.")
            tagProc.GetPriceBatchTagDataFromIRMA(sItemList, itemListSeparator, mlBatchHeader.PriceBatchHeaderId, CType(StartLabelTextBox.Text, Integer), mlBatchHeader.StoreNumber)
        End If

        Me._notCancelled = True
        logger.Debug("PerformTagFileWriterPrinting Exit")
    End Sub

    Public Sub SendSlawPrintBatchMessage(ByVal isReprint As Boolean)
        Cursor.Current = Cursors.WaitCursor

        If Not SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
            logger.Info("EnableSlawPrintBatching is turned off: No message will be sent to SLAW API.")
            Exit Sub
        End If

        ' ItemChgTypeID = 3 is a DEL batch.
        If mlBatchHeader.ItemChgTypeID = 3 Then
            logger.Info(String.Format("Print batch will not be sent to SLAW as PriceBatchHeaderID = {0} is a delete batch.", mlBatchHeader.PriceBatchHeaderId.ToString()))
            Exit Sub
        End If

        Dim printRequestDetailTable As DataTable
        Dim slawPrintBatchModel As SlawPrintBatchModel = New SlawPrintBatchModel

        printRequestDetailTable = ItemPriceBatchDetailDAO.GetPriceBatchDetailPrintRequestData(mlBatchHeader.PriceBatchHeaderId, sItemList, itemListSeparator)

        If printRequestDetailTable.Rows.Count > 0 Then
            Try
                logger.Info(String.Format("Sending print request to SLAW API: PriceBatchHeaderID = {0}.", mlBatchHeader.PriceBatchHeaderId.ToString()))

                Dim priceBatchItemFirstRow As DataRow = printRequestDetailTable.Rows(0)

                slawPrintBatchModel.Application = SlawConstants.IrmaApplication
                slawPrintBatchModel.BatchId = priceBatchItemFirstRow("PriceBatchHeaderID")
                slawPrintBatchModel.BatchName = priceBatchItemFirstRow("BatchDescription")
                slawPrintBatchModel.BatchType = SlawConstants.BatchTypeSignsOrLabels
                slawPrintBatchModel.BusinessUnitId = priceBatchItemFirstRow("Business_Unit")
                slawPrintBatchModel.EffectiveDate = Convert.ToDateTime(priceBatchItemFirstRow("StartDate")).ToString("s")
                slawPrintBatchModel.IsAdHoc = False
                slawPrintBatchModel.BatchEvent = If(isReprint, SlawConstants.BatchReprintEvent, SlawConstants.BatchPrintEvent)
                slawPrintBatchModel.ItemCount = printRequestDetailTable.Rows.Count

                If priceBatchItemFirstRow("ItemChgTypeDesc").Equals("Price") Then
                    slawPrintBatchModel.HasPriceChange = 1
                    slawPrintBatchModel.BatchChangeType = "PRC"
                Else
                    slawPrintBatchModel.HasPriceChange = 0
                    slawPrintBatchModel.BatchChangeType = If(priceBatchItemFirstRow("ItemChgTypeDesc") = "New", "NEW", "ITM")
                End If

                slawPrintBatchModel.BatchItems = New List(Of SlawPrintBatchItemModel)

                For Each priceBatchItemRow As DataRow In printRequestDetailTable.Rows
                    Dim slawPrintBatchItemModel As SlawPrintBatchItemModel = New SlawPrintBatchItemModel

                    slawPrintBatchItemModel.Identifier = priceBatchItemRow("Identifier")
                    slawPrintBatchItemModel.PrintOrder = 0
                    slawPrintBatchItemModel.StartDate = If(priceBatchItemRow("SaleStartDate") Is DBNull.Value, String.Empty, Convert.ToDateTime(priceBatchItemRow("SaleStartDate")).ToString("s"))
                    slawPrintBatchItemModel.Template = String.Empty
                    If Convert.ToInt32(priceBatchItemRow("TprBatchHasPriceChange")) = 1 Then
                        slawPrintBatchItemModel.TprHasRegPriceChange = True
                    Else
                        slawPrintBatchItemModel.TprHasRegPriceChange = False
                    End If

                    slawPrintBatchModel.BatchItems.Add(slawPrintBatchItemModel)
                Next

                Using slawWebApiWrapper As SlawWebApiWrapper = New SlawWebApiWrapper(String.Empty)
                    If isNetworkIssue Then
                        If slawPrintBatchModel IsNot Nothing And mlBatchHeader IsNot Nothing Then
                            Dim failedRequest As New PriceBatchFailedPrintRequestsBO With {.BatchName = slawPrintBatchModel.BatchName, .StoreNumber = mlBatchHeader.StoreNumber, .SubteamName = mlBatchHeader.SubteamName}
                            _failedPrintRequests.Add(failedRequest)
                            _slawApiErrorMessage = New StringBuilder(_slawApiErrorMessage).Append("Skipped: ").Append(mlBatchHeader.StoreNumber.ToString() + " ").Append(mlBatchHeader.BatchDescription).AppendLine().ToString()
                        End If
                    Else
                        slawWebApiWrapper.PostPrintHeader(slawPrintBatchModel)
                        logger.Info(String.Format("Sent print batch to SLAW API: PriceBatchHeaderID = {0}.", mlBatchHeader.PriceBatchHeaderId.ToString()))
                    End If
                End Using

            Catch apiException As Exception
                Dim slawJsonResponseText As String = Nothing
                Dim slawJsonResponse As SlawJsonResponse = Nothing
                If TypeOf (apiException) Is WebException Then
                    Dim apiWebException = TryCast(apiException, WebException)

                    If apiWebException IsNot Nothing Then
                        If apiWebException.Status = WebExceptionStatus.ProtocolError Then
                            Dim response As HttpWebResponse = TryCast(apiWebException.Response, HttpWebResponse)
                            If response IsNot Nothing Then
                                If IsNetworkIssueResponse(response) Then
                                    isNetworkIssue = True
                                Else
                                    Using responseReader As StreamReader = New StreamReader(response.GetResponseStream())
                                        slawJsonResponseText = responseReader.ReadToEnd()
                                        Dim serializer As JavaScriptSerializer = New JavaScriptSerializer()
                                        slawJsonResponse = serializer.Deserialize(Of SlawJsonResponse)(slawJsonResponseText)
                                    End Using
                                End If
                            End If
                        ElseIf apiWebException.Status = WebExceptionStatus.Timeout Or apiWebException.Status = WebExceptionStatus.UnknownError Or apiWebException.Status = WebExceptionStatus.ConnectFailure Then
                            isNetworkIssue = True
                        End If
                    End If
                End If

                If slawPrintBatchModel IsNot Nothing And mlBatchHeader IsNot Nothing Then
                    Dim failedRequest As New PriceBatchFailedPrintRequestsBO With {.BatchName = slawPrintBatchModel.BatchName, .StoreNumber = mlBatchHeader.StoreNumber, .SubteamName = mlBatchHeader.SubteamName}
                    _failedPrintRequests.Add(failedRequest)
                End If

                Dim errorMessageStringBuilder As StringBuilder = New StringBuilder(_slawApiErrorMessage).Append(apiException.GetBaseException().Message + " ").Append(mlBatchHeader.StoreNumber.ToString() + " ").Append(mlBatchHeader.BatchDescription).AppendLine()
                If slawJsonResponse IsNot Nothing AndAlso slawJsonResponse.ItemErrors IsNot Nothing AndAlso slawJsonResponse.ItemErrors.Any() Then
                    For Each itemError As String In slawJsonResponse.ItemErrors
                        errorMessageStringBuilder.AppendLine(itemError)
                    Next itemError
                    'errorMessageStringBuilder.AppendLine()

                    Dim failedRequest As New PriceBatchFailedPrintRequestsBO With {.BatchName = slawPrintBatchModel.BatchName, .StoreNumber = mlBatchHeader.StoreNumber, .SubteamName = errorMessageStringBuilder.Append(Environment.NewLine).ToString()}
                    _failedPrintRequests.Add(failedRequest)
                End If

                _slawApiErrorMessage = errorMessageStringBuilder.ToString()

                logger.Info(String.Format("Error in sending print batch to SLAW API: PriceBatchHeaderID = {0}. Error: {1}. Slaw Response: {2}",
                                          mlBatchHeader.PriceBatchHeaderId.ToString(), apiException.GetBaseException().Message, slawJsonResponseText))
            End Try
        Else
            logger.Info(String.Format("No print request records were returned from the database.  PriceBatchHeaderID = {0}.", mlBatchHeader.PriceBatchHeaderId.ToString()))
        End If

        Me._notCancelled = True
        Cursor.Current = Cursors.Default
    End Sub

    Private Shared Function IsNetworkIssueResponse(response As Object) As Boolean
        Dim responseStatusCode As Integer = CInt(response.StatusCode)
        Return Not NonNetworkIssueResponseCodes.Contains(responseStatusCode)
    End Function

    Private Sub StartLabelTextBox_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles StartLabelTextBox.Enter
        logger.Debug("StartLabelTextBox_Enter Enter")

        HighlightText(StartLabelTextBox)

        logger.Debug("StartLabelTextBox_Enter End")
    End Sub

    Private Sub StartLabelTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles StartLabelTextBox.KeyPress
        logger.Debug("StartLabelTextBox_KeyPress Enter")

        Dim keyAscii As Short = Asc(e.KeyChar)

        keyAscii = ValidateKeyPressEvent(keyAscii, StartLabelTextBox.Tag, StartLabelTextBox, 0, 0, 0)

        e.KeyChar = Chr(keyAscii)
        If keyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("StartLabelTextBox_KeyPress Exit")
    End Sub

    Private Function IsRegionUsingPrintLab() As Boolean
        Dim rs As DAO.Recordset = Nothing

        rs = SQLOpenRecordSet("SELECT W.POSFileWriterClass " &
                              "FROM StoreShelfTagConfig C " &
                              "JOIN POSWriter W ON W.POSFileWriterKey = C.POSFileWriterKey " &
                              "WHERE C.Store_No = " & CStr(mlBatchHeader.StoreNumber), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If Not rs Is Nothing Then
            If rs.RecordCount = 0 Then
                IsRegionUsingPrintLab = False
            ElseIf InStr(rs.Fields(0).Value.ToString, "PrintLab") > 0 Then
                IsRegionUsingPrintLab = True
            Else
                IsRegionUsingPrintLab = False
            End If
        Else
            IsRegionUsingPrintLab = False
        End If
    End Function

    Private Sub frmPricingPrintSignInfo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim Region As String = ConfigurationServices.AppSettings("region")

        If Region = "EU" Then
            With cmbTagOverride
                .Items.Clear()
                .DataSource = WholeFoods.IRMA.ItemHosting.DataAccess.LabelTypeDAO.GetLabelTypeList
                .DisplayMember = "LabelTypeDesc"
                .ValueMember = "LabelTypeID"
            End With

            optPriceType_Regular.Checked = True
            pnlPriceTypeOverrideOptions.Visible = True
            cmbTagOverride.SelectedItem = ugrdList.Selected.Rows(0).Cells("LabelTypeDesc").Value
        Else
            lblTagType.Visible = False
            cmbTagOverride.Visible = False
            pnlPriceTypeOverrideOptions.Visible = False
            cmbTagOverride.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmbTagType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTagOverride.KeyDown
        Select Case e.KeyCode
            Case Keys.Back, Keys.Delete
                cmbTagOverride.SelectedIndex = -1
                e.Handled = True
            Case Else
                'do nothing
        End Select
    End Sub

    Private Sub cmbTagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTagOverride.KeyPress
        Select Case Asc(e.KeyChar)
            Case Keys.Back, Keys.Delete
                cmbTagOverride.SelectedIndex = -1
                e.Handled = True
            Case Else
                'do nothing
        End Select
    End Sub
End Class
