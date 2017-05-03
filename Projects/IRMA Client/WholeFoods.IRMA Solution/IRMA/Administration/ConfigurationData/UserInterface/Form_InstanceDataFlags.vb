Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_InstanceDataFlags

    Private _isInitializing As Boolean
    Private _deleteOverrideList As New ArrayList
    Private _dataChanged As Boolean
    Private _overrideDataChanged As Boolean
    Private _activeRow As Integer

    Public WithEvents _addOverrideForm As Form_AddInstanceDataStoreOverrides

    Private Sub Form_InstanceDataFlags_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _isInitializing = True

        _dataChanged = False

        _overrideDataChanged = False

        _activeRow = -1

        ApplyToggle(False)

        'reset delete list
        _deleteOverrideList.Clear()

        BindData()
        BindStoreOverrideGrid()
    End Sub

    Private Sub Form_InstanceDataFlags_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'check if user has made changes that need to be saved
        If _deleteOverrideList.Count > 0 Or _dataChanged Or _overrideDataChanged Then
            'prompt user if they'd like to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesAdministration.GetString("msg_confirmInstanceDataFlagSaveChanges"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                ApplyChanges()
            End If
        End If
    End Sub

    Private Sub addOverrideForm_UpdateCallingForm(ByVal addedStores As ArrayList) Handles _addOverrideForm.UpdateCallingForm
        Dim deletedFlagsEnum As IEnumerator
        Dim addedFlagsEnum As IEnumerator
        Dim currentDeletedFlag As InstanceDataFlagsBO
        Dim currentAddedFlag As InstanceDataFlagsBO
        Dim currentDeleteIndex As Integer
        Dim deleteIndexList As New ArrayList

        'remove added stores from the deleted list if any exist
        deletedFlagsEnum = _deleteOverrideList.GetEnumerator
        addedFlagsEnum = addedStores.GetEnumerator

        While deletedFlagsEnum.MoveNext
            currentDeletedFlag = CType(deletedFlagsEnum.Current, InstanceDataFlagsBO)

            'only deal with deleted flags for the current selected FlagKey
            If currentDeletedFlag.FlagKey Is Me.ugInstanceDataFlags.ActiveRow.Cells("FlagKey").Value Then
                'check all added stores for this flag
                While addedFlagsEnum.MoveNext
                    currentAddedFlag = CType(addedFlagsEnum.Current, InstanceDataFlagsBO)

                    If currentDeletedFlag.StoreNo = currentAddedFlag.StoreNo Then
                        'remove this store/flag combo from the deleted list; must add index to a list so that 
                        'the _deleteOverrideList enumeration is not altered as it's being stepped through
                        deleteIndexList.Add(currentDeleteIndex)

                        'jump to next deleted item
                        Exit While
                    End If
                End While
            End If

            currentDeleteIndex += 1
        End While

        'remove all deleted indexes contained in the deleteIndexList
        For currentDeleteIndex = 0 To deleteIndexList.Count - 1
            _deleteOverrideList.RemoveAt(CType(deleteIndexList(currentDeleteIndex), Integer))
        Next

        're-bind data grid with added values
        BindStoreOverrideGrid()
    End Sub

    Private Sub ApplyToggle(ByVal enabled As Boolean)
        Button_OK.Enabled = enabled
    End Sub

    Private Sub BindData()
        Me.CenterToParent()

        Me.ugInstanceDataFlags.DataSource = InstanceDataDAO.GetAllInstanceDataFlags
        Me.ugInstanceDataFlags.DisplayLayout.Bands(0).Columns("FlagKey").CellActivation = Activation.NoEdit

        Me.ugInstanceDataFlags.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
        Me.ugInstanceDataFlags.DisplayLayout.Bands(0).Columns("FlagKey").Width = 200
        Me.ugInstanceDataFlags.Rows(0).Activate()
        _isInitializing = False
    End Sub

    Private Sub BindStoreOverrideGrid()
        Dim selectedflagkey As String = Me.ugInstanceDataFlags.ActiveRow.Cells("FlagKey").Value
        Me.UltraGrid_StoreOverrides.DataSource = InstanceDataDAO.GetInstanceDataFlagsForKey(selectedFlagKey, _deleteOverrideList)

        'format columns
        ' Make sure at least one entry was returned before configuring the columns
        If UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("StoreNo").Hidden = True
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("FlagKey").Hidden = True

            'sort columns in correct order
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("StoreName").Header.VisiblePosition = 0
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("FlagValue").Header.VisiblePosition = 1

            'format columns
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("StoreName").Width = 250
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("StoreName").CellActivation = Activation.NoEdit
            UltraGrid_StoreOverrides.DisplayLayout.Bands(0).Columns("CanStoreOverride").Hidden = True
        End If

    End Sub

    Private Function SaveOverrideData(ByVal instanceDataFlagRow As Integer) As Boolean
        Dim currentFlag As InstanceDataFlagsBO
        Dim overrideList As New ArrayList
        Dim dataSaved As Boolean = False
        Dim instanceDAO As New InstanceDataDAO
        Dim transaction As SqlTransaction = Nothing

        Try
            transaction = instanceDAO.GetTransaction
            For Each ugRow As UltraGridRow In Me.UltraGrid_StoreOverrides.Rows
                currentFlag = New InstanceDataFlagsBO
                currentFlag.FlagKey = Me.ugInstanceDataFlags.Rows(instanceDataFlagRow).Cells("FlagKey").Value
                currentFlag.FlagValue = ugRow.Cells("FlagValue").Value
                currentFlag.StoreNo = ugRow.Cells("StoreNo").Value
                overrideList.Add(currentFlag)
            Next
            'call update and pass arraylist
            instanceDAO.UpdateInstanceDataFlagsStoreOverride(overrideList, transaction)
            dataSaved = True
            _overrideDataChanged = False
            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            'inform user
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_InstanceDataFlags form: SaveOverrideData function"
            ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, args, SeverityLevel.Warning)
        End Try

    End Function

    Private Function SaveInstanceDataFlags() As Boolean
        Dim currentFlag As InstanceDataFlagsBO
        Dim flagList As New ArrayList
        Dim dataSaved As Boolean = False
        Dim instanceDAO As New InstanceDataDAO
        Dim transaction As SqlTransaction = Nothing

        Try
            transaction = instanceDAO.GetTransaction
            For Each dRow As UltraGridRow In Me.ugInstanceDataFlags.Rows
                currentFlag = New InstanceDataFlagsBO
                currentFlag.FlagKey = dRow.Cells("FlagKey").Value
                currentFlag.FlagValue = dRow.Cells("FlagValue").Value
                currentFlag.CanStoreOverride = dRow.Cells("CanStoreOverride").Value
                flagList.Add(currentFlag)
            Next
            'call update and pass arraylist
            instanceDAO.UpdateInstanceDataFlagValues(flagList, transaction)
            dataSaved = True
            _dataChanged = False
            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()

            'inform user
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_InstanceDataFlags form: SaveInstanceDataFlags function"
            ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, args, SeverityLevel.Warning)
        End Try

    End Function
    Private Function ApplyChanges() As Boolean
        Dim success As Boolean = False
        Dim flagEnum As IEnumerator
        Dim currentFlag As InstanceDataFlagsBO
        Dim instanceDAO As New InstanceDataDAO
        Dim transaction As SqlTransaction = Nothing

        'delete all tax flags
        flagEnum = _deleteOverrideList.GetEnumerator
        Me.Cursor = Cursors.WaitCursor
        Try
            'create transaction to share connection for all flags to delete
            transaction = instanceDAO.GetTransaction

            'loop through each flag and delete them
            If _deleteOverrideList.Count > 0 Then
                While flagEnum.MoveNext
                    currentFlag = CType(flagEnum.Current, InstanceDataFlagsBO)

                    'delete current tax flag
                    instanceDAO.DeleteInstanceDataFlagsStoreOverride(currentFlag, transaction)
                End While
            End If

            If _overrideDataChanged Then
                'update storoverrides
                SaveOverrideData(_activeRow)
            End If

            If _dataChanged Then
                'update all instancedataflags
                SaveInstanceDataFlags()
            End If

            transaction.Commit()
            success = True

            'empty deleteOverrideList
            _deleteOverrideList.Clear()

            ApplyToggle(False)
        Catch ex As Exception
            transaction.Rollback()

            'inform user
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_InstanceDataFlags form: ApplyChanges function"
            ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, args, SeverityLevel.Warning)
        End Try
        Me.Cursor = Cursors.Default
        Return success
    End Function

#Region "button events"

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        _addOverrideForm = New Form_AddInstanceDataStoreOverrides
        _addOverrideForm.FlagKey = Me.ugInstanceDataFlags.ActiveRow.Cells("FlagKey").Value
        _addOverrideForm.RegionalFlagValue = Me.ugInstanceDataFlags.ActiveRow.Cells("FlagValue").Value
        _addOverrideForm.DeletedFlags = _deleteOverrideList

        _addOverrideForm.ShowDialog()
        _addOverrideForm.Dispose()
    End Sub

    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False

        Try
            'get selected row
            If Me.UltraGrid_StoreOverrides.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_StoreOverrides.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_StoreOverrides.ActiveRow.Index
            End If

            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        'delete row if user has selected a row that is not the NewRow
        If isRowSelected Then
            Dim currentRow As UltraGridRow = Me.UltraGrid_StoreOverrides.Rows(selectedRowIndex)

            Dim flagBO As New InstanceDataFlagsBO
            flagBO.FlagKey = Me.ugInstanceDataFlags.ActiveRow.Cells("FlagKey").Value
            flagBO.StoreNo = CType(currentRow.Cells("StoreNo").Value, Integer)
            flagBO.StoreName = currentRow.Cells("StoreName").Value.ToString

            'don't delete overrides now - track which to delete and save to DB when user clicks 'OK' or closes form
            _deleteOverrideList.Add(flagBO)

            'update grid to reflect deleted override
            BindStoreOverrideGrid()
        Else
            'prompt user to select a row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub


    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        ApplyChanges()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        'close form -- user will be prompted if they wish to save changes upon close
        Me.Close()
    End Sub

#End Region

#Region "Grid Events"

    Private Sub ugInstanceDataFlags_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugInstanceDataFlags.CellChange
        If e.Cell.DataChanged Then
            'flag save changes
            _dataChanged = True
            ApplyToggle(True)
        End If
    End Sub


    Private Sub ugInstanceDataFlags_ClickCell(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.ClickCellEventArgs) Handles ugInstanceDataFlags.ClickCell
        If _activeRow <> e.Cell.Row.Index Then

            If _overrideDataChanged Then
                Dim result As DialogResult = MessageBox.Show(ResourcesAdministration.GetString("msg_confirmStoreOverrideDataChanges"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    'need function to only update storeoverride data
                    Me.Cursor = Cursors.WaitCursor
                    SaveOverrideData(_activeRow)
                    ApplyToggle(_dataChanged)
                    Me.Cursor = Cursors.Default
                Else
                    _overrideDataChanged = False
                End If
            End If

            BindStoreOverrideGrid()
            _activeRow = e.Cell.Row.Index

        End If

        If e.Cell.DataChanged Then
            'flag save changes
            _dataChanged = True
            ApplyToggle(True)
        End If
    End Sub

    Private Sub UltraGrid_StoreOverrides_ClickCell(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.ClickCellEventArgs) Handles UltraGrid_StoreOverrides.ClickCell
        If e.Cell.DataChanged Then
            _overrideDataChanged = True
            ApplyToggle(True)
        End If
    End Sub

#End Region

End Class