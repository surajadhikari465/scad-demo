Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess

Public Class Form_ManageTaxOverride

    Dim WithEvents taxOverrideValueForm As Form_ManageTaxOverrideValue
    Private deleteFlagList As New ArrayList
    Public _itemKey As Integer
    Public _storeNo As Integer
    Private _taxFlagValues As Hashtable

#Region "property definitions"

    Public Property ItemKey() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _storeNo
        End Get
        Set(ByVal value As Integer)
            _storeNo = value
        End Set
    End Property

#End Region

#Region "form events"

    ''' <summary>
    ''' Pre-populate the form with the existing tax flag values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxOverride_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load data to form control
        BindData()
    End Sub

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxOverride_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If deleteFlagList.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show(ResourcesTaxHosting.GetString("msg_confirmDelete"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                SaveDeleteChanges()
            End If
        End If
    End Sub

    Private Sub taxOverrideValueForm_UpdateCallingForm(ByVal newKey As String) Handles taxOverrideValueForm.UpdateCallingForm
        'remove newKey value from list of deleted items: handles the event where a user deletes an item and then re-adds it before saving
        Dim deleteEnum As IEnumerator = deleteFlagList.GetEnumerator
        Dim currentFlag As TaxOverrideBO
        Dim flagCount As Integer = 0
        Dim deleteFlagIndex As Integer = -1

        While deleteEnum.MoveNext
            flagCount += 1
            currentFlag = CType(deleteEnum.Current, TaxOverrideBO)

            If currentFlag.TaxFlagKey.Equals(newKey) Then
                'capture current index of matching flag key
                deleteFlagIndex = flagCount - 1
                Exit While
            End If
        End While

        'remove flag from delete list
        If deleteFlagIndex <> -1 Then
            deleteFlagList.RemoveAt(deleteFlagIndex)
        End If

        'rebind data grid
        BindData()
    End Sub

    Private Sub Button_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        taxOverrideValueForm = New Form_ManageTaxOverrideValue
        taxOverrideValueForm.StoreNo = Me.StoreNo
        taxOverrideValueForm.ItemKey = Me.ItemKey
        taxOverrideValueForm.IsEdit = False
        taxOverrideValueForm.ExistingTaxFlagValues = _taxFlagValues

        taxOverrideValueForm.ShowDialog(Me)
        taxOverrideValueForm.Dispose()
    End Sub

    Private Sub Button_Edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        UpdateRow(True)
    End Sub

    Private Sub Button_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        UpdateRow(False)
    End Sub

    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        Me.Close()
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

#End Region

    ''' <summary>
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        Dim taxOverrideBO As New TaxOverrideBO
        Dim existingTaxFlags As New Hashtable
        Dim taxFlagRowsEnum As IEnumerator = Me.UltraGrid_TaxOverride.Rows.GetEnumerator
        Dim row As UltraGridRow

        taxOverrideBO.ItemKey = Me.ItemKey
        taxOverrideBO.StoreNo = Me.StoreNo

        ' setup tax flag datagrid
        Dim taxOverrideDAO As New TaxOverrideDAO
        Me.UltraGrid_TaxOverride.DataSource = taxOverrideDAO.GetTaxOverrideList(taxOverrideBO, deleteFlagList)

        'populate class level hashtable w/ existing tax flag values; this will be passed to child form to determine if 
        'an added/edited tax flag already exists
        _taxFlagValues = New Hashtable
        While taxFlagRowsEnum.MoveNext
            row = CType(taxFlagRowsEnum.Current, UltraGridRow)
            _taxFlagValues.Add(row.Cells("TaxFlagKey").Value.ToString, Nothing)
        End While

        If Me.UltraGrid_TaxOverride.Rows.Count > 0 Then
            'first row appears to be defaulted because it is highlighted, but it's not actually selected.
            'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
            'below code is actually selecting row that is already highlighted.
            Me.UltraGrid_TaxOverride.Rows(0).Selected = True
        End If

        'format data control
        FormatDataGrid()
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        If UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("StoreNo").Hidden = True
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("ItemKey").Hidden = True
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("StoreName").Hidden = True

            'sort columns in correct order
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.VisiblePosition = 0
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.VisiblePosition = 1

            'set column names
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagKey")
            UltraGrid_TaxOverride.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagValue")
        End If
    End Sub

    Public Sub UpdateRow(ByVal isEdit As Boolean)
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Dim taxOverrideDAO As New TaxOverrideDAO
        Dim taxOverrideBO As New TaxOverrideBO

        Try
            'get selected row
            If Me.UltraGrid_TaxOverride.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_TaxOverride.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_TaxOverride.ActiveRow.Index
            End If

            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        'delete row if user has selected a row that is not the NewRow
        If isRowSelected Then
            Dim currentRow As UltraGridRow = Me.UltraGrid_TaxOverride.Rows(selectedRowIndex)

            'perform edit or delete operation based on flag passed in
            If isEdit Then
                taxOverrideValueForm = New Form_ManageTaxOverrideValue

                'set form values for selected TaxFlag
                taxOverrideValueForm.IsEdit = True
                taxOverrideValueForm.ItemKey = Me.ItemKey
                taxOverrideValueForm.StoreNo = Me.StoreNo
                taxOverrideValueForm.ExistingTaxFlagValues = _taxFlagValues

                taxOverrideValueForm.EditTaxFlagKey = currentRow.Cells("TaxFlagKey").Value.ToString

                If CType(currentRow.Cells("TaxFlagValue").Value, Boolean) Then
                    taxOverrideValueForm.RadioButton_TaxFlagValueYes.Checked = True
                Else
                    taxOverrideValueForm.RadioButton_TaxFlagValueNo.Checked = True
                End If

                'make tax flag key read only on edit and set focus to tax percent
                taxOverrideValueForm.ComboBox_TaxFlag.Enabled = False

                'open and bring focus to new form
                taxOverrideValueForm.ShowDialog(Me)
                taxOverrideValueForm.Dispose()
            Else
                'confirm user wishes to delete this item
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_deleteConfirmation"), currentRow.Cells("TaxFlagKey").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    'don't delete tax override flags now - track which to delete and save to DB 
                    'when user clicks 'OK' or closes form
                    taxOverrideBO.ItemKey = Me.ItemKey
                    taxOverrideBO.StoreNo = Me.StoreNo
                    taxOverrideBO.TaxFlagKey = currentRow.Cells("TaxFlagKey").Value.ToString

                    deleteFlagList.Add(taxOverrideBO)

                    'update grid to reflect deleted tax override flag
                    BindData()
                End If
            End If
        Else
            'prompt user to select a row
            If isEdit Then
                MessageBox.Show(ResourcesCommon.GetString("msg_selectEditRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Else
                MessageBox.Show(ResourcesCommon.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
            End If
        End If
    End Sub

    Private Sub SaveDeleteChanges()
        Dim flagEnum As IEnumerator
        Dim currentFlag As TaxOverrideBO
        Dim taxOverrideDAO As New TaxOverrideDAO
        Dim transaction As SqlTransaction = Nothing

        'delete all tax flags
        flagEnum = deleteFlagList.GetEnumerator

        Try
            'create transaction to share connection for all flags to delete
            transaction = taxOverrideDAO.GetTransaction

            'loop through each flag and delete them
            While flagEnum.MoveNext
                currentFlag = CType(flagEnum.Current, TaxOverrideBO)

                'delete current tax override flag

                taxOverrideDAO.DeleteData(currentFlag, transaction)
            End While

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
        End Try
    End Sub

End Class