Imports System.DateTime
Imports System.Reflection
Imports Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.Common

Public Class ItemAttributeUpdateForm


    Private _itemKey As Integer
    Private _identifier As String
    Private _description As String
    Private _itemAtribute As ItemAttribute = Nothing
    Private _attibuteIdentifiers As BusinessObjectCollection = Nothing



    Public Property ItemKey() As Integer
        Get
            Return _itemKey
        End Get
        Set(ByVal value As Integer)
            _itemKey = value
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Return _identifier
        End Get
        Set(ByVal value As String)
            _identifier = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

#Region "Event Handlers"
    Private Sub ItemAttributeUpdateForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        BindControls()

        ApplyAccess()

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        '-- Exit this screen
        Me.Close()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        SaveChanges()
        '-- Exit this screen
        Me.Close()
    End Sub

#End Region

#Region "Private Methods"
#Region "Main Action Methods"
    
#End Region
#Region "Support Methods"
    ''' <summary>
    ''' Disabling all input fields for users without SuperUser or Item Admin rights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ApplyAccess()

        If Not ((gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser) Then
            For Each chk As CheckBox In Me.frameChecks.Controls
                chk.Enabled = False
            Next
            For Each ctr As Control In Me.frameText.Controls
                If TypeOf (ctr) Is ComboBox Or TypeOf (ctr) Is TextBox Then
                    ctr.Enabled = False
                End If
            Next
            For Each ctr As Control In Me.frameDates.Controls
                If TypeOf (ctr) Is DateTimePicker Or TypeOf (ctr) Is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor Then
                    ctr.Enabled = False
                End If
            Next
            cmdUpdate.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Create data sources and bind to this form's controls.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindControls()
        Me.txtIdentifier.Text = _identifier
        Me.txtItemDesc.Text = _description
        'Get Item Key
        _itemKey = ItemAttributeDAO.Instance.GetItemKeyByIdentifier(_identifier)
        'Get Attributes and Identifiers
        _itemAtribute = ItemAttributeDAO.Instance.GetItemAttributeByItemKey(_itemKey)
        _attibuteIdentifiers = AttributeIdentifierDAO.Instance.GetAllAttributeIdentifiers()
        For Each theAttributeIdentifier As AttributeIdentifier In _attibuteIdentifiers
            Dim theFieldType As String = theAttributeIdentifier.FieldType
            If theFieldType.StartsWith("Check") Then
                BindCheckBoxes(_itemAtribute, theAttributeIdentifier)
            ElseIf theFieldType.StartsWith("Text") Then
                BindText(_itemAtribute, theAttributeIdentifier)
            ElseIf theFieldType.StartsWith("Date") Then
                BindDates(_itemAtribute, theAttributeIdentifier)
            End If

        Next

    End Sub

    Private Sub BindCheckBoxes(ByVal anAttribute As ItemAttribute, ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim isChecked As Boolean = False
        If Not anAttribute Is Nothing Then
            Dim aCheckBoxInfo, isNullInfo As PropertyInfo
            Dim aType As Type = anAttribute.GetType()
            aCheckBoxInfo = aType.GetProperty(name)
            isNullInfo = aType.GetProperty("Is" + name + "Null")
            Dim isNull As Boolean = CType(isNullInfo.GetValue(anAttribute, Nothing), Boolean)
            If isNull = False Then
                isChecked = CType(aCheckBoxInfo.GetValue(anAttribute, Nothing), Boolean)
            Else
                isChecked = False
            End If
        End If

        Dim aCheckBox As CheckBox = Nothing
        For Each instance As CheckBox In Me.frameChecks.Controls
            If instance.Name = name Then
                aCheckBox = instance
            End If
        Next
        aCheckBox.Checked = isChecked
        aCheckBox.Enabled = True
        aCheckBox.Text = anIdentifier.ScreenText
    End Sub

    Private Sub BindText(ByVal anAttribute As ItemAttribute, ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim aTextBoxInfo, isNullInfo As PropertyInfo
        Dim aValue As String
        If Not anAttribute Is Nothing Then
            Dim aType As Type = anAttribute.GetType()
            aTextBoxInfo = aType.GetProperty(name)
            isNullInfo = aType.GetProperty("Is" + name + "Null")
            Dim isNull As Boolean = CType(isNullInfo.GetValue(anAttribute, Nothing), Boolean)
            If isNull = False Then
                aValue = CType(aTextBoxInfo.GetValue(anAttribute, Nothing), String)
            Else
                aValue = ""
            End If
        Else
            aValue = ""
        End If

        Dim aTextBox As TextBox = Nothing
        Dim aLabel As Label = Nothing
        Dim aTextCombo As ComboBox = Nothing
        Dim isCombo As Boolean = anIdentifier.ComboBox
        Dim count As Integer = Me.frameText.Controls.Count
        Dim x As Integer
        Dim ctl As Control
        For x = 0 To count - 1
            ctl = Me.frameText.Controls.Item(x)
            If TypeOf (ctl) Is TextBox Then
                If ctl.Name = name Then
                    aTextBox = CType(ctl, TextBox)
                End If
            End If
            If TypeOf (ctl) Is Label Then
                If ctl.Name = name + "Label" Then
                    aLabel = CType(ctl, Label)
                End If
            End If
            If TypeOf (ctl) Is ComboBox Then
                If ctl.Name = name + "Combo" Then
                    aTextCombo = CType(ctl, ComboBox)
                End If
            End If
        Next
        If isCombo = False Then
            aTextBox.Enabled = True
            aTextBox.Text = aValue
            aTextBox.MaxLength = anIdentifier.MaxWidth
        Else
            aTextBox.Enabled = False
            aTextBox.Visible = False
            aTextCombo.Enabled = True
            aTextCombo.Visible = True
            PopulateCombo(aTextCombo, anIdentifier, aValue)
        End If
        aLabel.Text = anIdentifier.ScreenText
    End Sub
    Private Sub PopulateCombo(ByVal aComboBox As ComboBox, ByVal anIdentifier As AttributeIdentifier, _
        ByVal aValue As String)
        Dim ValueList As Array = anIdentifier.FieldValues.Split(CChar(","))
        aComboBox.DataSource = ValueList
        If Not aValue.Equals("") Then
            Dim index As Integer
            index = aComboBox.FindString(aValue)
            aComboBox.SelectedIndex = index
        ElseIf Not anIdentifier.IsDefaultValueNull And Not anIdentifier.DefaultValue.Equals("") Then
            Dim index As Integer
            index = aComboBox.FindString(anIdentifier.DefaultValue)
            aComboBox.SelectedIndex = index
        End If
    End Sub

    Private Sub BindDates(ByVal anAttribute As ItemAttribute, ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim aDateTimeInfo, isNullInfo As PropertyInfo
        Dim aValue As DateTime
        Dim isNull As Boolean = True
        If Not anAttribute Is Nothing Then
            Dim aType As Type = anAttribute.GetType()
            aDateTimeInfo = aType.GetProperty(name)
            isNullInfo = aType.GetProperty("Is" + name + "Null")
            isNull = CType(isNullInfo.GetValue(anAttribute, Nothing), Boolean)
            If isNull = False Then
                aValue = CType(aDateTimeInfo.GetValue(anAttribute, Nothing), DateTime)
            End If
        End If
        Dim aDateTimeEditor As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor = Nothing
        Dim aLabel As Label = Nothing
        Dim count As Integer = Me.frameDates.Controls.Count
        Dim x As Integer
        Dim ctl As Control
        For x = 0 To count - 1
            ctl = Me.frameDates.Controls.Item(x)
            If TypeOf (ctl) Is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor Then
                If ctl.Name = name Then
                    aDateTimeEditor = CType(ctl, Infragistics.Win.UltraWinEditors.UltraDateTimeEditor)
                End If
            End If
            If TypeOf (ctl) Is Label Then
                If ctl.Name = name + "Label" Then
                    aLabel = CType(ctl, Label)
                End If
            End If
        Next
        aDateTimeEditor.Enabled = True
        If isNull = False Then
            aDateTimeEditor.DateTime = aValue
        End If
        aLabel.Text = anIdentifier.ScreenText
    End Sub

    Private Sub SaveChanges()
        If _itemAtribute Is Nothing Then
            _itemAtribute = New ItemAttribute()
        End If
        For Each theAttributeIdentifier As AttributeIdentifier In _attibuteIdentifiers
            Dim theFieldType As String = theAttributeIdentifier.FieldType
            If theFieldType.StartsWith("Check") Then
                SaveCheckBox(theAttributeIdentifier)
            ElseIf theFieldType.StartsWith("Text") Then
                SaveText(theAttributeIdentifier)
            ElseIf theFieldType.StartsWith("Date") Then
                SaveDate(theAttributeIdentifier)
            End If
        Next
        _itemAtribute.ItemKey = _itemKey
        _itemAtribute.Save()
    End Sub

    Private Sub SaveCheckBox(ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim aCheckBoxInfo, isNullInfo As PropertyInfo
        Dim aType As Type = _itemAtribute.GetType()
        aCheckBoxInfo = aType.GetProperty(name)
        isNullInfo = aType.GetProperty("Is" + name + "Null")
        For Each instance As CheckBox In Me.frameChecks.Controls
            If instance.Name = name Then
                aCheckBoxInfo.SetValue(_itemAtribute, instance.Checked, Nothing)
                isNullInfo.SetValue(_itemAtribute, False, Nothing)
            End If
        Next
    End Sub

    Private Sub SaveText(ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim aTextBoxInfo, isNullInfo As PropertyInfo
        Dim aType As Type = _itemAtribute.GetType()
        aTextBoxInfo = aType.GetProperty(name)
        isNullInfo = aType.GetProperty("Is" + name + "Null")
        Dim aTextBox As TextBox = Nothing
        Dim aTextCombo As ComboBox = Nothing
        Dim isCombo As Boolean = anIdentifier.ComboBox
        Dim count As Integer = Me.frameText.Controls.Count
        Dim x As Integer
        Dim ctl As Control
        For x = 0 To count - 1
            ctl = Me.frameText.Controls.Item(x)
            If isCombo = False Then
                If TypeOf (ctl) Is TextBox Then
                    If ctl.Name = name Then
                        aTextBox = CType(ctl, TextBox)
                    End If
                End If
            Else
                If TypeOf (ctl) Is ComboBox Then
                    If ctl.Name = name + "Combo" Then
                        aTextCombo = CType(ctl, ComboBox)
                    End If
                End If
            End If

        Next
        If isCombo = False Then
            aTextBoxInfo.SetValue(_itemAtribute, aTextBox.Text, Nothing)
            isNullInfo.SetValue(_itemAtribute, False, Nothing)
        ElseIf isCombo = True And Not aTextCombo.SelectedItem Is Nothing Then
            aTextBoxInfo.SetValue(_itemAtribute, aTextCombo.SelectedItem.ToString, Nothing)
            isNullInfo.SetValue(_itemAtribute, False, Nothing)
        Else
            isNullInfo.SetValue(_itemAtribute, True, Nothing)
        End If
    End Sub

    Private Sub SaveDate(ByVal anIdentifier As AttributeIdentifier)
        Dim name As String = anIdentifier.FieldType
        Dim aDateInfo, isNullInfo As PropertyInfo
        Dim aType As Type = _itemAtribute.GetType()
        aDateInfo = aType.GetProperty(name)
        isNullInfo = aType.GetProperty("Is" + name + "Null")
        Dim aDateTimeEditor As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor = Nothing
        Dim count As Integer = Me.frameDates.Controls.Count
        Dim x As Integer
        Dim ctl As Control
        For x = 0 To count - 1
            ctl = Me.frameDates.Controls.Item(x)
            If TypeOf (ctl) Is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor Then
                If ctl.Name = name Then
                    aDateTimeEditor = CType(ctl, Infragistics.Win.UltraWinEditors.UltraDateTimeEditor)
                End If
            End If
        Next
        'only set value if one was provided
        If aDateTimeEditor.Value IsNot Nothing Then
            aDateInfo.SetValue(_itemAtribute, aDateTimeEditor.DateTime, Nothing)
            isNullInfo.SetValue(_itemAtribute, False, Nothing)
        Else
            'set the value to nothing
            aDateInfo.SetValue(_itemAtribute, Nothing, Nothing)
            'indicate that value is null
            isNullInfo.SetValue(_itemAtribute, True, Nothing)
        End If
    End Sub

#End Region


#End Region

End Class