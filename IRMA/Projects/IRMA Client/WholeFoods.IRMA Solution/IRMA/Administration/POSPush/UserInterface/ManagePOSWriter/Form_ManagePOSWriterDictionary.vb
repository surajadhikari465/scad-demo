Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_ManagePOSWriterDictionary
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' DataSet of dictionary definitions.
    ''' </summary>
    ''' <remarks></remarks>
    Dim _dataSet As DataSet
    ''' <summary>
    ''' Value of the POS Writer being edited.
    ''' </summary>
    ''' <remarks></remarks>
    Private _posFileWriterKey As Integer
    ''' <summary>
    ''' Description of the POS Writer being edited.
    ''' </summary>
    ''' <remarks></remarks>
    Private _POSFileWriterCode As String
    ''' <summary>
    ''' Value of the POS Data Type being edited.
    ''' </summary>
    ''' <remarks></remarks>
    Private _posDataTypeKey As Integer
#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

#Region "Events handled by this form"
#Region "form load"
    ''' <summary>
    ''' Pre-populate the form with the existing data dictionary values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManagePOSWriterDictionary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load data to form control
        BindData()
        'format data control
        FormatDataGrid()
    End Sub

#End Region

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManagePOSWriterDictionary_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _dataSet.HasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                SaveChanges()
            End If
        End If

        ' Raise event - allows the data on the parent form to be refreshed
        RaiseEvent UpdateCallingForm()
    End Sub

    ''' <summary>
    ''' Add new data dictionary value to grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        'jump to "new" row and place cursor so user can begin edit
        DataGridView_Dictionary.CurrentCell = DataGridView_Dictionary.Rows(DataGridView_Dictionary.NewRowIndex).Cells(DataGridView_Dictionary.Columns("FieldID").Index)
        DataGridView_Dictionary.BeginEdit(True)
    End Sub

    ''' <summary>
    ''' Delete data dictionary from grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        Dim selectedRowIndex As Integer

        If DataGridView_Dictionary.SelectedRows.Count = 0 Then
            If DataGridView_Dictionary.SelectedCells.Count > 0 Then
                selectedRowIndex = DataGridView_Dictionary.SelectedCells(0).RowIndex
            Else
                selectedRowIndex = 0
            End If
        Else
            selectedRowIndex = DataGridView_Dictionary.SelectedRows(0).Index
        End If

        'delete row if user has selected a row that is not the NewRow
        If selectedRowIndex <> DataGridView_Dictionary.NewRowIndex Then
            Dim currentRow As DataGridViewRow = DataGridView_Dictionary.Rows(selectedRowIndex)
            Dim writerDictionary As New POSWriterDictionaryBO(currentRow)

            'validate if DELETE can happen
            Select Case writerDictionary.ValidateDelete()
                Case POSWriterDictionaryStatus.Error_ColumnsAssociated
                    MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_deleteFieldId_associatedColumns"), writerDictionary.FieldIdCount, _POSFileWriterCode), ResourcesCommon.GetString("msg_titleWarning"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case POSWriterDictionaryStatus.Valid
                    'confirm user wishes to delete this item
                    Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_deleteConfirmation"), currentRow.Cells("FieldId").Value), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If result = Windows.Forms.DialogResult.Yes Then
                        'remove item from grid
                        DataGridView_Dictionary.Rows.Remove(currentRow)
                    End If
            End Select
        End If
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
        ' Read the data dictionary values
        _dataSet = POSWriterDictionaryDAO.GetPOSWriterDictionaryValues(_posFileWriterKey)
        DataGridView_Dictionary.DataSource = _dataSet.Tables(0)
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        DataGridView_Dictionary.MultiSelect = False

        ' Format the view
        If (DataGridView_Dictionary.Columns.Count > 0) Then
            DataGridView_Dictionary.Columns("POSFileWriterKey").Visible = False
            'DataGridView_Dictionary.Columns("POSDataTypeKey").Visible = False
            DataGridView_Dictionary.Columns("FieldIdCount").Visible = False

            DataGridView_Dictionary.Columns("FieldID").DisplayIndex = 0
            DataGridView_Dictionary.Columns("FieldID").HeaderText = ResourcesAdministration.GetString("label_fieldId")
            DataGridView_Dictionary.Columns("FieldID").ReadOnly = False

            DataGridView_Dictionary.Columns("DataType").DisplayIndex = 1
            DataGridView_Dictionary.Columns("DataType").HeaderText = ResourcesAdministration.GetString("label_dataType")
            DataGridView_Dictionary.Columns("DataType").ReadOnly = False

            DataGridView_Dictionary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
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
                ' All rows in the table will have the same POSFileWriterKey and POSDataTypeKey values - set these
                ' before the save (new items will not have them pre-filled like updates)
                Dim rowCounter As Integer
                Dim currentRow As DataGridViewRow
                For rowCounter = 0 To DataGridView_Dictionary.Rows.Count - 1
                    currentRow = DataGridView_Dictionary.Rows(rowCounter)
                    currentRow.Cells("POSFileWriterKey").Value = _posFileWriterKey
                    'currentRow.Cells("POSDataTypeKey").Value = _posDataTypeKey
                Next

                ' Apply the changes
                POSWriterDictionaryDAO.SavePOSWriterDictionaryValues(_dataSet, _posFileWriterKey)
            Catch ex As DBConcurrencyException
                'TODO message to user about concurrency exception
            Catch ex2 As Exception
                'TODO inform user this operation failed
            End Try
        End If
    End Sub

#Region "Property Definitions"
    Property POSFileWriterKey() As Integer
        Get
            Return _posFileWriterKey
        End Get
        Set(ByVal value As Integer)
            _posFileWriterKey = value
        End Set
    End Property

    Public Property POSFileWriterCode() As String
        Get
            Return _POSFileWriterCode
        End Get
        Set(ByVal value As String)
            _POSFileWriterCode = value
        End Set
    End Property

    Property POSDataTypeKey() As Integer
        Get
            Return _posDataTypeKey
        End Get
        Set(ByVal value As Integer)
            _posDataTypeKey = value
        End Set
    End Property
#End Region
End Class