Option Explicit On
Option Strict On

Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic

Public Class Form_ManageStorePOSConfig

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Form to create or edit a single StorePOSConfig.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editStorePOSConfigForm As Form_EditStorePOSConfig

    ''' <summary>
    ''' Form to delete a single StorePOSConfig.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents deleteStorePOSConfigForm As Form_DeleteStorePOSConfig
#End Region

#Region "Events handled by this form"
    ''' <summary>
    ''' This function reads the selected row from a data grid.
    ''' The row can be selected by highlighting the entire row or a single cell
    ''' within the row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSelectedRow(ByRef dataGrid As DataGridView) As DataGridViewRow
        ' Get the selected row
        Dim selectedRow As DataGridViewRow = Nothing
        If (dataGrid.SelectedRows.Count = 1) Then
            Dim rowEnum As IEnumerator = dataGrid.SelectedRows.GetEnumerator
            rowEnum.MoveNext()
            selectedRow = CType(rowEnum.Current, DataGridViewRow)
        ElseIf (dataGrid.SelectedCells.Count = 1) Then
            Dim cellEnum As IEnumerator = dataGrid.SelectedCells.GetEnumerator
            cellEnum.MoveNext()
            Dim selectedCell As DataGridViewCell = CType(cellEnum.Current, DataGridViewCell)
            selectedRow = selectedCell.OwningRow
        Else
            ' Error condition
            DisplayErrorMessage("A row must be selected to perform this action.")
        End If
        Return selectedRow
    End Function

#Region "Load Form"
    ''' <summary>
    ''' Load the form, querying the database to populate the list of configured stores.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_StorePOSConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()
        ' Populate the data grid
        PopulateStorePOSConfig()
    End Sub

#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the store configurations.  Refresh the store table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editStorePOSConfigForm_UpdateCallingForm() Handles editStorePOSConfigForm.UpdateCallingForm
        ' Refresh the data grid
        PopulateStorePOSConfig()
    End Sub

    ''' <summary>
    ''' Changes were made to the store configurations.  Refresh the store table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteStorePOSConfigForm_UpdateCallingForm() Handles deleteStorePOSConfigForm.UpdateCallingForm
        ' Refresh the data grid
        PopulateStorePOSConfig()
    End Sub
#End Region

#Region "Add Store events"
    ''' <summary>
    ''' Process the add store events
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessAddStore()
        Logger.LogDebug("ProcessAddStore entry", Me.GetType())
        Try
            ' Bring focus to the form
            editStorePOSConfigForm = New Form_EditStorePOSConfig()
            editStorePOSConfigForm.CurrentAction = FormAction.Create
            ' Show the form
            editStorePOSConfigForm.ShowDialog(Me)
            editStorePOSConfigForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessAddStore exception when getting type=Form_EditStorePOSConfig", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessAddStore exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the Store -> Add Store menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AddStoreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStoreToolStripMenuItem.Click
        Logger.LogDebug("AddStoreToolStripMenuItem_Click entry", Me.GetType())
        ProcessAddStore()
        Logger.LogDebug("AddStoreToolStripMenuItem_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the Add store button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Logger.LogDebug("Button_Add_Click entry", Me.GetType())
        ProcessAddStore()
        Logger.LogDebug("Button_Add_Click exit", Me.GetType())
    End Sub

#End Region

#Region "Edit Store events"
    ''' <summary>
    ''' Process the edit store events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessEditStore()
        Try
            ' Bring focus to the form
            editStorePOSConfigForm = New Form_EditStorePOSConfig()
            editStorePOSConfigForm.CurrentAction = FormAction.Edit
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                ' Populate the edit form with the values from the selected row
                editStorePOSConfigForm.StorePOSConfig = New StorePOSConfigBO(selectedRow)
                ' Show the form
                editStorePOSConfigForm.ShowDialog(Me)
                editStorePOSConfigForm.Dispose()
            End If
        Catch ex As Exception
            Logger.LogError("ProcessEditStore exception when getting type=Form_EditStorePOSConfig", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
    End Sub

    ''' <summary>
    ''' User selected the Store -> Edit Selected Store menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditSelectedStoreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditSelectedStoreToolStripMenuItem.Click
        Logger.LogDebug("EditSelectedStoreToolStripMenuItem_Click entry", Me.GetType())
        ProcessEditStore()
        Logger.LogDebug("EditSelectedStoreToolStripMenuItem_Click exit", Me.GetType())
    End Sub


    ''' <summary>
    ''' The user double clicked a row in the store table.
    ''' This is the same as selecting the Store -> Edit Selected Store menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ConfigItems.CellDoubleClick
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
        ProcessEditStore()
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Edit store button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
        ProcessEditStore()
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
    End Sub
#End Region

#Region "Delete Store events"
    ''' <summary>
    ''' Process the delete store events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessDeleteStore()
        Logger.LogDebug("ProcessDeleteStore entry", Me.GetType())
        Try
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                ' Populate the delete form with the values from the selected row
                deleteStorePOSConfigForm = New Form_DeleteStorePOSConfig()
                deleteStorePOSConfigForm.StorePOSConfig = New StorePOSConfigBO(selectedRow)
                ' Show the form
                deleteStorePOSConfigForm.ShowDialog(Me)
                deleteStorePOSConfigForm.Dispose()
            End If
        Catch ex As Exception
            Logger.LogError("ProcessDeleteStore exception when getting type=Form_DeleteStorePOSConfig", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessDeleteStore exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the Store -> Delete Selected Store menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteSelectedStoreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteSelectedStoreToolStripMenuItem.Click
        Logger.LogDebug("DeleteSelectedStoreToolStripMenuItem_Click entry", Me.GetType())
        ProcessDeleteStore()
        Logger.LogDebug("DeleteSelectedStoreToolStripMenuItem_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Delete store button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Logger.LogDebug("Button_Delete_Click entry", Me.GetType())
        ProcessDeleteStore()
        Logger.LogDebug("Button_Delete_Click exit", Me.GetType())
    End Sub
#End Region

#Region "Close button"
    ''' <summary>
    ''' The close button returns the user to the calling form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' The user clicked the 'X' button in top-right of window to close the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_StorePOSConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' No additional processing is needed because data can't be changed on this form
    End Sub

#End Region
#End Region

#Region "Populate and format DataGridView objects for the form"
    ''' <summary>
    ''' Populate the DataGridView_ConfigItems with the current data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateStorePOSConfig()
        Try
            ' Read the StorePOSConfig data
            Dim _dataSet As DataSet = StoreWriterConfigDAO.GetStorePOSConfigurations()

            DataGridView_ConfigItems.DataSource = _dataSet.Tables(0)
            DataGridView_ConfigItems.MultiSelect = False

            ' Format the view
            ' Make sure at least one entry was returned before configuring the columns
            If (DataGridView_ConfigItems.Columns.Count > 0) Then
                DataGridView_ConfigItems.Columns("POSFileWriterKey").Visible = False
                DataGridView_ConfigItems.Columns("POSFileWriterClass").Visible = False
                DataGridView_ConfigItems.Columns("ScaleFileWriterKey").Visible = False
                DataGridView_ConfigItems.Columns("ScaleFileWriterClass").Visible = False
                DataGridView_ConfigItems.Columns("ConfigType").Visible = False
               
                DataGridView_ConfigItems.Columns("Store_No").DisplayIndex = 0
                DataGridView_ConfigItems.Columns("Store_No").HeaderText = ResourcesAdministration.GetString("label_storeNo")
                DataGridView_ConfigItems.Columns("Store_No").ReadOnly = True

                DataGridView_ConfigItems.Columns("Store_Name").DisplayIndex = 1
                DataGridView_ConfigItems.Columns("Store_Name").HeaderText = ResourcesAdministration.GetString("label_storeName")
                DataGridView_ConfigItems.Columns("Store_Name").ReadOnly = True

                DataGridView_ConfigItems.Columns("POSFileWriterCode").DisplayIndex = 2
                DataGridView_ConfigItems.Columns("POSFileWriterCode").HeaderText = ResourcesAdministration.GetString("label_fileWriter")
                DataGridView_ConfigItems.Columns("POSFileWriterCode").ReadOnly = True

                DataGridView_ConfigItems.Columns("ScaleFileWriterCode").DisplayIndex = 3
                DataGridView_ConfigItems.Columns("ScaleFileWriterCode").HeaderText = ResourcesAdministration.GetString("label_scaleWriter")
                DataGridView_ConfigItems.Columns("ScaleFileWriterCode").ReadOnly = True

                DataGridView_ConfigItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If
        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), e)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_StorePOSConfig form: PopulateStorePOSConfig sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub
#End Region

End Class
