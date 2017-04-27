Imports System.Text
Imports System.Configuration.ConfigurationSettings
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess

Public Class Form_ManageTaxFlagValue

    Dim _isEdit As Boolean
    Dim _existingTaxFlagValues As Hashtable ' used for validating new values
    Dim _currentKey As String = Nothing
    Private _taxFlagData As TaxFlagBO
    Private _isCurrentActiveFlag As Boolean

#Region "property definitions"

    Public Property IsEdit() As Boolean
        Get
            Return _isEdit
        End Get
        Set(ByVal value As Boolean)
            _isEdit = value
        End Set
    End Property

    Public Property ExistingTaxFlagValues() As Hashtable
        Get
            Return _existingTaxFlagValues
        End Get
        Set(ByVal value As Hashtable)
            _existingTaxFlagValues = value
        End Set
    End Property

    Public Property TaxFlagData() As TaxFlagBO
        Get
            Return _taxFlagData
        End Get
        Set(ByVal value As TaxFlagBO)
            _taxFlagData = value
        End Set
    End Property

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm(ByVal newKey As String)
#End Region

#Region "form events"

    Private Sub Form_ManageTaxFlagValue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set up button and title bar labels
        LoadText()
        'format data control
        FormatControls()
        'pre-fill the data entry form
        RadioButton_TaxFlagValueNo.Checked = False
        RadioButton_TaxFlagValueYes.Checked = False
        If _isEdit Then
            If _taxFlagData.TaxFlagValue Then
                RadioButton_TaxFlagValueNo.Checked = False
                RadioButton_TaxFlagValueYes.Checked = True

                'indicates that this flag is the current active flag;
                'used below to validate if more than one flag is being set to active (if regional setting is true)
                _isCurrentActiveFlag = True
            Else
                RadioButton_TaxFlagValueNo.Checked = True
                RadioButton_TaxFlagValueYes.Checked = False
            End If
            TextBox_TaxFlagKey.Text = _taxFlagData.TaxFlagKey
            If Not _taxFlagData.TaxPercent Is Nothing Then
                TextBox_TaxPercent.Text = _taxFlagData.TaxPercent
            End If
            TextBox_POSID.Text = _taxFlagData.POSId

            If Not _taxFlagData.ExternalTaxGroupCode Is Nothing Then
                txtExternalTaxGroupCode.Text = _taxFlagData.ExternalTaxGroupCode
            End If
        End If
    End Sub

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxClass_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If False Then
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                SaveChanges()
            End If
        End If

        ' Raise event - allows the data on the parent form to be refreshed
        RaiseEvent UpdateCallingForm(_currentKey)
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
    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        If SaveChanges() Then
            Me.Close()
        End If
    End Sub

#End Region

    ''' <summary>
    ''' set localized string text for buttons and title bar
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadText()
        If Me.IsEdit Then
            Me.Text = ResourcesTaxHosting.GetString("label_titleBar_taxFlagValueEdit")
        Else
            Me.Text = ResourcesTaxHosting.GetString("label_titleBar_taxFlagValueAdd")
        End If
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatControls()
        Dim taxPercent As Decimal

        If TextBox_TaxPercent.Text <> "" Then
            taxPercent = CType(TextBox_TaxPercent.Text, Decimal)
            TextBox_TaxPercent.Text = taxPercent.ToString("#0.##")
        End If

        'limit tax flag key to 1 char
        Me.TextBox_TaxFlagKey.MaxLength = 1

        'make tax flag key read only on edit and set focus to tax percent
        If _isEdit Then
            TextBox_TaxFlagKey.ReadOnly = True
            TextBox_TaxFlagKey.TabStop = False
            TextBox_TaxPercent.Focus()
        End If
    End Sub

    Private Function SaveChanges() As Boolean
        Dim success As Boolean = False
        Dim processData As Boolean = False
        Dim taxFlagDAO As New TaxFlagDAO
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As TaxFlagStatus
        Dim warnAboutPercentDecimals As Boolean = False
        Dim isExistingTaxFlag As Boolean = False

        'set values in taxFlagBO object to transport to DAO
        _taxFlagData.TaxFlagKey = Me.TextBox_TaxFlagKey.Text

        If Me.RadioButton_TaxFlagValueYes.Checked Then
            _taxFlagData.TaxFlagValue = True
        Else
            _taxFlagData.TaxFlagValue = False
        End If

        'validate current set of data
        statusList = _taxFlagData.ValidateTaxFlagData(Me.IsEdit, Me.ExistingTaxFlagValues)

        'validate tax percent value
        currentStatus = _taxFlagData.ValidateTaxPercent(Me.TextBox_TaxPercent.Text)
        If currentStatus <> TaxFlagStatus.Valid Then
            statusList.Add(currentStatus)

            'this is just a warning -- set percent value as entered by user
            If currentStatus = TaxFlagStatus.Warning_TaxPercentDecimalPrecision Then
                _taxFlagData.TaxPercent = CType(Me.TextBox_TaxPercent.Text, Decimal).ToString()
            End If
        ElseIf Not Me.TextBox_TaxPercent.Text.Trim.Equals("") Then
            _taxFlagData.TaxPercent = CType(Me.TextBox_TaxPercent.Text, Decimal).ToString()
        End If

        'validate pos id value
        currentStatus = _taxFlagData.ValidatePOSID(Me.TextBox_POSID.Text)
        If currentStatus <> TaxFlagStatus.Valid Then
            statusList.Add(currentStatus)
        ElseIf Me.TextBox_POSID.Text.Trim.Equals("") Then
            _taxFlagData.POSId = Nothing
        Else
            _taxFlagData.POSId = Me.TextBox_POSID.Text
        End If

        ' Get tax group code.
        If txtExternalTaxGroupCode.Text.Trim.Equals("") Then
            _taxFlagData.ExternalTaxGroupCode = Nothing
        Else
            _taxFlagData.ExternalTaxGroupCode = Me.txtExternalTaxGroupCode.Text
        End If



        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, TaxFlagStatus)

            Select Case currentStatus
                Case TaxFlagStatus.Error_Duplicate_TaxFlagKey
                    message.Append(String.Format(ResourcesTaxHosting.GetString("msg_validation_duplicateTaxFlagKey"), Me.Label_TaxFlagKey.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_Required_TaxFlagKey
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_TaxFlagKey.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_Required_TaxPercent
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_TaxPercent.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_Required_POSID
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_POSID.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_NotNumeric_TaxPercent
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_TaxPercent.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_TaxPercentValue
                    message.Append(String.Format(ResourcesTaxHosting.GetString("msg_error_taxPercentValueRange"), Me.Label_TaxPercent.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_POSIDValue
                    message.Append(String.Format(ResourcesTaxHosting.GetString("msg_error_posIDValueRange"), Me.Label_POSID.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Error_NotNumeric_POSID
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), Me.Label_POSID.Text))
                    message.Append(Environment.NewLine)
                Case TaxFlagStatus.Warning_TaxPercentDecimalPrecision
                    warnAboutPercentDecimals = True
            End Select
        End While

        If message.Length <= 0 Then
            'If required by region, check that only 1 TaxFlag is active; if existing active flags and current flag is also set
            'to active then prompt user that all other tax flags will be set to inactive if they wish to proceed.
            If InstanceDataDAO.IsFlagActive("OnlyOneActiveTaxFlag") AndAlso Not _isCurrentActiveFlag AndAlso _taxFlagData.ValidateTaxFlagActive() = TaxFlagStatus.Error_MultipleActiveFlags Then
                'display confirm message
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_tooManyActiveFlags"), Me.Label_TaxFlagKey.Text), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    _taxFlagData.ResetActiveFlags = True
                    processData = True
                End If
            Else
                processData = True
            End If

            'warn user they have entered more than 2 decimals and all extra digits will be rounded
            If processData AndAlso warnAboutPercentDecimals Then
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_taxPercentDecimalPrecision"), Me.Label_TaxPercent.Text), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    processData = True
                Else
                    processData = False
                End If
            End If

            If processData Then
                'look for existing tax flags for the same jurisdiction where the TaxFlagKey = new/edited tax flag key and the tax class 
                'is not the current selected tax class;  this is because tax definitions (% and POS ID) are shared across the same
                'tax jurisdiction
                isExistingTaxFlag = taxFlagDAO.IsExistingTaxFlagForJurisdiction(_taxFlagData)

                'data is valid - perform insert or update
                If Me.IsEdit Then
                    'look for existing tax flags for the same jurisdiction where the TaxFlagKey = edited tax flag key
                    'if any exist: inform user that their percentage and pos code value will update the values for ALL flags in the jurisdiction
                    If isExistingTaxFlag Then
                        '{0} = tax flag; {1} = tax class; {2} = tax percent; {3} = POS ID; {4} = jurisdiction
                        Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_existingTaxFlag_Edit"), Me.Label_TaxFlagKey.Text, ResourcesTaxHosting.GetString("label_header_taxClass"), Me.Label_TaxPercent.Text, Me.Label_POSID.Text, ResourcesTaxHosting.GetString("label_header_taxJurisdiction")), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If result = Windows.Forms.DialogResult.Yes Then
                            processData = True
                        Else
                            processData = False
                        End If
                    End If

                    If processData = True Then
                        taxFlagDAO.UpdateTaxFlag(_taxFlagData)
                        success = True
                    End If
                Else
                    'look for existing tax flags for the same jurisdiction where the TaxFlagKey = newly entered tax flag key
                    'if any exist: inform user that their percentage and pos code value will be overridden by the shared ones
                    If isExistingTaxFlag Then
                        Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_existingTaxFlag_Add"), Me.Label_TaxFlagKey.Text, ResourcesTaxHosting.GetString("label_header_taxClass"), Me.Label_TaxPercent.Text, Me.Label_POSID.Text, ResourcesTaxHosting.GetString("label_header_taxJurisdiction")), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If result = Windows.Forms.DialogResult.Yes Then
                            processData = True
                        Else
                            processData = False
                        End If
                    End If

                    If processData = True Then
                        taxFlagDAO.InsertTaxFlag(_taxFlagData)

                        'set currentKey value to track the key that was just added;  this will be used to remove the item from
                        'the deleted list if the user has previously deleted this item before saving changes
                        _currentKey = _taxFlagData.TaxFlagKey

                        success = True
                    End If
                End If
            End If
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

End Class