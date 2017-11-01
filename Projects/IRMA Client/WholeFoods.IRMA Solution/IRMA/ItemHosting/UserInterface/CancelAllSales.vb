Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess

Public Class CancelAllSales

    Private _itemBO As ItemBO

    Private _userLockDate As String
    Private IsInitializing As Boolean
    Private mbFilling As Boolean

#Region "properties"

    Public Property ItemBO() As ItemBO
        Get
            Return _itemBO
        End Get
        Set(ByVal value As ItemBO)
            _itemBO = value
        End Set
    End Property

#End Region

    Private Sub CancelAllSales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()
        LockItem()
        SetupStoreSelectionGrid()

        If Not CheckAllStoreSelectionEnabled() Then
            RadioButton_All.Text = "All 365"
        End If
    End Sub

    ''' <summary>
    ''' put a LOCK on this item for the current user
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LockItem()
        'gRSRecordset = SQLOpenRecordSet("EXEC LockItem " & _itemBO.Item_Key & "," & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        'If CType(gRSRecordset.Fields("User_ID").Value, Integer) <> giUserID Then
        '    MsgBox(String.Format(ResourcesItemHosting.GetString("ItemLocked"), gRSRecordset.Fields("FullName").Value, gRSRecordset.Fields("User_ID_Date").Value), MsgBoxStyle.Critical, Me.Text)
        '    gRSRecordset.Close()
        '    gRSRecordset = Nothing
        '    Me.Close()
        '    Exit Sub
        'Else
        '    _userLockDate = gRSRecordset.Fields("User_ID_Date").Value.ToString
        'End If
        'gRSRecordset.Close()
        'gRSRecordset = Nothing

        Dim itemDAO As New ItemDAO
        Dim dsItemLock As DataSet = itemDAO.LockItem(_itemBO.Item_Key, giUserID)

        If CType(dsItemLock.Tables(0).Rows(0)("User_ID"), Integer) <> giUserID Then
            MsgBox(String.Format(ResourcesItemHosting.GetString("ItemLocked"), dsItemLock.Tables(0).Rows(0)("FullName"), dsItemLock.Tables(0).Rows(0)("User_ID_Date")), MsgBoxStyle.Critical, Me.Text)
        Else
            _userLockDate = dsItemLock.Tables(0).Rows(0)("User_ID_Date").ToString
        End If
    End Sub

    Private Sub SetupStoreSelectionGrid()
        '-- Fill out the store list
        Dim mdtStores As DataTable = WholeFoods.IRMA.ItemHosting.DataAccess.StoreDAO.GetRetailStoreList
        ugrdStoreList.DataSource = mdtStores

        DisableRowsWithGPMStore(mdtStores.Rows.Count)

        'load zone drop down
        LoadZone(cmbZones)

        'load state drop down
        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

        Call SetCombos()
    End Sub

    Private Sub DisableRowsWithGPMStore(ByVal rowcount As Integer)
        For currentRowNumber As Integer = 0 To rowcount - 1
            If ugrdStoreList.Rows(currentRowNumber).Cells("IsGPMStore").Value Then
                ugrdStoreList.Rows(currentRowNumber).Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled
            End If
        Next
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim adjustSaleData As New AdjustSaleDataBO
        Dim statusList As ArrayList
        Dim currentStatus As AdjustSaleDataStatus
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim storeList As New StringBuilder
        Dim row As UltraGridRow

        'set non-UI values required for save
        adjustSaleData.ItemKey = _itemBO.Item_Key
        adjustSaleData.StoreListSeparator = "|"c
        adjustSaleData.UserID = giUserID
        'adjustSaleData.UserIDDate = Date.Parse(userLockDate)
        adjustSaleData.UserIDDate = _userLockDate

        'get user entries from form        
        adjustSaleData.StartDate = CType(Me.dtpStartDate.Value, Date)

        'get list of selected stores
        If Me.ugrdStoreList.Selected.Rows.Count = 0 Then
        Else
            For Each row In Me.ugrdStoreList.Selected.Rows
                'Verify the item/store combination is currently on sale in IRMA.  If not, remove the store from the selected list and
                'notify the user.
                Dim itemOnSaleAtStore As Boolean = AdjustSaleDataDAO.IsItemOnSaleOrSalePendingForStore(adjustSaleData.ItemKey, CInt(row.Cells("Store_No").Value))

                If itemOnSaleAtStore Then
                    storeList.Append(row.Cells("Store_No").Value.ToString)
                    storeList.Append(adjustSaleData.StoreListSeparator)
                Else
                    MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_warning_CancelSales_NotOnSaleOrPendingSale"), row.Cells("Store_Name").Value.ToString.TrimEnd(" "c)), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    row.Selected = False
                End If
            Next
        End If
        'remove last separator from store list and set value to vendorDeal object
        If storeList.ToString.Length > 0 Then
            adjustSaleData.StoreList = storeList.ToString.Substring(0, storeList.ToString.Length - 1)
        End If

        'validate current set of data
        statusList = adjustSaleData.ValidateCancelAllSales

        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, AdjustSaleDataStatus)

            Select Case currentStatus
                Case AdjustSaleDataStatus.Error_StartDate_PastDate
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_startDateInFuture"), Me.Label_StartDate.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case AdjustSaleDataStatus.Error_Required_StoreList
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.GroupBox_StoreSel.Text))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            Dim result As DialogResult

            'perform final validations
            '1. Check to see if there are any pending price change batches for the Item/Store selected in the Building state.  
            '   If there are, the user will be warned that the pending changes will be removed from the batch.
            Dim batchesInBuildingStatus As Boolean = AdjustSaleDataDAO.CheckForPendingBatches(adjustSaleData, BatchStatus.Building)

            If batchesInBuildingStatus Then
                MessageBox.Show(ResourcesItemHosting.GetString("msg_warning_CancelSales_BatchesInBuildingStatus"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            '2. Check to see if there are any pending price change batches for the Item/Store selected in the Packaged, Ready, Printed, or Sent states.  
            '   If there are, the user will receive an error that they cannot cancel the sale for the store until these batches are Processed. 
            Dim batchesInPendingStatus As Boolean = AdjustSaleDataDAO.CheckForPendingBatches(adjustSaleData, BatchStatus.AllButProcessedAndBuilding)

            If batchesInPendingStatus Then
                'the user can not proceed with saving this change
                MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_error_AdjustSale_UnprocessedBatches"), ResourcesItemHosting.GetString("msg_text_CancelAllSales")), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                success = True
            Else
                'the user can proceed
                'prompt user with final warning that they are about to cancel all sales for the selected stores
                result = MessageBox.Show(ResourcesItemHosting.GetString("msg_confirm_CancelAllSales"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    'save data
                    Dim adjustSaleDAO As New AdjustSaleDataDAO

                    Try
                        adjustSaleDAO.CancelAllSales(adjustSaleData)
                        success = True
                    Catch ex As Exception
                        success = False
                        MessageBox.Show(ResourcesCommon.GetString("msg_dbError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Else
                    success = True
                End If
            End If
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

    Private Sub SetCombos()
        mbFilling = True

        'Zones.
        If Me.RadioButton_Zone.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If Me.RadioButton_State.Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False
    End Sub

#Region "Event Handlers"

    ''' <summary>
    ''' exit form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' save data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ApplyChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ApplyChanges.Click
        'save changes
        If ApplyChanges() Then
            'close this form is successful in saving
            Me.Hide()
        End If
    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        If IsInitializing Or mbFilling Then Exit Sub

        mbFilling = True

        Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False
    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged
        If mbFilling Or IsInitializing Then Exit Sub

        Me.RadioButton_Zone_CheckedChanged(Me.RadioButton_Zone, New System.EventArgs)
    End Sub

    Private Sub RadioButton_Manual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Manual.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- Manual.
            cmbZones.SelectedIndex = -1
            cmbStates.SelectedIndex = -1

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_All_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_All.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- All Stores or All 365 for RM
            If CheckAllStoreSelectionEnabled() Then
                StoreListGridSelectAll(ugrdStoreList, True)
            Else
                StoreListGridSelectAll365(ugrdStoreList)
            End If

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_AllWFM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_AllWFM.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- All WFM.
            Call StoreListGridSelectAllWFM(ugrdStoreList)

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_Zone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Zone.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- By Zone
            If cmbZones.SelectedIndex > -1 Then Call StoreListGridSelectByZone(ugrdStoreList, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))

            mbFilling = False
        End If
    End Sub

    Private Sub RadioButton_State_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_State.CheckedChanged
        If Me.IsInitializing Or mbFilling Then Exit Sub
        If CType(sender, RadioButton).Checked Then
            Call SetCombos()

            mbFilling = True

            ugrdStoreList.Selected.Rows.Clear()

            '-- By State.
            If cmbStates.SelectedIndex > -1 Then Call StoreListGridSelectByState(ugrdStoreList, VB6.GetItemData(cmbStates, cmbStates.SelectedIndex).ToString)

            mbFilling = False
        End If
    End Sub

#End Region

End Class