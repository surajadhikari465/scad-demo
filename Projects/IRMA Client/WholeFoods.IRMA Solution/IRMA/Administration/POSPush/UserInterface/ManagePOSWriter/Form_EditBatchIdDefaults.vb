Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Public Class Form_EditBatchIdDefaults

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' Value of the current writer configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputWriterConfig As POSWriterBO
    ''' <summary>
    ''' DataSet that holds change type data
    ''' </summary>
    ''' <remarks></remarks>
    Dim _writerChangeTypeDataSet As DataSet
    ''' <summary>
    ''' DataSet that holds price change type data
    ''' </summary>
    ''' <remarks></remarks>
    Dim _priceChangeDataSet As DataSet
    ''' <summary>
    ''' DataSet that holds item change type data
    ''' </summary>
    ''' <remarks></remarks>
    Dim _itemChangeDataSet As DataSet
    ''' <summary>
    ''' Form to edit a default batch id value
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editBatchIdValueForm As Form_EditBatchIdValue
#End Region

#Region "Properties"
    Public Property InputWriterConfig() As POSWriterBO
        Get
            Return _inputWriterConfig
        End Get
        Set(ByVal value As POSWriterBO)
            _inputWriterConfig = value
        End Set
    End Property
#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to one of the default batch id values.  Refresh the data grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditBatchIdValueForm_UpdateCallingForm() Handles editBatchIdValueForm.UpdateCallingForm
        ' Refresh the data grids
        PopulateDefaultValues()
    End Sub
#End Region

#Region "Form Events"
    ''' <summary>
    ''' Pre-fill the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditBatchIdDefaults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Logger.LogDebug("Form_EditBatchIdDefaults_Load entry", Me.GetType())
        Me.Label_FileWriterCode.Text = _inputWriterConfig.POSFileWriterCode
        Me.Label_WriterType.Text = _inputWriterConfig.WriterType

        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ' Hide the scale writer type from other writers
            Me.Label_ScaleWriterTypeHeader.Visible = False
            Me.Label_ScaleWriterType.Visible = False
        Else
            ' Prefill the scale writer values
            Me.Label_ScaleWriterType.Text = _inputWriterConfig.ScaleWriterTypeDesc
        End If

        ' Pre-fill each of the data grids
        PopulateDefaultValues()
        Logger.LogDebug("Form_EditBatchIdDefaults_Load exit", Me.GetType())
    End Sub

    Private Sub Button_EditPriceChangeTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditPriceChangeTypes.Click
        Logger.LogDebug("Button_EditPriceChangeTypes_Click entry", Me.GetType())
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Try
            'get selected row
            If UltraGrid_PriceChangeTypes.Selected.Rows.Count = 0 Then
                selectedRowIndex = UltraGrid_PriceChangeTypes.ActiveCell.Row.Index
            Else
                selectedRowIndex = UltraGrid_PriceChangeTypes.ActiveRow.Index
            End If
            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        If isRowSelected Then
            Dim currentRow As UltraGridRow = UltraGrid_PriceChangeTypes.Rows(selectedRowIndex)
            ' Bring focus to the form
            editBatchIdValueForm = New Form_EditBatchIdValue()
            ' Populate the edit form with the values from the selected writer
            editBatchIdValueForm.InputWriterConfig = _inputWriterConfig
            ' Pass in the values to pre-fill the batch id defaults
            Dim batchIdBO As New BatchIdDefaultBO(BatchIdDefaultType.PriceChange)
            batchIdBO.PopulateFromSelectedPriceChangeType(currentRow, _inputWriterConfig)
            editBatchIdValueForm.Label_ChangeTypeHeader.Text = GroupBox_PriceChangeTypes.Text & ":"
            editBatchIdValueForm.BatchIdDefaultBO = batchIdBO
            ' Show the form
            editBatchIdValueForm.ShowDialog(Me)
            editBatchIdValueForm.Dispose()
        Else
            ' prompt the user to select a row if they have not
            MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_validation_selectChangeTypeRow"), GroupBox_PriceChangeTypes.Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Logger.LogDebug("Button_EditPriceChangeTypes_Click exit", Me.GetType())
    End Sub

    Private Sub Button_EditItemChangeType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditItemChangeType.Click
        Logger.LogDebug("Button_EditItemChangeType_Click entry", Me.GetType())
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Try
            'get selected row
            If UltraGrid_ItemChangeTypes.Selected.Rows.Count = 0 Then
                selectedRowIndex = UltraGrid_ItemChangeTypes.ActiveCell.Row.Index
            Else
                selectedRowIndex = UltraGrid_ItemChangeTypes.ActiveRow.Index
            End If
            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        If isRowSelected Then
            Dim currentRow As UltraGridRow = UltraGrid_ItemChangeTypes.Rows(selectedRowIndex)
            ' Bring focus to the form
            editBatchIdValueForm = New Form_EditBatchIdValue()
            ' Populate the edit form with the values from the selected writer
            editBatchIdValueForm.InputWriterConfig = _inputWriterConfig
            ' Pass in the values to pre-fill the batch id defaults
            Dim batchIdBO As New BatchIdDefaultBO(BatchIdDefaultType.ItemChange)
            batchIdBO.PopulateFromSelectedItemChangeType(currentRow, _inputWriterConfig)
            editBatchIdValueForm.Label_ChangeTypeHeader.Text = GroupBox_ItemChangeTypes.Text & ":"
            editBatchIdValueForm.BatchIdDefaultBO = batchIdBO
            ' Show the form
            editBatchIdValueForm.ShowDialog(Me)
            editBatchIdValueForm.Dispose()
        Else
            ' prompt the user to select a row if they have not
            MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_validation_selectChangeTypeRow"), GroupBox_ItemChangeTypes.Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Logger.LogDebug("Button_EditItemChangeType_Click exit", Me.GetType())
    End Sub

    Private Sub Button_EditWriterChangeTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditWriterChangeTypes.Click
        Logger.LogDebug("Button_EditWriterChangeTypes_Click entry", Me.GetType())
        Dim selectedRowIndex As Integer
        Dim isRowSelected As Boolean = False
        Try
            'get selected row
            If UltraGrid_WriterChangeTypes.Selected.Rows.Count = 0 Then
                selectedRowIndex = UltraGrid_WriterChangeTypes.ActiveCell.Row.Index
            Else
                selectedRowIndex = UltraGrid_WriterChangeTypes.ActiveRow.Index
            End If
            isRowSelected = True
        Catch ex As Exception
            isRowSelected = False
        End Try

        If isRowSelected Then
            Dim currentRow As UltraGridRow = UltraGrid_WriterChangeTypes.Rows(selectedRowIndex)
            ' Bring focus to the form
            editBatchIdValueForm = New Form_EditBatchIdValue()
            ' Populate the edit form with the values from the selected writer
            editBatchIdValueForm.InputWriterConfig = _inputWriterConfig
            ' Pass in the values to pre-fill the batch id defaults
            Dim batchIdBO As New BatchIdDefaultBO(BatchIdDefaultType.WriterChange)
            batchIdBO.PopulateFromSelectedWriterChangeType(currentRow, _inputWriterConfig)
            editBatchIdValueForm.Label_ChangeTypeHeader.Text = GroupBox_WriterChangeTypes.Text & ":"
            editBatchIdValueForm.BatchIdDefaultBO = batchIdBO
            ' Show the form
            editBatchIdValueForm.ShowDialog(Me)
            editBatchIdValueForm.Dispose()
        Else
            ' prompt the user to select a row if they have not
            MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_validation_selectChangeTypeRow"), GroupBox_WriterChangeTypes.Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Logger.LogDebug("Button_EditWriterChangeTypes_Click exit", Me.GetType())
    End Sub
#End Region

    ''' <summary>
    ''' Query the database to fill the default batch id grids with current data.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateDefaultValues()
        Logger.LogDebug("PopulateDefaultValues entry", Me.GetType())
        Dim writerDAO As New POSWriterDAO
        ' PRICE CHANGE DATA GRID
        _priceChangeDataSet = writerDAO.GetBatchIdDefaultsByPriceChangeType(_inputWriterConfig)
        UltraGrid_PriceChangeTypes.DataSource = _priceChangeDataSet.Tables(0)
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("PriceChgTypeId").Hidden = True
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("POSFileWriterKey").Hidden = True
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("PriceChgTypeDesc").Header.Caption = "Change Type"
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("PriceChgTypeDesc").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").Header.Caption = "Default Batch Id"
        UltraGrid_PriceChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit

        ' ITEM CHANGE DATA GRID
        _itemChangeDataSet = writerDAO.GetBatchIdDefaultsByItemChangeType(_inputWriterConfig)
        UltraGrid_ItemChangeTypes.DataSource = _itemChangeDataSet.Tables(0)
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("ItemChgTypeId").Hidden = True
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("POSFileWriterKey").Hidden = True
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("ItemChgTypeDesc").Header.Caption = "Change Type"
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("ItemChgTypeDesc").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").Header.Caption = "Default Batch Id"
        UltraGrid_ItemChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit

        ' WRITER CHANGE TYPE DATA GRID
        _writerChangeTypeDataSet = writerDAO.GetBatchIdDefaultsByWriterChangeType(_inputWriterConfig)
        UltraGrid_WriterChangeTypes.DataSource = _writerChangeTypeDataSet.Tables(0)
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("POSChangeTypeKey").Hidden = True
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("POSFileWriterKey").Hidden = True
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("ChangeTypeDesc").Header.Caption = "Change Type"
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("ChangeTypeDesc").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").Header.Caption = "Default Batch Id"
        UltraGrid_WriterChangeTypes.DisplayLayout.Bands(0).Columns("POSBatchIdDefault").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Logger.LogDebug("PopulateDefaultValues exit", Me.GetType())
    End Sub
End Class