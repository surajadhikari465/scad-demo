Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic

Public Class Form_ManagePOSWriter

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Form to edit a single POSWriter.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editPOSWriterForm As Form_EditPOSWriter

    ''' <summary>
    ''' Form to add a single POSWriter.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents addPOSWriterForm As Form_AddFileWriter

    ''' <summary>
    ''' Form to disable a single POSWriter.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents deletePOSWriterForm As Form_DeletePOSWriter
#End Region

#Region "Events handled by this form"
#Region "Load Form"
    ''' <summary>
    ''' Load the form, querying the database to populate the list of configured file writers.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_POSWriterFileConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Populate the data grid
        PopulatePOSWriterData()
    End Sub

#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the writer table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub addPOSWriterDataForm_UpdateCallingForm() Handles addPOSWriterForm.UpdateCallingForm
        ' Refresh the data grid
        PopulatePOSWriterData()
    End Sub

    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the writer table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editPOSWriterDataForm_UpdateCallingForm() Handles editPOSWriterForm.UpdateCallingForm
        ' Refresh the data grid
        PopulatePOSWriterData()
    End Sub

    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the writer table.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeletePOSWriterDataForm_UpdateCallingForm() Handles deletePOSWriterForm.UpdateCallingForm
        ' Refresh the data grid
        PopulatePOSWriterData()
    End Sub
#End Region

#Region "Add Writer events"
    ''' <summary>
    ''' Process the add writer events
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessAddWriter()
        Logger.LogDebug("ProcessAddWriter entry", Me.GetType())
        Try
            ' Bring focus to the form
            addPOSWriterForm = New Form_AddFileWriter()

            ' Show the form
            addPOSWriterForm.ShowDialog(Me)
            addPOSWriterForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessAddWriter exception when getting type=Form_AddFileWriter", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessAddWriter exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the File Writer -> Add File Writer menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AddFileWriterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFileWriterToolStripMenuItem.Click
        Logger.LogDebug("AddFileWriterToolStripMenuItem_Click entry", Me.GetType())
        ProcessAddWriter()
        Logger.LogDebug("AddFileWriterToolStripMenuItem_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the Add writer button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Logger.LogDebug("Button_Add_Click entry", Me.GetType())
        ProcessAddWriter()
        Logger.LogDebug("Button_Add_Click exit", Me.GetType())
    End Sub
#End Region

#Region "Edit Writer events"
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

    ''' <summary>
    ''' Process the edit writer events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessEditWriter()
        Logger.LogDebug("ProcessEditWriter entry", Me.GetType())
        Try
            ' Bring focus to the form
            editPOSWriterForm = New Form_EditPOSWriter()
            editPOSWriterForm.CurrentAction = FormAction.Edit
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                ' Populate the edit form with the values from the selected row
                editPOSWriterForm.InputWriterConfig = New POSWriterBO(selectedRow)
                ' Show the form
                editPOSWriterForm.ShowDialog(Me)
            End If
            editPOSWriterForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessEditWriter exception when getting type=Form_EditPOSWriter", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessEditWriter exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the File Writer -> Edit Selected File Writer menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditSelectedFileWriterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditSelectedFileWriterToolStripMenuItem.Click
        Logger.LogDebug("EditSelectedFileWriterToolStripMenuItem_Click entry", Me.GetType())
        ProcessEditWriter()
        Logger.LogDebug("EditSelectedFileWriterToolStripMenuItem_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user double clicked a row in the writer table.
    ''' This is the same as selecting the File Writer -> Edit Selected File Writer menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_ConfigItems.CellDoubleClick
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
        ProcessEditWriter()
        Logger.LogDebug("DataGridView_ConfigItems_CellDoubleClick", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Edit writer button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
        ProcessEditWriter()
        Logger.LogDebug("Button_Edit_Click", Me.GetType())
    End Sub
#End Region

#Region "Delete/Disable Writer Events"
    ''' <summary>
    ''' Process the delete writer events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessDeleteWriter()
        Logger.LogDebug("ProcessDeleteWriter entry", Me.GetType())
        Try
            ' Bring focus to the form
            deletePOSWriterForm = New Form_DeletePOSWriter()
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(DataGridView_ConfigItems)
            If selectedRow IsNot Nothing Then
                ' Populate the delete form with the values from the selected row
                deletePOSWriterForm.WriterConfig = New POSWriterBO(selectedRow)
                ' Show the form
                deletePOSWriterForm.ShowDialog(Me)
            End If
            deletePOSWriterForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessDeleteWriter exception when getting type=Form_DeletePOSWriter", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessDeleteWriter exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' User selected the File Writer -> Disable Selected File Writer menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteSelectedFileWriterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisableSelectedFileWriterToolStripMenuItem.Click
        Logger.LogDebug("DeleteSelectedStoreToolStripMenuItem_Click entry", Me.GetType())
        ProcessDeleteWriter()
        Logger.LogDebug("DeleteSelectedStoreToolStripMenuItem_Click exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Delete writer button.
    ''' The delete actually just disables the writer so the configurations are not permanently deleted.
    ''' There is not a UI to re-enable the writer, but it can be done by a database admin.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Logger.LogDebug("Button_Delete_Click entry", Me.GetType())
        ProcessDeleteWriter()
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
    Private Sub Form_POSWriterConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' No additional processing is needed because data can't be changed on this form
    End Sub
#End Region

#End Region

#Region "Populate and format DataGridView objects for the form"
    ''' <summary>
    ''' Populate the DataGridView_ConfigItems with the current data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulatePOSWriterData()
        Try
            ' Read the POSWriter data
            Dim _dataSet As DataSet = POSWriterDAO.GetFileWriters(Nothing)

            DataGridView_ConfigItems.DataSource = _dataSet.Tables(0)
            DataGridView_ConfigItems.MultiSelect = False

            ' Format the view
            ' Make sure at least one entry was returned before configuring the columns
            If (DataGridView_ConfigItems.Columns.Count > 0) Then
                DataGridView_ConfigItems.Columns("POSFileWriterKey").Visible = False
                DataGridView_ConfigItems.Columns("Disabled").Visible = False
                DataGridView_ConfigItems.Columns("EscapeCharCount").Visible = False
                DataGridView_ConfigItems.Columns("AppendToFile").Visible = False
                DataGridView_ConfigItems.Columns("DelimChar").Visible = False
                DataGridView_ConfigItems.Columns("LeadingDelim").Visible = False
                DataGridView_ConfigItems.Columns("TrailingDelim").Visible = False
                DataGridView_ConfigItems.Columns("FieldIdDelim").Visible = False
                DataGridView_ConfigItems.Columns("TaxFlagTrueChar").Visible = False
                DataGridView_ConfigItems.Columns("TaxFlagFalseChar").Visible = False
                DataGridView_ConfigItems.Columns("EnforceDictionary").Visible = False
                DataGridView_ConfigItems.Columns("ScaleWriterType").Visible = False
                DataGridView_ConfigItems.Columns("OutputByIrmaBatches").Visible = False
                DataGridView_ConfigItems.Columns("BatchIdMin").Visible = False
                DataGridView_ConfigItems.Columns("BatchIdMax").Visible = False

                DataGridView_ConfigItems.Columns("POSFileWriterCode").DisplayIndex = 0
                DataGridView_ConfigItems.Columns("POSFileWriterCode").HeaderText = ResourcesAdministration.GetString("label_fileWriter")
                DataGridView_ConfigItems.Columns("POSFileWriterCode").ReadOnly = True

                DataGridView_ConfigItems.Columns("POSFileWriterClass").DisplayIndex = 1
                DataGridView_ConfigItems.Columns("POSFileWriterClass").HeaderText = ResourcesAdministration.GetString("label_writerClass")
                DataGridView_ConfigItems.Columns("POSFileWriterClass").ReadOnly = True

                DataGridView_ConfigItems.Columns("FixedWidth").DisplayIndex = 2
                DataGridView_ConfigItems.Columns("FixedWidth").HeaderText = ResourcesAdministration.GetString("label_fixedWidth")
                DataGridView_ConfigItems.Columns("FixedWidth").ReadOnly = True

                DataGridView_ConfigItems.Columns("FileWriterType").DisplayIndex = 3
                DataGridView_ConfigItems.Columns("FileWriterType").HeaderText = ResourcesAdministration.GetString("label_writerType")
                DataGridView_ConfigItems.Columns("FileWriterType").ReadOnly = True

                DataGridView_ConfigItems.Columns("ScaleWriterTypeDesc").DisplayIndex = 4
                DataGridView_ConfigItems.Columns("ScaleWriterTypeDesc").HeaderText = ResourcesAdministration.GetString("label_scaleWriterType")
                DataGridView_ConfigItems.Columns("ScaleWriterTypeDesc").ReadOnly = True

                DataGridView_ConfigItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If
        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), e)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_POSWriterConfig form: PopulatePOSWriterData sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub
#End Region

End Class
