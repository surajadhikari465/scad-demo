Imports System.Text
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess

Public Class Form_ManageTaxOverrideValue

    Dim _itemKey As Integer
    Dim _storeNo As Integer
    Dim _isEdit As Boolean
    Dim _existingTaxFlagValues As Hashtable
    Dim _currentKey As String = Nothing
    Dim _editKey As String

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

    Public Property EditTaxFlagKey() As String
        Get
            Return _editKey
        End Get
        Set(ByVal value As String)
            _editKey = value
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

    Private Sub Form_ManageTaxOverride_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set up button and title bar labels
        LoadText()
        'bind data to flag drop down
        BindTaxFlagDropDown()

        'verify that there are available flags to override
        If Me.ComboBox_TaxFlag.Items.Count <= 0 Then
            MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_warning_mustSetupTaxFlags"), ResourcesTaxHosting.GetString("label_header_taxFlagKey"), ResourcesTaxHosting.GetString("label_header_taxClass"), ResourcesTaxHosting.GetString("label_header_taxJurisdiction")), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Me.Close()
        Else
            'format data control
            FormatControls()
        End If
    End Sub

    ''' <summary>
    ''' Confirm user wants to save any changed data when they are clicking 'X' button in top-right of window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageTaxOverride_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
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
            Me.Text = ResourcesTaxHosting.GetString("label_titleBar_taxOverrideEdit")
        Else
            Me.Text = ResourcesTaxHosting.GetString("label_titleBar_taxOverrideAdd")
        End If
    End Sub

    Private Sub BindTaxFlagDropDown()
        Dim taxFlagDAO As New TaxFlagDAO
        Dim taxFlagList As ArrayList = taxFlagDAO.GetAvailableTaxFlagListForItem(Me.ItemKey, Me.StoreNo)

        Me.ComboBox_TaxFlag.DataSource = taxFlagList

        If taxFlagList.Count > 0 Then
            Me.ComboBox_TaxFlag.ValueMember = "TaxFlagKey"
            Me.ComboBox_TaxFlag.DisplayMember = "TaxFlagKey"

            If Me.IsEdit Then
                'select the proper item that is being edited
                Dim flagEnum As IEnumerator = Me.ComboBox_TaxFlag.Items.GetEnumerator
                Dim currentFlag As TaxFlagBO
                Dim flagIndex As Integer

                While flagEnum.MoveNext
                    currentFlag = CType(flagEnum.Current, TaxFlagBO)

                    If currentFlag.TaxFlagKey = Me.EditTaxFlagKey Then
                        'select this item
                        Me.ComboBox_TaxFlag.SelectedIndex = flagIndex
                        Exit While
                    End If

                    flagIndex += 1
                End While
            End If
        End If
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatControls()
        If Not Me.IsEdit Then
            'default KEY value to nothing when adding new value
            Me.ComboBox_TaxFlag.SelectedValue = -1
            Me.ComboBox_TaxFlag.TabStop = True
            Me.ComboBox_TaxFlag.Focus()
        Else
            Me.ComboBox_TaxFlag.TabStop = False
            If Me.RadioButton_TaxFlagValueNo.Checked Then
                Me.RadioButton_TaxFlagValueNo.Focus()
                Me.RadioButton_TaxFlagValueNo.TabStop = True
                Me.RadioButton_TaxFlagValueYes.TabStop = False
            Else
                Me.RadioButton_TaxFlagValueYes.Focus()
                Me.RadioButton_TaxFlagValueNo.TabStop = False
                Me.RadioButton_TaxFlagValueYes.TabStop = True
            End If
        End If
    End Sub

    Private Function SaveChanges() As Boolean
        Dim success As Boolean = False
        Dim taxOverrideDAO As New TaxOverrideDAO
        Dim taxOverrideBO As New TaxOverrideBO

        'set values in taxFlagBO object to transport to DAO
        taxOverrideBO.StoreNo = Me.StoreNo
        taxOverrideBO.ItemKey = Me.ItemKey
        taxOverrideBO.TaxFlagKey = CType(Me.ComboBox_TaxFlag.SelectedValue, String)

        If Me.RadioButton_TaxFlagValueYes.Checked Then
            taxOverrideBO.TaxFlagValue = True
        Else
            taxOverrideBO.TaxFlagValue = False
        End If

        Select Case taxOverrideBO.ValidateTaxFlagData(Me.IsEdit, Me.ExistingTaxFlagValues)
            Case TaxOverrideStatus.Error_Duplicate_TaxFlagKey
                MessageBox.Show(String.Format(ResourcesTaxHosting.GetString("msg_validation_duplicateTaxFlagKey"), Me.Label_TaxFlagKey.Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Case TaxOverrideStatus.Error_Required_TaxFlagKey
                MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_validation_required"), Me.Label_TaxFlagKey.Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Case TaxOverrideStatus.Valid
                'data is valid - perform insert or update
                If Me.IsEdit Then
                    taxOverrideDAO.UpdateTaxFlag(taxOverrideBO)
                Else
                    taxOverrideDAO.InsertTaxFlag(taxOverrideBO)

                    'set currentKey value to track the key that was just added;  this will be used to remove the item from
                    'the deleted list if the user has previously deleted this item before saving changes
                    _currentKey = taxOverrideBO.TaxFlagKey
                End If

                success = True
        End Select

        Return success
    End Function

End Class