Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_EditPOSWriterFileConfigBinaryInt

    Private isLoading As Boolean


#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' The current action that is being performed: New or Edit
    ''' </summary>
    ''' <remarks></remarks>
    Private _currentAction As FormAction
    ''' <summary>
    ''' Value of the current column data for edits
    ''' This value is set on input to identify the row selected by the user on the grid.
    ''' It could represent any of the bit positions for the binary int.
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputColumnData As POSWriterFileConfigBO
    ''' <summary>
    ''' Value of the current column data for edits.
    ''' This is an array with an entry for each bit position.
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputColumnBitData() As POSWriterFileConfigBO
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
        ' PopulateDataDictionary()
    End Sub
#End Region

#Region "form load"
    ''' <summary>
    ''' Load the form, pre-filling with the existing data for an edit.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_EditPOSWriterFileConfigBinaryInt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Logger.LogDebug("Form_EditPOSWriterFileConfigBinaryInt_Load entry", Me.GetType())
        'set up button and title bar labels
        LoadText()
        'sets flag for loading
        isLoading = True

        'Set all the combo boxes so that you are only able to choose from the list, not type in the control.
        _cbo_Bit_DataElementVal_0.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_1.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_2.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_3.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_4.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_5.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_6.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_DataElementVal_7.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_0.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_1.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_2.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_3.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_4.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_5.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_6.DropDownStyle = ComboBoxStyle.DropDownList
        _cbo_Bit_TaxFlagVal_7.DropDownStyle = ComboBoxStyle.DropDownList

        Try
            Dim dataElement As DataSet
            Dim blankRowDataElement As DataRow
            Dim dataTaxFlag As DataSet
            Dim blankRowTaxFlag As DataRow
            Dim index As Byte

            ' Pre-fill the header info
            Label_POSChangeTypeVal.Text = _inputColumnData.ChangeTypeDesc
            Label_POSWriterVal.Text = _inputColumnData.POSFileWriterCode
            Label_RowNoVal.Text = _inputColumnData.RowOrder.ToString
            Label_ColumnNoVal.Text = _inputColumnData.ColumnOrder.ToString

            ' Read the selections for the DataElement combo boxes
            dataElement = POSDataElementDAO.GetPOSDataValues(_inputColumnData.POSDataTypeKey, True)

            'insert blank row to top of dataset
            blankRowDataElement = dataElement.Tables(0).NewRow()
            blankRowDataElement(0) = DBNull.Value
            blankRowDataElement(1) = ""
            dataElement.Tables(0).Rows.InsertAt(blankRowDataElement, 0)

            ' Read the selections for the TaxFlag combo boxes
            dataTaxFlag = POSDataElementDAO.GetTaxFlagKeys()

            ' insert blank row to top of dataset
            blankRowTaxFlag = dataTaxFlag.Tables(0).NewRow()
            blankRowTaxFlag(0) = DBNull.Value
            dataTaxFlag.Tables(0).Rows.InsertAt(blankRowTaxFlag, 0)

            ''insert PIRUS specific item to tax hosting drop down
            'Dim pirusTaxHostingDataElement As DataRow
            'pirusTaxHostingDataElement = dataTaxFlag.Tables(0).NewRow()
            'pirusTaxHostingDataElement(0) = ResourcesAdministration.GetString("label_pirusActiveFlag")
            'dataTaxFlag.Tables(0).Rows.InsertAt(pirusTaxHostingDataElement, 1)

            ' Pre-fill the data for each bit position
            ReDim _inputColumnBitData(0 To 7)   'dimension column array for bits 0 to 7
            For index = 0 To 7
                ' Populate the combo boxes for this bit position
                cbo_Bit_DataElementVal(index).DataSource = dataElement.Copy.Tables(0)
                cbo_Bit_DataElementVal(index).DisplayMember = "DataElement"
                cbo_Bit_DataElementVal(index).ValueMember = "DataElement"
                'cbo_Bit_DataElementVal(index).Visible = False
                AddHandler cbo_Bit_DataElementVal(index).SelectedIndexChanged, AddressOf cbo_Bit_DataElementVal_SelectedIndexChanged

                cbo_Bit_TaxFlagVal(index).DataSource = dataTaxFlag.Copy.Tables(0)
                cbo_Bit_TaxFlagVal(index).DisplayMember = "TaxFlagKey"
                cbo_Bit_TaxFlagVal(index).ValueMember = "TaxFlagKey"
                'cbo_Bit_TaxFlagVal(index).Visible = False
                AddHandler cbo_Bit_TaxFlagVal(index).SelectedIndexChanged, AddressOf cbo_Bit_TaxFlagVal_SelectedIndexChanged

                'grp_Bit_Literal(index).Visible = False
                AddHandler rad_Bit_Literal(index).CheckedChanged, AddressOf rad_Bit_Literal_CheckedChanged
                AddHandler rad_Bit_True(index).CheckedChanged, AddressOf rad_Bit_True_CheckedChanged
                AddHandler rad_Bit_False(index).CheckedChanged, AddressOf rad_Bit_False_CheckedChanged
                AddHandler rad_Bit_DataElement(index).CheckedChanged, AddressOf rad_Bit_DataElement_CheckedChanged
                AddHandler rad_Bit_TaxFlag(index).CheckedChanged, AddressOf rad_Bit_TaxFlag_CheckedChanged

                ' Show/Hide form controls and pre-fill data based on the form action
                _inputColumnBitData(index) = New POSWriterFileConfigBO
            Next

            If _currentAction = FormAction.Edit Then
                ' Read all bits for the specified column from the POSWriterFileConfig data 
                Dim _dataSet As DataSet = POSWriterDAO.GetWriterFileConfigurationsByRowAndCol(_inputColumnData.POSFileWriterKey, _inputColumnData.POSChangeTypeKey, _inputColumnData.RowOrder, _inputColumnData.ColumnOrder)
                With _dataSet.CreateDataReader
                    While .Read
                        index = .GetByte(.GetOrdinal("BitOrder"))

                        _inputColumnBitData(index).RowOrder = .GetInt32(.GetOrdinal("RowOrder"))
                        _inputColumnBitData(index).ColumnOrder = .GetInt32(.GetOrdinal("ColumnOrder"))
                        _inputColumnBitData(index).BitOrder = index

                        _inputColumnBitData(index).POSFileWriterCode = .GetString(.GetOrdinal("POSFileWriterCode"))
                        _inputColumnBitData(index).ChangeTypeDesc = .GetString(.GetOrdinal("ChangeTypeDesc"))

                        _inputColumnBitData(index).IsLiteral = .GetBoolean(.GetOrdinal("IsLiteral"))
                        _inputColumnBitData(index).IsTaxFlag = .GetBoolean(.GetOrdinal("IsTaxFlag"))
                        '_inputColumnBitData(index).IsPackedDecimal = .GetBoolean(.GetOrdinal("IsPackedDecimal"))
                        _inputColumnBitData(index).DataElement = .GetString(.GetOrdinal("DataElement"))
                        _inputColumnBitData(index).FieldId = .GetString(.GetOrdinal("FieldId"))
                    End While

                    .Close()
                End With

                ' Show/Hide form controls and pre-fill data based on the form action
                For index = 0 To 7
                    If _inputColumnBitData(index).IsLiteral Then
                        ' this is a constant value
                        _inputColumnBitData(index).DataElementType = DataElementType.Literal
                        rad_Bit_Literal(index).Checked = True
                        If CBool(_inputColumnBitData(index).DataElement) = True Then
                            rad_Bit_True(index).Checked = True
                        Else
                            rad_Bit_False(index).Checked = True
                        End If
                    ElseIf _inputColumnBitData(index).IsTaxFlag Then
                        ' this is a tax flag value
                        _inputColumnBitData(index).DataElementType = DataElementType.TaxFlag
                        rad_Bit_TaxFlag(index).Checked = True

                        cbo_Bit_TaxFlagVal(index).SelectedValue = _inputColumnBitData(index).DataElement

                        ' it is possible the tax flag that is stored in the database is no longer configured for any of the
                        ' tax classes
                        If cbo_Bit_TaxFlagVal(index).SelectedValue Is Nothing Then
                            cbo_Bit_TaxFlagVal(index).SelectedValue = 0 ' set to the default DBNull selection
                        End If
                    Else
                        ' this is a dynamic value
                        _inputColumnBitData(index).DataElementType = DataElementType.Dynamic
                        rad_Bit_DataElement(index).Checked = True
                        cbo_Bit_DataElementVal(index).SelectedValue = _inputColumnBitData(index).DataElement
                    End If
                Next
            End If

            ' TODO: Need to add the enforce dictionary logic to the 
            ' pull downs available for binary values
            ' Show/Hide form controls and pre-fill data based on the enforce dictionary flag
            'If _inputColumnData.EnforceDictionary Then
            '    ' Enable the field id drop down box & manage field id buttons
            '    ComboBox_FieldIdVal.Visible = True
            '    Button_DataDictionary.Visible = True

            '    ' Disable the field id text box
            '    TextBox_FieldIdVal.Visible = False

            '    ' Populate the data dictionary combo box
            '    PopulateDataDictionary()
            'Else
            '    ' Disable the field id drop down box & manage field id buttons
            '    ComboBox_FieldIdVal.Visible = False
            '    Button_DataDictionary.Visible = False

            '    ' Enable the field id text box
            '    TextBox_FieldIdVal.Visible = True

            '    If _currentAction = FormAction.Edit Then
            '        TextBox_FieldIdVal.Text = _inputColumnData.FieldId
            '    End If
            'End If

            '' Show/Hide form controls based on the writer fixed width setting
            'If _inputColumnData.FixedWidth Then
            '    ' Hide the field fixed width settings - they do not apply
            '    Label_FixedWidthField.Visible = False
            '    CheckBox_FixedWidthField.Visible = False
            'End If

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            DisplayErrorMessage(ERROR_DB)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_EditPOSWriterFileConfigBinaryInt form: Form_EditPOSWriterFileConfigBinaryIntColumn_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
        isLoading = False
        Logger.LogDebug("Form_EditPOSWriterFileConfigBinaryInt_Load exit", Me.GetType())
    End Sub

    'Private Sub PopulateDataDictionary()
    '    Dim dataDictionary As DataSet
    '    Dim blankRowDictionary As DataRow

    '    ' Populate the selections for the DataDictionary combo box
    '    dataDictionary = POSWriterDictionaryDAO.GetPOSWriterDictionaryValues(_inputColumnData.POSFileWriterKey)

    '    'insert blank row to top of dataset
    '    blankRowDictionary = dataDictionary.Tables(0).NewRow()
    '    blankRowDictionary(0) = DBNull.Value
    '    dataDictionary.Tables(0).Rows.InsertAt(blankRowDictionary, 0)

    '    ComboBox_FieldIdVal.DataSource = dataDictionary.Tables(0)
    '    ComboBox_FieldIdVal.DisplayMember = "FieldID"
    '    ComboBox_FieldIdVal.ValueMember = "FieldID"

    '    If _currentAction = FormAction.Edit Then
    '        ComboBox_FieldIdVal.SelectedValue = _inputColumnData.FieldId
    '    End If
    'End Sub

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()


        Select Case _currentAction
            Case FormAction.Create
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriterFileConfigBinaryInt_Add")
            Case FormAction.Edit
                Me.Text = ResourcesAdministration.GetString("label_titleBar_EditPOSWriterFileConfigBinaryInt_Edit")

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
        Dim index As Short
        Dim sBitLabel As String
        Try
            ' Populate the business object with the form data and save the change to the database
            Dim message As New StringBuilder
            Dim currentStatus As POSWriterFileConfigStatus

            ' Populate the business object with the form data and save the change to the database

            _inputColumnData.FieldId = TextBox_FieldIdVal.Text

            'map each of the bits
            For index = 0 To 7
                _inputColumnBitData(index).RowOrder = _inputColumnData.RowOrder
                _inputColumnBitData(index).ColumnOrder = _inputColumnData.ColumnOrder
                _inputColumnBitData(index).BitOrder = index

                _inputColumnBitData(index).POSFileWriterKey = _inputColumnData.POSFileWriterKey
                _inputColumnBitData(index).POSChangeTypeKey = _inputColumnData.POSChangeTypeKey
                _inputColumnBitData(index).FieldId = _inputColumnData.FieldId

                _inputColumnBitData(index).IsBinaryInt = True

                'determine data element type and get related value
                If rad_Bit_Literal(index).Checked Then
                    ' this is a constant value
                    _inputColumnBitData(index).IsLiteral = True
                    _inputColumnBitData(index).IsTaxFlag = False
                    _inputColumnBitData(index).DataElementType = DataElementType.Literal
                    If rad_Bit_True(index).Checked Then
                        _inputColumnBitData(index).DataElement = "1"
                    ElseIf rad_Bit_False(index).Checked Then
                        _inputColumnBitData(index).DataElement = "0"
                    Else
                        _inputColumnBitData(index).DataElement = String.Empty
                    End If
                ElseIf rad_Bit_TaxFlag(index).Checked Then
                    ' this is a tax flag value
                    _inputColumnBitData(index).IsLiteral = False
                    _inputColumnBitData(index).IsTaxFlag = True
                    _inputColumnBitData(index).DataElementType = DataElementType.TaxFlag
                    Try
                        _inputColumnBitData(index).DataElement = cbo_Bit_TaxFlagVal(index).SelectedValue.ToString
                    Catch ex As NullReferenceException
                        ' if the tax values have changed since the writer was configured, a null reference 
                        ' exception will be thrown on save
                        _inputColumnBitData(index).DataElement = ""
                    End Try
                ElseIf rad_Bit_DataElement(index).Checked Then
                    ' this is a dynamic value
                    _inputColumnBitData(index).IsLiteral = False
                    _inputColumnBitData(index).IsTaxFlag = False
                    _inputColumnBitData(index).DataElementType = DataElementType.Dynamic
                    _inputColumnBitData(index).DataElement = cbo_Bit_DataElementVal(index).SelectedValue.ToString
                End If

                sBitLabel = "Bit " & index.ToString & ": "

                'validate data
                Dim statusList As ArrayList = _inputColumnBitData(index).ValidatePOSWriterData()
                Dim statusEnum As IEnumerator = statusList.GetEnumerator

                'loop through possible validation erorrs and build message string containing all errors
                While statusEnum.MoveNext
                    currentStatus = CType(statusEnum.Current, POSWriterFileConfigStatus)

                    Select Case currentStatus
                        Case POSWriterFileConfigStatus.Error_Required_LiteralDataElement
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), sBitLabel & Me.rad_Bit_Literal(index).Text))
                            message.Append(Environment.NewLine)
                        Case POSWriterFileConfigStatus.Error_Required_DynamicDataElement
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), sBitLabel & Me.rad_Bit_DataElement(index).Text))
                            message.Append(Environment.NewLine)
                        Case POSWriterFileConfigStatus.Error_Required_TaxFlagDataElement
                            message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), sBitLabel & Me.rad_Bit_TaxFlag(index).Text))
                            message.Append(Environment.NewLine)
                    End Select
                End While
            Next

            If message.Length <= 0 Then
                success = True

                'data is valid - perform insert/update
                Select Case _currentAction
                    Case FormAction.Create
                        POSWriterDAO.AddPOSWriterFileConfigRecord(_inputColumnBitData)
                    Case FormAction.Edit
                        POSWriterDAO.UpdatePOSWriterFileConfigRecord(_inputColumnBitData)
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
            args(0) = "Form_EditPOSWriterFileConfig form: ProcessSaveBinary()"
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

#Region "edit binary data"

    ''' <summary>
    ''' Disable the data element radio button and clear all associated data entry values.
    ''' </summary>
    ''' <param name="BitNumber"></param>
    ''' <remarks></remarks>
    Private Sub ClearDataElementValues(ByVal BitNumber As Byte)

        rad_Bit_DataElement(BitNumber).Checked = False
        cbo_Bit_DataElementVal(BitNumber).SelectedIndex = 0

    End Sub

    ''' <summary>
    ''' Disable the tax flag radio button and clear all associated data entry values.
    ''' </summary>
    ''' <param name="BitNumber"></param>
    ''' <remarks></remarks>
    Private Sub ClearTaxFlagValues(ByVal BitNumber As Byte)

        rad_Bit_TaxFlag(BitNumber).Checked = False
        cbo_Bit_TaxFlagVal(BitNumber).SelectedIndex = 0

    End Sub

    'Private Sub cbo_Bit_DataElementVal_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bit_DataElementVal.DropDown

    '    Call AdjustWidthComboBox_DropDown(sender, e)
    'End Sub

    Private Sub cbo_Bit_DataElementVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = cbo_Bit_DataElementVal.GetIndex(CType(sender, ComboBox))

        If cbo_Bit_DataElementVal(Index).SelectedIndex <> 0 Then
            'auto check data element radio button
            rad_Bit_DataElement(Index).Checked = True
        End If
    End Sub

    ''' <summary>
    ''' The user changed the value for the constant radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rad_Bit_Literal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = rad_Bit_Literal.GetIndex(CType(sender, RadioButton))

        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If rad_Bit_Literal(Index).Checked Then
            ' Clear the other data element values
            ClearDataElementValues(CByte(Index))
            ClearTaxFlagValues(CByte(Index))
            'grp_Bit_Literal(Index).Visible = True
        End If
    End Sub

    ''' <summary>
    ''' The user changed the value for the constant true value radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rad_Bit_True_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = rad_Bit_True.GetIndex(CType(sender, RadioButton))

        ' Set the change data flag
        hasChanges = True

        rad_Bit_Literal(Index).Checked = True
        ClearDataElementValues(CByte(Index))
        ClearTaxFlagValues(CByte(Index))

    End Sub

    ''' <summary>
    ''' The user changed the value for the constant false value radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rad_Bit_False_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = rad_Bit_False.GetIndex(CType(sender, RadioButton))

        ' Set the change data flag
        hasChanges = True

        rad_Bit_Literal(Index).Checked = True
        ClearDataElementValues(CByte(Index))
        ClearTaxFlagValues(CByte(Index))

    End Sub

    ''' <summary>
    ''' The user changed the value for the data element radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rad_Bit_DataElement_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Index As Short = rad_Bit_DataElement.GetIndex(CType(sender, RadioButton))

        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If rad_Bit_DataElement(Index).Checked Then
            ' Clear the other data element values
            ClearTaxFlagValues(CByte(Index))
            'cbo_Bit_DataElementVal(Index).Visible = True
        Else
            ' Clear this data element value
            ClearDataElementValues(CByte(Index))
            'cbo_Bit_DataElementVal(Index).Visible = False
        End If
    End Sub

    Private Sub cbo_Bit_TaxFlagVal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = cbo_Bit_TaxFlagVal.GetIndex(CType(sender, ComboBox))

        If cbo_Bit_TaxFlagVal(Index).SelectedIndex <> 0 Then
            'auto check tax flag radio button
            rad_Bit_TaxFlag(Index).Checked = True
        End If
    End Sub

    ''' <summary>
    ''' The user changed the value for the tax flag radio button.
    ''' Update all the input fields accordingly.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rad_Bit_TaxFlag_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim Index As Short = rad_Bit_TaxFlag.GetIndex(CType(sender, RadioButton))

        ' Set the change data flag
        hasChanges = True

        ' Did they select the radio button?
        If rad_Bit_TaxFlag(Index).Checked Then
            ' Clear the other data element values
            ClearDataElementValues(CByte(Index))
            'cbo_Bit_TaxFlagVal(Index).Visible = True
        Else
            ' Clear this data element value
            ClearTaxFlagValues(CByte(Index))
            'cbo_Bit_TaxFlagVal(Index).Visible = False
        End If
    End Sub

#End Region

#Region "button to view data element details"
    Private Sub Button_ViewDataElementDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Logger.LogDebug("Button_ViewDataElementDetails_Click entry", Me.GetType())
        ' Show the form
        viewDataElementDetailsForm = New Form_ViewDataElementDetails()
        viewDataElementDetailsForm.POSDataTypeKey = _inputColumnData.POSDataTypeKey
        viewDataElementDetailsForm.ShowDialog(Me)
        viewDataElementDetailsForm.Dispose()
        Logger.LogDebug("Button_ViewDataElementDetails_Click exit", Me.GetType())
    End Sub
#End Region

#Region "button to manage field ids"
    Private Sub Button_DataDictionary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    'Private Sub checkRadioTaxFlag(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _cbo_Bit_TaxFlagVal_0.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_1.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_2.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_3.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_4.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_5.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_6.SelectedIndexChanged, _cbo_Bit_TaxFlagVal_7.SelectedIndexChanged
    '    If Not isLoading Then

    '        '    Dim strComboName As String
    '        '    Dim comboName As String

    '        '    strComboName = CType(sender, ComboBox).Name
    '        '    strComboName = strComboName.Substring(strComboName.Length - 1)

    '        '    comboName = "_cbo_Bit_TaxFlagVal_" & strComboName
    '        '    For Each item As Control In Me.Controls
    '        '        If item.Name = comboName Then
    '        '            CType(item, RadioButton).Checked = True
    '        '        End If

    '        '    Next
    '        'End If

    '    'If _cbo_Bit_TaxFlagVal_0.Focus = True Then
    '    '    _rad_Bit_TaxFlag_0.Checked = True
    '    '_grp_Bit_0
    '    'End If
    '    Dim strComboName As String
    '    Dim comboName As String

    '    strComboName = CType(sender, ComboBox).Name
    '    strComboName = strComboName.Substring(strComboName.Length - 1)

    '        comboName = "_rad_Bit_TaxFlag_" & strComboName

    '    ' Iterate through all the controls on the from, looking for groupboxes to look inside 
    '    For Each item As Control In Me.Controls
    '            'look in groupboxes  
    '        If TypeOf item Is GroupBox Then
    '            ' Look through the groupboxes controls 
    '            For Each childItem As Control In item.Controls
    '                ' Make sure it's a radio button 
    '                    If TypeOf childItem Is RadioButton Then
    '                        ' See if this is the one we are looking for 
    '                        If childItem.Name = comboName Then
    '                            CType(item, RadioButton).Checked = True
    '                        End If
    '                    End If
    '                Next
    '        End If
    '        Next
    '    End If
    'End Sub

#Region "button to view data element details"
    Private Sub Button_ViewDataElementDetails_Bit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ViewDataElementDetails_Bit.Click
        Logger.LogDebug("Button_ViewDataElementDetails_Bit_Click entry", Me.GetType())
        ' Show the form
        viewDataElementDetailsForm = New Form_ViewDataElementDetails()
        viewDataElementDetailsForm.POSDataTypeKey = _inputColumnData.POSDataTypeKey
        viewDataElementDetailsForm.BooleanElementsOnly = True
        viewDataElementDetailsForm.ShowDialog(Me)
        viewDataElementDetailsForm.Dispose()
        Logger.LogDebug("Button_ViewDataElementDetails_Bit_Click exit", Me.GetType())
    End Sub
#End Region

End Class
