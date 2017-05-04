Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.UserInterface
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditPOSWriter

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Value of the current writer configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputWriterConfig As POSWriterBO
    ''' <summary>
    ''' Form to create or edit a single column.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editPOSWriterColumnForm As Form_EditPOSWriterFileConfig
    ''' <summary>
    ''' Form to create or edit a single binary int column.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editPOSWriterBinaryIntColumnForm As Form_EditPOSWriterFileConfigBinaryInt
    ''' <summary>
    ''' Form to delete a single column.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents deletePOSWriterColumnForm As Form_DeletePOSWriterFileConfig
    ''' <summary>
    ''' Form to edit a single POSWriter.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editBatchIdDefaults As Form_EditBatchIdDefaults
    ''' <summary>
    ''' Flag set to true if the selected row should be processed by the SelectedIndexChanged event.
    ''' </summary>
    ''' <remarks>this flag is necessary because populating the data grid on form load triggers
    ''' the SelectedIndexChanged event, and processed the event during form load will cause an 
    ''' infinite loop</remarks>
    Private processSelectedRow As Boolean = True
    ''' <summary>
    ''' DataSet that holds escape char data
    ''' </summary>
    ''' <remarks></remarks>
    Dim _escapeCharDataSet As DataSet

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

#Region "Load Form"

    ''' <summary>
    ''' Load the form, querying the database to populate the POSWriterFileConfig details for each
    ''' of the change types.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditPOSWriterFileConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'set up button and title bar labels
        LoadText()
        ' Set the title for the form
        Select Case _currentAction
            Case FormAction.Create
                ' enable/disable the appropriate labels and input boxes
                Me.TextBox_FileWriterCodeVal.Enabled = True
                ' initialize the POSWriterBO
                _inputWriterConfig = New POSWriterBO()
                'setup escape chars data grid
                PopulatePropertiesData()
            Case FormAction.Edit
                ' enable/disable the appropriate labels and input boxes
                Me.TextBox_FileWriterCodeVal.Enabled = False

                If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
                    ' Hide the scale writer type from other writers
                    Me.Label_ScaleWriterTypeHeader.Visible = False
                    Me.Label_ScaleWriterType.Visible = False
                    DeleteTab(Me.TabGroup.Controls(Me.ItemDataZoneChange_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.NutriFacts_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ExtraText_TabPage.Name))
                Else
                    ' Set the scale writer type for scale writers
                    Me.Label_ScaleWriterTypeHeader.Visible = True
                    Me.Label_ScaleWriterType.Visible = True
                    Me.Label_ScaleWriterType.Text = _inputWriterConfig.ScaleWriterTypeDesc

                    ' Delete the tabs that do not apply to the scale writers
                    ' These tabs do not apply to any of the scales
                    DeleteTab(Me.TabGroup.Controls(Me.PromotionalData_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.VendorAdd_TabPage.Name))
                    If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                        ' These tabs do not apply to corporate scales
                        DeleteTab(Me.TabGroup.Controls(Me.ItemDelete_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ItemDataZoneChange_TabPage.Name))
                    ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                        ' These tabs do not apply to zone scales
                        DeleteTab(Me.TabGroup.Controls(Me.ItemIdAdd_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ItemIdDelete_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ItemDataZoneChange_TabPage.Name)) ''new tab used only for store scale writers
                        DeleteTab(Me.TabGroup.Controls(Me.NutriFacts_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ExtraText_TabPage.Name))
                    ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                        ' These tabs do not apply to zone scales
                        DeleteTab(Me.TabGroup.Controls(Me.ItemIdAdd_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ItemIdDelete_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ItemDataZoneChange_TabPage.Name)) ''new tab used only for store scale writers
                        DeleteTab(Me.TabGroup.Controls(Me.NutriFacts_TabPage.Name))
                        DeleteTab(Me.TabGroup.Controls(Me.ExtraText_TabPage.Name))
                    End If

                    ' Rename the scale writer Item Data Change tab, based on the scale writer type
                    If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                        Me.ItemDataChange_TabPage.Text = ResourcesAdministration.GetString("label_tab_scale_corpItemDataChange")
                    ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                        Me.ItemDataChange_TabPage.Text = ResourcesAdministration.GetString("label_tab_scale_zoneItemDataChange")
                    ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                        Me.ItemDataChange_TabPage.Text = ResourcesAdministration.GetString("label_tab_scale_corpItemDataChange")
                        Me.ItemDataZoneChange_TabPage.Text = ResourcesAdministration.GetString("label_tab_scale_zoneItemDataChange")
                    End If
                End If

                If Not (_inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Or _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG) Then
                    ' Hide the tag writer type from other writers
                    DeleteTab(Me.TabGroup.Controls(Me.ShelfTag_TabPage.Name))
                Else
                    ' Hide the POS and Scale tabs from the Shelf Tag writer
                    DeleteTab(Me.TabGroup.Controls(Me.ItemIdAdd_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ItemIdDelete_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ItemDataChange_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ItemDelete_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.PromotionalData_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.VendorAdd_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ItemDataZoneChange_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.NutriFacts_TabPage.Name))
                    DeleteTab(Me.TabGroup.Controls(Me.ExtraText_TabPage.Name))
                End If

                ' Pre-fill the existing values for edits
                Me.TextBox_FileWriterCodeVal.Text = _inputWriterConfig.POSFileWriterCode
                Me.Label_WriterType.Text = _inputWriterConfig.WriterType

                PopulatePropertiesData()
                ' Load the details tab for each change type
                PopulateAllPOSWriterFileConfigData(False)
        End Select

        ' The Apply button is disabled on load
        Me.Properties_Button_Apply.Enabled = False
    End Sub

    ''' <summary>
    ''' Pre-fill the properties tab with the data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulatePropertiesData()
        'bind data to escape char data grid
        BindPropertiesTabData()

        ' Load the writer properties tab
        Me.Properties_TextBox_DelimCharVal.Text = _inputWriterConfig.DelimChar
        Me.Properties_CheckBox_LeadingDelimiter.Checked = _inputWriterConfig.LeadingDelimiter
        Me.Properties_CheckBox_TrailingDelimiter.Checked = _inputWriterConfig.TrailingDelimiter
        Me.Properties_CheckBox_FieldIdDelimiter.Checked = _inputWriterConfig.FieldIdDelim
        Me.Properties_CheckBox_EnforceDictionary.Checked = _inputWriterConfig.EnforceDictionary
        Me.ComboBox_FileWriterClass.SelectedItem = _inputWriterConfig.POSFileWriterClass
        Me.Properties_CheckBox_POSSectionHeader.Checked = _inputWriterConfig.OutputByIrmaBatches
        Me.Properties_CheckBox_FixedWidthVal.Checked = _inputWriterConfig.FixedWidth
        Me.Properties_CheckBox_AppendToFile.Checked = _inputWriterConfig.AppendToFile
        Me.Properties_TextBox_TaxFlagTrueVal.Text = _inputWriterConfig.TaxFlagTrueChar
        Me.Properties_TextBox_TaxFlagFalseVal.Text = _inputWriterConfig.TaxFlagFalseChar
        Me.Properties_TextBox_MinBatchId.Text = _inputWriterConfig.BatchIdMin
        Me.Properties_TextBox_MaxBatchId.Text = _inputWriterConfig.BatchIdMax

        UltraGrid_EscapeChars.DisplayLayout.Bands(0).Columns("POSFileWriterKey").Hidden = True
        UltraGrid_EscapeChars.DisplayLayout.Bands(0).Columns("EscapeCharValue").Header.Caption = ResourcesAdministration.GetString("label_header_escapeCharValue")
        UltraGrid_EscapeChars.DisplayLayout.Bands(0).Columns("EscapeCharReplacement").Header.Caption = ResourcesAdministration.GetString("lable_header_escapeCharRepalce")

        'limit escape char value and replacements to 10 chars
        UltraGrid_EscapeChars.DisplayLayout.Bands(0).Columns("EscapeCharValue").MaxLength = 10
        UltraGrid_EscapeChars.DisplayLayout.Bands(0).Columns("EscapeCharReplacement").MaxLength = 10
    End Sub

    ''' <summary>
    ''' Loads the details tab for each change type:
    ''' 1. If useSeletecRow is true, 
    '''        Read the row that was last selected by the user (so they will be in the same place they were before editing the column)
    '''    Otherwise,
    '''        Read row 1
    ''' 2. Refresh the data grid so it will include the most current values in the database
    ''' </summary>
    ''' <param name="useSelectedRow"></param>
    ''' <remarks></remarks>
    Private Sub PopulateAllPOSWriterFileConfigData(ByVal useSelectedRow As Boolean)
        Dim selectedRow As Integer = 1
        processSelectedRow = False


        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_POS Then
            ' Item ID Adds
            If useSelectedRow Then
                selectedRow = CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ItemIDAdd_ComboBox_Rows, Me.ItemIdAdd_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ItemIDAdd), selectedRow, Not useSelectedRow)
            ' Item Data Changes
            If useSelectedRow Then
                selectedRow = CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ItemDataChange_ComboBox_Rows, Me.ItemDataChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ItemChange), selectedRow, Not useSelectedRow)
            ' Item Deletes
            If useSelectedRow Then
                selectedRow = CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ItemDelete_ComboBox_Rows, Me.ItemDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ItemDelete), selectedRow, Not useSelectedRow)
            ' Item ID Deletes
            If useSelectedRow Then
                selectedRow = CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ItemIdDelete_ComboBox_Rows, Me.ItemIdDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ItemIDDelete), selectedRow, Not useSelectedRow)
            ' Promotional Data
            If useSelectedRow Then
                selectedRow = CType(PromotionalData_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.PromotionalData_ComboBox_Rows, Me.PromotionalData_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.PromotionalData), selectedRow, Not useSelectedRow)
            ' Vendor Add Data
            If useSelectedRow Then
                selectedRow = CType(VendorAdd_ComboBox_Rows.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.VendorAdd_ComboBox_Rows, Me.VendorAdd_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.VendorAdd), selectedRow, Not useSelectedRow)
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ' ShelfTag Data
            If useSelectedRow Then
                selectedRow = CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ShelfTag_CurrentRow_ComboBox, Me.ShelfTag_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ShelfTagFile), selectedRow, Not useSelectedRow)
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ' Electronic Shelf Tag Data
            If useSelectedRow Then
                selectedRow = CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer)
            End If
            PopulatePOSWriterFileConfigData(Me.ShelfTag_CurrentRow_ComboBox, Me.ShelfTag_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ElectronicShelfTag), selectedRow, Not useSelectedRow)
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ' Corp Scale Item Id Add Data
                If useSelectedRow Then
                    selectedRow = CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemIDAdd_ComboBox_Rows, Me.ItemIdAdd_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemIdAdd), selectedRow, Not useSelectedRow)
                ' Corp Scale Item Id Delete Data
                If useSelectedRow Then
                    selectedRow = CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemIdDelete_ComboBox_Rows, Me.ItemIdDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemIdDelete), selectedRow, Not useSelectedRow)
                ' Corp Scale Item Change Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDataChange_ComboBox_Rows, Me.ItemDataChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemChange), selectedRow, Not useSelectedRow)
                ' NutriFacts Data
                If useSelectedRow Then
                    selectedRow = CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.NutriFacts_ComboBox_Rows, Me.NutriFacts_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.NutriFact), selectedRow, Not useSelectedRow)
                ' Extra Text Data
                If useSelectedRow Then
                    selectedRow = CType(ExtraText_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ExtraText_ComboBox_Rows, Me.ExtraText_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ExtraText), selectedRow, Not useSelectedRow)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ' Zone Scale Item Delete Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDelete_ComboBox_Rows, Me.ItemDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScaleItemDelete), selectedRow, Not useSelectedRow)
                ' Zone Scale Price Change Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDataChange_ComboBox_Rows, Me.ItemDataChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScalePriceChange), selectedRow, Not useSelectedRow)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ' Zone Scale Item Delete Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDelete_ComboBox_Rows, Me.ItemDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScaleItemDelete), selectedRow, Not useSelectedRow)
                ' Zone Scale Price Change Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDataChange_ComboBox_Rows, Me.ItemDataChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScaleSmartXPriceChange), selectedRow, Not useSelectedRow)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ' Corporate Data
                ' Corp Scale Item Id Add Data
                If useSelectedRow Then
                    selectedRow = CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemIDAdd_ComboBox_Rows, Me.ItemIdAdd_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemIdAdd), selectedRow, Not useSelectedRow)
                ' Corp Scale Item Id Delete Data
                If useSelectedRow Then
                    selectedRow = CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemIdDelete_ComboBox_Rows, Me.ItemIdDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemIdDelete), selectedRow, Not useSelectedRow)
                ' Corp Scale Item Change Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDataChange_ComboBox_Rows, Me.ItemDataChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.CorpScaleItemChange), selectedRow, Not useSelectedRow)
                ' NutriFacts Data
                If useSelectedRow Then
                    selectedRow = CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.NutriFacts_ComboBox_Rows, Me.NutriFacts_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.NutriFact), selectedRow, Not useSelectedRow)
                ' Extra Text Data
                If useSelectedRow Then
                    selectedRow = CType(ExtraText_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ExtraText_ComboBox_Rows, Me.ExtraText_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ExtraText), selectedRow, Not useSelectedRow)
                ' Zone Data
                ' Zone Scale Item Delete Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDelete_ComboBox_Rows, Me.ItemDelete_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScaleItemDelete), selectedRow, Not useSelectedRow)
                ' Zone Scale Price Change Data
                If useSelectedRow Then
                    selectedRow = CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer)
                End If
                PopulatePOSWriterFileConfigData(Me.ItemDataZoneChange_ComboBox_Rows, Me.ItemDataZoneChange_DataGridView_ConfigItems, POSChangeTypeDAO.GetChangeType(POSChangeType.ZoneScalePriceChange), selectedRow, Not useSelectedRow)
            End If
        End If

        processSelectedRow = True
    End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriter_Add")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriter_Edit")
        End Select
    End Sub

    ''' <summary>
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindPropertiesTabData()
        ' Read the escape char data
        Dim writerDAO As New POSWriterDAO
        _escapeCharDataSet = writerDAO.GetWriterEscapeChars(_inputWriterConfig)
        '  _escapeCharDataSet.Tables(0).Columns(1).AllowDBNull = False

        Me.UltraGrid_EscapeChars.DataSource = _escapeCharDataSet.Tables(0)

        ' setup writer file class data
        Me.ComboBox_FileWriterClass.DataSource = writerDAO.GetPOSFileWriterClasses(Me.Label_WriterType.Text)
        Me.ComboBox_FileWriterClass.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Deletes the specified tab from the Tab Control.
    ''' There is not a clean way to hide a tab or make it not visible, so it must be deleted.
    ''' </summary>
    ''' <param name="tabControl"></param>
    ''' <remarks></remarks>
    Private Sub DeleteTab(ByVal tabControl As Control)
        Me.TabGroup.Controls.Remove(tabControl)
    End Sub

#End Region

#Region "Close Form"
    ''' <summary>
    ''' Prompt the user to save changes before closing the form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditPOSWriter_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' If the user is closing the form and they have made updates, 
        ' prompt them to save their changes.
        If Properties_Button_Apply.Enabled Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the changes
                ApplyChanges()
            End If
        End If
    End Sub

#End Region

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the tables.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editPOSWriterColumn_UpdateCallingForm() Handles editPOSWriterColumnForm.UpdateCallingForm
        ' Load the details tab for each change type:
        ' 1. Read the row that was last selected by the user (so they will be in the same place they were before editing the column)
        ' 2. Refresh the data grid so it will include the edits
        PopulateAllPOSWriterFileConfigData(True)
    End Sub

    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the tables.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub deletePOSWriterColumn_UpdateCallingForm() Handles deletePOSWriterColumnForm.UpdateCallingForm
        ' Load the details tab for each change type
        ' 1. Read the row that was last selected by the user (so they will be in the same place they were before deleting the column)
        ' 2. Refresh the data grid so it will include the deletion
        PopulateAllPOSWriterFileConfigData(True)
    End Sub

    ''' <summary>
    ''' Changes were made to the writer configurations.  Refresh the tables.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub editPOSWriterBinaryIntColumn_UpdateCallingForm() Handles editPOSWriterBinaryIntColumnForm.UpdateCallingForm
        ' Load the details tab for each change type
        ' 1. Read the row that was last selected by the user (so they will be in the same place they were before deleting the column)
        ' 2. Refresh the data grid so it will include the deletion
        PopulateAllPOSWriterFileConfigData(True)
    End Sub

#End Region

#Region "Changed Tab events"
    ''' <summary>
    ''' Prompt the user to save any changes they made.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TabPage_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Properties_TabPage.Leave
        ' If the user changed from the Properties tab and they have made updates, 
        ' prompt them to save their changes.
        If Properties_Button_Apply.Enabled Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the changes
                If ApplyChanges() Then
                    ' The Apply button is no longer enabled
                    Properties_Button_Apply.Enabled = False
                End If
            Else
                ' Reset the tab with the original data since they aren't saving their changes
                PopulatePropertiesData()
                ' The Apply button is no longer enabled
                Properties_Button_Apply.Enabled = False
            End If
        End If
    End Sub

#End Region

#Region "Properties Tab events"
#Region "OK, Cancel, and Apply Buttons"
    ''' <summary>
    ''' Initialize a new POSWriterBO and populates it with form input values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InitializePOSWriterBO() As POSWriterBO
        Dim posWriter As New POSWriterBO
        Select Case _currentAction
            Case FormAction.Edit
                posWriter.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
                posWriter.ScaleWriterType = _inputWriterConfig.ScaleWriterType
        End Select
        posWriter.WriterType = Me.Label_WriterType.Text
        posWriter.POSFileWriterCode = Me.TextBox_FileWriterCodeVal.Text
        If Me.ComboBox_FileWriterClass.SelectedValue IsNot Nothing Then
            posWriter.POSFileWriterClass = Me.ComboBox_FileWriterClass.SelectedValue.ToString
        End If
        posWriter.DelimChar = Me.Properties_TextBox_DelimCharVal.Text
        posWriter.EnforceDictionary = Me.Properties_CheckBox_EnforceDictionary.Checked
        posWriter.OutputByIrmaBatches = Me.Properties_CheckBox_POSSectionHeader.Checked
        posWriter.FixedWidth = Me.Properties_CheckBox_FixedWidthVal.Checked
        posWriter.AppendToFile = Me.Properties_CheckBox_AppendToFile.Checked
        posWriter.LeadingDelimiter = Me.Properties_CheckBox_LeadingDelimiter.Checked
        posWriter.TrailingDelimiter = Me.Properties_CheckBox_TrailingDelimiter.Checked
        posWriter.FieldIdDelim = Me.Properties_CheckBox_FieldIdDelimiter.Checked
        posWriter.TaxFlagTrueChar = Me.Properties_TextBox_TaxFlagTrueVal.Text
        posWriter.TaxFlagFalseChar = Me.Properties_TextBox_TaxFlagFalseVal.Text
        posWriter.BatchIdMin = Me.Properties_TextBox_MinBatchId.Text
        posWriter.BatchIdMax = Me.Properties_TextBox_MaxBatchId.Text

        ' update the _inputWriterConfig so that the child form will see the appropriate values
        _inputWriterConfig.EnforceDictionary = Me.Properties_CheckBox_EnforceDictionary.Checked
        _inputWriterConfig.FixedWidth = Me.Properties_CheckBox_FixedWidthVal.Checked
        _inputWriterConfig.AppendToFile = Me.Properties_CheckBox_AppendToFile.Checked

        Return posWriter
    End Function

    ''' <summary>
    ''' Processes the click of the OK button on any tab in the form:
    ''' - Saves the changes on the Properties tab.
    ''' - Closes the form and returns focus to the View Writers form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ProcessOKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ApplyChanges() Then
            ' Do not prompt the user to save changes on closing
            Properties_Button_Apply.Enabled = False
            ' Close the child window
            Me.Close()
        End If
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim posWriter As POSWriterBO = InitializePOSWriterBO()
        Dim newPOSFileWriterKey As Integer = -1
        Dim exMessage As String = ResourcesCommon.GetString("msg_dbError")

        'validate properties tab data
        Dim statusList As ArrayList = posWriter.ValidatePOSWriterData
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As POSWriterBOStatus

        'loop through possible validation erorrs and build message string containing all errors
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, POSWriterBOStatus)

            Select Case currentStatus
                Case POSWriterBOStatus.Error_Required_FileWriterType
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_WriterTypeHeader.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_FileWriterCode
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FileWriterCode.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_WriterClass
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Properties_Label_WriterClass.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_Delimiter
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Properties_Label_DelimChar.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Integer_BatchIdMin
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Properties_Label_MinBatchId.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Integer_BatchIdMax
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Properties_Label_MaxBatchId.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Range_BatchId
                    message.Append(ResourcesCommon.GetString("msg_validation_batchIdRange"))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            ' Save the change to the database
            Select Case _currentAction
                Case FormAction.Create
                    newPOSFileWriterKey = POSWriterDAO.AddPOSWriterRecord(posWriter)
                Case FormAction.Edit
                    POSWriterDAO.UpdatePOSWriterRecord(posWriter)
            End Select

            ' save the escape char changes to the database
            If _escapeCharDataSet.HasChanges() Then
                Try

                    Dim rowEnum As IEnumerator = _escapeCharDataSet.Tables(0).Rows.GetEnumerator
                    Dim currentRow As DataRow

                    While rowEnum.MoveNext
                        currentRow = CType(rowEnum.Current, DataRow)
                        'assign new key to any escape char data that was just added to new POSWriter
                        If newPOSFileWriterKey <> -1 And _currentAction = FormAction.Create Then
                            currentRow.Item("POSFileWriterKey") = newPOSFileWriterKey
                        End If
                        'remove any row where the escape char is blanked out.  
                        If currentRow.Item(1).ToString = "" Then
                            exMessage = "Escape Character Must Not Be Blank"
                            Throw New Exception()
                        End If
                    End While


                    Dim dao As New POSWriterDAO
                    dao.SaveEscapeCharData(_escapeCharDataSet, posWriter)
                Catch ex As DBConcurrencyException
                    BindPropertiesTabData()
                    MessageBox.Show(ResourcesCommon.GetString("msg_concurrencyError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex2 As Exception
                    'TODO log/handle exception
                    MessageBox.Show(exMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If

            ' Raise the save event - allows the data on the parent form to be refreshed
            RaiseEvent UpdateCallingForm()

            success = True
        Else
            success = False
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

    ''' <summary>
    ''' The user selected the Ok button on the Properties tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Cancel button on the Properties tab.
    ''' Do not save the changes.
    ''' Close the form and return focus to the View Writers form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_Button_Cancel.Click
        ' Do not prompt the user to save changes on closing
        Properties_Button_Apply.Enabled = False
        ' Close the child window
        Me.Close()
    End Sub

    ''' <summary>
    ''' The user selected the Apply button on the Properties tab.
    ''' Save the changes but do not close the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_Apply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_Button_Apply.Click
        If ApplyChanges() Then
            ' The Apply button is no longer enabled
            Properties_Button_Apply.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Open a separate form to allow the user to manage the default batch id values
    ''' for the POS system.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_DefaultBatchIds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_Button_DefaultBatchIds.Click
        Logger.LogDebug("Properties_Button_DefaultBatchIds_Click entry", Me.GetType())
        ' Bring focus to the form
        editBatchIdDefaults = New Form_EditBatchIdDefaults()
        ' Populate the edit form with the values from the selected writer
        editBatchIdDefaults.InputWriterConfig = _inputWriterConfig
        ' Show the form
        editBatchIdDefaults.ShowDialog(Me)
        editBatchIdDefaults.Dispose()
        Logger.LogDebug("Properties_Button_DefaultBatchIds_Click exit", Me.GetType())
    End Sub

#End Region

#Region "Data Change events"
    ''' <summary>
    ''' Enable the Apply button whenever a change is made to the form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnableApplyButton()
        Properties_Button_Apply.Enabled = True
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_WriterClassVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_POSSectionHeader_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_POSSectionHeader.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_DelimCharVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_TextBox_DelimCharVal.TextChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_EnforceDictionary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_EnforceDictionary.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_FixedWidthVal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_FixedWidthVal.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_LeadingDelimiter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_LeadingDelimiter.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_TrailingDelimiter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_TrailingDelimiter.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_CheckBox_FieldIdDelimiter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_CheckBox_FieldIdDelimiter.CheckedChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_TaxFlagTrueVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_TextBox_TaxFlagTrueVal.TextChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_TaxFlagFalseVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_TextBox_TaxFlagFalseVal.TextChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' activates 'Apply' button when a cell is updated
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_EscapeChars_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_EscapeChars.AfterCellUpdate
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' sets the POSFileWriterKey value for all new rows added to the data grid so this value will get saved to the database
    ''' also activates 'Apply' button 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_EscapeChars_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles UltraGrid_EscapeChars.AfterRowInsert
        e.Row.Cells("POSFileWriterKey").Value = _inputWriterConfig.POSFileWriterKey
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' activates 'Apply' button when a row is deleted 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UltraGrid_EscapeChars_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid_EscapeChars.AfterRowsDeleted
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_MinBatchId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_TextBox_MinBatchId.TextChanged
        EnableApplyButton()
    End Sub

    ''' <summary>
    ''' The user updated a value on the form.  Enable the Apply button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_TextBox_MaxBatchId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_TextBox_MaxBatchId.TextChanged
        EnableApplyButton()
    End Sub

#End Region

#End Region

#Region "Change Type tab events"
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

#Region "OK Buttons"
    ''' <summary>
    ''' The user selected the Ok button on the Item Id Add tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Item Id Delete tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Item Data Change tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Item Delete tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Promotional Data tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Vendor Adds tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    Private Sub ItemDataZoneChange_Button_OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    Private Sub ShelfTag_OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_OK_Button.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    Private Sub NutriFacts_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

    Private Sub ExtraText_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_OK.Click
        ProcessOKButton_Click(sender, e)
    End Sub

#End Region

#Region "Change selected row events"
    ''' <summary>
    ''' The user has selected a different row to work with for the given POSFileWriterKey / POSChangeTypeKey combination.
    ''' Refesh the data grid.
    ''' </summary>
    ''' <param name="changeType"></param>
    ''' <param name="comboBox"></param>
    ''' <param name="dataGrid"></param>
    ''' <remarks></remarks>
    Private Sub ProcessChangeSelectedRow(ByVal changeType As POSChangeType, ByRef comboBox As ComboBox, ByRef dataGrid As DataGridView)
        ' Refresh the data on the tab for the selected row
        If (processSelectedRow) Then
            If (comboBox.SelectedValue IsNot Nothing) Then
                Dim rowSelection As Integer = CType(comboBox.SelectedValue, Integer)
                PopulatePOSWriterFileConfigData(comboBox, dataGrid, POSChangeTypeDAO.GetChangeType(changeType), rowSelection, False)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIDAdd_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.ItemIDAdd, ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems)
        Else
            ProcessChangeSelectedRow(POSChangeType.CorpScaleItemIdAdd, ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems)
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.ItemIDDelete, ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems)
        Else
            ProcessChangeSelectedRow(POSChangeType.CorpScaleItemIdDelete, ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems)
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Item Data Changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.ItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessChangeSelectedRow(POSChangeType.CorpScaleItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessChangeSelectedRow(POSChangeType.ZoneScalePriceChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessChangeSelectedRow(POSChangeType.ZoneScaleSmartXPriceChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessChangeSelectedRow(POSChangeType.CorpScaleItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.ItemDelete, ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems)
        Else
            ProcessChangeSelectedRow(POSChangeType.ZoneScaleItemDelete, ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems)
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.PromotionalData, PromotionalData_ComboBox_Rows, PromotionalData_DataGridView_ConfigItems)
        End If
    End Sub

    ''' <summary>
    ''' Process a selected row change for Vendor Add Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.VendorAdd, VendorAdd_ComboBox_Rows, VendorAdd_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub ItemDataZoneChange_ComboBox_Rows_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_ComboBox_Rows.SelectedIndexChanged
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessChangeSelectedRow(POSChangeType.ItemChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessChangeSelectedRow(POSChangeType.CorpScaleItemChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessChangeSelectedRow(POSChangeType.ZoneScalePriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessChangeSelectedRow(POSChangeType.ZoneScaleSmartXPriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessChangeSelectedRow(POSChangeType.ZoneScalePriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            End If
        End If
    End Sub

    Private Sub ShelfTag_CurrentRow_ComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_CurrentRow_ComboBox.SelectedIndexChanged
        ProcessChangeSelectedRow(POSChangeType.ShelfTagFile, ShelfTag_CurrentRow_ComboBox, ShelfTag_DataGridView_ConfigItems)
    End Sub

    Private Sub NutriFacts_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_ComboBox_Rows.SelectedIndexChanged
        ProcessChangeSelectedRow(POSChangeType.NutriFact, NutriFacts_ComboBox_Rows, NutriFacts_DataGridView_ConfigItems)
    End Sub

    Private Sub ExtraText_ComboBox_Rows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_ComboBox_Rows.SelectedIndexChanged
        ProcessChangeSelectedRow(POSChangeType.ExtraText, NutriFacts_ComboBox_Rows, ExtraText_DataGridView_ConfigItems)
    End Sub


#End Region

#Region "Add Row events"
    Private Sub ProcessAddRow(ByVal changeType As POSChangeType, ByRef comboBox As ComboBox, ByRef dataGrid As DataGridView)
        Logger.LogDebug("ProcessAddRow entry", Me.GetType())
        processSelectedRow = False

        If comboBox.Items.Count <> (comboBox.SelectedIndex + 1) Then
            'inform user they must be on the max row to add a new one
            MessageBox.Show(ResourcesAdministration.GetString("msg_warning_addChangeTypeRowInvalid"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            If dataGrid.RowCount <= 0 Then
                MessageBox.Show(ResourcesAdministration.GetString("msg_warning_addChangeTypeRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                ' Add a new value to the row selection options and change to an empty data grid
                Dim currentRowCount As Integer = comboBox.Items.Count + 1 ' the number of items in the row selection box equals the # of rows
                PopulatePOSWriterFileConfigData(comboBox, dataGrid, POSChangeTypeDAO.GetChangeType(changeType), currentRowCount, True, True)
                ' Select the newly added row in the combo box
                comboBox.SelectedValue = currentRowCount
            End If
        End If

        processSelectedRow = True
        Logger.LogDebug("ProcessAddRow exit", Me.GetType())
    End Sub

    Private Sub ItemIdAdd_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.ItemIDAdd, ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems)
        Else
            ProcessAddRow(POSChangeType.CorpScaleItemIdAdd, ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub ItemIdDelete_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.ItemIDDelete, ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems)
        Else
            ProcessAddRow(POSChangeType.CorpScaleItemIdDelete, ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub ItemDataChange_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.ItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddRow(POSChangeType.CorpScaleItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddRow(POSChangeType.ZoneScalePriceChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddRow(POSChangeType.ZoneScaleSmartXPriceChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddRow(POSChangeType.CorpScaleItemChange, ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems)
            End If
        End If
    End Sub

    Private Sub ItemDelete_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.ItemDelete, ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems)
        Else
            ProcessAddRow(POSChangeType.ZoneScaleItemDelete, ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub PromotionalData_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.PromotionalData, PromotionalData_ComboBox_Rows, PromotionalData_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub VendorAdd_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.VendorAdd, VendorAdd_ComboBox_Rows, VendorAdd_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_AddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_AddRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddRow(POSChangeType.ItemChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddRow(POSChangeType.CorpScaleItemChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddRow(POSChangeType.ZoneScalePriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddRow(POSChangeType.ZoneScaleSmartXPriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddRow(POSChangeType.ZoneScalePriceChange, ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems)
            End If
        End If
    End Sub

    Private Sub ShelfTag_AddNewRow_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_AddNewRow_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessAddRow(POSChangeType.ShelfTagFile, ShelfTag_CurrentRow_ComboBox, ShelfTag_DataGridView_ConfigItems)
        End If
    End Sub

    Private Sub NutriFacts_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_AddRow.Click
        ProcessAddRow(POSChangeType.NutriFact, NutriFacts_ComboBox_Rows, NutriFacts_DataGridView_ConfigItems)
    End Sub

    Private Sub ExtraText_Button_AddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_AddRow.Click
        ProcessAddRow(POSChangeType.ExtraText, ExtraText_ComboBox_Rows, ExtraText_DataGridView_ConfigItems)
    End Sub

#End Region

#Region "Delete row events"
    ''' <summary>
    ''' delete row processing
    ''' </summary>
    ''' <param name="changeType"></param>
    ''' <param name="comboBox"></param>
    ''' <remarks></remarks>
    Private Sub ProcessDeleteRow(ByVal changeType As POSChangeType, ByRef comboBox As ComboBox)
        Logger.LogDebug("ProcessDeleteRow entry", Me.GetType())
        'confirm delete
        Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_confirmDeletePOSWriterFileConfigRow"), CType(comboBox.SelectedValue, Integer)), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = Windows.Forms.DialogResult.Yes Then
            'delete row
            Dim writerConfig As New POSWriterFileConfigBO
            writerConfig.RowOrder = CType(comboBox.SelectedValue, Integer)
            writerConfig.POSChangeTypeKey = changeType
            writerConfig.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
            POSWriterDAO.DeletePOSWriterFileConfigRow(writerConfig)

            'refresh row drop down
            RefreshRowComboBox(comboBox, POSChangeTypeDAO.GetChangeType(changeType), False)
        End If
        Logger.LogDebug("ProcessDeleteRow exit", Me.GetType())
    End Sub

    Private Sub ItemIdAdd_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.ItemIDAdd, Me.ItemIDAdd_ComboBox_Rows)
        Else
            ProcessDeleteRow(POSChangeType.CorpScaleItemIdAdd, Me.ItemIDAdd_ComboBox_Rows)
        End If
    End Sub

    Private Sub ItemIdDelete_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.ItemIDDelete, Me.ItemIdDelete_ComboBox_Rows)
        Else
            ProcessDeleteRow(POSChangeType.CorpScaleItemIdDelete, Me.ItemIdDelete_ComboBox_Rows)
        End If
    End Sub

    Private Sub ItemDataChange_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.ItemChange, Me.ItemDataChange_ComboBox_Rows)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessDeleteRow(POSChangeType.CorpScaleItemChange, Me.ItemDataChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessDeleteRow(POSChangeType.ZoneScalePriceChange, Me.ItemDataChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessDeleteRow(POSChangeType.ZoneScaleSmartXPriceChange, Me.ItemDataChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessDeleteRow(POSChangeType.CorpScaleItemChange, Me.ItemDataChange_ComboBox_Rows)
            End If
        End If
    End Sub

    Private Sub ItemDelete_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.ItemDelete, Me.ItemDelete_ComboBox_Rows)
        Else
            ProcessDeleteRow(POSChangeType.ZoneScaleItemDelete, Me.ItemDelete_ComboBox_Rows)
        End If
    End Sub

    Private Sub PromotionalData_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.PromotionalData, Me.PromotionalData_ComboBox_Rows)
        End If
    End Sub

    Private Sub VendorAdd_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.VendorAdd, Me.VendorAdd_ComboBox_Rows)
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_DeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_DeleteRow.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteRow(POSChangeType.ItemChange, Me.ItemDataZoneChange_ComboBox_Rows)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessDeleteRow(POSChangeType.CorpScaleItemChange, Me.ItemDataZoneChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessDeleteRow(POSChangeType.ZoneScalePriceChange, Me.ItemDataZoneChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessDeleteRow(POSChangeType.ZoneScaleSmartXPriceChange, Me.ItemDataZoneChange_ComboBox_Rows)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessDeleteRow(POSChangeType.ZoneScalePriceChange, Me.ItemDataZoneChange_ComboBox_Rows)
            End If
        End If
    End Sub

    Private Sub ShelfTag_DeleteRow_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_DeleteRow_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessDeleteRow(POSChangeType.ShelfTagFile, ShelfTag_CurrentRow_ComboBox)
        End If

    End Sub

    Private Sub NutriFacts_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_DeleteRow.Click
        ProcessDeleteRow(POSChangeType.NutriFact, Me.NutriFacts_ComboBox_Rows)
    End Sub

    Private Sub ExtraText_Button_DeleteRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_DeleteRow.Click
        ProcessDeleteRow(POSChangeType.ExtraText, Me.ExtraText_ComboBox_Rows)
    End Sub

#End Region

#Region "Reorder row events"
    Private Sub ProcessEditRowOrder(ByVal changeType As POSChangeType)
        ' TODO: Reorder row processing
        DisplayErrorMessage("-- not yet implemented --")
    End Sub

    Private Sub ItemIdAdd_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.ItemIDAdd)
        Else
            ProcessEditRowOrder(POSChangeType.CorpScaleItemIdAdd)
        End If
    End Sub

    Private Sub ItemIdDelete_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.ItemIDDelete)
        Else
            ProcessEditRowOrder(POSChangeType.CorpScaleItemIdDelete)
        End If
    End Sub

    Private Sub ItemDataChange_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditRowOrder(POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditRowOrder(POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditRowOrder(POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditRowOrder(POSChangeType.CorpScaleItemChange)
            End If
        End If
    End Sub

    Private Sub ItemDelete_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.ItemDelete)
        Else
            ProcessEditRowOrder(POSChangeType.ZoneScaleItemDelete)
        End If
    End Sub

    Private Sub PromotionalData_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.PromotionalData)
        End If
    End Sub

    Private Sub VendorAdd_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.VendorAdd)
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_EditRowOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_EditRowOrder.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditRowOrder(POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditRowOrder(POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditRowOrder(POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditRowOrder(POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditRowOrder(POSChangeType.ZoneScalePriceChange)
            End If
        End If
    End Sub

    Private Sub ShelfTag_Reorder_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_Reorder_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessEditRowOrder(POSChangeType.ShelfTagFile)
        End If

    End Sub

    Private Sub NutriFacts_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_EditRowOrder.Click
        ProcessEditRowOrder(POSChangeType.NutriFact)
    End Sub

    Private Sub ExtraText_Button_EditRowOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_EditRowOrder.Click
        ProcessEditRowOrder(POSChangeType.ExtraText)
    End Sub

#End Region

#Region "Edit Column events"
    ''' <summary>
    ''' Handles the common Edit Selected Column processing for all change types. 
    ''' </summary>
    ''' <param name="dataGrid"></param>
    ''' <remarks></remarks>
    Private Sub ProcessEditColumn(ByRef dataGrid As DataGridView, ByVal changeType As POSChangeType)
        Logger.LogDebug("ProcessEditColumn entry", Me.GetType())
        Try

            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(dataGrid)
            If selectedRow IsNot Nothing Then
                ' Get the data for the selected row
                Dim selectedConfigData As New POSWriterFileConfigBO(selectedRow)
                Dim changeTypeConfig As POSChangeTypeBO = POSChangeTypeDAO.GetChangeType(changeType)

                If selectedConfigData.IsBinaryInt Then
                    ' Display the Edit Binary Int form
                    editPOSWriterBinaryIntColumnForm = New Form_EditPOSWriterFileConfigBinaryInt()
                    editPOSWriterBinaryIntColumnForm.CurrentAction = FormAction.Edit

                    ' Populate the edit form with the values from the selected row
                    editPOSWriterBinaryIntColumnForm.InputColumnData = selectedConfigData
                    editPOSWriterBinaryIntColumnForm.InputColumnData.POSChangeTypeKey = changeTypeConfig.POSChangeTypeKey
                    editPOSWriterBinaryIntColumnForm.InputColumnData.ChangeTypeDesc = changeTypeConfig.ChangeTypeDesc
                    editPOSWriterBinaryIntColumnForm.InputColumnData.POSDataTypeKey = changeTypeConfig.POSDataTypeKey
                    editPOSWriterBinaryIntColumnForm.InputColumnData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
                    editPOSWriterBinaryIntColumnForm.InputColumnData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
                    editPOSWriterBinaryIntColumnForm.InputColumnData.EnforceDictionary = _inputWriterConfig.EnforceDictionary
                    editPOSWriterBinaryIntColumnForm.InputColumnData.FixedWidth = _inputWriterConfig.FixedWidth
                    editPOSWriterBinaryIntColumnForm.InputColumnData.AppendToFile = _inputWriterConfig.AppendToFile

                    ' Show the form
                    editPOSWriterBinaryIntColumnForm.ShowDialog(Me)
                    editPOSWriterBinaryIntColumnForm.Dispose()
                Else
                    ' Display the Edit form
                    editPOSWriterColumnForm = New Form_EditPOSWriterFileConfig()
                    editPOSWriterColumnForm.CurrentAction = FormAction.Edit

                    ' Populate the edit form with the values from the selected row
                    editPOSWriterColumnForm.InputColumnData = selectedConfigData
                    editPOSWriterColumnForm.InputColumnData.POSChangeTypeKey = changeTypeConfig.POSChangeTypeKey
                    editPOSWriterColumnForm.InputColumnData.ChangeTypeDesc = changeTypeConfig.ChangeTypeDesc
                    editPOSWriterColumnForm.InputColumnData.POSDataTypeKey = changeTypeConfig.POSDataTypeKey
                    editPOSWriterColumnForm.InputColumnData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
                    editPOSWriterColumnForm.InputColumnData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
                    editPOSWriterColumnForm.InputColumnData.EnforceDictionary = _inputWriterConfig.EnforceDictionary
                    editPOSWriterColumnForm.InputColumnData.FixedWidth = _inputWriterConfig.FixedWidth
                    editPOSWriterColumnForm.InputColumnData.AppendToFile = _inputWriterConfig.AppendToFile

                    ' Show the form
                    editPOSWriterColumnForm.ShowDialog(Me)
                    editPOSWriterColumnForm.Dispose()
                End If
            End If
        Catch ex As Exception
            Logger.LogError("ProcessEditColumn exception when getting type=Form_EditPOSWriterFileConfigColumn or type=Form_EditPOSWriterFileConfigBinaryInt", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessEditColumn exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Item Id Adds data grid.
    ''' This is the same action as Column -> Edit Selected Column.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ItemIdAdd_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.ItemIDAdd)
        Else
            ProcessEditColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdAdd)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Item Id Adds
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.ItemIDAdd)
        Else
            ProcessEditColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdAdd)
        End If
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Item Id Deletes data grid.
    ''' This is the same action as Column -> Edit Selected Column.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ItemIdDelete_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.ItemIDDelete)
        Else
            ProcessEditColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Item Id Deletes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.ItemIDDelete)
        Else
            ProcessEditColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Item Data Changes data grid.
    ''' This is the same action as Column -> Edit Selected Column.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ItemDataChange_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Item DataChanges
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Item Deletes data grid.
    ''' This is the same action as Column -> Edit Selected Column.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ItemDelete_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ItemDelete)
        Else
            ProcessEditColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ZoneScaleItemDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Item Deletes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ItemDelete)
        Else
            ProcessEditColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ZoneScaleItemDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Item Deletes data grid.
    ''' This is the same action as Column -> Edit Selected Column.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles PromotionalData_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(PromotionalData_DataGridView_ConfigItems, POSChangeType.PromotionalData)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Promotional Data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(PromotionalData_DataGridView_ConfigItems, POSChangeType.PromotionalData)
        End If
    End Sub

    ''' <summary>
    ''' The user double-clicked on a row in the Vendor Adds data grid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles VendorAdd_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(VendorAdd_DataGridView_ConfigItems, POSChangeType.VendorAdd)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Edit Column button for Vendor Adds
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(VendorAdd_DataGridView_ConfigItems, POSChangeType.VendorAdd)
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_EditCol_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_EditCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            End If
        End If
    End Sub

    Private Sub ItemDataZoneChange_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ItemDataZoneChange_DataGridView_ConfigItems.CellDoubleClick
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessEditColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            End If
        End If
    End Sub

    Private Sub ShelfTag_EditCol_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_EditCol_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessEditColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ShelfTagFile)
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ProcessEditColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ElectronicShelfTag)
        End If
    End Sub

    Private Sub ShelfTag_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ShelfTag_DataGridView_ConfigItems.CellDoubleClick
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Or _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ProcessEditColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ShelfTagFile)
        End If
    End Sub

    Private Sub NutriFacts_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles NutriFacts_DataGridView_ConfigItems.CellDoubleClick
        ProcessEditColumn(NutriFacts_DataGridView_ConfigItems, POSChangeType.NutriFact)
    End Sub

    Private Sub NutriFacts_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_EditCol.Click
        ProcessEditColumn(NutriFacts_DataGridView_ConfigItems, POSChangeType.NutriFact)
    End Sub

    Private Sub ExtraText_DataGridView_ConfigItems_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExtraText_DataGridView_ConfigItems.CellDoubleClick
        ProcessEditColumn(ExtraText_DataGridView_ConfigItems, POSChangeType.ExtraText)
    End Sub

    Private Sub ExtraText_Button_EditCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_EditCol.Click
        ProcessEditColumn(ExtraText_DataGridView_ConfigItems, POSChangeType.ExtraText)
    End Sub

#End Region

#Region "Add Column events"
    ''' <summary>
    ''' Handles the common Add Selected Column processing for all change types. 
    ''' </summary>
    ''' <param name="dataGrid"></param>
    ''' <remarks></remarks>
    Private Sub ProcessAddColumn(ByRef dataGrid As DataGridView, ByVal changeType As POSChangeType, ByVal currentRowNum As Integer)
        Logger.LogDebug("ProcessAddColumn entry", Me.GetType())
        Dim maxColumnNum As Integer
        Try
            'get the max column in the writer (since grid displays 8 rows for bit columns)
            If dataGrid.RowCount = 0 Then
                maxColumnNum = 0
            Else
                maxColumnNum = CType(dataGrid.Rows(dataGrid.RowCount - 1).Cells("ColumnOrder").Value, Integer)
            End If
            ' Bring focus to the form
            editPOSWriterColumnForm = New Form_EditPOSWriterFileConfig()
            editPOSWriterColumnForm.CurrentAction = FormAction.Create
            ' Populate the edit form with the values to create a new column
            editPOSWriterColumnForm.InputColumnData = New POSWriterFileConfigBO()
            editPOSWriterColumnForm.InputColumnData.RowOrder = currentRowNum
            editPOSWriterColumnForm.InputColumnData.ColumnOrder = maxColumnNum + 1
            Dim changeTypeConfig As POSChangeTypeBO = POSChangeTypeDAO.GetChangeType(changeType)
            editPOSWriterColumnForm.InputColumnData.POSChangeTypeKey = changeTypeConfig.POSChangeTypeKey
            editPOSWriterColumnForm.InputColumnData.ChangeTypeDesc = changeTypeConfig.ChangeTypeDesc
            editPOSWriterColumnForm.InputColumnData.POSDataTypeKey = changeTypeConfig.POSDataTypeKey
            editPOSWriterColumnForm.InputColumnData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
            editPOSWriterColumnForm.InputColumnData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
            editPOSWriterColumnForm.InputColumnData.EnforceDictionary = _inputWriterConfig.EnforceDictionary
            editPOSWriterColumnForm.InputColumnData.FixedWidth = _inputWriterConfig.FixedWidth
            editPOSWriterColumnForm.InputColumnData.AppendToFile = _inputWriterConfig.AppendToFile

            ' Show the form
            editPOSWriterColumnForm.ShowDialog(Me)
            editPOSWriterColumnForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessAddColumn exception when getting type=Form_EditPOSWriterFileConfigColumn", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessAddColumn exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.ItemIDAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.ItemIDDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Item DataChanges.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ZoneScaleItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(PromotionalData_DataGridView_ConfigItems, POSChangeType.PromotionalData, CType(PromotionalData_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Column button for Vendor Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(VendorAdd_DataGridView_ConfigItems, POSChangeType.VendorAdd, CType(VendorAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_AddCol_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_AddCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    Private Sub ShelfTag_AddNewCol_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_AddNewCol_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessAddColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ShelfTagFile, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ProcessAddColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ElectronicShelfTag, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        End If
    End Sub

    Private Sub NutriFacts_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_AddCol.Click
        ProcessAddColumn(NutriFacts_DataGridView_ConfigItems, POSChangeType.NutriFact, CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer))
    End Sub

    Private Sub ExtraText_Button_AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_AddCol.Click
        ProcessAddColumn(ExtraText_DataGridView_ConfigItems, POSChangeType.ExtraText, CType(ExtraText_ComboBox_Rows.SelectedValue, Integer))
    End Sub

#End Region

#Region "Add Binary Column events"
    ''' <summary>
    ''' Handles the common Add Selected Binary Column processing for all change types. 
    ''' </summary>
    ''' <param name="dataGrid"></param>
    ''' <remarks></remarks>
    Private Sub ProcessAddBinaryColumn(ByRef dataGrid As DataGridView, ByVal changeType As POSChangeType, ByVal currentRowNum As Integer)
        Logger.LogDebug("ProcessAddBinaryColumn entry", Me.GetType())
        Dim maxColumnNum As Integer
        Try
            'get the max column in the writer (since grid displays 8 rows for bit columns)
            If dataGrid.RowCount = 0 Then
                maxColumnNum = 0
            Else
                maxColumnNum = CType(dataGrid.Rows(dataGrid.RowCount - 1).Cells("ColumnOrder").Value, Integer)
            End If
            ' Bring focus to the form
            editPOSWriterBinaryIntColumnForm = New Form_EditPOSWriterFileConfigBinaryInt()
            editPOSWriterBinaryIntColumnForm.CurrentAction = FormAction.Create
            ' Populate the edit form with the values to create a new column
            editPOSWriterBinaryIntColumnForm.InputColumnData = New POSWriterFileConfigBO()
            editPOSWriterBinaryIntColumnForm.InputColumnData.RowOrder = currentRowNum
            editPOSWriterBinaryIntColumnForm.InputColumnData.ColumnOrder = maxColumnNum + 1
            Dim changeTypeConfig As POSChangeTypeBO = POSChangeTypeDAO.GetChangeType(changeType)
            editPOSWriterBinaryIntColumnForm.InputColumnData.POSChangeTypeKey = changeTypeConfig.POSChangeTypeKey
            editPOSWriterBinaryIntColumnForm.InputColumnData.ChangeTypeDesc = changeTypeConfig.ChangeTypeDesc
            editPOSWriterBinaryIntColumnForm.InputColumnData.POSDataTypeKey = changeTypeConfig.POSDataTypeKey
            editPOSWriterBinaryIntColumnForm.InputColumnData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
            editPOSWriterBinaryIntColumnForm.InputColumnData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
            editPOSWriterBinaryIntColumnForm.InputColumnData.EnforceDictionary = _inputWriterConfig.EnforceDictionary
            editPOSWriterBinaryIntColumnForm.InputColumnData.FixedWidth = _inputWriterConfig.FixedWidth
            editPOSWriterBinaryIntColumnForm.InputColumnData.AppendToFile = _inputWriterConfig.AppendToFile
            editPOSWriterBinaryIntColumnForm.InputColumnData.IsBinaryInt = True

            ' Show the form
            editPOSWriterBinaryIntColumnForm.ShowDialog(Me)
            editPOSWriterBinaryIntColumnForm.Dispose()
        Catch ex As Exception
            Logger.LogError("ProcessAddBinaryColumn exception when getting type=Form_EditPOSWriterFileConfigColumn", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessAddBinaryColumn exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Vendor Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(VendorAdd_DataGridView_ConfigItems, POSChangeType.VendorAdd, CType(VendorAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(PromotionalData_DataGridView_ConfigItems, POSChangeType.PromotionalData, CType(PromotionalData_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddBinaryColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ZoneScaleItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Item DataChanges.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddBinaryColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddBinaryColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddBinaryColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddBinaryColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.ItemIDDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddBinaryColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Add New Binary Column button for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.ItemIDAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessAddBinaryColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_AddColBinary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_AddColBinary.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessAddBinaryColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessAddBinaryColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessAddBinaryColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessAddBinaryColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessAddBinaryColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    Private Sub ShelfTag_AddBitCol_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_AddBitCol_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessAddBinaryColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ShelfTagFile, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessAddBinaryColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ElectronicShelfTag, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        End If
    End Sub

    Private Sub NutriFacts_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_AddColBinary.Click
        ProcessAddBinaryColumn(NutriFacts_DataGridView_ConfigItems, POSChangeType.NutriFact, CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer))
    End Sub

    Private Sub ExtraText_Button_AddColBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_AddColBinary.Click
        ProcessAddBinaryColumn(ExtraText_DataGridView_ConfigItems, POSChangeType.ExtraText, CType(ExtraText_ComboBox_Rows.SelectedValue, Integer))
    End Sub
#End Region

#Region "Delete Column events"
    ''' <summary>
    ''' Handles the common Delete Selected Column processing for all change types. 
    ''' </summary>
    ''' <param name="dataGrid"></param>
    ''' <remarks></remarks>
    Private Sub ProcessDeleteColumn(ByRef dataGrid As DataGridView, ByVal changeType As POSChangeType)
        Logger.LogDebug("ProcessDeleteColumn entry", Me.GetType())
        Try
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = getSelectedRow(dataGrid)
            If selectedRow IsNot Nothing Then
                ' Bring focus to the form
                deletePOSWriterColumnForm = New Form_DeletePOSWriterFileConfig()
                ' Populate the delete form with the values from the selected row
                deletePOSWriterColumnForm.InputColumnData = New POSWriterFileConfigBO(selectedRow)
                Dim changeTypeConfig As POSChangeTypeBO = POSChangeTypeDAO.GetChangeType(changeType)
                deletePOSWriterColumnForm.InputColumnData.POSChangeTypeKey = changeTypeConfig.POSChangeTypeKey
                deletePOSWriterColumnForm.InputColumnData.ChangeTypeDesc = changeTypeConfig.ChangeTypeDesc
                deletePOSWriterColumnForm.InputColumnData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
                deletePOSWriterColumnForm.InputColumnData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
                ' Show the form
                deletePOSWriterColumnForm.ShowDialog(Me)
                deletePOSWriterColumnForm.Dispose()
            End If
        Catch ex As Exception
            Logger.LogError("ProcessDeleteColumn exception when getting type=Form_DeletePOSWriterFileConfigColumn", Me.GetType(), ex)
            DisplayErrorMessage()
        End Try
        Logger.LogDebug("ProcessDeleteColumn exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.ItemIDAdd)
        Else
            ProcessDeleteColumn(ItemIdAdd_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdAdd)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.ItemIDDelete)
        Else
            ProcessDeleteColumn(ItemIdDelete_DataGridView_ConfigItems, POSChangeType.CorpScaleItemIdDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Item DataChanges.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessDeleteColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessDeleteColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessDeleteColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessDeleteColumn(ItemDataChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ItemDelete)
        Else
            ProcessDeleteColumn(ItemDelete_DataGridView_ConfigItems, POSChangeType.ZoneScaleItemDelete)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(PromotionalData_DataGridView_ConfigItems, POSChangeType.PromotionalData)
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Delete Column button for Vendor Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(VendorAdd_DataGridView_ConfigItems, POSChangeType.VendorAdd)
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_DeleteCol_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_DeleteCol.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessDeleteColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ItemChange)
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessDeleteColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.CorpScaleItemChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessDeleteColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessDeleteColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScaleSmartXPriceChange)
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessDeleteColumn(ItemDataZoneChange_DataGridView_ConfigItems, POSChangeType.ZoneScalePriceChange)
            End If
        End If
    End Sub

    Private Sub ShelfTag_DeleteCol_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_DeleteCol_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessDeleteColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ShelfTagFile)
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ProcessDeleteColumn(ShelfTag_DataGridView_ConfigItems, POSChangeType.ElectronicShelfTag)
        End If
    End Sub

    Private Sub NutriFacts_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_DeleteCol.Click
        ProcessDeleteColumn(NutriFacts_DataGridView_ConfigItems, POSChangeType.NutriFact)
    End Sub

    Private Sub ExtraText_Button_DeleteCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_DeleteCol.Click
        ProcessDeleteColumn(ExtraText_DataGridView_ConfigItems, POSChangeType.ExtraText)
    End Sub
#End Region

#Region "Reorder Column events"
    ''' <summary>
    ''' Handles the common column reorder processing for all change types. 
    ''' </summary>
    ''' <param name="comboBox"></param>
    ''' <param name="dataGrid"></param>
    ''' <param name="moveUp"></param>
    ''' <param name="changeType"></param>
    ''' <param name="selectedRowNum"></param>
    ''' <remarks></remarks>
    Private Sub ProcessReorderColumn(ByRef comboBox As ComboBox, ByRef dataGrid As DataGridView, ByVal moveUp As Boolean, ByVal changeType As POSChangeType, ByVal selectedRowNum As Integer)
        Logger.LogDebug("ProcessReorderColumn entry", Me.GetType())
        ' Get the selected row
        Dim selectedRow As DataGridViewRow = getSelectedRow(dataGrid)
        Dim newRowNum As Integer
        Dim maxRowNum As Integer
        Dim maxColumnNum As Integer
        Dim currentColumnNum As Integer

        If selectedRow IsNot Nothing Then
            ' get the selected column number
            currentColumnNum = CType(selectedRow.Cells("ColumnOrder").Value, Integer)

            'get the max rows in the grid
            maxRowNum = dataGrid.Rows.Count - 1

            'get the max column in the writer (since grid displays 8 rows for bit columns)
            maxColumnNum = CType(dataGrid.Rows(maxRowNum).Cells("ColumnOrder").Value, Integer)

            ' Verify the action can be performed on the selected row
            If moveUp AndAlso selectedRow.Index = 0 Then
                DisplayErrorMessage("Unable to move the first row up.")
                Return
            ElseIf Not moveUp AndAlso selectedRow.Index = maxRowNum Then
                DisplayErrorMessage("Unable to move the last row down.")
                Return
            End If

            ' Verify the action can be performed on the selected column for binary int rows
            If moveUp AndAlso currentColumnNum = 1 Then
                DisplayErrorMessage("Unable to move the first column up.")
                Return
            ElseIf Not moveUp AndAlso currentColumnNum = maxColumnNum Then
                DisplayErrorMessage("Unable to move the last column down.")
                Return
            End If

            If moveUp Then
                newRowNum = selectedRow.Index - 1
            Else
                newRowNum = selectedRow.Index + 1
            End If

            ' Populate the config data for the row being moved
            Dim configData As New POSWriterFileConfigBO(selectedRow)
            Dim changeConfig As POSChangeTypeBO = POSChangeTypeDAO.GetChangeType(changeType)
            configData.POSChangeTypeKey = changeConfig.POSChangeTypeKey
            configData.ChangeTypeDesc = changeConfig.ChangeTypeDesc
            configData.POSDataTypeKey = changeConfig.POSDataTypeKey
            configData.POSFileWriterKey = _inputWriterConfig.POSFileWriterKey
            configData.POSFileWriterCode = _inputWriterConfig.POSFileWriterCode
            configData.EnforceDictionary = _inputWriterConfig.EnforceDictionary
            configData.FixedWidth = _inputWriterConfig.FixedWidth
            configData.AppendToFile = _inputWriterConfig.AppendToFile

            ' Reorder the column, changing places with the row directly above or below it
            POSWriterDAO.ReorderPOSWriterFileConfigRecords(configData, changeConfig, moveUp)
            ' Refresh the details tab that displays the data for the change type
            PopulatePOSWriterFileConfigData(comboBox, dataGrid, POSChangeTypeDAO.GetChangeType(changeType), selectedRowNum, False)

            'set focus on new row position
            dataGrid.Rows(newRowNum).Selected = True
        End If

        Logger.LogDebug("ProcessReorderColumn exit", Me.GetType())
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems, True, POSChangeType.ItemIDAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems, True, POSChangeType.CorpScaleItemIdAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Item Id Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdAdd_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdAdd_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems, False, POSChangeType.ItemIDAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemIDAdd_ComboBox_Rows, ItemIdAdd_DataGridView_ConfigItems, False, POSChangeType.CorpScaleItemIdAdd, CType(ItemIDAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems, True, POSChangeType.ItemIDDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems, True, POSChangeType.CorpScaleItemIdDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Item Id Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemIdDelete_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemIdDelete_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems, False, POSChangeType.ItemIDDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemIdDelete_ComboBox_Rows, ItemIdDelete_DataGridView_ConfigItems, False, POSChangeType.CorpScaleItemIdDelete, CType(ItemIdDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Item Data Changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, True, POSChangeType.ItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, True, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, True, POSChangeType.ZoneScalePriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, True, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, True, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Item Data Changes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDataChange_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDataChange_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.ItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.ZoneScalePriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessReorderColumn(ItemDataChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.CorpScaleItemChange, CType(ItemDataChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems, True, POSChangeType.ItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems, True, POSChangeType.ZoneScaleItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Item Deletes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ItemDelete_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemDelete_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems, False, POSChangeType.ItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        Else
            ProcessReorderColumn(ItemDelete_ComboBox_Rows, ItemDelete_DataGridView_ConfigItems, False, POSChangeType.ZoneScaleItemDelete, CType(ItemDelete_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(PromotionalData_ComboBox_Rows, PromotionalData_DataGridView_ConfigItems, True, POSChangeType.PromotionalData, CType(PromotionalData_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Promotional Data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PromotionalData_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PromotionalData_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(PromotionalData_ComboBox_Rows, PromotionalData_DataGridView_ConfigItems, False, POSChangeType.PromotionalData, CType(PromotionalData_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Up buttom to reorder columns for Vendor Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(VendorAdd_ComboBox_Rows, VendorAdd_DataGridView_ConfigItems, True, POSChangeType.VendorAdd, CType(VendorAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    ''' <summary>
    ''' The user selected the Down buttom to reorder columns for Vendor Adds.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub VendorAdd_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VendorAdd_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(VendorAdd_ComboBox_Rows, VendorAdd_DataGridView_ConfigItems, False, POSChangeType.VendorAdd, CType(VendorAdd_ComboBox_Rows.SelectedValue, Integer))
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_Up_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_Up.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, True, POSChangeType.ItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, True, POSChangeType.CorpScaleItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, True, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, True, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, True, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    Private Sub ItemDataZoneChange_Button_Down_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemDataZoneChange_Button_Down.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_SCALE Then
            ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataChange_DataGridView_ConfigItems, False, POSChangeType.ItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
        Else
            If _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_CORPORATE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, False, POSChangeType.CorpScaleItemChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_ZONE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, False, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, False, POSChangeType.ZoneScaleSmartXPriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            ElseIf _inputWriterConfig.ScaleWriterTypeDesc = POSWriterBO.SCALE_WRITER_TYPE_STORE Then
                ProcessReorderColumn(ItemDataZoneChange_ComboBox_Rows, ItemDataZoneChange_DataGridView_ConfigItems, False, POSChangeType.ZoneScalePriceChange, CType(ItemDataZoneChange_ComboBox_Rows.SelectedValue, Integer))
            End If
        End If
    End Sub

    Private Sub ShelfTag_ColUp_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_ColUp_Button.Click
        If _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessReorderColumn(ShelfTag_CurrentRow_ComboBox, ShelfTag_DataGridView_ConfigItems, True, POSChangeType.ShelfTagFile, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        ElseIf _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_ELECTRONICSHELFTAG Then
            ProcessReorderColumn(ShelfTag_CurrentRow_ComboBox, ShelfTag_DataGridView_ConfigItems, True, POSChangeType.ElectronicShelfTag, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        End If
    End Sub

    Private Sub ShelfTag_ColDown_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShelfTag_ColDown_Button.Click
        If Not _inputWriterConfig.WriterType = POSWriterBO.WRITER_TYPE_TAG Then
            ProcessReorderColumn(ShelfTag_CurrentRow_ComboBox, ShelfTag_DataGridView_ConfigItems, False, POSChangeType.ShelfTagFile, CType(ShelfTag_CurrentRow_ComboBox.SelectedValue, Integer))
        End If
    End Sub

    Private Sub NutriFacts_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_Up.Click
        ProcessReorderColumn(NutriFacts_ComboBox_Rows, NutriFacts_DataGridView_ConfigItems, True, POSChangeType.NutriFact, CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer))
    End Sub

    Private Sub NutriFacts_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NutriFacts_Button_Down.Click
        ProcessReorderColumn(NutriFacts_ComboBox_Rows, NutriFacts_DataGridView_ConfigItems, False, POSChangeType.NutriFact, CType(NutriFacts_ComboBox_Rows.SelectedValue, Integer))
    End Sub

    Private Sub ExtraText_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_Up.Click
        ProcessReorderColumn(ExtraText_ComboBox_Rows, ExtraText_DataGridView_ConfigItems, True, POSChangeType.ExtraText, CType(ExtraText_ComboBox_Rows.SelectedValue, Integer))
    End Sub

    Private Sub ExtraText_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtraText_Button_Down.Click
        ProcessReorderColumn(ExtraText_ComboBox_Rows, ExtraText_DataGridView_ConfigItems, False, POSChangeType.ExtraText, CType(ExtraText_ComboBox_Rows.SelectedValue, Integer))
    End Sub
#End Region
#End Region

#End Region

#Region "Populate and format DataGridView objects for the form"
    ''' <summary>
    ''' Populate the row combo box with the existing rows for the given writer / change type.
    ''' </summary>
    ''' <param name="comboBox"></param>
    ''' <param name="changeType"></param>
    ''' <remarks></remarks>
    Private Sub RefreshRowComboBox(ByRef comboBox As ComboBox, ByVal changeType As POSChangeTypeBO)
        RefreshRowComboBox(comboBox, changeType, False)
    End Sub

    ''' <summary>
    ''' Populate the row combo box with the existing rows for the given writer / change type.
    ''' If the addNewRow flag is set to true, a new row is added to the collection.
    ''' </summary>
    ''' <param name="comboBox"></param>
    ''' <param name="changeType"></param>
    ''' <param name="addNewRow"></param>
    ''' <remarks></remarks>
    Private Sub RefreshRowComboBox(ByRef comboBox As ComboBox, ByVal changeType As POSChangeTypeBO, ByVal addNewRow As Boolean)
        Try
            ' Query the database to identify the number of rows available for this change type
            Dim rowCount As Integer
            Try
                rowCount = POSWriterDAO.GetRowCountForWriter(_inputWriterConfig, changeType)
            Catch ex As NullReferenceException
                ' If the row count was NULL, set it to zero
                rowCount = 0
            End Try
            Dim rowList As New ArrayList
            ' Build a row selection entry for each row
            Dim currentRow As Integer
            Dim rowSelectOption As ComboSelection
            If rowCount >= 1 Then
                For currentRow = 1 To rowCount
                    rowSelectOption = New ComboSelection(currentRow, String.Format(ResourcesAdministration.GetString("label_rowNum_withNum"), currentRow))
                    rowList.Add(rowSelectOption)
                Next currentRow
            Else
                ' No rows have been setup yet - default to row 1
                rowSelectOption = New ComboSelection(1, String.Format(ResourcesAdministration.GetString("label_rowNum_withNum"), 1))
                rowList.Add(rowSelectOption)
            End If
            ' Add a new row? - only add row if at least 1 row already exists
            If addNewRow AndAlso rowCount > 0 Then
                rowSelectOption = New ComboSelection(currentRow, String.Format(ResourcesAdministration.GetString("label_rowNum_withNum"), rowCount + 1))
                rowList.Add(rowSelectOption)
            End If
            ' Populate the ComboBox with the row selection
            comboBox.DataSource = rowList
            comboBox.DisplayMember = "Description"
            comboBox.ValueMember = "Value"
        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), e)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditPOSWriter form: RefreshRowComboBox sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

    End Sub

    ''' <summary>
    ''' Populate the DataGridView_ConfigItems with the current data from the database.
    ''' Defaults the addNewRow flag to false.
    ''' </summary>
    ''' <param name="comboBox"></param>
    ''' <param name="dataGrid"></param>
    ''' <param name="changeType"></param>
    ''' <param name="selectedRowNum"></param>
    ''' <param name="refreshRowCount"></param>
    ''' <remarks></remarks>
    Private Sub PopulatePOSWriterFileConfigData(ByRef comboBox As ComboBox, ByRef dataGrid As DataGridView, ByVal changeType As POSChangeTypeBO, ByVal selectedRowNum As Integer, ByVal refreshRowCount As Boolean)
        PopulatePOSWriterFileConfigData(comboBox, dataGrid, changeType, selectedRowNum, refreshRowCount, False)
    End Sub

    ''' <summary>
    ''' Populate the DataGridView_ConfigItems with the current data from the database.
    ''' If the refreshRowCount and addNewRow flags are set to true, a new row is added.
    ''' </summary>
    ''' <param name="comboBox"></param>
    ''' <param name="dataGrid"></param>
    ''' <param name="changeType"></param>
    ''' <param name="selectedRowNum"></param>
    ''' <param name="refreshRowCount"></param>
    ''' <param name="addNewRow"></param>
    ''' <remarks></remarks>
    Private Sub PopulatePOSWriterFileConfigData(ByRef comboBox As ComboBox, ByRef dataGrid As DataGridView, ByVal changeType As POSChangeTypeBO, ByVal selectedRowNum As Integer, ByVal refreshRowCount As Boolean, ByVal addNewRow As Boolean)
        If refreshRowCount Then
            RefreshRowComboBox(comboBox, changeType, addNewRow)
        End If

        ' Read the POSWriterFileConfig data
        Dim _dataSet As DataSet = POSWriterDAO.GetWriterFileConfigurationsByRowAndCol(_inputWriterConfig, changeType, selectedRowNum)

        dataGrid.DataSource = _dataSet.Tables(0)
        dataGrid.MultiSelect = False

        ' Format the view
        ' Make sure at least one entry was returned before configuring the columns
        If (dataGrid.Columns.Count > 0) Then
            dataGrid.Columns("POSFileWriterCode").Visible = False
            dataGrid.Columns("ChangeTypeDesc").Visible = False
            dataGrid.Columns("RowOrder").Visible = False
            dataGrid.Columns("IsTaxFlag").Visible = False
            dataGrid.Columns("IsDecimalValue").Visible = False
            dataGrid.Columns("IsPackedDecimal").Visible = False
            dataGrid.Columns("IsBinaryInt").Visible = False
            dataGrid.Columns("DecimalPrecision").Visible = False
            dataGrid.Columns("IncludeDecimal").Visible = False
            dataGrid.Columns("PadLeft").Visible = False
            dataGrid.Columns("FillChar").Visible = False
            dataGrid.Columns("LeadingChars").Visible = False
            dataGrid.Columns("TrailingChars").Visible = False
            dataGrid.Columns("IsBoolean").Visible = False
            dataGrid.Columns("BooleanTrueChar").Visible = False
            dataGrid.Columns("BooleanFalseChar").Visible = False
            dataGrid.Columns("FixedWidthField").Visible = False
            dataGrid.Columns("PackLength").Visible = False

            dataGrid.Columns("ColumnOrder").DisplayIndex = 0
            dataGrid.Columns("ColumnOrder").HeaderText = ResourcesAdministration.GetString("label_column")
            dataGrid.Columns("ColumnOrder").ReadOnly = True
            dataGrid.Columns("ColumnOrder").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader

            dataGrid.Columns("BitOrder").DisplayIndex = 1
            dataGrid.Columns("BitOrder").HeaderText = ResourcesAdministration.GetString("label_bitOrder")
            dataGrid.Columns("BitOrder").ReadOnly = True
            dataGrid.Columns("BitOrder").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader

            'TODO use literal and tax hosting flag to write out field type (LITERAL, DYNAMIC, TAX HOSTING, BOOLEAN) value
            dataGrid.Columns("IsLiteral").DisplayIndex = 2
            dataGrid.Columns("IsLiteral").HeaderText = ResourcesAdministration.GetString("label_literal")
            dataGrid.Columns("IsLiteral").ReadOnly = True
            dataGrid.Columns("IsLiteral").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader

            dataGrid.Columns("DataElement").DisplayIndex = 3
            dataGrid.Columns("DataElement").HeaderText = ResourcesAdministration.GetString("label_dataElement")
            dataGrid.Columns("DataElement").ReadOnly = True

            dataGrid.Columns("FieldId").DisplayIndex = 4
            dataGrid.Columns("FieldId").HeaderText = ResourcesAdministration.GetString("label_fieldId")
            dataGrid.Columns("FieldId").ReadOnly = True

            dataGrid.Columns("MaxFieldWidth").Visible = False
            dataGrid.Columns("TruncLeft").Visible = False
            dataGrid.Columns("DefaultValue").Visible = False

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
    End Sub

#End Region

#Region "Property Definitions"
    Property CurrentAction() As FormAction
        Get
            Return _currentAction
        End Get
        Set(ByVal value As FormAction)
            _currentAction = value
        End Set
    End Property

    Property InputWriterConfig() As POSWriterBO
        Get
            Return _inputWriterConfig
        End Get
        Set(ByVal value As POSWriterBO)
            _inputWriterConfig = value
        End Set
    End Property
#End Region

End Class
