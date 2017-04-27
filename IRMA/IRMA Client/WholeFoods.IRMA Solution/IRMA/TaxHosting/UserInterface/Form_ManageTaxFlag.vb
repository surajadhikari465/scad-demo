Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess
Imports WholeFoods.Utility

Public Class Form_ManageTaxFlag

    Dim WithEvents taxJurisdictionForm As Form_ManageTaxJurisdiction
    Dim WithEvents taxClassForm As Form_ManageTaxClass
    Dim WithEvents taxFlagValueForm As Form_ManageTaxFlagValue
    Private deleteFlagList As New ArrayList
    Private taxFlagValues As Hashtable


#Region "form events"

    ''' <summary>
    ''' Pre-populate the form with the existing tax flag values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxFlag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'reset delete list
        deleteFlagList.Clear()
        'load data to form control
        BindData()
    End Sub

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxFlag_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If deleteFlagList.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show(ResourcesTaxHosting.GetString("msg_confirmDelete"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                SaveDeleteChanges()
            End If
        End If
    End Sub

    Private Sub Button_AddJurisdiction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_AddJurisdiction.Click
        'open and bring focus to new form
        taxJurisdictionForm = New Form_ManageTaxJurisdiction
        taxJurisdictionForm.ShowDialog(Me)
        taxJurisdictionForm.Dispose()
    End Sub

    Private Sub Button_AddTaxClass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_AddTaxClass.Click
        'open and bring focus to new form
        taxClassForm = New Form_ManageTaxClass
        taxClassForm.ShowDialog(Me)
        taxClassForm.Dispose()
    End Sub

    Private Sub taxJurisdictionForm_UpdateCallingForm() Handles taxJurisdictionForm.UpdateCallingForm
        'rebind drop down
        BindImportTypeComboBox()
    End Sub

    Private Sub taxClassForm_UpdateCallingForm() Handles taxClassForm.UpdateCallingForm
        'rebind drop down
        BindTaxClassComboBox()
    End Sub

    Private Sub taxFlagValueForm_UpdateCallingForm(ByVal newKey As String) Handles taxFlagValueForm.UpdateCallingForm
        'remove newKey value from list of deleted items: handles the event where a user deletes an item and then re-adds it before saving
        Dim deleteEnum As IEnumerator = deleteFlagList.GetEnumerator
        Dim currentFlag As TaxFlagBO
        Dim flagCount As Integer = 0
        Dim deleteFlagIndex As Integer = -1

        While deleteEnum.MoveNext
            flagCount += 1
            currentFlag = CType(deleteEnum.Current, TaxFlagBO)

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
        BindTaxFlagGrid()
    End Sub

    Private Sub ComboBox_Jurisdiction_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_Jurisdiction.SelectedIndexChanged
        If CType(Me.ComboBox_TaxClass.SelectedValue, Integer) > 0 AndAlso CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer) > 0 Then
            BindTaxFlagGrid()
        Else
            Me.UltraGrid_TaxFlag.DataSource = Nothing
        End If
    End Sub

    Private Sub ComboBox_TaxClass_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_TaxClass.SelectedIndexChanged
        If CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer) > 0 AndAlso CType(Me.ComboBox_TaxClass.SelectedValue, Integer) > 0 Then
            BindTaxFlagGrid()
        Else
            UltraGrid_TaxFlag.DataSource = Nothing
        End If
    End Sub

    Private Sub Button_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        'validate user has selected TaxClass and TaxJurisdiction
        Dim taxFlagBO As New TaxFlagBO
        Dim statusList As ArrayList = taxFlagBO.ValidateAdd(CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer), CType(Me.ComboBox_TaxClass.SelectedValue, Integer))
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As TaxFlagStatus

        'loop through possible validation erorrs and build message string containing all errors
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, TaxFlagStatus)

            Select Case currentStatus
                Case TaxFlagStatus.Error_Required_TaxJurisdiction
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_Jurisdiction.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_Required_TaxClass
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_TaxClass.Text))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            taxFlagValueForm = New Form_ManageTaxFlagValue

            'set TaxClassID and TaxJurisdictionID for add form
            taxFlagBO.TaxClassId = CType(Me.ComboBox_TaxClass.SelectedValue, Integer)
            taxFlagBO.TaxJurisdictionId = CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer)
            taxFlagValueForm.TaxFlagData = taxFlagBO
            taxFlagValueForm.IsEdit = False
            taxFlagValueForm.ExistingTaxFlagValues = taxFlagValues

            taxFlagValueForm.ShowDialog(Me)
            taxFlagValueForm.Dispose()
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
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
        BindImportTypeComboBox()
        BindTaxClassComboBox()
    End Sub

    Private Sub BindImportTypeComboBox()
        ' setup tax jurisdiction drop down
        Dim jurisdictionDAO As New TaxJurisdictionDAO
        Dim jurisdictionList As ArrayList = jurisdictionDAO.GetJurisdictionList

        'insert blank row at start of drop down
        jurisdictionList.Insert(0, New TaxJurisdictionBO())

        ComboBox_Jurisdiction.DataSource = jurisdictionList
        ComboBox_Jurisdiction.ValueMember = "TaxJurisdictionID"
        ComboBox_Jurisdiction.DisplayMember = "TaxJurisdictionDesc"

        'default selection to blank row
        ComboBox_Jurisdiction.SelectedIndex = 0
    End Sub

    Private Sub BindTaxClassComboBox()
        ' setup tax class drop down
        Dim taxClassDAO As New TaxClassDAO
        Dim taxClassList As ArrayList = taxClassDAO.GetTaxClassList

        'insert blank row at start of drop down
        taxClassList.Insert(0, New TaxClassBO())

        ComboBox_TaxClass.DataSource = taxClassList
        ComboBox_TaxClass.ValueMember = "TaxClassID"
        ComboBox_TaxClass.DisplayMember = "TaxClassDesc"

        'default selection to blank row
        ComboBox_TaxClass.SelectedIndex = 0
    End Sub

    Private Sub BindTaxFlagGrid()
        Dim taxFlagBO As New TaxFlagBO
        Dim existingTaxFlags As New Hashtable
        Dim taxFlagRowsEnum As IEnumerator = UltraGrid_TaxFlag.Rows.GetEnumerator
        Dim row As UltraGridRow

        UltraGrid_TaxFlag.DataSource = Nothing
        UltraGrid_TaxFlag.DisplayLayout.Reset()

        taxFlagBO.TaxClassId = CType(Me.ComboBox_TaxClass.SelectedValue, Integer)
        taxFlagBO.TaxJurisdictionId = CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer)

        ' setup tax flag datagrid
        Dim taxFlagDAO As New TaxFlagDAO
        UltraGrid_TaxFlag.DataSource = taxFlagDAO.GetTaxFlagList(taxFlagBO, deleteFlagList)

        If UltraGrid_TaxFlag.DisplayLayout.Rows.Count <= 0 Then
            MsgBox("No data available for the selected jurisdiction and tax classification", MsgBoxStyle.Critical)
        End If

        'populate class level hashtable w/ existing tax flag values; this will be passed to child form to determine if 
        'an added/edited tax flag already exists
        taxFlagValues = New Hashtable
        While taxFlagRowsEnum.MoveNext
            row = CType(taxFlagRowsEnum.Current, UltraGridRow)
            taxFlagValues.Add(row.Cells("TaxFlagKey").Value.ToString, Nothing)
        End While

        If Me.UltraGrid_TaxFlag.Rows.Count > 0 Then
            'first row appears to be defaulted because it is highlighted, but it's not actually selected.
            'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
            'below code is actually selecting row that is already highlighted.
            Me.UltraGrid_TaxFlag.Rows(0).Selected = True
        End If

        'format data control
        FormatDataGrid()
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        If UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxClassId").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxClassDesc").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxJurisdictionId").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxJurisdictionDesc").Hidden = True
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("ResetActiveFlags").Hidden = True

            'sort columns in correct order
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.VisiblePosition = 0
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.VisiblePosition = 1
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").Header.VisiblePosition = 2
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("POSID").Header.VisiblePosition = 3
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("ExternalTaxGroupCode").Header.VisiblePosition = 4

            'set column names
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagKey").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagKey")
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxFlagValue").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxFlagValue")
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").Header.Caption = ResourcesTaxHosting.GetString("label_header_taxPercent")
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("POSID").Header.Caption = ResourcesTaxHosting.GetString("label_header_POSID")
            UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("ExternalTaxGroupCode").Header.Caption = ResourcesTaxHosting.GetString("label_header_ExternalTaxGroupCode")
   
            UltraGrid_TaxFlag.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
        End If
    End Sub

    'Private Sub UltraGrid_TaxFlag_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid_TaxFlag.InitializeLayout
    '    'format column data
    '    If UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns.Count > 0 Then
    '        UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").Format = "#0.##"
    '        UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").MaskInput = "{double:9.2}"
    '        UltraGrid_TaxFlag.DisplayLayout.Bands(0).Columns("TaxPercent").PromptChar = Nothing
    '    End If
    'End Sub

    Public Sub UpdateRow(ByVal isEdit As Boolean)
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Dim taxFlagDAO As New TaxFlagDAO
        Dim taxFlagBO As New TaxFlagBO
        Dim result As DialogResult

        Try
            'get selected row
            If Me.UltraGrid_TaxFlag.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_TaxFlag.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_TaxFlag.ActiveRow.Index
            End If

            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        'delete row if user has selected a row that is not the NewRow
        If isRowSelected Then
            Dim currentRow As UltraGridRow = Me.UltraGrid_TaxFlag.Rows(selectedRowIndex)

            'perform edit or delete operation based on flag passed in
            taxFlagBO.TaxClassId = CType(Me.ComboBox_TaxClass.SelectedValue, Integer)
            taxFlagBO.TaxJurisdictionId = CType(Me.ComboBox_Jurisdiction.SelectedValue, Integer)
            taxFlagBO.TaxFlagKey = currentRow.Cells("TaxFlagKey").Value.ToString

            If isEdit Then
                taxFlagValueForm = New Form_ManageTaxFlagValue

                'set form values for selected TaxFlag
                taxFlagValueForm.IsEdit = True
                taxFlagBO.TaxFlagKey = currentRow.Cells("TaxFlagKey").Value.ToString
                If currentRow.Cells("TaxPercent").Value IsNot Nothing Then
                    taxFlagBO.TaxPercent = currentRow.Cells("TaxPercent").Value.ToString
                End If
                If currentRow.Cells("POSID").Value IsNot Nothing Then
                    taxFlagBO.POSId = currentRow.Cells("POSID").Value.ToString
                End If
                taxFlagBO.TaxFlagValue = CType(currentRow.Cells("TaxFlagValue").Value, Boolean)
                If currentRow.Cells("ExternalTaxGroupCode").Value IsNot Nothing Then
                    taxFlagBO.ExternalTaxGroupCode = currentRow.Cells("ExternalTaxGroupCode").Value.ToString
                End If

                taxFlagValueForm.TaxFlagData = taxFlagBO
                taxFlagValueForm.ExistingTaxFlagValues = taxFlagValues
                taxFlagValueForm.ShowDialog(Me)
                taxFlagValueForm.Dispose()
            Else
                'see if this tax flag is overridden by any items (TaxOverride table)
                Dim overrideCount As Integer = TaxOverrideDAO.GetNumberItemsOverridenForFlag(taxFlagBO.TaxClassId, taxFlagBO.TaxJurisdictionId, taxFlagBO.TaxFlagKey)

                'confirm user wishes to delete this item
                If overrideCount > 0 Then
                    '{0} = Tax Flag; {1} = Tax Override item count; {2} = Tax Flag Key being deleted
                    result = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_deleteTaxFlag_TaxOverrideWarning"), ResourcesTaxHosting.GetString("label_header_taxFlagKey"), overrideCount.ToString, currentRow.Cells("TaxFlagKey").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Else
                    result = MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_deleteConfirmation"), currentRow.Cells("TaxFlagKey").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                End If

                If result = Windows.Forms.DialogResult.Yes Then
                    'don't delete tax flags now - track which to delete and save to DB 
                    'when user clicks 'OK' or closes form
                    deleteFlagList.Add(taxFlagBO)

                    'update grid to reflect deleted tax flag
                    BindTaxFlagGrid()
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
        Dim currentFlag As TaxFlagBO
        Dim taxFlagDAO As New TaxFlagDAO
        Dim transaction As SqlTransaction = Nothing

        'delete all tax flags
        flagEnum = deleteFlagList.GetEnumerator

        Try
            'create transaction to share connection for all flags to delete
            transaction = taxFlagDAO.GetTransaction

            'loop through each flag and delete them
            While flagEnum.MoveNext
                currentFlag = CType(flagEnum.Current, TaxFlagBO)

                'delete current tax flag
                taxFlagDAO.DeleteData(currentFlag, transaction)
            End While

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
        End Try
    End Sub

    Private Sub UltraGrid_TaxFlag_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles UltraGrid_TaxFlag.InitializeRow

        e.Row.Cells("ExternalTaxGroupCode").Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled

    End Sub
End Class