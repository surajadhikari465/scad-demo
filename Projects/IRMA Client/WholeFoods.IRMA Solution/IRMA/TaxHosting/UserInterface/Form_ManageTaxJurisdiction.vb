Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess

Public Class Form_ManageTaxJurisdiction

    Dim _dataSet As DataSet
    Dim _isNewRow As Boolean

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "form events"

    ''' <summary>
    ''' Pre-populate the form with the existing tax jurisdiction values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxJurisdiction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load data to form control
        BindData()
        'format data control
        FormatDataGrid()
    End Sub

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxJurisdiction_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _dataSet.HasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                SaveChanges()
            End If
        End If

        ' Raise event - allows the data on the parent form to be refreshed
        RaiseEvent UpdateCallingForm()
    End Sub

    ''' <summary>
    ''' Add new Jurisdiction to grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).AddNew()
    End Sub

    ''' <summary>
    ''' Delete Jurisdiction from grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Dim selectedRowIndex As Integer

        Try
            'get selected row
            If Me.UltraGrid_TaxJurisdiction.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_TaxJurisdiction.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_TaxJurisdiction.ActiveRow.Index
            End If
        Catch ex As Exception
            'user is trying to delete AddRow - prompt to select a valid row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Question)
            Return
        End Try

        'delete row if user has selected a row that is not the NewRow
        Dim currentRow As UltraGridRow = Me.UltraGrid_TaxJurisdiction.Rows(selectedRowIndex)
        Dim taxJurisdiction As New TaxJurisdictionBO(currentRow)

        'validate if DELETE can happen
        Select Case taxJurisdiction.ValidateDelete()
            Case TaxJurisdictionStatus.Error_StoresAssociated
                MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_deleteJurisdiction_store"), currentRow.Cells("StoreCount").Value), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Case TaxJurisdictionStatus.Warning_TaxFlagsAssociated
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_deleteJurisdiction_taxFlag"), currentRow.Cells("TaxFlagCount").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Windows.Forms.DialogResult.Yes Then
                    'remove item from grid
                    currentRow.Delete()
                End If
            Case TaxJurisdictionStatus.Valid
                'confirm user wishes to delete this item
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_deleteConfirmation"), currentRow.Cells("TaxJurisdictionDesc").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = Windows.Forms.DialogResult.Yes Then
                    'remove item from grid
                    currentRow.Delete()
                End If
        End Select

    End Sub

    ''' <summary>
    ''' sets flag to indicate that item being updated is actually a NEW item in the AddNew box
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_TaxJurisdiction_BeforeRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs) Handles UltraGrid_TaxJurisdiction.BeforeRowInsert
        _isNewRow = True
    End Sub

    ''' <summary>
    ''' exectues every time a row is updated.  this checks to see if the value being updated to is a duplicate
    ''' of an existing item in the dataSet.  if they are duplicates, the update is cancelled an the user is informed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_TaxJurisdiction_BeforeRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles UltraGrid_TaxJurisdiction.BeforeRowUpdate
        'validate that item just added is not a duplicate of existing item
        Dim updatedRowIndex As Integer = e.Row.Index
        Dim updatedValue As String

        'check that updated value contains text
        If (e.Row.Cells("TaxJurisdictionDesc").Value IsNot DBNull.Value) And (e.Row.Cells("TaxJurisdictionDesc").Value.ToString.Trim <> "") Then
            updatedValue = CType(e.Row.Cells("TaxJurisdictionDesc").Value, String)

            Dim rows As DataRowCollection = _dataSet.Tables(0).Rows
            Dim rowsEnum As IEnumerator = rows.GetEnumerator()
            Dim currentRow As DataRow
            Dim currentRowIndex As Integer
            Dim itemExistsCount As Integer

            If rowsEnum IsNot Nothing Then
                While rowsEnum.MoveNext
                    currentRow = CType(rowsEnum.Current, DataRow)

                    Try
                        'check that updatedValue does not match currentRow value
                        If updatedValue.Equals(_dataSet.Tables(0).Rows(currentRowIndex).Item("TaxJurisdictionDesc").ToString) Then
                            'track how many times item exists
                            itemExistsCount += 1
                        End If
                    Catch ex As DeletedRowInaccessibleException
                        'ignore deleted rows from dataset
                    End Try

                    currentRowIndex += 1
                End While
            End If

            'item exists count will be 2 if item is being updated and is the same as an existing item;
            'the count will only be 1 if a new item is being added that already exists, because the new 
            'item will not have been evaluated as part of the existing dataSet data
            If itemExistsCount > 1 Or (_isNewRow AndAlso itemExistsCount = 1) Then
                'item already exists: cancel update
                e.Cancel = True
                'inform user
                MessageBox.Show(ResourcesTaxHosting.GetString("msg_duplicateItem"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            'prompt user to enter a valid value and cancel update
            e.Cancel = True
            MessageBox.Show(ResourcesTaxHosting.GetString("msg_updateItemBlank"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        'reset _isNewRow flag
        _isNewRow = False
    End Sub

    ''' <summary>
    ''' called just before row is deleted by UltraGrid control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_TaxJurisdiction_BeforeRowsDeleted(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs) Handles UltraGrid_TaxJurisdiction.BeforeRowsDeleted
        ' Stop the grid from displaying its default message box.
        e.DisplayPromptMsg = False
    End Sub

    ''' <summary>
    ''' Exit form w/o saving any changes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Save changes (if any) to database
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        SaveChanges()
        Me.Close()
    End Sub

#End Region

    ''' <summary>
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        ' Read the tax jurisdiction definitions
        Dim dao As New TaxJurisdictionDAO
        _dataSet = dao.GetJurisdictions()

        Me.UltraGrid_TaxJurisdiction.DataSource = _dataSet.Tables(0)

        If Me.UltraGrid_TaxJurisdiction.Rows.Count > 0 Then
            'first row appears to be defaulted because it is highlighted, but it's not actually selected.
            'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
            'below code is actually selecting row that is already highlighted.
            Me.UltraGrid_TaxJurisdiction.Rows(0).Selected = True
        End If
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        ' Format the view
        If UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns.Count > 0 Then
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("TaxJurisdictionId").Hidden = True
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("StoreCount").Hidden = True
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("TaxFlagCount").Hidden = True
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("TaxJurisdictionDesc").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxJurisdiction")

            'limit tax jurisdiction description to 30 chars
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("TaxJurisdictionDesc").MaxLength = 30

            ' Limit regional jurisdiction ID to 15 chars
            UltraGrid_TaxJurisdiction.DisplayLayout.Bands(0).Columns("RegionalJurisdictionID").MaxLength = 15
        End If
    End Sub

    ''' <summary>
    ''' Saves changes to DataSet to database
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveChanges()
        ' save the changes to the database
        If _dataSet.HasChanges() Then
            Try
                Dim dao As New TaxJurisdictionDAO
                dao.SaveJurisdictions(_dataSet)
            Catch ex As DBConcurrencyException
                'TODO message to user about concurrency exception
                BindData()
                MessageBox.Show(ResourcesCommon.GetString("msg_concurrencyError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex2 As Exception
                'TODO inform user this operation failed
            End Try
        End If
    End Sub

End Class