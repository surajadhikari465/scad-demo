Option Strict Off
Option Explicit On

Imports System.Linq
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess

Friend Class frmPricingBatchDetail
	Inherits System.Windows.Forms.Form
    Private mdt As DataTable
    Private mdv As DataView

    Dim mrsDetail As ADODB.Recordset
    Dim _batchHeader As PriceBatchHeaderBO
    Dim mbGridCtrlKey As Boolean
    Dim mbSelChange As Boolean

    Dim isHeaderInfoChanged As Boolean
    Dim _isBatchInfoRefreshed As Boolean
    Dim IsLoading As Boolean
    Dim IsInitializing As Boolean
    Dim WithEvents frmPricingBatchItemSearch As frmPricingBatchItemSearch

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _noTagItemsExcluded As Boolean
    Public ReadOnly Property NoTagItemsExcluded() As Boolean
        Get
            Return _noTagItemsExcluded
        End Get
    End Property

    Public ReadOnly Property IsBatchInfoRefreshed() As Boolean
        Get
            Return _isBatchInfoRefreshed
        End Get
    End Property

    Public Property BatchHeader() As PriceBatchHeaderBO
        Get
            Return _batchHeader
        End Get
        Set(ByVal value As PriceBatchHeaderBO)
            _batchHeader = value
        End Set
    End Property

    Dim _noTagLogicWasIgnored As Boolean
    ' flag to signal caller whether batches were processed in a way which normally would have
    ' invoked the No-Tag logic but instead skipped this logic during processing as directed
    ' by the IgnoreNoTagLogic property
    Public ReadOnly Property NoTagLogicWasIgnored() As Boolean
        Get
            Return _noTagLogicWasIgnored
        End Get
    End Property

    ' parameter which can be used to skip No-Tag logic when processing batches.
    ' value corresponds to the [disabled] checkbox shown in the GUI
    Public Property IgnoreNoTagLogic() As Boolean
        Get
            Return chkIgnoreNoTagLogic.Checked
        End Get
        Set(ByVal value As Boolean)
            chkIgnoreNoTagLogic.Checked = value
        End Set
    End Property

    Private Sub frmPricingBatchDetail_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmPricingBatchDetail_Load Enter")
        CenterForm(Me)

        ' reset control values to default
        Me.chkExport.Checked = False
        Me.chkPrintOnly.Checked = False
        Me.cmbTagType.SelectedIndex = 0
        Me.txtField(4).Text = "1"

        ' The UK uses the Item.ItemType to determine the stock type, so
        ' this combo is irrelevant.
        If gsRegionCode.Equals("EU") Then
            cmbTagType.SelectedIndex = -1
            cmbTagType.Enabled = False
        Else
            cmbTagType.SelectedIndex = 0
        End If

        'disable auto apply date box if auto apply check box is checked
        AutoApplyDateUDTE.Enabled = (Not AutoApplyCheckBox.Checked)

        logger.Debug("frmPricingBatchDetail_Load Exit")
    End Sub


    Private Sub LoadDataTable(ByVal priceBatchHeaderID As Integer)
        logger.Debug("LoadDataTable Enter")
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        mdt = PriceBatchDetailDAO.GetDetailSearchData(priceBatchHeaderID, Me.Text)

        'Setup a column that you would like to sort on initially.
        mdt.AcceptChanges()
        mdv = New System.Data.DataView(mdt)

        Select Case True
            Case optPrintSign(0).Checked
                mdv.RowFilter = "PrintSign = 1"
            Case optPrintSign(1).Checked
                mdv.RowFilter = "PrintSign = 0"
        End Select

        mdv.Sort = "Item_Description"

        ugrdList.DataSource = mdv

        'This may or may not be required.
        If mdt.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdList.Rows(0).Selected = True
        Else
            'update calling form
            _isBatchInfoRefreshed = True
        End If

ExitSub:

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("LoadDataTable Exit")

    End Sub

    Public Sub SetBatchHeaderInfo(ByVal AllowMaintenance As Boolean)
        logger.Debug("SetBatchHeaderInfo Enter")
        Dim i As Integer
        IsLoading = True

        BatchDescriptionTextBox.Text = BatchHeader.BatchDescription
        AutoApplyCheckBox.Checked = BatchHeader.AutoApplyFlag
        PosBatchIdTextBox.Text = BatchHeader.POSBatchId
        If BatchHeader.AutoApplyDate <> Nothing Then AutoApplyDateUDTE.Value = BatchHeader.AutoApplyDate

        If BatchHeader.ItemChgTypeID = 4 Then  'If "Offer" then hide these buttons.
            fraMaintain.Visible = False
        Else
            If gbItemAdministrator AndAlso AllowMaintenance Then  'Central Pricing is either a SuperUser or SPE user.  Allow maint appears to always be true.
                cmdMaintain(0).Enabled = (BatchHeader.PriceBatchStatusID = 1)
                cmdMaintain(1).Enabled = (BatchHeader.PriceBatchStatusID < 3)
                cmdMaintain(2).Enabled = cmdMaintain(1).Enabled
                If cmdMaintain(0).Enabled OrElse cmdMaintain(1).Enabled Then
                    fraMaintain.Visible = True
                    HeaderFrame.Enabled = True
                Else
                    fraMaintain.Visible = False
                    HeaderFrame.Enabled = False
                End If
            Else
                fraMaintain.Visible = False
                HeaderFrame.Enabled = False
            End If
        End If

        If InstanceDataDAO.IsFlagActiveCached("BypassPrintShelfTags", BatchHeader.StoreNumber) Then
            cmdMarkAsPrinted.Visible = False
            cmdMarkAsPrinted.Enabled = False
        End If

        If BatchHeader.PriceBatchStatusID >= 5 _
            And (BatchHeader.ItemChgTypeID = 1 _
                 Or BatchHeader.ItemChgTypeID = 2 _
                 Or BatchHeader.ItemChgTypeID = 0) _
                Then
            cmdPrint.Enabled = True
        Else
            cmdPrint.Enabled = False
        End If

        'enable entire group of shelf tag input options
        fraProcess.Enabled = True
        fraProcess.Visible = True

        For i = Me.optPrintSign.LBound To Me.optPrintSign.UBound
            Me.optPrintSign(i).Enabled = cmdPrint.Enabled
        Next

        logger.Info("LoadDataTable Start: PriceBatchHeaderID=" & CStr(BatchHeader.PriceBatchHeaderId))
        Call LoadDataTable(BatchHeader.PriceBatchHeaderId)
        logger.Info("LoadDataTable End: PriceBatchHeaderID=" & CStr(BatchHeader.PriceBatchHeaderId))

        IsLoading = False

        logger.Debug("SetBatchHeaderInfo Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Enter")
        If isHeaderInfoChanged Then
            logger.Info(ResourcesPricing.GetString("SaveHeaderInfo"))
            If MsgBox(ResourcesPricing.GetString("SaveHeaderInfo"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                logger.Info(ResourcesPricing.GetString("SaveHeaderInfo Start"))
                SaveHeaderInfo()
                logger.Info(ResourcesPricing.GetString("SaveHeaderInfo End"))
            End If
        End If
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdMaintain_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMaintain.Click

        logger.Debug("cmdMaintain_Click Enter")

        Dim Index As Short = cmdMaintain.GetIndex(eventSender)

        Dim nTotalSelRows As Short
        Dim i As Short

        Select Case Index
            Case 0 'Add
                frmPricingBatchItemSearch = New frmPricingBatchItemSearch

                frmPricingBatchItemSearch.SetExistingBatchInfo(BatchHeader.PriceBatchHeaderId)

                If frmPricingBatchItemSearch.RunSearch Then
                    frmPricingBatchItemSearch.ShowDialog()
                    'Call LoadDataTable("EXEC GetPriceBatchDetail " & batchHeader.PriceBatchHeaderId)
                    Call LoadDataTable(BatchHeader.PriceBatchHeaderId)
                Else
                    frmPricingBatchItemSearch.Close()
                End If

            Case 1, 2 'Delete or Remove
                nTotalSelRows = ugrdList.Selected.Rows.Count

                If nTotalSelRows <= 0 Then
                    logger.Info(ResourcesIRMA.GetString("MustSelect"))
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                Else
                    Select Case Index
                        Case 1
                            If MsgBox(ResourcesPricing.GetString("DeletePriceChanges"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                                logger.Info(ResourcesIRMA.GetString("DeletePriceChanges"))
                                Exit Sub
                            End If
                        Case 2
                            If MsgBox(ResourcesPricing.GetString("RemoveItemsFromBatch"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                                logger.Info(ResourcesIRMA.GetString("RemoveItemsFromBatch"))
                                Exit Sub
                            End If
                    End Select
                End If

                For i = 0 To nTotalSelRows - 1
                    Select Case Index
                        Case 1
                            logger.Info("EXEC DeletePriceBatchDetail Start: PriceBatchDetailID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value.ToString)
                            SQLExecute("EXEC DeletePriceBatchDetail " & ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            logger.Info("EXEC DeletePriceBatchDetail End: PriceBatchDetailID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value.ToString)
                        Case 2
                            logger.Info("EXEC UpdatePriceBatchDetailCutHeader Start: PriceBatchDetailID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value.ToString)
                            SQLExecute("EXEC UpdatePriceBatchDetailCutHeader " & ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            logger.Info("EXEC UpdatePriceBatchDetailCutHeader End: PriceBatchDetailID=" + ugrdList.Selected.Rows(i).Cells("PriceBatchDetailID").Value.ToString)
                    End Select

                Next
                logger.Info("LoadDataTable Start " + BatchHeader.PriceBatchHeaderId.ToString)
                Call LoadDataTable(BatchHeader.PriceBatchHeaderId)
                logger.Info("LoadDataTable End " + BatchHeader.PriceBatchHeaderId.ToString)

        End Select

        logger.Debug("cmdMaintain_Click Exit")

    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrint.Click
        logger.Debug("cmdPrint_Click Enter")
        Dim iCopies As Short

        ' check to ensure only one batch was selected.
        If ugrdList.Selected.Rows.Count = 0 Then
            MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If Len(txtField(4).Text) > 0 Then
            iCopies = CShort(txtField(4).Text)
        Else
            iCopies = 1
        End If

        'open child form to print signs
        logger.Info("CallPrintSignInfoForm Start")
        CallPrintSignInfoForm(isAutoSelectAllItems:=False, isHideChildForm:=False, byPassNoTagRules:=IgnoreNoTagLogic, isReprint:=True)
        logger.Info("CallPrintSignInfoForm End")

        logger.Debug("cmdPrint_Click Exit")
    End Sub

    ' calls child form: PricingPrintSignInfo
    ' takes in params that either auto select all items to be printed and/or hides the form from view;
    ' the form is auto-selected and hidden when the following InstanceDataFlags are true: BypassPrintShelfTags 
    ' BypassPrintShelfTags_PerformPrintLogic
    Public Sub CallPrintSignInfoForm(ByVal isAutoSelectAllItems As Boolean, ByVal isHideChildForm As Boolean, Optional ByVal byPassNoTagRules As Boolean = False, Optional ByVal isReprint As Boolean = False)
        logger.Debug("CallPrintSignInfoForm Enter: isAutoSelectAllItems=" + isAutoSelectAllItems.ToString + ", isHideChildForm=" + isHideChildForm.ToString)

        Dim lItem_Key() As Integer
        Dim i As Integer

        If isAutoSelectAllItems Then
            logger.Info("CallPrintSignInfoForm - Auto selecting all items on the grid for printing: row count=" + ugrdList.Rows.Count.ToString)

            ReDim lItem_Key(ugrdList.Rows.Count - 1)

            'select ALL items in grid
            For i = 0 To ugrdList.Rows.Count - 1
                lItem_Key(i) = ugrdList.Rows(i).Cells("Item_Key").Value
            Next
        Else
            'populate the itemkey array with user selected items
            logger.Info("CallPrintSignInfoForm - Populating selecting items on the grid for printing: selected row count=" + ugrdList.Selected.Rows.Count.ToString)
            ReDim lItem_Key(ugrdList.Selected.Rows.Count - 1)
            For i = 0 To ugrdList.Selected.Rows.Count - 1
                lItem_Key(i) = ugrdList.Selected.Rows(i).Cells("Item_Key").Value
            Next
        End If

        Try
            If SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
                logger.Info("SLAW is enabled - applying no-tag logic.")
                ApplyNoTagLogic(lItem_Key, byPassNoTagRules)
            Else
                logger.Info("SLAW is not enabled - skipping no-tag logic.")
            End If
        Catch ex As Exception
            logger.Info(String.Format("Unexpected error occurred while applying no-tag logic: {0}", ex.ToString()))
            logger.Info("Proceeding to send print batch requests for all items in the batch.")
        End Try

        ' If the region does manual tag printing and no-tag exclusions do exist...
        If isAutoSelectAllItems = False And isHideChildForm = False And Me.NoTagItemsExcluded Then
            MsgBox("Print batch requests were not sent to SLAW for all items in your selection.  Please check the no-tag exclusion report for more detail.", MsgBoxStyle.Information, Me.Text)
        End If

        If lItem_Key.Length > 0 Then
            'send this pricebatchheaderid and itemlist to the summary form to figure out how many items per label type
            frmPricingPrintSignInfo.SetBatchHeaderInfo(lItem_Key, "|", _batchHeader)

            'hide form from view if flag = true
            If isHideChildForm Then
                frmPricingPrintSignInfo.Hide()

                'auto click the 'Print' button
                logger.Info("CallPrintSignInfoForm - Auto clicking the Print button on the hidden form")
                frmPricingPrintSignInfo.PerformTagFileWriterPrinting()
                frmPricingPrintSignInfo.SendSlawPrintBatchMessage(isReprint)
                _noTagLogicWasIgnored = (frmPricingPrintSignInfo.FailedPrintRequests.Count = 0)
                frmPricingPrintSignInfo.Close()
            Else
                'display form
                logger.Info("CallPrintSignInfoForm - Displaying the print form")
                frmPricingPrintSignInfo.ShowDialog()
            End If

            _noTagLogicWasIgnored = (frmPricingPrintSignInfo.NotCancelled) And (frmPricingPrintSignInfo.FailedPrintRequests.Count = 0)
            frmPricingPrintSignInfo.Dispose()
        End If

        logger.Debug("CallPrintSignInfoForm Exit")
    End Sub

    Private Sub ApplyNoTagLogic(ByRef lItem_Key() As Integer, Optional ByVal byPassNoTagRules As Boolean = False)
        Dim noTagDataAccess As New NoTagDataAccess()
        Dim noTagRules As New List(Of INoTagRule)
        Dim movementHistoryRule As New MovementHistoryRule(noTagDataAccess)
        Dim orderingHistoryRule As New OrderingHistoryRule(noTagDataAccess)
        Dim receivingHistoryRule As New ReceivingHistoryRule(noTagDataAccess)

        noTagRules.Add(movementHistoryRule)
        noTagRules.Add(orderingHistoryRule)
        noTagRules.Add(receivingHistoryRule)

        Dim noTagLogic As New PriceBatchNoTagLogic(noTagRules, noTagDataAccess, lItem_Key, _batchHeader)
        noTagLogic.ApplyNoTagLogic(byPassNoTagRules)

        Me._noTagLogicWasIgnored = byPassNoTagRules
        Me._noTagItemsExcluded = noTagLogic.ItemsExcluded
        lItem_Key = lItem_Key.Where(Function(i) Not noTagLogic.ExcludedItems.Contains(i)).ToArray()
    End Sub

    Private Sub frmPricingBatchDetail_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        logger.Debug("frmPricingBatchDetail_FormClosed Enter")
        BatchHeader = Nothing
        mdv.Dispose()
        logger.Debug("frmPricingBatchDetail_FormClosed Exit")
    End Sub

    Private Sub optPrintSign_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optPrintSign.CheckedChanged
        logger.Debug("optPrintSign_CheckedChanged Enter")
        If IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = optPrintSign.GetIndex(eventSender)
            Select Case Index
                Case 0
                    mdv.RowFilter = "PrintSign = 1"
                Case 1
                    mdv.RowFilter = "PrintSign = 0"
            End Select

        End If
        logger.Debug("optPrintSign_CheckedChanged Exit")
    End Sub

    Private Sub SaveHeaderInfo()
        logger.Debug("SaveHeaderInfo Enter")
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        ' Update the header object with the latest UI changes
        _batchHeader.BatchDescription = ConvertQuotes(BatchDescriptionTextBox.Text)
        _batchHeader.AutoApplyFlag = AutoApplyCheckBox.Checked
        _batchHeader.AutoApplyDate = AutoApplyDateUDTE.Value
        _batchHeader.POSBatchId = PosBatchIdTextBox.Text

        ' Validate the header data
        If CDate(AutoApplyDateUDTE.Value) < CDate(DateTime.Today) Then
            logger.Info(ResourcesItemBulkLoad.GetString("ApplyDateNotPast"))
            MsgBox(ResourcesItemBulkLoad.GetString("ApplyDateNotPast"), MsgBoxStyle.Critical, Me.Text)
            AutoApplyDateUDTE.Focus()
            Exit Sub
        End If

        ' validate the POS batch id format
        Dim priceBatchHeader As New PriceBatchHeaderBO
        Dim batchIdStatus As PriceBatchHeaderStatus = priceBatchHeader.ValidatePOSBatchId(PosBatchIdTextBox.Text, _batchHeader.StoreNumber)
        Select Case batchIdStatus
            Case PriceBatchHeaderStatus.Error_POSBatchId_Numeric
                logger.Info(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), PosBatchIdLabel.Text))
                MsgBox(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), PosBatchIdLabel.Text), MsgBoxStyle.Critical, Me.Text)
                PosBatchIdTextBox.Focus()
                Exit Sub
            Case PriceBatchHeaderStatus.Error_POSBatchId_MinRange
                logger.Info(String.Format(ResourcesCommon.GetString("msg_POSBatchIdMin"), priceBatchHeader.MinBatchIdAllowed, _batchHeader.StoreNumber))
                MsgBox(String.Format(ResourcesPricing.GetString("msg_POSBatchIdMin"), priceBatchHeader.MinBatchIdAllowed, _batchHeader.StoreNumber), MsgBoxStyle.Critical, Me.Text)
                PosBatchIdTextBox.Focus()
                Exit Sub
            Case PriceBatchHeaderStatus.Error_POSBatchId_MaxRange
                logger.Info(String.Format(ResourcesCommon.GetString("msg_POSBatchIdMax"), priceBatchHeader.MaxBatchIdAllowed, _batchHeader.StoreNumber))
                MsgBox(String.Format(ResourcesPricing.GetString("msg_POSBatchIdMax"), priceBatchHeader.MaxBatchIdAllowed, _batchHeader.StoreNumber), MsgBoxStyle.Critical, Me.Text)
                PosBatchIdTextBox.Focus()
                Exit Sub
        End Select

        ' Save the changes to the db
        logger.Info("PriceBatchDetailDAO.UpdatePriceBatchHeader Start: PriceBatchHeaderID=" + _batchHeader.PriceBatchHeaderId.ToString)
        PriceBatchDetailDAO.UpdatePriceBatchHeader(_batchHeader)
        logger.Info("PriceBatchDetailDAO.UpdatePriceBatchHeader End: PriceBatchHeaderID=" + _batchHeader.PriceBatchHeaderId.ToString)
        isHeaderInfoChanged = False
        _isBatchInfoRefreshed = True

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        logger.Debug("SaveHeaderInfo Exit")
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        logger.Debug("cmdSubmit_Click Enter")
        If isHeaderInfoChanged = True Then SaveHeaderInfo()
        logger.Debug("cmdSubmit_Click Exit")
    End Sub

    Private Sub AutoApplyCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoApplyCheckBox.CheckedChanged
        logger.Debug("AutoApplyCheckBox_CheckedChanged Enter")
        If IsLoading Then Exit Sub
        isHeaderInfoChanged = True
        AutoApplyDateUDTE.Enabled = (Not AutoApplyCheckBox.Checked)
        logger.Debug("AutoApplyCheckBox_CheckedChanged Exit")
    End Sub

    Private Sub AutoApplyDateUDTE_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoApplyDateUDTE.ValueChanged
        logger.Debug("AutoApplyDateUDTE_ValueChanged Enter")
        If AutoApplyDateUDTE.Value = Nothing Then AutoApplyDateUDTE.Value = Now.Date
        If IsLoading Or IsInitializing Then Exit Sub
        isHeaderInfoChanged = True
        logger.Debug("AutoApplyDateUDTE_ValueChanged Exit")
    End Sub

    Private Sub BatchDescriptionTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BatchDescriptionTextBox.TextChanged
        logger.Debug("BatchDescriptionTextBox_TextChanged Enter")
        If IsLoading Then Exit Sub
        isHeaderInfoChanged = True
        logger.Debug("BatchDescriptionTextBox_TextChanged Exit")
    End Sub

    Private Sub PosBatchIdTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PosBatchIdTextBox.TextChanged
        logger.Debug("PosBatchIdTextBox_TextChanged Enter")
        If IsLoading Then Exit Sub
        isHeaderInfoChanged = True
        logger.Debug("PosBatchIdTextBox_TextChanged Exit")
    End Sub

    Private Sub frmPricingBatchItemSearch_UpdateCallingForm(ByVal updatedBatchHeader As PriceBatchHeaderBO) Handles frmPricingBatchItemSearch.UpdateCallingForm
        'update batch data -- header info changed
        logger.Debug("frmPricingBatchItemSearch_UpdateCallingForm Enter")
        BatchDescriptionTextBox.Text = updatedBatchHeader.BatchDescription
        PosBatchIdTextBox.Text = updatedBatchHeader.POSBatchId
        AutoApplyCheckBox.Checked = updatedBatchHeader.AutoApplyFlag
        If updatedBatchHeader.AutoApplyDate <> Nothing Then AutoApplyDateUDTE.Value = updatedBatchHeader.AutoApplyDate
        logger.Debug("frmPricingBatchItemSearch_UpdateCallingForm Exit")
    End Sub

    Private Sub cmdMarkAsPrinted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMarkAsPrinted.Click
        logger.Debug("cmdMarkAsPrinted_Click Enter")
        MarkAsPrinted()
        logger.Debug("cmdMarkAsPrinted_Click Exit")
    End Sub

    Public Sub MarkAsPrinted()
        logger.Debug("MarkAsPrinted Enter")

        'This sets the batch to status 4, PRINTED.
        'This block should be placed on a "Mark as Printed" event rather than prior to actually creating the signs.
        If (BatchHeader.PriceBatchStatusID = 3) And gbPriceBatchProcessor Then
            Dim headerDAO As New PriceBatchHeaderDAO
            Dim header As New PriceBatchHeaderBO
            header.PriceBatchHeaderId = BatchHeader.PriceBatchHeaderId
            header.PriceBatchStatusID = 4

            'update PriceBatchHeader.PriceBatchStatusID
            headerDAO.UpdatePriceBatchStatus(header)

            BatchHeader.PriceBatchStatusID = 4
        End If

        logger.Debug("MarkAsPrinted Exit")
    End Sub
End Class
