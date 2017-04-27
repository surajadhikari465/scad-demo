Option Strict Off
Option Explicit On
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinDataSource
Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmVendorPOPaymentTypeSettings
    Inherits System.Windows.Forms.Form

    Private rsVendorStorePOPaymentSettings As DAO.Recordset
    Private formAndGridHeightDiff As Integer

    Private changesSaved As Boolean = False

    ' These are the ugrid columns.
    Public Enum GridColumns
        StoreNumber
        StoreName
        IsPayAgreedCost
        EffectiveDate
        HasChanged
        ClearChanges
        Index
        OriginalIsPayAgreedCost
        OriginalEffectiveDate
    End Enum

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub frmVendorPOPaymentTypeSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not changesSaved And changesExist() And (gbSuperUser Or gbVendorAdministrator) Then
            If saveBeforeExit() Then
                saveChanges()
            End If
        End If
    End Sub


    Private Sub frmVendorPOPaymentTypeSettings_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmVendorPOPaymentStoreOverride Entry")
        Me.MinimumSize = Me.Size
        Me.formAndGridHeightDiff = Me.Height - UltraGrid1.Height

        Dim blnEnableEdit As Boolean = False
        'If gbBuyer Or gbFacilityCreditProcessor Then blnEnableEdit = False
        If gbSuperUser Or gbVendorAdministrator Then blnEnableEdit = True

        Me.btnSetAllPayAgreed.Enabled = blnEnableEdit
        Me.btnSetAllPayInvoice.Enabled = blnEnableEdit
        Me.btnSaveAndExit.Enabled = blnEnableEdit

        Me.UltraDataSource1.Band.Columns(GridColumns.IsPayAgreedCost).ReadOnly = Not blnEnableEdit
        Me.UltraDataSource1.Band.Columns(GridColumns.EffectiveDate).ReadOnly = Not blnEnableEdit

        CenterForm(Me)
        PopGrid()

        logger.Debug("frmVendorPOPaymentStoreOverride Exit")
    End Sub
    Private Sub PopGrid(Optional ByRef bRefreshData As Boolean = True)
        logger.Debug("PopGrid Entry")
        Dim iLoop As Integer
        Dim bAddRow As Boolean

        If bRefreshData Then
            If Not (rsVendorStorePOPaymentSettings Is Nothing) Then
                On Error Resume Next
                rsVendorStorePOPaymentSettings.Close()
                On Error GoTo 0
            End If

            SQLOpenRS(rsVendorStorePOPaymentSettings, "EXEC GetIsPayAgreedCostVendorAllStores " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        UltraDataSource1.Rows.Clear()

        If Not (rsVendorStorePOPaymentSettings.EOF And rsVendorStorePOPaymentSettings.BOF) Then
            rsVendorStorePOPaymentSettings.MoveLast()
            UltraDataSource1.Rows.SetCount(rsVendorStorePOPaymentSettings.RecordCount)
            rsVendorStorePOPaymentSettings.MoveFirst()
        End If

        While Not rsVendorStorePOPaymentSettings.EOF
            iLoop = iLoop + 1
            PopGridRow(rsVendorStorePOPaymentSettings.Fields, iLoop)
            rsVendorStorePOPaymentSettings.MoveNext()
        End While

        rsVendorStorePOPaymentSettings.Close()
        logger.Debug("PopGrid Exit")
    End Sub
    Private Sub PopGridRow(ByVal rsFields As DAO.Fields, ByVal index As Integer)
        logger.Debug("PopGridRow Entry")
        Dim row As UltraDataRow

        'Get the first row.
        row = Me.UltraDataSource1.Rows(index - 1)
        row(GridColumns.StoreNumber) = rsFields("Store_No").Value
        row(GridColumns.StoreName) = rsFields("Store_Name").Value
        row(GridColumns.IsPayAgreedCost) = rsFields("isPayAgreedCost").Value
        row(GridColumns.OriginalIsPayAgreedCost) = rsFields("isPayAgreedCost").Value
        row(GridColumns.EffectiveDate) = rsFields("EffectiveDate").Value
        row(GridColumns.OriginalEffectiveDate) = rsFields("EffectiveDate").Value
        row(GridColumns.Index) = rsFields("Index").Value
        logger.Debug("PopGridRow Exit")
    End Sub


    Private Sub UltraGrid1_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid1.AfterCellUpdate
        logger.Debug("UltraGrid1_AfterCellUpdate Entry")

        ' Get hidden index value from the row that was changed.  This indexes into the data source because the grid row index can change via UI sorting.
        Dim dataRowIndex As Integer = CInt(e.Cell.Row.Cells.Item(GridColumns.Index).Value)

        ' Clear changes if 
        If UltraGrid1.ActiveCell.Column.Index = GridColumns.ClearChanges Then
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.IsPayAgreedCost) = Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.OriginalIsPayAgreedCost)
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.EffectiveDate) = Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.OriginalEffectiveDate)
        End If
        ' [Update Rules]
        ' We toggle the has-changed box if:
        ' 1) pay agreed cost is changed to value other than original value.
        ' 2) pay agreed cost is TRUE and date is changed to value other than original value.
        '
        ' 
        If Not isPayAgreedCost(e.Cell.Row) Then
            ' Clear date, since it's invalid for non-pay-agreed-cost setting.
            ' This is the value that should come in from the data source for non-pay-agreed-cost.
            Dim row As UltraDataRow = Me.UltraDataSource1.Rows(dataRowIndex)
            row(GridColumns.EffectiveDate) = DBNull.Value
        End If
        If Not (hasPayAgreedCostChanged(e.Cell.Row) Or hasEffectiveDateChanged(e.Cell.Row)) Then
            ' No changes, so clear has-changed (FALSE) and show clear-changes as 'cleared' (TRUE).
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.HasChanged) = False
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.ClearChanges) = True
        Else
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.HasChanged) = True
            Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.ClearChanges) = False
            If hasPayAgreedCostChanged(e.Cell.Row) Then
                ' Set effective date to today, if it has not changed.
                If Not hasEffectiveDateChanged(e.Cell.Row) Then
                    Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.EffectiveDate) = Date.Today
                End If
            End If
        End If

        logger.Debug("UltraGrid1_AfterCellUpdate Exit")
    End Sub

    Private Sub UltraGrid1_AfterEnterEditMode(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterEnterEditMode
        ' Date can't be changed if pay-agreed is false.
        If UltraGrid1.ActiveCell.Column.Index = GridColumns.EffectiveDate And Not isPayAgreedCost(UltraGrid1.ActiveCell.Row) Then
            UltraGrid1.ActiveCell.CancelUpdate()
            MessageBox.Show("Effective date is only valid when activating pay-by-agreed-cost.", "Change Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.EffectiveDate) = Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.OriginalEffectiveDate)
            'UltraGrid1.ActiveCell.Value = UltraGrid1.ActiveCell.OriginalValue
            Exit Sub
        End If

        If UltraGrid1.ActiveCell.Column.Index = GridColumns.ClearChanges And areChangesCleared(UltraGrid1.ActiveCell.Row) Then
            UltraGrid1.ActiveCell.CancelUpdate()
            MessageBox.Show("There are no changes to clear for this store.", "No Action To Perform", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.EffectiveDate) = Me.UltraDataSource1.Rows(dataRowIndex).Item(GridColumns.OriginalEffectiveDate)
            'UltraGrid1.ActiveCell.Value = UltraGrid1.ActiveCell.OriginalValue
            Exit Sub
        End If
    End Sub

    Private Sub UltraGrid1_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid1.InitializeLayout
        e.Layout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.AllRowsInBand
    End Sub
    Private Function areChangesCleared(ByVal row As Infragistics.Win.UltraWinGrid.UltraGridRow) As Boolean
        Return Boolean.Equals(row.Cells.Item(GridColumns.ClearChanges).Value, True)
    End Function
    Private Function hasPayAgreedCostChanged(ByVal row As Infragistics.Win.UltraWinGrid.UltraGridRow) As Boolean
        'MsgBox("agreed '" & row.Cells.Item(GridColumns.IsPayAgreedCost).Value.ToString & "'='" & row.Cells.Item(GridColumns.OriginalIsPayAgreedCost).Value.ToString & "' -> " _
        '& Boolean.Equals(row.Cells.Item(GridColumns.IsPayAgreedCost).Value, row.Cells.Item(GridColumns.OriginalIsPayAgreedCost).Value))
        Return Not Boolean.Equals(row.Cells.Item(GridColumns.IsPayAgreedCost).Value, row.Cells.Item(GridColumns.OriginalIsPayAgreedCost).Value)
    End Function
    Private Function isPayAgreedCost(ByVal row As Infragistics.Win.UltraWinGrid.UltraGridRow) As Boolean
        Return Boolean.Equals(row.Cells.Item(GridColumns.IsPayAgreedCost).Value, True)
    End Function
    Private Function hasEffectiveDateChanged(ByVal row As Infragistics.Win.UltraWinGrid.UltraGridRow) As Boolean
        'MsgBox("effective '" & row.Cells.Item(GridColumns.EffectiveDate).Value.ToString & "'='" & row.Cells.Item(GridColumns.OriginalEffectiveDate).Value.ToString & "' -> " _
        '& row.Cells.Item(GridColumns.EffectiveDate).Value.ToString.Equals(row.Cells.Item(GridColumns.OriginalEffectiveDate).Value.ToString))
        Dim effectiveDate As Date
        If TypeOf row.Cells.Item(GridColumns.EffectiveDate).Value Is DBNull Then
            effectiveDate = Date.Today
        Else
            effectiveDate = row.Cells.Item(GridColumns.EffectiveDate).Value
        End If
        Dim originalEffectiveDate As Date
        If TypeOf row.Cells.Item(GridColumns.OriginalEffectiveDate).Value Is DBNull Then
            originalEffectiveDate = Date.Today
        Else
            originalEffectiveDate = row.Cells.Item(GridColumns.OriginalEffectiveDate).Value
        End If

        Return effectiveDate.Year <> originalEffectiveDate.Year Or effectiveDate.Month <> originalEffectiveDate.Month Or effectiveDate.Day <> originalEffectiveDate.Day
        'Return Not row.Cells.Item(GridColumns.EffectiveDate).Value.ToString.Equals(row.Cells.Item(GridColumns.OriginalEffectiveDate).Value.ToString)
        'Date.Equals(row.Cells.Item(GridColumns.EffectiveDate).Value, row.Cells.Item(GridColumns.OriginalEffectiveDate).Value)
    End Function
    Private Sub frmVendorPOPaymentTypeSettings_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        'UltraGrid1.Height = Me.Height - formAndGridHeightDiff
    End Sub

    Private Sub btnSetAllPayAgreed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAllPayAgreed.Click
        For Each row As UltraDataRow In UltraDataSource1.Rows
            row(GridColumns.IsPayAgreedCost) = True
            row(GridColumns.EffectiveDate) = setAllPayAgreedCostDateTimePicker.Value
        Next
        For Each row As UltraGridRow In UltraGrid1.Rows
            Dim cell As UltraGridCell = row.Cells.Item(GridColumns.IsPayAgreedCost)
            cell.Activate()
            UltraGrid1_AfterCellUpdate(UltraGrid1, New Infragistics.Win.UltraWinGrid.CellEventArgs(cell))
        Next

    End Sub

    Private Sub btnSetAllPayInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAllPayInvoice.Click
        For Each row As UltraDataRow In UltraDataSource1.Rows
            row(GridColumns.IsPayAgreedCost) = False
        Next
        For Each row As UltraGridRow In UltraGrid1.Rows
            Dim cell As UltraGridCell = row.Cells.Item(GridColumns.IsPayAgreedCost)
            cell.Activate()
            ' This will clear effective date.
            UltraGrid1_AfterCellUpdate(UltraGrid1, New Infragistics.Win.UltraWinGrid.CellEventArgs(cell))
        Next
    End Sub

    Private Sub btnExitWithoutSaving_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExitWithoutSaving.Click
        ' User is prompted during form-closing sub.
        Me.Close()
    End Sub

    Private Sub btnSaveAndExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAndExit.Click
        saveChanges()
        Me.Close()
    End Sub
    Private Sub saveChanges()
        changesSaved = True
        Dim row As UltraDataRow
        Dim rowsChanged() As Integer = getDataSourceRowsChanged()
        Dim rowCount As Integer
        If rowsChanged Is Nothing Then
            rowCount = 0
        Else
            rowCount = rowsChanged.Length
        End If
        Dim successCount As Integer = 0
        For Each rowIndex As Integer In rowsChanged
            row = UltraDataSource1.Rows.Item(rowIndex)
            Dim deleteFlag As Boolean = False
            ' If pay-agreed-cost was cleared by user, we need to delete the row.
            If Boolean.Equals(row(GridColumns.IsPayAgreedCost), False) Then
                deleteFlag = True
            Else
                Do While frmVendor.txtAccountingContactEmail.Text = ""
                    VendorContactEmail.ShowDialog()
                Loop
            End If
            logger.Info("PO payment type change: storeName=" & row(GridColumns.StoreName) & "storeNumber=" & row(GridColumns.StoreNumber) & ", vendor_id=" & glVendorID & ", effectiveDate=" & row(GridColumns.EffectiveDate) & ", delete=" & deleteFlag)
            Try
                'MsgBox("row changed: store=" & row(GridColumns.StoreName) & ", payAgreed=" & row(GridColumns.IsPayAgreedCost))
                ' Can't pass DBNull in effective date.  It should only be null if being deleted, so we give it a default if this is a delete call.
                Dim effectiveDate As Date
                If deleteFlag Then
                    effectiveDate = Date.Today
                Else
                    effectiveDate = row(GridColumns.EffectiveDate)
                End If
                If VendorStorePayAgreedCostDAO.updatePayAgreedCostSetup(row(GridColumns.StoreNumber), glVendorID, effectiveDate, deleteFlag) Then
                    successCount += 1
                End If
            Catch ex As Exception
                logger.Error("Error updating vendor/store pay-agreed-cost setup: store=" & row(GridColumns.StoreName) & ", payAgreed=" & row(GridColumns.IsPayAgreedCost) & ", effectiveDate=" & row(GridColumns.EffectiveDate) & vbCrLf & ex.ToString)
            End Try
        Next
        If successCount <> rowCount Then
            Dim rowCountMsg As String = " updates, "
            Dim successCountMsg As String = " were"
            If rowCount = 1 Then
                rowCountMsg = " update, "
            End If
            If successCount = 1 Then
                successCountMsg = " was"
            End If
            MessageBox.Show("Of the " & rowCount & rowCountMsg & successCount & successCountMsg & " saved successfully.", _
                "Data Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
    Private Function changesExist() As Boolean
        For Each row As UltraDataRow In UltraDataSource1.Rows
            If row(GridColumns.HasChanged) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Function getDataSourceRowsChanged() As Integer()
        logger.Debug("getDataSourceRowsChanged Entry")

        Dim rowsChanged(UltraDataSource1.Rows.Count) As Integer
        Dim index As Integer = -1
        For Each row As UltraDataRow In UltraDataSource1.Rows
            If row(GridColumns.HasChanged) Then
                index += 1
                rowsChanged(index) = row(GridColumns.Index)
            End If
        Next
        If index = -1 Then
            rowsChanged = Nothing
        End If
        ReDim Preserve rowsChanged(index)
        logger.Info("getDataSourceRowsChanged: array index=" & index)
        logger.Debug("getDataSourceRowsChanged Exit")
        Return rowsChanged
    End Function
    Private Function saveBeforeExit() As Boolean
        ' If there are no changes, we don't need to prompt.
        'Dim rowsChanged() As Integer = getDataSourceRowsChanged()
        'If rowsChanged Is Nothing Then
        'Return False
        'End If
        Return MessageBox.Show("All changes will be lost.  Do you want to save your changes first?", _
            "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes
    End Function

    Private Sub lblHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblHelp.Click
        MessageBox.Show(getHelp, "PO Payment Type Setting Screen - Usage Notes", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Shared Function getHelp() As String
        Return "- To set a store as 'Pay Agreed Cost' (for this vendor), check the 'Pay Agreed Cost' checkbox in that row." _
        & vbCrLf & vbCrLf & "- To set a store as 'Pay Invoice Cost' (for this vendor), uncheck the 'Pay Agreed Cost' checkbox in that row." _
        & vbCrLf & vbCrLf & "- You can set all stores as pay-agreed or pay-invoice using the buttons at the top of the screen, then manually update any stores that don't apply to the global setting." _
        & vbCrLf & vbCrLf & "- The effective date can be changed at any time for 'Pay Agreed Cost' stores.  Example: Store enabled yesterday, effective next Monday, effective date needs to be next Tuesday.  Update just the effective date (you do not have to cycle through disable/enable again)." _
        & vbCrLf & vbCrLf & "- If any changes are made for a store, the 'Has Changed' checkbox in that row is checked.  This is simply an indicator so you know what stores have changed." _
        & vbCrLf & vbCrLf & "- You can clear changes for a store by clicking the 'Clear Changes' checkbox in that row." _
        & vbCrLf & vbCrLf & "- Save your changes by clicking the 'Save And Exit' button." _
        & vbCrLf & vbCrLf & "- You will be prompted to save unsaved changes before exiting." _
        & vbCrLf & vbCrLf & "- Saving changes in this screen does not save any changes made in any other vendor setup screens." _
        & vbCrLf & vbCrLf & "- You can sort the items in the list by clicking on a column header, such as ‘Store Name’." _
        & vbCrLf & vbCrLf & "- The effective date, when a store is enabled for ‘Pay Agreed Cost’, is a pure date and does not include a timestamp.  Therefore, the store (for the specified vendor) is live at 12AM on the effective date." _
        & vbCrLf & vbCrLf & "- Changes to a vendor-store’s PO payment type that are effective today or prior will update the payment type for all POs not yet sent for that vendor and store."
    End Function
End Class