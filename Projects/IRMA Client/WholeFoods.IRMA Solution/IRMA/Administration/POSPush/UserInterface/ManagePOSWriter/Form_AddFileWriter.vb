Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.UserInterface
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_AddFileWriter

#Region "Class Level Vars and Property Definitions"

    ''' <summary>
    ''' Form to edit a single POSWriter.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents editBatchIdDefaults As Form_EditBatchIdDefaults
    ''' <summary>
    ''' Value of the current writer configuration for edits
    ''' </summary>
    ''' <remarks></remarks>
    Private _inputWriterConfig As POSWriterBO
    ''' <summary>
    ''' DataSet that holds escape char data
    ''' </summary>
    ''' <remarks></remarks>
    Dim _escapeCharDataSet As DataSet
    ''' <summary>
    ''' tracks when the file writer type drop down has data bound to it; used by event on ComboBox_FileWriterType.SelectedValueChanged
    ''' </summary>
    ''' <remarks></remarks>
    Private _isWriterTypeDataBound As Boolean = False

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm()
#End Region

    Private Sub Form_AddFileWriter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' initialize the POSWriterBO
        _inputWriterConfig = New POSWriterBO()

        PopulatePropertiesData()
    End Sub

    ''' <summary>
    ''' Pre-fill the properties tab with the data from the database.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulatePropertiesData()
        'bind data to data grids and drop down boxes
        BindPropertiesTabData()

        'disable the writer class drop down.  it will have data bound to it when the writer type is selected.  the list will be
        'narrowed based on the writer type value
        ComboBox_FileWriterClass.Enabled = False

        'disable the scale writer type drop down.  it will be enabled when the scale writer type is selected.
        ComboBox_ScaleWriterType.Enabled = False

        ' Load the writer properties tab
        Me.Properties_TextBox_DelimCharVal.Text = _inputWriterConfig.DelimChar
        Me.Properties_CheckBox_LeadingDelimiter.Checked = _inputWriterConfig.LeadingDelimiter
        Me.Properties_CheckBox_TrailingDelimiter.Checked = _inputWriterConfig.TrailingDelimiter
        Me.Properties_CheckBox_FieldIdDelimiter.Checked = _inputWriterConfig.FieldIdDelim
        Me.Properties_CheckBox_EnforceDictionary.Checked = _inputWriterConfig.EnforceDictionary
        Me.ComboBox_FileWriterClass.SelectedText = _inputWriterConfig.POSFileWriterClass
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
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindPropertiesTabData()
        ' Read the escape char data
        Dim writerDAO As New POSWriterDAO
        _escapeCharDataSet = writerDAO.GetWriterEscapeChars(_inputWriterConfig)

        Me.UltraGrid_EscapeChars.DataSource = _escapeCharDataSet.Tables(0)

        ' setup file writer type drop down
        Me.ComboBox_FileWriterType.DataSource = writerDAO.GetFileWriterTypes(True)
        Me.ComboBox_FileWriterType.SelectedIndex = -1

        ' setup scale writer type drop down
        Me.ComboBox_ScaleWriterType.DataSource = writerDAO.GetScaleWriterTypes().Tables(0)
        Me.ComboBox_ScaleWriterType.DisplayMember = "ScaleWriterTypeDesc"
        Me.ComboBox_ScaleWriterType.ValueMember = "ScaleWriterTypeKey"
        Me.ComboBox_ScaleWriterType.SelectedIndex = -1

        _isWriterTypeDataBound = True
    End Sub

    Private Sub BindWriterClassDropDown()
        Dim writerDAO As New POSWriterDAO

        ' setup writer file class data
        Me.ComboBox_FileWriterClass.DataSource = writerDAO.GetPOSFileWriterClasses(Me.ComboBox_FileWriterType.SelectedValue.ToString)
        Me.ComboBox_FileWriterClass.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' The user selected the Ok button on the Properties tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Properties_Button_OK.Click
        If ApplyChanges() Then
            ' Close the child window
            Me.Close()
        End If
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
        ' Close the child window
        Me.Close()
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim posWriter As POSWriterBO = InitializePOSWriterBO()
        Dim newPOSFileWriterKey As Integer = -1

        'validate data
        Dim statusList As ArrayList = posWriter.ValidatePOSWriterData
        Dim statusEnum As IEnumerator = statusList.GetEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As POSWriterBOStatus

        'loop through possible validation erorrs and build message string containing all errors
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, POSWriterBOStatus)

            Select Case currentStatus
                Case POSWriterBOStatus.Error_Required_FileWriterType
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FileWriterType.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_FileWriterCode
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_FileWriterCode.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_WriterClass
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Properties_Label_WriterClass.Text))
                    message.Append(Environment.NewLine)
                Case POSWriterBOStatus.Error_Required_ScaleWriterType
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_ScaleType.Text))
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
                    message.Append(ResourcesCommon.GetString("msg_validation_invalid_batchIdRange"))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            ' data is valid; save the change to the database
            newPOSFileWriterKey = POSWriterDAO.AddPOSWriterRecord(posWriter)

            ' save the escape char changes to the database
            If _escapeCharDataSet.HasChanges() Then
                Try
                    'assign new key to any escape char data that was just added to new POSWriter
                    Dim rowEnum As IEnumerator = _escapeCharDataSet.Tables(0).Rows.GetEnumerator
                    Dim currentRow As DataRow

                    While rowEnum.MoveNext
                        currentRow = CType(rowEnum.Current, DataRow)
                        If newPOSFileWriterKey <> -1 Then
                            currentRow.Item("POSFileWriterKey") = newPOSFileWriterKey
                        End If
                    End While

                    Dim dao As New POSWriterDAO
                    dao.SaveEscapeCharData(_escapeCharDataSet, posWriter)
                Catch ex As DBConcurrencyException
                    BindPropertiesTabData()
                    MessageBox.Show(ResourcesCommon.GetString("msg_concurrencyError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex2 As Exception
                    'TODO log/handle exception
                    MessageBox.Show(ResourcesCommon.GetString("msg_dbError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
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
    ''' Open a separate form to allow the user to manage the default batch id values
    ''' for the POS system.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Properties_Button_DefaultBatchIds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    ''' <summary>
    ''' Initialize a new POSWriterBO and populates it with form input values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InitializePOSWriterBO() As POSWriterBO
        Dim posWriter As New POSWriterBO

        If Me.ComboBox_FileWriterType.SelectedValue IsNot Nothing Then
            posWriter.WriterType = Me.ComboBox_FileWriterType.SelectedValue.ToString
        End If
        If Me.ComboBox_ScaleWriterType.SelectedValue IsNot Nothing Then
            posWriter.ScaleWriterType = Me.ComboBox_ScaleWriterType.SelectedValue.ToString
            posWriter.ScaleWriterTypeDesc = Me.ComboBox_ScaleWriterType.SelectedText.ToString
        End If
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

    Private Sub ComboBox_FileWriterType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_FileWriterType.SelectedValueChanged
        If _isWriterTypeDataBound Then
            'enable writer class drop down
            Me.ComboBox_FileWriterClass.Enabled = True
            'bind data
            BindWriterClassDropDown()

            ' Enable the scale type based on the selection
            If Me.ComboBox_FileWriterType.SelectedValue.ToString = POSWriterBO.WRITER_TYPE_SCALE Then
                Me.ComboBox_ScaleWriterType.Enabled = True
            Else
                Me.ComboBox_ScaleWriterType.Enabled = False
                Me.ComboBox_ScaleWriterType.SelectedIndex = -1
            End If

        End If

    End Sub

End Class