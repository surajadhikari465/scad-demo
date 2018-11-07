Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditPOSWriterFileConfig

#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Value of the current column data for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputColumnData As POSWriterFileConfigBO
    ''' <summary>
    ''' Flag to keep track of user changes that have not been saved.
    ''' </summary>
    ''' <remarks></remarks>
    Private hasChanges As Boolean
    ''' <summary>
    ''' Form to manage the field id values.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents manageFieldIdForm As Form_ManagePOSWriterDictionary
    ''' <summary>
    ''' Form to view the POSDataElement descriptions.
    ''' </summary>
    ''' <remarks></remarks>
    Dim viewDataElementDetailsForm As Form_ViewDataElementDetails
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

#Region "Updates made to child form"
    ''' <summary>
    ''' Changes were made to the writer dictionary.  Refresh the combo box.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub manageFieldIdForm_UpdateCallingForm() Handles manageFieldIdForm.UpdateCallingForm
        ' Refresh the combo box
        PopulateDataDictionary()
    End Sub
#End Region

#Region "form load"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for an edit.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditPOSWriterFileConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Logger.LogDebug("Form_EditPOSWriterFileConfig_Load entry", Me.GetType())
        'set up button and title bar labels
        LoadText()

        Try
            Dim dataElement As DataSet
            Dim blankRowDataElement As DataRow
            Dim pirusTaxHostingDataElement As DataRow
            Dim dataTaxFlag As DataSet
            Dim blankRowTaxFlag As DataRow

            ' Populate the selections for the DataElement combo box
            dataElement = POSDataElementDAO.GetPOSDataValues(_inputColumnData.POSDataTypeKey)

            'insert blank row to top of dataset
            blankRowDataElement = dataElement.Tables(0).NewRow()
            blankRowDataElement(0) = DBNull.Value
            blankRowDataElement(1) = ""
            dataElement.Tables(0).Rows.InsertAt(blankRowDataElement, 0)

            ComboBox_DataElementVal.DataSource = dataElement.Tables(0)
            ComboBox_DataElementVal.DisplayMember = "DataElement"
            ComboBox_DataElementVal.ValueMember = "DataElement"

            ' Populate the selections for the TaxFlag combo box
            dataTaxFlag = POSDataElementDAO.GetTaxFlagKeys()

            ' insert blank row to top of dataset
            blankRowTaxFlag = dataTaxFlag.Tables(0).NewRow()
            blankRowTaxFlag(0) = DBNull.Value
            dataTaxFlag.Tables(0).Rows.InsertAt(blankRowTaxFlag, 0)

            'insert PIRUS specific item to tax hosting drop down
            pirusTaxHostingDataElement = dataTaxFlag.Tables(0).NewRow()
            pirusTaxHostingDataElement(0) = ResourcesAdministration.GetString("label_pirusActiveFlag")
            dataTaxFlag.Tables(0).Rows.InsertAt(pirusTaxHostingDataElement, 1)

            ComboBox_TaxFlagVal.DataSource = dataTaxFlag.Tables(0)
            ComboBox_TaxFlagVal.DisplayMember = "TaxFlagKey"
            ComboBox_TaxFlagVal.ValueMember = "TaxFlagKey"

            ' Pre-fill the header info
            Label_POSChangeTypeVal.Text = _inputColumnData.ChangeTypeDesc
            Label_POSWriterVal.Text = _inputColumnData.POSFileWriterCode
            Label_RowNoVal.Text = _inputColumnData.RowOrder.ToString
            Label_ColumnNoVal.Text = _inputColumnData.ColumnOrder.ToString

            ' Show/Hide form controls and pre-fill data based on the form action
            Select Case _currentAction
                Case FormAction.Edit
                    If _inputColumnData.IsLiteral Then
                        ' this is a constant value
                        _inputColumnData.DataElementType = DataElementType.Literal
                        RadioButton_Literal.Checked = True
                        TextBox_LiteralDataElementVal.Text = _inputColumnData.DataElement
                    ElseIf _inputColumnData.IsTaxFlag Then
                        ' this is a tax flag value
                        _inputColumnData.DataElementType = DataElementType.TaxFlag
                        RadioButton_TaxFlag.Checked = True
                        Try
                            ComboBox_TaxFlagVal.SelectedValue = _inputColumnData.DataElement
                        Catch ex As Exception
                            ' it is possible the tax flag that is stored in the database is no longer configured for any of the
                            ' tax classes; in this case the pull down should be left empty.
                        End Try
                    Else
                        ' this is a dynamic value
                        _inputColumnData.DataElementType = DataElementType.Dynamic
                        RadioButton_DataElement.Checked = True
                        ComboBox_DataElementVal.SelectedValue = _inputColumnData.DataElement
                    End If

                    'setup data formatting values
                    If _inputColumnData.PadLeft Then
                        Me.RadioButton_PadLeft.Checked = True
                    Else
                        Me.RadioButton_PadRight.Checked = True
                    End If

                    Me.TextBox_FillChar.Text = _inputColumnData.FillChar

                    If _inputColumnData.IsDecimalValue Then
                        RadioButton_DecimalValue.Checked = True
                        TextBox_DecimalMaxWidth.Text = _inputColumnData.MaxFieldWidth
                        TextBox_DecimalPrecision.Text = _inputColumnData.DecimalPrecision
                        If _inputColumnData.IncludeDecimal Then
                            CheckBox_IncludeDecimalPoint.Checked = True
                        End If
                    ElseIf _inputColumnData.IsPackedDecimal Then
                        ' this is a packed decimal value
                        RadioButton_PackedDecimalValue.Checked = True
                        TextBox_PackedDecimalLength.Text = _inputColumnData.PackLength
                        TextBox_PackedDecimalMaxWidth.Text = _inputColumnData.MaxFieldWidth
                        TextBox_PackedDecimalPrecision.Text = _inputColumnData.DecimalPrecision
                        If _inputColumnData.IncludeDecimal Then
                            CheckBox_IncludePackedDecimalPoint.Checked = True
                        End If
                    ElseIf _inputColumnData.IsBoolean Then
                        RadioButton_BooleanValue.Checked = True
                        TextBox_TrueChar.Text = _inputColumnData.BooleanTrueChar.ToString
                        TextBox_FalseChar.Text = _inputColumnData.BooleanFalseChar.ToString
                    Else
                        RadioButton_OtherValue.Checked = True
                        TextBox_MaxWidthVal.Text = _inputColumnData.MaxFieldWidth
                        CheckBox_TruncateLeft.Checked = _inputColumnData.TruncLeft
                        TextBox_DefaultValue.Text = _inputColumnData.DefaultValue
                        TextBox_LeadingChar.Text = _inputColumnData.LeadingChars
                        TextBox_TrailingChar.Text = _inputColumnData.TrailingChars
                        CheckBox_FixedWidthField.Checked = _inputColumnData.FixedWidthField
                    End If
            End Select

            ' Show/Hide form controls and pre-fill data based on the enforce dictionary flag
            If _inputColumnData.EnforceDictionary Then
                ' Enable the field id drop down box & manage field id buttons
                ComboBox_FieldIdVal.Visible = True
                Button_DataDictionary.Visible = True

                ' Disable the field id text box
                TextBox_FieldIdVal.Visible = False

                ' Populate the data dictionary combo box
                PopulateDataDictionary()
            Else
                ' Disable the field id drop down box & manage field id buttons
                ComboBox_FieldIdVal.Visible = False
                Button_DataDictionary.Visible = False

                ' Enable the field id text box
                TextBox_FieldIdVal.Visible = True

                If _currentAction = FormAction.Edit Then
                    TextBox_FieldIdVal.Text = _inputColumnData.FieldId
                End If
            End If

            ' Show/Hide form controls based on the writer fixed width setting
            If _inputColumnData.FixedWidth Then
                ' Hide the field fixed width settings - they do not apply
                CheckBox_FixedWidthField.Visible = False
            End If

            'The changes flag gets set with some of the pre-filling, so we need to reset it.  
            hasChanges = False

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditPOSWriterFileConfig form: Form_EditPOSWriterFileConfigColumn_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
        Logger.LogDebug("Form_EditPOSWriterFileConfig_Load exit", Me.GetType())
    End Sub

    Private Sub PopulateDataDictionary()
        Dim dataDictionary As DataSet
        Dim blankRowDictionary As DataRow

        ' Populate the selections for the DataDictionary combo box
        dataDictionary = POSWriterDictionaryDAO.GetPOSWriterDictionaryValues(_inputColumnData.POSFileWriterKey)

        'insert blank row to top of dataset
        blankRowDictionary = dataDictionary.Tables(0).NewRow()
        blankRowDictionary(0) = DBNull.Value
        dataDictionary.Tables(0).Rows.InsertAt(blankRowDictionary, 0)

        ComboBox_FieldIdVal.DataSource = dataDictionary.Tables(0)
        ComboBox_FieldIdVal.DisplayMember = "FieldID"
        ComboBox_FieldIdVal.ValueMember = "FieldID"

        If _currentAction = FormAction.Edit Then
            ComboBox_FieldIdVal.SelectedValue = _inputColumnData.FieldId
        End If
    End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriterFileConfig_Add")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriterFileConfig_Edit")
        End Select
    End Sub
#End Region

#Region "cancel button"
    ''' <summary>
    ''' The user selected the Cancel button.
    ''' Do not save the changes.
    ''' Close the form and return focus to the Edit Writer form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        ' Set the flag so they are not prompted to save
        hasChanges = False
        ' Close the child window
        Me.Close()
    End Sub
#End Region

#Region "Close Form"
    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditPOSWriterFileConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If hasChanges Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), ResourcesCommon.GetString("msg_titleConfirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                ' Save the updates to the database
                ProcessSave()
            End If
        End If
    End Sub
#End Region

#Region "save button"
    ''' <summary>
    ''' Save the changes to the database and update the parent form.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ProcessSave() As Boolean
        Logger.LogDebug("ProcessSave entry", Me.GetType())
        Dim success As Boolean = False
        Try
            ' Populate the business object with the form data and save the change to the database
            If _inputColumnData.EnforceDictionary Then
                _inputColumnData.FieldId = ComboBox_FieldIdVal.SelectedValue.ToString
            Else
                _inputColumnData.FieldId = TextBox_FieldIdVal.Text
            End If

            _inputColumnData.IsLiteral = RadioButton_Literal.Checked
            _inputColumnData.IsTaxFlag = RadioButton_TaxFlag.Checked
            _inputColumnData.IsDecimalValue = RadioButton_DecimalValue.Checked
            _inputColumnData.IsPackedDecimal = RadioButton_PackedDecimalValue.Checked
            _inputColumnData.IsBoolean = RadioButton_BooleanValue.Checked

            'determine data element type and get related value
            If RadioButton_Literal.Checked Then
                ' this is a constant value
                _inputColumnData.DataElementType = DataElementType.Literal
                _inputColumnData.DataElement = TextBox_LiteralDataElementVal.Text
            ElseIf RadioButton_TaxFlag.Checked Then
                ' this is a tax flag value
                _inputColumnData.DataElementType = DataElementType.TaxFlag
                Try
                    _inputColumnData.DataElement = ComboBox_TaxFlagVal.SelectedValue.ToString
                Catch ex As NullReferenceException
                    ' if the tax values have changed since the writer was configured, a null reference 
                    ' exception will be thrown on save
                    _inputColumnData.DataElement = ""
                End Try
            ElseIf Me.RadioButton_DataElement.Checked Then
                ' this is a dynamic value
                _inputColumnData.DataElementType = DataElementType.Dynamic
                _inputColumnData.DataElement = ComboBox_DataElementVal.SelectedValue.ToString
            End If

            'get data formatting values
            If _inputColumnData.PerformDataFormattingValidation Then
                If Me.RadioButton_PadLeft.Checked Then
                    _inputColumnData.PadLeft = True
                Else
                    _inputColumnData.PadLeft = False
                End If

                _inputColumnData.FillChar = Me.TextBox_FillChar.Text

                Select Case True
                    Case RadioButton_DecimalValue.Checked
                        'this is a decimal value
                        _inputColumnData.MaxFieldWidth = TextBox_DecimalMaxWidth.Text
                        _inputColumnData.DecimalPrecision = TextBox_DecimalPrecision.Text
                        If CheckBox_IncludeDecimalPoint.Checked Then
                            _inputColumnData.IncludeDecimal = True
                        End If
                    Case RadioButton_PackedDecimalValue.Checked
                        ' this is a packed decimal value
                        _inputColumnData.PackLength = TextBox_PackedDecimalLength.Text
                        _inputColumnData.MaxFieldWidth = TextBox_PackedDecimalMaxWidth.Text
                        _inputColumnData.DecimalPrecision = TextBox_PackedDecimalPrecision.Text
                        If CheckBox_IncludePackedDecimalPoint.Checked Then
                            _inputColumnData.IncludeDecimal = True
                        End If
                    Case RadioButton_BooleanValue.Checked
                        ' this is a boolean value
                        _inputColumnData.BooleanTrueChar = TextBox_TrueChar.Text
                        _inputColumnData.BooleanFalseChar = TextBox_FalseChar.Text
                    Case RadioButton_OtherValue.Checked
                        'this is an 'other' value (non-decimal)
                        _inputColumnData.MaxFieldWidth = TextBox_MaxWidthVal.Text
                        _inputColumnData.TruncLeft = CheckBox_TruncateLeft.Checked
                        _inputColumnData.DefaultValue = TextBox_DefaultValue.Text
                        _inputColumnData.LeadingChars = TextBox_LeadingChar.Text
                        _inputColumnData.TrailingChars = TextBox_TrailingChar.Text
                        _inputColumnData.FixedWidthField = CheckBox_FixedWidthField.Checked
                End Select
            End If

            'validate data
            Dim statusList As ArrayList = _inputColumnData.ValidatePOSWriterData()
            Dim statusEnum As IEnumerator = statusList.GetEnumerator
            Dim message As New StringBuilder
            Dim currentStatus As POSWriterFileConfigStatus

            'loop through possible validation erorrs and build message string containing all errors
            While statusEnum.MoveNext
                currentStatus = CType(statusEnum.Current, POSWriterFileConfigStatus)

                Select Case currentStatus
                    Case POSWriterFileConfigStatus.Error_Required_DataElementType
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.GroupBox_DataElementType.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_FieldId
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FieldId.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_LiteralDataElement
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.RadioButton_Literal.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_DynamicDataElement
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.RadioButton_DataElement.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_MaxWidth
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_MaxWidth_Decimal
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_MaxWidth_DecimalPrecision
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), ResourcesAdministration.GetString("label_DecimalPrecision")))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_PackLength
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_PackLength.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_PackLength
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_PackLength.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_MaxWidth_PackedDecimal
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_PackedDecimal_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_MaxWidth_PackedDecimalPrecision
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), ResourcesAdministration.GetString("label_DecimalPrecision")))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_MaxWidth
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_MaxWidth_Decimal
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_MaxWidth_DecimalPrecision
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), ResourcesAdministration.GetString("label_DecimalPrecision")))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_MaxWidth_PackedDecimal
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_PackedDecimal_MaxWidth.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Integer_MaxWidth_PackedDecimalPrecision
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), ResourcesAdministration.GetString("label_DecimalPrecision")))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_TaxFlagDataElement
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.RadioButton_TaxFlag.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_FillChar
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FillChar.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_BooleanTrueChar
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_TrueChar.Text))
                        message.Append(Environment.NewLine)
                    Case POSWriterFileConfigStatus.Error_Required_BooleanFalseChar
                        message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FalseChar.Text))
                        message.Append(Environment.NewLine)
                End Select
            End While

            If message.Length <= 0 Then
                success = True

                'data is valid - perform insert/update
                Select Case _currentAction
                    Case FormAction.Create
                        POSWriterDAO.AddPOSWriterFileConfigRecord(_inputColumnData)
                    Case FormAction.Edit
                        POSWriterDAO.UpdatePOSWriterFileConfigRecord(_inputColumnData)
                End Select

                ' set the changes flag to false because they've been saved
                hasChanges = False

                ' Raise the save event - allows the data on the parent form to be refreshed
                RaiseEvent UpdateCallingForm()
            Else
                success = False
                'display error msg
                MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditPOSWriterFileConfig form: ProcessSave sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try

        Return success
        Logger.LogDebug("ProcessSave exit", Me.GetType())
    End Function

    ''' <summary>
    ''' The user selected the Save button.
    ''' Save the changes.
    ''' Close the form and return focus to the Edit Writer form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        ' Save the updates to the database
        If ProcessSave() Then
            ' Close the child window
            Me.Close()
        End If
    End Sub
#End Region

#Region "edit data"
    ''' <summary>
    ''' Disable the Constant radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearConstantValues()
        RadioButton_Literal.Checked = False
        TextBox_LiteralDataElementVal.Text = ""
    End Sub

    ''' <summary>
    ''' Disable the data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearDataElementValues()
        RadioButton_DataElement.Checked = False
        ComboBox_DataElementVal.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Disable the tax flag radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearTaxFlagValues()
        RadioButton_TaxFlag.Checked = False
        ComboBox_TaxFlagVal.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Clear all data formatting section data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearDataFormattingValues()
        ClearDecimalDataElementValues()
        ClearPackedDecimalDataElementValues()
        ClearBooleanDataElementValues()
        ClearOtherDataElementValues()
        GroupBox_DataFormatting.Enabled = False
    End Sub

    ''' <summary>
    ''' Disable the decimal data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearDecimalDataElementValues()
        Me.RadioButton_DecimalValue.Checked = False
        Me.TextBox_DecimalMaxWidth.Text = ""
        Me.TextBox_DecimalPrecision.Text = ""
        Me.CheckBox_IncludeDecimalPoint.Checked = False
    End Sub

    ''' <summary>
    ''' Disable the packed decimal data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearPackedDecimalDataElementValues()
        Me.RadioButton_PackedDecimalValue.Checked = False
        Me.TextBox_PackedDecimalLength.Text = ""
        Me.TextBox_PackedDecimalMaxWidth.Text = ""
        Me.TextBox_PackedDecimalPrecision.Text = ""
        Me.CheckBox_IncludePackedDecimalPoint.Checked = False
    End Sub

    ''' <summary>
    ''' Disable the boolean data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearBooleanDataElementValues()
        Me.RadioButton_BooleanValue.Checked = False
        Me.TextBox_TrueChar.Text = ""
        Me.TextBox_FalseChar.Text = ""
    End Sub

    ''' <summary>
    ''' Disable the "other" (non-decimal) data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearOtherDataElementValues()
        Me.RadioButton_OtherValue.Checked = False
        CheckBox_TruncateLeft.Checked = False
        CheckBox_FixedWidthField.Checked = False
        TextBox_MaxWidthVal.Text = ""
        TextBox_DefaultValue.Text = ""
        TextBox_LeadingChar.Text = ""
        TextBox_TrailingChar.Text = ""
    End Sub

    ''' <summary>
    ''' The user changed the value for the constant radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RadioButton_Literal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton_Literal.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_Literal.Checked Then
            ' Clear the other data element values
            ClearDataElementValues()
            ClearTaxFlagValues()
            ClearDataFormattingValues()
        Else
            ' Clear this value
            ClearConstantValues()
        End If
    End Sub

    ''' <summary>
    ''' The user changed the value for the data element radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RadioButton_DataElement_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton_DataElement.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_DataElement.Checked Then
            ' Clear the other data element values
            ClearConstantValues()
            ClearTaxFlagValues()
            Me.GroupBox_DataFormatting.Enabled = True
        Else
            ' Clear this data element value
            ClearDataElementValues()
        End If
    End Sub

    ''' <summary>
    ''' The user changed the value for the tax flag radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RadioButton_TaxFlag_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_TaxFlag.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_TaxFlag.Checked Then
            ' Clear the other data element values
            ClearConstantValues()
            ClearDataElementValues()
            Me.GroupBox_DataFormatting.Enabled = True
        Else
            ' Clear this data element value
            ClearTaxFlagValues()
        End If
    End Sub

    Private Sub RadioButton_DecimalValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_DecimalValue.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_DecimalValue.Checked Then
            ' Clear the other data element values
            ClearPackedDecimalDataElementValues()
            ClearBooleanDataElementValues()
            ClearOtherDataElementValues()
        Else
            ' Clear this data element value
            ClearDecimalDataElementValues()
        End If
    End Sub

    Private Sub RadioButton_PackedDecimalValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_PackedDecimalValue.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_PackedDecimalValue.Checked Then
            ' Clear the other data element values
            ClearDecimalDataElementValues()
            ClearBooleanDataElementValues()
            ClearOtherDataElementValues()
        Else
            ' Clear this data element value
            ClearPackedDecimalDataElementValues()
        End If
    End Sub

    Private Sub RadioButton_BooleanValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_BooleanValue.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_BooleanValue.Checked Then
            ' Clear the other data element values
            ClearDecimalDataElementValues()
            ClearPackedDecimalDataElementValues()
            ClearOtherDataElementValues()
        Else
            ' Clear this data element value
            ClearBooleanDataElementValues()
        End If
    End Sub

    Private Sub RadioButton_OtherValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_OtherValue.CheckedChanged
        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If RadioButton_OtherValue.Checked Then
            ' Clear the other data element values
            ClearDecimalDataElementValues()
            ClearPackedDecimalDataElementValues()
            ClearBooleanDataElementValues()
        Else
            ' Clear this data element value
            ClearOtherDataElementValues()
        End If
    End Sub

    Private Sub TextBox_LiteralDataElementVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_LiteralDataElementVal.TextChanged
        If Not TextBox_LiteralDataElementVal.Text.Equals("") Then
            'auto check literal radio button if literal text is entered
            RadioButton_Literal.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub ComboBox_DataElementVal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_DataElementVal.SelectedIndexChanged
        If ComboBox_DataElementVal.SelectedIndex <> 0 Then
            'auto check data element radio button
            RadioButton_DataElement.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub AdjustWidthComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_DataElementVal.DropDown
        'Dim senderComboBox As ComboBox = CType(sender, ComboBox)
        'Dim width As Integer = senderComboBox.DropDownWidth
        'Dim g As Graphics = senderComboBox.CreateGraphics()
        'Dim font As Font = senderComboBox.Font
        'Dim vertScrollBarWidth As Integer = CType(IIf((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0), Integer)

        'Dim newWidth As Integer
        'Dim itemEnum As IEnumerator = CType(sender, ComboBox).Items.GetEnumerator
        'Dim currentItem As String

        'While itemEnum.MoveNext
        '    currentItem = CType(itemEnum.Current, DataRowView).Row.Item("DataElement").ToString
        '    newWidth = CType(g.MeasureString(currentItem, font).Width, Integer) + vertScrollBarWidth

        '    If width < newWidth Then
        '        width = newWidth
        '    End If
        'End While

        'senderComboBox.DropDownWidth = width
    End Sub

    Private Sub ComboBox_TaxFlagVal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_TaxFlagVal.SelectedIndexChanged
        If ComboBox_TaxFlagVal.SelectedIndex <> 0 Then
            'auto check tax flag radio button
            RadioButton_TaxFlag.Checked = True

            'if item selected is PIRUS item then enable the data formatting section
            If ComboBox_TaxFlagVal.SelectedValue.Equals(ResourcesAdministration.GetString("label_pirusActiveFlag")) Then
                Me.GroupBox_DataFormatting.Enabled = True
            Else
                ClearDataFormattingValues()
            End If
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_DecimalMaxWidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_DecimalMaxWidth.TextChanged
        If Not TextBox_DecimalMaxWidth.Text.Equals("") Then
            'auto check dynamic-decimal radio button if value is entered
            Me.RadioButton_DecimalValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_DecimalPrecision_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_DecimalPrecision.TextChanged
        If Not TextBox_DecimalPrecision.Text.Equals("") Then
            'auto check dynamic-decimal radio button if value is entered
            Me.RadioButton_DecimalValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub CheckBox_IncludeDecimalPoint_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_IncludeDecimalPoint.CheckedChanged
        'auto check dynamic-decimal radio button if value is entered
        Me.RadioButton_DecimalValue.Checked = True
        hasChanges = True
    End Sub

    Private Sub TextBox_PackedDecimalMaxWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PackedDecimalMaxWidth.TextChanged
        If Not TextBox_PackedDecimalMaxWidth.Text.Equals("") Then
            ' auto check packed decimal radio button if value is entered
            Me.RadioButton_PackedDecimalValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_TrueChar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_TrueChar.TextChanged
        If Not TextBox_TrueChar.Text.Equals("") Then
            'auto check dynamic-boolean radio button if value is entered
            Me.RadioButton_BooleanValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_FalseChar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_FalseChar.TextChanged
        If Not TextBox_FalseChar.Text.Equals("") Then
            'auto check dynamic-boolean radio button if value is entered
            Me.RadioButton_BooleanValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_DefaultValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_DefaultValue.TextChanged
        If Not TextBox_DefaultValue.Text.Equals("") Then
            'auto check dynamic-other radio button if value is entered
            Me.RadioButton_OtherValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub TextBox_MaxWidthVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_MaxWidthVal.TextChanged
        If Not TextBox_MaxWidthVal.Text.Equals("") Then
            'auto check dynamic-other radio button if value is entered
            Me.RadioButton_OtherValue.Checked = True
        End If
        hasChanges = True
    End Sub

    Private Sub CheckBox_TruncateLeft_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_TruncateLeft.CheckedChanged
        'auto check dynamic-other radio button if value is entered
        Me.RadioButton_OtherValue.Checked = True
        hasChanges = True
    End Sub
    Private Sub ComboBox_FieldIdVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_FieldIdVal.TextChanged
        hasChanges = True
    End Sub
    Private Sub TextBox_FieldIdVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_FieldIdVal.TextChanged
        hasChanges = True
    End Sub


#End Region

#Region "button to view data element details"
    Private Sub Button_ViewDataElementDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ViewDataElementDetails.Click
        Logger.LogDebug("Button_ViewDataElementDetails_Click entry", Me.GetType())
        ' Show the form
        viewDataElementDetailsForm = New Form_ViewDataElementDetails()
        viewDataElementDetailsForm.POSDataTypeKey = _inputColumnData.POSDataTypeKey
        viewDataElementDetailsForm.BooleanElementsOnly = False
        viewDataElementDetailsForm.ShowDialog(Me)
        viewDataElementDetailsForm.Dispose()
        Logger.LogDebug("Button_ViewDataElementDetails_Click exit", Me.GetType())
    End Sub
#End Region

#Region "button to manage field ids"
    Private Sub Button_DataDictionary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DataDictionary.Click
        Logger.LogDebug("Button_DataDictionary_Click entry", Me.GetType())
        ' Show the form
        manageFieldIdForm = New Form_ManagePOSWriterDictionary()
        manageFieldIdForm.POSFileWriterKey = _inputColumnData.POSFileWriterKey
        manageFieldIdForm.POSDataTypeKey = _inputColumnData.POSDataTypeKey
        manageFieldIdForm.POSFileWriterCode = _inputColumnData.POSFileWriterCode
        manageFieldIdForm.ShowDialog(Me)
        manageFieldIdForm.Dispose()
        Logger.LogDebug("Button_DataDictionary_Click exit", Me.GetType())
    End Sub

#End Region

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

    Property InputColumnData() As POSWriterFileConfigBO
        Get
            Return _inputColumnData
        End Get
        Set(ByVal value As POSWriterFileConfigBO)
            _inputColumnData = value
        End Set
    End Property
#End Region

End Class
